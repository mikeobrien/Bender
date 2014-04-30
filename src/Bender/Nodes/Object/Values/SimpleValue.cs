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
        public CachedType SpecifiedType { get; private set; }
        public bool IsReadonly { get; private set; }
        public void EnsureValue() { }

        public CachedType ActualType
        { 
            get { return Instance != null ? Instance.GetCachedType() : SpecifiedType; } 
        }
    }
}