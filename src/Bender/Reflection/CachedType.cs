using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;

namespace Bender.Reflection
{
    public class CachedType
    {
        private readonly Lazy<TypeCode> _typeCode;
        private readonly Lazy<IEnumerable<CachedMember>> _filteredMembers;
        private readonly Lazy<ConcurrentDictionary<string, CachedMember>> _members;
        private readonly Lazy<ConcurrentDictionary<int, Action<object, object[]>>> _actions;
        private readonly Lazy<ConcurrentDictionary<int, Func<object, object[], object>>> _funcs; 
        private readonly Lazy<bool> _isEnum;
        private readonly Lazy<bool> _isSimpleType;
        private readonly Lazy<bool> _isEnumerable;
        private readonly Lazy<bool> _isDictionary;
        private readonly Lazy<bool> _isNonGenericList;
        private readonly Lazy<bool> _isGenericList;
        private readonly Lazy<bool> _isGenericListInterface;
        private readonly Lazy<bool> _isList;
        private readonly Lazy<bool> _isListInterface;
        private readonly Lazy<bool> _isGenericDictionary;
        private readonly Lazy<bool> _isNonGenericDictionary;
        private readonly Lazy<bool> _isGenericDictionaryInterface;
        private readonly Lazy<bool> _isDictionaryInterface;
        private readonly Lazy<bool> _isGenericEnumerableInterface;
        private readonly Lazy<bool> _isEnumerableInterface;
        private readonly Lazy<bool> _isGenericEnumerable;
        private readonly Lazy<CachedType> _genericEnumerableType;
        private readonly Lazy<bool> _isNullable;
        private readonly Lazy<Type> _concreteGenericListType;
        private readonly Lazy<Type> _concreteGenericDictionaryType;
        private readonly Lazy<string> _genericBaseName;
        private readonly Lazy<string> _friendlyFullName;
        private readonly Lazy<IEnumerable<CachedType>> _genericTypeArguments;
        private readonly Lazy<CachedType> _underlyingType;
        private readonly Lazy<CachedType> _elementType;
        private readonly Lazy<KeyValuePair<CachedType, CachedType>> _genericDictionaryTypes;
        private readonly Lazy<IEnumerable<Attribute>> _attributes;

        public CachedType(Type type)
        {
            Type = type;
            Name = type.Name;
            FullName = type.FullName;
            _friendlyFullName = new Lazy<string>(type.GetFriendlyTypeFullName);
            _genericBaseName = new Lazy<string>(type.GetGenericTypeBaseName);
            _filteredMembers = new Lazy<IEnumerable<CachedMember>>(() => 
                type.GetPropertiesAndFields(
                    typeof(IEnumerable), typeof(IEnumerable<>),
                    typeof(IList), typeof(IList<>),
                    typeof(IDictionary), typeof(IDictionary<,>),
                    typeof(ICollection), typeof(ICollection<>))
                    .Select(x => new CachedMember(x)).ToList());
            _members = new Lazy<ConcurrentDictionary<string, CachedMember>>(
                () => new ConcurrentDictionary<string, CachedMember>());
            _actions = new Lazy<ConcurrentDictionary<int, Action<object, object[]>>>(
                () => new ConcurrentDictionary<int, Action<object, object[]>>());
            _funcs = new Lazy<ConcurrentDictionary<int, Func<object, object[], object>>>(
                () => new ConcurrentDictionary<int, Func<object, object[], object>>());
            _isSimpleType = new Lazy<bool>(type.IsSimpleType);
            _isEnumerable = new Lazy<bool>(type.IsEnumerable);
            _isDictionary = new Lazy<bool>(type.IsDictionary);
            _isList = new Lazy<bool>(type.IsList);
            _isNonGenericList = new Lazy<bool>(type.IsNonGenericList);
            _isGenericList = new Lazy<bool>(type.IsGenericList);
            _isGenericListInterface = new Lazy<bool>(type.IsGenericListInterface);
            _isListInterface = new Lazy<bool>(type.IsListInterface);
            _isNonGenericDictionary = new Lazy<bool>(type.IsNonGenericDictionary);
            _isGenericDictionary = new Lazy<bool>(type.IsGenericDictionary);
            _isGenericDictionaryInterface = new Lazy<bool>(type.IsGenericDictionaryInterface);
            _isDictionaryInterface = new Lazy<bool>(type.IsDictionaryInterface);
            _isGenericEnumerableInterface = new Lazy<bool>(type.IsGenericEnumerableInterface);
            _isEnumerableInterface = new Lazy<bool>(type.IsEnumerableInterface);
            _isGenericEnumerable = new Lazy<bool>(type.IsGenericEnumerable);
            _genericEnumerableType = new Lazy<CachedType>(() => type.GetGenericEnumerableType().GetCachedType());
            IsGenericType = type.IsGenericType;
            _genericTypeArguments = new Lazy<IEnumerable<CachedType>>(() => 
                type.GetGenericArguments().Select(x => x.GetCachedType()));
            _isNullable = new Lazy<bool>(type.IsNullable);
            IsArray = type.IsArray;
            IsInterface = type.IsInterface;
            IsValueType = type.IsValueType;
            _typeCode = new Lazy<TypeCode>(type.GetTypeCode);
            IsGenericType = type.IsGenericType;
            IsInBclCollectionNamespace = type.IsInBclCollectionNamespace();
            _concreteGenericListType = new Lazy<Type>(type.MakeConcreteGenericListType);
            _concreteGenericDictionaryType = new Lazy<Type>(type.MakeConcreteGenericDictionaryType);
            _underlyingType = new Lazy<CachedType>(() => TypeCache.GetType(type.GetUnderlyingNullableType()));
            _isEnum = new Lazy<bool>(() => type.GetUnderlyingNullableType().IsEnum);
            _elementType = new Lazy<CachedType>(() => TypeCache.GetType(type.GetElementType()));
            _genericDictionaryTypes = new Lazy<KeyValuePair<CachedType, CachedType>>(
                () => type.GetGenericDictionaryTypes().Map(x => new KeyValuePair<CachedType, CachedType>(x.Key.GetCachedType(), x.Value.GetCachedType())));
            _attributes = new Lazy<IEnumerable<Attribute>>(
                () => type.GetCustomAttributes(true).Cast<Attribute>().ToList());

        }

        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string FriendlyFullName { get { return _friendlyFullName.Value; } }
        public string GenericBaseName { get { return _genericBaseName.Value; } }
        public Type Type { get; private set; }
        public CachedType UnderlyingType { get { return _underlyingType.Value; } }

