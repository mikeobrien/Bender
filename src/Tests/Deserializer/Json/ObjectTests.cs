using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class ObjectTests
    {
        // Simple types

        public class SimpleTypeField
        {
            public string StringProperty { get; set; }
            public Uri UriProperty { get; set; }
            public UriFormat EnumProperty { get; set; } public UriFormat? EnumNullableProperty { get; set; }
            public DateTime DateTimeProperty { get; set; } public DateTime? DateTimeNullableProperty { get; set; }
            public TimeSpan TimeSpanProperty { get; set; } public TimeSpan? TimeSpanNullableProperty { get; set; }
            public Guid GuidProperty { get; set; } public Guid? GuidNullableProperty { get; set; }
            public Boolean BooleanProperty { get; set; } public Boolean? BooleanNullableProperty { get; set; }
            public Byte ByteProperty { get; set; } public Byte? ByteNullableProperty { get; set; }
            public SByte SByteProperty { get; set; } public SByte? SByteNullableProperty { get; set; }
            public Int16 Int16Property { get; set; } public Int16? Int16NullableProperty { get; set; }
            public UInt16 UInt16Property { get; set; } public UInt16? UInt16NullableProperty { get; set; }
            public Int32 Int32Property { get; set; } public Int32? Int32NullableProperty { get; set; }
            public UInt32 UInt32Property { get; set; } public UInt32? UInt32NullableProperty { get; set; }
            public Int64 Int64Property { get; set; } public Int64? Int64NullableProperty { get; set; }
            public UInt64 UInt64Property { get; set; } public UInt64? UInt64NullableProperty { get; set; }
            public IntPtr IntPtrProperty { get; set; } public IntPtr? IntPtrNullableProperty { get; set; }
            public UIntPtr UIntPtrProperty { get; set; } public UIntPtr? UIntPtrNullableProperty { get; set; }
            public Char CharProperty { get; set; } public Char? CharNullableProperty { get; set; }
            public Double DoubleProperty { get; set; } public Double? DoubleNullableProperty { get; set; }
            public Single SingleProperty { get; set; } public Single? SingleNullableProperty { get; set; }
            public Decimal DecimalProperty { get; set; } public Decimal? DecimalNullableProperty { get; set; }

            public string StringField;
            public Uri UriField;
            public UriFormat EnumField; public UriFormat? EnumNullableField;
            public DateTime DateTimeField; public DateTime? DateTimeNullableField;
            public TimeSpan TimeSpanField; public TimeSpan? TimeSpanNullableField;
            public Guid GuidField; public Guid? GuidNullableField;
            public Boolean BooleanField; public Boolean? BooleanNullableField;
            public Byte ByteField; public Byte? ByteNullableField;
            public SByte SByteField; public SByte? SByteNullableField;
            public Int16 Int16Field; public Int16? Int16NullableField;
            public UInt16 UInt16Field; public UInt16? UInt16NullableField;
            public Int32 Int32Field; public Int32? Int32NullableField;
            public UInt32 UInt32Field; public UInt32? UInt32NullableField;
            public Int64 Int64Field; public Int64? Int64NullableField;
            public UInt64 UInt64Field; public UInt64? UInt64NullableField;
            public IntPtr IntPtrField; public IntPtr? IntPtrNullableField;
            public UIntPtr UIntPtrField; public UIntPtr? UIntPtrNullableField;
            public Char CharField; public Char? CharNullableField;
            public Double DoubleField; public Double? DoubleNullableField;
            public Single SingleField; public Single? SingleNullableField;
            public Decimal DecimalField; public Decimal? DecimalNullableField;
        }

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleFieldTypes = TestCases.Create("Property", "Field")
            .AddType<string>("1", "String")
            .AddType<Uri>(new Uri("http://www.xkcd.com"), "Uri")

            .AddType<UriFormat>(UriFormat.UriEscaped, "Enum").AddType<UriFormat?>(UriFormat.UriEscaped, "EnumNullable")

            .AddType<DateTime>(DateTime.Today, "DateTime").AddType<DateTime?>(DateTime.Today, "DateTimeNullable")
            .AddType<TimeSpan>(TimeSpan.MaxValue, "TimeSpan").AddType<TimeSpan?>(TimeSpan.MaxValue, "TimeSpanNullable")
            .AddType<Guid>(RandomGuid, "Guid").AddType<Guid?>(RandomGuid, "GuidNullable")

            .AddType<Boolean>(true, "Boolean").AddType<Boolean?>(true, "BooleanNullable")
            .AddType<Byte>(5, "Byte").AddType<Byte?>(55, "ByteNullable")
            .AddType<SByte>(6, "SByte").AddType<SByte?>(66, "SByteNullable")
            .AddType<Int16>(7, "Int16").AddType<Int16?>(77, "Int16Nullable")
            .AddType<UInt16>(8, "UInt16").AddType<UInt16?>(88, "UInt16Nullable")
            .AddType<Int32>(9, "Int32").AddType<Int32?>(99, "Int32Nullable")
            .AddType<UInt32>(10, "UInt32").AddType<UInt32?>(110, "UInt32Nullable")
            .AddType<Int64>(11, "Int64").AddType<Int64?>(111, "Int64Nullable")
            .AddType<UInt64>(12, "UInt64").AddType<UInt64?>(120, "UInt64Nullable")
            .AddType<IntPtr>(new IntPtr(13), "IntPtr").AddType<IntPtr?>(new IntPtr(130), "IntPtrNullable")
            .AddType<UIntPtr>(new UIntPtr(14), "UIntPtr").AddType<UIntPtr?>(new UIntPtr(140), "UIntPtrNullable")
            .AddType<Char>('a', "Char").AddType<Char?>('b', "CharNullable")
            .AddType<Double>(15, "Double").AddType<Double?>(150, "DoubleNullable")
            .AddType<Single>(16, "Single").AddType<Single?>(160, "SingleNullable")
            .AddType<Decimal>(17, "Decimal").AddType<Decimal?>(170, "DecimalNullable")

            .All;

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_deserialize_typed_fields(string suffix, Type type, object value, string name)
        {
            var json = "{{ \"{0}\": {1} }}".ToFormat(name + suffix,
                type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : "\"" + value + "\"");

            var result = Deserialize.Json<SimpleTypeField>(json, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldEqual(value);
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_deserialize_string_fields(string suffix, Type type, object value, string name)
        {
            var json = "{{ \"{0}\": \"{1}\" }}".ToFormat(name + suffix,
                type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : value);

            var result = Deserialize.Json<SimpleTypeField>(json, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldEqual(value);
        }

        private static readonly object[] SimpleFieldReferenceTypes = TestCases.Create("Property", "Field")
            .AddType<string>("1", "String")
            .AddType<Uri>(new Uri("http://www.xkcd.com"), "Uri")

            .AddType<UriFormat?>(UriFormat.UriEscaped, "EnumNullable")

            .AddType<DateTime?>(DateTime.Today, "DateTimeNullable")
            .AddType<TimeSpan?>(TimeSpan.MaxValue, "TimeSpanNullable")
            .AddType<Guid?>(RandomGuid, "GuidNullable")

            .AddType<Boolean?>(true, "BooleanNullable")
            .AddType<Byte?>(55, "ByteNullable")
            .AddType<SByte?>(66, "SByteNullable")
            .AddType<Int16?>(77, "Int16Nullable")
            .AddType<UInt16?>(88, "UInt16Nullable")
            .AddType<Int32?>(99, "Int32Nullable")
            .AddType<UInt32?>(110, "UInt32Nullable")
            .AddType<Int64?>(111, "Int64Nullable")
            .AddType<UInt64?>(120, "UInt64Nullable")
            .AddType<IntPtr?>(new IntPtr(130), "IntPtrNullable")
            .AddType<UIntPtr?>(new UIntPtr(140), "UIntPtrNullable")
            .AddType<Char?>('b', "CharNullable")
            .AddType<Double?>(150, "DoubleNullable")
            .AddType<Single?>(160, "SingleNullable")
            .AddType<Decimal?>(170, "DecimalNullable")

            .All;

        [Test]
        [TestCaseSource("SimpleFieldReferenceTypes")]
        public void should_deserialize_null_reference_type_fields(string suffix, Type type, object value, string name)
        {
            var json = "{{ \"{0}\": null }}".ToFormat(name + suffix);

            var result = Deserialize.Json<SimpleTypeField>(json, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldBeNull();
        }

        private static readonly object[] SimpleFieldValueTypes = TestCases.Create("Property", "Field")
            .AddType<UriFormat>(UriFormat.UriEscaped, "Enum")

            .AddType<DateTime>(DateTime.Today, "DateTime")
            .AddType<TimeSpan>(TimeSpan.MaxValue, "TimeSpan")
            .AddType<Guid>(RandomGuid, "Guid")

            .AddType<Boolean>(true, "Boolean")
            .AddType<Byte>(5, "Byte")
            .AddType<SByte>(6, "SByte")
            .AddType<Int16>(7, "Int16")
            .AddType<UInt16>(8, "UInt16")
            .AddType<Int32>(9, "Int32")
            .AddType<UInt32>(10, "UInt32")
            .AddType<Int64>(11, "Int64")
            .AddType<UInt64>(12, "UInt64")
            .AddType<IntPtr>(new IntPtr(13), "IntPtr")
            .AddType<UIntPtr>(new UIntPtr(14), "UIntPtr")
            .AddType<Char>('a', "Char")
            .AddType<Double>(15, "Double")
            .AddType<Single>(16, "Single")
            .AddType<Decimal>(17, "Decimal")

            .All;

        [Test]
        [TestCaseSource("SimpleFieldValueTypes")]
        public void should_fail_to_deserialize_null_value_type_fields(string suffix, Type type, object value, string name)
        {
            var json = "{{ \"{0}\": null }}".ToFormat(name + suffix);
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json<SimpleTypeField>(json, x => x.IncludePublicFields()));

            exception.Message.ShouldEqual(("Error deserializing json field '$.{0}' to " +
                "'Tests.Deserializer.Json.ObjectTests.SimpleTypeField.{0}': Value cannot be null.").ToFormat(name + suffix));
            exception.FriendlyMessage.ShouldEqual(("Could not read json field " +
                "'$.{0}': Value cannot be null.").ToFormat(name + suffix));
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_fail_to_parse_empty_fields(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var json = "{{ \"{0}\": \"\" }}".ToFormat(name + suffix);
            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields());

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Json<SimpleTypeField>(json, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing json field '$.{0}' to 'Tests.Deserializer." +
                "Json.ObjectTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName));

            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.{0}': ".ToFormat(elementName) +
                Options.Create().Deserialization.FriendlyParseErrorMessages[messageType].ToFormat(""));

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var json = "{{ \"{0}\": \"\" }}".ToFormat(name + suffix);

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields()
                .Deserialization(y => y.WithFriendlyParseErrorMessage(messageType, "yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Json<SimpleTypeField>(json, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing json field '$.{0}' to 'Tests.Deserializer." +
                "Json.ObjectTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName));
            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.{0}': yada".ToFormat(elementName));
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json<SimpleTypeField>("{ \"Int32Property\": \"\" }", options));

            exception.Message.ShouldEqual("Error deserializing json field '$.Int32Property' to 'Tests.Deserializer." +
                "Json.ObjectTests.SimpleTypeField.Int32Property': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.Int32Property': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        // Out of the box types

        public class OutOfTheBoxTypes
        {
            public IPAddress IPAddress { get; set; }
            public Version Version { get; set; }
            public MailAddress MailAddress { get; set; }
            public byte[] ByteArray { get; set; }
            public SqlConnectionStringBuilder ConnectionString { get; set; }
        }

        [Test]
        public void should_deserialize_ip_address()
        {
            Deserialize.Json<OutOfTheBoxTypes>("{ \"IPAddress\": \"192.168.1.1\" }")
                .IPAddress.ShouldEqual(IPAddress.Parse("192.168.1.1"));
        }

        [Test]
        public void should_deserialize_version()
        {
            Deserialize.Json<OutOfTheBoxTypes>("{ \"Version\": \"1.2.3.4\" }")
                .Version.ShouldEqual(Version.Parse("1.2.3.4"));
        }

        [Test]
        public void should_deserialize_mail_address()
        {
            Deserialize.Json<OutOfTheBoxTypes>("{ \"MailAddress\": \"test@test.com\" }")
                .MailAddress.ShouldEqual(new MailAddress("test@test.com"));
        }

        [Test]
        public void should_deserialize_byte_array()
        {
            Deserialize.Json<OutOfTheBoxTypes>("{ \"ByteArray\": \"b2ggaGFp\" }")
                .ByteArray.ShouldEqual(ASCIIEncoding.ASCII.GetBytes("oh hai"));
        }

        [Test]
        public void should_deserialize_connection_string()
        {
            var result = Deserialize.Json<OutOfTheBoxTypes>("{ \"ConnectionString\": " +
                "\"server=localhost;database=myapp;Integrated Security=SSPI\" }");
            result.ConnectionString.DataSource.ShouldEqual("localhost");
            result.ConnectionString.InitialCatalog.ShouldEqual("myapp");
            result.ConnectionString.IntegratedSecurity.ShouldBeTrue();
        }

        // Complex types 

        public class ComplexType
        {
            public string Property { get; set; }
        }

        public class RootType
        {
            public ComplexType Complex { get; set; }
        }

        [Test]
        public void should_deserialize_complex_type()
        {
            var json = "{ \"Complex\": { \"Property\": \"hai\" }}";

            var result = Deserialize.Json<RootType>(json).Complex;

            result.ShouldNotBeNull();
            result.Property.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_null_complex_type()
        {
            var json = "{ \"Complex\": null }";

            var result = Deserialize.Json<RootType>(json);

            result.ShouldNotBeNull();
            result.Complex.ShouldBeNull();
        }

        [Test]
        public void should_fail_to_deserialize_value_as_complex_type()
        {
            var json = "{ \"Complex\": \"hai\" }";

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Json<RootType>(json));

            exception.Message.ShouldEqual("Error deserializing json field '$.Complex' to 'Tests.Deserializer." +
                                          "Json.ObjectTests.RootType.Complex': Cannot map a value node to an object node.");
            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.Complex': Should be an object but was a value.");

            exception.InnerException.ShouldBeType<NodeTypeMismatchException>();
        }

        // Xml attributes

        public class XmlAttributes
        {
            [XmlAttribute("AttributeNameOverride")]
            public string AttributeName { get; set; }

            [XmlElement("ElementNameOverride")]
            public string ElementName { get; set; }

            [XmlArray("StringNameOverride")]
            public string StringName { get; set; }

            [XmlArray("ArrayNameOverride")]
            public List<string> ArrayName { get; set; }
        }

        [Test]
        public void should_override_field_name_with_xml_attribute_attribute()
        {
            Deserialize.Json<XmlAttributes>("{ \"AttributeNameOverride\": \"hai\" }")
                .AttributeName.ShouldEqual("hai");
        }

        [Test]
        public void should_override_field_name_with_xml_element_attribute()
        {
            Deserialize.Json<XmlAttributes>("{ \"ElementNameOverride\": \"hai\" }")
                .ElementName.ShouldEqual("hai");
        }

        [Test]
        public void should_override_field_name_with_xml_array_attribute()
        {
            Deserialize.Json<XmlAttributes>("{ \"ArrayNameOverride\": [ \"hai\" ] }")
                .ArrayName.ShouldEqual(new List<string> { "hai" });
        }

        [Test]
        public void should_not_override_field_name_with_xml_array_attribute_when_not_an_enumerable()
        {
            Deserialize.Json<XmlAttributes>("{ \"StringNameOverride\": \"hai\" }")
                .StringName.ShouldBeNull();
        }

        // Case sensitivity

        public class CaseSensitivity
        {
            public string Property { get; set; }
            public string Field;
        }

        [Test]
        [TestCase("Property")]
        [TestCase("Field")]
        public void should_deserialize_fields_case_insensitively(string name)
        {
            var json = "{{ \"{0}\": \"hai\" }}".ToFormat(name.ToUpper());
            var result = Deserialize.Json<CaseSensitivity>(json, x => x
                .IncludePublicFields().Deserialization(y => y.IgnoreNameCase()));
            result.GetPropertyOrFieldValue(name).ShouldEqual("hai");
        }

        [Test]
        [TestCase("Property")]
        [TestCase("Field")]
        public void should_deserialize_fields_with_case_comparison(string name)
        {
            var json = "{{ \"{0}\": \"hai\" }}".ToFormat(name.ToUpper());
            var result = Deserialize.Json<CaseSensitivity>(json, x => x
                .IncludePublicFields().Deserialization(y => y
                    .WithNameComparison(StringComparison.CurrentCultureIgnoreCase)));
            result.GetPropertyOrFieldValue(name).ShouldEqual("hai");
        }

        [Test]
        [TestCase("Property")]
        [TestCase("Field")]
        public void should_not_deserialize_fields_case_insensitively_by_default(string name)
        {
            var json = "{{ \"{0}\": \"hai\" }}".ToFormat(name.ToUpper());
            var result = Deserialize.Json<CaseSensitivity>(json, x => x.IncludePublicFields());
            result.GetPropertyOrFieldValue(name).ShouldBeNull();
        }

        // Unmapped elements

        public class UnmappedElements { }

        [Test]
        public void should_not_fail_on_unmapped_elements_by_default()
        {
            Assert.DoesNotThrow(() => Deserialize.Json<UnmappedElements>("{ \"yada\": true }")
                .ShouldBeType<UnmappedElements>());
        }

        [Test]
        public void should_fail_on_unmapped_elements_when_configured()
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Json<UnmappedElements>("{ \"yada\": true }",
                x => x.Deserialization(y => y.FailOnUnmatchedElements())));

            exception.Message.ShouldEqual("Error deserializing json field '$.yada' to 'Tests.Deserializer" +
                                          ".Json.ObjectTests.UnmappedElements': Field 'yada' is not recognized.");
            exception.FriendlyMessage.ShouldEqual("Could not read json field '$.yada': Field 'yada' is not recognized.");
            exception.InnerException.ShouldBeType<UnrecognizedNodeDeserializationException>();
        }

        // Unmapped members

        public class UnmappedMembers
        {
            public string Property { get; set; }
        }

        [Test]
        public void should_not_fail_on_unmapped_members_by_default()
        {
            Assert.DoesNotThrow(() => Deserialize.Json<UnmappedMembers>("{}")
                .ShouldBeType<UnmappedMembers>());
        }

        [Test]
        public void should_fail_on_unmapped_members_when_configured()
        {
            var exception = Assert.Throws<FriendlyMappingException>(() =>
                Deserialize.Json<UnmappedMembers>("{}",
                x => x.Deserialization(y => y.FailOnUnmatchedMembers())));

            exception.Message.ShouldEqual("Error deserializing json object '$' to 'Tests.Deserializer" +
                                          ".Json.ObjectTests.UnmappedMembers': The following children were expected but not found: 'Property'.");
            exception.FriendlyMessage.ShouldEqual("Could not read json object '$': The following children were expected but not found: 'Property'.");
            exception.InnerException.ShouldBeType<MissingNodeDeserializationException>();
        }

        // Filtering types

        public class FilterTypes
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        [Test]
        public void should_include_types_when()
        {
            var json = "{ \"Int\": 1, \"String\": \"hai\" }";
            var result = Deserialize.Json<FilterTypes>(json, x => x
                .IncludeTypesWhen((t, o) => t.Name == "Int32"));
            result.Int.ShouldEqual(1);
            result.String.ShouldBeNull();
        }

        [Test]
        public void should_exclude_types()
        {
            var json = "{ \"Int\": 1, \"String\": \"hai\" }";
            var result = Deserialize.Json<FilterTypes>(json, x => x
                .ExcludeType<int>());
            result.Int.ShouldEqual(0);
            result.String.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_types_when()
        {
            var json = "{ \"Int\": 1, \"String\": \"hai\" }";
            var result = Deserialize.Json<FilterTypes>(json, x => x
                .ExcludeTypesWhen((t, o) => t.Name == "Int32"));
            result.Int.ShouldEqual(0);
            result.String.ShouldEqual("hai");
        }

        // Filtering members

        public class FilterMembers
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }

            [XmlIgnore]
            public string IgnoredProperty { get; set; }

            [XmlIgnore] 
            public string IgnoredField;

            private string PrivateProperty { get; set; }
            public string GetPrivatePropertyValue() {  return PrivateProperty; }

            public string Field;

            private string _privateField;
            public string GetPrivateFieldValue() { return _privateField; }
        }

        [Test]
        public void should_include_members_when()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<FilterMembers>(json, x => x
                .IncludeMembersWhen((t, o) => t.Name == "Property1"));
            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();
        }

        [Test]
        public void should_exclude_members_when()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<FilterMembers>(json, x => x
                .ExcludeMembersWhen((t, o) => t.Name == "Property1"));
            result.Property1.ShouldBeNull();
            result.Property2.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_members_with_xml_ignore_attribute_applied()
        {
            var json = "{ \"Property1\": \"oh\", \"Field\": \"hai\", " +
                       "  \"IgnoredProperty\": \"oh\", \"IgnoredField\": \"hai\"}";
            var result = Deserialize.Json<FilterMembers>(json, x => x.IncludePublicFields());
            result.Property1.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
            result.IgnoredProperty.ShouldBeNull();
            result.IgnoredField.ShouldBeNull();
        }

        [Test]
        public void should_exclude_private_properties_by_default()
        {
            Deserialize.Json<FilterMembers>("{ \"PrivateProperty\": \"hai\" }")
                .GetPrivatePropertyValue().ShouldBeNull();
        }

        [Test]
        public void should_include_private_properties_when_configured()
        {
            Deserialize.Json<FilterMembers>("{ \"PrivateProperty\": \"hai\" }",
                x => x.IncludeNonPublicProperties())
                .GetPrivatePropertyValue().ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_public_fields_by_default()
        {
            Deserialize.Json<FilterMembers>("{ \"Field\": \"hai\" }")
                .Field.ShouldBeNull();
        }

        [Test]
        public void should_include_public_fields_when_configured()
        {
            Deserialize.Json<FilterMembers>("{ \"Field\": \"hai\" }",
                x => x.IncludePublicFields())
                .Field.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_private_fields_by_default()
        {
            Deserialize.Json<FilterMembers>("{ \"_privateField\": \"hai\" }")
                .GetPrivateFieldValue().ShouldBeNull();
        }

        [Test]
        public void should_include_private_fields_when_configured()
        {
            Deserialize.Json<FilterMembers>("{ \"_privateField\": \"hai\" }",
                x => x.IncludeNonPublicFields())
                .GetPrivateFieldValue().ShouldEqual("hai");
        }

        // Naming conventions

        public class NamingConventions
        {
            public string PropertyValue { get; set; }
            public string PropertyValue2 { get; set; }
            public string FieldValue;
            public string FieldValue2;
        }

        [Test]
        public void should_use_snake_case_when_configured()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"property_value\": \"oh\", \"field_value\": \"hai\" }",
                x => x.UseSnakeCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_camel_case_when_configured()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"propertyValue\": \"oh\", \"fieldValue\": \"hai\" }",
                x => x.UseCamelCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_json_only_camel_case_when_configured()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"propertyValue\": \"oh\", \"fieldValue\": \"hai\" }",
                x => x.UseJsonCamelCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_global_naming_convention()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithNamingConvention(y => "$" + y).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_conditional_global_naming_convention_conditionally()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithNamingConvention(y => "$" + y, 
                    y => y.StartsWith("Property")).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_member_name_modification_convention()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithMemberNamingConvention((n, c) => "$" + n).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_member_name_modification_convention_conditionally()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithMemberNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.StartsWith("Property")).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_field_name_modification_convention()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithFieldNamingConvention((n, c) => "$" + n).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_field_name_modification_convention_conditionally()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue2\": \"oh\", \"$FieldValue\": \"hai\", \"$FieldValue2\": \"there\" }",
                x => x.WithFieldNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.PropertyValue2.ShouldBeNull();
            result.FieldValue.ShouldBeNull();
            result.FieldValue2.ShouldEqual("there");
        }

        [Test]
        public void should_apply_property_name_modification_convention()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue\": \"hai\" }",
                x => x.WithPropertyNamingConvention((n, c) => "$" + n).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_property_name_modification_convention_conditionally()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"$PropertyValue\": \"oh\", \"$FieldValue2\": \"hai\", \"$PropertyValue2\": \"there\" }",
                x => x.WithPropertyNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.PropertyValue2.ShouldEqual("there");
            result.FieldValue.ShouldBeNull();
            result.FieldValue2.ShouldBeNull();
        }

        [Test]
        public void should_allow_multiple_naming_conventions_to_be_applied()
        {
            var result = Deserialize.Json<NamingConventions>(
                "{ \"+(PropertyValue)*\": \"oh\", " +
                  "\"$(PropertyValue2)*|\": \"hai\", " + 
                  "\"-[FieldValue]^\": \"o\", " +
                  "\"@[FieldValue2]^%\": \"rly\" }",
                x => x
                    .WithPropertyNamingConvention((n, c) => "(" + c.Member.Name + ")")
                    .WithFieldNamingConvention((n, c) => "[" + c.Member.Name + "]")
                    .WithPropertyNamingConvention((n, c) => "+" + n, (n, c) => !c.Member.Name.EndsWith("2"))
                    .WithPropertyNamingConvention((n, c) => "$" + n, (n, c) => c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => "-" + n, (n, c) => !c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => "@" + n, (n, c) => c.Member.Name.EndsWith("2"))
                    .WithPropertyNamingConvention((n, c) => n + "*")
                    .WithPropertyNamingConvention((n, c) => n + "|", (n, c) => c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => n + "^")
                    .WithFieldNamingConvention((n, c) => n + "%", (n, c) => c.Member.Name.EndsWith("2"))
                    .IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.PropertyValue2.ShouldEqual("hai");
            result.FieldValue.ShouldEqual("o");
            result.FieldValue2.ShouldEqual("rly");
        }


        public class Response
        {
            public List<AoDOption> AodHourlyStatuses { get; set; }
        }

        public class AoDOption
        {
            public AoDOption() { }
            public AoDOption(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id { get; }
            public string Name { get; }
        }
    }
}
