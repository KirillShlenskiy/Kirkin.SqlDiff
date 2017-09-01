using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Kirkin.Data;
using Kirkin.Diff;
using Kirkin.Diff.Data;

using Kirkin.SqlDiff.Properties;

namespace Kirkin.SqlDiff
{
    public partial class MainForm : Form
    {
        private readonly string DefaultText;

        public MainForm()
        {
            InitializeComponent();

            Text += $" ({Assembly.GetExecutingAssembly().GetName().Version})";
            DefaultText = Text;
        }

        private async void MainForm_Load(object sender, EventArgs _)
        {
            ConnectionStringTextBox1.Text = Settings.Default.ConnectionString1;
            ConnectionStringTextBox2.Text = Settings.Default.ConnectionString2;

            CommandTextTextBox1.AcceptsTab = false;
            CommandTextTextBox2.AcceptsTab = false;

            KeyPreview = true;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F5) {
                    ExecuteDiff();
                }
            };

            await Task.Yield();

            CommandTextTextBox1.Focus();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ConnectionString1 = ConnectionStringTextBox1.Text;
            Settings.Default.ConnectionString2 = ConnectionStringTextBox2.Text;

            Settings.Default.Save();
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (ExecuteButton.Tag != null)
            {
                ((CancellationTokenSource)ExecuteButton.Tag).Cancel();

                ExecuteButton.Enabled = false;
            }
            else
            {
                ExecuteDiff();
            }
        }

        private async void ExecuteDiff()
        {
            if (string.IsNullOrEmpty(ConnectionStringTextBox1.Text) || string.IsNullOrEmpty(ConnectionStringTextBox2.Text))
            {
                MessageBox.Show("Please enter both connection strings.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(CommandTextTextBox1.Text) || string.IsNullOrEmpty(CommandTextTextBox2.Text))
            {
                MessageBox.Show("Please enter both SQL commands.", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CancellationTokenSource cancellation = new CancellationTokenSource();

            ExecuteButton.Tag = cancellation;
            ExecuteButton.Text = "Cancel";

            try
            {
                Text = DefaultText + ": executing left ...";
                Stopwatch ds1Stopwatch = Stopwatch.StartNew();
                DataSetLite ds1 = await ProduceDataSetAsync(ConnectionStringTextBox1.Text, CommandTextTextBox1.Text, cancellation.Token);

                ds1Stopwatch.Stop();
                cancellation.Token.ThrowIfCancellationRequested();

                Text = DefaultText + ": executing right ...";
                Stopwatch ds2Stopwatch = Stopwatch.StartNew();
                DataSetLite ds2 = await ProduceDataSetAsync(ConnectionStringTextBox2.Text, CommandTextTextBox2.Text, cancellation.Token);

                ds2Stopwatch.Stop();
                cancellation.Token.ThrowIfCancellationRequested();

                Text = DefaultText + ": comparing ...";
                Stopwatch diffStopwatch = Stopwatch.StartNew();
                DiffResult diff = await Task.Run(() => DataSetLiteDiff.Compare(ds1, ds2));

                diffStopwatch.Stop();
                cancellation.Token.ThrowIfCancellationRequested();

                StringBuilder resultText = new StringBuilder();

                resultText.Append($"Time taken (left): {(double)ds1Stopwatch.ElapsedMilliseconds / 1000:0.###}s, ");
                resultText.Append($"right: {(double)ds2Stopwatch.ElapsedMilliseconds / 1000:0.###}s, ");
                resultText.AppendLine($"compare: {(double)diffStopwatch.ElapsedMilliseconds / 1000:0.###}s");
                resultText.AppendLine();

                if (diff.AreSame)
                {
                    resultText.AppendLine($"Result sets identical. Tables: {ds1.Tables.Count}, Rows: {ds1.Tables.Sum(dt => dt.Rows.Count)}.");
                }
                else
                {
                    resultText.AppendLine(diff.ToString(DiffTextFormat.Tree));
                }

                if (resultText.Length > 1500)
                {
                    resultText.Length = 1500;
                    resultText.Append(" ...");
                }

                ExecuteButton.Enabled = true;
                Text = DefaultText + ": done";

                if (diff.AreSame)
                {
                    MessageBox.Show(resultText.ToString(), "No diff", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult dr = MessageBox.Show(resultText.ToString(), "Changes detected - view detailed diff?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                    if (dr == DialogResult.Yes) {
                        new DetailedDiffView(ds1, ds2).Show();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected.
            }
            finally
            {
                ExecuteButton.Tag = null;
                ExecuteButton.Text = "Execute";
                ExecuteButton.Enabled = true;
                Text = DefaultText;
            }
        }

        private static async Task<DataSetLite> ProduceDataSetAsync(string connectionString, string commandText, CancellationToken ct)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(ct).ConfigureAwait(false);

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandTimeout = 0;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(ct).ConfigureAwait(false))
                    {
                        DataSetLite ds = new DataSetLite();

                        while (true)
                        {
                            ds.Tables.Add(await TableFromReaderAsync(reader, ct).ConfigureAwait(false));

                            if (!await reader.NextResultAsync(ct).ConfigureAwait(false)) {
                                break;
                            }
                        }

                        return ds;
                    }
                }
            }
        }

        private static async Task<DataTableLite> TableFromReaderAsync(SqlDataReader reader, CancellationToken ct)
        {
            DataTableLite table = new DataTableLite();

            for (int i = 0; i < reader.FieldCount; i++) {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            object[] itemArray = new object[reader.FieldCount];

            while (await reader.ReadAsync(ct).ConfigureAwait(false))
            {
                for (int i = 0; i < itemArray.Length; i++) {
                    itemArray[i] = reader[i];
                }

                table.Rows.Add(itemArray);
            }

            table.Rows.TrimExcess(); // Manage GC pressure.

            return table;
        }
    }
}