using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bender.Nodes
{
    public class NodeTypeReadonlyException : BenderException
    {
        public NodeTypeReadonlyException() : 
            base("Node type cannot be changed.") { }
    }

    public class ValueNotSupportedException : BenderException
    {
        public ValueNotSupportedException() : 
            base("Values not supported on object nodes.") { }
    }

    public class ChildrenNotSupportedException : BenderException
    {
        public ChildrenNotSupportedException() : 
            base("Children not supported on this node.") { }
    }

    public class NameNotSupportedException : BenderException
    {
        public NameNotSupportedException() : 
            base("Names not supported on this node.") { }
    }

    public class EncodingNotSupportedException : BenderException
    {
        public EncodingNotSupportedException() :
            base("Encoding is not supported on this node.") { }
    }

    public abstract class NodeBase : INode
    {
        private readonly Metadata _metadata = new Metadata();

        protected NodeBase(INode parent = null)
        {
            Parent = parent;
            HasParent = parent != null;
        }

        public virtual string Format { get; set; }
        public virtual string Type { get; set; }
        public virtual string Path { get; set; }
        public virtual bool IsNamed { get; set; }
        public virtual Metadata Metadata { get { return _metadata; } }
        public virtual INode Parent { get; private set; }
        public virtual bool HasParent { get; private set; }

        public virtual bool HasFixedNodeType { get; set; }

        public NodeType NodeType
        {
            get
            {
                return GetNodeType();
            }
            set
            {
                if (HasFixedNodeType) throw new NodeTypeReadonlyException();
                SetNodeType(value);
            }
        }

        protected abstract NodeType GetNodeType();

        protected virtual void SetNodeType(NodeType nodeType)
        {
            throw new NodeTypeReadonlyException();
        }

        public string Name
        {
            get
            {
                if (!IsNamed) throw new NameNotSupportedException();
                return GetName();
            }
            set
            {
                if (!IsNamed) throw new NameNotSupportedException();
                SetName(value);
            }
        }

        protected virtual string GetName()
        {
            throw new NameNotSupportedException();
        }

        protected virtual void SetName(string name)
        {
            throw new NameNotSupportedException();
        }

        public virtual object Value
        {
            get
            {
                if (!this.IsValueOrVariable()) throw new ValueNotSupportedException();
                return GetValue();
            }
            set
            {
                if (!this.IsValueOrVariable()) throw new ValueNotSupportedException();
                SetValue(value);
            }
        }

        protected virtual object GetValue()
        {
            throw new ValueNotSupportedException();
        }

        protected virtual void SetValue(object value)
        {
            throw new ValueNotSupportedException();
        }

        public virtual void Initialize() { }
        public virtual void Validate() { }

        public virtual void Add(INode node, Action<INode> modify)
        {
            if (NodeType.IsValue()) throw new ChildrenNotSupportedException();
            var named = node.IsNamed && !(node.HasParent && node.Parent.IsObject() && NodeType.IsArray());
            if (!named && NodeType.IsObject()) throw new UnnamedChildrenNotSupportedException();
            AddNode(node, named, modify);
        }

        protected virtual void AddNode(INode node, bool named, Action<INode> modify)
        {
            throw new ChildrenNotSupportedException();
        }

        public IEnumerator<INode> GetEnumerator()
        {
            if (NodeType.IsValue()) throw new ChildrenNotSupportedException();
            return GetNodes().GetEnumerator();
        }

        protected virtual IEnumerable<INode> GetNodes()
        {
            throw new ChildrenNotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            throw new EncodingNotSupportedException();
        }
    }
}
