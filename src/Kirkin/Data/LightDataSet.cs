using System.Collections.Generic;

namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataSet-like data structure.
    /// </summary>
    public sealed class LightDataSet
    {
        /// <summary>
        /// Collection of tables that belong to this dataset.
        /// </summary>
        public List<LightDataTable> Tables { get; } = new List<LightDataTable>();
    }
}