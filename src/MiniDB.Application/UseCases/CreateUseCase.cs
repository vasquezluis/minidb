using MiniDB.Application.Services;
using MiniDB.Domain;

namespace MiniDB.Application.UseCases
{
    public class CreateUseCase
    {
        private readonly DatabaseService _dbService;

        public CreateUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Execute(string tableName, IEnumerable<Column> columns)
        {
            if (_dbService.Database.Tables.Any(t => t.Name == tableName))
                throw new InvalidOperationException("Table already exists.");

            var table = new Table(tableName, columns.ToList());

            _dbService.Database.RegisterTable(table);

            _dbService.Storage.SaveTable(table);
        }
    }
}