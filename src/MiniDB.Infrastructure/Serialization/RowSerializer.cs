using System.Text;
using MiniDB.Domain;

namespace MiniDB.Infrastructure.Serialization
{
    public class RowSerializer
    {
        public void WriteRow(Stream stream, Table table, Row row, bool isDeleted = false)
        {
            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(isDeleted); // 1 byte

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];
                var value = row.GetValue(i);

                switch (column.Type)
                {
                    case DataType.Int:
                        writer.Write((int)value!);
                        break;

                    case DataType.String:
                        WriteFixedString(writer, (string)value!, column.MaxLength);
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported type {column.Type}");
                }
            }
        }

        public (Row Row, bool IsDeleted) ReadRow(Stream stream, Table table)
        {
            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            bool isDeleted = reader.ReadBoolean();

            var values = new object?[table.Columns.Count];

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];

                switch (column.Type)
                {
                    case DataType.Int:
                        values[i] = reader.ReadInt32();
                        break;

                    case DataType.String:
                        values[i] = ReadFixedString(reader, column.MaxLength);
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported type {column.Type}");
                }
            }

            return (new Row(values), isDeleted);
        }

        private void WriteFixedString(BinaryWriter writer, string value, int maxLength)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            if (bytes.Length > maxLength)
                throw new InvalidOperationException(
                    $"String exceeds fixed length of {maxLength} bytes.");

            writer.Write(bytes);

            int padding = maxLength - bytes.Length;
            if (padding > 0)
            {
                writer.Write(new byte[padding]);
            }
        }

        private string ReadFixedString(BinaryReader reader, int maxLength)
        {
            var bytes = reader.ReadBytes(maxLength);

            int actualLength = Array.FindLastIndex(bytes, b => b != 0) + 1;

            if (actualLength <= 0)
                return string.Empty;

            return Encoding.UTF8.GetString(bytes, 0, actualLength);
        }
    }
}