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

        public ICollection Keys { get { return _dictionary.Keys; } }
        public ICollection Values { get { return _dictionary.Values; } }
        public bool IsReadOnly { get { return _dictionary.IsReadOnly; } }
        public bool IsFixedSize { get { return _dictionary.IsFixedSize; } }
        public int Count { get { return _dictionary.Count; } }
        public object SyncRoot { get { return _dictionary.SyncRoot; } }
        public bool IsSynchronized { get { return _dictionary.IsSynchronized; } }

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
