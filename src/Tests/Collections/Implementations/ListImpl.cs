using System;
using System.Collections;

namespace Tests.Collections.Implementations
{
    public class ListImpl : IList
    {
        private readonly IList _list;

        public ListImpl()
        {
            _list = new ArrayList();
        }

        public ListImpl(IList list)
        {
            _list = list;
        }

        public int Count => _list.Count;
        public object SyncRoot => _list.SyncRoot;
        public bool IsSynchronized => _list.IsSynchronized;
        public bool IsReadOnly => _list.IsReadOnly;
        public bool IsFixedSize => _list.IsFixedSize;

        public int Add(object value)
        {
            return _list.Add(value);
        }

        public bool Contains(object value)
        {
            return _list.Contains(value);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int IndexOf(object value)
        {
            return _list.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
        }

        public void Remove(object value)
        {
            _list.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
