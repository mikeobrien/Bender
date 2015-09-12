using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Bender;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
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
        public void should_serialize_fields(string suffix, Type type, object value, string name)
        {
            var memberName = name + suffix;
            var @object = new SimpleTypeField();
            @object.SetPropertyOrFieldValue(memberName, value);
            var result = Serialize.Json(@object, x => x.IncludePublicFields().IncludeMembersWhen((m, o) => m.Name == memberName));

            var json = "{{\"{0}\":{1}}}".ToFormat(memberName,
                type.IsNumeric() || type.IsBoolean() ? 
                    value.ToString().ToLower() : 
                    "\"" + value.ToString().Replace("/", "\\/") + "\"");
            result.ShouldEqual(json);
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
        public void should_serialize_ip_address()
        {
            Serialize.Json(new OutOfTheBoxTypes { IPAddress = IPAddress.Parse("192.168.1.1") })
                .ShouldEqual("{\"IPAddress\":\"192.168.1.1\"}");
        }

        [Test]
        public void should_serialize_version()
        {
            Serialize.Json(new OutOfTheBoxTypes { Version = Version.Parse("1.2.3.4") })
                .ShouldEqual("{\"Version\":\"1.2.3.4\"}");
        }

        [Test]
        public void should_serialize_mail_address()
        {
            Serialize.Json(new OutOfTheBoxTypes { MailAddress = new MailAddress("test@test.com") })
                .ShouldEqual("{\"MailAddress\":\"test@test.com\"}");
        }

        [Test]
        public void should_serialize_byte_array()
        {
            Serialize.Json(new OutOfTheBoxTypes { ByteArray = ASCIIEncoding.ASCII.GetBytes("oh hai") })
                .ShouldEqual("{\"ByteArray\":\"b2ggaGFp\"}");
        }

        [Test]
        public void should_serialize_connection_string()
        {
            Serialize.Json(new OutOfTheBoxTypes { ConnectionString = new SqlConnectionStringBuilder(
                    "server=localhost;database=myapp;Integrated Security=SSPI") })
                .ShouldEqual("{\"ConnectionString\":\"Data Source=localhost;" +
                    "Initial Catalog=myapp;Integrated Security=True\"}");
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
        public void should_serialize_complex_type()
        {
            Serialize.Json(new RootType { Complex = new ComplexType { Property = "hai" }})
                .ShouldEqual("{\"Complex\":{\"Property\":\"hai\"}}");
        }

        [Test]
        public void should_not_serialize_null_complex_type()
        {
            Serialize.Json(new RootType()).ShouldEqual("{}");
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
            Serialize.Json(new ActualType { Actual = "oh", Specified = "hai" }, typeof(ISpecifiedType))
                .ShouldEqual("{\"Specified\":\"hai\"}");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Json(new ActualType { Actual = "oh", Specified = "hai" }, typeof(ISpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("{\"Actual\":\"oh\",\"Specified\":\"hai\"}");
        }

        public class MemberSpecifiedType
        {
            public ISpecifiedType Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(MemberSpecifiedType))
                .ShouldEqual("{\"Specified\":{\"Specified\":\"hai\"}}");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Json(new MemberSpecifiedType { Specified = new ActualType 
                { Actual = "oh", Specified = "hai" } }, typeof(MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("{\"Specified\":{\"Actual\":\"oh\",\"Specified\":\"hai\"}}");
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
            Serialize.Json(new XmlAttributes { AttributeName = "hai" })
                .ShouldEqual("{\"AttributeNameOverride\":\"hai\"}");
        }

        [Test]
        public void should_override_field_name_with_xml_element_attribute()
        {
            Serialize.Json(new XmlAttributes { ElementName = "hai" })
                .ShouldEqual("{\"ElementNameOverride\":\"hai\"}");
        }

        [Test]
        public void should_override_field_name_with_xml_array_attribute()
        {
            Serialize.Json(new XmlAttributes { ArrayName = new List<string> { "hai" } })
                .ShouldEqual("{\"ArrayNameOverride\":[\"hai\"]}");
        }

        [Test]
        public void should_not_override_field_name_with_xml_array_attribute_when_not_an_enumerable()
        {
            Serialize.Json(new XmlAttributes { StringName = "hai" })
                .ShouldEqual("{\"StringName\":\"hai\"}");
        }

        // Filtering types

        public class FilterTypes
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        private static readonly FilterTypes FilterTypesInstance = 
            new FilterTypes { Int = 1, String = "hai" };

        [Test]
        public void should_include_types_when()
        {
            Serialize.Json(FilterTypesInstance, x => x
                .IncludeTypesWhen((t, o) => t.Name == "Int32"))
                .ShouldEqual("{\"Int\":1}");
        }

        [Test]
        public void should_exclude_types()
        {
            Serialize.Json(FilterTypesInstance, x => x
                .ExcludeType<int>())
                .ShouldEqual("{\"String\":\"hai\"}");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Json(FilterTypesInstance, x => x
                .ExcludeTypesWhen((t, o) => t.Name == "Int32"))
                .ShouldEqual("{\"String\":\"hai\"}");
        }

        // Filtering members

        public class FilterMembers
        {
            public FilterMembers(string privateProperty = null, string privateField = null)
            {
                PrivateProperty = privateProperty;
                _privateField = privateField;
            }

            public string Property1 { get; set; }
            public string Property2 { get; set; }

            [XmlIgnore]
            public string IgnoredProperty { get; set; }

            [XmlIgnore]
            public string IgnoredField;

            private string PrivateProperty { get; set; }

            public string Field;
            private string _privateField;
        }

        [Test]
        public void should_include_members_when()
        {
            Serialize.Json(
                new FilterMembers { Property1 = "oh", Property2 = "hai" }, 
                x => x.IncludeMembersWhen((t, o) => t.Name == "Property1"))
                .ShouldEqual("{\"Property1\":\"oh\"}");
        }

        [Test]
        public void should_exclude_members_when()
        {
            Serialize.Json(
                new FilterMembers { Property1 = "oh", Property2 = "hai" }, 
                x => x.ExcludeMembersWhen((t, o) => t.Name == "Property1"))
                .ShouldEqual("{\"Property2\":\"hai\"}");
        }

        [Test]
        public void should_exclude_members_with_xml_ignore_attribute_applied()
        {
            Serialize.Json(
                new FilterMembers { Property1 = "oh", Field = "hai", 
                    IgnoredProperty = "oh", IgnoredField = "hai" }, 
                x => x.IncludePublicFields())
                .ShouldEqual("{\"Property1\":\"oh\",\"Field\":\"hai\"}");
        }

        [Test]
        public void should_exclude_private_properties_by_default()
        {
            Serialize.Json(new FilterMembers("hai")).ShouldEqual("{}");
        }

        [Test]
        public void should_include_private_properties_when_configured()
        {
            Serialize.Json(new FilterMembers("hai"),
                x => x.IncludeNonPublicProperties())
                .ShouldEqual("{\"PrivateProperty\":\"hai\"}");
        }

        [Test]
        public void should_exclude_public_fields_by_default()
        {
            Serialize.Json(new FilterMembers { Field = "hai" }).ShouldEqual("{}");
        }

        [Test]
        public void should_include_public_fields_when_configured()
        {
            Serialize.Json(new FilterMembers { Field = "hai" },
                x => x.IncludePublicFields())
                .ShouldEqual("{\"Field\":\"hai\"}");
        }

        [Test]
        public void should_exclude_private_fields_by_default()
        {
            Serialize.Json(new FilterMembers(privateField: "hai")).ShouldEqual("{}");
        }

        [Test]
        public void should_include_private_fields_when_configured()
        {
            Serialize.Json(new FilterMembers(privateField: "hai"),
                x => x.IncludeNonPublicFields())
                .ShouldEqual("{\"_privateField\":\"hai\"}");
        }

        // Circular references

        public class CircularReference
        {
            public CircularReferenceNode Value { get; set; }
        }

        public class CircularReferenceNode
        {
            public string Value1 { get; set; }
            public CircularReference Value2 { get; set; }
        }

        [Test]
        public void should_not_allow_circular_references()
        {
            var graph = new CircularReference { Value = new CircularReferenceNode { Value1 = "hai" } };
            graph.Value.Value2 = graph;
            Serialize.Json(graph).ShouldEqual("{\"Value\":{\"Value1\":\"hai\"}}");
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
            Serialize.Json(new NamingConventions {PropertyValue = "oh", FieldValue = "hai"},
                x => x.UseSnakeCaseNaming().IncludePublicFields())
                .ShouldEqual("{\"property_value\":\"oh\",\"field_value\":\"hai\"}");
        }

        [Test]
        public void should_use_camel_case_when_configured()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.UseCamelCaseNaming().IncludePublicFields())
                .ShouldEqual("{\"propertyValue\":\"oh\",\"fieldValue\":\"hai\"}");
        }

        [Test]
        public void should_use_json_only_camel_case_when_configured()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.UseJsonCamelCaseNaming().IncludePublicFields())
                .ShouldEqual("{\"propertyValue\":\"oh\",\"fieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_global_naming_convention()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithNamingConvention(y => "$" + y).IncludePublicFields())
                .ShouldEqual("{\"$PropertyValue\":\"oh\",\"$FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_use_conditional_global_naming_convention_conditionally()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithNamingConvention(y => "$" + y,
                    y => y.StartsWith("Property")).IncludePublicFields())
                .ShouldEqual("{\"$PropertyValue\":\"oh\",\"FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_member_name_modification_convention()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithMemberNamingConvention((n, c) => "$" + n).IncludePublicFields())
                .ShouldEqual("{\"$PropertyValue\":\"oh\",\"$FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_member_name_modification_convention_conditionally()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithMemberNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.StartsWith("Property")).IncludePublicFields())
                .ShouldEqual("{\"$PropertyValue\":\"oh\",\"FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_field_name_modification_convention()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithFieldNamingConvention((n, c) => "$" + n).IncludePublicFields())
                .ShouldEqual("{\"PropertyValue\":\"oh\",\"$FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_field_name_modification_convention_conditionally()
        {
            Serialize.Json(new NamingConventions { PropertyValue2 = "oh", FieldValue = "hai", FieldValue2 = "there" },
                x => x.WithFieldNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields())
                .ShouldEqual("{\"PropertyValue2\":\"oh\",\"FieldValue\":\"hai\",\"$FieldValue2\":\"there\"}");
        }

        [Test]
        public void should_apply_property_name_modification_convention()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithPropertyNamingConvention((n, c) => "$" + n).IncludePublicFields())
                .ShouldEqual("{\"$PropertyValue\":\"oh\",\"FieldValue\":\"hai\"}");
        }

        [Test]
        public void should_apply_property_name_modification_convention_conditionally()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", FieldValue2 = "hai", PropertyValue2 = "there" },
                x => x.WithPropertyNamingConvention((n, c) => "$" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields())
                .ShouldEqual("{\"PropertyValue\":\"oh\",\"$PropertyValue2\":\"there\",\"FieldValue2\":\"hai\"}");
        }

        [Test]
        public void should_allow_multiple_naming_conventions_to_be_applied()
        {
            Serialize.Json(new NamingConventions { PropertyValue = "oh", PropertyValue2 = "hai", FieldValue = "o", FieldValue2 = "rly" },
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
                    .IncludePublicFields())
                .ShouldEqual("{\"+(PropertyValue)*\":\"oh\"," +
                  "\"$(PropertyValue2)*|\":\"hai\"," +
                  "\"-[FieldValue]^\":\"o\"," +
                  "\"@[FieldValue2]^%\":\"rly\"}");
        }
    }
}
