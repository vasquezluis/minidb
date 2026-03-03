using System.Text;
using MiniDB.Domain;

namespace MiniDB.Infrastructure.Serialization
{
    public class MetadataSerializer
    {
        private const string MetadataFileName = "database.meta";

        public Database Load(string path)
        {
            var db = new Database();

            var fullPath = Path.Combine(path, MetadataFileName);

            if (!File.Exists(fullPath))
            {
                Directory.CreateDirectory(path);
                using (File.Create(fullPath)) { }
                return db;
            }

            using var stream = File.OpenRead(fullPath);
            using var reader = new BinaryReader(stream, Encoding.UTF8);

            if (stream.Length == 0)
                return db;

            int tableCount = reader.ReadInt32();

            for (int i = 0; i < tableCount; i++)
            {
                var table = ReadTable(reader);
                db.RegisterTable(table);
            }

            return db;
        }

        public void Save(string path, Database database)
        {
            var fullPath = Path.Combine(path, MetadataFileName);

            using var stream = File.Create(fullPath);
            using var writer = new BinaryWriter(stream, Encoding.UTF8);

            writer.Write(database.Tables.Count);

            foreach (var table in database.Tables)
            {
                WriteTable(writer, table);
            }
        }

        private void WriteTable(BinaryWriter writer, Table table)
        {
            WriteString(writer, table.Name);

            writer.Write(table.Columns.Count);

            foreach (var column in table.Columns)
            {
                WriteString(writer, column.Name);
                writer.Write((int)column.Type);
                writer.Write(column.MaxLength);
            }
        }

        private Table ReadTable(BinaryReader reader)
        {
            var tableName = ReadString(reader);
            int columnCount = reader.ReadInt32();

            var columns = new List<Column>();

            for (int i = 0; i < columnCount; i++)
            {
                var name = ReadString(reader);
                var type = (DataType)reader.ReadInt32();
                var maxLength = reader.ReadInt32();

                columns.Add(new Column(name, type, maxLength));
            }

            return new Table(tableName, columns);
        }

        private void WriteString(BinaryWriter writer, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        private string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            var bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}