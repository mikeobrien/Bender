using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Json
{
    [TestFixture]
    public class XmlAttributeTests
    {
        public class ComplexType { public int Value { get; set; } }

        // XmlElementAttribute

        public class InheritedListOfComplexType : List<ComplexType> { }

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
            const string json = "{ \"SomeValue\": 5 }";
            Bender.Deserializer.Create().DeserializeJson<ComplexTypePropertyCustomElementName>(json).Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_property_element_name()
        {
            const string json = "{ \"SomeItem\": { \"Value\": 5 } }";
            Bender.Deserializer.Create().DeserializeJson<ComplexTypePropertyCustomElementName>(json).Item.Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_list_property_element_name()
        {
            const string json = "{ \"SomeComplexItems\": [ { \"Value\": 5 } ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ComplexTypePropertyCustomElementName>(json).ComplexItems;
            result.Count.ShouldEqual(1);
            result[0].Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_element_name()
        {
            const string json = "{ \"SomeInheritedItems\": [ { \"Value\": 5 } ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ComplexTypePropertyCustomElementName>(json).InheritedListItems;
            result.Count.ShouldEqual(1);
            result[0].Value.ShouldEqual(5);
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
            const string json = @"
                {
                    ""Value1"": ""oh"",
                    ""Value2"": ""hai""
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<IgnoreProperty>(json);
            result.Value1.ShouldEqual("oh");
            result.Value2.ShouldBeNull();
        }
    }
}
