using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Collections;
using Bender.Extensions;
using Flexo;

namespace Bender.Nodes
{
    public class JsonNode : NodeBase
    {
        public const string NodeFormat = "json";

        public JsonNode(NodeType type)
        {
            Element = JElement.Create(type == NodeType.Array ? ElementType.Array : ElementType.Object);
        }

        public JsonNode(string json) { Element = Exception<JsonParseException>
            .Map(() => JElement.Load(json), x => new ParseException(x, NodeFormat)); }

        public JsonNode(byte[] bytes, Encoding encoding = null) { Element = Exception<JsonParseException>
            .Map(() => JElement.Load(bytes, encoding), x => new ParseException(x, NodeFormat)); }

        public JsonNode(Stream stream, Encoding encoding = null) { Element = Exception<JsonParseException>
            .Map(() => JElement.Load(stream, encoding), x => new ParseException(x, NodeFormat)); }

        private JsonNode(JElement element, JsonNode parent) : base(parent)
        {
            Element = element;
        }

        public JElement Element { get; private set; }
        public override string Format { get { return NodeFormat; } }
        public override string Path { get { return Element.Path; } }
        public override bool IsNamed { get { return Element.IsNamed; } }

        public override string Type
        {
            get
            {
                if (Element.HasParent && Element.Parent.IsObject) return "field";
                switch (Element.Type)
                {
                    case ElementType.Array: return "array";
                    case ElementType.Object: return "object";
                    default : return "value";
                }
            }
        }

        protected override NodeType GetNodeType()
        {
            switch (Element.Type)
            {
                case ElementType.Object: return NodeType.Object;
                case ElementType.Array: return NodeType.Array;
                default: return NodeType.Value;
            }
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            Element.Type = GetElementType(nodeType);
        }

        protected override string GetName()
        {
            return Element.Name;
        }

        protected override void SetName(string name)
        {
            Element.Name = name;
        }

        protected override object GetValue()
        {
            return Element.Value;
        }

        protected override void SetValue(object value)
        {
            Element.Value = value;
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is JsonNode) Element.Insert(((JsonNode)node).Element);
            else new JsonNode(named && !NodeType.IsArray() ? 
                Element.AddMember(node.Name, GetElementType(node.NodeType)) : 
                Element.AddArrayElement(GetElementType(node.NodeType)), this).Configure(modify);
        }

        private static ElementType GetElementType(NodeType type)
        {
            switch (type)
            {
                case NodeType.Array: return ElementType.Array;
                case NodeType.Object: return ElementType.Object;
                default: return ElementType.Null;
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return Element.Select(x => new JsonNode(x, this));
        }

        public override void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            Element.Encode(stream, encoding, pretty);
        }
    }
}
