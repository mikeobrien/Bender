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
    public class ArrayTests
    {
        // Simple types 

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleArrayTypes = TestCases.Create()
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
        [TestCaseSource(nameof(SimpleArrayTypes))]
        public void should_serialize_typed_array_items(Type type, object value)
        {
            var list = type.MakeGenericListType().CreateInstance().As<IList>();
            list.Add(value);
            Serialize.Json(list).ShouldEqual("[{0}]".ToFormat(
                type.IsNumeric() || type.IsBoolean()
                ? value.ToString().ToLower()
                : "\"" + value.ToString().Replace("/", "\\/") + "\""));
        }

        // Complex types

        public class ComplexType
        {
            public string Property {get; set; }
        }

        private static readonly ComplexType ComplexTypeInstance = new ComplexType { Property = "hai" };

        private static readonly object[] ComplexTypeArrays = TestCases.Create()
            .AddTypeAndValues(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<IEnumerable<ComplexType>>(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues(new GenericListImpl<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<ComplexType[]>(new[] { ComplexTypeInstance })
            .AddTypeAndValues(new ArrayList { ComplexTypeInstance })
            .AddTypeAndValues<IEnumerable>(new ArrayList { ComplexTypeInstance })
            .AddTypeAndValues(new EnumerableImpl(ComplexTypeInstance))
            .AddTypeAndValues(new GenericEnumerableImpl<ComplexType>(ComplexTypeInstance))

        .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeArrays))]
        public void should_serialize_list_of_complex_type(Type type, object list)
        {
            Serialize.Json(list, type).ShouldEqual("[{\"Property\":\"hai\"}]");
        }

        // Array types

        private static readonly object[] ComplexTypeArrayOfArrays = TestCases.Create()
            .AddTypeAndValues(new List<IList<ComplexType>> { new List<ComplexType> { ComplexTypeInstance } })
            .AddTypeAndValues<IEnumerable<IList<ComplexType>>>(new List<IList<ComplexType>> { new List<ComplexType> { ComplexTypeInstance } })
            .AddTypeAndValues<IList<IList<ComplexType>>>(new List<IList<ComplexType>> { new List<ComplexType> { ComplexTypeInstance } })
            .AddTypeAndValues(new GenericListImpl<IList<ComplexType>> { new List<ComplexType> { ComplexTypeInstance } })
            .AddTypeAndValues<IList<ComplexType>[]>(new[] { new List<ComplexType> { ComplexTypeInstance } })
            .AddTypeAndValues(new ArrayList { new ArrayList { ComplexTypeInstance } })
            .AddTypeAndValues<IEnumerable>(new ArrayList { new ArrayList { ComplexTypeInstance } })
            .AddTypeAndValues(new EnumerableImpl((object)new ArrayList { ComplexTypeInstance }))
            .AddTypeAndValues(new GenericEnumerableImpl<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance}))
            .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeArrayOfArrays))]
        public void should_serialize_array_of_arrays(Type type, object list)
        {
            Serialize.Json(list, type).ShouldEqual("[[{\"Property\":\"hai\"}]]");
        }

        // Dictionary types

        private static readonly object[] ComplexTypeDictionaries = TestCases.Create()
            .AddTypeAndValues<Hashtable[]>(new[] { new Hashtable
                { { "item", ComplexTypeInstance } } })
            .AddTypeAndValues<Dictionary<string, ComplexType>[]>(new[] { new Dictionary<string, ComplexType> 
                { { "item", ComplexTypeInstance } } })
            .AddTypeAndValues<IEnumerable<Dictionary<string, ComplexType>>>(new List<Dictionary<string, ComplexType>> 
                { new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .AddTypeAndValues(new List<Dictionary<string, ComplexType>> { new Dictionary<string, ComplexType> 
                { { "item", ComplexTypeInstance } } })
            .AddTypeAndValues<IList<Dictionary<string, ComplexType>>>(new List<Dictionary<string, ComplexType>> 
                { new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .AddTypeAndValues(new GenericListImpl<Dictionary<string, ComplexType>> 
                { new Dictionary<string, ComplexType> { { "item", ComplexTypeInstance } } })
            .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeDictionaries))]
        public void should_serialize_array_of_dictionary(Type type, object list)
        {
            Serialize.Json(list, type).ShouldEqual("[{\"item\":{\"Property\":\"hai\"}}]");
        }

        // Array member

        public class Model
        {
            public ArrayList NonGenericArray { get; set; }
            public IEnumerable EnumerableInterface { get; set; }
            public EnumerableImpl EnumerableImpl { get; set; }
            public GenericEnumerableImpl<ComplexType> ComplexEnumerableImpl { get; set; }

            public ComplexType[] ComplexArray { get; set; }
            public IEnumerable<ComplexType> ComplexEnumerableInterface { get; set; }
            public List<ComplexType> ComplexList { get; set; }
            public IList<ComplexType> ComplexListInterface { get; set; }
            public GenericListImpl<ComplexType> ComplexListImpl { get; set; }
        }

        private static readonly object[] ComplexArrayMembers = TestCases.Create()
            .Add("ComplexArray", new Model { ComplexArray = new ComplexType[] { ComplexTypeInstance } })
            .Add("ComplexEnumerableInterface", new Model { ComplexEnumerableInterface = new List<ComplexType> { ComplexTypeInstance } })
            .Add("ComplexList", new Model { ComplexList = new List<ComplexType> { ComplexTypeInstance } })
            .Add("ComplexListInterface", new Model { ComplexListInterface = new ComplexType[] { ComplexTypeInstance } })
            .Add("ComplexListImpl", new Model { ComplexListImpl = new GenericListImpl<ComplexType> { ComplexTypeInstance } })
            .Add("NonGenericArray", new Model { NonGenericArray = new ArrayList { ComplexTypeInstance } })
            .Add("EnumerableInterface", new Model { EnumerableInterface = new ArrayList { ComplexTypeInstance } })
            .Add("EnumerableImpl", new Model { EnumerableImpl = new EnumerableImpl(ComplexTypeInstance) })
            .Add("ComplexEnumerableImpl", new Model { ComplexEnumerableImpl = new GenericEnumerableImpl<ComplexType>(ComplexTypeInstance) })
            .All;

        [Test]
        [TestCaseSource(nameof(ComplexArrayMembers))]
        public void should_serialize_member_array_of_complex_type(string name, Model model)
        {
            Serialize.Json(model).ShouldEqual("{{\"{0}\":[{{\"Property\":\"hai\"}}]}}".ToFormat(name));
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
            Serialize.Json(new List<ActualType> { new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>))
                .ShouldEqual("[{\"Specified\":\"hai\"}]");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Json(new List<ActualType> { new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("[{\"Actual\":\"oh\",\"Specified\":\"hai\"}]");
        }

        public class MemberSpecifiedType
        {
            public List<ISpecifiedType> Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new List<ISpecifiedType> 
                { new ActualType { Actual = "oh", Specified = "hai" } } },
                typeof(MemberSpecifiedType))
                .ShouldEqual("{\"Specified\":[{\"Specified\":\"hai\"}]}");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new List<ISpecifiedType> 
                { new ActualType { Actual = "oh", Specified = "hai" } } },
                typeof(MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("{\"Specified\":[{\"Actual\":\"oh\",\"Specified\":\"hai\"}]}");
        }

        // Enumerable vs object handling

        public class EnumerableMember
        {
            public EnumerableImplementation EnumerableImpl { get; set; }
        }

        public class EnumerableImplementation : GenericListImpl<string>
        {
            public string Property { get; set; }
            public string Field;
        }

        [Test]
        public void should_not_treat_enumerable_root_as_object_by_default()
        {
            Serialize.Json(new EnumerableImplementation { "oh", "hai" })
                .ShouldEqual("[\"oh\",\"hai\"]");
        }

        [Test]
        public void should_treat_enumerable_root_as_object_when_configured()
        {
            Serialize.Json(new EnumerableImplementation { Property = "oh", Field = "hai" },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"Property\":\"oh\",\"Field\":\"hai\"}");
        }

        [Test]
        public void should_not_treat_enumerable_member_as_object_by_default()
        {
            Serialize.Json(new EnumerableMember { EnumerableImpl = new EnumerableImplementation { "oh", "hai" } })
                .ShouldEqual("{\"EnumerableImpl\":[\"oh\",\"hai\"]}");
        }

        [Test]
        public void should_treat_enumerable_member_as_object_when_configured()
        {
            Serialize.Json(new EnumerableMember { EnumerableImpl = new EnumerableImplementation { Property = "oh", Field = "hai" } },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"EnumerableImpl\":{\"Property\":\"oh\",\"Field\":\"hai\"}}");
        }

        [Test]
        public void should_not_treat_enumerable_array_item_as_object_by_default()
        {
            Serialize.Json(new List<EnumerableImplementation> { new EnumerableImplementation { "oh", "hai" }})
                .ShouldEqual("[[\"oh\",\"hai\"]]");
        }

        [Test]
        public void should_treat_enumerable_array_item_as_object_when_configured()
        {
            Serialize.Json(new List<EnumerableImplementation> { new EnumerableImplementation { Property = "oh", Field = "hai" }},
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual("[{\"Property\":\"oh\",\"Field\":\"hai\"}]");
        }

        [Test]
        public void should_not_treat_enumerable_dictionary_entry_as_object_by_default()
        {
            Serialize.Json(new Dictionary<string, EnumerableImplementation> { { "item", 
                    new EnumerableImplementation { "oh", "hai" } } })
                .ShouldEqual("{\"item\":[\"oh\",\"hai\"]}");
        }

        [Test]
        public void should_treat_enumerable_dictionary_entry_as_object_when_configured()
        {
            Serialize.Json(new Dictionary<string, EnumerableImplementation> { { "item", 
                    new EnumerableImplementation { Property = "oh", Field = "hai" } } },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual("{\"item\":{\"Property\":\"oh\",\"Field\":\"hai\"}}");
        }

        // Type filtering

        [Test]
        public void should_not_include_types_when_does_not_match()
        {
            Serialize.Json<IList<ComplexType>>(new List<ComplexType> { new ComplexType() },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType2"))
                .ShouldEqual("[]");
        }

        [Test]
        public void should_include_types_when_matches()
        {
            Serialize.Json<IList<ComplexType>>(new List<ComplexType> { new ComplexType() },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual("[{}]");
        }

        [Test]
        public void should_filter_types()
        {
            Serialize.Json<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance },
                x => x.ExcludeType<ComplexType>())
                .ShouldEqual("[]");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Json<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance },
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual("[]");
        }

        // Circular references

        public class CircularReference
        {
            public CircularReferenceNode Value { get; set; }
        }

        public class CircularReferenceNode
        {
            public string Value1 { get; set; }
            public List<CircularReference> Value2 { get; set; }
        }

        [Test]
        public void should_not_allow_circular_references()
        {
            var graph = new CircularReference { Value = new CircularReferenceNode { Value1 = "hai" } };
            graph.Value.Value2 = new List<CircularReference> { graph };
            Serialize.Json(graph).ShouldEqual("{\"Value\":{\"Value1\":\"hai\",\"Value2\":[]}}");
        }
    }
}
