using System.Collections.Generic;

using Kirkin.Data;

namespace Kirkin.Diff.Data
{
    public static class DataSetDiff
    {
        public static DiffResult Compare(LightDataSet x, LightDataSet y)
        {
            return Compare("DataSet", x, y);
        }

        internal static DiffResult Compare(string name, LightDataSet x, LightDataSet y)
        {
            List<DiffResult> entries = new List<DiffResult>();
            DiffResult tableCount = DiffResult.Create("Table count", x.Tables.Count, y.Tables.Count);

            entries.Add(tableCount);

            if (tableCount.AreSame) {
                entries.Add(new DiffResult("Tables", GetTableDiffs(x, y)));
            }

            return new DiffResult(name, entries.ToArray());
        }

        private static DiffResult[] GetTableDiffs(LightDataSet x, LightDataSet y)
        {
            DiffResult[] entries = new DiffResult[x.Tables.Count];

            for (int i = 0; i < x.Tables.Count; i++) {
                entries[i] = DataTableDiff.Compare($"Table {i}", x.Tables[i], y.Tables[i]);
            }

            return entries;
        }
    }
}