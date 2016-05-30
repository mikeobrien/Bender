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
    public class DictionaryTests
    {
        // Simple types

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleDictionaryTypes = TestCases.Create()
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
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_deserialize_typed_dictionary_elements(Type type, object value, object defaultValue)
        {
            var xml = "<DictionaryOf{0}><item>{1}</item></DictionaryOf{0}>".ToFormat(type.GetUnderlyingNullableType().Name, value);

            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Xml(xml, dictionaryType).As<IDictionary>();

            result.ShouldNotBeNull();
            result.ShouldBeType(dictionaryType);
            result.Count.ShouldEqual(1);
            result["item"].ShouldEqual(value);
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_deserialize_typed_dictionary_attributes(Type type, object value, object defaultValue)
        {
            var xml = "<DictionaryOf{0} item=\"{1}\" />".ToFormat(type.GetUnderlyingNullableType().Name, value);

            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Xml(xml, dictionaryType).As<IDictionary>();

            result.ShouldNotBeNull();
            result.ShouldBeType(dictionaryType);
            result.Count.ShouldEqual(1);
            result["item"].ShouldEqual(value);
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
        public void should_deserialize_empty_reference_type_dictionary_entries(Type type)
        {
            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Xml("<DictionaryOf{0}><item/></DictionaryOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), dictionaryType).As<IDictionary>();

            result.ShouldNotBeNull();
            result.ShouldBeType(dictionaryType);
            result.Count.ShouldEqual(1);
            result["item"].ShouldBeNull();
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
        public void should_fail_to_deserialize_null_value_type_dictionary_entries(Type type)
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<DictionaryOf{0}><item/></DictionaryOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericDictionaryType<string>()));

            exception.Message.ShouldEqual(("Error deserializing xml element '/DictionaryOf{0}/item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {1}>[\"item\"]': Value cannot be null.").ToFormat(type.Name, type.FullName));
            exception.FriendlyMessage.ShouldEqual(("Could not read xml element " +
                "'/DictionaryOf{0}/item': Value cannot be null.").ToFormat(type.Name));
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_fail_to_parse_empty_dictionary_entries(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<DictionaryOf{0}><item></item></DictionaryOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericDictionaryType<string>()));

            exception.Message.ShouldStartWith(("Error deserializing xml element '/DictionaryOf{0}/item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {1}>[\"item\"]': Error parsing ''. ").ToFormat(type.GetUnderlyingNullableType().Name, type.GetFriendlyTypeFullName()));

            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/DictionaryOf{0}/item': ".ToFormat(type.GetUnderlyingNullableType().Name) +
                Options.Create().Deserialization.FriendlyParseErrorMessages[messageType].ToFormat(""));

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_fail_to_parse_empty_dictionary_entries_with_custom_parse_message(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage(messageType, "yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml("<DictionaryOf{0}><item></item></DictionaryOf{0}>"
                .ToFormat(type.GetUnderlyingNullableType().Name), type.MakeGenericDictionaryType<string>(), options));

            exception.Message.ShouldStartWith(("Error deserializing xml element '/DictionaryOf{0}/item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {1}>[\"item\"]': Error parsing ''. ").ToFormat(type.GetUnderlyingNullableType().Name, type.GetFriendlyTypeFullName()));
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/DictionaryOf{0}/item': yada".ToFormat(type.GetUnderlyingNullableType().Name));
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_dictionary_entries_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml(
                "<DictionaryOfInt32><item></item></DictionaryOfInt32>", typeof(Dictionary<string, int>), options));

            exception.Message.ShouldEqual("Error deserializing xml element '/DictionaryOfInt32/item' to 'System.Collections." +
                "Generic.Dictionary<System.String, System.Int32>[\"item\"]': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/DictionaryOfInt32/item': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        // Complex types

        public class ComplexType
        {
            public string Property { get; set; }
        }

        [Test]
        [TestCase(typeof(Dictionary<string, ComplexType>))]
        [TestCase(typeof(IDictionary<string, ComplexType>))]
        [TestCase(typeof(GenericDictionaryImpl<string, ComplexType>))]
        public void should_deserialize_dictionary_of_complex_type(Type type)
        {
            var xml = "<DictionaryOfComplexType><item><Property>hai</Property></item></DictionaryOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IDictionary<string, ComplexType>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result["item"].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(DictionaryImpl))]
        public void should_fail_to_deserialize_non_generic_dictionaries(Type type)
        {
            var xml = "<DictionaryOfAnyType><item><Property>hai</Property></item></DictionaryOfAnyType>";
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Xml(xml, type));

            exception.Message.ShouldEqual(("Error deserializing xml element '/DictionaryOfAnyType/item' to " +
                "'{0}': Non generic dictionary '{0}' is not supported for deserialization. " +
                "Only generic dictionaries can be deserialized.").ToFormat(type.GetFriendlyTypeFullName()));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        // Array types

        [Test]
        [TestCase(typeof(Dictionary<string, List<ComplexType>>))]
        [TestCase(typeof(IDictionary<string, List<ComplexType>>))]
        [TestCase(typeof(GenericDictionaryImpl<string, List<ComplexType>>))]
        public void should_deserialize_dictionary_of_dictionary(Type type)
        {
            var xml = "<DictionaryOfArrayOfComplexType><item><ComplexType><Property>hai</Property></ComplexType></item></DictionaryOfArrayOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IDictionary<string, List<ComplexType>>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result["item"][0].Property.ShouldEqual("hai");
        }

        // Dictionary types

        [Test]
        [TestCase(typeof(Dictionary<string, Dictionary<string, ComplexType>>))]
        [TestCase(typeof(IDictionary<string, Dictionary<string, ComplexType>>))]
        [TestCase(typeof(GenericDictionaryImpl<string, Dictionary<string, ComplexType>>))]
        public void should_deserialize_dictionary_of_array(Type type)
        {
            var xml = "<DictionaryOfDictionaryOfComplexType><item1><item2><Property>hai</Property></item2></item1></DictionaryOfDictionaryOfComplexType>";

            var result = Deserialize.Xml(xml, type).As<IDictionary<string, Dictionary<string, ComplexType>>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result["item1"]["item2"].Property.ShouldEqual("hai");
        }

        // Dictionary member

        public class RootType
        {
            public Hashtable NonGenericDictionary { get; set; }
            public IDictionary DictionaryInterface { get; set; }
            public DictionaryImpl DictionaryImpl { get; set; }

            public Dictionary<string, ComplexType> ComplexDictionary { get; set; }
            public IDictionary<string, ComplexType> ComplexDictionaryInterface { get; set; }
            public GenericDictionaryImpl<string, ComplexType> ComplexDictionaryImpl { get; set; }
        }

        [Test]
        [TestCase("ComplexDictionary")]
        [TestCase("ComplexDictionaryInterface")]
        [TestCase("ComplexDictionaryImpl")]
        public void should_deserialize_member_dictionary_of_complex_type(string name)
        {
            var xml = "<RootType><{0}><item><Property>hai</Property></item></{0}></RootType>".ToFormat(name);

            var result = Deserialize.Xml<RootType>(xml)
                .GetPropertyOrFieldValue(name).As<IDictionary<string, ComplexType>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result["item"].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase("NonGenericDictionary")]
        [TestCase("DictionaryInterface")]
        [TestCase("DictionaryImpl")]
        public void should_fail_to_deserialize_member_non_generic_arrays(string name)
        {
            var xml = "<RootType><{0}><item><Property>hai</Property></item></{0}></RootType>".ToFormat(name);
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Xml<RootType>(xml));

            exception.Message.ShouldEqual(("Error deserializing xml element '/RootType/{0}/item' to " +
                "'Tests.Deserializer.Xml.DictionaryTests.RootType.{0}': Non generic dictionary '{1}' " +
                "is not supported for deserialization. Only generic dictionaries can be deserialized.")
                    .ToFormat(name, typeof(RootType).GetProperty(name)
                        .PropertyType.GetFriendlyTypeFullName()));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_deserialize_empty_dictionary_of_complex_type()
        {
            var xml = "<RootType><ComplexDictionary /></RootType>";

            var result = Deserialize.Xml<RootType>(xml);

            result.ShouldNotBeNull();
            result.ComplexDictionary.ShouldNotBeNull();
            result.ComplexDictionary.ShouldBeEmpty();
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
            var result = Deserialize.Xml<DictionaryImplementation>(
                "<DictionaryOfString><item1>oh</item1><item2>hai</item2></DictionaryOfString>");

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_root_as_object_when_configured()
        {
            var result = Deserialize.Xml<DictionaryImplementation>(
                "<DictionaryImplementation><Property>oh</Property><Field>hai</Field></DictionaryImplementation>",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields());

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_member_as_object_by_default()
        {
            var result = Deserialize.Xml<DictionaryMember>(
                "<DictionaryMember><DictionaryImpl><item1>oh</item1><item2>hai</item2></DictionaryImpl></DictionaryMember>").DictionaryImpl;

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_member_as_object_when_configured()
        {
            var result = Deserialize.Xml<DictionaryMember>(
                "<DictionaryMember><DictionaryImpl><Property>oh</Property><Field>hai</Field></DictionaryImpl></DictionaryMember>",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()).DictionaryImpl;

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_array_item_as_object_by_default()
        {
            var result = Deserialize.Xml<List<DictionaryImplementation>>(
                "<ArrayOfDictionaryOfString><DictionaryOfString><item1>oh</item1><item2>hai</item2></DictionaryOfString></ArrayOfDictionaryOfString>").First();

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_array_item_as_object_when_configured()
        {
            var result = Deserialize.Xml<List<DictionaryImplementation>>(
                "<ArrayOfDictionaryImplementation><DictionaryImplementation><Property>oh</Property><Field>hai</Field></DictionaryImplementation></ArrayOfDictionaryImplementation>",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()).First();

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_dictionary_entry_as_object_by_default()
        {
            var result = Deserialize.Xml<Dictionary<string, DictionaryImplementation>>(
                "<DictionaryOfDictionaryOfString><item><item1>oh</item1><item2>hai</item2></item></DictionaryOfDictionaryOfString>")["item"];

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_dictionary_entry_as_object_when_configured()
        {
            var result = Deserialize.Xml<Dictionary<string, DictionaryImplementation>>(
                "<DictionaryOfDictionaryImplementation><item><Property>oh</Property><Field>hai</Field></item></DictionaryOfDictionaryImplementation>",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())["item"];

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        // Case sensitivity

        [Test]
        public void should_deserialize_case_insensitively()
        {
            var xml = "<DICTIONARYOFSTRING><item>hai</item></DICTIONARYOFSTRING>";
            var result = Deserialize.Xml<Dictionary<string, string>>(xml, x => x.Deserialization(y => y.IgnoreNameCase()));
            result["item"].ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_with_case_comparison()
        {
            var xml = "<DICTIONARYOFSTRING><item>hai</item></DICTIONARYOFSTRING>";
            var result = Deserialize.Xml<Dictionary<string, string>>(xml, x => x.Deserialization(y => y
                    .WithNameComparison(StringComparison.CurrentCultureIgnoreCase)));
            result["item"].ShouldEqual("hai");
        }

        [Test]
        public void should_not_deserialize_case_insensitively_by_default()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(
                () => Deserialize.Xml<Dictionary<string, string>>(
                    "<DICTIONARYOFSTRING />", x => x.IncludePublicFields()));

            exception.Message.ShouldEqual("Xml root name 'DICTIONARYOFSTRING' does not match " +
                "expected name of 'DictionaryOfString'. Deserializing 'System.Collections.Generic." +
                "Dictionary<System.String, System.String>'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'DICTIONARYOFSTRING' does not match " +
                "expected name of 'DictionaryOfString'.");
        }

        // Filtering attributes

        private const string FilterAttributesXml = "<DictionaryOfString Attribute=\"oh\"><Element>hai</Element></DictionaryOfString>";

        [Test]
        public void should_include_attributes_by_default()
        {
            var result = Deserialize.Xml<Dictionary<string, string>>(FilterAttributesXml);
            result["Attribute"].ShouldEqual("oh");
            result["Element"].ShouldEqual("hai");
        }

        [Test]
        public void should_ignore_attributes_when_configured()
        {
            var result = Deserialize.Xml<Dictionary<string, string>>(FilterAttributesXml, x => x.Deserialization(y => y.IgnoreXmlAttributes()));
            result.ContainsKey("Attribute").ShouldBeFalse();
            result["Element"].ShouldEqual("hai");
        }

        // Type filtering

        [Test]
        public void should_include_types_when()
        {
            Deserialize.Xml<IDictionary<string, ComplexType>>(
                "<DictionaryOfComplexType><item><Property>hai</Property></item></DictionaryOfComplexType>", 
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(1);
        }

        [Test]
        public void should_filter_types()
        {
            Deserialize.Xml<IDictionary<string, ComplexType>>(
                "<DictionaryOfComplexType><item><Property>hai</Property></item></DictionaryOfComplexType>",
                x => x.ExcludeType<ComplexType>()).ShouldTotal(0);
        }

        [Test]
        public void should_exclude_types_when()
        {
            Deserialize.Xml<IDictionary<string, ComplexType>>(
                "<DictionaryOfComplexType><item><Property>hai</Property></item></DictionaryOfComplexType>", 
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(0);
        }

        // Name validation

        [Test]
        public void should_not_ignore_root_name_by_default()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(
                () => Deserialize.Xml<Dictionary<string, string>>("<sdfgsdfasdfasdfas />"));

            exception.Message.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not match expected " +
                "name of 'DictionaryOfString'. Deserializing 'System.Collections.Generic" +
                ".Dictionary<System.String, System.String>'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not " +
                "match expected name of 'DictionaryOfString'.");
        }

        [Test]
        public void should_ignore_root_name_when_configured()
        {
            var result = Deserialize.Xml<Dictionary<string, string>>(
                "<sdfgsdfasdfasdfas><item>oh</item></sdfgsdfasdfasdfas>",
                x => x.Deserialization(y => y.IgnoreRootName()));

            result["item"].ShouldEqual("oh");
        }

        // Naming conventions

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Deserialize.Xml<Dictionary<string, string>>("<DictionaryOfString />").ShouldNotBeNull();
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Deserialize.Xml<Dictionary<string, string>>("<Hashtable_String />",
                x => x.WithDictionaryTypeNameFormat("Hashtable_{0}")).ShouldNotBeNull();
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot : Dictionary<string, string> { }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlRoot>("<Root />").ShouldNotBeNull();
        }

        [XmlType("Type")]
        public class XmlType : Dictionary<string, string> { }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlType>("<Type />").ShouldNotBeNull();
        }
    }
}
