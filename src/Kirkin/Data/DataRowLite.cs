namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataRow-like data structure.
    /// </summary>
    public sealed class DataRowLite
    {
        /// <summary>
        /// Table that this row belongs to.
        /// </summary>
        public DataTableLite Table { get; }

        /// <summary>
        /// Row cell values.
        /// </summary>
        public object[] ItemArray { get; }

        internal DataRowLite(DataTableLite table, object[] itemArray)
        {
            Table = table;
            ItemArray = itemArray;
        }
    }
}