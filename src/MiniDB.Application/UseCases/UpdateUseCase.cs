using MiniDB.Application.Services;
using MiniDB.Domain;

namespace MiniDB.Application.UseCases
{
    public class UpdateUseCase
    {
        private readonly DatabaseService _dbService;

        public UpdateUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Execute(string tableName, Func<Row, bool> predicate, Func<Row, object?[]> updater)
        {
            var table = GetTable(tableName);

            var rows = _dbService.Storage.ReadRows(table).ToList();

            var updated = new List<Row>();

            foreach (var row in rows)
            {
                if (predicate(row))
                    updated.Add(table.CreateRow(updater(row)));
                else
                    updated.Add(row);
            }

            _dbService.Storage.RewriteTable(table, updated);
        }

        private Table GetTable(string name)
            => _dbService.Database.Tables
                .FirstOrDefault(t => t.Name == name)
                ?? throw new InvalidOperationException("Table not found.");
    }
}