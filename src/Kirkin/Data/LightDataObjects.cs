using System;
using System.Collections.Generic;

namespace Kirkin.Data
{
    public sealed class LightDataSet
    {
        public List<LightDataTable> Tables { get; } = new List<LightDataTable>();
    }

    public sealed class LightDataTable
    {
        public LightDataColumnCollection Columns { get; }
        public LightDataRowCollection Rows { get; }

        public LightDataTable()
        {
            Columns = new LightDataColumnCollection();
            Rows = new LightDataRowCollection(this);
        }
    }

    public sealed class LightDataColumnCollection : List<LightDataColumn>
    {
        public void Add(string name, Type dataType)
        {
            Add(new LightDataColumn(name, dataType));
        }
    }

    public sealed class LightDataColumn
    {
        public string ColumnName { get; }
        public Type DataType { get; }

        public LightDataColumn(string name, Type dataType)
        {
            ColumnName = name;
            DataType = dataType;
        }
    }

    public sealed class LightDataRowCollection : List<LightDataRow>
    {
        private readonly LightDataTable Table;

        public LightDataRowCollection(LightDataTable table)
        {
            Table = table;
        }

        public void Add(params object[] itemArray)
        {
            base.Add(new LightDataRow(Table, itemArray));
        }
    }

    public sealed class LightDataRow
    {
        public LightDataTable Table { get; }
        public object[] ItemArray { get; }

        public LightDataRow(LightDataTable table, object[] itemArray)
        {
            Table = table;
            ItemArray = itemArray;
        }
    }
}