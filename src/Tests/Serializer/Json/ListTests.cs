using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class ListTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class InheritedListOfComplexTypes : List<ComplexType> { }
        public class InheritedListOfSimpleTypes : List<int> { }

        // Lists

        [Test]
        public void should_serialize_list_of_complex_types()
        {
            var json = Bender.Serializer.Create().SerializeJson(new List<ComplexType> {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2"); 
        }

        [Test]
        public void should_serialize_inherited_list_of_complex_types()
        {
            var json = Bender.Serializer.Create().SerializeJson(new InheritedListOfComplexTypes
                {
                    new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(3);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2");
            items.JsonObjectItem(3).JsonNumberField("Value").Value.ShouldEqual("3");  
        }

        [Test]
        public void should_serialize_list_interface_of_complex_types()
        {
            var json = Bender.Serializer.Create().SerializeJson((IList<ComplexType>)new List<ComplexType> {
                new ComplexType { Value = 1 },
                new ComplexType { Value = 2 }});
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2"); 
        }

        [Test]
        public void should_serialize_list_of_simple_types()
        {
            var json = Bender.Serializer.Create().SerializeJson(new List<int> { 1, 2 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(2);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2"); 
        }

        [Test]
        public void should_serialize_list_interface_of_simple_types()
        {
            var json = Bender.Serializer.Create().SerializeJson((IList<int>)new List<int> { 1, 2 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(2);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2"); 
        }

        [Test]
        public void should_serialize_inherited_list_of_simple_types()
        {
            var json = Bender.Serializer.Create().SerializeJson(new InheritedListOfSimpleTypes { 1, 2, 3 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable()
        {
            var json = Bender.Serializer.Create().SerializeJson((IEnumerable<int>)new List<int> { 1, 2, 3 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_enumerable()
        {
            var json = Bender.Serializer.Create().SerializeJson((IEnumerable)new List<int> { 1, 2, 3 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_list()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ArrayList { 1, 2, 3 });
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array()
        {
            var json = Bender.Serializer.Create().SerializeJson(new List<int> { 1, 2, 3 }.ToArray());
            var items = json.ParseJson().JsonRootNumberArray();
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
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
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty {
                ComplexItems = new List<ComplexType> {
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }}
            });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("ComplexItems");
            items.Count().ShouldEqual(3);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2");
            items.JsonObjectItem(3).JsonNumberField("Value").Value.ShouldEqual("3");  
        }

        [Test]
        public void should_serialize_simple_type_list_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty { SimpleItems = new List<int> { 1, 2, 3 } });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("SimpleItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_inherited_list_of_simple_types_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty
            {
                InheritedSimpleItems = new InheritedListOfSimpleTypes { 1, 2, 3 }
            });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("InheritedSimpleItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_inherited_list_of_complex_types_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty
            {
                InheritedComplexItems = new InheritedListOfComplexTypes { new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 } }
            });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("InheritedComplexItems");
            items.Count().ShouldEqual(3);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2");
            items.JsonObjectItem(3).JsonNumberField("Value").Value.ShouldEqual("3");  
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty
            {
                InterfaceComplexItems = new List<ComplexType> {
                    new ComplexType { Value = 1 },
                    new ComplexType { Value = 2 },
                    new ComplexType { Value = 3 }}
            });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("InterfaceComplexItems");
            items.Count().ShouldEqual(3);
            items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2");
            items.JsonObjectItem(3).JsonNumberField("Value").Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty { GenericEnumerableItems = new List<int> { 1, 2, 3 } });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("GenericEnumerableItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_enumerable_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty { EnumerableItems = new List<int> { 1, 2, 3 } });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("EnumerableItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_list_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty { ArrayListItems = new ArrayList { 1, 2, 3 } });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("ArrayListItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
        }

        [Test]
        public void should_serialize_simple_type_array_property()
        {
            var json = Bender.Serializer.Create().SerializeJson(new ListProperty { ArrayItems = new List<int> { 1, 2, 3 }.ToArray() });
            var items = json.ParseJson().JsonRoot().JsonNumberArrayField("ArrayItems");
            items.Count().ShouldEqual(3);
            items.JsonNumberItem(1).Value.ShouldEqual("1");
            items.JsonNumberItem(2).Value.ShouldEqual("2");
            items.JsonNumberItem(3).Value.ShouldEqual("3");
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
            var json = Bender.Serializer.Create().SerializeJson(new ComplexGenericTypeProperty
            {
                Items = new List<ComplexGenericType<string, int>> {
                    new ComplexGenericType<string, int> { Value1 = "oh", Value2 = 5 },
                    new ComplexGenericType<string, int> { Value1 = "hai", Value2 = 6 } }
            });
            var items = json.ParseJson().JsonRoot().JsonObjectArrayField("Items");
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Value1").Value.ShouldEqual("oh");
            items.JsonObjectItem(1).JsonNumberField("Value2").Value.ShouldEqual("5");
            items.JsonObjectItem(2).JsonStringField("Value1").Value.ShouldEqual("hai");
            items.JsonObjectItem(2).JsonNumberField("Value2").Value.ShouldEqual("6");
        }

        [Test]
        public void should_serialize_complex_generic_type_list_name()
        {
            var json = Bender.Serializer.Create().SerializeJson(new List<ComplexGenericType<string, int>> {
                    new ComplexGenericType<string, int> { Value1 = "oh", Value2 = 5 },
                    new ComplexGenericType<string, int> { Value1 = "hai", Value2 = 6 } });
            var items = json.ParseJson().JsonRootObjectArray();
            items.Count().ShouldEqual(2);
            items.JsonObjectItem(1).JsonStringField("Value1").Value.ShouldEqual("oh");
            items.JsonObjectItem(1).JsonNumberField("Value2").Value.ShouldEqual("5");
            items.JsonObjectItem(2).JsonStringField("Value1").Value.ShouldEqual("hai");
            items.JsonObjectItem(2).JsonNumberField("Value2").Value.ShouldEqual("6");
        }
    }
}
