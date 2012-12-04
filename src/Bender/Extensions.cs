using System;
using System.Collections.Generic;

namespace Bender
{
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
    }
}