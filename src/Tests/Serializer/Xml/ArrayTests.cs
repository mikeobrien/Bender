using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bender;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.Xml
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
        public void should_serialize_typed_array_elements(Type type, object value)
        {
            var list = type.MakeGenericListType().CreateInstance().As<IList>();
            list.Add(value);
            Serialize.Xml(list).ShouldEqual(Xml.Declaration + "<ArrayOfAnyType><{0}>{1}</{0}></ArrayOfAnyType>"
                .ToFormat(type.GetUnderlyingNullableType().Name,
                type.GetUnderlyingNullableType() == typeof(bool) ? value.ToString().ToLower() : value));
        }

        [Test]
        [TestCaseSource(nameof(SimpleArrayTypes))]
        public void should_serialize_typed_array_attributes(Type type, object value)
        {
            var list = type.MakeGenericListType().CreateInstance().As<IList>();
            list.Add(value);
            Serialize.Xml(list, x => x.Serialization(y => y.XmlValuesAsAttributes()))
                .ShouldEqual(Xml.Declaration + "<ArrayOfAnyType {0}=\"{1}\" />"
                    .ToFormat(type.GetUnderlyingNullableType().Name,
                type.GetUnderlyingNullableType() == typeof(bool) ? value.ToString().ToLower() : value));
        }

        // Complex types

        public class ComplexType
        {
            public string Property {get; set; }
        }

        private readonly static ComplexType ComplexTypeInstance = new ComplexType { Property = "hai" };

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
            Serialize.Xml(list, type).ShouldEqual(Xml.Declaration + "<ArrayOf{0}><ComplexType><Property>hai</Property></ComplexType></ArrayOf{0}>"
                .ToFormat(type.IsGenericEnumerable() ? "ComplexType" : "AnyType"));
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
            Serialize.Xml(list, type).ShouldEqual(Xml.Declaration + "<ArrayOf{0}{1}><ArrayOf{1}><ComplexType><Property>hai</Property></ComplexType></ArrayOf{1}></ArrayOf{0}{1}>"
                .ToFormat(type.IsGenericEnumerable() ? "ArrayOf" : "", type.IsGenericEnumerable() && 
                          type.GetGenericEnumerableType().IsGenericEnumerable() ? "ComplexType" : "AnyType"));
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
            Serialize.Xml(list, type).ShouldEqual(Xml.Declaration + "<ArrayOfDictionaryOf{0}><DictionaryOf{0}><item><Property>hai</Property></item></DictionaryOf{0}></ArrayOfDictionaryOf{0}>"
                .ToFormat(type.GetGenericEnumerableType().IsGenericEnumerable() ? "ComplexType" : "AnyType"));
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
            Serialize.Xml(model).ShouldEqual(Xml.Declaration + "<Model><{0}><ComplexType><Property>hai</Property></ComplexType></{0}></Model>".ToFormat(name));
        }

        // Array sibling items

        public class ArraySibling
        {
            public string Property { get; set; }
            [XmlArrayItem("SiblingProperty"), XmlSiblings]
            public List<string> SimpleTypeSiblings { get; set; }
            [XmlArrayItem("Sibling"), XmlSiblings]
            public List<ArraySibling> ObjectSiblings { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_array_items_as_siblings_when_xml_sibling_attribute_applied()
        {
            Serialize.Xml(new ArraySibling
            {
                Property = "property",
                SimpleTypeSiblings = new List<string>
                {
                    "sibling property 1",
                    "sibling property 2"
                }
            }).ShouldEqual(Xml.Declaration +
                "<ArraySibling>" +
                    "<Property>property</Property>" +
                    "<SiblingProperty>sibling property 1</SiblingProperty>" +
                    "<SiblingProperty>sibling property 2</SiblingProperty>" +
                "</ArraySibling>");
        }

        [Test]
        public void should_serialize_object_array_items_as_siblings_when_xml_sibling_attribute_applied()
        {
            Serialize.Xml(new ArraySibling
            {
                Property = "property",
                ObjectSiblings = new List<ArraySibling>
                {
                    new ArraySibling
                    {
                        Property = "sibling property 1",
                        ObjectSiblings = new List<ArraySibling>
                        {
                            new ArraySibling { Property = "sibling property 1a" },
                            new ArraySibling { Property = "sibling property 1b" }
                        }
                    },
                    new ArraySibling
                    {
                        Property = "sibling property 2",
                        ObjectSiblings = new List<ArraySibling>
                        {
                            new ArraySibling { Property = "sibling property 2a" },
                            new ArraySibling { Property = "sibling property 2b" }
                        }
                    }
                }
            }).ShouldEqual(Xml.Declaration +
                "<ArraySibling>" +
                    "<Property>property</Property>" +
                    "<Sibling>" +
                        "<Property>sibling property 1</Property>" +
                        "<Sibling>" +
                            "<Property>sibling property 1a</Property>" +
                        "</Sibling>" +
                        "<Sibling>" +
                            "<Property>sibling property 1b</Property>" +
                        "</Sibling>" +
                    "</Sibling>" +
                    "<Sibling>" +
                        "<Property>sibling property 2</Property>" +
                        "<Sibling>" +
                            "<Property>sibling property 2a</Property>" +
                        "</Sibling>" +
                        "<Sibling>" +
                            "<Property>sibling property 2b</Property>" +
                        "</Sibling>" +
                    "</Sibling>" +
                "</ArraySibling>");
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
            Serialize.Xml(new List<ActualType> { new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>))
                .ShouldEqual(Xml.Declaration + "<ArrayOfISpecifiedType><ISpecifiedType><Specified>hai</Specified></ISpecifiedType></ArrayOfISpecifiedType>");
        }
        
        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Xml(new List<ActualType> { new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration + "<ArrayOfISpecifiedType><ActualType><Actual>oh</Actual><Specified>hai</Specified></ActualType></ArrayOfISpecifiedType>");
        }

        public class MemberSpecifiedType
        {
            public List<ISpecifiedType> Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Xml(new MemberSpecifiedType { Specified = new List<ISpecifiedType> 
                { new ActualType { Actual = "oh", Specified = "hai" } } },
                typeof(MemberSpecifiedType))
                .ShouldEqual(Xml.Declaration + "<MemberSpecifiedType><Specified><ISpecifiedType><Specified>hai</Specified></ISpecifiedType></Specified></MemberSpecifiedType>");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Xml(new MemberSpecifiedType { Specified = new List<ISpecifiedType> 
                { new ActualType { Actual = "oh", Specified = "hai" } } },
                typeof(MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration + "<MemberSpecifiedType><Specified><ActualType><Actual>oh</Actual><Specified>hai</Specified></ActualType></Specified></MemberSpecifiedType>");
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
            Serialize.Xml(new EnumerableImplementation { "oh", "hai" })
                .ShouldEqual(Xml.Declaration + "<ArrayOfString><String>oh</String><String>hai</String></ArrayOfString>");
        }

        [Test]
        public void should_treat_enumerable_root_as_object_when_configured()
        {
            Serialize.Xml(new EnumerableImplementation { Property = "oh", Field = "hai" },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<EnumerableImplementation><Property>oh</Property><Field>hai</Field></EnumerableImplementation>");
        }

        [Test]
        public void should_not_treat_enumerable_member_as_object_by_default()
        {
            Serialize.Xml(new EnumerableMember { EnumerableImpl = new EnumerableImplementation { "oh", "hai" } })
                .ShouldEqual(Xml.Declaration + "<EnumerableMember><EnumerableImpl><String>oh</String><String>hai</String></EnumerableImpl></EnumerableMember>");
        }

        [Test]
        public void should_treat_enumerable_member_as_object_when_configured()
        {
            Serialize.Xml(new EnumerableMember { EnumerableImpl = new EnumerableImplementation { Property = "oh", Field = "hai" } },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<EnumerableMember><EnumerableImpl><Property>oh</Property><Field>hai</Field></EnumerableImpl></EnumerableMember>");
        }

        [Test]
        public void should_not_treat_enumerable_array_item_as_object_by_default()
        {
            Serialize.Xml(new List<EnumerableImplementation> { new EnumerableImplementation { "oh", "hai" }})
                .ShouldEqual(Xml.Declaration + "<ArrayOfArrayOfString><ArrayOfString><String>oh</String><String>hai</String></ArrayOfString></ArrayOfArrayOfString>");
        }

        [Test]
        public void should_treat_enumerable_array_item_as_object_when_configured()
        {
            Serialize.Xml(new List<EnumerableImplementation> { new EnumerableImplementation { Property = "oh", Field = "hai" }},
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<ArrayOfEnumerableImplementation><EnumerableImplementation><Property>oh</Property><Field>hai</Field></EnumerableImplementation></ArrayOfEnumerableImplementation>");
        }

        [Test]
        public void should_not_treat_enumerable_dictionary_entry_as_object_by_default()
        {
            Serialize.Xml(new Dictionary<string, EnumerableImplementation> { { "item", 
                    new EnumerableImplementation { "oh", "hai" } } })
                .ShouldEqual(Xml.Declaration + "<DictionaryOfArrayOfString><item><String>oh</String><String>hai</String></item></DictionaryOfArrayOfString>");
        }

        [Test]
        public void should_treat_enumerable_dictionary_entry_as_object_when_configured()
        {
            Serialize.Xml(new Dictionary<string, EnumerableImplementation> { { "item", 
                    new EnumerableImplementation { Property = "oh", Field = "hai" } } },
                x => x.TreatEnumerableImplsAsObjects().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<DictionaryOfEnumerableImplementation><item><Property>oh</Property><Field>hai</Field></item></DictionaryOfEnumerableImplementation>");
        }

        // Type filtering

        [Test]
        public void should_not_include_types_when_does_not_match()
        {
            Serialize.Xml<IList<ComplexType>>(new List<ComplexType> { new ComplexType() },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType2"))
                .ShouldEqual(Xml.Declaration + "<ArrayOfComplexType />");
        }

        [Test]
        public void should_include_types_when_matches()
        {
            Serialize.Xml<IList<ComplexType>>(new List<ComplexType> { new ComplexType() },
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual(Xml.Declaration + "<ArrayOfComplexType><ComplexType /></ArrayOfComplexType>");
        }

        [Test]
        public void should_filter_types()
        {
            Serialize.Xml<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance },
                x => x.ExcludeType<ComplexType>())
                .ShouldEqual(Xml.Declaration + "<ArrayOfComplexType />");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Xml<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance },
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType"))
                .ShouldEqual(Xml.Declaration + "<ArrayOfComplexType />");
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
            Serialize.Xml(graph).ShouldEqual(Xml.Declaration + "<CircularReference><Value><Value1>hai</Value1><Value2 /></Value></CircularReference>");
        }

        // Namespaces

        [Test]
        public void should_set_default_namespace()
        {
            Serialize.Xml(new List<string> { "hai" },
                x => x.Serialization(y => y.WithDefaultXmlNamespace("urn:yada")))
                .ShouldEqual(Xml.Declaration + "<ArrayOfString xmlns=\"urn:yada\"><String>hai</String></ArrayOfString>");
        }

        [Test]
        public void should_add_namespace_prefix()
        {
            Serialize.Xml(new List<string> { "hai" },
                x => x.Serialization(y => y.AddXmlNamespace("abc", "urn:yada").AddXmlVisitor<string>((s, t, o) => t.SetNamespacePrefix("abc"))))
                .ShouldEqual(Xml.Declaration + "<ArrayOfString xmlns:abc=\"urn:yada\"><abc:String>hai</abc:String></ArrayOfString>");
        }

        // Naming conventions

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Serialize.Xml(new List<string>()).ShouldEqual(Xml.Declaration + "<ArrayOfString />");
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Serialize.Xml(new List<string>(),
                x => x.WithEnumerableTypeNameFormat("Collection_{0}")).ShouldEqual(Xml.Declaration + "<Collection_String />");
        }

        [Test]
        public void should_use_xml_serializer_non_generic_item_type_name_format_by_default()
        {
            Serialize.Xml(new ArrayList()).ShouldEqual(Xml.Declaration + "<ArrayOfAnyType />");
        }

        [Test]
        public void should_use_configured_non_generic_item_type_name_format()
        {
            Serialize.Xml(new ArrayList(),
                x => x.WithDefaultItemTypeName("Yada")).ShouldEqual(Xml.Declaration + "<ArrayOfYada />");
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot : List<string> { }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlRoot()).ShouldEqual(Xml.Declaration + "<Root />");
        }

        [XmlType("Type")]
        public class XmlType : List<string> { }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlType()).ShouldEqual(Xml.Declaration + "<Type />");
        }

        public class ArrayItemName
        {
            [XmlArrayItem("Item")]
            public List<string> Items { get; set; }
        }

        [Test]
        public void should_override_item_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new ArrayItemName { Items = new List<string> { "hai"} })
                .ShouldEqual(Xml.Declaration + "<ArrayItemName><Items><Item>hai</Item></Items></ArrayItemName>");
        }
    }
}
