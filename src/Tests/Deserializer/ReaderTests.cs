using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class ReaderTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class CustomReader
        {
            public DateTime SimpleType { get; set; }
            public ComplexType ComplexType { get; set; }
            public List<int> ListOfSimpleTypes { get; set; }
            public List<ComplexType> ListOfComplexTypes { get; set; }
            public IList<int> ListInterfaceOfSimpleTypes { get; set; }
            public IList<ComplexType> ListInterfaceOfComplexTypes { get; set; }
        }

        [Test]
        public void should_deserialize_simple_type_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <SimpleType>11-59</SimpleType>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<DateTime>((o, p, n) => DateTime.ParseExact(n.Element.Value, "hh-mm", CultureInfo.InvariantCulture))).Deserialize<CustomReader>(xml);

            result.SimpleType.ShouldEqual(DateTime.ParseExact("11-59", "hh-mm", CultureInfo.InvariantCulture));
        }

        [Test]
        public void should_deserialize_complex_type_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ComplexType>5</ComplexType>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>((o, p, n) => new ComplexType { Value = int.Parse(n.Element.Value)})).Deserialize<CustomReader>(xml);

            result.ComplexType.Value.ShouldEqual(5);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfComplexTypes>1,2,3</ListOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<List<ComplexType>>((o, p, n) => n.Element.Value.Split(',').Select(y => new ComplexType { Value = int.Parse(y)}).ToList())).Deserialize<CustomReader>(xml);
            
            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfSimpleTypes>1,2,3</ListOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<List<int>>((o, p, n) => n.Element.Value.Split(',').Select(int.Parse).ToList())).Deserialize<CustomReader>(xml);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfComplexTypes>
                        <ComplexType>[1]</ComplexType>
                        <ComplexType>[2]</ComplexType>
                        <ComplexType>[3]</ComplexType>
                    </ListOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>((o, p, n) => new ComplexType { Value = int.Parse(n.Element.Value.Replace("[", "").Replace("]", "")) })).Deserialize<CustomReader>(xml);

            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfSimpleTypes>
                        <Int32>[1]</Int32>
                        <Int32>[2]</Int32>
                        <Int32>[3]</Int32>
                    </ListOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<int>((o, p, n) => int.Parse(n.Element.Value.Replace("[", "").Replace("]", "")))).Deserialize<CustomReader>(xml);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfComplexTypes>1,2,3</ListInterfaceOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<IList<ComplexType>>((o, p, n) => n.Element.Value.Split(',').Select(y => new ComplexType { Value = int.Parse(y) }).ToList())).Deserialize<CustomReader>(xml);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfSimpleTypes>1,2,3</ListInterfaceOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<IList<int>>((o, p, n) => n.Element.Value.Split(',').Select(int.Parse).ToList())).Deserialize<CustomReader>(xml);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfComplexTypes>
                        <ComplexType>[1]</ComplexType>
                        <ComplexType>[2]</ComplexType>
                        <ComplexType>[3]</ComplexType>
                    </ListInterfaceOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>((o, p, n) => new ComplexType { Value = int.Parse(n.Element.Value.Replace("[", "").Replace("]", "")) })).Deserialize<CustomReader>(xml);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfSimpleTypes>
                        <Int32>[1]</Int32>
                        <Int32>[2]</Int32>
                        <Int32>[3]</Int32>
                    </ListInterfaceOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<int>((o, p, n) => int.Parse(n.Element.Value.Replace("[", "").Replace("]", "")))).Deserialize<CustomReader>(xml);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        // Complex type attributes

        public class AttributeComplexType
        {
            public static AttributeComplexType Parse(string value, bool fail)
            {
                if (fail) throw new ParseException(typeof(AttributeComplexType));
                return new AttributeComplexType { Value = value };
            }
            public string Value { get; set; }
        }

        public class ComplexAttribute
        {
            public AttributeComplexType Complex { get; set; }
        }

        [Test]
        public void should_set_complex_attribute_type_with_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader((o, p, n) => AttributeComplexType.Parse(n.Value, false))).Deserialize<ComplexAttribute>(xml);
            result.Complex.Value.ShouldEqual("hai");
        }

        [Test]
        public void should_throw_set_value_exception_when_failing_to_set_complex_attribute_type_with_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            Assert.Throws<SetValueException>(() => Bender.Deserializer.Create(
                x => x.AddReader((o, p, n) => AttributeComplexType.Parse(n.Value, true))).Deserialize<ComplexAttribute>(xml));
        }

        [Test]
        public void should_throw_set_value_exception_when_deserializing_complex_attribute_type_without_a_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            Assert.Throws<SetValueException>(() => Bender.Deserializer.Create().Deserialize<ComplexAttribute>(xml).Complex.ShouldBeNull());
        }
    }
}
