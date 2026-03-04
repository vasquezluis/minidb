using MiniDB.Application.Services;
using MiniDB.Domain;

namespace MiniDB.Application.UseCases
{
    public class InsertUseCase
    {
        private readonly DatabaseService _dbService;

        public InsertUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Execute(string tableName, IEnumerable<object?> values)
        {
            var table = GetTable(tableName);

            var row = table.CreateRow(values);

            _dbService.Storage.InsertRow(table, row);
        }

        private Table GetTable(string name)
            => _dbService.Database.Tables
                .FirstOrDefault(t => t.Name == name)
                ?? throw new InvalidOperationException("Table not found.");
    }
}