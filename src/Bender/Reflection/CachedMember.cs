using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bender.Collections;

namespace Bender.Reflection
{
    public class CachedMember
    {
        private readonly Lazy<IEnumerable<Attribute>> _attributes;
        private readonly Lazy<bool> _isPublicPropertyOrField;
        private readonly Lazy<CachedType> _type;
        private readonly Lazy<CachedType> _declaringType;
        private readonly Lazy<Type> _memberType;
        private readonly Lazy<bool> _isOptional;
        private readonly Lazy<bool> _isReadonly;
        private readonly Lazy<Action<object, object>> _setter;
        private readonly Lazy<Func<object, object>> _getter;
        private readonly Lazy<Func<object, object>> _underlyingGetter;
        private readonly Lazy<Action<object, object, object>> _indexedSetter;
        private readonly Lazy<Func<object, object, object>> _indexedGetter; 

        public CachedMember(MemberInfo member)
        {
            Name = member.Name;
            MemberInfo = member;
            IsProperty = member.MemberType == MemberTypes.Property;
            IsField = member.MemberType == MemberTypes.Field;
            if (IsField) FieldInfo = (FieldInfo)member;
            if (IsProperty)
            {
                PropertyInfo = (PropertyInfo)member;
                HasGetter = PropertyInfo.GetMethod != null;
            }
            _isPublicPropertyOrField = new Lazy<bool>(member.IsPublicPropertyOrField);
            _attributes = new Lazy<IEnumerable<Attribute>>(
                () => member.GetCustomAttributes(true).Cast<Attribute>().ToList());
            _memberType = new Lazy<Type>(member.GetPropertyOrFieldType);
            _isOptional = new Lazy<bool>(() => _memberType.Value.IsOptional());
            _type = new Lazy<CachedType>(() => (_isOptional.Value ? 
                _memberType.Value.GetUnderlyingOptionalType() : 
                _memberType.Value).ToCachedType());
            _declaringType = new Lazy<CachedType>(() => member.DeclaringType.ToCachedType());
            _isReadonly = new Lazy<bool>(member.IsReadonly);

            _setter = new Lazy<Action<object, object>>(() => IsProperty ? 
                PropertyInfo.BuildSetter(_type.Value) : 
                FieldInfo.BuildSetter(_type.Value));
            _getter = new Lazy<Func<object, object>>(() => IsProperty ? 
                PropertyInfo.BuildGetter(_type.Value) : 
                FieldInfo.BuildGetter(_type.Value));
            _underlyingGetter = new Lazy<Func<object, object>>(() => IsProperty ?
                PropertyInfo.BuildGetter() :
                FieldInfo.BuildGetter());

            if (IsProperty)
            {
                _indexedSetter = new Lazy<Action<object, object, object>>(
                    () => PropertyInfo.BuildIndexedSetter());
                _indexedGetter = new Lazy<Func<object, object, object>>(
                    () => PropertyInfo.BuildIndexedGetter());
            }
        }

        public MemberInfo MemberInfo { get; }
        public PropertyInfo PropertyInfo { get; }
        public FieldInfo FieldInfo { get; }

        public string Name { get; set; }
        public CachedType Type => _type.Value;
        public CachedType DeclaringType => _declaringType.Value;
        public bool IsProperty { get; }
        public bool HasGetter { get; }
        public bool IsField { get; }
        public bool IsPublicPropertyOrField => _isPublicPropertyOrField.Value;
        public bool IsReadonly => _isReadonly.Value;
        public bool IsOptional => _isOptional.Value;
        public IEnumerable<Attribute> Attributes => _attributes.Value;

        public static implicit operator MemberInfo(CachedMember member)
        {
            return member.MemberInfo;
        }

        public static implicit operator PropertyInfo(CachedMember member)
        {
            return member.PropertyInfo;
        }

        public static implicit operator FieldInfo(CachedMember member)
        {
            return member.FieldInfo;
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _attributes.Value.Any(x => x is T);
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return (T)_attributes.Value.FirstOrDefault(x => x is T);
        }

        public bool HasValue(object instance)
        {
            return !IsOptional || _underlyingGetter.Value(instance).As<IOptional>().HasValue;
        }

        public void SetValue(object instance, object value)
        {
            _setter.Value(instance, value);
        }

        public object GetValue(object instance)
        {
            return _getter.Value(instance);
        }

        public void SetValue(object instance, object name, object value)
        {
            _indexedSetter.Value(instance, name, value);
        }

        public object GetValue(object instance, object name)
        {
            return _indexedGetter.Value(instance, name);
        }
    }
}