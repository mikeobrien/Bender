using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Xml
{
    [TestFixture]
    public class WriterTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class CustomWriter
        {
            public DateTime SimpleType { get; set; }
            public ComplexType ComplexType { get; set; }
            public List<int> ListOfSimpleTypes { get; set; }
            public List<ComplexType> ListOfComplexTypes { get; set; }
            public IList<int> ListInterfaceOfSimpleTypes { get; set; }
            public IList<ComplexType> ListInterfaceOfComplexTypes { get; set; }
        }

        // Value writers

        [Test]
        public void should_serialize_simple_type_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<DateTime>((v, s, t, o) => v.ToString("hh-mm"))))
                .SerializeXml(new CustomWriter { SimpleType = DateTime.MaxValue });
            xml.ParseXml().Element("CustomWriter").Element("SimpleType").Value.ShouldEqual("11-59");   
        }

        [Test]
        public void should_serialize_complex_type_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<ComplexType>((v, s, t, o) => v.Value.ToString())))
                .SerializeXml(new CustomWriter { ComplexType = new ComplexType { Value = 5 } });
            xml.ParseXml().Element("CustomWriter").Element("ComplexType").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<List<ComplexType>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.Value.ToString()).ToArray()))))
                .SerializeXml(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            xml.ParseXml().Element("CustomWriter").Element("ListOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<List<int>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.ToString()).ToArray()))))
                .SerializeXml(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            xml.ParseXml().Element("CustomWriter").Element("ListOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<ComplexType>((v, s, t, o) => "[{0}]".ToFormat(v.Value))))
                .SerializeXml(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = xml.ParseXml().Element("CustomWriter").Element("ListOfComplexTypes").Elements("ComplexType");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<int>((v, s, t, o) => "[{0}]".ToFormat(v))))
                .SerializeXml(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = xml.ParseXml().Element("CustomWriter").Element("ListOfSimpleTypes").Elements("Int32");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<IList<ComplexType>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.Value.ToString()).ToArray()))))
                .SerializeXml(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<IList<int>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.ToString()).ToArray()))))
                .SerializeXml(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<ComplexType>((v, s, t, o) => "[{0}]".ToFormat(v.Value))))
                .SerializeXml(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfComplexTypes").Elements("ComplexType");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<int>((v, s, t, o) => "[{0}]".ToFormat(v))))
                .SerializeXml(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfSimpleTypes").Elements("Int32");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        // Node writers

        [Test]
        public void should_apply_node_writer_to_elements()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y
                .AddXmlVisitor((s, t, o) => t.Name = "Yada" + t.Name)))
                    .SerializeXml(new CustomWriter
                    {
                        ComplexType = new ComplexType(),
                        ListOfSimpleTypes = new List<int>(),
                        ListOfComplexTypes = new List<ComplexType>(),
                        ListInterfaceOfSimpleTypes = new List<int>(),
                        ListInterfaceOfComplexTypes = new List<ComplexType>()
                    });
            var results = xml.ParseXml().Element("YadaCustomWriter");
            results.Element("YadaSimpleType").ShouldNotBeNull();
            results.Element("YadaComplexType").ShouldNotBeNull();
            results.Element("YadaListOfSimpleTypes").ShouldNotBeNull();
            results.Element("YadaListOfComplexTypes").ShouldNotBeNull();
            results.Element("YadaListInterfaceOfSimpleTypes").ShouldNotBeNull();
            results.Element("YadaListInterfaceOfComplexTypes").ShouldNotBeNull();
        }

        public class AttributeComplexType
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        [Test]
        public void should_apply_node_writer_to_attributes()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.XmlValuesAsAttributes()
                .AddXmlVisitor((s, t, o) => t.Name = "Yada" + t.Name)))                
                .SerializeXml(new AttributeComplexType { Value2 = "" });
            var results = xml.ParseXml().Element("YadaAttributeComplexType");
            results.Attribute("YadaValue1").ShouldNotBeNull();
            results.Attribute("YadaValue2").ShouldNotBeNull();
        }

        // Built in writers

        public class BuiltInWriters
        {
            public bool Bool { get; set; }
            public byte[] ByteArray { get; set; }
            public Uri Uri { get; set; }
            public Version Version { get; set; }
            public MailAddress MailAddress { get; set; }
            public IPAddress IpAddress { get; set; }
        }

        [Test]
        public void should_serialize_bool()
        {
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { Bool = true }).ParseXml()
                .Element("BuiltInWriters").Element("Bool").Value.ShouldEqual("true");
        }

        [Test]
        public void should_serialize_uri()
        {
            var uri = new Uri("http://www.google.com");
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { Uri = uri }).ParseXml()
                .Element("BuiltInWriters").Element("Uri").Value.ShouldEqual(uri.ToString());
        }

        [Test]
        public void should_serialize_mail_address()
        {
            var email = new MailAddress("test@test.com", "Test");
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { MailAddress = email }).ParseXml()
                .Element("BuiltInWriters").Element("MailAddress").Value.ShouldEqual(email.ToString());
        }

        [Test]
        public void should_serialize_version()
        {
            var version = new Version(1, 2, 3, 4);
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { Version = version }).ParseXml()
                .Element("BuiltInWriters").Element("Version").Value.ShouldEqual(version.ToString());
        }

        [Test]
        public void should_serialize_ip_address()
        {
            var ip = IPAddress.Parse("192.168.1.1");
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { IpAddress = ip }).ParseXml()
                .Element("BuiltInWriters").Element("IpAddress").Value.ShouldEqual(ip.ToString());
        }
    }
}
