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
            var table = _dbService.Database.GetTable(tableName);

            var rows = _dbService.ReadRows(table).ToList();

            var remainingRows = rows.Where(r => !predicate(r)).ToList();

            _dbService.RewriteTable(table, remainingRows);
        }
    }
}