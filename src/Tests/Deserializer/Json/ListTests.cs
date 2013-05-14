using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class ListTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class InheritedListOfComplexTypes : List<ComplexType> { }
        public class InheritedListOfSimpleTypes : List<int> { }

        // Lists

        [Test]
        public void should_deserialize_list_of_complex_types()
        {
            const string json = @"
                [
                    { ""Value"": 1 },
                    { ""Value"": 2 }
                ]";
            var result = Bender.Deserializer.Create().DeserializeJson<List<ComplexType>>(json);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_inherited_list_of_complex_types()
        {
            const string json = @"
                [
                    { ""Value"": 1 },
                    { ""Value"": 2 },
                    { ""Value"": 3 }
                ]";
            var result = Bender.Deserializer.Create().DeserializeJson<InheritedListOfComplexTypes>(json);
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_list_interface_of_complex_types()
        {
            const string json = @"
                [
                    { ""Value"": 1 },
                    { ""Value"": 2 }
                ]";
            var result = Bender.Deserializer.Create().DeserializeJson<IList<ComplexType>>(json);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_list_of_simple_types()
        {
            const string json = @"[ 1, 2 ]";
            var result = Bender.Deserializer.Create().DeserializeJson<List<int>>(json);
            result.Count.ShouldEqual(2);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_list_interface_of_simple_types()
        {
            const string json = @"[ 1, 2 ]";
            var result = Bender.Deserializer.Create().DeserializeJson<IList<int>>(json);
            result.Count.ShouldEqual(2);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_inherited_list_of_simple_types()
        {
            const string json = @"[ 1, 2, 3 ]";
            var result = Bender.Deserializer.Create().DeserializeJson<InheritedListOfSimpleTypes>(json);
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable()
        {
            const string json = @"[ 1, 2, 3 ]";
            var result = Bender.Deserializer.Create().DeserializeJson<IEnumerable<int>>(json);
            result.Count().ShouldEqual(3);
            result.First().ShouldEqual(1);
            result.Skip(1).First().ShouldEqual(2);
            result.Skip(2).First().ShouldEqual(3);
        }

        [Test]
        public void should_serialize_simple_type_enumerable()
        {
            const string json = @"[ 1 ]";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().DeserializeJson<IEnumerable>(json));
        }

        [Test]
        public void should_serialize_simple_type_array_list()
        {
            const string json = @"[ 1 ]";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().DeserializeJson<ArrayList>(json));
        }

        [Test]
        public void should_serialize_simple_type_array()
        {
            const string json = @"[ 1, 2, 3 ]";
            var result = Bender.Deserializer.Create().DeserializeJson<int[]>(json);
            result.Length.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
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
        public void should_deserialize_complex_type_list_property()
        {
            const string json = @"
                {
                    ""ComplexItems"": [
                        { ""Value"": 1 },
                        { ""Value"": 2 },
                        { ""Value"": 3 }
                    ]
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).ComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property()
        {
            const string json = @"{ ""SimpleItems"": [ 1, 2, 3 ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).SimpleItems;
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_inherited_list_of_simple_types_property()
        {
            const string json = @"{ ""InheritedSimpleItems"": [ 1, 2, 3 ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).InheritedSimpleItems;
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_inherited_list_of_complex_types_property()
        {
            const string json = @"
                {
                    ""InheritedComplexItems"": [
                        { ""Value"": 1 },
                        { ""Value"": 2 },
                        { ""Value"": 3 }
                    ]
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).InheritedComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property()
        {
            const string json = @"
                {
                    ""InterfaceComplexItems"": [
                        { ""Value"": 1 },
                        { ""Value"": 2 },
                        { ""Value"": 3 }
                    ]
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).InterfaceComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_generic_enumerable_property()
        {
            const string json = @"{ ""GenericEnumerableItems"": [ 1, 2, 3 ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).GenericEnumerableItems;
            result.Count().ShouldEqual(3);
            result.First().ShouldEqual(1);
            result.Skip(1).First().ShouldEqual(2);
            result.Skip(2).First().ShouldEqual(3);
        }

        [Test]
        public void should_not_deserialize_simple_type_enumerable_property()
        {
            const string json = @"{ ""EnumerableItems"": [ 3 ] }";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().DeserializeJson<ListProperty>(json));
        }

        [Test]
        public void should_not_deserialize_simple_type_array_list_property()
        {
            const string json = @"{ ""ArrayListItems"": [ 1 ] }";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().DeserializeJson<ListProperty>(json));
        }

        [Test]
        public void should_deserialize_simple_type_array_property()
        {
            const string json = @"{ ""ArrayItems"": [ 1, 2, 3 ] }";
            var result = Bender.Deserializer.Create().DeserializeJson<ListProperty>(json).ArrayItems;
            result.Length.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
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
        public void should_deserialize_complex_generic_type_list_property_name()
        {
            const string json = @"
                {
                    ""Items"": [
                        { ""Value1"": ""oh"", ""Value2"": 5 },
                        { ""Value1"": ""hai"", ""Value2"": 6 }
                    ]
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<ComplexGenericTypeProperty>(json).Items;
            result.Count.ShouldEqual(2);
            result[0].Value1.ShouldEqual("oh");
            result[0].Value2.ShouldEqual(5);
            result[1].Value1.ShouldEqual("hai");
            result[1].Value2.ShouldEqual(6);
        }

        [Test]
        public void should_deserialize_complex_generic_type_list_name()
        {
            const string json = @"
                [
                    { ""Value1"": ""oh"", ""Value2"": 5 },
                    { ""Value1"": ""hai"", ""Value2"": 6 }
                ]";
            var result = Bender.Deserializer.Create().DeserializeJson<List<ComplexGenericType<string, int>>>(json);
            result.Count.ShouldEqual(2);
            result[0].Value1.ShouldEqual("oh");
            result[0].Value2.ShouldEqual(5);
            result[1].Value1.ShouldEqual("hai");
            result[1].Value2.ShouldEqual(6);
        }
    }
}
