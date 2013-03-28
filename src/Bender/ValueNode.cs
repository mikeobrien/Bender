using System;
using System.Xml.Linq;

namespace Bender
{
    public enum ValueNodeType { Element, Attribute }

    public class ValueNode
    {
        public ValueNode(XObject node)
        {
            Object = node;
            if (node is XElement)
            {
                Element = (XElement) node;
                NodeType = ValueNodeType.Element;
            }
            else if (node is XAttribute)
            {
                Attribute = (XAttribute) node;
                NodeType = ValueNodeType.Attribute;
            }
            else throw new ArgumentException("XObject must be an XElement or XAttribute.", "node");
        }

        public ValueNodeType NodeType { get; set; }
        public XObject Object { get; set; }
        public XElement Element { get; set; }
        public XAttribute Attribute { get; set; }

        public XName Name { get { return NodeType == ValueNodeType.Attribute ? Attribute.Name : Element.Name; } }

        public string Value
        {
            get { return NodeType == ValueNodeType.Attribute ? Attribute.Value : Element.Value; }
            set { if (NodeType == ValueNodeType.Attribute) Attribute.Value = value; else Element.Value = value; }
        }
    }
}