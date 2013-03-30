using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bender
{
    public class ParseException : Exception { public ParseException(Type type) : base("Unable to parse " + type) { } }

    public static class ReflectionExtensions
    {
        public static List<PropertyInfo> GetSerializableProperties(this Type type, List<Func<Type, bool>> typeFilter = null)
        {
            return type.GetProperties(typeFilter).Where(x => x.CanRead).ToList();
        }

        public static List<PropertyInfo> GetDeserializableProperties(this Type type, List<Func<Type, bool>> typeFilter = null)
        {
            return type.GetProperties(typeFilter).Where(x => x.CanWrite).ToList();
        }

        private static IEnumerable<PropertyInfo> GetProperties(this Type type, IEnumerable<Func<Type, bool>> typeFilter = null)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => typeFilter == null || !typeFilter.Any(y => y(x.PropertyType)));
        }

        public static T GetCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return (T)property.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            return (T)type.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public static bool HasCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttribute<T>() != null;
        }

        public static object Parse(this string value, Type type, bool defaultNonNullableTypes)
        {
            if (value.IsNullOrEmpty() && type.IsNullable()) return null;
            var returnDefault = value.IsNullOrEmpty() && defaultNonNullableTypes;
            if (type.IsEnumOrNullable()) return returnDefault ?
                Activator.CreateInstance(type) : Enum.Parse(type.GetUnderlyingNullableType(), value);
            switch (type.GetTypeCode(true))
            {
                case TypeCode.String: return value;
                case TypeCode.Char: return value.First();
                case TypeCode.Boolean: return !returnDefault && Boolean.Parse(value);
                case TypeCode.SByte: return returnDefault ? (sbyte)0 : SByte.Parse(value);
                case TypeCode.Byte: return returnDefault ? (byte)0 : Byte.Parse(value);
                case TypeCode.Int16: return returnDefault ? (short)0 : Int16.Parse(value);
                case TypeCode.UInt16: return returnDefault ? (ushort)0 : UInt16.Parse(value);
                case TypeCode.Int32: return returnDefault ? 0 : Int32.Parse(value);
                case TypeCode.UInt32: return returnDefault ? (uint)0 : UInt32.Parse(value);
                case TypeCode.Int64: return returnDefault ? (long)0 : Int64.Parse(value);
                case TypeCode.UInt64: return returnDefault ? (ulong)0 : UInt64.Parse(value);
                case TypeCode.Single: return returnDefault ? (float)0 : Single.Parse(value);
                case TypeCode.Double: return returnDefault ? (double)0 : Double.Parse(value);
                case TypeCode.Decimal: return returnDefault ? (decimal)0 : Decimal.Parse(value);
                case TypeCode.DateTime: return returnDefault ? DateTime.MinValue : DateTime.Parse(value);
                default:
                    if (type.IsTypeOrNullable<Guid>()) return returnDefault ? Guid.Empty : Guid.Parse(value);
                    if (type.IsTypeOrNullable<TimeSpan>()) return returnDefault ? TimeSpan.MinValue : TimeSpan.Parse(value);
                    throw new ParseException(type);
            }
        }

        public static void SetValue(this PropertyInfo property, object instance, Func<object> value, Func<Exception, Exception> exception)
        {
            try
            {
                property.SetValue(instance, value(), null);
            }
            catch (Exception e)
            {
                throw exception(e);
            }
        }

        public static bool IsSimpleType(this Type type)
        {
            Func<Type, bool> isSimpleType = x => x.IsPrimitive || x.IsEnum || x == typeof(string) || x == typeof(byte[]) || x == typeof(decimal) ||
                 x == typeof(DateTime) || x == typeof(TimeSpan) || x == typeof(Guid) || x == typeof(Uri) || x == typeof(object);
            return isSimpleType(type) || (type.IsNullable() && isSimpleType(Nullable.GetUnderlyingType(type)));
        }

        public static Type GetUnderlyingNullableType(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }

        public static bool IsEnumOrNullable(this Type type)
        {
            return type.IsEnum || (type.IsNullable() && Nullable.GetUnderlyingType(type).IsEnum);
        }

        public static bool IsTypeOrNullable<T>(this Type type)
        {
            return type == typeof(T) || (type.IsNullable() && Nullable.GetUnderlyingType(type) == typeof(T));
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static TypeCode GetTypeCode(this Type type, bool includeNullable)
        {
            return includeNullable && type.IsNullable() ? Type.GetTypeCode(Nullable.GetUnderlyingType(type)) : Type.GetTypeCode(type);
        }

        public static bool IsBclType(this Type type)
        {
            return type.Namespace == "System" || type.Namespace.StartsWith("System.");
        }

        public static bool HasParameterlessConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool HasConstructor(this Type type, params Type[] arguments)
        {
            return type.GetConstructor(arguments) != null;
        }

        public static bool IsList(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsListInterface());
        }

        public static bool IsListInterface(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
        }

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

        public static bool IsClrCollectionType(this Type type)
        {
            return type.Namespace.StartsWith("System.Collections.") || type.Namespace == "System.Collections";
        }

        public static IList CreateListOfEnumerableType(this Type type)
        {
            if (type.IsInterface)
            {
                var itemType = type.GetGenericEnumerableType();
                if (itemType != null) return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType)); 
                throw new ArgumentException(string.Format("Interface {0} does not inherit from IEnumerable<T>.", type), "type");
            }
            if (type.IsArray) return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetElementType())); 
            if (type.IsList()) return (IList)Activator.CreateInstance(type);
            throw new ArgumentException(string.Format("Type {0} does not implement IList.", type), "type");
        }
    }
}