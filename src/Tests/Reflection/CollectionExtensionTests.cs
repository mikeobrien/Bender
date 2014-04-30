using System;
using System.Collections;
using System.Collections.Generic;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Reflection
{
    [TestFixture]
    public class CollectionExtensionTests
    {
        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(List<string>))]
        public void should_indicate_if_a_type_is_a_clr_collection(Type type)
        {
            type.GetCachedType().IsBclCollectionType.ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(object))]
        [TestCase(typeof(IComparer))]
        public void should_indicate_if_a_type_is_not_a_clr_collection(Type type)
        {
            type.GetCachedType().IsBclCollectionType.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_that_an_anon_type_is_not_a_clr_collection()
        {
            new { }.GetCachedType().IsBclCollectionType.ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IComparer))]
        public void should_indicate_if_a_type_is_in_a_clr_collection(Type type)
        {
            type.IsInBclCollectionNamespace().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_in_a_clr_collection()
        {
            typeof(object).IsInBclCollectionNamespace().ShouldBeFalse();
        }

        // Arrays

        [Test]
        public void should_create_an_array()
        {
            typeof(int[]).CreateArray().ShouldBeType<int[]>();
        }

        // Enumerable

        [Test]
        public void should_indicate_if_a_type_is_ienumerable()
        {
            typeof(IEnumerable).IsEnumerableInterface().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_ienumerable()
        {
            typeof(ICollection).IsEnumerableInterface().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(IEnumerable<string>))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(string))]
        [TestCase(typeof(ICollection))]
        public void should_indicate_if_type_is_enumerable(Type type)
        {
            type.IsEnumerable().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_type_is_not_enumerable()
        {
            typeof(object).IsEnumerable().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_an_interface_is_a_generic_enumerable()
        {
            typeof(IEnumerable<string>).IsGenericEnumerableInterface().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(List<string>))]
        public void should_indicate_if_an_interface_is_not_a_generic_enumerable(Type type)
        {
            type.IsGenericEnumerableInterface().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IEnumerable<string>))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(string))]
        public void should_indicate_if_a_type_is_a_generic_enumerable(Type type)
        {
            type.IsGenericEnumerable().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(object))]
        [TestCase(typeof(ICollection))]
        public void should_indicate_if_a_type_is_not_a_generic_enumerable(Type type)
        {
            type.IsGenericEnumerable().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IEnumerable<string>))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        public void should_get_the_generic_enumerable_type(Type type)
        {
            type.GetGenericEnumerableType().ShouldEqual(typeof(string));
        }

        // List

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(List<string>))]
        public void should_indicate_an_type_is_a_non_generic_list(Type type)
        {
            type.IsNonGenericList().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_type_is_not_a_non_generic_list()
        {
            typeof(GenericStringListImpl).IsNonGenericList().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(List<string>))]
        public void should_indicate_an_object_is_a_non_generic_list(Type type)
        {
            type.CreateInstance().IsNonGenericList().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_object_is_not_a_non_generic_list()
        {
            new GenericStringListImpl().IsNonGenericList().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IList))]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        public void should_indicate_if_type_is_a_list(Type type)
        {
            type.IsList().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(object))]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IDictionary))]
        public void should_indicate_if_type_is_not_a_list(Type type)
        {
            type.IsList().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_an_object_is_a_list()
        {
            new ArrayList().IsList().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_object_is_not_a_list()
        {
            new Hashtable().IsList().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_null_object_is_not_a_list()
        {
            ((object)null).IsList().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_type_is_a_list_interface()
        {
            typeof (IList).IsListInterface().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IList<string>))]
        public void should_indicate_if_type_is_not_a_list_interface(Type type)
        {
            type.IsListInterface().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        public void should_indicate_if_type_is_a_generic_list(Type type)
        {
            type.IsGenericList().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(object))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(ArrayList))]
        public void should_indicate_if_type_is_not_a_generic_list(Type type)
        {
            type.IsGenericList().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_instance_is_a_generic_list()
        {
            new List<string>().IsGenericList().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_instance_is_not_a_generic_list()
        {
            new ArrayList().IsGenericList().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_type_is_a_generic_list_interface()
        {
            typeof(IList<string>).IsGenericListInterface().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList))]
        public void should_indicate_if_type_is_not_a_generic_list_interface(Type type)
        {
            type.IsGenericListInterface().ShouldBeFalse();
        }

        [Test]
        public void should_create_a_generic_list()
        {
            typeof(IList<int>).GetCachedType().CreateGenericListInstance().ShouldBeType<List<int>>();
        }

        // Dictionary

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(Dictionary<string, string>))]
        public void should_indicate_an_type_is_a_non_generic_dictionary(Type type)
        {
            type.IsNonGenericDictionary().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_type_is_not_a_non_generic_dictionary()
        {
            typeof(GenericStringDictionaryImpl).IsNonGenericDictionary().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(Dictionary<string, string>))]
        public void should_indicate_an_object_is_a_non_generic_dictionary(Type type)
        {
            type.CreateInstance().IsNonGenericDictionary().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_object_is_not_a_non_generic_dictionary()
        {
            new GenericStringDictionaryImpl().IsNonGenericDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_null_object_is_not_a_non_generic_dictionary()
        {
            ((object)null).IsNonGenericDictionary().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(Dictionary<string, string>))]
        [TestCase(typeof(IDictionary<string, string>))]
        public void should_indicate_if_type_is_a_dictionary(Type type)
        {
            type.IsDictionary().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(object))]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IList))]
        public void should_indicate_if_type_is_not_a_dictionary(Type type)
        {
            type.IsDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_type_is_a_dictionary_interface()
        {
            typeof(IDictionary).IsDictionaryInterface().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IDictionary<,>))]
        [TestCase(typeof(Hashtable))]
        public void should_indicate_if_type_is_a_dictionary_interface(Type type)
        {
            type.IsDictionaryInterface().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(Dictionary<string, string>))]
        [TestCase(typeof(IDictionary<string, string>))]
        public void should_indicate_if_type_is_a_generic_dictionary(Type type)
        {
            type.IsGenericDictionary().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(object))]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IDictionary))]
        public void should_indicate_if_type_is_not_a_generic_dictionary(Type type)
        {
            type.IsGenericDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_an_object_is_a_generic_dictionary()
        {
            new Dictionary<string, string>().IsGenericDictionary().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_an_object_is_not_a_generic_dictionary()
        {
            new Hashtable().IsGenericDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_a_null_object_is_not_a_generic_dictionary()
        {
            ((object)null).IsGenericDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_object_is_not_a_generic_dictionary()
        {
            new Hashtable().IsGenericDictionary().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_type_is_a_generic_dictionary_interface()
        {
            typeof(IDictionary<string, string>).IsGenericDictionaryInterface().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(Dictionary<string, string>))]
        public void should_indicate_if_type_is_not_a_generic_dictionary_interface(Type type)
        {
            type.IsGenericDictionaryInterface().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IDictionary<string, int>))]
        [TestCase(typeof(Dictionary<string, int>))]
        public void should_get_generic_dictionary_types(Type type)
        {
            var result = type.GetGenericDictionaryTypes();
            result.Key.ShouldEqual(typeof(string));
            result.Value.ShouldEqual(typeof(int));
        }

        [Test]
        public void should_create_a_generic_dictionary()
        {
            typeof(IDictionary<string, int>).GetCachedType().CreateGenericDictionaryInstance().ShouldBeType<Dictionary<string, int>>();
        }
    }
}
