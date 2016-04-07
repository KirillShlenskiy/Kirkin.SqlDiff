using System.Collections.Generic;

using Kirkin.Data;

namespace Kirkin.Diff.Data
{
    internal static class DataRowDiff
    {
        internal static DiffResult Compare(string name, LightDataRow x, LightDataRow y)
        {
            return new DiffResult(name, GetCellDiffs(x, y));
        }

        private static DiffResult[] GetCellDiffs(LightDataRow x, LightDataRow y)
        {
            List<DiffResult> entries = null;

            // Perf: avoid new array allocation on every ItemArray getter call.
            object[] xItemArray = x.ItemArray;
            object[] yItemArray = y.ItemArray;

            for (int i = 0; i < xItemArray.Length; i++)
            {
                if (!PrimitiveEqualityComparer.Instance.Equals(xItemArray[i], yItemArray[i]))
                {
                    if (entries == null) entries = new List<DiffResult>();

                    entries.Add(DiffResult.Create(x.Table.Columns[i].ColumnName, xItemArray[i], yItemArray[i]));
                }
            }

            return entries == null
                ? DiffResult.EmptyDiffResultArray
                : entries.ToArray();
        }
    }
}