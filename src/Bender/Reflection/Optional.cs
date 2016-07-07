using Bender.Collections;

namespace Bender.Reflection
{
    public interface IOptional
    {
        bool HasValue { get; }
        object Value { get; set; }
    }

    public interface IOptional<T> : IOptional
    {
        new T Value { get; set; }
    }

    public struct Optional<T> : IOptional<T>
    {
        private T _value;

        public bool HasValue { get; private set; }

        object IOptional.Value
        {
            get { return Value; }
            set { Value = (T)value; }
        }

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

        public override bool Equals(object @object)
        {
            return _value != null && @object is IOptional ? 
                _value.Equals(@object.As<IOptional>().Value) :
                base.Equals(@object);
        }

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? base.GetHashCode();
        }
    }
}
