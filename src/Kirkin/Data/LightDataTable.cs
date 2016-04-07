using System;
using System.Collections.Generic;

namespace Kirkin.Data
{
    /// <summary>
    /// Lightweight DataColumn-like data structure.
    /// </summary>
    public sealed class LightDataTable
    {
        /// <summary>
        /// Collection of column definitions in this table.
        /// </summary>
        public LightDataColumnCollection Columns { get; }

        /// <summary>
        /// Collection of rows that belong to this table.
        /// </summary>
        public LightDataRowCollection Rows { get; }

        /// <summary>
        /// Creates a new <see cref="LightDataTable"/> instance.
        /// </summary>
        public LightDataTable()
        {
            Columns = new LightDataColumnCollection();
            Rows = new LightDataRowCollection(this);
        }

        public sealed class LightDataColumnCollection : List<LightDataColumn>
        {
            internal LightDataColumnCollection()
            {
            }

            /// <summary>
            /// Creates a new column with the given name and data type, and adds it to the table.
            /// </summary>
            public LightDataColumn Add(string name, Type dataType)
            {
                LightDataColumn column = new LightDataColumn(name, dataType);

                Add(column);

                return column;
            }
        }

        public sealed class LightDataRowCollection : List<LightDataRow>
        {
            private readonly LightDataTable Table;

            internal LightDataRowCollection(LightDataTable table)
            {
                Table = table;
            }

            /// <summary>
            /// Creates a new row from the given item array and adds it to the table.
            /// </summary>
            public LightDataRow Add(params object[] itemArray)
            {
                if (itemArray == null) throw new ArgumentNullException(nameof(itemArray));

                LightDataRow row = new LightDataRow(Table, itemArray);

                base.Add(row);

                return row;
            }
        }
    }
}