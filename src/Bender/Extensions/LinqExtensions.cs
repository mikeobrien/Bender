using System.Collections.Generic;
using System.Linq;

namespace Bender.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<int> To(this int start, int end)
        {
            return Enumerable.Range(start, end);
        }
    }
}
