using System.Collections;
using System.Collections.Generic;

namespace Tests.Collections.Implementations
{
    public class GenericDictionaryImpl<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly bool? _readonly;

        private readonly IDictionary<TKey, TValue> _dictionary;

        public GenericDictionaryImpl()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _readonly = false;
        }

        public GenericDictionaryImpl(bool? @readonly)
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _readonly = @readonly;
        }

        public GenericDictionaryImpl(IDictionary<TKey, TValue> dictionary, 
            bool? @readonly = null)
        {
            _dictionary = dictionary;
            _readonly = @readonly;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Remove(item);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _readonly.HasValue ? _readonly.Value : _dictionary.IsReadOnly; }
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }
    }
}
