using System;

namespace Bender.Nodes.Xml
{
    public class XmlSiblingsAttribute : Attribute
    {
        public XmlSiblingsAttribute() { }

        public XmlSiblingsAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; set; }
    }
}