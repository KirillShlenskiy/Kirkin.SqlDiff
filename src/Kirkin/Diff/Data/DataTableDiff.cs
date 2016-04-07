using System.Collections.Generic;
using System.Threading.Tasks;

using Kirkin.Data;

namespace Kirkin.Diff.Data
{
    public static class DataTableDiff
    {
        public static DiffResult Compare(LightDataTable x, LightDataTable y)
        {
            return Compare("DataTable", x, y);
        }

        internal static DiffResult Compare(string name, LightDataTable x, LightDataTable y)
        {
            List<DiffResult> entries = new List<DiffResult>();
            DiffResult columnCount = DiffResult.Create("Column count", x.Columns.Count, y.Columns.Count);

            entries.Add(columnCount);

            if (columnCount.AreSame)
            {
                entries.Add(new DiffResult("Column names", GetColumnNameDiffs(x, y)));
                entries.Add(new DiffResult("Column types", GetColumnDataTypeDiffs(x, y)));
            }

            DiffResult rowCount = DiffResult.Create("Row count", x.Rows.Count, y.Rows.Count);

            entries.Add(rowCount);

            if (columnCount.AreSame && rowCount.AreSame) {
                entries.Add(new DiffResult("Rows", GetRowDiffs(x, y)));
            }

            return new DiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetColumnNameDiffs(LightDataTable x, LightDataTable y)
        {
            DiffResult[] entries = new DiffResult[x.Columns.Count];

            for (int i = 0; i < x.Columns.Count; i++) {
                entries[i] = DiffResult.Create($"Column {i}", x.Columns[i].ColumnName, y.Columns[i].ColumnName);
            }

            return entries;
        }

        private static DiffResult[] GetColumnDataTypeDiffs(LightDataTable x, LightDataTable y)
        {
            DiffResult[] entries = new DiffResult[x.Columns.Count];

            for (int i = 0; i < x.Columns.Count; i++) {
                entries[i] = DiffResult.Create($"Column {i}", x.Columns[i].DataType, y.Columns[i].DataType);
            }

            return entries;
        }

        private static DiffResult[] GetRowDiffs(LightDataTable x, LightDataTable y)
        {
            DiffResult[] results = new DiffResult[x.Rows.Count];

            Parallel.For(0, x.Rows.Count, i => results[i] = DataRowDiff.Compare($"Row {i}", x.Rows[i], y.Rows[i]));

            return results;
        }
    }
}