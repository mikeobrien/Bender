using System;
using System.Collections;
using System.Linq;
using Bender.Extensions;
using Bender.Reflection;

namespace Bender.Collections
{
    public class GenericListAdapter : IList
    {
        private readonly object _list;
        private readonly CachedType _type;

        private GenericListAdapter(object list, CachedType type)
        {
            _list = list;
            _type = type;
        }

        public static IList Create(object list)
        {
            var type = list.ToCachedType();
            if (list is GenericListAdapter || type.IsNonGenericList) return (IList)list;
            if (!type.IsGenericList)
                throw new ArgumentException("Generic list adapter source '{0}' is not a generic list."
                    .ToFormat(type.FriendlyFullName));
            return new GenericListAdapter(list, type);
        }

        public bool IsReadOnly { get { return (bool)_type.GetMember("IsReadOnly").GetValue(_list); } }
        public bool IsFixedSize { get { return false; } }

        public int Count { get { return (int)_type.GetMember("Count").GetValue(_list); } }
        public object SyncRoot { get { return _list; } }
        public bool IsSynchronized { get { return false; } }

        public object this[int key]
        {
            get { return _type.GetIndexer().GetValue(_list, key); }
            set { _type.GetIndexer().SetValue(_list, key, value); }
        }

        public bool Contains(object value)
        {
            return _type.InvokeFunc<bool>("Contains", _list, value);
        }

        public int IndexOf(object value)
        {
            return _type.InvokeFunc<int>("IndexOf", _list, value);
        }

        public int Add(object value)
        {
            _type.InvokeAction("Add", _list, value);
            return _type.InvokeFunc<int>("IndexOf", _list, value);
        }

        public void Insert(int index, object value)
        {
            _type.InvokeAction("Insert", _list, index, value);
        }

        public void Remove(object value)
        {
            _type.InvokeAction("Remove", _list, value);
        }

        public void RemoveAt(int index)
        {
            _type.InvokeAction("RemoveAt", _list, index);
        }

        public void Clear()
        {
            _type.InvokeAction("Clear", _list);
        }

        public void CopyTo(Array array, int index)
        {
            var source = this.Cast<object>().ToArray();
            Array.Copy(source, 0, array, index, source.Length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _type.InvokeFunc<IEnumerator>("GetEnumerator", _list);
        }
    }
}
