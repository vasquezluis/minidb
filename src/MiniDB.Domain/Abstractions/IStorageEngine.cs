namespace MiniDB.Domain.Abstractions
{
    public interface IStorageEngine
    {
        Database LoadDatabase(string path);
        void SaveTable(Table table);

        void InsertRow(Table table, Row row);
        IEnumerable<Row> ReadRows(Table table);

        void RewriteTable(Table table, IEnumerable<Row> rows);
    }
}