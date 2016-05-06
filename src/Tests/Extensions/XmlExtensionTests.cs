using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
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
            document.Root.XPathSelectElement("/oh/hai/yada").Attribute("yada")
                .GetPath().ShouldEqual("/oh/hai/yada/@yada");
        }

        [Test]
        public void should_transform_xml()
        {
            var document = XDocument.Parse(
                @"<?xml version=""1.0"" ?>
                <persons>
                  <person username=""JS1"">
                    <name>John</name>
                    <family-name>Smith</family-name>
                  </person>
                  <person username=""MI1"">
                    <name>Morka</name>
                    <family-name>Ismincius</family-name>
                  </person>
                </persons>");
            var xsl = 
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0"">
                  <xsl:output method=""xml"" indent=""yes""/>

                  <xsl:template match=""/persons"">
                    <root>
                      <xsl:apply-templates select=""person""/>
                    </root>
                  </xsl:template>

                  <xsl:template match=""person"">
                    <name username=""{@username}"">
                      <xsl:value-of select=""name"" />
                    </name>
                  </xsl:template>

                </xsl:stylesheet>";

            document.Root.Transform(xsl).ToString(SaveOptions.DisableFormatting).ShouldEqual(
                "<root>" +
                    @"<name username=""JS1"">John</name>" +
                    @"<name username=""MI1"">Morka</name>" +
                "</root>");
        }

        [Test]
        public void should_return_element_if_xsl_not_specified(
            [Values(null, "")] string xsl)
        {
            var element = new XElement("fark");
            element.Transform(xsl).ShouldEqual(element);
        }
    }
}
