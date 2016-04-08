using Kirkin.Data;
using Kirkin.Diff;
using Kirkin.Diff.Data;

using Xunit;
using Xunit.Abstractions;

namespace Kirkin.Tests.Diff
{
    public class DataDiffTests
    {
        private readonly ITestOutputHelper Output;

        public DataDiffTests(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void EmptyTableDiff()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            Assert.True(DataTableLiteDiff.Compare(dt1, dt2).AreSame);
        }

        [Fact]
        public void SimpleTableDiff()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            dt1.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("ID", typeof(int));

            Assert.True(DataTableLiteDiff.Compare(dt1, dt2).AreSame);
        }

        [Fact]
        public void ColumnCountMismatchDiff()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            dt1.Columns.Add("ID", typeof(int));

            Assert.False(DataTableLiteDiff.Compare(dt1, dt2).AreSame);
        }

        [Fact]
        public void ColumnNameMismatchDiff()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            dt1.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("IDz", typeof(int));

            DiffResult diff = DataTableLiteDiff.Compare(dt1, dt2);

            Assert.False(diff.AreSame);

            Output.WriteLine(diff.ToString(DiffTextFormat.List));
        }

        [Fact]
        public void ColumnDataTypeMismatchDiff()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            dt1.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("ID", typeof(string));

            DiffResult diff = DataTableLiteDiff.Compare(dt1, dt2);

            Assert.False(diff.AreSame);

            Output.WriteLine(diff.ToString(DiffTextFormat.List));
        }

        [Fact]
        public void DataCompare()
        {
            DataTableLite dt1 = new DataTableLite();
            DataTableLite dt2 = new DataTableLite();

            dt1.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("ID", typeof(int));
            dt1.Columns.Add("Value", typeof(string));
            dt2.Columns.Add("Value", typeof(string));

            dt1.Rows.Add(1, "Hello");
            dt2.Rows.Add(1, "Hello");
            dt1.Rows.Add(2, "Moshi Moshi");
            dt2.Rows.Add(2, "Moshi Moshi");

            Assert.True(DataTableLiteDiff.Compare(dt1, dt2).AreSame);

            dt1.Rows.Add(3, "Aloha");

            DiffResult diff1 = DataTableLiteDiff.Compare(dt1, dt2);

            Assert.False(diff1.AreSame);
            Assert.Equal("DataTable -> Row count: 3 | 2", diff1.ToString());

            dt2.Rows.Add(4, "Whaaaa");

            DiffResult diff2 = DataTableLiteDiff.Compare(dt1, dt2);
            string message = diff2.ToString();

            Assert.False(diff2.AreSame);
            //Assert.Equal("Row count mismatch: 3 vs 2.", diff.Message);

            Output.WriteLine(diff2.ToString(DiffTextFormat.Tree));
        }
    }
}