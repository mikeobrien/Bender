﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class ParseException : Exception { public ParseException(Type type) : base("Unable to parse " + type) { } }

    public static class Extensions
    {
        public static IEnumerable<T> Traverse<T>(this T source, Func<T, T> result) where T : class
        {
            var node = source;
            while (node != null)
            {
                yield return node;
                node = result(node);
            }
        }

        public static string GetXmlName(this PropertyInfo property)
        {
            var elementName = property.GetCustomAttribute<XmlElementAttribute>();
            return elementName != null && elementName.ElementName != null ? elementName.ElementName : property.Name;
        }

        public static string GetXmlName(this Type type, string listNameFormat, string typeNameFormat)
        {
            var xmlType = type.GetCustomAttribute<XmlTypeAttribute>();
            if (xmlType != null && xmlType.TypeName != null) return xmlType.TypeName;
            if (type.IsGenericType)
            {
                var typeDefinition = type.GetGenericTypeDefinition();
                var typeArguments = type.GetGenericArguments().Select(x => GetXmlName(x, listNameFormat, typeNameFormat)).Aggregate((a, i) => a + i);
                return typeDefinition == typeof(List<>) ?
                    string.Format(listNameFormat, typeArguments) :
                    string.Format(typeNameFormat, typeDefinition.Name.Remove(typeDefinition.Name.IndexOf('`')), typeArguments);
            }
            return type.Name;
        }

        public static PropertyInfo[] GetSerializableProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
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
            return type == typeof (T) || (type.IsNullable() && Nullable.GetUnderlyingType(type) == typeof (T));
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        public static TypeCode GetTypeCode(this Type type, bool includeNullable)
        {
            return includeNullable && type.IsNullable() ? Type.GetTypeCode(Nullable.GetUnderlyingType(type)) : Type.GetTypeCode(type);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsList(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IList<>));
        }

        public static Type GetListType(this Type type)
        {
            return type.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)).GetGenericArguments()[0];
        }

        public static bool HasCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttribute<T>() != null;
        }

        public static string GetXPath(this XElement element)
        {
            return (element.Ancestors().Any() ? "/" + element.Ancestors().Select(x => x.Name.LocalName)
                            .Aggregate((a, i) => a + "/" + i) : "") + "/" + element.Name.LocalName;
        }

        public static object Read(this Dictionary<Type, Func<Options, PropertyInfo, string, object>> readers,
            Type type, Options options, PropertyInfo property, string value)
        {
            return readers[type](options, property, value);
        }

        public static string Write(this Dictionary<Type, Func<Options, PropertyInfo, object, string>> writers,
            Type type, Options options, PropertyInfo property, object value)
        {
            return writers[type](options, property, value);
        }

        public static T Read<T>(this Dictionary<Type, Func<Options, PropertyInfo, string, object>> readers, 
            Options options, PropertyInfo property, string value)
        {
            return (T)readers.Read(typeof(T), options, property, value);
        }

        public static string Write<T>(this Dictionary<Type, Func<Options, PropertyInfo, object, string>> writers, 
            Options options, PropertyInfo property, object value)
        {
            return writers.Write(typeof(T), options, property, value);
        }
    }
}