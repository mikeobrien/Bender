using System.Collections;
using System.Collections.Generic;

namespace Tests.Collections.Implementations
{
    public class GenericStringDictionaryImpl : IDictionary<string, string>
    {
        private readonly bool? _readonly;

        private readonly IDictionary<string, string> _dictionary;

        public GenericStringDictionaryImpl()
        {
            _dictionary = new Dictionary<string, string>();
            _readonly = false;
        }

        public GenericStringDictionaryImpl(bool? @readonly)
        {
            _dictionary = new Dictionary<string, string>();
            _readonly = @readonly;
        }

        public GenericStringDictionaryImpl(IDictionary<string, string> dictionary, 
            bool? @readonly = null)
        {
            _dictionary = dictionary;
            _readonly = @readonly;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
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

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, string value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public string this[string key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<string> Values
        {
            get { return _dictionary.Values; }
        }
    }
}
