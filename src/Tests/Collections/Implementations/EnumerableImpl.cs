using System.Collections;

namespace Tests.Collections.Implementations
{
    public class EnumerableImpl : IEnumerable
    {
        private readonly IEnumerable _enumerable;

        public EnumerableImpl()
        {
            _enumerable = new ArrayList();
        }

        public EnumerableImpl(IEnumerable enumerable)
        {
            _enumerable = enumerable;
        }

        public EnumerableImpl(params object[] items)
        {
            _enumerable = items;
        }

        public IEnumerator GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }
    }
}
