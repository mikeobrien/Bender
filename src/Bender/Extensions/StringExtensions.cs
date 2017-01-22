using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bender.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this object value)
        {
            return value == null || value.ToString().IsEmpty();
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsEmpty(this string value)
        {
            return value == "";
        }

        public static bool IgnoreCase(this StringComparison comparison)
        {
            return comparison == StringComparison.CurrentCultureIgnoreCase ||
                   comparison == StringComparison.InvariantCultureIgnoreCase ||
                   comparison == StringComparison.OrdinalIgnoreCase;
        }

        public static bool StartsWithIgnoreCase(this string source, string compare)
        {
            return source.StartsWith(compare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string source, string compare)
        {
            return source.Equals(compare, StringComparison.OrdinalIgnoreCase);
        }

        public static string Truncate(this object value, int length)
        {
            if (value == null) return null;
            var stringValue = value.ToString();
            return stringValue.Length > length ? stringValue.Substring(0, length) + "..." : stringValue;
        }

        public static Stream ToStream(this string value, Encoding encoding = null)
        {
            return new MemoryStream((encoding ?? UTF8Encoding.NoBOM).GetBytes(value));
        }

        public static void WriteToStream(this string value, Stream stream, Encoding encoding = null)
        {
            var writer = new StreamWriter(stream, encoding ?? UTF8Encoding.NoBOM);
            writer.Write(value);
            writer.Flush();
        }

        public static string ToFormat(this string value, params object[] args)
        {
            return args.Any() ? String.Format(value, args) : value;
        }

        public static string RemoveNewLines(this string value)
        {
            return value.Replace(Environment.NewLine, " ");
        }

        public static string ToInitialCaps(this string value)
        {
            return !value.IsNullOrEmpty() ? 
                value.Substring(0, 1).ToUpper() + 
                value.Substring(1) : value;
        }

        public static string ToCamelCase(this string value)
        {
            if (String.IsNullOrEmpty(value) || 
                !Char.IsUpper(value[0])) return value;

            var chars = value.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !Char.IsUpper(chars[i])) break;

                var hasNext = i + 1 < chars.Length;
                if (i > 0 && hasNext && !Char.IsUpper(chars[i + 1])) break;

                chars[i] = Char.ToLowerInvariant(chars[i]);
            }

            return new string(chars);
        }

        public static string ToSeparatedCase(this string value, bool lower, string seperator)
        {
            if (String.IsNullOrEmpty(value)) return value;
            var result = value[0].ToString() + Regex.Replace(value.Substring(1), 
                "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", seperator + "$1");
            return lower ? result.ToLower() : result;
        }

        public static IComparer ToComparer(this StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;
                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
                default: throw new ArgumentException("Not a valid string comparison type.");
            }
        }

        private static readonly string[] ArticleVowels = { "a", "e", "i", "o" };

        public static string GetArticle(this object value)
        {
            return !value.IsNullOrEmpty() && 
                ArticleVowels.Contains(value.ToString().ToLower().Substring(0, 1)) ? "an" : "a";
        }

        public static string ToLower(this object value)
        {
            return value.ToString().ToLower();
        }
    }
}
