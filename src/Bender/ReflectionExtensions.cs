using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bender
{
    public class ParseException : FormatException
    {
        public ParseException(string friendlyMessage) : base(friendlyMessage)
        { FriendlyMessage = friendlyMessage; }
        public ParseException(Exception exception, string friendlyMessage) : base(exception.Message, exception)
        { FriendlyMessage = friendlyMessage; }
        public string FriendlyMessage { get; private set; }
    }

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

        public static object ParseSimpleType(this string value, Type type, bool defaultNonNullableTypes, Dictionary<Type, string> parseErrorMesages)
        {
            if (value.IsNullOrEmpty() && type.IsNullable()) return null;
            var returnDefault = value.IsNullOrEmpty() && defaultNonNullableTypes;
            if (type.IsEnumOrNullable())
                return returnDefault ? Activator.CreateInstance(type) : Exceptions.Wrap(() => Enum.Parse(type.GetUnderlyingNullableType(), value),
                                                    x => new ParseException(x, parseErrorMesages[typeof(Enum)]));

            switch (type.GetTypeCode(true))
            {
                case TypeCode.String: return value;
                case TypeCode.Char: if (value != null && value.Length == 1) return value.First(); throw new ParseException(parseErrorMesages[typeof(char)]);
                case TypeCode.Boolean: return !returnDefault && Exceptions.Wrap(() => Boolean.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(bool)]));
                case TypeCode.SByte: return returnDefault ? (sbyte)0 : Exceptions.Wrap(() => SByte.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(sbyte)]));
                case TypeCode.Byte: return returnDefault ? (byte)0 : Exceptions.Wrap(() => Byte.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(byte)]));
                case TypeCode.Int16: return returnDefault ? (short)0 : Exceptions.Wrap(() => Int16.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(short)]));
                case TypeCode.UInt16: return returnDefault ? (ushort)0 : Exceptions.Wrap(() => UInt16.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(ushort)]));
                case TypeCode.Int32: return returnDefault ? 0 : Exceptions.Wrap(() => Int32.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(int)]));
                case TypeCode.UInt32: return returnDefault ? (uint)0 : Exceptions.Wrap(() => UInt32.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(uint)]));
                case TypeCode.Int64: return returnDefault ? (long)0 : Exceptions.Wrap(() => Int64.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(long)]));
                case TypeCode.UInt64: return returnDefault ? (ulong)0 : Exceptions.Wrap(() => UInt64.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(ulong)]));
                case TypeCode.Single: return returnDefault ? (float)0 : Exceptions.Wrap(() => Single.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(float)]));
                case TypeCode.Double: return returnDefault ? (double)0 : Exceptions.Wrap(() => Double.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(double)]));
                case TypeCode.Decimal: return returnDefault ? (decimal)0 : Exceptions.Wrap(() => Decimal.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(decimal)]));
                case TypeCode.DateTime: return returnDefault ? DateTime.MinValue : Exceptions.Wrap(() => DateTime.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(DateTime)]));
                default:
                    if (type.IsTypeOrNullable<Guid>()) return returnDefault ? Guid.Empty : Exceptions.Wrap(() => Guid.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(Guid)]));
                    if (type.IsTypeOrNullable<TimeSpan>()) return returnDefault ? TimeSpan.Zero : Exceptions.Wrap(() => TimeSpan.Parse(value), x => new ParseException(x, parseErrorMesages[typeof(TimeSpan)]));
                    throw new InvalidOperationException("Could not parse type {0}.".ToFormat(type));
            }
        }

        public static string ToFriendlyType(this Type type)
        {
            if (type.IsEnumOrNullable()) return "enumeration";
            switch (type.GetTypeCode(true))
            {
                case TypeCode.String: return "string";
                case TypeCode.Char: return "char";
                case TypeCode.Boolean: return "boolean";
                case TypeCode.SByte: return "signedByte";
                case TypeCode.Byte: return "byte";
                case TypeCode.Int16: return "word";
                case TypeCode.UInt16: return "usignedWord";
                case TypeCode.Int32: return "integer";
                case TypeCode.UInt32: return "usignedInteger";
                case TypeCode.Int64: return "long";
                case TypeCode.UInt64: return "usignedLong";
                case TypeCode.Single: return "singleFloat";
                case TypeCode.Double: return "doubleFloat";
                case TypeCode.Decimal: return "decimal";
                case TypeCode.DateTime: return "datetime";
                default:
                    if (type.IsTypeOrNullable<Guid>()) return "guid";
                    if (type.IsTypeOrNullable<TimeSpan>()) return "duration";
                    return type.Name;
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
                throw new ArgumentException("Interface {0} does not inherit from IEnumerable<T>.".ToFormat(type), "type");
            }
            if (type.IsArray) return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetElementType())); 
            if (type.IsList()) return (IList)Activator.CreateInstance(type);
            throw new ArgumentException("Type {0} does not implement IList.".ToFormat(type), "type");
        }

        public static T ParseEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof (T), value, true);
        }
    }
}