using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Kirkin.Data;

namespace Kirkin.Diff.Data
{
    public sealed class DataTableDiff : DiffEngine<LightDataTable>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static DataTableDiff Default { get; } = new DataTableDiff();

        /// <summary>
        /// Compares two data tables using the given comparer and returns their diff.
        /// </summary>
        public static DiffResult Compare(LightDataTable x, LightDataTable y, IEqualityComparer comparer = null)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            return Default.Compare("DataTable", x, y, comparer ?? PrimitiveEqualityComparer.Instance);
        }

        protected internal override DiffResult Compare(string name, LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = new List<DiffResult>();
            DiffResult columnCount = DiffResult.Create("Column count", x.Columns.Count, y.Columns.Count, comparer);

            entries.Add(columnCount);

            if (columnCount.AreSame)
            {
                entries.Add(new DiffResult("Column names", GetColumnNameDiffs(x, y, comparer)));
                entries.Add(new DiffResult("Column types", GetColumnDataTypeDiffs(x, y, comparer)));
            }

            DiffResult rowCount = DiffResult.Create("Row count", x.Rows.Count, y.Rows.Count, comparer);

            entries.Add(rowCount);

            if (columnCount.AreSame && rowCount.AreSame) {
                entries.Add(new DiffResult("Rows", GetRowDiffs(x, y, comparer)));
            }

            return new DiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetColumnNameDiffs(LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            DiffResult[] entries = new DiffResult[x.Columns.Count];

            for (int i = 0; i < x.Columns.Count; i++) {
                entries[i] = DiffResult.Create($"Column {i}", x.Columns[i].ColumnName, y.Columns[i].ColumnName, comparer);
            }

            return entries;
        }

        private static DiffResult[] GetColumnDataTypeDiffs(LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            DiffResult[] entries = new DiffResult[x.Columns.Count];

            for (int i = 0; i < x.Columns.Count; i++) {
                entries[i] = DiffResult.Create($"Column {i}", x.Columns[i].DataType, y.Columns[i].DataType, comparer);
            }

            return entries;
        }

        private static DiffResult[] GetRowDiffs(LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            DiffResult[] results = new DiffResult[x.Rows.Count];

            Parallel.For(0, x.Rows.Count, i => results[i] = DataRowDiff.Default.Compare($"Row {i}", x.Rows[i], y.Rows[i], comparer));

            return results;
        }
    }
}