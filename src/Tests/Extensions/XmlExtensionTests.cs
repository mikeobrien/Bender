using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Bender.Extensions;
using Bender.Nodes.Xml;
using NUnit.Framework;
using Should;

namespace Tests.Extensions
{
    [TestFixture]
    public class XmlExtensionTests
    {
        [Test]
        public void should_create_and_add_an_element()
        {
            var parent = new XElement("parent");
            var child = parent.CreateElement("child");
            child.Name.ShouldEqual("child");
            child.Parent.ShouldEqual(parent);
            parent.Elements().First().ShouldEqual(child);
        }

        [Test]
        public void should_create_and_add_an_attribute()
        {
            var parent = new XElement("parent");
            var attribute = parent.CreateAttribute("attribute");
            attribute.Name.ShouldEqual("attribute");
            attribute.Parent.ShouldEqual(parent);
            parent.Attributes().First().ShouldEqual(attribute);
        }

        [Test]
        [TestCase("/oh", "/oh")]
        [TestCase("/oh/hai/yada", "/oh/hai/yada")]
        public void should_generate_xml_element_xpath(string xpath, string result)
        {
            var document = XDocument.Parse("<oh><hai><yada yada=\"\"/></hai></oh>");
            document.Root.XPathSelectElement(xpath).GetPath().ShouldEqual(result);
        }

        [Test]
        public void should_generate_xml_attribute_xpath()
        {
            var document = XDocument.Parse("<oh><hai><yada yada=\"\"/></hai></oh>");
            document.Root.XPathSelectElement("/oh/hai/yada").Attribute("yada").GetPath().ShouldEqual("/oh/hai/yada/@yada");
        }
    }
}
