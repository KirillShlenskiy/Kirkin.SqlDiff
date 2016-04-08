using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using DiffPlex.Model;

using FastColoredTextBoxNS;

using Kirkin.Data;
using Kirkin.Diff.DiffResults;

namespace Kirkin.SqlDiff
{
    public partial class DetailedDiffView : Form
    {
        private readonly DataTableLite LeftTable;
        private readonly DataTableLite RightTable;

        public DetailedDiffView(DataTableLite leftTable, DataTableLite rightTable)
        {
            LeftTable = leftTable;
            RightTable = rightTable;

            InitializeComponent();
        }

        // Designer support.
        public DetailedDiffView()
        {
            InitializeComponent();
        }

        private void DetailedDiffView_Load(object sender, EventArgs _)
        {
            TextBox1.Scroll += (s, e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    if (TextBox2.VerticalScroll.Value != e.NewValue)
                    {
                        TextBox2.VerticalScroll.Value = e.NewValue;
                        ScheduleUpdate(TextBox2);
                    }
                }
            };

            TextBox2.Scroll += (s, e) =>
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    if (TextBox1.VerticalScroll.Value != e.NewValue)
                    {
                        TextBox1.VerticalScroll.Value = e.NewValue;
                        ScheduleUpdate(TextBox1);
                    }
                }
            };

            RenderDiff();
        }

        private async void ScheduleUpdate(Control control)
        {
            await Task.Yield();

            control.Refresh();
        }

        private void RenderDiff()
        {
            string leftContents = DataDescription(LeftTable);
            string rightContents = DataDescription(RightTable);

            Differ engine = new Differ();
            SideBySideDiffBuilder diffBuilder = new SideBySideDiffBuilder(engine);
            SideBySideDiffModel result = diffBuilder.BuildDiffModel(leftContents, rightContents);

            RenderDiff(TextBox1, result.OldText.Lines);
            RenderDiff(TextBox2, result.NewText.Lines);
        }

        private static void RenderDiff(FastColoredTextBox textBox, List<DiffPiece> lines)
        {
            Style normalStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Regular);
            Style lineAddedStyle = new TextStyle(Brushes.Black, Brushes.Green, FontStyle.Regular);
            Style lineChangedStyle = new TextStyle(Brushes.Black, Brushes.LightBlue, FontStyle.Regular);
            Style lineRemovedStyle = new TextStyle(Brushes.Black, Brushes.Red, FontStyle.Regular);

            textBox.BeginUpdate();

            try
            {
                textBox.Clear();

                foreach (DiffPiece line in lines)
                {
                    Style style;

                    switch (line.Type)
                    {
                        case ChangeType.Inserted:
                            style = lineAddedStyle;
                            break;
                        case ChangeType.Deleted:
                            style = lineRemovedStyle;
                            break;
                        case ChangeType.Modified:
                            style = lineChangedStyle;
                            break;
                        default:
                            style = normalStyle;
                            break;
                    }

                    textBox.AppendText(line.Text, style);
                    textBox.AppendText(Environment.NewLine);
                }
            }
            finally
            {
                textBox.EndUpdate();
            }
        }

        private static string DataDescription(DataTableLite table)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder line = new StringBuilder();

            foreach (DataRowLite row in table.Rows)
            {
                line.Clear();

                foreach (object value in row.ItemArray)
                {
                    if (line.Length != 0) line.Append(", ");

                    line.Append(DiffTextUtil.ToString(value));
                }

                sb.AppendLine(line.ToString());
            }

            return sb.ToString();
        }
    }
}