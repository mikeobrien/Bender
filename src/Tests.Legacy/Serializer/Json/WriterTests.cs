using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Bender.Extensions;
using Flexo;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Json
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
            var json = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<DateTime>((v, s, t, o) => v.ToString("hh-mm"))))
                .SerializeJson(new CustomWriter { SimpleType = DateTime.MaxValue });
            json.ParseJson().JsonRoot().JsonStringField("SimpleType").Value.ShouldEqual("11-59");   
        }

        [Test]
        public void should_serialize_complex_type_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y =>
                y.AddWriter<ComplexType>((v, s, t, o) => v.Value.ToString())))
                .SerializeJson(new CustomWriter { ComplexType = new ComplexType { Value = 5 } });
            json.ParseJson().JsonRoot().JsonStringField("ComplexType").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<List<ComplexType>>((v, s, t, o) => 
                string.Join(",", v.Select(z => z.Value.ToString()).ToArray()))))
                .SerializeJson(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            json.ParseJson().JsonRoot().JsonStringField("ListOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_property_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<List<int>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.ToString()).ToArray()))))
                .SerializeJson(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            json.ParseJson().JsonRoot().JsonStringField("ListOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_property_item_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y =>
                y.AddWriter<ComplexType>((v, s, t, o) => "[{0}]".ToFormat(v.Value))))
                .SerializeJson(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = json.ParseJson().JsonRoot().JsonStringArrayField("ListOfComplexTypes");
            results.Count().ShouldEqual(3);
            results.JsonStringItem(1).Value.ShouldEqual("[1]");
            results.JsonStringItem(2).Value.ShouldEqual("[2]");
            results.JsonStringItem(3).Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_property_item_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y
                .AddWriter<int>((v, s, t, o) => "[{0}]".ToFormat(v))
                .AddJsonVisitor<int>((s, t, o) => t.Element.Type = ElementType.String)))
                    .SerializeJson(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = json.ParseJson().JsonRoot().JsonStringArrayField("ListOfSimpleTypes");
            results.Count().ShouldEqual(3);
            results.JsonStringItem(1).Value.ShouldEqual("[1]");
            results.JsonStringItem(2).Value.ShouldEqual("[2]");
            results.JsonStringItem(3).Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<IList<ComplexType>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.Value.ToString()).ToArray()))))
                .SerializeJson(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            json.ParseJson().JsonRoot().JsonStringField("ListInterfaceOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y.AddWriter<IList<int>>(
                    (v, s, t, o) => string.Join(",", v.Select(z => z.ToString()).ToArray()))))
                .SerializeJson(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            json.ParseJson().JsonRoot().JsonStringField("ListInterfaceOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_item_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => 
                y.AddWriter<ComplexType>((v, s, t, o) => "[{0}]".ToFormat(v.Value))))
                .SerializeJson(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = json.ParseJson().JsonRoot().JsonStringArrayField("ListInterfaceOfComplexTypes");
            results.Count().ShouldEqual(3);
            results.JsonStringItem(1).Value.ShouldEqual("[1]");
            results.JsonStringItem(2).Value.ShouldEqual("[2]");
            results.JsonStringItem(3).Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_item_with_custom_writer()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y
                .AddWriter<int>((v, s, t, o) => "[{0}]".ToFormat(v))
                .AddJsonVisitor<int>((s, t, o) => t.Element.Type = ElementType.String)))
                    .SerializeJson(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = json.ParseJson().JsonRoot().JsonStringArrayField("ListInterfaceOfSimpleTypes");
            results.Count().ShouldEqual(3);
            results.JsonStringItem(1).Value.ShouldEqual("[1]");
            results.JsonStringItem(2).Value.ShouldEqual("[2]");
            results.JsonStringItem(3).Value.ShouldEqual("[3]"); 
        }

        // Node writers

        [Test]
        public void should_apply_node_writer_to_elements()
        {
            var json = Bender.Serializer.Create(x => x.Serialization(y => y
                .AddJsonVisitor((s, t, o) => t.Name = "Yada" + t.Name, (s, t, o) => t.IsNamed)))
                    .SerializeJson(new CustomWriter
                    {
                        ComplexType = new ComplexType(),
                        ListOfSimpleTypes = new List<int>(),
                        ListOfComplexTypes = new List<ComplexType>(),
                        ListInterfaceOfSimpleTypes = new List<int>(),
                        ListInterfaceOfComplexTypes = new List<ComplexType>()
                    });
            var results = json.ParseJson().JsonRoot();
            results.JsonFieldExists("YadaSimpleType").ShouldBeTrue();
            results.JsonFieldExists("YadaComplexType").ShouldBeTrue();
            results.JsonFieldExists("YadaListOfSimpleTypes").ShouldBeTrue();
            results.JsonFieldExists("YadaListOfComplexTypes").ShouldBeTrue();
            results.JsonFieldExists("YadaListInterfaceOfSimpleTypes").ShouldBeTrue();
            results.JsonFieldExists("YadaListInterfaceOfComplexTypes").ShouldBeTrue();
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
            Bender.Serializer.Create().SerializeJson(new BuiltInWriters { Bool = true }).ParseJson()
                .JsonRoot().JsonBooleanField("Bool").Value.ShouldEqual("true");
        }

        //[Test]
        //public void should_serialize_byte_array()
        //{
        //    var bytes = new byte[] { 1, 2, 3 };
        //    Bender.Serializer.Create().SerializeJson(new BuiltInWriters { ByteArray = bytes }).ParseJson()
        //        .JsonRoot().JsonStringField("ByteArray").Value.ShouldEqual(Convert.ToBase64String(bytes));
        //}

        [Test]
        public void should_serialize_uri()
        {
            var uri = new Uri("http://www.google.com");
            Bender.Serializer.Create().SerializeJson(new BuiltInWriters { Uri = uri }).ParseJson()
                .JsonRoot().JsonStringField("Uri").Value.ShouldEqual(uri.ToString());
        }

        [Test]
        public void should_serialize_mail_address()
        {
            var email = new MailAddress("test@test.com", "Test");
            Bender.Serializer.Create().SerializeJson(new BuiltInWriters { MailAddress = email }).ParseJson()
                .JsonRoot().JsonStringField("MailAddress").Value.ShouldEqual(email.ToString());
        }

        [Test]
        public void should_serialize_version()
        {
            var version = new Version(1, 2, 3, 4);
            Bender.Serializer.Create().SerializeJson(new BuiltInWriters { Version = version }).ParseJson()
                .JsonRoot().JsonStringField("Version").Value.ShouldEqual(version.ToString());
        }

        [Test]
        public void should_serialize_ip_address()
        {
            var ip = IPAddress.Parse("192.168.1.1");
            Bender.Serializer.Create().SerializeJson(new BuiltInWriters { IpAddress = ip }).ParseJson()
                .JsonRoot().JsonStringField("IpAddress").Value.ShouldEqual(ip.ToString());
        }
    }
}
