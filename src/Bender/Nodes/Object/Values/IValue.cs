using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public interface IValue
    {
        object Instance { get; set; }
        CachedType SpecifiedType { get; }
        CachedType ActualType { get; }
        bool IsReadonly { get; }
        bool HasValue { get; }

        void EnsureValue();
    }
}