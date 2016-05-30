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
            bool @readonly = false)
        {
            Instance = instance;
            SpecifiedType = specifiedType;
            IsReadonly = @readonly;
        }

        public object Instance { get; set; }
        public CachedType SpecifiedType { get; }
        public bool IsReadonly { get; }
        public void EnsureValue() { }

        public CachedType ActualType => Instance != null ? Instance.ToCachedType() : SpecifiedType;
    }
}