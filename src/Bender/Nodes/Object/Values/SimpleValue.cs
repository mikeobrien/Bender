using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public class SimpleValue : IValue
    {
        public SimpleValue(CachedType specifiedType)
        {
            SpecifiedType = specifiedType;
        }

        public SimpleValue(
            object instance,
            CachedType specifiedType, 
            bool @readonly = false,
            bool hasValue = true)
        {
            Instance = instance;
            SpecifiedType = specifiedType;
            IsReadonly = @readonly;
            HasValue = hasValue;
        }

        public object Instance { get; set; }
        public CachedType SpecifiedType { get; }
        public bool IsReadonly { get; }
        public bool HasValue { get; }
        public void EnsureValue() { }

        public CachedType ActualType => Instance != null ? Instance.ToCachedType() : SpecifiedType;
    }
}