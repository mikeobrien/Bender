using System;

namespace Bender.Nodes.Xml.Microsoft
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
