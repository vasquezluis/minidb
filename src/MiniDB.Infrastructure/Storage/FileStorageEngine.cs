using System.Text;
using MiniDB.Domain;
using MiniDB.Domain.Abstractions;
using MiniDB.Infrastructure.Serialization;

namespace MiniDB.Infrastructure.Storage
{
    public class FileStorageEngine : IStorageEngine
    {
        private readonly string _basePath;
        private readonly MetadataSerializer _metadataSerializer;
        private readonly RowSerializer _rowSerializer;

        public FileStorageEngine(string basePath)
        {
            _basePath = basePath;
            _metadataSerializer = new MetadataSerializer();
            _rowSerializer = new RowSerializer();
        }

        public Database LoadDatabase(string path)
        {
            return _metadataSerializer.Load(path);
        }

        public void SaveTable(Table table)
        {
            var db = _metadataSerializer.Load(_basePath);
            db.RegisterTable(table);
            _metadataSerializer.Save(_basePath, db);

            CreateTableFile(table);
        }

        public void InsertRow(Table table, Row row)
        {
            var filePath = GetTableFilePath(table);

            using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            _rowSerializer.WriteRow(stream, table, row);
        }

        public IEnumerable<Row> ReadRows(Table table)
        {
            var filePath = GetTableFilePath(table);

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            SkipHeader(stream);

            while (stream.Position < stream.Length)
            {
                var (row, isDeleted) = _rowSerializer.ReadRow(stream, table);

                if (!isDeleted)
                    yield return row;
            }
        }

        public void RewriteTable(Table table, IEnumerable<Row> rows)
        {
            var filePath = GetTableFilePath(table);
            var tempPath = filePath + ".tmp";

            using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            {
                WriteHeader(stream, table);

                foreach (var row in rows)
                {
                    _rowSerializer.WriteRow(stream, table, row);
                }
            }

            File.Delete(filePath);
            File.Move(tempPath, filePath);
        }

        private string GetTableFilePath(Table table)
            => Path.Combine(_basePath, $"{table.Name}.table");

        private void CreateTableFile(Table table)
        {
            var path = GetTableFilePath(table);

            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            WriteHeader(stream, table);
        }

        private void WriteHeader(Stream stream, Table table)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(table.CalculateRowSize());
            writer.Write(table.Columns.Count);
        }

        private void SkipHeader(Stream stream)
        {
            stream.Seek(sizeof(int) * 2, SeekOrigin.Begin);
        }
    }
}