using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bender.Nodes
{
    public enum NodeType { Object, Array, Value, Variable }

    public static class NodeTypeExtensions
    {
        public static bool IsValue(this NodeType type) { return type == NodeType.Value; }
        public static bool IsObject(this NodeType type) { return type == NodeType.Object; }
        public static bool IsArray(this NodeType type) { return type == NodeType.Array; }
    }

    public interface INode : IEnumerable<INode>
    {
        string Name { get; }
        object Value { get; set; }
        NodeType NodeType { get; set; }
        bool HasFixedNodeType { get; }
        string Path { get; }
        string Format { get; }
        string Type { get; }
        bool IsNamed { get; }
        Metadata Metadata { get; }
        INode Parent { get; }
        bool HasParent { get; }

        void Initialize();

        void Add(INode node, Action<INode> modify);

        void Validate();

        void Encode(Stream stream, Encoding encoding = null, bool pretty = false);
    }
}