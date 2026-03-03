namespace MiniDB.Domain
{
    public class Table
    {
        public string Name { get; }
        public IReadOnlyList<Column> Columns { get; set; }

        public Table(string name, IEnumerable<Column> columns)
        {
            Name = name;
            Columns = columns.ToList();
        }

        public Row CreateRow(IEnumerable<object?> values)
        {
            var valueList = values.ToList();

            if (valueList.Count != Columns.Count)
                throw new InvalidOperationException("Column count mismatch.");

            for (int i = 0; i < Columns.Count; i++)
            {
                ValidateType(valueList[i], Columns[i].Type);
            }

            return new Row(valueList);
        }

        private void ValidateType(object? value, DataType type)
        {
            switch (type)
            {
                case DataType.Int when value is not int:
                    throw new InvalidOperationException("Invalid int value.");
                case DataType.String when value is not string:
                    throw new InvalidOperationException("Invalid string value.");
            }
        }

        public int CalculateRowSize()
        {
            int size = 1; // IsDeleted flag

            foreach (var column in Columns)
            {
                switch (column.Type)
                {
                    case DataType.Int:
                        size += 4;
                        break;

                    case DataType.String:
                        size += column.MaxLength;
                        break;
                }
            }

            return size;
        }
    }
}