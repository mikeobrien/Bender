using System;
using System.Linq;
using Bender.Extensions;
using Bender.Collections;

namespace Bender.Reflection
{
    public enum TypeKind { Simple, Dictionary, Enumerable, Complex }

    public static class TypeExtensions
    {
        public static string GetGenericTypeBaseName(this Type type)
        {
            var index = type.Name.IndexOf('`');
            return index > 0 ? type.Name.Remove(index) : type.Name;
        }

        public static string GetGenericTypeBaseFullName(this Type type)
        {
            var index = type.FullName.IndexOf('`');
            return index > 0 ? type.FullName.Remove(index) : type.FullName;
        }

        public static string GetFriendlyTypeFullName(this Type type)
        {
            return (!type.IsGenericType ? type.FullName : 
                (type.IsNullable() ?
                    "{0}?".ToFormat(GetFriendlyTypeFullName(type.GetUnderlyingNullableType())) : 
                    "{0}<{1}>".ToFormat(type.GetGenericTypeBaseFullName(),
                        type.GetGenericArguments().Select(GetFriendlyTypeFullName).Aggregate(", "))))
                .Replace("+", ".");
        }

        public static bool Implements<T>(this Type type)
        {
            return type.Implements(typeof(T));
        }

        public static bool Implements(this Type type, Type @interface)
        {
            return type.GetInterfaces().Any(x => @interface == 
                (x.IsGenericType && @interface.IsGenericType && 
                 @interface.IsGenericTypeDefinition ? x.GetGenericTypeDefinition() : x));
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            return type.IsNullable() ? Type.GetTypeCode(Nullable.GetUnderlyingType(type)) : Type.GetTypeCode(type);
        }

        public static TypeKind GetKind(this CachedType type,
            bool enumerableImplementationsAreComplex,
            bool dictionaryImplementationsAreComplex)
        {
            if (type.IsSimpleType) return TypeKind.Simple;
            if (type.IsDictionary) return !type.IsInBclCollectionNamespace && 
                dictionaryImplementationsAreComplex ? TypeKind.Complex : TypeKind.Dictionary;
            if (type.IsEnumerable) return !type.IsInBclCollectionNamespace && 
                enumerableImplementationsAreComplex ? TypeKind.Complex : TypeKind.Enumerable;
            return TypeKind.Complex;
        }

        public static bool IsSimple(this TypeKind kind)
        {
            return kind == TypeKind.Simple;
        }

        public static bool IsEnumerable(this TypeKind kind)
        {
            return kind == TypeKind.Enumerable;
        }

        public static bool IsDictionary(this TypeKind kind)
        {
            return kind == TypeKind.Dictionary;
        }

        public static bool Is<T>(this Type type)
        {
            return type == typeof(T);
        }

        public static bool Is<T>(this CachedType type)
        {
            return type.Type == typeof(T);
        }

        public static bool Is<T>(this Type type, bool includeNullable) where T : struct
        {
            return type.Is(typeof(T)) || (includeNullable && type.Is(typeof(T?)));
        }

        public static bool Is<T>(this CachedType type, bool includeNullable) where T : struct
        {
            return type.Type.Is<T>(includeNullable);
        }

        public static bool Is(this Type type, Type compare)
        {
            return type == compare || (type.IsGenericType && compare.IsGenericType && 
                compare.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == compare);
        }

        public static bool IsTypeOf(this CachedType type, object value)
        {
            return value != null && type.Type == value.GetType();
        }

        public static bool IsOrImplements<T>(this Type type)
        {
            return type == typeof(T) || (typeof(T).IsInterface && type.Implements<T>());
        }

        public static bool CanBeCastTo<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public static bool CanBeCastTo<T>(this CachedType type)
        {
            return type.Type.CanBeCastTo<T>();
        }

        public static bool CanBeCastTo<T>(this Type type, bool includeNullable) where T : struct
        {
            return (includeNullable ? typeof(T?) : typeof(T)).IsAssignableFrom(type);
        }

        public static object CreateInstance(this Type type, params object[] dependencies)
        {
            return Activator.CreateInstance(type, dependencies);
        }

        // Enums

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum || (type.IsNullable() && Nullable.GetUnderlyingType(type).IsEnum);
        }

        // Constructor

        private static readonly Type[] EmptyTypes = {};

