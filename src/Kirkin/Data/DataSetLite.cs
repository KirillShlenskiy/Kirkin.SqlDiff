using System.Collections.Generic;

namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataSet-like data structure.
    /// </summary>
    public sealed class DataSetLite
    {
        /// <summary>
        /// Collection of tables that belong to this dataset.
        /// </summary>
        public List<DataTableLite> Tables { get; } = new List<DataTableLite>();
    }
}