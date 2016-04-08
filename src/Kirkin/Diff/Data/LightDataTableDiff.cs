using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Kirkin.Data;
using Kirkin.Diff.DiffResults;

namespace Kirkin.Diff.Data
{
    public sealed class LightDataTableDiff : DiffEngine<LightDataTable>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static LightDataTableDiff Default { get; } = new LightDataTableDiff();

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
            DiffResult columns = new DiffResult("Columns", GetColumnDiffs(x, y, comparer));

            entries.Add(columns);

            DiffResult rowCount = new SimpleDiffResult("Row count", x.Rows.Count, y.Rows.Count, comparer);

            entries.Add(rowCount);

            if (columns.AreSame && rowCount.AreSame) {
                entries.Add(new DiffResult("Rows", GetRowDiffs(x, y, comparer)));
            }

            return new DiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetColumnDiffs(LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = new List<DiffResult>();

            for (int i = 0; i < Math.Max(x.Columns.Count, y.Columns.Count); i++)
            {
                LightDataColumn xCol = x.Columns.ElementAtOrDefault(i);
                LightDataColumn yCol = y.Columns.ElementAtOrDefault(i);

                if (xCol == null)
                {
                    entries.Add(new CustomDiffResult($"Column[{i}] ({yCol.ColumnName}: {yCol.DataType.Name})", false, "Only exists on the right."));
                }
                else if (yCol == null)
                {
                    entries.Add(new CustomDiffResult($"Column[{i}] ({xCol.ColumnName}: {xCol.DataType.Name})", false, "Only exists on the left."));
                }
                else
                {
                    entries.Add(new SimpleDiffResult($"Column[{i}] name", xCol.ColumnName, yCol.ColumnName, comparer));
                    entries.Add(new SimpleDiffResult($"Column[{i}] type", xCol.DataType, yCol.DataType, comparer));
                }
            }

            return entries.ToArray();
        }

        private static DiffResult[] GetRowDiffs(LightDataTable x, LightDataTable y, IEqualityComparer comparer)
        {
            DiffResult[] results = new DiffResult[x.Rows.Count];

            Parallel.For(0, x.Rows.Count, i => results[i] = LightDataRowDiff.Default.Compare($"Row {i}", x.Rows[i], y.Rows[i], comparer));

            return results;
        }
    }
}