using System;
using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public class MemberValue : IValue
    {
        private readonly IValue _value;
        private readonly CachedMember _member;

        public MemberValue(IValue value, CachedMember member, 
            Func<CachedType, bool> useActualType)
        {
            _value = value;
            _member = member;
            var type = member.Type;
            SpecifiedType = useActualType(type) && Instance != null ? 
                Instance.ToCachedType() : type;
            IsReadonly = _member.IsReadonly;
        }

        public object Instance
        {
            get { return _member.GetValue(_value.Instance); }
            set { _member.SetValue(_value.Instance, value); }
        }

        public CachedType SpecifiedType { get; }
        public bool IsReadonly { get; }
        public void EnsureValue() { }

        public CachedType ActualType => Instance != null ? 
            Instance.ToCachedType() : SpecifiedType;
    }
}