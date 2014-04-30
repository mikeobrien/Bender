using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;

namespace Bender.Reflection
{
    public static class CollectionExtensions
    {
        private static readonly string BclCollectionNamespace = typeof(IEnumerable).Namespace;

        public static bool IsInBclCollectionNamespace(this Type type)
        {
            return (type.Namespace != null && 
                (type.Namespace.StartsWith(BclCollectionNamespace + ".") ||
                    type.Namespace == BclCollectionNamespace));
        }

        // Arrays

        public static object CreateArray(this Type type, int length = 0)
        {
            return Activator.CreateInstance(type, length);
        }

        public static object CreateArray(this CachedType type, int length = 0)
        {
            return type.Type.CreateArray();
        }

        // Enumerable 

        public static bool IsEnumerable(this Type type)
        {
            return type.IsEnumerableInterface() || type.GetInterfaces().Any(x => x == typeof(IEnumerable));
        }

        public static bool IsEnumerableInterface(this Type type)
        {
            return type == typeof(IEnumerable);
        }

        public static bool IsGenericEnumerable(this Type type)
        {
            return type.IsGenericEnumerableInterface() ||
                type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsGenericEnumerableInterface(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        public static Type GetGenericEnumerableType(this Type type)
        {
            var enumerableInterface = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ? type :
                type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return enumerableInterface == null ? null : enumerableInterface.GetGenericArguments()[0];
        }

        // List

        public static bool IsNonGenericList(this Type type)
        {
            return type == typeof(IList) || type.Implements<IList>();
        }

        public static bool IsNonGenericList(this object @object)
        {
            return @object != null && @object.GetType().IsNonGenericList();
        }

        public static bool IsList(this Type type)
        {
            return type == typeof(IList) || type.Implements<IList>() || type.IsGenericList();
        }

        public static bool IsList(this object @object)
        {
            return @object != null && @object.GetType().IsList();
        }

        public static bool IsListInterface(this Type type)
        {
            return type == typeof(IList);
        }

        public static bool IsGenericList(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericListInterface()) || type.IsGenericListInterface();
        }

        public static bool IsGenericList(this object @object)
        {
            return @object != null && @object.GetType().IsGenericList();
        }

        public static bool IsGenericListInterface(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
        }

        public static Type MakeConcreteGenericListType(this Type type)
        {
            return typeof(List<>).MakeGenericType(type.GetGenericEnumerableType());
        }

        // Dictionary

        public static bool IsNonGenericDictionary(this Type type)
        {
            return type == typeof(IDictionary) || type.Implements<IDictionary>();
        }

        public static bool IsNonGenericDictionary(this object @object)
        {
            return @object != null && @object.GetType().IsNonGenericDictionary();
        }

        public static bool IsDictionary(this Type type)
        {
            return type.IsNonGenericDictionary() || type.IsGenericDictionary();
        }

        public static bool IsDictionaryInterface(this Type type)
        {
            return type == typeof(IDictionary);
        }

        public static bool IsGenericDictionary(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericDictionaryInterface()) || type.IsGenericDictionaryInterface();
        }

        public static bool IsGenericDictionary(this object @object)
        {
            return @object != null && @object.GetType().IsGenericDictionary();
        }

        public static bool IsGenericDictionaryInterface(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>);
        }

        public static KeyValuePair<Type, Type> GetGenericDictionaryTypes(this Type type)
        {
            return (type.IsGenericDictionaryInterface() ? type
                : type.GetInterfaces().First(x => x.IsGenericDictionaryInterface()))
                .GetGenericArguments()
                .Map(x => new KeyValuePair<Type, Type>(x[0], x[1]));
        }

        public static Type MakeConcreteGenericDictionaryType(this Type type)
        {
            return type.GetGenericDictionaryTypes()
                .Map(x => typeof(Dictionary<,>).MakeGenericType(x.Key, x.Value));
        }
    }
}
