namespace MiniDB.Domain
{
    public class Row
    {
        private readonly List<object?> _values;

        public IReadOnlyList<object?> Values => _values;

        public Row(IEnumerable<object?> values)
        {
            _values = values.ToList();
        }

        public object? GetValue(int index)
        {
            return _values[index];
        }

        public void SetValue(int index, object? value)
        {
            _values[index] = value;
        }
    }
}