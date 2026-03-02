namespace MiniDB.Domain
{
    public class Database
    {
        private readonly Dictionary<string, Table> _tables = new();

        public IReadOnlyCollection<Table> Tables => _tables.Values;

        public void AddTable(Table table)
        {
            if (_tables.ContainsKey(table.Name))
                throw new InvalidOperationException("Table already exists.");

            _tables.Add(table.Name, table);
        }

        public Table GetTable(string name)
        {
            if (!_tables.TryGetValue(name, out var table))
                throw new InvalidOperationException("Table not found.");

            return table;
        }
    }
}