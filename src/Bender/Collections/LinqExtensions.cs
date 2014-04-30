using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Bender.Reflection;

namespace Bender.Collections
{
    public static class LinqExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.Keys.Cast<string>().ToDictionary(x => x, x => source[x]);
        }

        public static T PipeWhen<T>(this T value, bool predicate) where T : class
        {
            return value.PipeWhen(predicate, null);
        }

        public static T PipeWhen<T>(this T value, bool predicate, T @default)
        {
            return predicate ? value : @default;
        }

        public static T PipeWhen<T>(this T value, Func<T, bool> predicate) where T : class
        {
            return value.PipeWhen(predicate, null);
        }

        public static T PipeWhen<T>(this T value, Func<T, bool> predicate, T @default)
        {
            return value != null && predicate(value) ? value : @default;
        }

        public static T PipeWhen<T>(this T value, Func<T, T> func, bool predicate)
        {
            return predicate ? func(value) : value;
        }

        public static T PipeWhen<T>(this T value, Func<T, T> func, Func<T, bool> predicate)
        {
            return predicate(value) ? func(value) : value;
        }

        public static void PipeWhen<T>(this T value, Action<T> func, bool predicate)
        {
            if (predicate) func(value);
        }

        public static void PipeWhen<T>(this T value, Action<T> func, Func<T, bool> predicate)
        {
            if (predicate(value)) func(value);
        }

        public static TValue Map<T, TValue>(this T source, Func<T, TValue> map)
        {
            return map(source);
        }

        public static TValue MapOrDefault<T, TValue>(this T source, Func<T, TValue> map, TValue @default) where T : class
        {
            return source != null ? source.Map(map) : @default;
        }

        public static TValue MapOrDefault<T, TValue>(this T source, Func<T, TValue> value) where T : class
        {
            return source.MapOrDefault(value, TypeExtensions.NullOrDefault<TValue>());
        }

        public static string Aggregate<T>(this IEnumerable<T> source, string seperator)
        {
            return source.Select(x => x.ToString()).Aggregate((a, i) => a + seperator + i);
        }

        public static string Aggregate<T>(this IEnumerable<T> source, Func<T, string> select, string seperator = "")
        {
            return source.Select(select).Aggregate((a, i) => a + seperator + i);
        }

        public static string Aggregate<T>(this IEnumerable<T> source, string start, string seperator, string end)
        {
            return source.Any() ? start + source.Select(x => x.ToString()).Aggregate((a, i) => a + seperator + i) + end : "";
        }

        public static IList<T> Add<T>(this IList<T> list, T item, bool predicate)
        {
            if (predicate) list.Add(item);
            return list;
        }

        public static T AddItem<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> source, IEnumerable<T> compare,
            Func<T, T, bool> predicate)
        {
            return source.Where(s => !compare.Any(c => predicate(s, c)));
        }

        public static IEnumerable<T> Walk<T>(this T source, Func<T, T> map) where T : class
        {
            var current = source;
            while (current != null)
            {
                yield return current;
                current = map(current);
            }
        }

        public static string Aggregate(this IEnumerable<string> source)
        {
            return source.Aggregate((a, i) => a + i);
        }

        public static IEnumerable<DictionaryEntry> AsEnumerable(this IDictionary source)
        {
            foreach (var item in source) { yield return (DictionaryEntry)item; }
        }

        public static T As<T>(this object source)
        {
            return source == null ? default(T) : (T) source;
        }

        public static IEnumerable<TSource> Where<TSource, TMap>(this IEnumerable<TSource> source,
            Func<TSource, TMap> map, Func<TMap, bool> predicate)
        {
            return predicate != null ? source.Where(x => predicate(map(x))) : source;
        } 

        public static IEnumerable<TSource> Where<TSource, TMap1, TMap2>(this IEnumerable<TSource> source,
            Func<TSource, TMap1> map1, Func<TSource, TMap2> map2, Func<TMap1, TMap2, bool> predicate)
        {
            return predicate != null ? source.Where(x => predicate(map1(x), map2(x))) : source;
        } 
    }
}
