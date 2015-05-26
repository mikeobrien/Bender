using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Collections;
using Bender.Extensions;
using UTF8Encoding = Bender.Extensions.UTF8Encoding;

namespace Bender.Nodes
{
    public class Node : NodeBase
    {
        private readonly Action<INode, Stream, Encoding, bool> _encoder;

        public Node() { }

        public Node(
            string name = null, 
            string format = null, 
            INode parent = null, 
            Metadata metadata = null, 
            Action<INode, Stream, Encoding, bool> encoder = null) : base(parent)
        {
            _name = name;
            IsNamed = name != null;
            _encoder = encoder;
            Format = format;
            if (metadata != null) Metadata.Add(metadata);
        }

        public static Node CreateValue()
        {
            return new Node { NodeType = NodeType.Value };
        }

        public static Node CreateValueFrom(object value)
        {
            return new Node
            {
                NodeType = NodeType.Value, 
                Value = value
            };
        }

        public static Node CreateValue(string name)
        {
            return new Node
            {
                IsNamed = true,
                Name = name,
                NodeType = NodeType.Value
            };
        }

        public static Node CreateValue(string name, object value)
        {
            return new Node
            {
                IsNamed = true, 
                Name = name, 
                NodeType = NodeType.Value, 
                Value = value
            };
        }

        public static Node CreateObject()
        {
            return new Node { NodeType = NodeType.Object };
        }

        public static Node CreateObject(string name)
        {
            return new Node
            {
                IsNamed = true,
                Name = name,
                NodeType = NodeType.Object
            };
        }

        public static Node CreateArray()
        {
            return new Node { NodeType = NodeType.Array };
        }

        public static Node CreateArray(string name)
        {
            return new Node
            {
                IsNamed = true,
                Name = name,
                NodeType = NodeType.Array
            };
        }

        private readonly List<INode> _children = new List<INode>();
        private string _name;
        private object _value;
        private NodeType _nodeType;

        public override bool HasFixedNodeType { get; set; }

        protected override NodeType GetNodeType()
        {
            return _nodeType;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            _nodeType = nodeType;
        }

        protected override string GetName()
        {
            return _name;
        }

        protected override void SetName(string name)
        {
            _name = name;
        }

        protected override object GetValue()
        {
            return _value;
        }

        protected override void SetValue(object value)
        {
            _value = value;
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            var child = new Node(named ? node.Name : null, Format, this, null, _encoder) { NodeType = node.NodeType };
            child.Metadata.Add(node.Metadata);
            modify(child);
            _children.AddItem(child);
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return _children;
        }

        public override string Path { get { return this.Walk<INode>(x => x.Parent).Select(x => x.IsNamed ? x.Name : "*").Aggregate("."); } }

        public override void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            if (_encoder != null) _encoder(this, stream, encoding ?? UTF8Encoding.NoBOM, pretty);
        }
    }
}
