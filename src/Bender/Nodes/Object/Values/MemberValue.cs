using System;
using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public class MemberValue : IValue
    {
        private readonly IValue _value;
        private readonly CachedMember _member;

        public MemberValue(IValue value, CachedMember member, Func<CachedType, bool> useActualType)
        {
            _value = value;
            _member = member;
            var type = member.Type;
            SpecifiedType = useActualType(type) && Instance != null ? Instance.GetCachedType() : type;
            IsReadonly = _member.IsReadonly;
        }

        public object Instance
        {
            get { return _member.GetValue(_value.Instance); }
            set { _member.SetValue(_value.Instance, value); }
        }

        public CachedType SpecifiedType { get; private set; }
        public bool IsReadonly { get; private set; }
        public void EnsureValue() { }

        public CachedType ActualType
        {
            get { return Instance != null ? Instance.GetCachedType() : SpecifiedType; }
        }
    }
}