namespace Bender.Reflection
{
    public interface IOptional
    {
        bool HasValue { get; }
    }

    public interface IOptional<T> : IOptional
    {
        T Value { get; set; }
    }

    public struct Optional<T> : IOptional<T>
    {
        private T _value;

        public bool HasValue { get; private set; }

        public T Value
        {
            get { return _value; }
            set
            {
                HasValue = true;
                _value = value;
            }
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T> { Value = value };
        }

        public static implicit operator T(Optional<T> optional)
        {
            return optional.Value;
        }
    }
}
