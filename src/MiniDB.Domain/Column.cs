namespace MiniDB.Domain
{
    public class Column
    {
        public string Name { get; }
        public DataType Type { get; }

        public Column(string name, DataType type)
        {
            Name = name;
            Type = type;
        }
    }
}