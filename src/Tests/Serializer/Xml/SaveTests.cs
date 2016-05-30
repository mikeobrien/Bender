using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class SaveTests
    {
        public static Action<OptionsDsl> Configure = x => x.UseCamelCaseNaming();
        public static Options Options = Options.Create(x => x.UseCamelCaseNaming());

        public interface IModel
        {
            string Oh { get; set; }
        }

        public class Model : IModel
        {
            public string Oh { get; set; }
            public string Hai { get; set; }
        }

        public static Model ModelInstance = new Model { Oh = "Hai", Hai = "Oh" };

        // String

        public const string XmlUtf8Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        public const string XmlUnicodeDeclaration = "<?xml version=\"1.0\" encoding=\"utf-16BE\"?>";

        public const string Xml = "<Model><Oh>Hai</Oh><Hai>Oh</Hai></Model>";
        public const string LowerCaseXml = "<model><oh>Hai</oh><hai>Oh</hai></model>";

        public const string XmlByType = "<IModel><Oh>Hai</Oh></IModel>";
        public const string LowerCaseXmlByType = "<iModel><oh>Hai</oh></iModel>";

        private static readonly object[] StringCases = TestCases.Create()

            .AddFunc(() => Serialize.Xml(ModelInstance), Xml)
            .AddFunc(() => Serialize.Xml(ModelInstance, Configure), LowerCaseXml)
            .AddFunc(() => Serialize.Xml(ModelInstance, Options), LowerCaseXml)

            .AddFunc(() => Serialize.Xml<IModel>(ModelInstance), XmlByType)
            .AddFunc(() => Serialize.Xml<IModel>(ModelInstance, Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.Xml<IModel>(ModelInstance, Options), LowerCaseXmlByType)

            .AddFunc(() => Serialize.Xml(ModelInstance, typeof(IModel)), XmlByType)
            .AddFunc(() => Serialize.Xml(ModelInstance, typeof(IModel), Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.Xml(ModelInstance, typeof(IModel), Options), LowerCaseXmlByType)

            .All;

        [Test]
        [TestCaseSource(nameof(StringCases))]
        public void should_serialize_string(Func<string> serialize, string output)
        {
            serialize().ShouldEqual(XmlUtf8Declaration + output);
        }

        // XDocument

        private static readonly object[] DocumentCases = TestCases.Create()

            .AddFunc(() => Serialize.XmlDocument(ModelInstance), Xml)
            .AddFunc(() => Serialize.XmlDocument(ModelInstance, Configure), LowerCaseXml)
            .AddFunc(() => Serialize.XmlDocument(ModelInstance, Options), LowerCaseXml)

            .AddFunc(() => Serialize.XmlDocument<IModel>(ModelInstance), XmlByType)
            .AddFunc(() => Serialize.XmlDocument<IModel>(ModelInstance, Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.XmlDocument<IModel>(ModelInstance, Options), LowerCaseXmlByType)

            .AddFunc(() => Serialize.XmlDocument(ModelInstance, typeof(IModel)), XmlByType)
            .AddFunc(() => Serialize.XmlDocument(ModelInstance, typeof(IModel), Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.XmlDocument(ModelInstance, typeof(IModel), Options), LowerCaseXmlByType)

            .All;

        [Test]
        [TestCaseSource(nameof(DocumentCases))]
        public void should_serialize_xdocument(Func<XDocument> serialize, string output)
        {
            serialize().ToString().Replace("\r\n", "").Replace(" ", "").ShouldEqual(output);
        }

        // XElement

        private static readonly object[] ElementCases = TestCases.Create()

            .AddFunc(() => Serialize.XmlElement(ModelInstance), Xml)
            .AddFunc(() => Serialize.XmlElement(ModelInstance, Configure), LowerCaseXml)
            .AddFunc(() => Serialize.XmlElement(ModelInstance, Options), LowerCaseXml)

            .AddFunc(() => Serialize.XmlElement<IModel>(ModelInstance), XmlByType)
            .AddFunc(() => Serialize.XmlElement<IModel>(ModelInstance, Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.XmlElement<IModel>(ModelInstance, Options), LowerCaseXmlByType)

            .AddFunc(() => Serialize.XmlElement(ModelInstance, typeof(IModel)), XmlByType)
            .AddFunc(() => Serialize.XmlElement(ModelInstance, typeof(IModel), Configure), LowerCaseXmlByType)
            .AddFunc(() => Serialize.XmlElement(ModelInstance, typeof(IModel), Options), LowerCaseXmlByType)

            .All;

        [Test]
        [TestCaseSource(nameof(ElementCases))]
        public void should_serialize_xelement(Func<XElement> serialize, string output)
        {
            serialize().ToString().Replace("\r\n", "").Replace(" ", "").ShouldEqual(output);
        }

        // Bytes
        
        private static readonly byte[] UnicodeBom = { 254, 255 };

        private static readonly byte[] XmlBytesUtf8 = Encoding.UTF8.GetBytes(XmlUtf8Declaration + Xml);
        private static readonly byte[] XmlBytesUnicode = UnicodeBom.Concat(Encoding.BigEndianUnicode.GetBytes(XmlUnicodeDeclaration + Xml)).ToArray();
        private static readonly byte[] LowerCaseXmlBytesUtf8 = Encoding.UTF8.GetBytes(XmlUtf8Declaration + LowerCaseXml);
        private static readonly byte[] LowerCaseXmlBytesUnicode = UnicodeBom.Concat(Encoding.BigEndianUnicode.GetBytes(XmlUnicodeDeclaration + LowerCaseXml)).ToArray();

        private static readonly byte[] XmlBytesUtf8ByType = Encoding.UTF8.GetBytes(XmlUtf8Declaration + XmlByType);
        private static readonly byte[] XmlBytesUnicodeByType = UnicodeBom.Concat(Encoding.BigEndianUnicode.GetBytes(XmlUnicodeDeclaration + XmlByType)).ToArray();
        private static readonly byte[] LowerCaseXmlBytesUtf8ByType = Encoding.UTF8.GetBytes(XmlUtf8Declaration + LowerCaseXmlByType);
        private static readonly byte[] LowerCaseXmlBytesUnicodeByType = UnicodeBom.Concat(Encoding.BigEndianUnicode.GetBytes(XmlUnicodeDeclaration + LowerCaseXmlByType)).ToArray();

        private static readonly object[] ByteCases = TestCases.Create()

            .AddFunc(() => Serialize.XmlBytes(ModelInstance), XmlBytesUtf8)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, Configure), LowerCaseXmlBytesUtf8)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, Options), LowerCaseXmlBytesUtf8)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, Encoding.BigEndianUnicode), XmlBytesUnicode)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseXmlBytesUnicode)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseXmlBytesUnicode)

            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance), XmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance, Configure), LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance, Options), LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode), XmlBytesUnicodeByType)
            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseXmlBytesUnicodeByType)
            .AddFunc(() => Serialize.XmlBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseXmlBytesUnicodeByType)

            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel)), XmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel), Configure), LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel), Options), LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode), XmlBytesUnicodeByType)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Configure), LowerCaseXmlBytesUnicodeByType)
            .AddFunc(() => Serialize.XmlBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Options), LowerCaseXmlBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(ByteCases))]
        public void should_serialize_bytes(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }

        // Stream

        private static readonly Stream XmlStreamUtf8 = new MemoryStream(XmlBytesUtf8);
        private static readonly Stream XmlStreamUnicode = new MemoryStream(XmlBytesUnicode);
        private static readonly Stream LowerCaseXmlStreamUtf8 = new MemoryStream(LowerCaseXmlBytesUtf8);
        private static readonly Stream LowerCaseXmlStreamUnicode = new MemoryStream(LowerCaseXmlBytesUnicode);

        private static readonly Stream XmlStreamUtf8ByType = new MemoryStream(XmlBytesUtf8ByType);
        private static readonly Stream XmlStreamUnicodeByType = new MemoryStream(XmlBytesUnicodeByType);
        private static readonly Stream LowerCaseXmlStreamUtf8ByType = new MemoryStream(LowerCaseXmlBytesUtf8ByType);
        private static readonly Stream LowerCaseXmlStreamUnicodeByType = new MemoryStream(LowerCaseXmlBytesUnicodeByType);

        [SetUp]
        public void CreateStreams()
        {
            XmlStreamUtf8.Reset();
            XmlStreamUnicode.Reset();
            LowerCaseXmlStreamUtf8.Reset();
            LowerCaseXmlStreamUnicode.Reset();

            XmlStreamUtf8ByType.Reset();
            XmlStreamUnicodeByType.Reset();
            LowerCaseXmlStreamUtf8ByType.Reset();
            LowerCaseXmlStreamUnicodeByType.Reset();
        }

        private static readonly object[] AsStreamCases = TestCases.Create()

            .AddFunc(() => Serialize.XmlStream(ModelInstance), XmlStreamUtf8)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, Configure), LowerCaseXmlStreamUtf8)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, Options), LowerCaseXmlStreamUtf8)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, Encoding.BigEndianUnicode), XmlStreamUnicode)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicode)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicode)

            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance), XmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance, Configure), LowerCaseXmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance, Options), LowerCaseXmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance, Encoding.BigEndianUnicode), XmlStreamUnicodeByType)
            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicodeByType)
            .AddFunc(() => Serialize.XmlStream<IModel>(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicodeByType)

            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel)), XmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel), Configure), LowerCaseXmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel), Options), LowerCaseXmlStreamUtf8ByType)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode), XmlStreamUnicodeByType)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicodeByType)
            .AddFunc(() => Serialize.XmlStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(AsStreamCases))]
        public void should_serialize_as_stream(Func<Stream> serialize, Stream output)
        {
            serialize().ShouldEqual(output);
        }

        private static readonly object[] ToStreamCases = TestCases.Create()

            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x), XmlStreamUtf8)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x, Configure), LowerCaseXmlStreamUtf8)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x, Options), LowerCaseXmlStreamUtf8)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x, Encoding.BigEndianUnicode), XmlStreamUnicode)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x, Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicode)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, x, Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicode)

            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x), XmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x, Configure), LowerCaseXmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x, Options), LowerCaseXmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode), XmlStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.XmlStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicodeByType)

            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x), XmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x, Configure), LowerCaseXmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x, Options), LowerCaseXmlStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode), XmlStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode, Configure), LowerCaseXmlStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.XmlStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode, Options), LowerCaseXmlStreamUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(ToStreamCases))]
        public void should_serialize_to_stream(Action<Stream> serialize, Stream output)
        {
            var stream = new MemoryStream();
            serialize(stream);
            stream.Seek(0, SeekOrigin.Begin);
            stream.ShouldEqual(output);
        }

        // Files

        private static readonly string XmlPathUtf8 = Path.GetTempFileName();
        private static readonly string XmlPathUnicode = Path.GetTempFileName();
        private static readonly string LowerCaseXmlPathUtf8 = Path.GetTempFileName();
        private static readonly string LowerCaseXmlPathUnicode = Path.GetTempFileName();

        private static readonly string XmlPathUtf8ByType = Path.GetTempFileName();
        private static readonly string XmlPathUnicodeByType = Path.GetTempFileName();
        private static readonly string LowerCaseXmlPathUtf8ByType = Path.GetTempFileName();
        private static readonly string LowerCaseXmlPathUnicodeByType = Path.GetTempFileName();

        [SetUp]
        public void CreateFiles()
        {
            File.WriteAllBytes(XmlPathUtf8, XmlBytesUtf8);
            File.WriteAllBytes(XmlPathUnicode, XmlBytesUnicode);
            File.WriteAllBytes(LowerCaseXmlPathUtf8, LowerCaseXmlBytesUtf8);
            File.WriteAllBytes(LowerCaseXmlPathUnicode, LowerCaseXmlBytesUnicode);

            File.WriteAllBytes(XmlPathUtf8ByType, XmlBytesUtf8ByType);
            File.WriteAllBytes(XmlPathUnicodeByType, XmlBytesUnicodeByType);
            File.WriteAllBytes(LowerCaseXmlPathUtf8ByType, LowerCaseXmlBytesUtf8ByType);
            File.WriteAllBytes(LowerCaseXmlPathUnicodeByType, LowerCaseXmlBytesUnicodeByType);
        }

        [TearDown]
        public void DeleteFiles()
        {
            File.Delete(XmlPathUtf8);
            File.Delete(XmlPathUnicode);
            File.Delete(LowerCaseXmlPathUtf8);
            File.Delete(LowerCaseXmlPathUnicode);

            File.Delete(XmlPathUtf8ByType);
            File.Delete(XmlPathUnicodeByType);
            File.Delete(LowerCaseXmlPathUtf8ByType);
            File.Delete(LowerCaseXmlPathUnicodeByType);
        }

        private static readonly object[] FileCases = TestCases.Create()

            .AddFunc(() => { Serialize.XmlFile(ModelInstance, XmlPathUtf8); 
                return File.ReadAllBytes(XmlPathUtf8); }, XmlBytesUtf8)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, LowerCaseXmlPathUtf8, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8); }, LowerCaseXmlBytesUtf8)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, LowerCaseXmlPathUtf8, Options); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8); }, LowerCaseXmlBytesUtf8)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, XmlPathUnicode, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(XmlPathUnicode); }, XmlBytesUnicode)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, LowerCaseXmlPathUnicode, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUnicode); }, LowerCaseXmlBytesUnicode)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, LowerCaseXmlPathUnicode, Encoding.BigEndianUnicode, Options); 
                return File.ReadAllBytes(LowerCaseXmlPathUnicode); }, LowerCaseXmlBytesUnicode)

            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, XmlPathUtf8ByType); 
                return File.ReadAllBytes(XmlPathUtf8ByType); }, XmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, LowerCaseXmlPathUtf8ByType, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8ByType); }, LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, LowerCaseXmlPathUtf8ByType, Options); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8ByType); }, LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, XmlPathUnicodeByType, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(XmlPathUnicodeByType); }, XmlBytesUnicodeByType)
            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, LowerCaseXmlPathUnicodeByType, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUnicodeByType); }, LowerCaseXmlBytesUnicodeByType)
            .AddFunc(() => { Serialize.XmlFile<IModel>(ModelInstance, LowerCaseXmlPathUnicodeByType, Encoding.BigEndianUnicode, Options);
 
                return File.ReadAllBytes(LowerCaseXmlPathUnicodeByType); }, LowerCaseXmlBytesUnicodeByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), XmlPathUtf8ByType); 
                return File.ReadAllBytes(XmlPathUtf8ByType); }, XmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), LowerCaseXmlPathUtf8ByType, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8ByType); }, LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), LowerCaseXmlPathUtf8ByType, Options); 
                return File.ReadAllBytes(LowerCaseXmlPathUtf8ByType); }, LowerCaseXmlBytesUtf8ByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), XmlPathUnicodeByType, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(XmlPathUnicodeByType); }, XmlBytesUnicodeByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), LowerCaseXmlPathUnicodeByType, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseXmlPathUnicodeByType); }, LowerCaseXmlBytesUnicodeByType)
            .AddFunc(() => { Serialize.XmlFile(ModelInstance, typeof(IModel), LowerCaseXmlPathUnicodeByType, Encoding.BigEndianUnicode, Options); 
                return File.ReadAllBytes(LowerCaseXmlPathUnicodeByType); }, LowerCaseXmlBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(FileCases))]
        public void should_serialize_file(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }
    }
}
