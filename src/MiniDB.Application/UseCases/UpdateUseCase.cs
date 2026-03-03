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

        public void Execute(string tableName, Func<Row, bool> predicate, Action<Row> updateAction)
        {
            var table = _dbService.Database.GetTable(tableName);

            var rows = _dbService.ReadRows(table).ToList();

            foreach (var row in rows)
            {
                if (predicate(row))
                {
                    updateAction(row);
                }
            }

            _dbService.RewriteTable(table, rows);
        }
    }
}