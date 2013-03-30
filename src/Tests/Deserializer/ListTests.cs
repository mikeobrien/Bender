using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class ListTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class InheritedListOfComplexTypes : List<ComplexType> { }
        public class InheritedListOfSimpleTypes : List<int> { }

        [Test]
        public void should_deserialize_list_with_custom_name_format()
        {
            const string xml = @"
                <ComplexTypes>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                </ComplexTypes>";
            var result = Bender.Deserializer.Create(x => x.WithDefaultGenericListNameFormat("{0}s"))
                .Deserialize<List<ComplexType>>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

        // Lists

        [Test]
        public void should_deserialize_list_of_complex_types()
        {
            const string xml = @"
                <ArrayOfComplexType>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                </ArrayOfComplexType>";
            var result = Bender.Deserializer.Create().Deserialize<List<ComplexType>>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_inherited_list_of_complex_types()
        {
            const string xml = @"
                <InheritedListOfComplexTypes>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                    <ComplexType><Value>3</Value></ComplexType>
                </InheritedListOfComplexTypes>";
            var result = Bender.Deserializer.Create().Deserialize<InheritedListOfComplexTypes>(xml);
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_list_interface_of_complex_types()
        {
            const string xml = @"
                <ArrayOfComplexType>
                    <ComplexType><Value>1</Value></ComplexType>
                    <ComplexType><Value>2</Value></ComplexType>
                </ArrayOfComplexType>";
            var result = Bender.Deserializer.Create().Deserialize<IList<ComplexType>>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_list_of_simple_types()
        {
            const string xml = @"
                <ArrayOfInt32>
                    <Int32>1</Int32>
                    <Int32>2</Int32>
                </ArrayOfInt32>";
            var result = Bender.Deserializer.Create().Deserialize<List<int>>(xml);
            result.Count.ShouldEqual(2);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_list_interface_of_simple_types()
        {
            const string xml = @"
                <ArrayOfInt32>
                    <Int32>1</Int32>
                    <Int32>2</Int32>
                </ArrayOfInt32>";
            var result = Bender.Deserializer.Create().Deserialize<IList<int>>(xml);
            result.Count.ShouldEqual(2);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_inherited_list_of_simple_types()
        {
            const string xml = @"
                <InheritedListOfSimpleTypes>
                    <Int32>1</Int32>
                    <Int32>2</Int32>
                    <Int32>3</Int32>
                </InheritedListOfSimpleTypes>";
            var result = Bender.Deserializer.Create().Deserialize<InheritedListOfSimpleTypes>(xml);
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_serialize_simple_type_generic_enumerable()
        {
            const string xml = @"
                <ArrayOfInt32>
                    <Int32>1</Int32>
                    <Int32>2</Int32>
                    <Int32>3</Int32>
                </ArrayOfInt32>";
            var result = Bender.Deserializer.Create().Deserialize<IEnumerable<int>>(xml);
            result.Count().ShouldEqual(3);
            result.First().ShouldEqual(1);
            result.Skip(1).First().ShouldEqual(2);
            result.Skip(2).First().ShouldEqual(3);
        }

        [Test]
        public void should_serialize_simple_type_enumerable()
        {
            const string xml = @"<ArrayOfObject><Int32>1</Int32></ArrayOfObject>";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().Deserialize<IEnumerable>(xml));
        }

        [Test]
        public void should_serialize_simple_type_array_list()
        {
            const string xml = @"<ArrayOfObject><Int32>1</Int32></ArrayOfObject>";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().Deserialize<ArrayList>(xml));
        }

        [Test]
        public void should_serialize_simple_type_array()
        {
            const string xml = @"
                <ArrayOfInt32>
                    <Int32>1</Int32>
                    <Int32>2</Int32>
                    <Int32>3</Int32>
                </ArrayOfInt32>";
            var result = Bender.Deserializer.Create().Deserialize<int[]>(xml);
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
            const string xml = @"
                <ListProperty>
                    <ComplexItems>
                        <ComplexType><Value>1</Value></ComplexType>
                        <ComplexType><Value>2</Value></ComplexType>
                        <ComplexType><Value>3</Value></ComplexType>
                    </ComplexItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).ComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property()
        {
            const string xml = @"
                <ListProperty>
                    <SimpleItems>
                        <Int32>1</Int32>
                        <Int32>2</Int32>
                        <Int32>3</Int32>
                    </SimpleItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).SimpleItems;
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_inherited_list_of_simple_types_property()
        {
            const string xml = @"
                <ListProperty>
                    <InheritedSimpleItems>
                        <Int32>1</Int32>
                        <Int32>2</Int32>
                        <Int32>3</Int32>
                    </InheritedSimpleItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).InheritedSimpleItems;
            result.Count.ShouldEqual(3);
            result[0].ShouldEqual(1);
            result[1].ShouldEqual(2);
            result[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_inherited_list_of_complex_types_property()
        {
            const string xml = @"
                <ListProperty>
                    <InheritedComplexItems>
                        <ComplexType><Value>1</Value></ComplexType>
                        <ComplexType><Value>2</Value></ComplexType>
                        <ComplexType><Value>3</Value></ComplexType>
                    </InheritedComplexItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).InheritedComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property()
        {
            const string xml = @"
                <ListProperty>
                    <InterfaceComplexItems>
                        <ComplexType><Value>1</Value></ComplexType>
                        <ComplexType><Value>2</Value></ComplexType>
                        <ComplexType><Value>3</Value></ComplexType>
                    </InterfaceComplexItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).InterfaceComplexItems;
            result.Count.ShouldEqual(3);
            result[0].Value.ShouldEqual(1);
            result[1].Value.ShouldEqual(2);
            result[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_generic_enumerable_property()
        {
            const string xml = @"
                <ListProperty>
                    <GenericEnumerableItems>
                        <Int32>1</Int32>
                        <Int32>2</Int32>
                        <Int32>3</Int32>
                    </GenericEnumerableItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).GenericEnumerableItems;
            result.Count().ShouldEqual(3);
            result.First().ShouldEqual(1);
            result.Skip(1).First().ShouldEqual(2);
            result.Skip(2).First().ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_enumerable_property()
        {
            const string xml = @"<ListProperty><EnumerableItems><Int32>3</Int32></EnumerableItems></ListProperty>";
            Assert.Throws<SetValueException>(() => Bender.Deserializer.Create().Deserialize<ListProperty>(xml));
        }

        [Test]
        public void should_deserialize_simple_type_array_list_property()
        {
            const string xml = @"<ListProperty><ArrayListItems><Int32>1</Int32></ArrayListItems></ListProperty>";
            Assert.Throws<SetValueException>(() => Bender.Deserializer.Create().Deserialize<ListProperty>(xml));
        }

        [Test]
        public void should_deserialize_simple_type_array_property()
        {
            const string xml = @"
                <ListProperty>
                    <ArrayItems>
                        <Int32>1</Int32>
                        <Int32>2</Int32>
                        <Int32>3</Int32>
                    </ArrayItems>
                </ListProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ListProperty>(xml).ArrayItems;
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
            const string xml = @"
                <ComplexGenericTypeProperty>
                    <Items>
                        <ComplexGenericTypeOfStringInt32><Value1>oh</Value1><Value2>5</Value2></ComplexGenericTypeOfStringInt32>
                        <ComplexGenericTypeOfStringInt32><Value1>hai</Value1><Value2>6</Value2></ComplexGenericTypeOfStringInt32>
                    </Items>
                </ComplexGenericTypeProperty>";
            var result = Bender.Deserializer.Create().Deserialize<ComplexGenericTypeProperty>(xml).Items;
            result.Count.ShouldEqual(2);
            result[0].Value1.ShouldEqual("oh");
            result[0].Value2.ShouldEqual(5);
            result[1].Value1.ShouldEqual("hai");
            result[1].Value2.ShouldEqual(6);
        }

        [Test]
        public void should_deserialize_complex_generic_type_list_name()
        {
            const string xml = @"
                <ArrayOfComplexGenericTypeOfStringInt32>
                    <ComplexGenericTypeOfStringInt32><Value1>oh</Value1><Value2>5</Value2></ComplexGenericTypeOfStringInt32>
                    <ComplexGenericTypeOfStringInt32><Value1>hai</Value1><Value2>6</Value2></ComplexGenericTypeOfStringInt32>
                </ArrayOfComplexGenericTypeOfStringInt32>";
            var result = Bender.Deserializer.Create().Deserialize<List<ComplexGenericType<string, int>>>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value1.ShouldEqual("oh");
            result[0].Value2.ShouldEqual(5);
            result[1].Value1.ShouldEqual("hai");
            result[1].Value2.ShouldEqual(6);
        }
    }
}
