using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Bender;
using Bender.Extensions;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class ObjectTests
    {
        // Simple types

        public class SimpleTypeField
        {
            public object ObjectProperty { get; set; }
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

            public string ObjectField;
            public string StringField;
            public Uri UriField;
            public UriFormat EnumField;
            public UriFormat? EnumNullableField;
            public DateTime DateTimeField;
            public DateTime? DateTimeNullableField;
            public TimeSpan TimeSpanField;
            public TimeSpan? TimeSpanNullableField;
            public Guid GuidField;
            public Guid? GuidNullableField;
            public Boolean BooleanField;
            public Boolean? BooleanNullableField;
            public Byte ByteField;
            public Byte? ByteNullableField;
            public SByte SByteField;
            public SByte? SByteNullableField;
            public Int16 Int16Field;
            public Int16? Int16NullableField;
            public UInt16 UInt16Field;
            public UInt16? UInt16NullableField;
            public Int32 Int32Field;
            public Int32? Int32NullableField;
            public UInt32 UInt32Field;
            public UInt32? UInt32NullableField;
            public Int64 Int64Field;
            public Int64? Int64NullableField;
            public UInt64 UInt64Field;
            public UInt64? UInt64NullableField;
            public IntPtr IntPtrField;
            public IntPtr? IntPtrNullableField;
            public UIntPtr UIntPtrField;
            public UIntPtr? UIntPtrNullableField;
            public Char CharField;
            public Char? CharNullableField;
            public Double DoubleField;
            public Double? DoubleNullableField;
            public Single SingleField;
            public Single? SingleNullableField;
            public Decimal DecimalField;
            public Decimal? DecimalNullableField;
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
        public void should_serialize_value_elements(string suffix, Type type, object value, string name)
        {
            var memberName = name + suffix;
            var @object = new SimpleTypeField();
            @object.SetPropertyOrFieldValue(memberName, value);
            var result = Serialize.Xml(@object,
                x => x.IncludePublicFields().IncludeMembersWhen((m, o) => m.Name == memberName));

            var xml = "<SimpleTypeField><{0}>{1}</{0}></SimpleTypeField>".ToFormat(memberName,
                type.GetUnderlyingNullableType() == typeof (bool) ? value.ToString().ToLower() : value);
            result.ShouldEqual(Xml.Declaration + xml);
        }

        [Test]
        [TestCaseSource(nameof(SimpleFieldTypes))]
        public void should_serialize_value_attributes(string suffix, Type type, object value, string name)
        {
            var memberName = name + suffix;
            var @object = new SimpleTypeField();
            @object.SetPropertyOrFieldValue(memberName, value);
            var result = Serialize.Xml(@object,
                x => x.IncludePublicFields().IncludeMembersWhen((m, o) => m.Name == memberName)
                    .Serialization(y => y.XmlValuesAsAttributes()));

            var xml = "<SimpleTypeField {0}=\"{1}\" />".ToFormat(memberName,
                type.GetUnderlyingNullableType() == typeof (bool) ? value.ToString().ToLower() : value);
            result.ShouldEqual(Xml.Declaration + xml);
        }

        private static readonly object[] NullableTypes = TestCases.Create("Property", "Field")
            .AddType<object>("Object")
            .AddType<string>("String")
            .AddType<Uri>("Uri")

            .AddType<UriFormat?>("EnumNullable")

            .AddType<DateTime?>("DateTimeNullable")
            .AddType<TimeSpan?>("TimeSpanNullable")
            .AddType<Guid?>("GuidNullable")

            .AddType<Boolean?>("BooleanNullable")
            .AddType<Byte?>("ByteNullable")
            .AddType<SByte?>("SByteNullable")
            .AddType<Int16?>("Int16Nullable")
            .AddType<UInt16?>("UInt16Nullable")
            .AddType<Int32?>("Int32Nullable")
            .AddType<UInt32?>("UInt32Nullable")
            .AddType<Int64?>("Int64Nullable")
            .AddType<UInt64?>("UInt64Nullable")
            .AddType<IntPtr?>("IntPtrNullable")
            .AddType<UIntPtr?>("UIntPtrNullable")
            .AddType<Char?>("CharNullable")
            .AddType<Double?>("DoubleNullable")
            .AddType<Single?>("SingleNullable")
            .AddType<Decimal?>("DecimalNullable")

            .All;

        [Test]
        [TestCaseSource(nameof(NullableTypes))]
        public void should_serialize_null_members_when_configured(string suffix, Type type, string name)
        {
            var memberName = name + suffix;
            var @object = new SimpleTypeField();
            @object.SetPropertyOrFieldValue(memberName, null);
            var result = Serialize.Xml(@object, x => x
                .IncludePublicFields()
                .IncludeMembersWhen((m, o) => m.Name == memberName)
                .Serialization(y => y.IncludeNullMembers()));

            var xml = "<SimpleTypeField><{0} /></SimpleTypeField>".ToFormat(memberName);
            result.ShouldEqual(Xml.Declaration + xml);
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

        [Test]
        public void Should_serialize_optional_values()
        {
            var model = new OptionalValues
            {
                OptionalReferenceTypeProperty = "fark",
                OptionalValueTypeProperty = 5,
                OptionalNullableTypeProperty = 6,
                OptionalReferenceTypeField = "farker",
                OptionalValueTypeField = 7,
                OptionalNullableTypeField = 8
            };

            var result = Serialize.Xml(model, x => x.IncludePublicFields());

            result.ShouldEqual(Xml.Declaration + 
                "<OptionalValues>" +
                    "<OptionalReferenceTypeProperty>fark</OptionalReferenceTypeProperty>" +
                    "<OptionalValueTypeProperty>5</OptionalValueTypeProperty>" +
                    "<OptionalNullableTypeProperty>6</OptionalNullableTypeProperty>" +
                    "<OptionalReferenceTypeField>farker</OptionalReferenceTypeField>" +
                    "<OptionalValueTypeField>7</OptionalValueTypeField>" +
                    "<OptionalNullableTypeField>8</OptionalNullableTypeField>" +
                "</OptionalValues>");
        }

        [Test]
        public void Should_serialize_empty_optional_value_type_values_and_omit_reference_type_values()
        {
            Serialize.Xml(new OptionalValues(), x => x.IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<OptionalValues />");
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
            Serialize.Xml(new OutOfTheBoxTypes {IPAddress = IPAddress.Parse("192.168.1.1")})
                .ShouldEqual(Xml.Declaration + 
                    "<OutOfTheBoxTypes><IPAddress>192.168.1.1</IPAddress></OutOfTheBoxTypes>");
        }

        [Test]
        public void should_serialize_version()
        {
            Serialize.Xml(new OutOfTheBoxTypes {Version = Version.Parse("1.2.3.4")})
                .ShouldEqual(Xml.Declaration + 
                    "<OutOfTheBoxTypes><Version>1.2.3.4</Version></OutOfTheBoxTypes>");
        }

        [Test]
        public void should_serialize_mail_address()
        {
            Serialize.Xml(new OutOfTheBoxTypes {MailAddress = new MailAddress("test@test.com")})
                .ShouldEqual(Xml.Declaration +
                    "<OutOfTheBoxTypes><MailAddress>test@test.com</MailAddress></OutOfTheBoxTypes>");
        }

        [Test]
        public void should_serialize_byte_array()
        {
            Serialize.Xml(new OutOfTheBoxTypes {ByteArray = ASCIIEncoding.ASCII.GetBytes("oh hai")})
                .ShouldEqual(Xml.Declaration + 
                    "<OutOfTheBoxTypes><ByteArray>b2ggaGFp</ByteArray></OutOfTheBoxTypes>");
        }

        [Test]
        public void should_serialize_connection_string()
        {
            Serialize.Xml(new OutOfTheBoxTypes { ConnectionString = new SqlConnectionStringBuilder(
                    "server=localhost;database=myapp;Integrated Security=SSPI") })
                .ShouldEqual(Xml.Declaration + "<OutOfTheBoxTypes><ConnectionString>Data Source=localhost;" +
                    "Initial Catalog=myapp;Integrated Security=True</ConnectionString></OutOfTheBoxTypes>");
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
            Serialize.Xml(new RootType {Complex = new ComplexType {Property = "hai"}})
                .ShouldEqual(Xml.Declaration + "<RootType><Complex><Property>hai</Property></Complex></RootType>");
        }

        [Test]
        public void should_not_serialize_null_complex_type()
        {
            Serialize.Xml(new RootType()).ShouldEqual(Xml.Declaration + "<RootType />");
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
            Serialize.Xml(new ActualType {Actual = "oh", Specified = "hai"}, typeof (ISpecifiedType))
                .ShouldEqual(Xml.Declaration + "<ISpecifiedType><Specified>hai</Specified></ISpecifiedType>");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Xml(new ActualType {Actual = "oh", Specified = "hai"}, typeof (ISpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration +
                             "<ISpecifiedType><Actual>oh</Actual><Specified>hai</Specified></ISpecifiedType>");
        }

        public class MemberSpecifiedType
        {
            public ISpecifiedType Specified { get; set; }
        }

        [Test]
        public void should_serialize_member_specified_type_by_default()
        {
            Serialize.Xml(new MemberSpecifiedType
            {
                Specified = new ActualType
                {Actual = "oh", Specified = "hai"}
            }, typeof (MemberSpecifiedType))
                .ShouldEqual(Xml.Declaration +
                             "<MemberSpecifiedType><Specified><Specified>hai</Specified></Specified></MemberSpecifiedType>");
        }

        [Test]
        public void should_serialize_member_actual_type_when_configured()
        {
            Serialize.Xml(new MemberSpecifiedType
            {
                Specified = new ActualType
                {Actual = "oh", Specified = "hai"}
            }, typeof (MemberSpecifiedType),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual(Xml.Declaration +
                             "<MemberSpecifiedType><Specified><Actual>oh</Actual><Specified>hai</Specified></Specified></MemberSpecifiedType>");
        }

        // Xml attributes

        [XmlRoot("Root")]
        public class XmlRoot
        {
        }

        [Test]
        public void should_override_root_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlRoot()).ShouldEqual(Xml.Declaration + "<Root />");
        }

        [XmlType("Type")]
        public class XmlType
        {
        }

        [Test]
        public void should_override_type_name_with_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlType()).ShouldEqual(Xml.Declaration + "<Type />");
        }

        public class XmlAttributes
        {
            [Bender.Nodes.Xml.Microsoft.XmlAttribute("AttributeNameOverride")]
            public string BenderXmlAttributeName { get; set; }

            [System.Xml.Serialization.XmlAttribute("AttributeNameOverride")]
            public string SystemXmlAttributeName { get; set; }

            [XmlElement("ElementNameOverride")]
            public string ElementName { get; set; }

            [XmlArray("StringNameOverride")]
            public string StringName { get; set; }

            [XmlArray("ArrayNameOverride")]
            public List<string> ArrayName { get; set; }
        }

        [Test]
        public void should_override_field_name_with_bender_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlAttributes { BenderXmlAttributeName = "hai"})
                .ShouldEqual(Xml.Declaration + "<XmlAttributes AttributeNameOverride=\"hai\" />");
        }

        [Test]
        public void should_override_field_name_with_system_xml_attribute_attribute()
        {
            Serialize.Xml(new XmlAttributes { SystemXmlAttributeName = "hai" })
                .ShouldEqual(Xml.Declaration + "<XmlAttributes AttributeNameOverride=\"hai\" />");
        }

        [Test]
        public void should_override_field_name_with_xml_element_attribute()
        {
            Serialize.Xml(new XmlAttributes {ElementName = "hai"})
                .ShouldEqual(Xml.Declaration +
                             "<XmlAttributes><ElementNameOverride>hai</ElementNameOverride></XmlAttributes>");
        }

        [Test]
        public void should_override_field_name_with_xml_array_attribute()
        {
            Serialize.Xml(new XmlAttributes {ArrayName = new List<string> {"hai"}})
                .ShouldEqual(Xml.Declaration +
                             "<XmlAttributes><ArrayNameOverride><String>hai</String></ArrayNameOverride></XmlAttributes>");
        }

        [Test]
        public void should_not_override_field_name_with_xml_array_attribute_when_not_an_enumerable()
        {
            Serialize.Xml(new XmlAttributes {StringName = "hai"})
                .ShouldEqual(Xml.Declaration + "<XmlAttributes><StringName>hai</StringName></XmlAttributes>");
        }

        public class XmlElementNamespace
        {
            [XmlElement(Namespace = "http://namespace.org")]
            public string Namespace { get; set; }

            [XmlElement(Namespace = "abc")]
            public string Prefix { get; set; }
        }

        [Test]
        public void should_set_property_namespace_prefix_from_xml_attribute()
        {
            Serialize.Xml(new XmlElementNamespace {Namespace = "oh", Prefix = "hai"}, x => x.Serialization(y =>
                y.AddXmlNamespace("abc", "http://namespace.org"))).ShouldEqual(Xml.Declaration +
                    "<XmlElementNamespace xmlns:abc=\"http://namespace.org\">" +
                        "<abc:Namespace>oh</abc:Namespace>" +
                        "<abc:Prefix>hai</abc:Prefix>" +
                    "</XmlElementNamespace>");
        }

        [XmlType(Namespace = "http://namespace.org")]
        public class XmlElementDefaultNamespace
        {
            public string Value { get; set; }
        }

        [Test]
        public void should_set_default_type_namespace_from_xml_attribute()
        {
            Serialize.Xml(new XmlElementDefaultNamespace { Value = "hai" })
                .ShouldEqual(Xml.Declaration +
                    "<XmlElementDefaultNamespace xmlns=\"http://namespace.org\">" +
                        "<Value>hai</Value>" +
                    "</XmlElementDefaultNamespace>");
        }

        [XmlRoot("element", Namespace = "abc")]
        public class XmlRootNamespacePrefix { }

        [XmlRoot("element", Namespace = "http://namespace.org")]
        public class XmlRootNamespace { }

        [XmlType("element", Namespace = "abc")]
        public class XmlTypeNamespacePrefix { }

        [XmlType("element", Namespace = "http://namespace.org")]
        public class XmlTypeNamespace { }

        private static readonly object[][] PrefixAttributeTypeCases =
        {
            new object[] {typeof (XmlRootNamespacePrefix)},
            new object[] {typeof (XmlRootNamespacePrefix)},
            new object[] {typeof (XmlRootNamespacePrefix)},
            new object[] {typeof (XmlRootNamespacePrefix)}
        };

        [Test]
        [TestCaseSource(nameof(PrefixAttributeTypeCases))]
        public void should_set_type_namespace_prefix_from_xml_attribute(Type type)
        {
            Serialize.Xml(type.CreateInstance(), x => x.Serialization(y =>
                y.AddXmlNamespace("abc", "http://namespace.org"))).ShouldEqual(Xml.Declaration +
                    "<abc:element xmlns:abc=\"http://namespace.org\" />");
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
            Serialize.Xml(FilterTypesInstance, x => x
                .IncludeTypesWhen((t, o) => t.Name == "Int32"))
                .ShouldEqual(Xml.Declaration + "<FilterTypes><Int>1</Int></FilterTypes>");
        }

        [Test]
        public void should_exclude_types()
        {
            Serialize.Xml(FilterTypesInstance, x => x
                .ExcludeType<int>())
                .ShouldEqual(Xml.Declaration + "<FilterTypes><String>hai</String></FilterTypes>");
        }

        [Test]
        public void should_exclude_types_when()
        {
            Serialize.Xml(FilterTypesInstance, x => x
                .ExcludeTypesWhen((t, o) => t.Name == "Int32"))
                .ShouldEqual(Xml.Declaration + "<FilterTypes><String>hai</String></FilterTypes>");
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
            Serialize.Xml(
                new FilterMembers { Property1 = "oh", Property2 = "hai" }, 
                x => x.IncludeMembersWhen((t, o) => t.Name == "Property1"))
                .ShouldEqual(Xml.Declaration + "<FilterMembers><Property1>oh</Property1></FilterMembers>");
        }

        [Test]
        public void should_exclude_members_when()
        {
            Serialize.Xml(
                new FilterMembers { Property1 = "oh", Property2 = "hai" }, 
                x => x.ExcludeMembersWhen((t, o) => t.Name == "Property1"))
                .ShouldEqual(Xml.Declaration + "<FilterMembers><Property2>hai</Property2></FilterMembers>");
        }

        [Test]
        public void should_exclude_members_with_xml_ignore_attribute_applied()
        {
            Serialize.Xml(
                new FilterMembers { Property1 = "oh", Field = "hai", 
                    IgnoredProperty = "oh", IgnoredField = "hai" }, 
                x => x.IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<FilterMembers><Property1>oh</Property1><Field>hai</Field></FilterMembers>");
        }

        [Test]
        public void should_exclude_private_properties_by_default()
        {
            Serialize.Xml(new FilterMembers("hai")).ShouldEqual(Xml.Declaration + "<FilterMembers />");
        }

        [Test]
        public void should_include_private_properties_when_configured()
        {
            Serialize.Xml(new FilterMembers("hai"),
                x => x.IncludeNonPublicProperties())
                .ShouldEqual(Xml.Declaration + "<FilterMembers><PrivateProperty>hai</PrivateProperty></FilterMembers>");
        }

        [Test]
        public void should_exclude_public_fields_by_default()
        {
            Serialize.Xml(new FilterMembers { Field = "hai" }).ShouldEqual(Xml.Declaration + "<FilterMembers />");
        }

        [Test]
        public void should_include_public_fields_when_configured()
        {
            Serialize.Xml(new FilterMembers { Field = "hai" },
                x => x.IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<FilterMembers><Field>hai</Field></FilterMembers>");
        }

        [Test]
        public void should_exclude_private_fields_by_default()
        {
            Serialize.Xml(new FilterMembers(privateField: "hai")).ShouldEqual(Xml.Declaration + "<FilterMembers />");
        }

        [Test]
        public void should_include_private_fields_when_configured()
        {
            Serialize.Xml(new FilterMembers(privateField: "hai"),
                x => x.IncludeNonPublicFields())
                .ShouldEqual(Xml.Declaration + "<FilterMembers><_privateField>hai</_privateField></FilterMembers>");
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
            Serialize.Xml(graph).ShouldEqual(Xml.Declaration + "<CircularReference><Value><Value1>hai</Value1></Value></CircularReference>");
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
            Serialize.Xml(new NamingConventions {PropertyValue = "oh", FieldValue = "hai"},
                x => x.UseSnakeCaseNaming().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<naming_conventions><property_value>oh</property_value><field_value>hai</field_value></naming_conventions>");
        }

        [Test]
        public void should_use_camel_case_when_configured()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.UseCamelCaseNaming().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<namingConventions><propertyValue>oh</propertyValue><fieldValue>hai</fieldValue></namingConventions>");
        }

        [Test]
        public void should_use_xml_only_spinal_case_when_configured()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.UseXmlSpinalCaseNaming().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<naming-conventions><property-value>oh</property-value><field-value>hai</field-value></naming-conventions>");
        }

        [Test]
        public void should_use_xml_only_train_case_when_configured()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.UseXmlTrainCaseNaming().IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<Naming-Conventions><Property-Value>oh</Property-Value><Field-Value>hai</Field-Value></Naming-Conventions>");
        }

        [Test]
        public void should_apply_global_naming_convention()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithNamingConvention(y => "_" + y).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<_NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></_NamingConventions>");
        }

        [Test]
        public void should_use_conditional_global_naming_convention_conditionally()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithNamingConvention(y => "_" + y,
                    y => y.StartsWith("Property")).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><_PropertyValue>oh</_PropertyValue><FieldValue>hai</FieldValue></NamingConventions>");
        }

        [Test]
        public void should_apply_member_name_modification_convention()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithMemberNamingConvention((n, c) => "_" + n).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><_PropertyValue>oh</_PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>");
        }

        [Test]
        public void should_apply_member_name_modification_convention_conditionally()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithMemberNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.StartsWith("Property")).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><_PropertyValue>oh</_PropertyValue><FieldValue>hai</FieldValue></NamingConventions>");
        }

        [Test]
        public void should_apply_field_name_modification_convention()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithFieldNamingConvention((n, c) => "_" + n).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><PropertyValue>oh</PropertyValue><_FieldValue>hai</_FieldValue></NamingConventions>");
        }

        [Test]
        public void should_apply_field_name_modification_convention_conditionally()
        {
            Serialize.Xml(new NamingConventions { PropertyValue2 = "oh", FieldValue = "hai", FieldValue2 = "there" },
                x => x.WithFieldNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><PropertyValue2>oh</PropertyValue2><FieldValue>hai</FieldValue><_FieldValue2>there</_FieldValue2></NamingConventions>");
        }

        [Test]
        public void should_apply_property_name_modification_convention()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue = "hai" },
                x => x.WithPropertyNamingConvention((n, c) => "_" + n).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><_PropertyValue>oh</_PropertyValue><FieldValue>hai</FieldValue></NamingConventions>");
        }

        [Test]
        public void should_apply_property_name_modification_convention_conditionally()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", FieldValue2 = "hai", PropertyValue2 = "there" },
                x => x.WithPropertyNamingConvention((n, c) => "_" + n,
                    (n, c) => c.Member.Name.EndsWith("2")).IncludePublicFields())
                .ShouldEqual(Xml.Declaration + "<NamingConventions><PropertyValue>oh</PropertyValue><_PropertyValue2>there</_PropertyValue2><FieldValue2>hai</FieldValue2></NamingConventions>");
        }

        [Test]
        public void should_allow_multiple_naming_conventions_to_be_applied()
        {
            Serialize.Xml(new NamingConventions { PropertyValue = "oh", PropertyValue2 = "hai", FieldValue = "o", FieldValue2 = "rly" },
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
                    .IncludePublicFields())
                .ShouldEqual(Xml.Declaration + 
                    "<NamingConventions>" + 
                        "<caPropertyValueag>oh</caPropertyValueag>" +
                        "<daPropertyValue2agh>hai</daPropertyValue2agh>" +
                        "<ebFieldValuebi>o</ebFieldValuebi>" +
                        "<fbFieldValue2bij>rly</fbFieldValue2bij>" +
                    "</NamingConventions>");
        }

        public class GenericTypeNameConvention<T> { }

        [Test]
        public void should_use_xml_serializer_generic_type_name_format_by_default()
        {
            Serialize.Xml(new GenericTypeNameConvention<string>())
                .ShouldEqual(Xml.Declaration + "<GenericTypeNameConventionOfString />");
        }

        [Test]
        public void should_use_configured_generic_type_name_format()
        {
            Serialize.Xml(new GenericTypeNameConvention<string>(),
                x => x.WithGenericTypeNameFormat("{0}_{1}"))
                .ShouldEqual(Xml.Declaration + "<GenericTypeNameConvention_String />");
        }

        // Encoding

        public class Encoding { public string Oh { get; set; } }

        [Test]
        public void should_encode_pretty()
        {
            Serialize.Xml(new Encoding { Oh = "hai" },
                x => x.Serialization(y => y.PrettyPrint()))
                .ShouldEqual(Xml.Declaration + "\r\n<Encoding>\r\n\t<Oh>hai</Oh>\r\n</Encoding>");
        }

        [Test]
        public void should_encode_members_as_attributes()
        {
            Serialize.Xml(new Encoding { Oh = "hai" },
                x => x.Serialization(y => y.XmlValuesAsAttributes()))
                .ShouldEqual(Xml.Declaration + "<Encoding Oh=\"hai\" />");
        }

        // Namespaces

        public class Namespaces { public string Oh { get; set; } }

        [Test]
        public void should_set_default_namespace()
        {
            Serialize.Xml(new Namespaces { Oh = "hai" },
                x => x.Serialization(y => y.WithDefaultXmlNamespace("urn:yada")))
                .ShouldEqual(Xml.Declaration + "<Namespaces xmlns=\"urn:yada\"><Oh>hai</Oh></Namespaces>");
        }

        [Test]
        public void should_add_namespace_prefix_to_element()
        {
            Serialize.Xml(new Namespaces { Oh = "hai" },
                x => x.Serialization(y => y.AddXmlNamespace("abc", "urn:yada").AddXmlVisitor<string>((s, t, o) => t.SetNamespacePrefix("abc"))))
                .ShouldEqual(Xml.Declaration + "<Namespaces xmlns:abc=\"urn:yada\"><abc:Oh>hai</abc:Oh></Namespaces>");
        }

        [Test]
        public void should_add_namespace_prefix_to_attribute()
        {
            Serialize.Xml(new Namespaces { Oh = "hai" },
                x => x.Serialization(y => y.XmlValuesAsAttributes().AddXmlNamespace("abc", "urn:yada")
                    .AddXmlVisitor<string>((s, t, o) => t.SetNamespacePrefix("abc"))))
                .ShouldEqual(Xml.Declaration + "<Namespaces xmlns:abc=\"urn:yada\" abc:Oh=\"hai\" />");
        }

        // Attributes

        public class WithAttribute
        {
            [WithAttribute("fark", "farker")]
            public string Content { get; set; }
        }

        [Test]
        public void should_add_attribute_to_element()
        {
            Serialize.Xml(new WithAttribute { Content = "hai" })
                .ShouldEqual(Xml.Declaration + "<WithAttribute><Content fark=\"farker\">hai</Content></WithAttribute>");
        }

        // Non numeric float handling

        public class FloatValue
        {
            public float Value { get; set; }
        }

        [Test]
        public void should_return_raw_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new FloatValue { Value = value })
                .ShouldEqual($"{Xml.Declaration}<FloatValue><Value>{value}</Value></FloatValue>");
        }

        [Test]
        public void should_return_name_of_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new FloatValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsName()))
                .ShouldEqual($"{Xml.Declaration}<FloatValue><Value>{value}</Value></FloatValue>");
        }

        [Test]
        public void should_return_zero_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new FloatValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsZero()))
                .ShouldEqual($"{Xml.Declaration}<FloatValue><Value>0</Value></FloatValue>");
        }

        public class NullableFloatValue
        {
            public float? Value { get; set; }
        }

        [Test]
        public void should_return_raw_non_numeric_nullable_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new NullableFloatValue { Value = value })
                .ShouldEqual($"{Xml.Declaration}<NullableFloatValue><Value>{value}</Value></NullableFloatValue>");
        }

        [Test]
        public void should_return_name_of_non_numeric_nullable_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new NullableFloatValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsName()))
                .ShouldEqual($"{Xml.Declaration}<NullableFloatValue><Value>{value}</Value></NullableFloatValue>");
        }

        [Test]
        public void should_return_zero_non_numeric_nullable_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity)] float value)
        {
            Serialize.Xml(new NullableFloatValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsZero()))
                .ShouldEqual($"{Xml.Declaration}<NullableFloatValue><Value>0</Value></NullableFloatValue>");
        }

        public class DoubleValue
        {
            public double Value { get; set; }
        }

        [Test]
        public void should_return_raw_non_numeric_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new DoubleValue { Value = value })
                .ShouldEqual($"{Xml.Declaration}<DoubleValue><Value>{value}</Value></DoubleValue>");
        }

        [Test]
        public void should_return_name_of_non_numeric_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new DoubleValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsName()))
                .ShouldEqual($"{Xml.Declaration}<DoubleValue><Value>{value}</Value></DoubleValue>");
        }

        [Test]
        public void should_return_zero_non_numeric_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new DoubleValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsZero()))
                .ShouldEqual($"{Xml.Declaration}<DoubleValue><Value>0</Value></DoubleValue>");
        }

        public class NullableDoubleValue
        {
            public double? Value { get; set; }
        }

        [Test]
        public void should_return_raw_non_numeric_nullable_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new NullableDoubleValue { Value = value })
                .ShouldEqual($"{Xml.Declaration}<NullableDoubleValue><Value>{value}</Value></NullableDoubleValue>");
        }

        [Test]
        public void should_return_name_of_non_numeric_nullable_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new NullableDoubleValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsName()))
                .ShouldEqual($"{Xml.Declaration}<NullableDoubleValue><Value>{value}</Value></NullableDoubleValue>");
        }

        [Test]
        public void should_return_zero_non_numeric_nullable_double_when_configured(
            [Values(double.NaN, double.NegativeInfinity, double.PositiveInfinity)] double value)
        {
            Serialize.Xml(new NullableDoubleValue { Value = value },
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsZero()))
                .ShouldEqual($"{Xml.Declaration}<NullableDoubleValue><Value>0</Value></NullableDoubleValue>");
        }
    }
}
