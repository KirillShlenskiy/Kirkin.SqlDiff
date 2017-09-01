using System;
using System.Collections;
using System.Collections.Generic;

using Kirkin.Data;
using Kirkin.Diff.DiffResults;

namespace Kirkin.Diff.Data
{
    public sealed class DataSetLiteDiff : DiffEngine<DataSetLite>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static DataSetLiteDiff Default { get; } = new DataSetLiteDiff();

        /// <summary>
        /// Compares two datasets using the given comparer and returns their diff.
        /// </summary>
        public static DiffResult Compare(DataSetLite x, DataSetLite y, IEqualityComparer comparer = null)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            return Default.Compare("DataSet", x, y, comparer ?? PrimitiveEqualityComparer.Instance);
        }

        private DataSetLiteDiff()
        {
        }

        protected internal override DiffResult Compare(string name, DataSetLite x, DataSetLite y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = new List<DiffResult>();
            DiffResult tableCount = new SimpleDiffResult("Table count", x.Tables.Count, y.Tables.Count, comparer);

            entries.Add(tableCount);

            if (tableCount.AreSame) {
                entries.Add(new CompositeDiffResult("Tables", GetTableDiffs(x, y, comparer)));
            }

            return new CompositeDiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetTableDiffs(DataSetLite x, DataSetLite y, IEqualityComparer comparer)
        {
            DiffResult[] entries = new DiffResult[x.Tables.Count];

            for (int i = 0; i < x.Tables.Count; i++) {
                entries[i] = DataTableLiteDiff.Default.Compare($"Table {i}", x.Tables[i], y.Tables[i], comparer);
            }

            return entries;
        }
    }
}