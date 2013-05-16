using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bender
{
    public static class Exceptions
    {
        public static TResult Wrap<T, TResult>(Func<TResult> action, Func<T, Exception> wrapper) where T : Exception
        {
            try { return action(); } catch (T exception) { throw wrapper(exception); }
        }

        public static object Wrap<T>(Func<object> action, Func<T, Exception> wrapper) where T : Exception
        {
            try { return action(); } catch (T exception) { throw wrapper(exception); }
        }

        public static TResult Wrap<TResult>(Func<TResult> action, Func<Exception, Exception> wrapper)
        {
            try { return action(); } catch (Exception exception) { throw wrapper(exception); }
        }
    }

    public static class Extensions
    {
        public static Dictionary<string, TElement> ToDictionary<TSource, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, string> keySelector,
            Func<TSource, TElement> elementSelector,
            bool caseInsensitiveKey)
        {
            var result = source.ToDictionary(keySelector, elementSelector);
            return caseInsensitiveKey ? new Dictionary<string, TElement>(result, StringComparer.OrdinalIgnoreCase) : result;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static void ForEach(this IEnumerable source, Action<object> action)
        {
            foreach (var item in source) action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        public static IEnumerable AsEnumerable(this object source)
        {
            return (IEnumerable) source;
        }

        public static IEnumerable Select(this IEnumerable source, Func<object, object> map)
        {
            return source.Cast<object>().Select<object, object>(map);
        }

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> source1, bool shouldConcat, IEnumerable<T> source2)
        {
            return shouldConcat ? source1.Concat(source2) : source1;
        }

        public static Array ToArray(this IList source, Type type)
        {
            var array = Array.CreateInstance(type, source.Count);
            source.CopyTo(array, 0);
            return array;
        }

        public static string TruncateAt(this string value, int length)
        {
            return !string.IsNullOrEmpty(value) && length > 0 && value.Length > length ? value.Substring(0, length) + "..." : value;
        }

        public static string ToFormat(this string value, params object[] args)
        {
            return string.Format(value, args);
        }
    }
}