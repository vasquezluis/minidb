namespace MiniDB.Domain
{
    public class Column
    {
        public string Name { get; }
        public DataType Type { get; }
        public int MaxLength { get; }

        public Column(string name, DataType type, int maxLength = 0)
        {
            Name = name;
            Type = type;

            if (type == DataType.String && maxLength <= 0)
                throw new InvalidOperationException("String columns must define MaxLength.");

            MaxLength = maxLength;
        }
    }
}
