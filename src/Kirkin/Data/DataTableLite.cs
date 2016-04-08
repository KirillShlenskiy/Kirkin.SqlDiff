using System;
using System.Collections.Generic;

namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataColumn-like data structure.
    /// </summary>
    public sealed class DataTableLite
    {
        /// <summary>
        /// Collection of column definitions in this table.
        /// </summary>
        public DataColumnLiteCollection Columns { get; }

        /// <summary>
        /// Collection of rows that belong to this table.
        /// </summary>
        public DataRowLiteCollection Rows { get; }

        /// <summary>
        /// Creates a new <see cref="DataTableLite"/> instance.
        /// </summary>
        public DataTableLite()
        {
            Columns = new DataColumnLiteCollection();
            Rows = new DataRowLiteCollection(this);
        }

        public sealed class DataColumnLiteCollection : List<DataColumnLite>
        {
            internal DataColumnLiteCollection()
            {
            }

            /// <summary>
            /// Creates a new column with the given name and data type, and adds it to the table.
            /// </summary>
            public DataColumnLite Add(string name, Type dataType)
            {
                DataColumnLite column = new DataColumnLite(name, dataType);

                Add(column);

                return column;
            }
        }

        public sealed class DataRowLiteCollection : List<DataRowLite>
        {
            private readonly DataTableLite Table;

            internal DataRowLiteCollection(DataTableLite table)
            {
                Table = table;
            }

            /// <summary>
            /// Creates a new row from the given item array and adds it to the table.
            /// </summary>
            public DataRowLite Add(params object[] itemArray)
            {
                if (itemArray == null) throw new ArgumentNullException(nameof(itemArray));

                DataRowLite row = new DataRowLite(Table, itemArray);

                base.Add(row);

                return row;
            }
        }
    }
}