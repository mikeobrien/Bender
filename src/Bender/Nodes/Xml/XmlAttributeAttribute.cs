using System;

namespace Bender.Nodes.Xml
{
    public class XmlAttributeAttribute : Attribute
    {
        public XmlAttributeAttribute() { }

        public XmlAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; }
    }
}
