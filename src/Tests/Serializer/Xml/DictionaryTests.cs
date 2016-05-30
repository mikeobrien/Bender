using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bender;
using Bender.Collections;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class DictionaryTests
    {
        // Simple types

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleDictionaryTypes = TestCases.Create()
            .AddTypeAndValues<string>("1")
            .AddTypeAndValues<Uri>(new Uri("http://www.xkcd.com"))

            .AddTypeAndValues<UriFormat>(UriFormat.UriEscaped)
            .AddTypeAndValues<UriFormat?>(UriFormat.UriEscaped)

            .AddTypeAndValues<DateTime>(DateTime.Today).AddTypeAndValues<DateTime?>(DateTime.Today)
            .AddTypeAndValues<TimeSpan>(TimeSpan.MaxValue).AddTypeAndValues<TimeSpan?>(TimeSpan.MaxValue)
            .AddTypeAndValues<Guid>(RandomGuid).AddTypeAndValues<Guid?>(RandomGuid)

            .AddTypeAndValues<Boolean>(true).AddTypeAndValues<Boolean?>(true)
            .AddTypeAndValues<Byte>(5).AddTypeAndValues<Byte?>(55)
            .AddTypeAndValues<SByte>(6).AddTypeAndValues<SByte?>(66)
            .AddTypeAndValues<Int16>(7).AddTypeAndValues<Int16?>(77)
            .AddTypeAndValues<UInt16>(8).AddTypeAndValues<UInt16?>(88)
            .AddTypeAndValues<Int32>(9).AddTypeAndValues<Int32?>(99)
            .AddTypeAndValues<UInt32>(10).AddTypeAndValues<UInt32?>(110)
            .AddTypeAndValues<Int64>(11).AddTypeAndValues<Int64?>(111)
            .AddTypeAndValues<UInt64>(12).AddTypeAndValues<UInt64?>(120)
            .AddTypeAndValues<IntPtr>(new IntPtr(13)).AddTypeAndValues<IntPtr?>(new IntPtr(130))
            .AddTypeAndValues<UIntPtr>(new UIntPtr(14)).AddTypeAndValues<UIntPtr?>(new UIntPtr(140))
            .AddTypeAndValues<Char>('a').AddTypeAndValues<Char?>('b')
            .AddTypeAndValues<Double>(15).AddTypeAndValues<Double?>(150)
            .AddTypeAndValues<Single>(16).AddTypeAndValues<Single?>(160)
            .AddTypeAndValues<Decimal>(17).AddTypeAndValues<Decimal?>(170)

            .All;

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_serialize_typed_dictionary_elements(Type type, object value)
        {
            var dictionary = type.MakeGenericDictionaryType<string>().CreateInstance().As<IDictionary>();
            dictionary.Add("item", value);
            var expectedValue = type.GetUnderlyingNullableType() == typeof(bool) ? value.ToString().ToLower() : value;
            Serialize.Xml(dictionary).ShouldEqual(Xml.Declaration + 
                $"<DictionaryOfAnyType><item>{expectedValue}</item></DictionaryOfAnyType>");
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_serialize_typed_dictionary_attributes(Type type, object value)
        {
            var dictionary = type.MakeGenericDictionaryType<string>().CreateInstance().As<IDictionary>();
            dictionary.Add("item", value);
            var expectedValue = type.GetUnderlyingNullableType() == typeof(bool) ? value.ToString().ToLower() : value;
            Serialize.Xml(dictionary, x => x.Serialization(y => y.XmlValuesAsAttributes()))
                .ShouldEqual(Xml.Declaration + $"<DictionaryOfAnyType item=\"{expectedValue}\" />");
        }

        // Complex types

        public class ComplexType
        {
            public string Property { get; set; }
        }

        private static readonly ComplexType ComplexTypeInstance = new ComplexType { Property = "hai" };

        private static readonly object[] ComplexTypeDictionaries = TestCases.Create()
            .AddTypeAndValues(new Dictionary<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues<IDictionary<string, ComplexType>>(new Dictionary<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues(new GenericDictionaryImpl<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues(new Hashtable {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues<IDictionary>(new Hashtable { { "item", ComplexTypeInstance } })
            .AddTypeAndValues(new DictionaryImpl {{ "item", ComplexTypeInstance }})

        .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeDictionaries))]
        public void should_serialize_dictionary_of_complex_type(Type type, object dictionary)
        {
            var elementName = type.IsGenericEnumerable() ? "ComplexType" : "AnyType";
            Serialize.Xml(dictionary, type).ShouldEqual(Xml.Declaration + 
                $"<DictionaryOf{elementName}><item><Property>hai</Property></item></DictionaryOf{elementName}>");
        }

        // Array types

        private static readonly object[] ComplexTypeDictionaryOfArrays = TestCases.Create()
            .AddTypeAndValues(new Dictionary<string, List<ComplexType>> 
                { { "item", new List<ComplexType> { ComplexTypeInstance } } })
            .AddTypeAndValues<IDictionary<string, List<ComplexType>>>(new Dictionary<string, List<ComplexType>> 
                { { "item", new List<ComplexType> { ComplexTypeInstance } } })
            .AddTypeAndValues(new GenericDictionaryImpl<string, List<ComplexType>> 
                { { "item", new List<ComplexType> { ComplexTypeInstance } } })

        .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeDictionaryOfArrays))]
        public void should_serialize_dictionary_of_dictionary(Type type, object dictionary)
        {
            Serialize.Xml(dictionary, type).ShouldEqual(Xml.Declaration + "<DictionaryOfArrayOfComplexType><item><ComplexType><Property>hai</Property></ComplexType></item></DictionaryOfArrayOfComplexType>");
        }

        // Dictionary types

        private static readonly object[] ComplexTypeDictionaryOfDictionaries = TestCases.Create()
            .AddTypeAndValues(new Hashtable
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })
            .AddTypeAndValues(new Dictionary<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })
            .AddTypeAndValues<IDictionary<string, Dictionary<string, ComplexType>>>(new Dictionary<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })
            .AddTypeAndValues(new GenericDictionaryImpl<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })

        .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeDictionaryOfDictionaries))]
        public void should_serialize_dictionary_of_array(Type type, object dictionary)
        {
            var elementName = type.IsGenericEnumerable() ? "DictionaryOfComplexType" : "AnyType";
            Serialize.Xml(dictionary, type).ShouldEqual(Xml.Declaration + 
                $"<DictionaryOf{elementName}><item1><item2><Property>hai" +
                $"</Property></item2></item1></DictionaryOf{elementName}>");
        }

        // Dictionary member

        public class Model
        {
            public Hashtable NonGenericDictionary { get; set; }
            public IDictionary DictionaryInterface { get; set; }
            public DictionaryImpl DictionaryImpl { get; set; }

            public Dictionary<string, ComplexType> ComplexDictionary { get; set; }
            public IDictionary<string, ComplexType> ComplexDictionaryInterface { get; set; }
            public GenericDictionaryImpl<string, ComplexType> ComplexDictionaryImpl { get; set; }
        }

        private static readonly object[] ComplexArrayMembers = TestCases.Create()
            .Add("ComplexDictionary", new Model { ComplexDictionary = new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .Add("ComplexDictionaryInterface", new Model { ComplexDictionaryInterface = new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .Add("ComplexDictionaryImpl", new Model { ComplexDictionaryImpl = new GenericDictionaryImpl<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .Add("NonGenericDictionary", new Model { NonGenericDictionary = new Hashtable { { "item", ComplexTypeInstance } } })
            .Add("DictionaryInterface", new Model { DictionaryInterface = new Hashtable { { "item", ComplexTypeInstance } } })
            .Add("DictionaryImpl", new Model {DictionaryImpl  = new DictionaryImpl { { "item", ComplexTypeInstance } } })
            .All;

        [Test]
        [TestCaseSource(nameof(ComplexArrayMembers))]
        public void should_serialize_member_array_of_complex_type(string name, Model model)
        {
            Serialize.Xml(model).ShouldEqual(Xml.Declaration + 
                $"<Model><{name}><item><Property>hai</Property></item></{name}></Model>");
        }

        // Actual vs specified type

        public class ActualType : ISpecifiedType
        {
            public string Actual { get; set; }
            public string Specified { get; set; }
        }

        public interface ISpecifiedType
        {
            string Specified { get; set; }
        }

        [Test]
        public void should_serialize_specified_type_by_default()
        {
            Serialize.Xml(new Dictionary<string, ActualType> { { "item", new ActualType 
                { Actual = "oh", Specified = "hai" } } }, typeof(Dictionary<string, ISpecifiedType>))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfISpecifiedType><item><Specified>hai</Specified></item></DictionaryOfISpecifiedType>");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Xml(new Dictionary<string, ActualType> { { "item", new ActualType 
                { Actual = "oh", Specified = "hai" } } }, typeof(Dictionary<string, ISpecifiedType>),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfISpecifiedType><item><Actual>oh</Actual><Specified>hai</Specified></item></DictionaryOfISpecifiedType>");
        }

        public class MemberSpecifiedType
        {
            public Dictionary<string, ISpecifiedType> Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Xml(new MemberSpecifiedType { Specified = new Dictionary<string, ISpecifiedType> 
                { { "item", new ActualType { Actual = "oh", Specified = "hai" } } } },
                typeof(MemberSpecifiedType))
                .ShouldEqual(Xml.Declaration + "<MemberSpecifiedType><Specified><item><Specified>hai</Specified></item></Specified></MemberSpecifiedType>");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Xml(new MemberSpecifiedType { Specified = new Dictionary<string, ISpecifiedType> 
                { { "item", new ActualType { Actual = "oh", Specified = "hai" } } } },
                typeof(MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration + "<MemberSpecifiedType><Specified><item><Actual>oh</Actual><Specified>hai</Specified></item></Specified></MemberSpecifiedType>");
        }

        // Dictionary vs object handling

        public class DictionaryMember
        {
            public DictionaryImplementation DictionaryImpl { get; set; }
        }

        public class DictionaryImplementation : GenericDictionaryImpl<string, string>
        {
            public string Property { get; set; }
            public string Field;
        }

        [Test]
        public void should_not_treat_dictionary_root_as_object_by_default()
        {
            Serialize.Xml(
                new DictionaryImplementation {{ "item1", "oh" }, { "item2", "hai" }})
                .ShouldEqual(Xml.Declaration + "<DictionaryOfString><item1>oh</item1><item2>hai</item2></DictionaryOfString>");
        }

        [Test]
        public void should_treat_dictionary_root_as_object_when_configured()
        {
            Serialize.Xml(
                new DictionaryImplementation { Property = "oh", Field = "hai" },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<DictionaryImplementation><Property>oh</Property><Field>hai</Field></DictionaryImplementation>");
        }

        [Test]
        public void should_not_treat_dictionary_member_as_object_by_default()
        {
            Serialize.Xml(new DictionaryMember { DictionaryImpl = new DictionaryImplementation 
                    { { "item1", "oh" }, { "item2", "hai" } } })
                .ShouldEqual(Xml.Declaration + "<DictionaryMember><DictionaryImpl><item1>oh</item1><item2>hai</item2></DictionaryImpl></DictionaryMember>");
        }

        [Test]
        public void should_treat_dictionary_member_as_object_when_configured()
        {
            Serialize.Xml(
                new DictionaryMember { DictionaryImpl = new DictionaryImplementation { Property = "oh", Field = "hai" } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<DictionaryMember><DictionaryImpl><Property>oh</Property><Field>hai</Field></DictionaryImpl></DictionaryMember>");
        }

        [Test]
        public void should_not_treat_dictionary_array_item_as_object_by_default()
        {
            Serialize.Xml(
                new List<DictionaryImplementation> { new DictionaryImplementation 
                    { { "item1", "oh" }, { "item2", "hai" } } })
                .ShouldEqual(Xml.Declaration + "<ArrayOfDictionaryOfString><DictionaryOfString><item1>oh</item1><item2>hai</item2></DictionaryOfString></ArrayOfDictionaryOfString>");
        }

        [Test]
        public void should_treat_dictionary_array_item_as_object_when_configured()
        {
            Serialize.Xml(
                new List<DictionaryImplementation> { new DictionaryImplementation 
                    { Property = "oh", Field = "hai" } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<ArrayOfDictionaryImplementation><DictionaryImplementation><Property>oh</Property><Field>hai</Field></DictionaryImplementation></ArrayOfDictionaryImplementation>");
        }

        [Test]
        public void should_not_treat_dictionary_dictionary_entry_as_object_by_default()
        {
            Serialize.Xml(
                new Dictionary<string, DictionaryImplementation> 
                    { { "item", new DictionaryImplementation { { "item1", "oh" }, { "item2", "hai" } } } })
                .ShouldEqual(Xml.Declaration + "<DictionaryOfDictionaryOfString><item><item1>oh</item1><item2>hai</item2></item></DictionaryOfDictionaryOfString>");
        }

        [Test]
        public void should_treat_dictionary_dictionary_entry_as_object_when_configured()
        {
            Serialize.Xml(
                new Dictionary<string, DictionaryImplementation> 
                    { { "item", new DictionaryImplementation { Property = "oh", Field = "hai" } } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<DictionaryOfDictionaryImplementation><item><Property>oh</Property><Field>hai</Field></item></DictionaryOfDictionaryImplementation>");
        }

        // Type filtering

        [Test]
        public void should_not_include_types_when_does_not_match()
        {
            Serialize.Xml(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType2"))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfComplexType />");
        }

        [Test]
        public void should_include_types_when_matches()
        {
            Serialize.Xml(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance} },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfComplexType><item /></DictionaryOfComplexType>");
        }

        [Test]
        public void should_filter_types()
        {
            Serialize.Xml(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.ExcludeType<ComplexType>())
                .ShouldEqual(Xml.Declaration + "<DictionaryOfComplexType />");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Xml(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfComplexType />");
        }

        // Circular references

        public class CircularReference
        {
            public CircularReferenceNode Value { get; set; }
        }

        public class CircularReferenceNode
        {
            public string Value1 { get; set; }
            public Dictionary<string, CircularReference> Value2 { get; set; }
        }

        [Test]
        public void should_not_allow_circular_references()
        {
            var graph = new CircularReference { Value = new CircularReferenceNode { Value1 = "hai" } };
            graph.Value.Value2 = new Dictionary<string, CircularReference> { { "item", graph } };
            Serialize.Xml(graph).ShouldEqual(Xml.Declaration + "<CircularReference><Value><Value1>hai</Value1><Value2 /></Value></CircularReference>");
        }

        // Namespaces

        [Test]
        public void should_set_default_namespace()
        {
            Serialize.Xml(new Dictionary<string, string> { { "item", "hai" } },
                x => x.Serialization(y => y.WithDefaultXmlNamespace("urn:yada")))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfString xmlns=\"urn:yada\"><item>hai</item></DictionaryOfString>");
        }

        [Test]
        public void should_add_namespace_prefix()
        {
            Serialize.Xml(new Dictionary<string, string> { { "item", "hai" } },
                x => x.Serialization(y => y.AddXmlNamespace("abc", "urn:yada").AddXmlVisitor<string>((s, t, o) => t.SetNamespacePrefix("abc"))))
                .ShouldEqual(Xml.Declaration + "<DictionaryOfString xmlns:abc=\"urn:yada\"><abc:item>hai</abc:item></DictionaryOfString>");
        }

        // Naming conventions

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Serialize.Xml(new Dictionary<string, string>()).ShouldEqual(Xml.Declaration + "<DictionaryOfString />");
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Serialize.Xml(new Dictionary<string, string>(),
                x => x.WithDictionaryTypeNameFormat("Hashtable_{0}")).ShouldEqual(Xml.Declaration + "<Hashtable_String />");
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot : Dictionary<string, string> { }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlRoot()).ShouldEqual(Xml.Declaration + "<Root />");
        }

        [XmlType("Type")]
        public class XmlType : Dictionary<string, string> { }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlType()).ShouldEqual(Xml.Declaration + "<Type />");
        }
    }
}
