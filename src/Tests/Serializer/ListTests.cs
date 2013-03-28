using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
            var xml = Bender.Serializer.Create(x => x.WithDefaultGenericListNameFormat("{0}s"))
                .Serialize(new List<ComplexType> { new ComplexType(), new ComplexType() });
            var root = XDocument.Parse(xml).Element("ComplexTypes");
            root.ShouldNotBeNull();
        }

        // Lists

        [Test]
        public void should_serialize_list_of_complex_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<ComplexType> {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var root = XDocument.Parse(xml).Element("ArrayOfComplexType").Elements("ComplexType");
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
            var root = XDocument.Parse(xml).Element("InheritedListOfComplexTypes").Elements("ComplexType");
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
            var root = XDocument.Parse(xml).Element("ArrayOfComplexType").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_list_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new List<int> { 1, 2 });
            var root = XDocument.Parse(xml).Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_list_interface_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize((IList<int>)new List<int> { 1, 2 });
            var root = XDocument.Parse(xml).Element("ArrayOfInt32").Elements("Int32");
            root.ShouldNotBeNull();
            root.Count().ShouldEqual(2);
            root.First().Value.ShouldEqual("1");
            root.Skip(1).First().Value.ShouldEqual("2");
        }

        [Test]
        public void should_serialize_inherited_list_of_simple_types()
        {
            var xml = Bender.Serializer.Create().Serialize(new InheritedListOfSimpleTypes { 1, 2, 3 });
            var root = XDocument.Parse(xml).Element("InheritedListOfSimpleTypes").Elements("Int32");
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
            var root = XDocument.Parse(xml).Element("ListProperty").Element("ComplexItems").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_list_property()
        {
            var xml = Bender.Serializer.Create().Serialize(new ListProperty { SimpleItems = new List<int> { 1, 2, 3 } });
            var root = XDocument.Parse(xml).Element("ListProperty").Element("SimpleItems").Elements("Int32");
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
            var root = XDocument.Parse(xml).Element("ListProperty").Element("InheritedSimpleItems").Elements("Int32");
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
            var root = XDocument.Parse(xml).Element("ListProperty").Element("InheritedComplexItems").Elements("ComplexType");
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
            var root = XDocument.Parse(xml).Element("ListProperty").Element("InterfaceComplexItems").Elements("ComplexType");
            root.ShouldNotBeNull();
            root.First().Element("Value").Value.ShouldEqual("1");
            root.Skip(1).First().Element("Value").Value.ShouldEqual("2");
            root.Skip(2).First().Element("Value").Value.ShouldEqual("3");
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
            var root = XDocument.Parse(xml).Element("ComplexGenericTypeProperty").Element("Items").Elements("ComplexGenericTypeOfStringInt32");
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
            var root = XDocument.Parse(xml).Element("ArrayOfComplexGenericTypeOfStringInt32").Elements("ComplexGenericTypeOfStringInt32");
            root.First().Element("Value1").Value.ShouldEqual("oh");
            root.First().Element("Value2").Value.ShouldEqual("5");
            root.Skip(1).First().Element("Value1").Value.ShouldEqual("hai");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("6");
        }
    }
}
