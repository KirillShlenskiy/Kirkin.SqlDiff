namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataRow-like data structure.
    /// </summary>
    public sealed class LightDataRow
    {
        /// <summary>
        /// Table that this row belongs to.
        /// </summary>
        public LightDataTable Table { get; }

        /// <summary>
        /// Row cell values.
        /// </summary>
        public object[] ItemArray { get; }

        internal LightDataRow(LightDataTable table, object[] itemArray)
        {
            Table = table;
            ItemArray = itemArray;
        }
    }
}