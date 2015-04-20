using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;
using Bender.Configuration;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.NamingConventions
{
    [TestFixture]
    public class TypeNamingConventionsTests
    {
        private string GetName<T>(bool isRoot = false, Options options = null)
        {
            return GetName(typeof (T), isRoot, options);
        }

        private string GetName(Type type, bool isRoot = false, Options options = null)
        {
            return TypeNamingConvention.Create()
                .GetName(new TypeContext(type.GetCachedType(), new Context(options ?? Options.Create(), Mode.Deserialize, "xml"), isRoot));
        }

        public class SomeClass { }
            
        [Test]
        public void should_return_type_name_by_default()
        {
            GetName<SomeClass>().ShouldEqual("SomeClass");
        }

        // Xml attributes

        [XmlRoot]
        public class XmlRootWithNoName { }

        [Test]
        public void should_return_type_name_if_xml_root_is_not_set()
        {
            GetName<XmlRootWithNoName>().ShouldEqual("XmlRootWithNoName");
        }

        [XmlRoot("SomeRoot")]
        public class XmlRoot { }

        [Test]
        public void should_return_xml_root_name()
        {
            GetName<XmlRoot>(true).ShouldEqual("SomeRoot");
        }

        [Test]
        public void should_return_type_name_when_not_root()
        {
            GetName<XmlRoot>().ShouldEqual("XmlRoot");
        }

        [XmlRoot("SomeRoot"), XmlType("SomeType")]
        public class XmlRootAndType { }

        [Test]
        public void should_return_xml_root_over_xml_type()
        {
            GetName<XmlRootAndType>(true).ShouldEqual("SomeRoot");
        }

        [XmlType]
        public class XmlTypeWithNoName { }

        [Test]
        public void should_return_type_name_if_xml_type_is_not_set()
        {
            GetName<XmlTypeWithNoName>().ShouldEqual("XmlTypeWithNoName");
        }

        [XmlType("SomeType")]
        public class XmlType { }

        [Test]
        public void should_return_xml_type()
        {
            GetName<XmlType>().ShouldEqual("SomeType");
        }

        // Nullable types

        public struct NullableStruct { }

        [Test]
        [TestCase(typeof(int?), "Int32")]
        [TestCase(typeof(NullableStruct?), "NullableStruct")]
        public void should_return_underlying_nullable_type_name(Type type, string name)
        {
            GetName(type).ShouldEqual(name);
        }

        // Generic types

        [Test]
        public void should_return_generic_name()
        {
            GetName<KeyValuePair<string, int>>()
                .ShouldEqual(TypeNamingConvention.DefaultGenericTypeNameFormat.ToFormat(
                        "KeyValuePair", "StringInt32"));
        }

        public class GenericBaseType : Tuple<string> { public GenericBaseType() : base("") { } }

        [Test]
        public void should_not_return_generic_base_type_name()
        {
            GetName<GenericBaseType>().ShouldEqual("GenericBaseType");
        }

        [Test]
        public void should_return_custom_generic_name_format()
        {
            const string format = "{0}yada{1}";
            GetName<KeyValuePair<string, int>>(options: Options.Create(x => x.WithGenericTypeNameFormat(format)))
                .ShouldEqual(format.ToFormat("KeyValuePair", "StringInt32"));
        }

        [Test]
        public void should_return_nested_generic_name()
        {
            GetName<KeyValuePair<KeyValuePair<string, int>, KeyValuePair<int, string>>>()
                .ShouldEqual(TypeNamingConvention.DefaultGenericTypeNameFormat.ToFormat("KeyValuePair",
                        TypeNamingConvention.DefaultGenericTypeNameFormat.ToFormat("KeyValuePair", "StringInt32") +
                        TypeNamingConvention.DefaultGenericTypeNameFormat.ToFormat("KeyValuePair", "Int32String")));
        }

        // Enumerable types

        [Test]
        public void should_return_enumerable_name_format_for_arrays()
        {
            GetName<string[]>().ShouldEqual(TypeNamingConvention
                .DefaultEnumerableNameFormat.ToFormat("String"));
        }

        public class InheritedNameValueCollection : NameValueCollection { }
        public class InheritedArrayList : ArrayList { }
        public class InheritedGenericList : List<String> {}

        [Test]
        [TestCase(typeof(NameValueCollection), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(ArrayList), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IEnumerable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IList), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(InheritedNameValueCollection), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(InheritedArrayList), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(ListImpl), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(EnumerableImpl), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IList<string>), "String")]
        [TestCase(typeof(List<string>), "String")]
        [TestCase(typeof(IEnumerable<string>), "String")]
        [TestCase(typeof(InheritedGenericList), "String")]
        [TestCase(typeof(GenericStringListImpl), "String")]
        [TestCase(typeof(GenericStringEnumerableImpl), "String")]
        public void should_return_enumerable_name_format(Type type, string itemTypeName)
        {
            GetName(type).ShouldEqual(TypeNamingConvention.DefaultEnumerableNameFormat
                .ToFormat(itemTypeName));
        }

        [Test]
        [TestCase(typeof(NameValueCollection), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(List<string>), "String")]
        public void should_return_custom_enumerable_type_name_format(Type type, string itemTypeName)
        {
            const string format = "yada{0}";
            GetName(type, options: Options.Create(x => x.WithEnumerableTypeNameFormat(format)))
                .ShouldEqual(format.ToFormat(itemTypeName));
        }

        [Test]
        public void should_return_custom_enumerable_default_item_name()
        {
            const string itemName = "yada";
            GetName<NameValueCollection>(options: Options.Create(x => x.WithDefaultItemTypeName(itemName)))
                .ShouldEqual(TypeNamingConvention.DefaultEnumerableNameFormat
                .ToFormat(itemName));
        }

        [Test]
        [TestCase(typeof(InheritedGenericList))]
        [TestCase(typeof(GenericStringListImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(InheritedNameValueCollection))]
        [TestCase(typeof(InheritedArrayList))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(EnumerableImpl))]
        public void should_return_type_name_if_enumerable_implementations_are_treated_as_objects(Type type)
        {
            GetName(type, options: Options.Create(x => x.TreatEnumerableImplsAsObjects()))
                .ShouldEqual(type.Name);
        }

        [Test]
        [TestCase(typeof(IList<string>), "String")]
        [TestCase(typeof(List<string>), "String")]
        [TestCase(typeof(IEnumerable<string>), "String")]
        [TestCase(typeof(NameValueCollection), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(ArrayList), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IEnumerable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IList), TypeNamingConvention.DefaultItemTypeName)]
        public void should_not_return_type_name_for_bcl_enumerables_when_implementations_are_treated_as_objects(Type type, string itemTypeName)
        {
            GetName(type, options: Options.Create(x => x.TreatEnumerableImplsAsObjects()))
                .ShouldEqual(TypeNamingConvention.DefaultEnumerableNameFormat.ToFormat(itemTypeName));
        }

        // Dictionary types

        public class InheritedHashtable : Hashtable { }
        public class InheritedGenericDictionary : Dictionary<string, int> { }

        [Test]
        [TestCase(typeof(Hashtable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IDictionary), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(InheritedHashtable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(DictionaryImpl), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(Dictionary<string, int>), "Int32")]
        [TestCase(typeof(IDictionary<string, int>), "Int32")]
        [TestCase(typeof(InheritedGenericDictionary), "Int32")]
        [TestCase(typeof(GenericStringDictionaryImpl), "String")]
        public void should_return_dictionary_name_format(Type type, string itemTypeName)
        {
            GetName(type)
                .ShouldEqual(TypeNamingConvention.DefaultDictionaryNameFormat
                .ToFormat(itemTypeName));
        }

        [Test]
        [TestCase(typeof(InheritedHashtable))]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(InheritedGenericDictionary))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        public void should_return_type_name_if_dictionary_implementations_are_treated_as_objects(Type type)
        {
            GetName(type, options: Options.Create(x => x.TreatDictionaryImplsAsObjects()))
                .ShouldEqual(type.Name);
        }

        [Test]
        [TestCase(typeof(Dictionary<string, int>), "Int32")]
        [TestCase(typeof(IDictionary<string, int>), "Int32")]
        [TestCase(typeof(Hashtable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(IDictionary), TypeNamingConvention.DefaultItemTypeName)]
        public void should_not_return_type_name_for_bcl_dictionaries_when_implementations_are_treated_as_objects(Type type, string itemTypeName)
        {
            GetName(type, options: Options.Create(x => x.TreatDictionaryImplsAsObjects()))
                .ShouldEqual(TypeNamingConvention.DefaultDictionaryNameFormat.ToFormat(itemTypeName));
        }

        [Test]
        [TestCase(typeof(Hashtable), TypeNamingConvention.DefaultItemTypeName)]
        [TestCase(typeof(Dictionary<string, int>), "Int32")]
        public void should_return_custom_dictionary_name_format(Type type, string itemTypeName)
        {
            const string format = "yada{0}";
            GetName(type, options: Options.Create(x => x.WithDictionaryTypeNameFormat(format)))
                .ShouldEqual(format.ToFormat(itemTypeName));
        }

        [Test]
        public void should_return_custom_dictionary_default_item_name()
        {
            const string itemName = "yada";
            GetName<Hashtable>(options: Options.Create(x => x.WithDefaultItemTypeName(itemName)))
                .ShouldEqual(TypeNamingConvention.DefaultDictionaryNameFormat
                .ToFormat(itemName));
        }
    }
}
