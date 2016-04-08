﻿using System.Collections;
using System.Collections.Generic;

using Kirkin.Data;
using Kirkin.Diff.DiffResults;

namespace Kirkin.Diff.Data
{
    internal sealed class LightDataRowDiff : DiffEngine<LightDataRow>
    {
        /// <summary>
        /// Default diff engine instance.
        /// </summary>
        internal static LightDataRowDiff Default { get; } = new LightDataRowDiff();

        private LightDataRowDiff()
        {
        }

        protected internal override DiffResult Compare(string name, LightDataRow x, LightDataRow y, IEqualityComparer comparer)
        {
            return new DiffResult(name, GetCellDiffs(x, y, comparer));
        }

        private static DiffResult[] GetCellDiffs(LightDataRow x, LightDataRow y, IEqualityComparer comparer)
        {
            List<DiffResult> entries = null;

            // Perf: avoid new array allocation on every ItemArray getter call.
            object[] xItemArray = x.ItemArray;
            object[] yItemArray = y.ItemArray;

            for (int i = 0; i < xItemArray.Length; i++)
            {
                if (!comparer.Equals(xItemArray[i], yItemArray[i]))
                {
                    if (entries == null) entries = new List<DiffResult>();

                    entries.Add(new SimpleDiffResult(x.Table.Columns[i].ColumnName, xItemArray[i], yItemArray[i], comparer));
                }
            }

            return entries == null
                ? DiffResult.EmptyDiffResultArray
                : entries.ToArray();
        }
    }
}