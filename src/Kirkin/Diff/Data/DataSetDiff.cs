using System;
using System.Collections;
using System.Collections.Generic;

using Kirkin.Data;

namespace Kirkin.Diff.Data
{
    public sealed class DataSetDiff : DiffEngine<LightDataSet>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static DataSetDiff Default { get; } = new DataSetDiff();

        /// <summary>
        /// Compares two datasets using the given comparer and returns their diff.
        /// </summary>
        public static DiffResult Compare(LightDataSet x, LightDataSet y, IEqualityComparer comparer = null)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            return Default.Compare("DataSet", x, y, comparer ?? PrimitiveEqualityComparer.Instance);
        }

        private DataSetDiff()
        {
        }

        protected internal override DiffResult Compare(string name, LightDataSet x, LightDataSet y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = new List<DiffResult>();
            DiffResult tableCount = DiffResult.Create("Table count", x.Tables.Count, y.Tables.Count, comparer);

            entries.Add(tableCount);

            if (tableCount.AreSame) {
                entries.Add(new DiffResult("Tables", GetTableDiffs(x, y, comparer)));
            }

            return new DiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetTableDiffs(LightDataSet x, LightDataSet y, IEqualityComparer comparer)
        {
            DiffResult[] entries = new DiffResult[x.Tables.Count];

            for (int i = 0; i < x.Tables.Count; i++) {
                entries[i] = DataTableDiff.Default.Compare($"Table {i}", x.Tables[i], y.Tables[i], comparer);
            }

            return entries;
        }
    }
}