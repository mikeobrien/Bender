using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class ListTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class InheritedListOfComplexTypes : List<ComplexType> { }
        public class InheritedListOfSimpleTypes : List<int> { }

        [Test]
        public void should_serialize_list_with_custom_name_format()
        {
            var xml = Bender.Serializer.Create(x => x.WithDefaultGenericListXmlNameFormat("{0}s"))
                .Serialize(new List<ComplexType> { new ComplexType { Value = 1 }, new ComplexType { Value = 2 } });
            var root = xml.ParseXml().Element("ComplexTypes").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2"); 
        }

        // Lists

        [Test]
        public void should_serialize_list_of_complex_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<ComplexType> {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var root = xml.ParseXml().Element("ArrayOfComplexType").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2"); 
        }

        [Test]
        public void should_serialize_inherited_list_of_complex_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new InheritedListOfComplexTypes
                {
                    new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                });
            var root = xml.ParseXml().Element("InheritedListOfComplexTypes").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_list_interface_of_complex_types()
        {
            var xml = Bender.Serializer.Create().Serialize((IList<ComplexType>)new List<ComplexType> {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var root = xml.ParseXml().Element("ArrayOfComplexType").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_list_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<int> { 1, 2 });
            var root = xml.ParseXml().Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_list_interface_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize((IList<int>)new List<int> { 1, 2 });
            var root = xml.ParseXml().Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_inherited_list_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new InheritedListOfSimpleTypes { 1, 2, 3 });
            var root = xml.ParseXml().Element("InheritedListOfSimpleTypes").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable()
        {
            var xml = Bender.Serializer.Create().Serialize((IEnumerable<int>)new List<int> { 1, 2, 3 });
            var root = xml.ParseXml().Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_enumerable()
        {
            var xml = Bender.Serializer.Create().Serialize((IEnumerable)new List<int> { 1, 2, 3 });
            var root = xml.ParseXml().Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_list()
        {
            var xml = Bender.Serializer.Create().Serialize(new ArrayList { 1, 2, 3 });
            var root = xml.ParseXml().Element("ArrayOfObject").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<int> { 1, 2, 3 }.ToArray());
            var root = xml.ParseXml().Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        // List properties

        public class ListProperty
        {
            public List<ComplexType> ComplexItems { get; set; } 
            public List<int> SimpleItems { get; set; }
            public InheritedListOfSimpleTypes InheritedSimpleItems { get; set; }
            public InheritedListOfComplexTypes InheritedComplexItems { get; set; }
            public IList<ComplexType> InterfaceComplexItems { get; set; }
            public IEnumerable<int> GenericEnumerableItems { get; set; }
            public IEnumerable EnumerableItems { get; set; }
            public ArrayList ArrayListItems { get; set; }
            public int[] ArrayItems { get; set; }
 
        }

        [Test]
        public void should_serialize_complex_type_list_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty {
                ComplexItems = new List<ComplexType> {
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }}
            });
            var root = xml.ParseXml().Element("ListProperty").Element("ComplexItems").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_list_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { SimpleItems = new List<int> { 1, 2, 3 } });
            var root = xml.ParseXml().Element("ListProperty").Element("SimpleItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_inherited_list_of_simple_types_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty
            {
                InheritedSimpleItems = new InheritedListOfSimpleTypes { 1, 2, 3 }
            });
            var root = xml.ParseXml().Element("ListProperty").Element("InheritedSimpleItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_inherited_list_of_complex_types_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty
            {
                InheritedComplexItems = new InheritedListOfComplexTypes { new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 } }
            });
            var root = xml.ParseXml().Element("ListProperty").Element("InheritedComplexItems").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty
            {
                InterfaceComplexItems = new List<ComplexType> {
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }}
            });
            var root = xml.ParseXml().Element("ListProperty").Element("InterfaceComplexItems").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { GenericEnumerableItems = new List<int> { 1, 2, 3 } });
            var root = xml.ParseXml().Element("ListProperty").Element("GenericEnumerableItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_enumerable_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { EnumerableItems = new List<int> { 1, 2, 3 } });
            var root = xml.ParseXml().Element("ListProperty").Element("EnumerableItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_list_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { ArrayListItems = new ArrayList { 1, 2, 3 } });
            var root = xml.ParseXml().Element("ListProperty").Element("ArrayListItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { ArrayItems = new List<int> { 1, 2, 3 }.ToArray() });
            var root = xml.ParseXml().Element("ListProperty").Element("ArrayItems").Elements("Int32");
            root.ShouldNotBeNull();
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
            root.Skip(2).First().Value.ShouldEqual("3");
        }

        // Generic list items

        public class ComplexGenericType<T1, T2>
        {
            public T1 Value1 { get; set; }
            public T2 Value2 { get; set; }
        }

        public class ComplexGenericTypeProperty
        {
            public List<ComplexGenericType<string, int>> Items { get; set; }
        }

        [Test]
        public void should_serialize_complex_generic_type_list_property_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new ComplexGenericTypeProperty
            {
                Items = new List<ComplexGenericType<string, int>> {
                    new ComplexGenericType<string, int> { Value1 = "oh", Value2 = 5 },
                    new ComplexGenericType<string, int> { Value1 = "hai", Value2 = 6 } }
            });
            var root = xml.ParseXml().Element("ComplexGenericTypeProperty").Element("Items").Elements("ComplexGenericTypeOfStringInt32");
            root.First().Element("Value1").Value.ShouldEqual("oh");
            root.First().Element("Value2").Value.ShouldEqual("5");
            root.Skip(1).First().Element("Value1").Value.ShouldEqual("hai");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("6");
        }

        [Test]
        public void should_serialize_complex_generic_type_list_name()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<ComplexGenericType<string, int>> {
                    new ComplexGenericType<string, int> { Value1 = "oh", Value2 = 5 },
                    new ComplexGenericType<string, int> { Value1 = "hai", Value2 = 6 } });
            var root = xml.ParseXml().Element("ArrayOfComplexGenericTypeOfStringInt32").Elements("ComplexGenericTypeOfStringInt32");
            root.First().Element("Value1").Value.ShouldEqual("oh");
            root.First().Element("Value2").Value.ShouldEqual("5");
            root.Skip(1).First().Element("Value1").Value.ShouldEqual("hai");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("6");
        }
    }
}
