using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Bender.Extensions;

namespace Bender.Nodes.Xml
{
    public static class XmlExtensions
    {
        public static XElement CreateElement(this XElement element, XName name)
        {
            var newElement = new XElement(name);
            element.Add(newElement);
            return newElement;
        }

        public static XAttribute CreateAttribute(this XElement element, XName name)
        {
            var newAttribute = new XAttribute(name, "");
            element.Add(newAttribute);
            return newAttribute;
        }

        public static string GetPath(this XObject node)
        {
            var attribute = node as XAttribute;
            var element = node as XElement ?? attribute.Parent;
            var ancestors = element.Ancestors();
            return (ancestors.Any() ? "/" + ancestors.Select(x => x.Name.LocalName).Reverse()
                .Aggregate((a, i) => a + "/" + i) : "") + "/" + element.Name.LocalName +
                    (attribute != null ? "/@" + attribute.Name : "");
        }

        public static XElement Transform(this XElement element, string xsl)
        {
            if (xsl.IsNullOrEmpty()) return element;
            var document = new XDocument();
            using (var writer = document.CreateWriter())
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StringReader(xsl)));
                xslt.Transform(element.CreateNavigator(), writer);
            }
            return document.Root;
        }
    }
}
