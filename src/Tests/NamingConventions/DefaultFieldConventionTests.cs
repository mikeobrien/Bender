using System.Reflection;
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
    public class DefaultFieldConventionTests
    {
        public string GetName<T>(string field, Options options = null)
        {
            return MemberNamingConventions.Create()
                .GetName(new MemberContext(new CachedMember(typeof(T).GetField(field, 
                    BindingFlags.NonPublic | BindingFlags.Instance)), 
                    new Context(options ?? Options.Create(), Mode.Deserialize, "xml")));
        }

        public class FieldNameConventions
        {
            [XmlAttribute]
            private string _noNameAttributeProperty;
            [Bender.Nodes.Xml.Microsoft.XmlAttribute("SomeAttribute")]
            private string _benderXmlAttributeProperty;
            [XmlAttribute("SomeAttribute")]
            private string _systemXmlAttributeProperty;
            [XmlElement]
            private string _noNameElementProperty;
            [XmlElement("SomeElement")]
            private string _elementProperty;
            [XmlArray]
            private string[] _noNameArrayProperty;
            [XmlArray("SomeArray")]
            private string[] _arrayProperty;
            [XmlArray("SomeArray")]
            private string _notAnArrayProperty;

            [XmlSiblings("SiblingSomeItem")]
            private string[] _siblingsProperty;
        }

        [Test]
        public void should_use_field_name_if_name_not_set_in_xml_attribute()
        {
            GetName<FieldNameConventions>("_noNameAttributeProperty")
                .ShouldEqual("_noNameAttributeProperty");
        }

        [Test]
        public void should_use_field_bender_xml_attribute_name()
        {
            GetName<FieldNameConventions>("_benderXmlAttributeProperty")
                .ShouldEqual("SomeAttribute");
        }

        [Test]
        public void should_use_field_system_xml_attribute_name()
        {
            GetName<FieldNameConventions>("_systemXmlAttributeProperty")
                .ShouldEqual("SomeAttribute");
        }

        [Test]
        public void should_use_xml_siblings_name()
        {
            GetName<FieldNameConventions>("_siblingsProperty")
                .ShouldEqual("SiblingSomeItem");
        }

        [Test]
        public void should_use_field_name_if_name_not_set_in_xml_element()
        {
            GetName<FieldNameConventions>("_noNameElementProperty")
                .ShouldEqual("_noNameElementProperty");
        }

        [Test]
        public void should_use_field_xml_element_name()
        {
            GetName<FieldNameConventions>("_elementProperty")
                .ShouldEqual("SomeElement");
        }

        [Test]
        public void should_use_field_name_if_name_not_set_in_xml_array()
        {
            GetName<FieldNameConventions>("_noNameArrayProperty")
                .ShouldEqual("_noNameArrayProperty");
        }

        [Test]
        public void should_use_fieldxml_array_name()
        {
            GetName<FieldNameConventions>("_arrayProperty")
                .ShouldEqual("SomeArray");
        }

        [Test]
        public void should_use_field_name_if_xml_array_not_on_an_enumerable_property()
        {
            GetName<FieldNameConventions>("_notAnArrayProperty")
                .ShouldEqual("_notAnArrayProperty");
        }
    }
}
