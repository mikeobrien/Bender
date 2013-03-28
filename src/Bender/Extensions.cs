﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bender
{
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

        public static IEnumerable AsEnumerable(this object source)
        {
            return (IEnumerable) source;
        }

        public static IEnumerable Select(this IEnumerable source, Func<object, object> map)
        {
            return source.Cast<object>().Select<object, object>(map);
        }
    }
}