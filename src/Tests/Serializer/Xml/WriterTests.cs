using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
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
            var xml = Bender.Serializer.Create(x => x.AddWriter<DateTime>(y => y.Node.Value = y.Value.ToString("hh-mm")))
                .SerializeXml(new CustomWriter { SimpleType = DateTime.MaxValue });
            xml.ParseXml().Element("CustomWriter").Element("SimpleType").Value.ShouldEqual("11-59");   
        }

        [Test]
        public void should_serialize_complex_type_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>(y => y.Node.Value = y.Value.Value.ToString()))
                .SerializeXml(new CustomWriter { ComplexType = new ComplexType { Value = 5 } });
            xml.ParseXml().Element("CustomWriter").Element("ComplexType").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<List<ComplexType>>(y => 
                    y.Node.Value = string.Join(",", y.Value.Select(z => z.Value.ToString()).ToArray())))
                .SerializeXml(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            xml.ParseXml().Element("CustomWriter").Element("ListOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<List<int>>(y => 
                    y.Node.Value = string.Join(",", y.Value.Select(z => z.ToString()).ToArray())))
                .SerializeXml(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            xml.ParseXml().Element("CustomWriter").Element("ListOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>(y => { if (y.Value != null) y.Node.Value = "[{0}]".ToFormat(y.Value.Value); }))
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
            var xml = Bender.Serializer.Create(x => x.AddWriter<int>(y => y.Node.Value = "[{0}]".ToFormat(y.Value)))
                .SerializeXml(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = xml.ParseXml().Element("CustomWriter").Element("ListOfSimpleTypes").Elements("Int32");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<IList<ComplexType>>(y => 
                    y.Node.Value = string.Join(",", y.Value.Select(z => z.Value.ToString()).ToArray())))
                .SerializeXml(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<IList<int>>(y =>
                    y.Node.Value = string.Join(",", y.Value.Select(z => z.ToString()).ToArray())))
                .SerializeXml(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            xml.ParseXml().Element("CustomWriter").Element("ListInterfaceOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>(y => { if (y.Value != null) y.Node.Value = "[{0}]".ToFormat(y.Value.Value); }))
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
            var xml = Bender.Serializer.Create(x => x.AddWriter<int>(y => y.Node.Value = "[{0}]".ToFormat(y.Value)))
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
            var xml = Bender.Serializer.Create(x => x
                .AddWriter(y => y.Value == null, y => y.Node.Name = "Null" + y.Node.Name)).SerializeXml(new CustomWriter());
            var results = xml.ParseXml().Element("CustomWriter");
            results.Element("SimpleType").ShouldNotBeNull();
            results.Element("NullComplexType").ShouldNotBeNull();
            results.Element("NullListOfSimpleTypes").ShouldNotBeNull();
            results.Element("NullListOfComplexTypes").ShouldNotBeNull();
            results.Element("NullListInterfaceOfSimpleTypes").ShouldNotBeNull();
            results.Element("NullListInterfaceOfComplexTypes").ShouldNotBeNull();
        }

        public class AttributeComplexType
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        [Test]
        public void should_apply_node_writer_to_attributes()
        {
            var xml = Bender.Serializer.Create(x => x.XmlValuesAsAttributes()
                .AddWriter(y => y.Value == null, y => y.Node.Name = "Null" + y.Node.Name)).SerializeXml(new AttributeComplexType());
            var results = xml.ParseXml().Element("AttributeComplexType");
            results.Attribute("Value1").ShouldNotBeNull();
            results.Attribute("NullValue2").ShouldNotBeNull();
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
        public void should_serialize_byte_array()
        {
            var bytes = new byte[] { 1, 2, 3 };
            Bender.Serializer.Create().SerializeXml(new BuiltInWriters { ByteArray = bytes }).ParseXml()
                .Element("BuiltInWriters").Element("ByteArray").Value.ShouldEqual(Convert.ToBase64String(bytes));
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
