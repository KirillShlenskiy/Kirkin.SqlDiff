using System;

namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataColumn-like data structure.
    /// </summary>
    public sealed class DataColumnLite
    {
        /// <summary>
        /// Name of the column. Cannot be null.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Data type of the column.
        /// </summary>
        public Type DataType { get; }

        /// <summary>
        /// Creates a new <see cref="DataColumnLite"/> instance.
        /// </summary>
        public DataColumnLite(string name, Type dataType)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (dataType == null) throw new ArgumentNullException(nameof(dataType));

            ColumnName = name;
            DataType = dataType;
        }
    }
}