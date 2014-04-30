using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class LoadTests
    {
        public static Action<OptionsDsl> Configure = x => x.Deserialization(y => y.IgnoreNameCase());
        public static Options Options = Options.Create(x => x.Deserialization(y => y.IgnoreNameCase()));

        public class Model
        {
            public string Oh { get; set; }
        }

        // Errors

        [Test]
        public void should_fail_when_xml_is_not_valid()
        {
            var message = Assert.Throws<ParseException>(() => Deserialize.
                Xml<Model>("<yada><sdfsdf></yada>")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse xml: 'sdfsdf' is expected  Line 1, position 18.");
            #else 
            message.ShouldEqual("Unable to parse xml: The 'sdfsdf' start tag on line 1 position 8 does not match the end tag of 'yada'. Line 1, position 17.");
            #endif
        }

        [Test]
        public void should_fail_when_there_are_multiple_root_elements()
        {
            var message = Assert.Throws<ParseException>(() => Deserialize.
                Xml<Model>("<oh></oh><hai></hai>")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse xml: Multiple document element was detected.  Line 1, position 11.");
            #else 
            message.ShouldEqual("Unable to parse xml: There are multiple root elements. Line 1, position 11.");
            #endif
        }

        // String 

        public const string Xml = "<Model><Oh>Hai</Oh></Model>";
        public const string LowerCaseXml = "<model><oh>Hai</oh></model>";

        private static readonly XDocument XmlDocument = XDocument.Parse(Xml);
        private static readonly XDocument LowerCaseXmlDocument = XDocument.Parse(LowerCaseXml);

        private static readonly XElement XmlElement = XmlDocument.Root;
        private static readonly XElement LowerCaseXmlElement = LowerCaseXmlDocument.Root;
        
        private readonly static byte[] XmlBytesUtf8 = Encoding.UTF8.GetBytes(Xml);
        private readonly static byte[] XmlBytesUnicode = Encoding.BigEndianUnicode.GetBytes(Xml);
        private readonly static byte[] LowerCaseXmlBytesUtf8 = Encoding.UTF8.GetBytes(LowerCaseXml);
        private readonly static byte[] LowerCaseXmlBytesUnicode = Encoding.BigEndianUnicode.GetBytes(LowerCaseXml);

        private readonly static Stream XmlStreamUtf8 = new MemoryStream(XmlBytesUtf8);
        private readonly static Stream XmlStreamUnicode = new MemoryStream(XmlBytesUnicode);
        private readonly static Stream LowerCaseXmlStreamUtf8 = new MemoryStream(LowerCaseXmlBytesUtf8);
        private readonly static Stream LowerCaseXmlStreamUnicode = new MemoryStream(LowerCaseXmlBytesUnicode);

        private readonly static string XmlPathUtf8 = Path.GetTempFileName();
        private readonly static string XmlPathUnicode = Path.GetTempFileName();
        private readonly static string LowerCaseXmlPathUtf8 = Path.GetTempFileName();
        private readonly static string LowerCaseXmlPathUnicode = Path.GetTempFileName();

        [SetUp]
        public void Setup()
        {
            XmlStreamUtf8.Reset();
            XmlStreamUnicode.Reset();
            LowerCaseXmlStreamUtf8.Reset();
            LowerCaseXmlStreamUnicode.Reset();

            File.WriteAllBytes(XmlPathUtf8, XmlBytesUtf8);
            File.WriteAllBytes(XmlPathUnicode, XmlBytesUnicode);
            File.WriteAllBytes(LowerCaseXmlPathUtf8, LowerCaseXmlBytesUtf8);
            File.WriteAllBytes(LowerCaseXmlPathUnicode, LowerCaseXmlBytesUnicode);
        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(XmlPathUtf8);
            File.Delete(XmlPathUnicode);
            File.Delete(LowerCaseXmlPathUtf8);
            File.Delete(LowerCaseXmlPathUnicode);
        }

        private static readonly object[] ToObjectCases = TestCases.Create()

            // String

            .AddFunc(() => Deserialize.Xml(Xml, typeof(Model)))
            .AddFunc(() => Deserialize.Xml(LowerCaseXml, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXml, typeof(Model), Options))
            .AddFunc(() => Deserialize.Xml<Model>(Xml))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXml, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXml, Options))

            // Document

            .AddFunc(() => Deserialize.Xml(XmlDocument, typeof(Model)))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlDocument, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlDocument, typeof(Model), Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlDocument))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlDocument, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlDocument, Options))

            // Element

            .AddFunc(() => Deserialize.Xml(XmlElement, typeof(Model)))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlElement, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlElement, typeof(Model), Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlElement))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlElement, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlElement, Options))

            // Bytes

            .AddFunc(() => Deserialize.Xml(XmlBytesUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlBytesUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlBytesUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.Xml(XmlBytesUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlBytesUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlBytesUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlBytesUtf8))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlBytesUtf8, Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlBytesUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Xml(XmlStreamUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlStreamUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlStreamUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.Xml(XmlStreamUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlStreamUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(LowerCaseXmlStreamUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlStreamUtf8))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlStreamUtf8, Options))
            .AddFunc(() => Deserialize.Xml<Model>(XmlStreamUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml<Model>(LowerCaseXmlStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.XmlFile(XmlPathUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.XmlFile(LowerCaseXmlPathUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.XmlFile(LowerCaseXmlPathUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.XmlFile(LowerCaseXmlPathUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.XmlFile(LowerCaseXmlPathUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.XmlFile<Model>(XmlPathUtf8))
            .AddFunc(() => Deserialize.XmlFile<Model>(LowerCaseXmlPathUtf8, Configure))
            .AddFunc(() => Deserialize.XmlFile<Model>(LowerCaseXmlPathUtf8, Options))
            .AddFunc(() => Deserialize.XmlFile<Model>(XmlPathUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.XmlFile<Model>(LowerCaseXmlPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.XmlFile<Model>(LowerCaseXmlPathUnicode, Encoding.BigEndianUnicode, Options))

            .All;

        [Test]
        [TestCaseSource("ToObjectCases")]
        public void should_deserialize_to_object(Func<object> deserialize)
        {
            deserialize().As<Model>().Oh.ShouldEqual("Hai");
        }

        private static readonly object[] ToNodeCases = TestCases.Create()

            // String 

            .AddFunc(() => Deserialize.Xml(Xml))
            .AddFunc(() => Deserialize.Xml(Xml, Configure))
            .AddFunc(() => Deserialize.Xml(Xml, Options))

            // Document 

            .AddFunc(() => Deserialize.Xml(XmlDocument))
            .AddFunc(() => Deserialize.Xml(XmlDocument, Configure))
            .AddFunc(() => Deserialize.Xml(XmlDocument, Options))

            // Element 

            .AddFunc(() => Deserialize.Xml(XmlElement))
            .AddFunc(() => Deserialize.Xml(XmlElement, Configure))
            .AddFunc(() => Deserialize.Xml(XmlElement, Options))
            
            // Bytes

            .AddFunc(() => Deserialize.Xml(XmlBytesUtf8))
            .AddFunc(() => Deserialize.Xml(XmlBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(XmlBytesUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Xml(XmlBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Xml(XmlBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(XmlBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Xml(XmlStreamUtf8))
            .AddFunc(() => Deserialize.Xml(XmlStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(XmlStreamUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Xml(XmlStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Xml(XmlStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Xml(XmlStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.XmlFile(XmlPathUtf8))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUtf8, Configure))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.XmlFile(XmlPathUnicode, Encoding.BigEndianUnicode, Options))
            
            .All;

        [Test]
        [TestCaseSource("ToNodeCases")]
        public void should_deserialize_to_node(Func<INode> deserialize)
        {
            deserialize().GetNode("Oh").Value.ShouldEqual("Hai");;
        }
    }
}
