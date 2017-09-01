using System.Collections;
using System.Collections.Generic;

using Kirkin.Data;
using Kirkin.Diff.DiffResults;

namespace Kirkin.Diff.Data
{
    internal sealed class DataRowLiteDiff : DiffEngine<DataRowLite>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static DataRowLiteDiff Default { get; } = new DataRowLiteDiff();

        private DataRowLiteDiff()
        {
        }

        protected internal override DiffResult Compare(string name, DataRowLite x, DataRowLite y, IEqualityComparer comparer)
        {
            return new CompositeDiffResult(name, GetCellDiffs(x, y, comparer));
        }

        private static DiffResult[] GetCellDiffs(DataRowLite x, DataRowLite y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = null;

            for (int i = 0; i < x.Table.Columns.Count; i++)
            {
                if (!comparer.Equals(x[i], y[i]))
                {
                    if (entries == null) entries = new List<DiffResult>();

                    entries.Add(new SimpleDiffResult(x.Table.Columns[i].ColumnName, x[i], y[i], comparer));
                }
            }

            return entries == null
                ? DiffResult.EmptyDiffResultArray
                : entries.ToArray();
        }
    }
}