using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
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
            const string xml = @"
                <CustomReader>
                    <SimpleType>11-59</SimpleType>
                    <SimpleNullableType>10-49</SimpleNullableType>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<DateTime>(y => DateTime.ParseExact(y.Node.Value, "hh-mm", CultureInfo.InvariantCulture), true)).DeserializeXml<CustomReader>(xml);

            result.SimpleType.ShouldEqual(DateTime.ParseExact("11-59", "hh-mm", CultureInfo.InvariantCulture));
            result.SimpleNullableType.ShouldEqual(DateTime.ParseExact("10-49", "hh-mm", CultureInfo.InvariantCulture));
        }

        [Test]
        public void should_return_parse_error_when_reader_fails()
        {
            var deserializer = Bender.Deserializer.Create(
                x => x.AddReader<DateTime>(y => DateTime.ParseExact(y.Node.Value, "h:m:s", CultureInfo.InvariantCulture), true));

            Assert.Throws<ValueParseException>(() => deserializer.DeserializeXml<CustomReader>("<CustomReader><SimpleType>11-59</SimpleType></CustomReader>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value '11-59' in the '/CustomReader/SimpleType' element as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
            Assert.Throws<ValueParseException>(() => deserializer.DeserializeXml<CustomReader>("<CustomReader><SimpleNullableType>10-49</SimpleNullableType></CustomReader>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value '10-49' in the '/CustomReader/SimpleNullableType' element as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
        }

        [Test]
        public void should_return_parse_error_with_custom_message_when_reader_fails()
        {
            var deserializer = Bender.Deserializer.Create(x => x
                    .WithFriendlyParseErrorMessage<DateTime>("Frog is wrong.")
                    .AddReader<DateTime>(y => DateTime.ParseExact(y.Node.Value, "h:m:s", CultureInfo.InvariantCulture), true));

            Assert.Throws<ValueParseException>(() => deserializer.DeserializeXml<CustomReader>("<CustomReader><SimpleType>11-59</SimpleType></CustomReader>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value '11-59' in the '/CustomReader/SimpleType' element as a datetime: Frog is wrong.");
            Assert.Throws<ValueParseException>(() => deserializer.DeserializeXml<CustomReader>("<CustomReader><SimpleNullableType>10-49</SimpleNullableType></CustomReader>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value '10-49' in the '/CustomReader/SimpleNullableType' element as a datetime: Frog is wrong.");
        }

        [Test]
        public void should_deserialize_complex_type_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ComplexType>5</ComplexType>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>(y => new ComplexType { Value = int.Parse(y.Node.Value)})).DeserializeXml<CustomReader>(xml);

            result.ComplexType.Value.ShouldEqual(5);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfComplexTypes>1,2,3</ListOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<List<ComplexType>>(y => y.Node.Value.Split(',').Select(z => new ComplexType { Value = int.Parse(z)}).ToList())).DeserializeXml<CustomReader>(xml);
            
            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfSimpleTypes>1,2,3</ListOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<List<int>>(y => y.Node.Value.Split(',').Select(int.Parse).ToList())).DeserializeXml<CustomReader>(xml);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfComplexTypes>
                        <ComplexType>[1]</ComplexType>
                        <ComplexType>[2]</ComplexType>
                        <ComplexType>[3]</ComplexType>
                    </ListOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>(y => new ComplexType { Value = int.Parse(y.Node.Value.Replace("[", "").Replace("]", "")) })).DeserializeXml<CustomReader>(xml);

            result.ListOfComplexTypes.Count.ShouldEqual(3);
            result.ListOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListOfSimpleTypes>
                        <Int32>[1]</Int32>
                        <Int32>[2]</Int32>
                        <Int32>[3]</Int32>
                    </ListOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<int>(y => int.Parse(y.Node.Value.Replace("[", "").Replace("]", "")))).DeserializeXml<CustomReader>(xml);

            result.ListOfSimpleTypes.Count.ShouldEqual(3);
            result.ListOfSimpleTypes[0].ShouldEqual(1);
            result.ListOfSimpleTypes[1].ShouldEqual(2);
            result.ListOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfComplexTypes>1,2,3</ListInterfaceOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<IList<ComplexType>>(y => y.Node.Value.Split(',').Select(z => new ComplexType { Value = int.Parse(z) }).ToList())).DeserializeXml<CustomReader>(xml);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfSimpleTypes>1,2,3</ListInterfaceOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<IList<int>>(y => y.Node.Value.Split(',').Select(int.Parse).ToList())).DeserializeXml<CustomReader>(xml);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_complex_type_list_interface_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfComplexTypes>
                        <ComplexType>[1]</ComplexType>
                        <ComplexType>[2]</ComplexType>
                        <ComplexType>[3]</ComplexType>
                    </ListInterfaceOfComplexTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<ComplexType>(y => new ComplexType { Value = int.Parse(y.Node.Value.Replace("[", "").Replace("]", "")) })).DeserializeXml<CustomReader>(xml);

            result.ListInterfaceOfComplexTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfComplexTypes[0].Value.ShouldEqual(1);
            result.ListInterfaceOfComplexTypes[1].Value.ShouldEqual(2);
            result.ListInterfaceOfComplexTypes[2].Value.ShouldEqual(3);
        }

        [Test]
        public void should_deserialize_simple_type_list_interface_property_item_with_custom_reader()
        {
            const string xml = @"
                <CustomReader>
                    <ListInterfaceOfSimpleTypes>
                        <Int32>[1]</Int32>
                        <Int32>[2]</Int32>
                        <Int32>[3]</Int32>
                    </ListInterfaceOfSimpleTypes>
                </CustomReader>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader<int>(y => int.Parse(y.Node.Value.Replace("[", "").Replace("]", "")))).DeserializeXml<CustomReader>(xml);

            result.ListInterfaceOfSimpleTypes.Count.ShouldEqual(3);
            result.ListInterfaceOfSimpleTypes[0].ShouldEqual(1);
            result.ListInterfaceOfSimpleTypes[1].ShouldEqual(2);
            result.ListInterfaceOfSimpleTypes[2].ShouldEqual(3);
        }

        // Complex type attributes

        public class AttributeComplexType
        {
            public static AttributeComplexType Parse(string value, bool fail)
            {
                if (fail) throw new InvalidOperationException();
                return new AttributeComplexType { Value = value };
            }
            public string Value { get; set; }
        }

        public class ComplexAttribute
        {
            public AttributeComplexType Complex { get; set; }
        }

        [Test]
        public void should_set_complex_attribute_type_with_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            var result = Bender.Deserializer.Create(
                x => x.AddReader(y => AttributeComplexType.Parse(y.Node.Value, false))).DeserializeXml<ComplexAttribute>(xml);
            result.Complex.Value.ShouldEqual("hai");
        }

        [Test]
        public void should_throw_deserialize_exception_when_failing_to_set_complex_attribute_type_with_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(
                x => x.AddReader(y => AttributeComplexType.Parse(y.Node.Value, true))).DeserializeXml<ComplexAttribute>(xml));
        }

        [Test]
        public void should_throw_deserialize_exception_when_deserializing_complex_attribute_type_without_a_reader()
        {
            const string xml = @"<ComplexAttribute Complex=""hai""/>";
            Assert.Throws<DeserializeException>(() => Bender.Deserializer.Create().DeserializeXml<ComplexAttribute>(xml).Complex.ShouldBeNull());
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
        public void should_deserialize_byte_array()
        {
            const string xml = @"<BuiltInReaders><ByteArray>AQID</ByteArray></BuiltInReaders>";
            Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml).ByteArray.ShouldEqual(new byte[] { 1, 2, 3 });
        }

        [Test]
        public void should_not_deserialize_improperly_formated_byte_array()
        {
            const string xml = @"<BuiltInReaders><ByteArray>56y45u456u</ByteArray></BuiltInReaders>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml)).FriendlyMessage
                .ShouldEqual("Unable to parse the value '56y45u456u' in the '/BuiltInReaders/ByteArray' element as a Byte[]: Not formatted correctly, must be formatted as base64 string.");
        }

        [Test]
        public void should_deserialize_uri()
        {
            const string xml = @"<BuiltInReaders><Uri>http://www.google.com/</Uri></BuiltInReaders>";
            Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml).Uri.ShouldEqual(new Uri("http://www.google.com"));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_uri()
        {
            const string xml = @"<BuiltInReaders><Uri>46b464646436</Uri></BuiltInReaders>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml)).FriendlyMessage
                .ShouldEqual("Unable to parse the value '46b464646436' in the '/BuiltInReaders/Uri' element as a Uri: Not formatted correctly, must be formatted as 'http://domain.com'.");
        }

        [Test]
        public void should_deserialize_mail_address()
        {
            const string xml = @"<BuiltInReaders><MailAddress>""Test"" &lt;test@test.com&gt;</MailAddress></BuiltInReaders>";
            Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml).MailAddress.ShouldEqual(new MailAddress("test@test.com", "Test"));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_mail_address()
        {
            const string xml = @"<BuiltInReaders><MailAddress>34b64634b643b6</MailAddress></BuiltInReaders>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml)).FriendlyMessage
                .ShouldEqual("Unable to parse the value '34b64634b643b6' in the '/BuiltInReaders/MailAddress' element as a MailAddress: Not formatted correctly, must be formatted as 'username@domain.com'.");
        }

        [Test]
        public void should_deserialize_version()
        {
            const string xml = @"<BuiltInReaders><Version>1.2.3.4</Version></BuiltInReaders>";
            Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml).Version.ShouldEqual(new Version(1, 2, 3, 4));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_version()
        {
            const string xml = @"<BuiltInReaders><Version>4b6345b6345b634</Version></BuiltInReaders>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml)).FriendlyMessage
                .ShouldEqual("Unable to parse the value '4b6345b6345b634' in the '/BuiltInReaders/Version' element as a Version: Not formatted correctly, must be formatted as '1.2.3.4'.");
        }

        [Test]
        public void should_deserialize_ip_address()
        {
            const string xml = @"<BuiltInReaders><IpAddress>192.168.1.1</IpAddress></BuiltInReaders>";
            Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml).IpAddress.ShouldEqual(IPAddress.Parse("192.168.1.1"));
        }

        [Test]
        public void should_not_deserialize_improperly_formated_ip_address()
        {
            const string xml = @"<BuiltInReaders><IpAddress>45b634b6345b6345</IpAddress></BuiltInReaders>";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeXml<BuiltInReaders>(xml)).FriendlyMessage
                .ShouldEqual("Unable to parse the value '45b634b6345b6345' in the '/BuiltInReaders/IpAddress' element as a IPAddress: Not formatted correctly, must be formatted as '1.2.3.4'.");
        }
    }
}
