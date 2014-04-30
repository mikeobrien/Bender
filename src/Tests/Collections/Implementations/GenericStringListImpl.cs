using System.Collections;
using System.Collections.Generic;

namespace Tests.Collections.Implementations
{
    public class GenericStringListImpl : IList<string>
    {
        private readonly IList<string> _list;

        public GenericStringListImpl()
        {
            _list = new List<string>();
        }

        public GenericStringListImpl(IList<string> list)
        {
            _list = list;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(string item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _list.Remove(item);
        }

        public int Count { get { return _list.Count; } }
        public bool IsReadOnly { get { return _list.IsReadOnly; } }

        public int IndexOf(string item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public string this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }
}
