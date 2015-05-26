using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class FormUrlEncodedTests
    {
        public class SimpleTypeField
        {
            public string StringProperty { get; set; }
            public Uri UriProperty { get; set; }
            public UriFormat EnumProperty { get; set; }
            public UriFormat? EnumNullableProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public DateTime? DateTimeNullableProperty { get; set; }
            public TimeSpan TimeSpanProperty { get; set; }
            public TimeSpan? TimeSpanNullableProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public Guid? GuidNullableProperty { get; set; }
            public Boolean BooleanProperty { get; set; }
            public Boolean? BooleanNullableProperty { get; set; }
            public Byte ByteProperty { get; set; }
            public Byte? ByteNullableProperty { get; set; }
            public SByte SByteProperty { get; set; }
            public SByte? SByteNullableProperty { get; set; }
            public Int16 Int16Property { get; set; }
            public Int16? Int16NullableProperty { get; set; }
            public UInt16 UInt16Property { get; set; }
            public UInt16? UInt16NullableProperty { get; set; }
            public Int32 Int32Property { get; set; }
            public Int32? Int32NullableProperty { get; set; }
            public UInt32 UInt32Property { get; set; }
            public UInt32? UInt32NullableProperty { get; set; }
            public Int64 Int64Property { get; set; }
            public Int64? Int64NullableProperty { get; set; }
            public UInt64 UInt64Property { get; set; }
            public UInt64? UInt64NullableProperty { get; set; }
            public IntPtr IntPtrProperty { get; set; }
            public IntPtr? IntPtrNullableProperty { get; set; }
            public UIntPtr UIntPtrProperty { get; set; }
            public UIntPtr? UIntPtrNullableProperty { get; set; }
            public Char CharProperty { get; set; }
            public Char? CharNullableProperty { get; set; }
            public Double DoubleProperty { get; set; }
            public Double? DoubleNullableProperty { get; set; }
            public Single SingleProperty { get; set; }
            public Single? SingleNullableProperty { get; set; }
            public Decimal DecimalProperty { get; set; }
            public Decimal? DecimalNullableProperty { get; set; }

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
            var form = "{0}={1}".ToFormat(name + suffix,
                type.IsBoolean() ? value.ToString().ToLower() : HttpUtility.UrlEncode(value.ToString()));

            var result = Deserialize.FormUrlEncoded<SimpleTypeField>(form, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldEqual(value);
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_deserialize_string_fields(string suffix, Type type, object value, string name)
        {
            var form = "{0}={1}".ToFormat(name + suffix,
                type.IsBoolean() ? value.ToString().ToLower() : value);

            var result = Deserialize.FormUrlEncoded<SimpleTypeField>(form, x => x.IncludePublicFields());

            result.ShouldNotBeNull();
            result.ShouldBeType<SimpleTypeField>();
            result.GetPropertyOrFieldValue(name + suffix).ShouldEqual(value);
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_fail_to_parse_empty_fields(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var form = "{0}=".ToFormat(name + suffix);
            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields());

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.FormUrlEncoded<SimpleTypeField>(form, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing url encoded form value '{0}' to 'Tests.Deserializer." +
                "FormUrlEncodedTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName));

            exception.FriendlyMessage.ShouldEqual("Could not read url encoded form value '{0}': ".ToFormat(elementName) +
                Options.Create().Deserialization.FriendlyParseErrorMessages[messageType].ToFormat(""));

            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        [TestCaseSource("SimpleFieldTypes")]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message(string suffix, Type type, object value, string name)
        {
            if (type == typeof(string)) return;

            var form = "{0}=".ToFormat(name + suffix);

            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            var options = Options.Create(x => x.IncludePublicFields()
                .Deserialization(y => y.WithFriendlyParseErrorMessage(messageType, "yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.FormUrlEncoded<SimpleTypeField>(form, options));

            var elementName = messageType.Name + (type.IsNullable() ? "Nullable" : "") + suffix;

            exception.Message.ShouldStartWith(("Error deserializing url encoded form value '{0}' to 'Tests.Deserializer." +
                "FormUrlEncodedTests.SimpleTypeField.{0}': Error parsing ''. ").ToFormat(elementName));
            exception.FriendlyMessage.ShouldEqual("Could not read url encoded form value '{0}': yada".ToFormat(elementName));
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        [Test]
        public void should_fail_to_parse_empty_fields_with_custom_parse_message_using_generic_overload()
        {
            var options = Options.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada")));

            var exception = Assert.Throws<FriendlyMappingException>(() =>
                Deserialize.FormUrlEncoded<SimpleTypeField>("Int32Property=", options));

            exception.Message.ShouldEqual("Error deserializing url encoded form value 'Int32Property' to 'Tests.Deserializer." +
                "FormUrlEncodedTests.SimpleTypeField.Int32Property': Error parsing ''. Input string was not in a correct format.");
            exception.FriendlyMessage.ShouldEqual("Could not read url encoded form value 'Int32Property': yada");
            exception.InnerException.ShouldBeType<ValueParseException>();
        }

        // Out of the box types

        public class OutOfTheBoxTypes
        {
            public IPAddress IPAddress { get; set; }
            public Version Version { get; set; }
            public MailAddress MailAddress { get; set; }
            public byte[] ByteArray { get; set; }
        }

        [Test]
        public void should_deserialize_ip_address()
        {
            Deserialize.FormUrlEncoded<OutOfTheBoxTypes>("IPAddress=192.168.1.1")
                .IPAddress.ShouldEqual(IPAddress.Parse("192.168.1.1"));
        }

        [Test]
        public void should_deserialize_version()
        {
            Deserialize.FormUrlEncoded<OutOfTheBoxTypes>("Version=1.2.3.4")
                .Version.ShouldEqual(Version.Parse("1.2.3.4"));
        }

        [Test]
        public void should_deserialize_mail_address()
        {
            Deserialize.FormUrlEncoded<OutOfTheBoxTypes>("MailAddress=test@test.com")
                .MailAddress.ShouldEqual(new MailAddress("test@test.com"));
        }

        [Test]
        public void should_deserialize_byte_array()
        {
            Deserialize.FormUrlEncoded<OutOfTheBoxTypes>("ByteArray=b2ggaGFp")
                .ByteArray.ShouldEqual(ASCIIEncoding.ASCII.GetBytes("oh hai"));
        }
    }
}
