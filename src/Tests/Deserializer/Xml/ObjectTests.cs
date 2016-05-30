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

namespace Tests.Deserializer.Xml
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
        [TestCaseSource(nameof(SimpleFieldTypes))]
        public void should_deserialize_values_as_elements(string suffix, Type type, object value, string name)
        {
            var xml = "<SimpleTypeField><{0}>{1}</{0}></SimpleTypeField>".ToFormat(name + suffix, value);

            var result = Deserialize.Xml<SimpleTypeField>(xml, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldEqual(value);
        }

        [Test]
        [TestCaseSource(nameof(SimpleFieldTypes))]
        public void should_deserialize_values_as_attributes(string suffix, Type type, object value, string name)
        {
            var xml = "<SimpleTypeField {0}=\"{1}\" />".ToFormat(name + suffix, value);

            var result = Deserialize.Xml<SimpleTypeField>(xml, x => x.IncludePublicFields());

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
        [TestCaseSource(nameof(SimpleFieldReferenceTypes))]
        public void should_deserialize_empty_value_elements_as_null_for_reference_types(string suffix, Type type, object value, string name)
        {
            var xml = "<SimpleTypeField><{0} /></SimpleTypeField>".ToFormat(name + suffix);

            var result = Deserialize.Xml<SimpleTypeField>(xml, x => x.IncludePublicFields());

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
        [TestCaseSource(nameof(SimpleFieldValueTypes))]
        public void should_fail_to_deserialize_empty_value_elements_as_null_for_value_types(string suffix, Type type, object value, string name)
        {
            var xml = "<SimpleTypeField><{0} /></SimpleTypeField>".ToFormat(name + suffix);
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Xml<SimpleTypeField>(xml, x => x.IncludePublicFields()));

            exception.Message.ShouldEqual(("Error deserializing xml element '/SimpleTypeField/{0}' to " +
                "'Tests.Deserializer.Xml.ObjectTests.SimpleTypeField.{0}': Value cannot be null.").ToFormat(name + suffix));
            exception.FriendlyMessage.ShouldEqual(("Could not read xml element " +
                "'/SimpleTypeField/{0}': Value cannot be null.").ToFormat(name + suffix));
            exception.InnerException.ShouldBeType<ValueCannotBeNullDeserializationException>();
        }

        [Test]
        [TestCaseSource(nameof(SimpleFieldTypes))]
        public void should_fail_to_parse_empty_value_elements(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var xml = "<SimpleTypeField><{0}></{0}></SimpleTypeField>".ToFormat(name + suffix);
            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields());

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml<SimpleTypeField>(xml, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing xml element '/SimpleTypeField/{0}' to 'Tests.Deserializer." +
                "Xml.ObjectTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName));

            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/SimpleTypeField/{0}': ".ToFormat(elementName) +
                Options.Create().Deserialization.FriendlyParseErrorMessages[messageType].ToFormat(""));

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        [TestCaseSource(nameof(SimpleFieldTypes))]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var xml = "<SimpleTypeField><{0}></{0}></SimpleTypeField>".ToFormat(name + suffix);

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields()
                .Deserialization(y => y.WithFriendlyParseErrorMessage(messageType, "yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Xml<SimpleTypeField>(xml, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing xml element '/SimpleTypeField/{0}' to 'Tests.Deserializer." +
                "Xml.ObjectTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName, type.GetFriendlyTypeFullName()));
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/SimpleTypeField/{0}': yada".ToFormat(elementName));
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Xml<SimpleTypeField>("<SimpleTypeField><Int32Property></Int32Property></SimpleTypeField>", options));

            exception.Message.ShouldEqual("Error deserializing xml element '/SimpleTypeField/Int32Property' to 'Tests.Deserializer." +
                "Xml.ObjectTests.SimpleTypeField.Int32Property': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/SimpleTypeField/Int32Property': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        // Optional values

        public class OptionalValues
        {
            public Optional<string> OptionalReferenceTypeProperty { get; set; }
            public Optional<int> OptionalValueTypeProperty { get; set; }
            public Optional<int?> OptionalNullableTypeProperty { get; set; }

            public Optional<string> OptionalReferenceTypeField;
            public Optional<int> OptionalValueTypeField;
            public Optional<int?> OptionalNullableTypeField;
        }

        private static readonly object[] OptionalTypes = TestCases.Create("Property", "Field")
            .AddType<string>("hai", "OptionalReferenceType")
            .AddType<int>(5, "OptionalValueType")
            .AddType<int?>(6, "OptionalNullableType")
            .All;

        [Test]
        [TestCaseSource(nameof(OptionalTypes))]
        public void Should_deserialize_optional_values(string suffix, Type type, object value, string name)
        {
            var xml = "<OptionalValues><{0}>{1}</{0}></OptionalValues>".ToFormat(name + suffix, value);

            var result = Deserialize.Xml<OptionalValues>(xml, x => x.IncludePublicFields());

            var optional = result.GetPropertyOrFieldValue(name + suffix);
            optional.GetPropertyOrFieldValue("HasValue").ShouldEqual(true);
            optional.GetPropertyOrFieldValue("Value").ShouldEqual(value);
        }

        [Test]
        public void Should_not_set_missing_values()
        {
            var xml = "<OptionalValues><OptionalReferenceTypeProperty>" +
                       "hai</OptionalReferenceTypeProperty></OptionalValues>";

            var result = Deserialize.Xml<OptionalValues>(xml);

            result.OptionalReferenceTypeProperty.HasValue.ShouldBeTrue();
            result.OptionalReferenceTypeProperty.Value.ShouldEqual("hai");

            result.OptionalValueTypeProperty.HasValue.ShouldBeFalse();
            result.OptionalValueTypeProperty.Value.ShouldEqual(0);

            result.OptionalNullableTypeProperty.HasValue.ShouldBeFalse();
            result.OptionalNullableTypeProperty.Value.ShouldBeNull();

            result.OptionalReferenceTypeField.HasValue.ShouldBeFalse();
            result.OptionalReferenceTypeField.Value.ShouldBeNull();

            result.OptionalValueTypeField.HasValue.ShouldBeFalse();
            result.OptionalValueTypeField.Value.ShouldEqual(0);

            result.OptionalNullableTypeField.HasValue.ShouldBeFalse();
            result.OptionalNullableTypeField.Value.ShouldBeNull();
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
            Deserialize.Xml<OutOfTheBoxTypes>("<OutOfTheBoxTypes>" +
                    "<IPAddress>192.168.1.1</IPAddress></OutOfTheBoxTypes>")
                .IPAddress.ShouldEqual(IPAddress.Parse("192.168.1.1"));
        }

        [Test]
        public void should_deserialize_version()
        {
            Deserialize.Xml<OutOfTheBoxTypes>("<OutOfTheBoxTypes>" +
                    "<Version>1.2.3.4</Version></OutOfTheBoxTypes>")
                .Version.ShouldEqual(Version.Parse("1.2.3.4"));
        }

        [Test]
        public void should_deserialize_mail_address()
        {
            Deserialize.Xml<OutOfTheBoxTypes>("<OutOfTheBoxTypes>" +
                    "<MailAddress>test@test.com</MailAddress></OutOfTheBoxTypes>")
                .MailAddress.ShouldEqual(new MailAddress("test@test.com"));
        }

        [Test]
        public void should_deserialize_byte_array()
        {
            Deserialize.Xml<OutOfTheBoxTypes>("<OutOfTheBoxTypes>" +
                    "<ByteArray>b2ggaGFp</ByteArray></OutOfTheBoxTypes>")
                .ByteArray.ShouldEqual(ASCIIEncoding.ASCII.GetBytes("oh hai"));
        }

        [Test]
        public void should_deserialize_connection_string()
        {
            var result = Deserialize.Xml<OutOfTheBoxTypes>("<OutOfTheBoxTypes>" +
                "<ConnectionString>server=localhost;database=myapp;" +
                "Integrated Security=SSPI</ConnectionString></OutOfTheBoxTypes>");
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
            var result = Deserialize.Xml<RootType>(
                "<RootType><Complex><Property>hai</Property></Complex></RootType>").Complex;

            result.ShouldNotBeNull();
            result.Property.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_empty_complex_type()
        {
            var result = Deserialize.Xml<RootType>(
                "<RootType><Complex /></RootType>");

            result.ShouldNotBeNull();
            result.Complex.ShouldNotBeNull();
            result.Complex.Property.ShouldBeNull();
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot { }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlRoot>("<Root />").ShouldNotBeNull();
        }

        [XmlType("Type")]
        public class XmlType { }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Deserialize.Xml<XmlType>("<Type />").ShouldNotBeNull();
        }

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
            Deserialize.Xml<XmlAttributes>("<XmlAttributes><AttributeNameOverride>hai</AttributeNameOverride></XmlAttributes>")
                .AttributeName.ShouldEqual("hai");
        }

        [Test]
        public void should_override_field_name_with_xml_element_attribute()
        {
            Deserialize.Xml<XmlAttributes>("<XmlAttributes><ElementNameOverride>hai</ElementNameOverride></XmlAttributes>")
                .ElementName.ShouldEqual("hai");
        }

        [Test]
        public void should_override_field_name_with_xml_array_attribute()
        {
            Deserialize.Xml<XmlAttributes>("<XmlAttributes><ArrayNameOverride><String>hai</String></ArrayNameOverride></XmlAttributes>")
                .ArrayName.ShouldEqual(new List<string> { "hai" });
        }

        [Test]
        public void should_not_override_field_name_with_xml_array_attribute_when_not_an_enumerable()
        {
            Deserialize.Xml<XmlAttributes>("<XmlAttributes><StringNameOverride>hai</StringNameOverride></XmlAttributes>")
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
        public void should_deserialize_case_insensitively(string name)
        {
            var xml = "<CASESENSITIVITY><{0}>hai</{0}></CASESENSITIVITY>".ToFormat(name.ToUpper());
            var result = Deserialize.Xml<CaseSensitivity>(xml, x => x
                .IncludePublicFields().Deserialization(y => y.IgnoreNameCase()));
            result.GetPropertyOrFieldValue(name).ShouldEqual("hai");
        }

        [Test]
        [TestCase("Property")]
        [TestCase("Field")]
        public void should_deserialize_with_case_comparison(string name)
        {
            var xml = "<CASESENSITIVITY><{0}>hai</{0}></CASESENSITIVITY>".ToFormat(name.ToUpper());
            var result = Deserialize.Xml<CaseSensitivity>(xml, x => x
                .IncludePublicFields().Deserialization(y => y
                    .WithNameComparison(StringComparison.CurrentCultureIgnoreCase)));
            result.GetPropertyOrFieldValue(name).ShouldEqual("hai");
        }

        [Test]
        [TestCase("Property")]
        [TestCase("Field")]
        public void should_not_deserialize_case_insensitively_by_default(string name)
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(
                () => Deserialize.Xml<List<string>>("<CASESENSITIVITY />", x => x.IncludePublicFields()));

            exception.Message.ShouldEqual("Xml root name 'CASESENSITIVITY' does not match expected name " +
                "of 'ArrayOfString'. Deserializing 'System.Collections.Generic.List<System.String>'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'CASESENSITIVITY' does not match expected name " +
                "of 'ArrayOfString'.");
        }

        // Unmapped elements

        public class UnmappedElements { }

        [Test]
        public void should_not_fail_on_unmapped_elements_by_default()
        {
            Assert.DoesNotThrow(() => Deserialize.Xml<UnmappedElements>("<UnmappedElements><yada>true</yada></UnmappedElements>")
                .ShouldBeType<UnmappedElements>());
        }

        [Test]
        public void should_fail_on_unmapped_elements_when_configured()
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Deserialize.Xml<UnmappedElements>("<UnmappedElements><yada>true</yada></UnmappedElements>",
                x => x.Deserialization(y => y.FailOnUnmatchedElements())));

            exception.Message.ShouldEqual("Error deserializing xml element '/UnmappedElements/yada' to 'Tests.Deserializer" +
                                          ".Xml.ObjectTests.UnmappedElements': Element 'yada' is not recognized.");
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/UnmappedElements/yada': Element 'yada' is not recognized.");
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
            Assert.DoesNotThrow(() => Deserialize.Xml<UnmappedMembers>("<UnmappedMembers />")
                .ShouldBeType<UnmappedMembers>());
        }

        [Test]
        public void should_fail_on_unmapped_members_when_configured()
        {
            var exception = Assert.Throws<FriendlyMappingException>(() =>
                Deserialize.Xml<UnmappedMembers>("<UnmappedMembers />",
                x => x.Deserialization(y => y.FailOnUnmatchedMembers())));

            exception.Message.ShouldEqual("Error deserializing xml element '/UnmappedMembers' to 'Tests.Deserializer" +
                                          ".Xml.ObjectTests.UnmappedMembers': The following children were expected but not found: 'Property'.");
            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/UnmappedMembers': The following children were expected but not found: 'Property'.");
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
            var xml = "<FilterTypes><Int>1</Int><String>hai</String></FilterTypes>";
            var result = Deserialize.Xml<FilterTypes>(xml, x => x
                .IncludeTypesWhen((t, o) => t.Name == "Int32"));
            result.Int.ShouldEqual(1);
            result.String.ShouldBeNull();
        }

        [Test]
        public void should_exclude_types()
        {
            var xml = "<FilterTypes><Int>1</Int><String>hai</String></FilterTypes>";
            var result = Deserialize.Xml<FilterTypes>(xml, x => x
                .ExcludeType<int>());
            result.Int.ShouldEqual(0);
            result.String.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_types_when()
        {
            var xml = "<FilterTypes><Int>1</Int><String>hai</String></FilterTypes>";
            var result = Deserialize.Xml<FilterTypes>(xml, x => x
                .ExcludeTypesWhen((t, o) => t.Name == "Int32"));
            result.Int.ShouldEqual(0);
            result.String.ShouldEqual("hai");
        }

        // Filtering attributes

        public class FilterAttributes
        {
            public string Element { get; set; }
            public string Attribute { get; set; }
        }

        private const string FilterAttributesXml = "<FilterAttributes Attribute=\"oh\"><Element>hai</Element></FilterAttributes>";

        [Test]
        public void should_include_attributes_by_default()
        {
            var result = Deserialize.Xml<FilterAttributes>(FilterAttributesXml);
            result.Attribute.ShouldEqual("oh");
            result.Element.ShouldEqual("hai");
        }

        [Test]
        public void should_ignore_attributes_when_configured()
        {
            var result = Deserialize.Xml<FilterAttributes>(FilterAttributesXml, x => x.Deserialization(y => y.IgnoreXmlAttributes()));
            result.Attribute.ShouldBeNull();
            result.Element.ShouldEqual("hai");
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
            var xml = "<FilterMembers><Property1>oh</Property1><Property2>hai</Property2></FilterMembers>";
            var result = Deserialize.Xml<FilterMembers>(xml, x => x
                .IncludeMembersWhen((t, o) => t.Name == "Property1"));
            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();
        }

        [Test]
        public void should_exclude_members_when()
        {
            var xml = "<FilterMembers><Property1>oh</Property1><Property2>hai</Property2></FilterMembers>";
            var result = Deserialize.Xml<FilterMembers>(xml, x => x
                .ExcludeMembersWhen((t, o) => t.Name == "Property1"));
            result.Property1.ShouldBeNull();
            result.Property2.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_members_with_xml_ignore_attribute_applied()
        {
            var xml = "<FilterMembers><Property1>oh</Property1>,<Field>hai</Field>," +
                       "<IgnoredProperty>oh</IgnoredProperty><IgnoredField>hai</IgnoredField></FilterMembers>";
            var result = Deserialize.Xml<FilterMembers>(xml, x => x.IncludePublicFields());
            result.Property1.ShouldEqual("oh");
            result.Field.ShouldEqual("hai");
            result.IgnoredProperty.ShouldBeNull();
            result.IgnoredField.ShouldBeNull();
        }

        [Test]
        public void should_exclude_private_properties_by_default()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><PrivateProperty>hai</PrivateProperty></FilterMembers>")
                .GetPrivatePropertyValue().ShouldBeNull();
        }

        [Test]
        public void should_include_private_properties_when_configured()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><PrivateProperty>hai</PrivateProperty></FilterMembers>",
                x => x.IncludeNonPublicProperties())
                .GetPrivatePropertyValue().ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_public_fields_by_default()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><Field>hai</Field></FilterMembers>")
                .Field.ShouldBeNull();
        }

        [Test]
        public void should_include_public_fields_when_configured()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><Field>hai</Field></FilterMembers>",
                x => x.IncludePublicFields())
                .Field.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_private_fields_by_default()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><_privateField>hai</_privateField></FilterMembers>")
                .GetPrivateFieldValue().ShouldBeNull();
        }

        [Test]
        public void should_include_private_fields_when_configured()
        {
            Deserialize.Xml<FilterMembers>("<FilterMembers><_privateField>hai</_privateField></FilterMembers>",
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
            var result = Deserialize.Xml<NamingConventions>(
                "<naming_conventions><property_value>oh</property_value><field_value>hai</field_value></naming_conventions>",
                x => x.UseSnakeCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_camel_case_when_configured()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<namingConventions><propertyValue>oh</propertyValue><fieldValue>hai</fieldValue></namingConventions>",
                x => x.UseCamelCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_xml_spinal_case_when_configured()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<naming-conventions><property-value>oh</property-value><field-value>hai</field-value></naming-conventions>",
                x => x.UseXmlSpinalCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_xml_train_case_when_configured()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<Naming-Conventions><Property-Value>oh</Property-Value><Field-Value>hai</Field-Value></Naming-Conventions>",
                x => x.UseXmlTrainCaseNaming().IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_global_naming_convention()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<_NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></_NamingConventions>",
                x => x.WithNamingConvention(y => "_" + y).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_use_conditional_global_naming_convention_conditionally()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>",
                x => x.WithNamingConvention(y => "_" + y, 
                    y => y.StartsWith("Property")).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_member_name_modification_convention()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>",
                x => x.WithMemberNamingConvention((n, c) => "_" + n).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_member_name_modification_convention_conditionally()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>",
                x => x.WithMemberNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.StartsWith("Property")).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_field_name_modification_convention()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>",
                x => x.WithFieldNamingConvention((n, c) => "_" + n).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.FieldValue.ShouldEqual("hai");
        }

        [Test]
        public void should_apply_field_name_modification_convention_conditionally()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue2>oh</_PropertyValue2>,<_FieldValue>hai</_FieldValue><_FieldValue2>there</_FieldValue2></NamingConventions>",
                x => x.WithFieldNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.PropertyValue2.ShouldBeNull();
            result.FieldValue.ShouldBeNull();
            result.FieldValue2.ShouldEqual("there");
        }

        [Test]
        public void should_apply_property_name_modification_convention()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>",
                x => x.WithPropertyNamingConvention((n, c) => "_" + n).IncludePublicFields());

            result.PropertyValue.ShouldEqual("oh");
            result.FieldValue.ShouldBeNull();
        }

        [Test]
        public void should_apply_property_name_modification_convention_conditionally()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions><_PropertyValue>oh</_PropertyValue>,<_FieldValue2>hai</_FieldValue2><_PropertyValue2>there</_PropertyValue2></NamingConventions>",
                x => x.WithPropertyNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields());

            result.PropertyValue.ShouldBeNull();
            result.PropertyValue2.ShouldEqual("there");
            result.FieldValue.ShouldBeNull();
            result.FieldValue2.ShouldBeNull();
        }

        [Test]
        public void should_allow_multiple_naming_conventions_to_be_applied()
        {
            var result = Deserialize.Xml<NamingConventions>(
                "<NamingConventions>" +
                    "<caPropertyValueag>oh</caPropertyValueag>," +
                    "<daPropertyValue2agh>hai</daPropertyValue2agh>," +
                    "<ebFieldValuebi>o</ebFieldValuebi>," +
                    "<fbFieldValue2bij>rly</fbFieldValue2bij>" + 
                "</NamingConventions>",
                x => x
                    .WithPropertyNamingConvention((n, c) => "a" + c.Member.Name + "a")
                    .WithFieldNamingConvention((n, c) => "b" + c.Member.Name + "b")
                    .WithPropertyNamingConvention((n, c) => "c" + n, (n, c) => !c.Member.Name.EndsWith("2"))
                    .WithPropertyNamingConvention((n, c) => "d" + n, (n, c) => c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => "e" + n, (n, c) => !c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => "f" + n, (n, c) => c.Member.Name.EndsWith("2"))
                    .WithPropertyNamingConvention((n, c) => n + "g")
                    .WithPropertyNamingConvention((n, c) => n + "h", (n, c) => c.Member.Name.EndsWith("2"))
                    .WithFieldNamingConvention((n, c) => n + "i")
                    .WithFieldNamingConvention((n, c) => n + "j", (n, c) => c.Member.Name.EndsWith("2"))
                    .IncludePublicFields().Deserialization(y => y.FailOnUnmatchedMembers()));

            result.PropertyValue.ShouldEqual("oh");
            result.PropertyValue2.ShouldEqual("hai");
            result.FieldValue.ShouldEqual("o");
            result.FieldValue2.ShouldEqual("rly");
        }

        public class GenericTypeNameConvention<T> { }

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Deserialize.Xml<GenericTypeNameConvention<string>>("<GenericTypeNameConventionOfString />").ShouldNotBeNull();
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Deserialize.Xml<GenericTypeNameConvention<string>>("<GenericTypeNameConvention_String />",
                x => x.WithGenericTypeNameFormat("{0}_{1}")).ShouldNotBeNull();
        }

        // Name validation

        public class NameValidation { public string Value { get; set; } }

        [Test]
        public void should_not_ignore_root_name_by_default()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(
                () => Deserialize.Xml<NameValidation>("<sdfgsdfasdfasdfas />"));

            exception.Message.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not match " +
                "expected name of 'NameValidation'. Deserializing 'Tests.Deserializer." +
                "Xml.ObjectTests.NameValidation'.");

            exception.FriendlyMessage.ShouldEqual("Xml root name 'sdfgsdfasdfasdfas' does not match " +
                "expected name of 'NameValidation'.");
        }

        [Test]
        public void should_ignore_root_name_when_configured()
        {
            var result = Deserialize.Xml<NameValidation>(
                "<sdfgsdfasdfasdfas><Value>oh</Value></sdfgsdfasdfasdfas>",
                x => x.Deserialization(y => y.IgnoreRootName()));

            result.Value.ShouldEqual("oh");
        }
    }
}
