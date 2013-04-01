using System;
using System.Xml.Linq;

namespace Bender
{
    public enum ValueNodeType { Element, Attribute }

    public class ValueNode
    {
        public ValueNode(XObject node)
        {
            if (!(node is XElement) && !(node is XAttribute)) 
                throw new ArgumentException("XObject must be an XElement or XAttribute.", "node");
            Object = node;
        }

        public ValueNodeType NodeType { get { return Object is XElement ? ValueNodeType.Element : ValueNodeType.Attribute ; } }
        public XObject Object { get; set; }
        public XElement Element { get { return (XElement)Object; } }
        public XAttribute Attribute { get { return (XAttribute)Object; } }

        public XName Name { 
            get { return NodeType == ValueNodeType.Attribute ? Attribute.Name : Element.Name; }
            set
            {
                if (NodeType == ValueNodeType.Attribute)
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
            get { return NodeType == ValueNodeType.Attribute ? Attribute.Value : Element.Value; }
            set { if (NodeType == ValueNodeType.Attribute) Attribute.Value = value; else Element.Value = value; }
        }
    }
}