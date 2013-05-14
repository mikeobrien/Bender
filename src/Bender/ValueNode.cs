using System;
using System.Xml;
using System.Xml.Linq;

namespace Bender
{
    public enum NodeType { XmlElement, XmlAttribute, JsonField }

    public class ValueNode
    {
        public ValueNode(XObject node, Format format)
        {
            if (node.NodeType != XmlNodeType.Attribute && node.NodeType != XmlNodeType.Element) 
                throw new ArgumentException("XObject must be an XElement or XAttribute.", "node");
            Object = node;
            Format = format;
            NodeType = format == Format.Json ? NodeType.JsonField : 
                (node.NodeType == XmlNodeType.Attribute ? NodeType.XmlAttribute : NodeType.XmlElement);
        }

        public NodeType NodeType { get; private set; }
        public XObject Object { get; set; }
        public XElement Element { get { return (XElement)Object; } }
        public XAttribute Attribute { get { return (XAttribute)Object; } }
        public Format Format { get; private set; }

        public XName Name {
            get { return Object.NodeType == XmlNodeType.Attribute ? Attribute.Name : Element.Name; }
            set
            {
                if (Object.NodeType == XmlNodeType.Attribute)
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
            get { return Object.NodeType == XmlNodeType.Attribute ? Attribute.Value : Element.Value; }
            set { if (Object.NodeType == XmlNodeType.Attribute) Attribute.Value = value; else Element.Value = value; }
        }
    }
}