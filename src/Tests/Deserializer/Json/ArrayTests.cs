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
        public void should_deserialize_typed_array_items(Type type, object value, object defaultValue)
        {
            var json = "[ {0} ]".ToFormat(type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : "\"" + value + "\"");

            var listType = type.MakeGenericListType();
            var result = Deserialize.Json(json, listType).As<IList>();

            result.ShouldNotBeNull();
            result.ShouldBeType(listType);
            result.Count.ShouldEqual(1);
            result[0].ShouldEqual(value);
        }

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_deserialize_string_array_items(Type type, object value, object defaultValue)
        {
            var json = "[ \"{0}\" ]".ToFormat(type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : value);

            var listType = type.MakeGenericListType();
            var result = Deserialize.Json(json, listType).As<IList>();

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
        public void should_deserialize_null_reference_type_array_items(Type type)
        {
            var listType = type.MakeGenericListType();
            var result = Deserialize.Json("[ null ]", listType).As<IList>();

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
        public void should_fail_to_deserialize_null_value_type_array_items(Type type)
        {
            var exception = Assert.Throws<FriendlyMappingException>(() =>
                Deserialize.Json("[ null ]", type.MakeGenericListType()));

            exception.Message.ShouldEqual(("Error deserializing json value '$[1]' to 'System.Collections." +
                "Generic.List<{1}>[]': Value cannot be null.").ToFormat(type.Name, type.FullName));
            exception.FriendlyMessage.ShouldEqual("Could not read json value '$[1]': Value cannot be null.");
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource("SimpleArrayTypes")]
        public void should_fail_to_parse_empty_array_items(Type type, object value, object defaultValue)
        {
            if (type == typeof(string)) return;

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json("[ \"\" ]", type.MakeGenericListType()));

            exception.Message.ShouldStartWith(("Error deserializing json value '$[1]' to 'System.Collections." +
                "Generic.List<{0}>[]': Error parsing ''. ").ToFormat(type.GetFriendlyTypeFullName()));

            exception.FriendlyMessage.ShouldEqual("Could not read json value '$[1]': " +
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

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json("[ \"\" ]", type.MakeGenericListType(), options));

            exception.Message.ShouldStartWith(("Error deserializing json value '$[1]' to 'System.Collections." +
                "Generic.List<{0}>[]': Error parsing ''. ").ToFormat(type.GetFriendlyTypeFullName()));
            exception.FriendlyMessage.ShouldEqual("Could not read json value '$[1]': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_array_items_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Json("[ \"\" ]", typeof(List<int>), options));

            exception.Message.ShouldEqual("Error deserializing json value '$[1]' to 'System.Collections." +
                "Generic.List<System.Int32>[]': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read json value '$[1]': yada");
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
            var json = "[ { \"Property\": \"hai\" } ]";

            var result = Deserialize.Json(json, type).As<IList<ComplexType>>();

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
                Deserialize.Json("[ { \"Property\": \"hai\" } ]", type));

            exception.Message.ShouldEqual(("Error deserializing json object '$[1]' to " +
                "'{0}': Non generic {1} '{0}' is not supported for deserialization. Only " +
                "generic lists and generic enumerable interfaces can be deserialized.")
                    .ToFormat(type.GetFriendlyTypeFullName(), type.IsList() ? "list" : "enumerable"));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_generic_enumerable_impls_of_complex_type()
        {
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Json<GenericEnumerableImpl<ComplexType>>(
                    "[ { \"Property\": \"hai\" } ]"));

            exception.Message.ShouldEqual("Error deserializing json object '$[1]' to " +
                "'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.Json.ArrayTests.ComplexType>': " +
                "Enumerable 'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.Json.ArrayTests.ComplexType>' " +
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
            var json = "[ [ { \"Property\": \"hai\" } ] ]";

            var result = Deserialize.Json(json, type).As<IList<IList<ComplexType>>>();

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
            var json = "[ { \"item\" : { \"Property\": \"hai\" } } ]";

            var result = Deserialize.Json(json, type).As<IList<Dictionary<string, ComplexType>>>();

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
            var json = "{{ \"{0}\" : [ {{ \"Property\": \"hai\" }} ] }}".ToFormat(name);

            var result = Deserialize.Json<RootType>(json)
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
            var json = "{{ \"{0}\" : [ {{ \"Property\": \"hai\" }} ] }}".ToFormat(name);
            var exception = Assert.Throws<MappingException>(() => Deserialize.Json<RootType>(json));

            var type = typeof(RootType).GetProperty(name).PropertyType;
            exception.Message.ShouldEqual(("Error deserializing json object '$.{0}[1]' to " +
                "'Tests.Deserializer.Json.ArrayTests.RootType.{0}': Non generic {2} '{1}' is not " +
                "supported for deserialization. Only generic lists and generic enumerable " +
                "interfaces can be deserialized.").ToFormat(name, type.GetFriendlyTypeFullName(),
                    type.IsList() ? "list" : "enumerable"));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_member_generic_enumerable_impls()
        {
            var json = "{ \"ComplexEnumerableImpl\" : [ { \"Property\": \"hai\" } ] }}";
            var exception =  Assert.Throws<MappingException>(() => Deserialize.Json<RootType>(json));

            exception.Message.ShouldEqual("Error deserializing json object '$.ComplexEnumerableImpl[1]' to " +
                "'Tests.Deserializer.Json.ArrayTests.RootType.ComplexEnumerableImpl': Enumerable 'Tests.Collections." +
                "Implementations.GenericEnumerableImpl<Tests.Deserializer.Json.ArrayTests.ComplexType>' is not supported " +
                "for deserialization. Only generic lists and generic enumerable interfaces can be deserialized.");
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_deserialize_null_member_array_of_complex_type()
        {
            var json = "{ \"ComplexArray\": null }";

            var result = Deserialize.Json<RootType>(json);

            result.ShouldNotBeNull();
            result.ComplexArray.ShouldBeNull();
        }

        // Objects to arrays

        [Test]
        [TestCase("ComplexArray")]
        [TestCase("ComplexEnumerableInterface")]
        [TestCase("ComplexList")]
        [TestCase("ComplexListInterface")]
        [TestCase("ComplexListImpl")]
        public void should_deserialize_object_into_member_array_of_complex_type(string name)
        {
            var json = "{{ \"{0}\" : {{ \"Complex\": {{ \"Property\": \"hai\" }} }} }}".ToFormat(name);

            var result = Deserialize.Json<RootType>(json)
                .GetPropertyOrFieldValue(name).As<IList<ComplexType>>();

            result.ShouldNotBeNull();
            result.ShouldTotal(1);
            result[0].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase("NonGenericArray")]
        [TestCase("EnumerableInterface")]
        [TestCase("EnumerableImpl")]
        public void should_fail_to_deserialize_object_into_member_non_generic_array(string name)
        {
            var json = "{{ \"{0}\" : {{ \"Complex\": {{ \"Property\": \"hai\" }} }} }}".ToFormat(name);
            var exception = Assert.Throws<MappingException>(() => Deserialize.Json<RootType>(json));

            var type = typeof(RootType).GetProperty(name).PropertyType;
            exception.Message.ShouldEqual(("Error deserializing json field '$.{0}.Complex' to " +
                "'Tests.Deserializer.Json.ArrayTests.RootType.{0}': Non generic {2} '{1}' is not " +
                "supported for deserialization. Only generic lists and generic enumerable " +
                "interfaces can be deserialized.").ToFormat(name, type.GetFriendlyTypeFullName(), 
                    type.IsList() ? "list" : "enumerable"));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_object_into_member_generic_enumerable_impls()
        {
            var json = "{ \"ComplexEnumerableImpl\" : { \"Complex\": { \"Property\": \"hai\" } } }";
            var exception = Assert.Throws<MappingException>(() => Deserialize.Json<RootType>(json));

            exception.Message.ShouldEqual("Error deserializing json field '$.ComplexEnumerableImpl.Complex' " +
                "to 'Tests.Deserializer.Json.ArrayTests.RootType.ComplexEnumerableImpl': Enumerable 'Tests.Collections" +
                ".Implementations.GenericEnumerableImpl<Tests.Deserializer.Json.ArrayTests.ComplexType>' is not supported " +
                "for deserialization. Only generic lists and generic enumerable interfaces can be deserialized.");
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
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
            var result = Deserialize.Json<EnumerableImplementation>("[ \"oh\", \"hai\" ]").ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_root_as_object_when_configured()
        {
            var result = Deserialize.Json<EnumerableImplementation>(
                "{ \"Property\": \"oh\", \"Field\": \"hai\" }",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields());

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_member_as_object_by_default()
        {
            var result = Deserialize.Json<EnumerableMember>("{ \"EnumerableImpl\": [ \"oh\", \"hai\" ] }").EnumerableImpl.ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_member_as_object_when_configured()
        {
            var result = Deserialize.Json<EnumerableMember>(
                "{ \"EnumerableImpl\": { \"Property\": \"oh\", \"Field\": \"hai\" } }",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields()).EnumerableImpl;

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_array_item_as_object_by_default()
        {
            var result = Deserialize.Json<List<EnumerableImplementation>>("[ [ \"oh\", \"hai\" ] ]").First().ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_array_item_as_object_when_configured()
        {
            var result = Deserialize.Json<List<EnumerableImplementation>>(
                "[ { \"Property\": \"oh\", \"Field\": \"hai\" } ]",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields()).First();

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_not_treat_enumerable_dictionary_entry_as_object_by_default()
        {
            var result = Deserialize.Json<Dictionary<string, EnumerableImplementation>>(
                "{ \"item\": [ \"oh\", \"hai\" ] }")["item"].ToList();

            result[0].ShouldEqual("oh");
            result[1].ShouldEqual("hai");
        }

        [Test]
        public void should_treat_enumerable_dictionary_entry_as_object_when_configured()
        {
            var result = Deserialize.Json<Dictionary<string, EnumerableImplementation>>(
                "{ \"item\": { \"Property\": \"oh\", \"Field\": \"hai\" } }",
                x => x.TreatEnumerableImplementationsAsObjects().IncludePublicFields())["item"];

            result.Property.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
        }

        // Type filtering

        [Test]
        public void should_include_types_when()
        {
            Deserialize.Json<IList<ComplexType>>(
                "[ { } ]", x => x
                .IncludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(1);
        }

        [Test]
        public void should_filter_types()
        {
            Deserialize.Json<IList<ComplexType>>("[ { \"Property\": \"hai\" } ]",
                x => x.ExcludeType<ComplexType>()).ShouldTotal(0);
        }

        [Test]
        public void should_exclude_types_when()
        {
            Deserialize.Json<IList<ComplexType>>(
                "[ { \"Property\": \"hai\" } ]", x => x
                .ExcludeTypesWhen((t, o) => t.Name == "ComplexType")).ShouldTotal(0);
        }
    }
}
