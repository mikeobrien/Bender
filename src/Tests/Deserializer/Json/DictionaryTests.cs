using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace Tests.Deserializer.Json
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
        public void should_deserialize_typed_dictionary_entries(Type type, object value, object defaultValue)
        {
            var json = "{{ \"item\": {0} }}".ToFormat(type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : "\"" + value + "\"");

            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Json(json, dictionaryType).As<IDictionary>();

            result.ShouldNotBeNull();
            result.ShouldBeType(dictionaryType);
            result.Count.ShouldEqual(1);
            result["item"].ShouldEqual(value);
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_deserialize_string_dictionary_entries(Type type, object value, object defaultValue)
        {
            var json = "{{ \"item\": \"{0}\" }}".ToFormat(type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : value);

            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Json(json, dictionaryType).As<IDictionary>();

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
        public void should_deserialize_null_reference_type_dictionary_entries(Type type)
        {
            var dictionaryType = type.MakeGenericDictionaryType<string>();
            var result = Deserialize.Json("{ \"item\": null }", dictionaryType).As<IDictionary>();

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
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json("{ \"item\": null }", type.MakeGenericDictionaryType<string>()));

            exception.Message.ShouldEqual(("Error deserializing json field '$.item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {1}>[\"item\"]': Value cannot be null.").ToFormat(type.Name, type.FullName));
            exception.FriendlyMessage.ShouldEqual(("Could not read json field " +
                "'$.item': Value cannot be null.").ToFormat(type.Name));
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource(nameof(SimpleDictionaryTypes))]
        public void should_fail_to_parse_empty_dictionary_entries(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json("{ \"item\": \"\" }", type.MakeGenericDictionaryType<string>()));

            exception.Message.ShouldStartWith(("Error deserializing json field '$.item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {0}>[\"item\"]': Error parsing ''. ").ToFormat(type.GetFriendlyTypeFullName()));

            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.item': " +
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

            var exception = Assert.Throws<FriendlyMappingException>(() =>
                Deserialize.Json("{ \"item\": \"\" }", type.MakeGenericDictionaryType<string>(), options));

            exception.Message.ShouldStartWith(("Error deserializing json field '$.item' to 'System.Collections." +
                "Generic.Dictionary<System.String, {0}>[\"item\"]': Error parsing ''. ").ToFormat(type.GetFriendlyTypeFullName()));

            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.item': yada");

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_dictionary_entries_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json("{ \"item\": \"\" }", typeof(Dictionary<string, int>), options));

            exception.Message.ShouldEqual("Error deserializing json field '$.item' to 'System.Collections." +
                "Generic.Dictionary<System.String, System.Int32>[\"item\"]': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.item': yada");
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
            var json = "{ \"item\": { \"Property\": \"hai\" } }";

            var result = Deserialize.Json(json, type).As<IDictionary<string, ComplexType>>();

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
            var json = "{ \"item\": { \"Property\": \"hai\" } }";
            var exception = Assert.Throws<MappingException>(() => Deserialize.Json(json, type));

            exception.Message.ShouldEqual(("Error deserializing json field '$.item' to " +
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
            var json = "{ \"item\": [ { \"Property\": \"hai\" } ] }";

            var result = Deserialize.Json(json, type).As<IDictionary<string, List<ComplexType>>>();

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
            var json = "{ \"item1\": { \"item2\": { \"Property\": \"hai\" } } }";

            var result = Deserialize.Json(json, type).As<IDictionary<string, Dictionary<string, ComplexType>>>();

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
            var json = "{{ \"{0}\" : {{ \"item\": {{ \"Property\": \"hai\" }} }} }}".ToFormat(name);

            var result = Deserialize.Json<RootType>(json)
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
            var json = "{{ \"{0}\" : {{ \"item\": {{ \"Property\": \"hai\" }} }} }}".ToFormat(name);
            var exception = Assert.Throws<MappingException>(() => Deserialize.Json<RootType>(json));

            exception.Message.ShouldEqual(("Error deserializing json field '$.{0}.item' to " +
                "'Tests.Deserializer.Json.DictionaryTests.RootType.{0}': Non generic dictionary '{1}' " +
                "is not supported for deserialization. Only generic dictionaries can be deserialized.")
                    .ToFormat(name, typeof(RootType).GetProperty(name)
                        .PropertyType.GetFriendlyTypeFullName()));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_deserialize_null_dictionary_of_complex_type()
        {
            var json = "{ \"ComplexDictionary\": null }";

            var result = Deserialize.Json<RootType>(json);

            result.ShouldNotBeNull();
            result.ComplexDictionary.ShouldBeNull();
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
            var result = Deserialize.Json<DictionaryImplementation>(
                "{ \"item1\": \"oh\", \"item2\": \"hai\" }");

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_root_as_object_when_configured()
        {
            var result = Deserialize.Json<DictionaryImplementation>(
                "{ \"Property\": \"oh\", \"Field\": \"hai\" }",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields());

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_member_as_object_by_default()
        {
            var result = Deserialize.Json<DictionaryMember>(
                "{ \"DictionaryImpl\": { \"item1\": \"oh\", \"item2\": \"hai\" } }").DictionaryImpl;

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_member_as_object_when_configured()
        {
            var result = Deserialize.Json<DictionaryMember>(
                "{ \"DictionaryImpl\": { \"Property\": \"oh\", \"Field\": \"hai\" } }",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()).DictionaryImpl;

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_array_item_as_object_by_default()
        {
            var result = Deserialize.Json<List<DictionaryImplementation>>(
                "[ { \"item1\": \"oh\", \"item2\": \"hai\" } ]").First();

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_array_item_as_object_when_configured()
        {
            var result = Deserialize.Json<List<DictionaryImplementation>>(
                "[ { \"Property\": \"oh\", \"Field\": \"hai\" } ]",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()).First();

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_dictionary_dictionary_entry_as_object_by_default()
        {
            var result = Deserialize.Json<Dictionary<string, DictionaryImplementation>>(
                "{ \"item\": { \"item1\": \"oh\", \"item2\": \"hai\" } }")["item"];

            result["item1"].ShouldEqual("oh");
            result["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_dictionary_dictionary_entry_as_object_when_configured()
        {
            var result = Deserialize.Json<Dictionary<string, DictionaryImplementation>>(
                "{ \"item\": { \"Property\": \"oh\", \"Field\": \"hai\" } }",
                x => x.TreatDictionaryImplsAsObjects().IncludePublicFields())["item"];

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        // Type filtering

        [Test]
        public void should_include_types_when()
        {
            Deserialize.Json<IDictionary<string, ComplexType>>(
                "{ \"item\": { \"Property\": \"hai\" } }", 
                x => x.IncludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(1);
        }

        [Test]
        public void should_filter_types()
        {
            Deserialize.Json<IDictionary<string, ComplexType>>(
                "{ \"item\": { \"Property\": \"hai\" } }",
                x => x.ExcludeType<ComplexType>()).ShouldTotal(0);
        }

        [Test]
        public void should_exclude_types_when()
        {
            Deserialize.Json<IDictionary<string, ComplexType>>(
                "{ \"item\": { \"Property\": \"hai\" } }", 
                x => x.ExcludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(0);
        }
    }
}
