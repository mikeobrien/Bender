using System;
using System.Xml;
using System.Xml.Linq;

namespace Bender
{
    public class ValueNode
    {
        public ValueNode(XObject node)
        {
            if (node.NodeType != XmlNodeType.Attribute && node.NodeType != XmlNodeType.Element) 
                throw new ArgumentException("XObject must be an XElement or XAttribute.", "node");
            Object = node;
        }

        public XmlNodeType NodeType { get { return Object.NodeType; } }
        public XObject Object { get; set; }
        public XElement Element { get { return (XElement)Object; } }
        public XAttribute Attribute { get { return (XAttribute)Object; } }

        public XName Name {
            get { return NodeType == XmlNodeType.Attribute ? Attribute.Name : Element.Name; }
            set
            {
                if (NodeType == XmlNodeType.Attribute)
                {
                    var attribute = new XAttribute(value, Attribute.Value);
                    if (Attribute.Parent != null)
                    {
                        var parent = Attribute.Parent;
                        Attribute.Remove();
                        parent.Add(attribute);
                    }
                    Object = attribute;
                } else Element.Name = value;
            } 
        }

        public string Value
        {
            get { return NodeType == XmlNodeType.Attribute ? Attribute.Value : Element.Value; }
            set { if (NodeType == XmlNodeType.Attribute) Attribute.Value = value; else Element.Value = value; }
        }
    }
}