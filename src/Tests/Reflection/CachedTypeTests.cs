using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Reflection
{   
    [TestFixture]
    public class CachedTypeTests
    {
        [XmlType, XmlRoot]
        public class SomeType { }

        public static Type SomeTypeInfo = typeof(SomeType);
        public static CachedType SomeTypeCached = new CachedType(SomeTypeInfo);

        [Test]
        public void should_return_name()
        {
            SomeTypeCached.Name.ShouldEqual(SomeTypeInfo.Name);
        }

        [Test]
        public void should_return_full_name()
        {
            SomeTypeCached.FullName.ShouldEqual(SomeTypeInfo.FullName);
        }

        [Test]
        public void should_return_generic_base_name()
        {
            new CachedType(typeof(KeyValuePair<,>)).GenericBaseName.ShouldEqual("KeyValuePair");
        }

        [Test]
        public void should_return_friendly_full_name()
        {
            new CachedType(typeof(KeyValuePair<string, int>)).FriendlyFullName.ShouldEqual("System.Collections.Generic.KeyValuePair<System.String, System.Int32>");
        }

        [Test]
        public void should_return_type()
        {
            SomeTypeCached.Type.ShouldEqual(SomeTypeInfo);
        }

        [Test]
        public void should_return_underlying()
        {
            new CachedType(typeof(int?)).UnderlyingType.Type.ShouldEqual(typeof(int));
        }

        public class Members
        {
            public string this[string index] { get { return null; } set { } }
            public string Field; 
            public string Property { get; set; } 
            public void Method() { }
        }
        
        public class EnumerableMembers : EnumerableImpl { public string Field; public string Property { get; set; } }
        public class GenericEnumerableMembers : GenericStringEnumerableImpl { public string Field; public string Property { get; set; } }
        public class ListMembers : ListImpl { public string Field; public string Property { get; set; } }
        public class GenericListMembers : GenericStringListImpl { public string Field; public string Property { get; set; } }
        public class DictionaryMembers : DictionaryImpl { public string Field; public string Property { get; set; } }
        public class GenericDictionaryMembers : GenericStringDictionaryImpl { public string Field; public string Property { get; set; } }
        public class CollectionMembers : CollectionImpl { public string Field; public string Property { get; set; } }
        public class GenericCollectionMembers : GenericStringCollectionImpl { public string Field; public string Property { get; set; } }
        
        [Test]
        [TestCase(typeof(Members))]
        [TestCase(typeof(EnumerableMembers))]
        [TestCase(typeof(GenericEnumerableMembers))]
        [TestCase(typeof(ListMembers))]
        [TestCase(typeof(GenericListMembers))]
        [TestCase(typeof(DictionaryMembers))]
        [TestCase(typeof(GenericDictionaryMembers))]
        [TestCase(typeof(CollectionMembers))]
        [TestCase(typeof(GenericCollectionMembers))]
        public void should_return_members(Type type)
        {
            var members = new CachedType(type).Members;
            members.ShouldTotal(2);
            members.ShouldContain(x => x.Name == "Field");
            members.ShouldContain(x => x.Name == "Property");
        }

        [Test]
        public void should_get_member()
        {
            new CachedType(typeof(Members)).GetMember("Property")
                .PropertyInfo.ShouldEqual(typeof(Members).GetProperty("Property"));
        }

        [Test]
        public void should_get_indexer()
        {
            new CachedType(typeof(Members)).GetIndexer()
                .PropertyInfo.ShouldEqual(typeof(Members).GetProperty("Item"));
        }

        [Test]
        public void should_indicate_a_simple_type()
        {
            new CachedType(typeof(int?)).IsSimpleType.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_when_not_a_simple_type()
        {
            SomeTypeCached.IsSimpleType.ShouldBeFalse();
        } 

        // Enumerable

        [Test]
        public void should_indicate_enumerable()
        {
            new CachedType(typeof(EnumerableImpl)).IsEnumerable.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_enumerable()
        {
            SomeTypeCached.IsEnumerable.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_enumerable_interface()
        {
            new CachedType(typeof(IEnumerable)).IsEnumerableInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_enumerable_interface()
        {
            new CachedType(typeof(IEnumerable<string>)).IsEnumerableInterface.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_enumerable()
        {
            new CachedType(typeof(GenericStringEnumerableImpl)).IsGenericEnumerable.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_enumerable()
        {
            new CachedType(typeof(EnumerableImpl)).IsGenericEnumerable.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_enumerable_interface()
        {
            new CachedType(typeof(IEnumerable<string>)).IsGenericEnumerableInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_enumerable_interface()
        {
            new CachedType(typeof(IEnumerable)).IsGenericEnumerableInterface.ShouldBeFalse();
        }

        [Test]
        public void should_get_generic_enumerable_type()
        {
            new CachedType(typeof(GenericStringEnumerableImpl)).GenericEnumerableType.Type.ShouldBe<string>();
        }


        // List

        [Test]
        public void should_indicate_a_list()
        {
            new CachedType(typeof(ListImpl)).IsList.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_list()
        {
            SomeTypeCached.IsList.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_non_generic_list()
        {
            new CachedType(typeof(ListImpl)).IsNonGenericList.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_non_generic_list()
        {
            new CachedType(typeof(GenericStringListImpl)).IsNonGenericList.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_list_interface()
        {
            new CachedType(typeof(IList)).IsListInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_list_interface()
        {
            new CachedType(typeof(IEnumerable)).IsListInterface.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_list()
        {
            new CachedType(typeof(GenericStringListImpl)).IsGenericList.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_list()
        {
            new CachedType(typeof(ListImpl)).IsGenericList.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_list_interface()
        {
            new CachedType(typeof(IList<string>)).IsGenericListInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_list_interface()
        {
            new CachedType(typeof(IEnumerable<string>)).IsGenericListInterface.ShouldBeFalse();
        }

        [Test]
        public void should_create_generic_list_intstance()
        {
            var instance = new CachedType(typeof(IList<string>)).CreateGenericListInstance();
            instance.ShouldNotBeNull();
            instance.ShouldBeType<List<string>>();
        }



        // Dictionary

        [Test]
        public void should_indicate_a_dictionary()
        {
            new CachedType(typeof(DictionaryImpl)).IsDictionary.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_dictionary()
        {
            SomeTypeCached.IsDictionary.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_dictionary_interface()
        {
            new CachedType(typeof(IDictionary)).IsDictionaryInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_dictionary_interface()
        {
            new CachedType(typeof(IDictionary<string, string>)).IsDictionaryInterface.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_non_generic_dictionary()
        {
            new CachedType(typeof(DictionaryImpl)).IsNonGenericDictionary.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_non_generic_dictionary()
        {
            new CachedType(typeof(GenericStringDictionaryImpl)).IsNonGenericDictionary.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_dictionary()
        {
            new CachedType(typeof(GenericStringDictionaryImpl)).IsGenericDictionary.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_dictionary()
        {
            new CachedType(typeof(DictionaryImpl)).IsGenericDictionary.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_generic_dictionary_interface()
        {
            new CachedType(typeof(IDictionary<string, string>)).IsGenericDictionaryInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_a_generic_dictionary_interface()
        {
            new CachedType(typeof(IDictionary)).IsGenericDictionaryInterface.ShouldBeFalse();
        }

        [Test]
        public void should_create_generic_dictionary_instance()
        {
            var instance = new CachedType(typeof(IDictionary<string, int>)).CreateGenericDictionaryInstance();
            instance.ShouldNotBeNull();
            instance.ShouldBeType<Dictionary<string, int>>();
        }

        [Test]
        public void should_get_generic_dictionary_types()
        {
            var types = new CachedType(typeof(IDictionary<string, int>)).GenericDictionaryTypes;
            types.Key.Type.ShouldBe<string>();
            types.Value.Type.ShouldBe<int>();
        }

        // Array

        [Test]
        public void should_indicate_if_array()
        {
            new CachedType(typeof(int[])).IsArray.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_array()
        {
            SomeTypeCached.IsArray.ShouldBeFalse();
        }

        [Test]
        public void should_get_element_type()
        {
            new CachedType(typeof(string[])).ElementType.Type.ShouldBe<string>();
        }

        // Generics

        [Test]
        public void should_indicate_if_generic()
        {
            new CachedType(typeof(KeyValuePair<string, string>)).IsGenericType.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_generic()
        {
            SomeTypeCached.IsGenericType.ShouldBeFalse();
        }

        [Test]
        public void should_get_generic_type_arguments()
        {
            var types = new CachedType(typeof(IDictionary<string, int>)).GenericArguments.ToList();
            types[0].Type.ShouldBe<string>();
            types[1].Type.ShouldBe<int>();
        }
        
        // Type info

        [Test]
        public void should_indicate_if_nullable()
        {
            new CachedType(typeof(int?)).IsNullable.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_nullable()
        {
            new CachedType(typeof(int)).IsNullable.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_enum()
        {
            new CachedType(typeof(TypeCode)).IsEnum.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_enum()
        {
            SomeTypeCached.IsEnum.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_value_type()
        {
            new CachedType(typeof(int)).IsValueType.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_value_type()
        {
            SomeTypeCached.IsValueType.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_interface()
        {
            new CachedType(typeof(IEnumerable)).IsInterface.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_interface()
        {
            SomeTypeCached.IsInterface.ShouldBeFalse();
        }

        [Test]
        public void should_return_type_code()
        {
            new CachedType(typeof(int)).TypeCode.ShouldEqual(TypeCode.Int32);
        }

        // BCL Collections

        [Test]
        public void should_indicate_if_in_bcl_collection_namespace()
        {
            new CachedType(typeof(IEnumerable)).IsInBclCollectionNamespace.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_in_bcl_collection_namespace()
        {
            new CachedType(typeof(GenericStringEnumerableImpl)).IsInBclCollectionNamespace.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_bcl_collection_type()
        {
            new CachedType(typeof(IEnumerable)).IsBclCollectionType.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_not_bcl_collection_type()
        {
            new CachedType(typeof(GenericStringEnumerableImpl)).IsBclCollectionType.ShouldBeFalse();
        }

        // Attributes

        [Test]
        public void should_return_attribute()
        {
            SomeTypeCached.GetAttribute<XmlTypeAttribute>().ShouldNotBeNull();
            SomeTypeCached.GetAttribute<XmlRootAttribute>().ShouldNotBeNull();
        }

        [Test]
        public void should_return_null_when_attribute_not_applied_attribute()
        {
            SomeTypeCached.GetAttribute<XmlArrayItemAttribute>().ShouldBeNull();
        }

        [Test]
        public void should_indicate_if_type_has_attributes()
        {
            SomeTypeCached.HasAttribute<XmlTypeAttribute>().ShouldBeTrue();
            SomeTypeCached.HasAttribute<XmlRootAttribute>().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_type_does_not_have_an_attributes()
        {
            SomeTypeCached.HasAttribute<XmlArrayItemAttribute>().ShouldBeFalse();
        }

        // Methods

        public class Methods
        {
            public string ActionValue;
            public void ActionWithNoArguments() { ActionValue = "hai"; }
            public void Action(string value) { ActionValue = value; }
            public string FuncWithNoArguments() { return "hai"; }
            public string Func(string value) { return value; }
        }

        [Test]
        public void should_call_action_with_no_arguments()
        {
            var instance = new Methods();
            new CachedType(typeof(Methods)).InvokeAction("ActionWithNoArguments", instance);
            instance.ActionValue.ShouldEqual("hai");
        }

        [Test]
        public void should_call_action()
        {
            var instance = new Methods();
            new CachedType(typeof(Methods)).InvokeAction("Action", instance, "hai");
            instance.ActionValue.ShouldEqual("hai");
        }

        [Test]
        public void should_call_func_with_no_arguments()
        {
            new CachedType(typeof(Methods)).InvokeFunc<string>("FuncWithNoArguments", new Methods())
                .ShouldEqual("hai");
        }

        [Test]
        public void should_call_func()
        {
            new CachedType(typeof(Methods)).InvokeFunc<string>("Func", new Methods(), "hai")
                .ShouldEqual("hai");
        }
    }
}
