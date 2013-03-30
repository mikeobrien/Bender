using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class XmlAttributeTests
    {
        public class ComplexType { public int Value { get; set; } }

        // XmlArrayAttribute & XmlArrayItemAttribute

        public class InheritedComplexTypeList : List<ComplexType> {}

        public class CustomElementAndItemNameListProperty
        {
            [XmlArray("Simple"), XmlArrayItem("Item")]
            public List<int> SimpleItems { get; set; }

            [XmlArray("Complex"), XmlArrayItem("Item")]
            public List<ComplexType> ComplexItems { get; set; }

            [XmlArray("InterfaceComplex"), XmlArrayItem("Item")]
            public IList<ComplexType> InterfaceComplexItems { get; set; }

            [XmlArray("InheritedComplex"), XmlArrayItem("Item")]
            public InheritedComplexTypeList InheritedComplexItems { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_list_property_with_custom_element_and_item_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new CustomElementAndItemNameListProperty { SimpleItems = new List<int> { 1, 2, 3 } });
            var root = xml.ParseXml().Element("CustomElementAndItemNameListProperty").Element("Simple").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_element_and_item_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new CustomElementAndItemNameListProperty
            {
                ComplexItems = new List<ComplexType> { 
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }
            } });
            var root = xml.ParseXml().Element("CustomElementAndItemNameListProperty").Element("Complex").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_element_and_item_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new CustomElementAndItemNameListProperty
            {
                InterfaceComplexItems = new List<ComplexType> { 
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }
            }
            });
            var root = xml.ParseXml().Element("CustomElementAndItemNameListProperty").Element("InterfaceComplex").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_with_custom_element_and_item_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new CustomElementAndItemNameListProperty
            {
                InheritedComplexItems = new InheritedComplexTypeList { 
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }
            }
            });
            var root = xml.ParseXml().Element("CustomElementAndItemNameListProperty").Element("InheritedComplex").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        // XmlTypeAttribute

        [XmlType("Type")]
        public class ComplexTypeCustomTypeName
        {
            public int Value { get; set; }
        }

        [Test]
        public void should_serialize_complex_type_custom_type_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypeCustomTypeName { Value = 5 });
            Debug.WriteLine(xml);
            xml.ParseXml().Element("Type").Element("Value").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_list_of_complex_type_custom_type_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<ComplexTypeCustomTypeName> { new ComplexTypeCustomTypeName { Value = 5 }});
            Debug.WriteLine(xml);
            xml.ParseXml().Element("ArrayOfType").Element("Type").Element("Value").Value.ShouldEqual("5");
        }

        [XmlType("Type")]
        public class ComplexTypeCustomTypeNameList : List<ComplexType> { }

        [Test]
        public void should_serialize_inherited_list_type_with_custom_type_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypeCustomTypeNameList {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var root = xml.ParseXml().Element("Type").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
        }

        [XmlType("Item")]
        public class ComplexTypeCustomItemTypeName { public int Value { get; set; } }

        public class ComplexTypeCustomItemTypeNameListProperty
        {
            public List<ComplexTypeCustomItemTypeName> Items { get; set; }
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_item_type_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypeCustomItemTypeNameListProperty {
                Items = new List<ComplexTypeCustomItemTypeName> { 
                    new ComplexTypeCustomItemTypeName { Value = 1 },
                    new ComplexTypeCustomItemTypeName { Value = 2 },
                    new ComplexTypeCustomItemTypeName { Value = 3 }
            } });
            var root = xml.ParseXml().Element("ComplexTypeCustomItemTypeNameListProperty").Element("Items").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_complex_type_list_with_custom_item_type_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<ComplexTypeCustomItemTypeName> { 
                    new ComplexTypeCustomItemTypeName { Value = 1 },
                    new ComplexTypeCustomItemTypeName { Value = 2 },
                    new ComplexTypeCustomItemTypeName { Value = 3 }
            });
            var root = xml.ParseXml().Element("ArrayOfItem").Elements("Item");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        // XmlRootAttribute

        [XmlRoot("Root")]
        public class ComplexTypeCustomRootName
        {
            public int Value { get; set; }
        }

        [Test]
        public void should_serialize_complex_type_custom_root_element_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypeCustomRootName { Value = 5 });
            Debug.WriteLine(xml);
            xml.ParseXml().Element("Root").Element("Value").Value.ShouldEqual("5");
        }

        [XmlRoot("Root")]
        public class ComplexTypeCustomRootNameList : List<ComplexType> { }

        [Test]
        public void should_serialize_inherited_list_type_with_custom_root_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypeCustomRootNameList {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var root = xml.ParseXml().Element("Root").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
        }

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
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypePropertyCustomElementName { Value = 5 });
            xml.ParseXml().Element("ComplexTypePropertyCustomElementName").Element("SomeValue").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_property_element_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypePropertyCustomElementName { Item = new ComplexType { Value = 5 } });
            xml.ParseXml().Element("ComplexTypePropertyCustomElementName").Element("SomeItem").Element("Value").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_element_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypePropertyCustomElementName 
                { ComplexItems = new List<ComplexType> { new ComplexType { Value = 5 } } });
            xml.ParseXml().Element("ComplexTypePropertyCustomElementName").Element("SomeComplexItems")
                .Elements("ComplexType").Single().Element("Value").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_element_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexTypePropertyCustomElementName
                { InheritedListItems = new InheritedListOfComplexType { new ComplexType { Value = 5 } } });
            xml.ParseXml().Element("ComplexTypePropertyCustomElementName").Element("SomeInheritedItems")
                .Elements("ComplexType").Single().Element("Value").Value.ShouldEqual("5");
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
            var xml = Bender.Serializer.Create().Serialize(new IgnoreProperty { Value1 = "oh", Value2 = "hai" });
            xml.ParseXml().Element("IgnoreProperty").Element("Value1").Value.ShouldEqual("oh");
            xml.ParseXml().Element("IgnoreProperty").Element("Value2").ShouldBeNull();
        }
    }
}
