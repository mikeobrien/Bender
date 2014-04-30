using System;
using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public class LazyValue : IValue
    {
        private readonly Func<object> _factory;
        private bool _initialized;
        private readonly Lazy<CachedType> _actualType; 

        public LazyValue(IValue value, Func<object> factory)
        {
            InnerValue = value;
            _factory = factory;
            _actualType = new Lazy<CachedType>(() => InnerValue.Instance.GetCachedType());
        }

        public object Instance
        {
            get { return _initialized ? InnerValue.Instance : Initialize(); }
            set
            {
                _initialized = true;
                InnerValue.Instance = value;
            }
        }

        public CachedType SpecifiedType { get { return InnerValue.SpecifiedType; } }
        public bool IsReadonly { get { return InnerValue.IsReadonly; } }
        public IValue InnerValue { get; private set; }

        public CachedType ActualType
        {
            get 
            { 
                return _initialized && InnerValue.Instance != null ?
                    _actualType.Value : SpecifiedType;
            }
        }

        public void EnsureValue()
        {
            Initialize();
        }

        private object Initialize()
        {
            return Instance = _factory();
        }
    }
}