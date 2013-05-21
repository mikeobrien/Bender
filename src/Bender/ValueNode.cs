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
                throw new ArgumentException("Node must be an XElement or XAttribute.", "node");
            Object = node;
            Format = format;
            NodeType = format == Format.Json ? NodeType.JsonField : 
                (node.NodeType == XmlNodeType.Attribute ? NodeType.XmlAttribute : NodeType.XmlElement);
            if (format == Format.Json) JsonField = new JsonField((XElement)node);
        }

        public NodeType NodeType { get; private set; }
        public XObject Object { get; set; }
        public XElement XmlElement { get { return (XElement)Object; } }
        public XAttribute XmlAttribute { get { return (XAttribute)Object; } }
        public JsonField JsonField { get; private set; }
        public Format Format { get; private set; }

        public XName Name {
            get { return Object.NodeType == XmlNodeType.Attribute ? XmlAttribute.Name : XmlElement.Name; }
            set
            {
                if (Object.NodeType == XmlNodeType.Attribute)
                {
                    var attribute = new XAttribute(value, XmlAttribute.Value);
                    if (XmlAttribute.Parent != null)
                    {
                        var parent = XmlAttribute.Parent;
                        XmlAttribute.Remove();
                        parent.Add(attribute);
                    }
                    Object = attribute;
                } else XmlElement.Name = value;
            } 
        }

        public string Value
        {
            get {
                if (Object.NodeType == XmlNodeType.Attribute) return XmlAttribute.Value;
                if ((NodeType == NodeType.XmlElement && XmlElement.IsEmpty) ||
                    (NodeType == NodeType.JsonField && JsonField.DataType == JsonDataType.Null)) return null;
                return XmlElement.Value;
            }
            set { if (Object.NodeType == XmlNodeType.Attribute) XmlAttribute.Value = value; else XmlElement.Value = value; }
        }
    }
}