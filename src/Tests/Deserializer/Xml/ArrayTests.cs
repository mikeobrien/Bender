using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class ArrayTests
    {
        // Simple types 

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleArrayTypes = TestCases.Create()
            .AddTypeAndValues<string>("1", null)
            .AddTypeAndValues<Uri>(new Uri("http://www.xkcd.com"), null)

            .AddTypeAndValues<UriFormat>(UriFormat.UriEscaped, UriFormat.UriEscaped)
            .AddTypeAndValues<UriFormat?>(UriFormat.UriEscaped, null)

            .AddTypeAndValues<DateTime>(DateTime.Today, DateTime.MinValue).AddTypeAndValues<DateTime?>(DateTime.Today, null)
            .AddTypeAndValues<TimeSpan>(TimeSpan.MaxValue, TimeSpan.Zero).AddTypeAndValues<TimeSpan?>(TimeSpan.MaxValue, null)
            .AddTypeAndValues<Guid>(RandomGuid, Guid.Empty).AddTypeAndValues<Guid?>(RandomGuid, null)

            .AddTypeAndValues<Boolean>(true, false).AddTypeAndValues<Boolean?>(true, null)
            .AddTypeAndValues<Byte>(5, 0).AddTypeAndValues<Byte?>(55, null)
            .AddTypeAndValues<SByte>(6, 0).AddTypeAndValues<SByte?>(66, null)
            .AddTypeAndValues<Int16>(7, 0).AddTypeAndValues<Int16?>(77, null)
            .AddTypeAndValues<UInt16>(8, 0).AddTypeAndValues<UInt16?>(88, null)
            .AddTypeAndValues<Int32>(9, 0).AddTypeAndValues<Int32?>(99, null)
            .AddTypeAndValues<UInt32>(10, 0).AddTypeAndValues<UInt32?>(110, null)
            .AddTypeAndValues<Int64>(11, 0).AddTypeAndValues<Int64?>(111, null)
            .AddTypeAndValues<UInt64>(12, 0).AddTypeAndValues<UInt64?>(120, null)
            .AddTypeAndValues<IntPtr>(new IntPtr(13), IntPtr.Zero).AddTypeAndValues<IntPtr?>(new IntPtr(130), null)
            .AddTypeAndValues<UIntPtr>(new UIntPtr(14), UIntPtr.Zero).AddTypeAndValues<UIntPtr?>(new UIntPtr(140), null)
            .AddTypeAndValues<Char>('a', Char.MinValue).AddTypeAndValues<Char?>('b', null)
            .AddTypeAndValues<Double>(15, 0).AddTypeAndValues<Double?>(150, null)
            .AddTypeAndValues<Single>(16, 0).AddTypeAndValues<Single?>(160, null)
            .AddTypeAndValues<Decimal>(17, Decimal.MinValue).AddTypeAndValues<Decimal?>(170, null)

            .All;

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_deserialize_typed_elements(Type type, object value, object defaultValue)
        {
            var xml = "<ArrayOf{0}><{0}>{1}</{0}></ArrayOf{0}>".ToFormat(type.GetUnderlyingNullableType().Name, value);

            var listType = type.MakeGenericListType();
            var result = Deserialize.Xml(xml, listType).As<IList>();

            result.ShouldNotBeNull();
            result.ShouldBeType(listType);
            result.Count.ShouldEqual(1);
            result[0].ShouldEqual(value);
        }

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_deserialize_typed_attributes(Type type, object value, object defaultValue)
        {
            var xml = "<ArrayOf{0} {0}=\"{1}\" />".ToFormat(type.GetUnderlyingNullableType().Name, value);

            var listType = type.MakeGenericListType();
            var result = Deserialize.Xml(xml, listType).As<IList>();

            result.ShouldNotBeNull();
            result.ShouldBeType(listType);
            result.Count.ShouldEqual(1);
            result[0].ShouldEqual(value);
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Uri))]
        [TestCase(typeof(UriFormat?))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid?))]
        [TestCase(typeof(Boolean?))]
        [TestCase(typeof(Byte?))]
        [TestCase(typeof(SByte?))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64?))]
        [TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(Char?))]
        [TestCase(typeof(Double?))]
        [TestCase(typeof(Single?))]
        [TestCase(typeof(Decimal?))]
        public void should_deserialize_empty_reference_type_array_items(Type type)
        {
            var listType = type.MakeGenericListType();
            var xml = "<ArrayOf{0}><{0}/></ArrayOf{0}>".ToFormat(type.GetUnderlyingNullableType().Name);
            var result = Deserialize.Xml(xml, listType).As<IList>();

            result.ShouldNotBeNull();
            result.ShouldBeType(listType);
            result.Count.ShouldEqual(1);
            result[0].ShouldBeNull();
        }

        [Test]
        [TestCase(typeof(UriFormat))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(Boolean))]
        [TestCase(typeof(Byte))]
        [TestCase(typeof(SByte))]
        [TestCase(typeof(Int16))]
        [TestCase(typeof(UInt16))]
        [TestCase(typeof(Int32))]
        [TestCase(typeof(UInt32))]
        [TestCase(typeof(Int64))]
        [TestCase(typeof(UInt64))]
        [TestCase(typeof(IntPtr))]
        [TestCase(typeof(UIntPtr))]
        [TestCase(typeof(Char))]
        [TestCase(typeof(Double))]
        [TestCase(typeof(Single))]
        [TestCase(typeof(Decimal))]
        public void should_fail_to_deserialize_empty_value_type_array_items(Type type)
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<ArrayOf{0}><{0}/></ArrayOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericListType()));

            exception.Message.ShouldEqual(("Error deserializing xml element '/ArrayOf{0}/{0}' to 'System.Collections." +
                "Generic.List<{1}>[]': Value cannot be null.").ToFormat(type.Name, type.FullName));
            exception.FriendlyMessage.ShouldEqual(("Could not read xml element " +
                "'/ArrayOf{0}/{0}': Value cannot be null.").ToFormat(type.Name));
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_fail_to_parse_empty_array_items(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<ArrayOf{0}><{0}></{0}></ArrayOf{0}>"
                    .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericListType()));

            exception.Message.ShouldStartWith(("Error deserializing xml element '/ArrayOf{0}/{0}' to 'System.Collections." +
                "Generic.List<{1}>[]': Error parsing ''. ").ToFormat(type.GetUnderlyingNullableType().Name, type.GetFriendlyTypeFullName()));

            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/ArrayOf{0}/{0}': ".ToFormat(type.GetUnderlyingNullableType().Name) +
                Options.Create().Deserialization.FriendlyParseErrorMessages[messageType].ToFormat(""));

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_fail_to_parse_empty_array_items_with_custom_parse_message(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage(messageType, "yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<ArrayOf{0}><{0}></{0}></ArrayOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericListType(), options));

            exception.Message.ShouldStartWith(("Error deserializing xml element '/ArrayOf{0}/{0}' to 'System.Collections." +
                "Generic.List<{1}>[]': Error parsing ''. ").ToFormat(type.GetUnderlyingNullableType().Name, type.GetFriendlyTypeFullName()));
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/ArrayOf{0}/{0}': yada".ToFormat(type.GetUnderlyingNullableType().Name));
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_array_items_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Xml("<ArrayOfInt32><Int32></Int32></ArrayOfInt32>", typeof(List<int>), options));

            exception.Message.ShouldEqual("Error deserializing xml element '/ArrayOfInt32/Int32' to 'System.Collections." +
                "Generic.List<System.Int32>[]': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/ArrayOfInt32/Int32': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        // Complex types

        public class ComplexType
        {
            public string Property { get; set; }
        }

        [Test]
        [TestCase(typeof(ComplexType[]))]
        [TestCase(typeof(IEnumerable<ComplexType>))]
        [TestCase(typeof(List<ComplexType>))]
        [TestCase(typeof(IList<ComplexType>))]
        [TestCase(typeof(GenericListImpl<ComplexType>))]
        public void should_deserialize_array_of_complex_type(Type type)
        {
            var xml = "<ArrayOfComplexType><ComplexType><Property>hai</Property></ComplexType></ArrayOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IList<ComplexType>>();

            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);
            result[0].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(EnumerableImpl))]
        public void should_fail_to_deserialize_non_generic_arrays_of_complex_type(Type type)
        {
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Xml("<ArrayOfAnyType><ComplexType><Property>hai</Property></ComplexType></ArrayOfAnyType>", type));
            
            exception.Message.ShouldEqual(("Error deserializing xml element '/ArrayOfAnyType/ComplexType' to '{0}': " +
                "Non generic {1} '{0}' is not supported for deserialization. Only generic lists and generic enumerable " +
                "interfaces can be deserialized.").ToFormat(type.GetFriendlyTypeFullName(), type.IsList() ? "list" : "enumerable"));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_generic_enumerable_impls_of_complex_type()
        {
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Xml<GenericEnumerableImpl<ComplexType>>(
                    "<ArrayOfComplexType><ComplexType><Property>hai</Property></ComplexType></ArrayOfComplexType>"));

            exception.Message.ShouldEqual("Error deserializing xml element '/ArrayOfComplexType/ComplexType' to " +
                "'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.Xml.ArrayTests.ComplexType>': " +
                "Enumerable 'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.Xml.ArrayTests.ComplexType>' " +
                "is not supported for deserialization. Only generic lists and generic enumerable interfaces can be deserialized.");
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        // Array types

        [Test]
        [TestCase(typeof(ComplexType[][]))]
        [TestCase(typeof(IEnumerable<IList<ComplexType>>))]
        [TestCase(typeof(List<IList<ComplexType>>))]
        [TestCase(typeof(IList<IList<ComplexType>>))]
        [TestCase(typeof(GenericListImpl<IList<ComplexType>>))]
        public void should_deserialize_array_of_arrays(Type type)
        {
            var xml = "<ArrayOfArrayOfComplexType><ArrayOfComplexType><ComplexType><Property>hai</Property></ComplexType></ArrayOfComplexType></ArrayOfArrayOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IList<IList<ComplexType>>>();

            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);
            result[0][0].Property.ShouldEqual("hai");
        }

        // Dictionary types

        [Test]
        [TestCase(typeof(Dictionary<string, ComplexType>[]))]
        [TestCase(typeof(IEnumerable<Dictionary<string, ComplexType>>))]
        [TestCase(typeof(List<Dictionary<string, ComplexType>>))]
        [TestCase(typeof(IList<Dictionary<string, ComplexType>>))]
        [TestCase(typeof(GenericListImpl<Dictionary<string, ComplexType>>))]
        public void should_deserialize_array_of_dictionary(Type type)
        {
            var xml = "<ArrayOfDictionaryOfComplexType><DictionaryOfComplexType><item><Property>hai</Property></item></DictionaryOfComplexType></ArrayOfDictionaryOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IList<Dictionary<string, ComplexType>>>();

            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);
            result[0]["item"].Property.ShouldEqual("hai");
        }

        // Array member

        public class RootType
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

        [Test]
        [TestCase("ComplexArray")]
        [TestCase("ComplexEnumerableInterface")]
        [TestCase("ComplexList")]
        [TestCase("ComplexListInterface")]
        [TestCase("ComplexListImpl")]
        public void should_deserialize_member_array_of_complex_type(string name)
        {
            var xml = "<RootType><{0}><ComplexType><Property>hai</Property></ComplexType></{0}></RootType>".ToFormat(name);

            var result = Deserialize.Xml<RootType>(xml)
                .GetPropertyOrFieldValue(name).As<IList<ComplexType>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result[0].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase("NonGenericArray")]
        [TestCase("EnumerableInterface")]
        [TestCase("EnumerableImpl")]
        public void should_fail_to_deserialize_member_non_generic_arrays(string name)
        {
            var xml = "<RootType><{0}><Object /></{0}></RootType>".ToFormat(name);
            var exception =Assert.Throws<MappingException>(() =>
                Deserialize.Xml<RootType>(xml));

            var type = typeof (RootType).GetProperty(name).PropertyType;
            exception.Message.ShouldEqual(("Error deserializing xml element '/RootType/{0}/Object' to " +
                "'Tests.Deserializer.Xml.ArrayTests.RootType.{0}': Non generic {1} '{2}' is not supported for " +
                "deserialization. Only generic lists and generic enumerable interfaces can be deserialized.")
                .ToFormat(name, type.IsList() ? "list" : "enumerable", type.GetFriendlyTypeFullName()));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_member_generic_enumerable_impls()
        {
            var xml = "<RootType><ComplexEnumerableImpl><Object /></ComplexEnumerableImpl></RootType>";
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Xml<RootType>(xml));

            exception.Message.ShouldEqual("Error deserializing xml element " +
                "'/RootType/ComplexEnumerableImpl/Object' to 'Tests.Deserializer.Xml.ArrayTests.RootType.ComplexEnumerableImpl': " +
                "Enumerable 'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.Xml.ArrayTests.ComplexType>' " +
                "is not supported for deserialization. Only generic lists and generic enumerable interfaces can be deserialized.");
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_deserialize_empty_member_array_of_complex_type()
        {
            var xml = "<RootType><ComplexArray /></RootType>";

            var result = Deserialize.Xml<RootType>(xml);

            result.ShouldNotBeNull();
            result.ComplexArray.ShouldNotBeNull();
            result.ComplexArray.ShouldBeEmpty();
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
            var result = Deserialize.Xml<EnumerableImplementation>("<ArrayOfString><String>oh</String><String>hai</String></ArrayOfString>").ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_root_as_object_when_configured()
        {
            var result = Deserialize.Xml<EnumerableImplementation>(
                "<EnumerableImplementation><Property>oh</Property><Field>hai</Field></EnumerableImplementation>",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields());

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_member_as_object_by_default()
        {
            var result = Deserialize.Xml<EnumerableMember>("<EnumerableMember><EnumerableImpl><String>oh</String><String>hai</String></EnumerableImpl></EnumerableMember>").EnumerableImpl.ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_member_as_object_when_configured()
        {
            var result = Deserialize.Xml<EnumerableMember>(
                "<EnumerableMember><EnumerableImpl><Property>oh</Property><Field>hai</Field></EnumerableImpl></EnumerableMember>",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields()).EnumerableImpl;

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_array_item_as_object_by_default()
        {
            var result = Deserialize.Xml<List<EnumerableImplementation>>("<ArrayOfArrayOfString><ArrayOfString><String>oh</String><String>hai</String></ArrayOfString></ArrayOfArrayOfString>").First().ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_array_item_as_object_when_configured()
        {
            var result = Deserialize.Xml<List<EnumerableImplementation>>(
                "<ArrayOfEnumerableImplementation><EnumerableImplementation><Property>oh</Property><Field>hai</Field></EnumerableImplementation></ArrayOfEnumerableImplementation>",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields()).First();

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_dictionary_entry_as_object_by_default()
        {
            var result = Deserialize.Xml<Dictionary<string, EnumerableImplementation>>(
                "<DictionaryOfArrayOfString><item><String>oh</String><String>hai</String></item></DictionaryOfArrayOfString>")["item"].ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_dictionary_entry_as_object_when_configured()
        {
            var result = Deserialize.Xml<Dictionary<string, EnumerableImplementation>>(
                "<DictionaryOfEnumerableImplementation><item><Property>oh</Property><Field>hai</Field></item></DictionaryOfEnumerableImplementation>",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields())["item"];

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        // Case sensitivity

        [Test]
        public void should_deserialize_case_insensitively()
        {
            var xml = "<ARRAYOFSTRING><STRING>hai</STRING></ARRAYOFSTRING>";
            var result = Deserialize.Xml<List<string>>(xml, x => x.Deserialization(y => y.IgnoreNameCase()));
            result[0].ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_with_case_comparison()
        {
            var xml = "<ARRAYOFSTRING><STRING>hai</STRING></ARRAYOFSTRING>";
            var result = Deserialize.Xml<List<string>>(xml, x => x.Deserialization(y => y
                    .WithNameComparison(StringComparison.CurrentCultureIgnoreCase)));
            result[0].ShouldEqual("hai");
        }

        [Test]
        public void should_not_deserialize_case_insensitively_by_default()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(() => Deserialize
                .Xml<List<string>>("<ARRAYOFSTRING />", x => x.IncludePublicFields()));

            exception.Message.ShouldEqual("Xml root name 'ARRAYOFSTRING' does not match expected " +
                                          "name of 'ArrayOfString'. Deserializing " +
                                          "'System.Collections.Generic.List<System.String>'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'ARRAYOFSTRING' does not " +
                                                  "match expected name of 'ArrayOfString'.");
        }

        // Filtering attributes

        private const string FilterAttributesXml = "<ArrayOfString Attribute=\"oh\"><String>hai</String></ArrayOfString>";

        [Test]
        public void should_include_attributes_by_default()
        {
            var exception = Assert.Throws<FriendlyMappingException>(
                () => Deserialize.Xml<List<string>>(FilterAttributesXml));

            exception.Message.ShouldEqual("Error deserializing xml attribute '/ArrayOfString/@Attribute' to " +
                "'System.Collections.Generic.List<System.String>': Name 'Attribute' does not match expected name of 'String'.");

            exception.FriendlyMessage.ShouldEqual("Could not read xml attribute '/ArrayOfString/@Attribute': " +
                                          "Name 'Attribute' does not match expected name of 'String'.");

            exception.InnerException.ShouldBeType<InvalidItemNameDeserializationException>();
        }

        [Test]
        public void should_ignore_attributes_when_configured()
        {
            var result = Deserialize.Xml<List<string>>(FilterAttributesXml, x => x.Deserialization(y => y.IgnoreXmlAttributes()));
            result[0].ShouldEqual("hai");
        }

        // Type filtering

        [Test]
        public void should_include_types_when()
        {
            Deserialize.Xml<IList<ComplexType>>(
                "<ArrayOfComplexType><ComplexType /></ArrayOfComplexType>", x => x
                .IncludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(1);
        }

        [Test]
        public void should_filter_types()
        {
            Deserialize.Xml<IList<ComplexType>>("<ArrayOfComplexType><ComplexType /></ArrayOfComplexType>",
                x => x.ExcludeType<ComplexType>()).ShouldTotal(0);
        }

        [Test]
        public void should_exclude_types_when()
        {
            Deserialize.Xml<IList<ComplexType>>(
                "<ArrayOfComplexType><ComplexType /></ArrayOfComplexType>", x => x
                .ExcludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(0);
        }

        // Name validation

        [Test]
        public void should_not_ignore_root_name_by_default()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(
                () => Deserialize.Xml<List<string>>("<sdfgsdfasdfasdfas />"));

            exception.Message.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not " +
                "match expected name of 'ArrayOfString'. Deserializing " +
                "'System.Collections.Generic.List<System.String>'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not " +
                "match expected name of 'ArrayOfString'.");
        }

        [Test]
        public void should_ignore_root_name_when_configured()
        {
            var result = Deserialize.Xml<List<string>>(
                "<sdfgsdfasdfasdfas><String>oh</String></sdfgsdfasdfasdfas>",
                x => x.Deserialization(y => y.IgnoreRootName()));

            result[0].ShouldEqual("oh");
        }

        [Test]
        public void should_not_ignore_item_name_by_default()
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Xml<List<string>>("<ArrayOfString><sdfgsdfasdfasdfas /></ArrayOfString>"));

            exception.Message.ShouldEqual("Error deserializing xml element '/ArrayOfString/sdfgsdfasdfasdfas'" + 
                " to 'System.Collections.Generic.List<System.String>': Name 'sdfgsdfasdfasdfas' does " + 
                "not match expected name of 'String'.");

            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/ArrayOfString/sdfgsdfasdfasdfas': " + 
                "Name 'sdfgsdfasdfasdfas' does not match expected name of 'String'.");

            exception.InnerException.ShouldBeType<InvalidItemNameDeserializationException>();
        }

        [Test]
        public void should_ignore_item_name_when_configured()
        {
            var result = Deserialize.Xml<List<string>>(
                "<ArrayOfString><sdfgsdfasdfasdfas>oh</sdfgsdfasdfasdfas></ArrayOfString>",
                x => x.Deserialization(y => y.IgnoreArrayItemNames()));

            result[0].ShouldEqual("oh");
        }

        // Naming conventions

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Deserialize.Xml<List<string>>("<ArrayOfString />").ShouldNotBeNull();
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Deserialize.Xml<List<string>>("<Collection_String />",
                x => x.WithEnumerableTypeNameFormat("Collection_{0}")).ShouldNotBeNull();
        }

        [Test]
        public void should_use_xml_serializer_non_generic_item_type_name_format_by_default()
        {
            Deserialize.Xml<ArrayList>("<ArrayOfAnyType />").ShouldNotBeNull();
        }

        [Test]
        public void should_use_configured_non_generic_item_type_name_format()
        {
            Deserialize.Xml<ArrayList>("<ArrayOfYada />",
                x => x.WithDefaultItemTypeName("Yada")).ShouldNotBeNull();
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot : List<string> { }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlRoot>("<Root />").ShouldNotBeNull();
        }

        [XmlType("Type")]
        public class XmlType : List<string> { }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlType>("<Type />").ShouldNotBeNull();
        }

        public class ArrayItemName
        {
            [XmlArrayItem("Item")]
            public List<string> Items { get; set; }
        }

        [Test]
        public void should_override_item_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<ArrayItemName>("<ArrayItemName><Items><Item>hai</Item></Items></ArrayItemName>")
                .Items[0].ShouldEqual("hai");
        }
    }
}
