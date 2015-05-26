using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Bender;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
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
        public void should_serialize_fields(string suffix, Type type, object value, string name)
        {
            var memberName = name + suffix;
            var @object = new SimpleTypeField();
            @object.SetPropertyOrFieldValue(memberName, value);
            var result = Serialize.FormUrlEncoded(@object, x => x.IncludePublicFields().IncludeMembersWhen((m, o) => m.Name == memberName));

            var json = "{0}={1}".ToFormat(memberName,
                type.IsBoolean() ? value.ToString().ToLower() : HttpUtility.UrlEncode(value.ToString()));
            result.ShouldEqual(json);
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
        public void should_serialize_ip_address()
        {
            Serialize.FormUrlEncoded(new OutOfTheBoxTypes { IPAddress = IPAddress.Parse("192.168.1.1") })
                .ShouldEqual("IPAddress=192.168.1.1");
        }

        [Test]
        public void should_serialize_version()
        {
            Serialize.FormUrlEncoded(new OutOfTheBoxTypes { Version = Version.Parse("1.2.3.4") })
                .ShouldEqual("Version=1.2.3.4");
        }

        [Test]
        public void should_serialize_mail_address()
        {
            Serialize.FormUrlEncoded(new OutOfTheBoxTypes { MailAddress = new MailAddress("test@test.com") })
                .ShouldEqual("MailAddress=test%40test.com");
        }

        [Test]
        public void should_serialize_byte_array()
        {
            Serialize.FormUrlEncoded(new OutOfTheBoxTypes { ByteArray = ASCIIEncoding.ASCII.GetBytes("oh hai") })
                .ShouldEqual("ByteArray=b2ggaGFp");
        }
    }
}
