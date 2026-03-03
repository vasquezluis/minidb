using MiniDB.Domain;
using MiniDB.Domain.Abstractions;

namespace MiniDB.Application.Services
{
    public class DatabaseService
    {
        private readonly IStorageEngine _storage;
        private Database _database;

        public DatabaseService(IStorageEngine storage, Database database)
        {
            _storage = storage;
            _database = database;
        }

        public Database Database => _database;

        public void SaveTable(Table table)
        {
            _storage.SaveTable(table);
        }

        public void InsertRow(Table table, Row row)
        {
            _storage.InsertRow(table, row);
        }

        public IEnumerable<Row> ReadRows(Table table)
        {
            return _storage.ReadRows(table);
        }

        public void RewriteTable(Table table, IEnumerable<Row> rows)
        {
            _storage.RewriteTable(table, rows);
        }
    }
}