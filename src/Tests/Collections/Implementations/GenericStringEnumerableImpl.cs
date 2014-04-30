using System.Collections;
using System.Collections.Generic;

namespace Tests.Collections.Implementations
{
    public class GenericStringEnumerableImpl : IEnumerable<string>
    {
        private readonly IEnumerable<string> _enumerable;

        public GenericStringEnumerableImpl()
        {
            _enumerable = new List<string>();
        }

        public GenericStringEnumerableImpl(IEnumerable<string> enumerable)
        {
            _enumerable = enumerable;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
