using System.Collections;
using System.Collections.Generic;

namespace Tests.Collections.Implementations
{
    public class GenericEnumerableImpl<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public GenericEnumerableImpl()
        {
            _enumerable = new List<T>();
        }

        public GenericEnumerableImpl(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public GenericEnumerableImpl(params T[] items)
        {
            _enumerable = items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
