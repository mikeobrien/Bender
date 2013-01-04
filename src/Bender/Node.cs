using System;
using System.Xml.Linq;

namespace Bender
{
    public enum NodeType { Element, Attribute }

    public class Node
    {
        public Node(XObject node)
        {
            Object = node;
            if (node is XElement)
            {
                Element = (XElement) node;
                NodeType = NodeType.Element;
            }
            else if (node is XAttribute)
            {
                Attribute = (XAttribute) node;
                NodeType = NodeType.Attribute;
            }
            else throw new ArgumentException("XObject must be an XElement or XAttribute.", "node");
        }

        public static Node Create(string name, NodeType type)
        {
            return new Node(type == NodeType.Element ? (XObject)new XElement(name) : (XObject)new XAttribute(name, ""));
        }

        public NodeType NodeType { get; set; }
        public XObject Object { get; set; }
        public XElement Element { get; set; }
        public XAttribute Attribute { get; set; }

        public XName Name { get { return NodeType == NodeType.Attribute ? Attribute.Name : Element.Name; } }

        public string Value
        {
            get { return NodeType == NodeType.Attribute ? Attribute.Value : Element.Value; }
            set { if (NodeType == NodeType.Attribute) Attribute.Value = value; else Element.Value = value; }
        }
    }
}