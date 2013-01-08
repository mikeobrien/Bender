using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Xml.Serialization;

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

        private static IEnumerable<PropertyInfo> GetProperties(this Type type, List<Func<Type, bool>> typeFilter = null)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => !x.HasCustomAttribute<XmlIgnoreAttribute>() &&
                            (typeFilter == null || !typeFilter.Any(y => y(x.PropertyType))));
        }

        public static T GetCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return (T)property.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            return (T)type.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));
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

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsArray) return Activator.CreateInstance(type, new object[] {0});
            if (type == typeof (string)) return string.Empty;
            if (type == typeof (Guid)) return Guid.Empty;
            if (type.IsEnumOrNullable() || type.IsPrimitive || 
                type.IsValueType || type.IsList()) return Activator.CreateInstance(type);
            return null;
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

        public static bool IsBclType(this Type type)
        {
            return type.Namespace == "System" || type.Namespace.StartsWith("System.");
        }

        public static bool IsList(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
        }

        public static Type GetListType(this Type type)
        {
            return type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)).GetGenericArguments()[0];
        }

        public static bool HasCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttribute<T>() != null;
        }
    }
}