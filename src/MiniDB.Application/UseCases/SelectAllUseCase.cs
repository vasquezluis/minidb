using MiniDB.Application.Services;
using MiniDB.Domain;

namespace MiniDB.Application.UseCases
{
    public class SelectAllUseCase
    {
        private readonly DatabaseService _dbService;

        public SelectAllUseCase(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public IEnumerable<Row> Execute(string tableName)
        {
            var table = _dbService.Database.GetTable(tableName);

            return _dbService.ReadRows(table);
        }
    }
}