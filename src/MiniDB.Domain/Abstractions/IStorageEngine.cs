namespace MiniDB.Domain.Abstractions
{
    public interface IStorageEngine
    {
        void CreateTable(Table table);
        void Insert(string tableName, Row row);
        IEnumerable<Row> Scan(string tableName);
        void Update(string tableName, Func<Row, bool> predicate, Action<Row> update);
        void Delete(string tableName, Func<Row, bool> predicate);
    }
}