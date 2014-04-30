using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Bender;
using Bender.Nodes;
using Bender.Nodes.Object;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Json
{
    [TestFixture]
    public class ReaderTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class CustomReader
        {
            public DateTime SimpleType { get; set; }
            public DateTime? SimpleNullableType { get; set; }
            public ComplexType ComplexType { get; set; }
            public List<int> ListOfSimpleTypes { get; set; }
            public List<ComplexType> ListOfComplexTypes { get; set; }
            public IList<int> ListInterfaceOfSimpleTypes { get; set; }
            public IList<ComplexType> ListInterfaceOfComplexTypes { get; set; }
        }

        [Test]
        public void should_deserialize_simple_type_with_custom_reader()
        {
            const string json = "{ \"SimpleType\": \"11-59\", \"SimpleNullableType\": \"10-49\" }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<DateTime>((s, t, o) => DateTime.ParseExact(s.Value.ToString(), "hh-mm", CultureInfo.InvariantCulture), true)))
                    .DeserializeJson<CustomReader>(json);

            result.SimpleType.ShouldEqual(DateTime.ParseExact("11-59", "hh-mm", CultureInfo.InvariantCulture));
            result.SimpleNullableType.ShouldEqual(DateTime.ParseExact("10-49", "hh-mm", CultureInfo.InvariantCulture));
        }

        [Test]
        public void should_return_parse_error_when_reader_fails()
        {
            var deserializer = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<DateTime>((s, t, o) => DateTime.ParseExact(s.Value.ToString(), "h:m:s", CultureInfo.InvariantCulture), true)));

            Assert.Throws<FriendlyMappingException>(() => deserializer.DeserializeJson<CustomReader>("{ \"SimpleType\": \"11-59\" }"))
                .FriendlyMessage.ShouldContain("Date '11-59' not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
            Assert.Throws<FriendlyMappingException>(() => deserializer.DeserializeJson<CustomReader>("{ \"SimpleNullableType\": \"10-49\" }"))
                .FriendlyMessage.ShouldContain("Date '10-49' not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
        }

        [Test]
        public void should_return_parse_error_with_custom_message_when_reader_fails()
        {
            var deserializer = Bender.Deserializer.Create(x => x.Deserialization(y => y
                    .WithFriendlyParseErrorMessage<DateTime>("Frog is wrong.")
                    .AddReader<DateTime>((s, t, o) => DateTime.ParseExact(s.Value.ToString(), "h:m:s", CultureInfo.InvariantCulture), true)));

            Assert.Throws<FriendlyMappingException>(() => deserializer.DeserializeJson<CustomReader>("{ \"SimpleType\": \"11-59\" }"))
                .FriendlyMessage.ShouldContain("Frog is wrong.");
            Assert.Throws<FriendlyMappingException>(() => deserializer.DeserializeJson<CustomReader>("{ \"SimpleNullableType\": \"10-49\" }"))
                .FriendlyMessage.ShouldContain("Frog is wrong.");
        }

        [Test]
        public void should_deserialize_complex_type_with_custom_reader()
        {
            const string json = "{ \"ComplexType\": 5 }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<ComplexType>((s, t, o) => new ComplexType { Value = int.Parse(s.Value.ToString()) })))
                    .DeserializeJson<CustomReader>(json);

            result.ComplexType.Value.ShouldEqual(5);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_with_custom_reader()
        {
            const string json = "{ \"ListOfComplexTypes\": \"1, 2, 3\" }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<List<ComplexType>>((s, t, o) => s.Value.ToString().Split(',').Select(z => new ComplexType { Value = int.Parse(z) }).ToList())))
                    .DeserializeJson<CustomReader>(json);
            
            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_with_custom_reader()
        {
            const string json = "{ \"ListOfSimpleTypes\": \"1, 2, 3\" }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<List<int>>((s, t, o) => s.Value.ToString().Split(',').Select(int.Parse).ToList())))
                    .DeserializeJson<CustomReader>(json);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_item_with_custom_reader()
        {
            const string json = "{ \"ListOfComplexTypes\": [ \"[1]\", \"[2]\", \"[3]\" ] }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<ComplexType>((s, t, o) => new ComplexType { Value = int.Parse(s.Value.ToString().Replace("[", "").Replace("]", "")) })))
                    .DeserializeJson<CustomReader>(json);

            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_item_with_custom_reader()
        {
            const string json = "{ \"ListOfSimpleTypes\": [ \"[1]\", \"[2]\", \"[3]\" ] }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<int>((s, t, o) => int.Parse(s.Value.ToString().Replace("[", "").Replace("]", "")))))
                    .DeserializeJson<CustomReader>(json);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_with_custom_reader()
        {
            const string json = "{ \"ListInterfaceOfComplexTypes\": \"1, 2, 3\" }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<IList<ComplexType>>((s, t, o) => s.Value.ToString().Split(',').Select(z => new ComplexType { Value = int.Parse(z) }).ToList())))
                    .DeserializeJson<CustomReader>(json);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_with_custom_reader()
        {
            const string json = "{ \"ListInterfaceOfSimpleTypes\": \"1, 2, 3\" }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<IList<int>>((s, t, o) => s.Value.ToString().Split(',').Select(int.Parse).ToList())))
                    .DeserializeJson<CustomReader>(json);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_item_with_custom_reader()
        {
            const string json = "{ \"ListInterfaceOfComplexTypes\": [ \"[1]\", \"[2]\", \"[3]\" ] }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<ComplexType>((s, t, o) => new ComplexType { Value = int.Parse(s.Value.ToString().Replace("[", "").Replace("]", "")) })))
                    .DeserializeJson<CustomReader>(json);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_item_with_custom_reader()
        {
            const string json = "{ \"ListInterfaceOfSimpleTypes\": [ \"[1]\", \"[2]\", \"[3]\" ] }";
            var result = Bender.Deserializer.Create(
                x => x.Deserialization(y => y.AddReader<int>((s, t, o) => int.Parse(s.Value.ToString().Replace("[", "").Replace("]", "")))))
                    .DeserializeJson<CustomReader>(json);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        // Built in readers

        public class BuiltInReaders
        {
            public byte[] ByteArray { get; set; }
            public Uri Uri { get; set; }
            public Version Version { get; set; }
            public MailAddress MailAddress { get; set; }
            public IPAddress IpAddress { get; set; }
        }

        [Test]
        public void should_not_deserialize_improperly_formated_byte_array()
        {
            //const string json = "{ \"ByteArray\": \"56y45u456u\" }";
            //Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json)).FriendlyMessage
            //    .ShouldContain("Unable to parse the value '56y45u456u' in the 'ByteArray' field as a Byte[]: Not formatted correctly, must be formatted as base64 string.");
        }

        [Test]
        public void should_not_deserialize_improperly_formated_uri()
        {
            const string json = "{ \"Uri\": \"46b464646436\" }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json)).FriendlyMessage
                .ShouldContain("Uri '46b464646436' not formatted correctly, must be formatted as 'scheme://host'.");
        }

        [Test]
        public void should_deserialize_mail_address()
        {
            const string json = "{ \"MailAddress\": \"\\\"Test\\\" <test@test.com>\" }";
            Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json).MailAddress.ShouldEqual(new MailAddress("test@test.com", "Test"));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_mail_address()
        {
            const string json = "{ \"MailAddress\": \"34b64634b643b6\" }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json)).FriendlyMessage
                .ShouldContain("Email address '34b64634b643b6' not formatted correctly, must be formatted as 'username@domain.com'.");
        }

        [Test]
        public void should_deserialize_version()
        {
            const string json = "{ \"Version\": \"1.2.3.4\" }";
            Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json).Version.ShouldEqual(new Version(1, 2, 3, 4));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_version()
        {
            const string json = "{ \"Version\": \"4b6345b6345b634\" }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json)).FriendlyMessage
                .ShouldContain("Version '4b6345b6345b634' not formatted correctly, must be formatted as '1.2.3.4'.");
        }

        [Test]
        public void should_deserialize_ip_address()
        {
            const string json = "{ \"IpAddress\": \"192.168.1.1\" }";
            Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json).IpAddress.ShouldEqual(IPAddress.Parse("192.168.1.1"));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_ip_address()
        {
            const string json = "{ \"IpAddress\": \"45b634b6345b6345\" }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<BuiltInReaders>(json)).FriendlyMessage
                .ShouldContain("IP address '45b634b6345b6345' not formatted correctly, must be formatted as '1.2.3.4'.");
        }
    }
}
