using MiniDB.Domain;
using MiniDB.Domain.Abstractions;

namespace MiniDB.Application.Services
{

    public class DatabaseService
    {
        private readonly IStorageEngine _storage;
        private readonly string _dbPath;

        public Database Database { get; }

        public DatabaseService(IStorageEngine storage, string dbPath)
        {
            _storage = storage;
            _dbPath = dbPath;
            Database = _storage.LoadDatabase(dbPath);
        }

        public IStorageEngine Storage => _storage;
    }
}