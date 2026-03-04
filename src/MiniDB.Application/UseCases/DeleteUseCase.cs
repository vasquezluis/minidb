using MiniDB.Application.Services;
using MiniDB.Domain;

namespace MiniDB.Application.UseCases
{
    public class DeleteUseCase
    {
        private readonly DatabaseService _dbService;

        public DeleteUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Execute(string tableName, Func<Row, bool> predicate)
        {
            var table = GetTable(tableName);

            var rows = _dbService.Storage.ReadRows(table).ToList();

            var filtered = rows.Where(r => !predicate(r)).ToList();

            _dbService.Storage.RewriteTable(table, filtered);
        }

        private Table GetTable(string name)
            => _dbService.Database.Tables
                .FirstOrDefault(t => t.Name == name)
                ?? throw new InvalidOperationException("Table not found.");
    }
}