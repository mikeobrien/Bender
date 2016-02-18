using System.Xml.Serialization;
using Bender.Configuration;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.NamingConventions
{
    [TestFixture]
    public class DefaultPropertyConventionTests
    {
        public string GetName<T>(string property, Options options = null)
        {
            return MemberNamingConventions.Create()
                .GetName(new MemberContext(new CachedMember(typeof(T).GetProperty(property)), 
                    new Context(options ?? Options.Create(), Mode.Deserialize, "xml")));
        }

        public class PropertyNameConventions
        {
            [System.Xml.Serialization.XmlAttribute]
            public string NoNameAttributeProperty { get; set; }
            [Bender.Nodes.Xml.XmlAttribute("SomeAttribute")]
            public string BenderXmlAttributeProperty { get; set; }
            [System.Xml.Serialization.XmlAttribute("SomeAttribute")]
            public string SystemXmlAttributeProperty { get; set; }
            [XmlElement]
            public string NoNameElementProperty { get; set; }
            [XmlElement("SomeElement")]
            public string ElementProperty { get; set; }
            [XmlArray]
            public string[] NoNameArrayProperty { get; set; }
            [XmlArray("SomeArray")]
            public string[] ArrayProperty { get; set; }
            [XmlArray("SomeArray")]
            public string NotAnArrayProperty { get; set; }

            [XmlSiblings("SiblingSomeItem")]
            public string[] SiblingsProperty { get; set; }
        }

        [Test]
        public void should_use_property_name_if_name_not_set_in_xml_attribute()
        {
            GetName<PropertyNameConventions>("NoNameAttributeProperty")
                .ShouldEqual("NoNameAttributeProperty");
        }

        [Test]
        public void should_use_property_bender_xml_attribute_name()
        {
            GetName<PropertyNameConventions>("BenderXmlAttributeProperty")
                .ShouldEqual("SomeAttribute");
        }

        [Test]
        public void should_use_property_system_xml_attribute_name()
        {
            GetName<PropertyNameConventions>("SystemXmlAttributeProperty")
                .ShouldEqual("SomeAttribute");
        }

        [Test]
        public void should_use_xml_siblings_name()
        {
            GetName<PropertyNameConventions>("SiblingsProperty")
                .ShouldEqual("SiblingSomeItem");
        }

        [Test]
        public void should_use_property_name_if_name_not_set_in_xml_element()
        {
            GetName<PropertyNameConventions>("NoNameElementProperty")
                .ShouldEqual("NoNameElementProperty");
        }

        [Test]
        public void should_use_property_xml_element_name()
        {
            GetName<PropertyNameConventions>("ElementProperty")
                .ShouldEqual("SomeElement");
        }

        [Test]
        public void should_use_property_name_if_name_not_set_in_xml_array()
        {
            GetName<PropertyNameConventions>("NoNameArrayProperty")
                .ShouldEqual("NoNameArrayProperty");
        }

        [Test]
        public void should_use_property_xml_array_name()
        {
            GetName<PropertyNameConventions>("ArrayProperty")
                .ShouldEqual("SomeArray");
        }

        [Test]
        public void should_use_property_name_if_xml_array_not_on_an_enumerable_property()
        {
            GetName<PropertyNameConventions>("NotAnArrayProperty")
                .ShouldEqual("NotAnArrayProperty");
        }
    }
}
