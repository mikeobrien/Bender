using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class XmlAttributeTests
    {
        public class ComplexType { public int Value { get; set; } }

        // XmlArrayAttribute & XmlArrayItemAttribute

        public class InheritedComplexTypeList : List<ComplexType> { }

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
            const string xml = @"
                <CustomElementAndItemNameListProperty>
                    <Simple>
                        <Item>1</Item>
                        <Item>2</Item>
                        <Item>3</Item>
                    </Simple>
                </CustomElementAndItemNameListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<CustomElementAndItemNameListProperty>(xml).SimpleItems;
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_element_and_item_name()
        {
            const string xml = @"
                <CustomElementAndItemNameListProperty>
                    <Complex>
                        <Item><Value>1</Value></Item>
                        <Item><Value>2</Value></Item>
                        <Item><Value>3</Value></Item>
                    </Complex>
                </CustomElementAndItemNameListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<CustomElementAndItemNameListProperty>(xml).ComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_element_and_item_name()
        {
            const string xml = @"
                <CustomElementAndItemNameListProperty>
                    <InterfaceComplex>
                        <Item><Value>1</Value></Item>
                        <Item><Value>2</Value></Item>
                        <Item><Value>3</Value></Item>
                    </InterfaceComplex>
                </CustomElementAndItemNameListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<CustomElementAndItemNameListProperty>(xml).InterfaceComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_with_custom_element_and_item_name()
        {
            const string xml = @"
                <CustomElementAndItemNameListProperty>
                    <InheritedComplex>
                        <Item><Value>1</Value></Item>
                        <Item><Value>2</Value></Item>
                        <Item><Value>3</Value></Item>
                    </InheritedComplex>
                </CustomElementAndItemNameListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<CustomElementAndItemNameListProperty>(xml).InheritedComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
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
            const string xml = @"
                <Type>
                    <Value>5</Value>
                </Type>";
            Bender.Deserializer.Create().Deserialize<ComplexTypeCustomTypeName>(xml).Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_list_of_complex_type_custom_type_name()
        {
            const string xml = @"
                <ArrayOfType>
                    <Type><Value>5</Value></Type>
                </ArrayOfType>";
            var result = Bender.Deserializer.Create().Deserialize<List<ComplexTypeCustomTypeName>>(xml);
            result.Count.ShouldEqual(1);
            result[0].Value.ShouldEqual(5);
        }

        [XmlType("Type")]
        public class ComplexTypeCustomTypeNameList : List<ComplexType> { }

        [Test]
        public void should_serialize_inherited_list_type_with_custom_type_name()
        {
            const string xml = @"
                <Type>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                </Type>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexTypeCustomTypeNameList>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
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
            const string xml = @"
                <ComplexTypeCustomItemTypeNameListProperty>
                    <Items>
                        <Item><Value>1</Value></Item>
                        <Item><Value>2</Value></Item>
                        <Item><Value>3</Value></Item>
                    </Items>
                </ComplexTypeCustomItemTypeNameListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexTypeCustomItemTypeNameListProperty>(xml).Items;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_serialize_complex_type_list_with_custom_item_type_name()
        {
            const string xml = @"
                <ArrayOfItem>
                    <Item><Value>1</Value></Item>
                    <Item><Value>2</Value></Item>
                    <Item><Value>3</Value></Item>
                </ArrayOfItem>";
            var result = Bender.Deserializer.Create().Deserialize<List<ComplexTypeCustomItemTypeName>>(xml);
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
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
            const string xml = @"
                <Root>
                    <Value>5</Value>
                </Root>";
            Bender.Deserializer.Create().Deserialize<ComplexTypeCustomRootName>(xml).Value.ShouldEqual(5);
        }

        [XmlRoot("Root")]
        public class ComplexTypeCustomRootNameList : List<ComplexType> { }

        [Test]
        public void should_serialize_inherited_list_type_with_custom_root_name()
        {
            const string xml = @"
                <Root>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                </Root>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexTypeCustomRootNameList>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

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
            const string xml = @"
                <ComplexTypePropertyCustomElementName>
                    <SomeValue>5</SomeValue>
                </ComplexTypePropertyCustomElementName>";
            Bender.Deserializer.Create().Deserialize<ComplexTypePropertyCustomElementName>(xml).Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_property_element_name()
        {
            const string xml = @"
                <ComplexTypePropertyCustomElementName>
                    <SomeItem><Value>5</Value></SomeItem>
                </ComplexTypePropertyCustomElementName>";
            Bender.Deserializer.Create().Deserialize<ComplexTypePropertyCustomElementName>(xml).Item.Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_list_property_element_name()
        {
            const string xml = @"
                <ComplexTypePropertyCustomElementName>
                    <SomeComplexItems>
                        <ComplexType><Value>5</Value></ComplexType>
                    </SomeComplexItems>
                </ComplexTypePropertyCustomElementName>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexTypePropertyCustomElementName>(xml).ComplexItems;
            result.Count.ShouldEqual(1);
            result[0].Value.ShouldEqual(5);
        }

        [Test]
        public void should_serialize_complex_type_inherited_list_property_element_name()
        {
            const string xml = @"
                <ComplexTypePropertyCustomElementName>
                    <SomeInheritedItems>
                        <ComplexType><Value>5</Value></ComplexType>
                    </SomeInheritedItems>
                </ComplexTypePropertyCustomElementName>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexTypePropertyCustomElementName>(xml).InheritedListItems;
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
            const string xml = @"
                <IgnoreProperty>
                    <Value1>oh</Value1>
                    <Value2>hai</Value2>
                </IgnoreProperty>";
            var result = Bender.Deserializer.Create().Deserialize<IgnoreProperty>(xml);
            result.Value1.ShouldEqual("oh");
            result.Value2.ShouldBeNull();
        }

        // XmlAttribute

        public class AttributeProperty
        {
            public string Value1 { get; set; }
            [XmlAttribute]
            public string Value2 { get; set; }
            [XmlAttribute("SomeValue")]
            public string Value3 { get; set; }
        }

        [Test]
        public void should_serialize_members_marked_as_attributes()
        {
            const string xml =
                "<AttributeProperty Value2=\"hai\" SomeValue=\"there\"><Value1>oh</Value1></AttributeProperty>";
            var result = Bender.Deserializer.Create().Deserialize<AttributeProperty>(xml);
            result.Value1.ShouldEqual("oh");
            result.Value2.ShouldEqual("hai");
            result.Value3.ShouldEqual("there");
        }
    }
}
