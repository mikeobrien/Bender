using System.Collections.Generic;
using System.Linq;

namespace Bender
{
    public class LinkedNode<T>
    {
        public LinkedNode(T value) : this(null, value) { }

        public LinkedNode(LinkedNode<T> node, T value)
        {
            Next = node;
            Value = value;
        }

        public LinkedNode<T> Next { get; private set; } 
        public T Value { get; private set; }

        public static LinkedNode<T> Create(T value)
        {
            return new LinkedNode<T>(value);
        } 

        public LinkedNode<T> Add(T value)
        {
            return new LinkedNode<T>(this, value);
        }

        public bool Any()
        {
            return Next != null;
        }

        public bool Any(T value)
        {
            return AsEnumerable().Any(x => x.Equals(value));
        }

        public IEnumerable<T> AsEnumerable()
        {
            var node = this;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }
    }
}