        public IEnumerable<CachedMember> Members { get { return _filteredMembers.Value; } }
        public bool IsSimpleType { get { return _isSimpleType.Value; } }
        public bool IsEnumerable { get { return _isEnumerable.Value; } }
        public bool IsDictionary { get { return _isDictionary.Value; } }
        public bool IsList { get { return _isList.Value; } }
        public bool IsGenericListInterface { get { return _isGenericListInterface.Value; } }
        public bool IsListInterface { get { return _isListInterface.Value; } }
        public bool IsNonGenericDictionary { get { return _isNonGenericDictionary.Value; } }
        public bool IsGenericDictionary { get { return _isGenericDictionary.Value; } }
        public bool IsGenericDictionaryInterface { get { return _isGenericDictionaryInterface.Value; } }
        public bool IsDictionaryInterface { get { return _isDictionaryInterface.Value; } }
        public bool IsNonGenericList { get { return _isNonGenericList.Value; } }
        public bool IsGenericList { get { return _isGenericList.Value; } }
        public bool IsGenericEnumerableInterface { get { return _isGenericEnumerableInterface.Value; } }
        public bool IsEnumerableInterface { get { return _isEnumerableInterface.Value; } }
        public bool IsGenericEnumerable { get { return _isGenericEnumerable.Value; } }
        public CachedType GenericEnumerableType { get { return _genericEnumerableType.Value; } }
        public CachedType ElementType { get { return _elementType.Value; } }
        public KeyValuePair<CachedType, CachedType> GenericDictionaryTypes { get { return _genericDictionaryTypes.Value; } }
        public bool IsNullable { get { return _isNullable.Value; } }
        public bool IsGenericType { get; private set; }
        public IEnumerable<CachedType> GenericArguments { get { return _genericTypeArguments.Value; } }
        public bool IsArray { get; private set; }
        public bool IsEnum { get { return _isEnum.Value; } }
        public bool IsValueType { get; private set; }
        public bool IsInterface { get; private set; }
        public TypeCode TypeCode { get { return _typeCode.Value; } }
        public bool IsInBclCollectionNamespace { get; private set; }
        public bool IsBclCollectionType { get { return IsInBclCollectionNamespace && IsEnumerable; } }

        public object CreateGenericListInstance()
        {
            return _concreteGenericListType.Value.CreateInstance();
        }

        public object CreateGenericDictionaryInstance()
        {
            return _concreteGenericDictionaryType.Value.CreateInstance();
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _attributes.Value.Any(x => x is T);
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return (T)_attributes.Value.FirstOrDefault(x => x is T);
        }

        public CachedMember GetMember(string name)
        {
            if (!_members.Value.ContainsKey(name))
            {
                var member = new CachedMember(Type.GetMember(name)[0]);
                if (_members.Value.TryAdd(name, member)) return member;
            }
            return _members.Value[name];
        }

        public CachedMember GetIndexer()
        {
            return GetMember("Item");
        }

        public void InvokeAction(string name, object instance, params object[] parameters)
        {
            GetMethod(_actions.Value,
                (n, p) => Type.CreateActionDelegate(name, p),
                name, parameters)(instance, parameters);
        }

        public TResult InvokeFunc<TResult>(string name, object instance, params object[] parameters)
        {
            return (TResult)GetMethod(_funcs.Value, 
                (n, p) => Type.CreateFuncDelegate(name, p),
                name, parameters)(instance, parameters);
        }

        private T GetMethod<T>(
            ConcurrentDictionary<int, T> methods, 
            Func<string, Type[], T> factory, 
            string name,  
            params object[] parameters)
        {
            var parameterTypes = parameters.Select(x => x.GetType());
            var id = name.GetHashCode();
            if (parameterTypes.Any()) id = id | parameterTypes.Select(x => x.GetHashCode()).Aggregate((a, i) => a | i);
            T method;

            if (!_funcs.Value.ContainsKey(id))
            {
                method = factory(name, parameterTypes.ToArray());
                if (!methods.TryAdd(id, method)) method = methods[id];
            }
            else method = methods[id];
            return method;
        }
    }
}