using MiniDB.Application.Services;

namespace MiniDB.Application.UseCases
{
    public class InsertRowUseCase
    {
        private readonly DatabaseService _dbService;

        public InsertRowUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Execute(string tableName, IEnumerable<object?> values)
        {
            var table = _dbService.Database.GetTable(tableName);

            var row = table.CreateRow(values);

            _dbService.InsertRow(table, row);
        }
    }
}