using System;
using System.Collections;
using System.Linq;
using Bender.Extensions;
using Bender.Reflection;

namespace Bender.Collections
{
    public class GenericDictionaryAdapter : IDictionary
    {
        private readonly object _dictionary;
        private readonly CachedType _type;

        private GenericDictionaryAdapter(object dictionary, CachedType type)
        {
            _dictionary = dictionary;
            _type = type;
        }

        public static IDictionary Create(object dictionary)
        {
            var type = dictionary.GetCachedType();
            if (dictionary is GenericDictionaryAdapter ||
                type.IsNonGenericDictionary) return (IDictionary)dictionary;
            if (!type.IsGenericDictionary)
                throw new ArgumentException("Generic dictionary adapter source '{0}' is not a generic dictionary."
                    .ToFormat(type.FriendlyFullName));
            return new GenericDictionaryAdapter(dictionary, type);
        }

        public ICollection Keys { get { return (ICollection)_type.GetMember("Keys").GetValue(_dictionary); } }
        public ICollection Values { get { return (ICollection)_type.GetMember("Values").GetValue(_dictionary); } }

        public bool IsReadOnly { get { return (bool)_type.GetMember("IsReadOnly").GetValue(_dictionary); } }
        public bool IsFixedSize { get { return false; } }

        public int Count { get { return (int)_type.GetMember("Count").GetValue(_dictionary); } }
        public object SyncRoot { get { return _dictionary; } }
        public bool IsSynchronized { get { return false; } }

        public object this[object key]
        {
            get { return _type.GetIndexer().GetValue(_dictionary, key); }
            set { _type.GetIndexer().SetValue(_dictionary, key, value); }
        }

        public bool Contains(object key)
        {
            return _type.InvokeFunc<bool>("ContainsKey", _dictionary, key);
        }

        public void Add(object key, object value)
        {
            _type.InvokeAction("Add", _dictionary, key, value);
        }

        public void Remove(object key)
        {
            _type.InvokeAction("Remove", _dictionary, key);
        }

        public void Clear()
        {
            _type.InvokeAction("Clear", _dictionary);
        }

        public void CopyTo(Array array, int index)
        {
            var source = this.Cast<object>().ToArray();
            Array.Copy(source, 0, array, index, source.Length);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new GenericDictionaryAdapterEnumerator(_dictionary);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class GenericDictionaryAdapterEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator _enumerator;

            public GenericDictionaryAdapterEnumerator(object dictionary)
            {
                _enumerator = ((IEnumerable)dictionary).GetEnumerator();
            }

            public Object Current
            {
                get
                {
                    var type = _enumerator.Current.GetCachedType();
                    return new DictionaryEntry(
                        type.GetMember("Key").GetValue(_enumerator.Current),
                        type.GetMember("Value").GetValue(_enumerator.Current));
                }
            }

            public DictionaryEntry Entry { get { return (DictionaryEntry)Current; } }
            public Object Key { get { return Entry.Key; } }
            public Object Value { get { return Entry.Value; } }
            public Boolean MoveNext() { return _enumerator.MoveNext(); }
            public void Reset() { _enumerator.Reset(); }
        }
    }
}
