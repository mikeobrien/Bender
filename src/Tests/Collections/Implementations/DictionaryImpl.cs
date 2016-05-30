using System;
using System.Collections;

namespace Tests.Collections.Implementations
{
    public class DictionaryImpl : IDictionary
    {
        private readonly IDictionary _dictionary;

        public DictionaryImpl()
        {
            _dictionary = new Hashtable();
        }

        public DictionaryImpl(IDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        public ICollection Keys => _dictionary.Keys;
        public ICollection Values => _dictionary.Values;
        public bool IsReadOnly => _dictionary.IsReadOnly;
        public bool IsFixedSize => _dictionary.IsFixedSize;
        public int Count => _dictionary.Count;
        public object SyncRoot => _dictionary.SyncRoot;
        public bool IsSynchronized => _dictionary.IsSynchronized;

        public bool Contains(object key)
        {
            return _dictionary.Contains(key);
        }

        public void Add(object key, object value)
        {
            _dictionary.Add(key, value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Remove(object key)
        {
            _dictionary.Remove(key);
        }

        public object this[object key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public void CopyTo(Array array, int index)
        {
            _dictionary.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
