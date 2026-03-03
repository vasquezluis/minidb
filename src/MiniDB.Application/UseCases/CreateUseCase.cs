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
            var table = new Table(tableName, columns);

            _dbService.Database.RegisterTable(table);

            _dbService.SaveTable(table);
        }
    }
}