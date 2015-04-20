using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Collections;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.Json
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
        [TestCaseSource("SimpleDictionaryTypes")]
        public void should_serialize_typed_dictionary_entries(Type type, object value)
        {
            var dictionary = type.MakeGenericDictionaryType<string>().CreateInstance().As<IDictionary>();
            dictionary.Add("item", value);
            Serialize.Json(dictionary).ShouldEqual("{{\"item\":{0}}}".ToFormat(
                type.IsNumeric() || type.IsBoolean()
                ? value.ToString().ToLower()
                : "\"" + value.ToString().Replace("/", "\\/") + "\""));
        }

        // Complex types

        public class ComplexType
        {
            public string Property { get; set; }
        }

        private readonly static ComplexType ComplexTypeInstance = new ComplexType { Property = "hai" };

        private static readonly object[] ComplexTypeDictionaries = TestCases.Create()
            .AddTypeAndValues(new Dictionary<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues<IDictionary<string, ComplexType>>(new Dictionary<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues(new GenericDictionaryImpl<string, ComplexType> {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues(new Hashtable {{ "item", ComplexTypeInstance }})
            .AddTypeAndValues<IDictionary>(new Hashtable { { "item", ComplexTypeInstance } })
            .AddTypeAndValues(new DictionaryImpl {{ "item", ComplexTypeInstance }})

        .All;

        [Test]
        [TestCaseSource("ComplexTypeDictionaries")]
        public void should_serialize_dictionary_of_complex_type(Type type, object dictionary)
        {
            Serialize.Json(dictionary, type).ShouldEqual("{\"item\":{\"Property\":\"hai\"}}");
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
        [TestCaseSource("ComplexTypeDictionaryOfArrays")]
        public void should_serialize_dictionary_of_dictionary(Type type, object dictionary)
        {
            Serialize.Json(dictionary, type).ShouldEqual("{\"item\":[{\"Property\":\"hai\"}]}");
        }

        // Dictionary types

        private static readonly object[] ComplexTypeDictionaryOfDictionaries = TestCases.Create()
            .AddTypeAndValues(new Dictionary<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })
            .AddTypeAndValues<IDictionary<string, Dictionary<string, ComplexType>>>(new Dictionary<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })
            .AddTypeAndValues(new GenericDictionaryImpl<string, Dictionary<string, ComplexType>> 
                { { "item1", new Dictionary<string, ComplexType> { { "item2", ComplexTypeInstance } } } })

        .All;

        [Test]
        [TestCaseSource("ComplexTypeDictionaryOfDictionaries")]
        public void should_serialize_dictionary_of_array(Type type, object dictionary)
        {
            Serialize.Json(dictionary, type).ShouldEqual("{\"item1\":{\"item2\":{\"Property\":\"hai\"}}}");
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
        [TestCaseSource("ComplexArrayMembers")]
        public void should_serialize_member_array_of_complex_type(string name, Model model)
        {
            Serialize.Json(model).ShouldEqual("{{\"{0}\":{{\"item\":{{\"Property\":\"hai\"}}}}}}".ToFormat(name));
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
            Serialize.Json(new Dictionary<string, ActualType> { { "item", new ActualType 
                { Actual = "oh", Specified = "hai" } } }, typeof(Dictionary<string, ISpecifiedType>))
                .ShouldEqual("{\"item\":{\"Specified\":\"hai\"}}");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Json(new Dictionary<string, ActualType> { { "item", new ActualType 
                { Actual = "oh", Specified = "hai" } } }, typeof(Dictionary<string, ISpecifiedType>),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("{\"item\":{\"Actual\":\"oh\",\"Specified\":\"hai\"}}");
        }

        public class MemberSpecifiedType
        {
            public Dictionary<string, ISpecifiedType> Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new Dictionary<string, ISpecifiedType> 
                { { "item", new ActualType { Actual = "oh", Specified = "hai" } } } },
                typeof(MemberSpecifiedType))
                .ShouldEqual("{\"Specified\":{\"item\":{\"Specified\":\"hai\"}}}");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new Dictionary<string, ISpecifiedType> 
                { { "item", new ActualType { Actual = "oh", Specified = "hai" } } } },
                typeof(MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("{\"Specified\":{\"item\":{\"Actual\":\"oh\",\"Specified\":\"hai\"}}}");
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
            Serialize.Json(
                new DictionaryImplementation {{ "item1", "oh" }, { "item2", "hai" }})
                .ShouldEqual("{\"item1\":\"oh\",\"item2\":\"hai\"}");
        }

        [Test]
        public void should_treat_dictionary_root_as_object_when_configured()
        {
            Serialize.Json(
                new DictionaryImplementation { Property = "oh", Field = "hai" },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"Property\":\"oh\",\"Field\":\"hai\"}");
        }

        [Test]
        public void should_not_treat_dictionary_member_as_object_by_default()
        {
            Serialize.Json(new DictionaryMember { DictionaryImpl = new DictionaryImplementation 
                    { { "item1", "oh" }, { "item2", "hai" } } })
                .ShouldEqual("{\"DictionaryImpl\":{\"item1\":\"oh\",\"item2\":\"hai\"}}");
        }

        [Test]
        public void should_treat_dictionary_member_as_object_when_configured()
        {
            Serialize.Json(
                new DictionaryMember { DictionaryImpl = new DictionaryImplementation { Property = "oh", Field = "hai" } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"DictionaryImpl\":{\"Property\":\"oh\",\"Field\":\"hai\"}}");
        }

        [Test]
        public void should_not_treat_dictionary_array_item_as_object_by_default()
        {
            Serialize.Json(
                new List<DictionaryImplementation> { new DictionaryImplementation 
                    { { "item1", "oh" }, { "item2", "hai" } } })
                .ShouldEqual("[{\"item1\":\"oh\",\"item2\":\"hai\"}]");
        }

        [Test]
        public void should_treat_dictionary_array_item_as_object_when_configured()
        {
            Serialize.Json(
                new List<DictionaryImplementation> { new DictionaryImplementation 
                    { Property = "oh", Field = "hai" } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual("[{\"Property\":\"oh\",\"Field\":\"hai\"}]");
        }

        [Test]
        public void should_not_treat_dictionary_dictionary_entry_as_object_by_default()
        {
            Serialize.Json(
                new Dictionary<string, DictionaryImplementation> 
                    { { "item", new DictionaryImplementation { { "item1", "oh" }, { "item2", "hai" } } } })
                .ShouldEqual("{\"item\":{\"item1\":\"oh\",\"item2\":\"hai\"}}");
        }

        [Test]
        public void should_treat_dictionary_dictionary_entry_as_object_when_configured()
        {
            Serialize.Json(
                new Dictionary<string, DictionaryImplementation> 
                    { { "item", new DictionaryImplementation { Property = "oh", Field = "hai" } } },
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"item\":{\"Property\":\"oh\",\"Field\":\"hai\"}}");
        }

        // Type filtering

        [Test]
        public void should_not_include_types_when_does_not_match()
        {
            Serialize.Json(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType2"))
                .ShouldEqual("{}");
        }

        [Test]
        public void should_include_types_when_matches()
        {
            Serialize.Json(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance} },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual("{\"item\":{}}");
        }

        [Test]
        public void should_filter_types()
        {
            Serialize.Json(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.ExcludeType<ComplexType>())
                .ShouldEqual("{}");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Json(new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } },
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual("{}");
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
            Serialize.Json(graph).ShouldEqual("{\"Value\":{\"Value1\":\"hai\",\"Value2\":{}}}");
        }
    }
}
