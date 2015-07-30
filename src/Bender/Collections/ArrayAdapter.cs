using System;
using System.Collections;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using Bender.Extensions;

namespace Bender.Collections
{
    public class ArrayAdapter : IList
    {
        private readonly IValue _array;
        private readonly IList _list;
        private readonly CachedType _listType;

        public ArrayAdapter(IValue array)
        {
            _array = array;
            _list = array.ActualType.CreateGenericListInstance().As<IList>();
            _listType = _list.GetType().ToCachedType();
            _listType.InvokeAction("AddRange", _list, array.Instance);
        }

        public static IList Create(IValue array)
        {
            if (array.Instance is ArrayAdapter || 
                (array.Instance is IList && !array.ActualType.IsArray))
                return (IList)array.Instance;
            if (!array.ActualType.IsArray)
                throw new ArgumentException("Array adapter source '{0}' is not an array."
                    .ToFormat(array.ActualType.FriendlyFullName));
            return new ArrayAdapter(array);
        }

        private void Sync()
        {
            _array.Instance = _listType.InvokeFunc<Array>("ToArray", _list);
        }

        public object Source { get { return _array; } }

        public bool IsReadOnly { get { return false; } }
        public bool IsFixedSize { get { return false; } }

        public int Count { get { return _list.Count; } }
        public object SyncRoot { get { return _array.Instance; } }
        public bool IsSynchronized { get { return false; } }

        public object this[int key]
        {
            get { return _list[key]; }
            set { _list[key] = value; }
        }

        public bool Contains(object value)
        {
            return _list.Contains(value);
        }

        public int IndexOf(object value)
        {
            return _list.IndexOf(value);
        }

        public int Add(object value)
        {
            var index = _list.Add(value);
            Sync();
            return index;
        }

        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
            Sync();
        }

        public void Remove(object value)
        {
            _list.Remove(value);
            Sync();
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            Sync();
        }

        public void Clear()
        {
            _list.Clear();
            Sync();
        }

        public void CopyTo(Array array, int index)
        {
            var source = _array.Instance.As<Array>();
            Array.Copy(source, 0, array, index, source.Length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
