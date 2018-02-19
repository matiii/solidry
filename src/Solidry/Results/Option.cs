namespace Solidry.Results
{
    /// <summary>
    /// Instead return null, return option.
    /// </summary>
    /// <typeparam name="T">Payload</typeparam>
    public struct Option<T>
    {
        public T Value { get; }

        public bool HasValue { get; }

        public Option(T value)
        {
            HasValue = true;
            Value = value;
        }

        public static Option<T> Create(T value) => new Option<T>(value);

        public static Option<T> Empty { get; } = new Option<T>();
    }
}