using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class XmlAttributeTests
    {
        public class ComplexType { public int Value { get; set; } }

        // XmlElementAttribute

        public class InheritedListOfComplexType : List<ComplexType> {}

        public class ComplexTypePropertyCustomElementName
        {
            [XmlElement("SomeComplexItems")]
            public List<ComplexType> ComplexItems { get; set; }

            [XmlElement("SomeInheritedItems")]
            public InheritedListOfComplexType InheritedListItems { get; set; }

            [XmlElement("SomeItem")]
            public ComplexType Item { get; set; }

            [XmlElement("SomeValue")]
            public int Value { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_property_element_name()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ComplexTypePropertyCustomElementName { Value = 5 });
            json.ParseJson().JsonRoot().JsonNumberField("SomeValue").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_property_element_name()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ComplexTypePropertyCustomElementName { Item = new ComplexType { Value = 5 } });
            json.ParseJson().JsonRoot().JsonObjectField("SomeItem").JsonNumberField("Value").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_element_name()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ComplexTypePropertyCustomElementName 
                { ComplexItems = new List<ComplexType> { new ComplexType { Value = 5 } } });
            json.ParseJson().JsonRoot().JsonObjectArrayField("SomeComplexItems").JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_element_name()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ComplexTypePropertyCustomElementName
                { InheritedListItems = new InheritedListOfComplexType { new ComplexType { Value = 5 } } });
            json.ParseJson().JsonRoot().JsonObjectArrayField("SomeInheritedItems").JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("5");
        }

        // XmlIgnoreAttribute

        public class IgnoreProperty
        {
            public string Value1 { get; set; }
            [XmlIgnore]
            public string Value2 { get; set; }
        }

        [Test]
        public void should_not_serialize_ignored_members()
        {
            var json = Bender.Serializer.Create().SerializeJson(new IgnoreProperty { Value1 = "oh", Value2 = "hai" });
            json.ParseJson().JsonRoot().JsonStringField("Value1").Value.ShouldEqual("oh");
            json.ParseJson().JsonRoot().JsonFieldExists("Value2").ShouldBeFalse();
        }
    }
}