        public static bool HasMatchingConstructor(this Type type, Type[] arguments = null)
        {
            return type.GetConstructor(arguments ?? EmptyTypes) != null;
        }

        public static bool HasMatchingConstructor(this Type type, object[] parameters)
        {
            return type.HasMatchingConstructor(parameters?.Select(x => x.GetType()).ToArray());
        }

        // Simple types

        public static bool IsNumeric(this object value)
        {
            return value is decimal || value is float || value is double || value is sbyte || value is byte ||
                   value is short || value is ushort || value is int || value is uint || value is long || value is ulong;
        }
                  
        public static bool IsNumeric(this Type type)
        {
            return 
                type == typeof(decimal) || type == typeof(decimal?) || 
                type == typeof(float) || type == typeof(float?) || 
                type == typeof(double) || type == typeof(double?) || 
                type == typeof(sbyte) || type == typeof(sbyte?) || 
                type == typeof(byte) || type == typeof(byte?) || 
                type == typeof(short) || type == typeof(short?) || 
                type == typeof(ushort) || type == typeof(ushort?) || 
                type == typeof(int) || type == typeof(int?) || 
                type == typeof(uint) || type == typeof(uint?) || 
                type == typeof(long) || type == typeof(long?) || 
                type == typeof(ulong) || type == typeof(ulong?);
        }

        public static bool IsBoolean(this Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        public static bool IsSimpleType(this Type type)
        {
            Func<Type, bool> isSimpleType = x => x.IsPrimitive || x.IsEnum || x == typeof(string) || x == typeof(decimal) ||
                 x == typeof(DateTime) || x == typeof(TimeSpan) || x == typeof(Guid) || x == typeof(Uri);
            return isSimpleType(type) || (type.IsNullable() && isSimpleType(Nullable.GetUnderlyingType(type)));
        }

        public static object ConvertToEnum(this object value, Type enumType)
        {
            if (value is decimal) value = Convert.ToInt32((decimal) value);
            if (value is float) value = Convert.ToInt32((float)value);
            if (value is double) value = Convert.ToInt32((double)value);
            return Enum.ToObject(enumType, value);
        }

        public static object ParseEnum(this string value, CachedType type, bool ignoreCase)
        {
            return Enum.Parse(type.UnderlyingType.Type, value, ignoreCase);
        }

        public static object ParseSimpleType(this string value, CachedType type)
        {
            if (type.Is<string>() || value == null) return value;
            if (type.IsEnum) return value.ParseEnum(type, true);
            switch (type.TypeCode)
            {
                case TypeCode.Char: 
                    if (value.Length == 1) return value[0];
                    throw new Exception("Char length {0} is invalid.".ToFormat(value.Length));
                case TypeCode.Boolean: return Boolean.Parse(value.ToLower());
                case TypeCode.SByte: return SByte.Parse(value);
                case TypeCode.Byte: return Byte.Parse(value);
                case TypeCode.Int16: return Int16.Parse(value);
                case TypeCode.UInt16: return UInt16.Parse(value);
                case TypeCode.Int32: return Int32.Parse(value);
                case TypeCode.UInt32: return UInt32.Parse(value);
                case TypeCode.Int64: return Int64.Parse(value);
                case TypeCode.UInt64: return UInt64.Parse(value);
                case TypeCode.Single: return Single.Parse(value);
                case TypeCode.Double: return Double.Parse(value);
                case TypeCode.Decimal: return Decimal.Parse(value);
                case TypeCode.DateTime: return DateTime.Parse(value);
                default:
                    if (type.Is<Guid>(true)) return Guid.Parse(value);
                    if (type.Is<TimeSpan>(true)) return TimeSpan.Parse(value);
                    if (type.Is<Uri>()) return new Uri(value);
                    if (type.Is<IntPtr>(true)) return new IntPtr(Int64.Parse(value));
                    if (type.Is<UIntPtr>(true)) return new UIntPtr(UInt64.Parse(value));
                    throw new ArgumentException("Type '{0}' is not a simple type.".ToFormat(type));
            }
        }

        // Nullable types

        public static bool IsOptional(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public static Type GetUnderlyingOptionalType(this Type type)
        {
            return type.IsOptional() ? type.GetGenericArguments().First() : type;
        }

        public static Type GetUnderlyingNullableType(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static T NullOrDefault<T>()
        {
            return typeof(T) == typeof(string) ? (T)(object)null : default(T);
        }
    }
}
