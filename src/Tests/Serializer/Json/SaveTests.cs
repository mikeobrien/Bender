using System;
using System.IO;
using System.Text;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
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

        public const string Json = "{\"Oh\":\"Hai\",\"Hai\":\"Oh\"}";
        public const string LowerCaseJson = "{\"oh\":\"Hai\",\"hai\":\"Oh\"}";

        public const string JsonByType = "{\"Oh\":\"Hai\"}";
        public const string LowerCaseJsonByType = "{\"oh\":\"Hai\"}";

        private static readonly object[] StringCases = TestCases.Create()

            .AddFunc(() => Serialize.Json(ModelInstance), Json)
            .AddFunc(() => Serialize.Json(ModelInstance, Configure), LowerCaseJson)
            .AddFunc(() => Serialize.Json(ModelInstance, Options), LowerCaseJson)

            .AddFunc(() => Serialize.Json<IModel>(ModelInstance), JsonByType)
            .AddFunc(() => Serialize.Json<IModel>(ModelInstance, Configure), LowerCaseJsonByType)
            .AddFunc(() => Serialize.Json<IModel>(ModelInstance, Options), LowerCaseJsonByType)

            .AddFunc(() => Serialize.Json(ModelInstance, typeof(IModel)), JsonByType)
            .AddFunc(() => Serialize.Json(ModelInstance, typeof(IModel), Configure), LowerCaseJsonByType)
            .AddFunc(() => Serialize.Json(ModelInstance, typeof(IModel), Options), LowerCaseJsonByType)

            .All;

        [Test]
        [TestCaseSource("StringCases")]
        public void should_serialize_string(Func<string> serialize, string output)
        {
            serialize().ShouldEqual(output);
        }

        // Bytes

        private readonly static byte[] JsonBytesUtf8 = Encoding.UTF8.GetBytes(Json);
        private readonly static byte[] JsonBytesUnicode = Encoding.BigEndianUnicode.GetBytes(Json);
        private readonly static byte[] LowerCaseJsonBytesUtf8 = Encoding.UTF8.GetBytes(LowerCaseJson);
        private readonly static byte[] LowerCaseJsonBytesUnicode = Encoding.BigEndianUnicode.GetBytes(LowerCaseJson);

        private readonly static byte[] JsonBytesUtf8ByType = Encoding.UTF8.GetBytes(JsonByType);
        private readonly static byte[] JsonBytesUnicodeByType = Encoding.BigEndianUnicode.GetBytes(JsonByType);
        private readonly static byte[] LowerCaseJsonBytesUtf8ByType = Encoding.UTF8.GetBytes(LowerCaseJsonByType);
        private readonly static byte[] LowerCaseJsonBytesUnicodeByType = Encoding.BigEndianUnicode.GetBytes(LowerCaseJsonByType);

        private static readonly object[] ByteCases = TestCases.Create()

            .AddFunc(() => Serialize.JsonBytes(ModelInstance), JsonBytesUtf8)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, Configure), LowerCaseJsonBytesUtf8)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, Options), LowerCaseJsonBytesUtf8)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, Encoding.BigEndianUnicode), JsonBytesUnicode)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseJsonBytesUnicode)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseJsonBytesUnicode)

            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance), JsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance, Configure), LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance, Options), LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode), JsonBytesUnicodeByType)
            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseJsonBytesUnicodeByType)
            .AddFunc(() => Serialize.JsonBytes<IModel>(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseJsonBytesUnicodeByType)

            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel)), JsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel), Configure), LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel), Options), LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode), JsonBytesUnicodeByType)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Configure), LowerCaseJsonBytesUnicodeByType)
            .AddFunc(() => Serialize.JsonBytes(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Options), LowerCaseJsonBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource("ByteCases")]
        public void should_serialize_bytes(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }

        // Stream

        private readonly static Stream JsonStreamUtf8 = new MemoryStream(JsonBytesUtf8);
        private readonly static Stream JsonStreamUnicode = new MemoryStream(JsonBytesUnicode);
        private readonly static Stream LowerCaseJsonStreamUtf8 = new MemoryStream(LowerCaseJsonBytesUtf8);
        private readonly static Stream LowerCaseJsonStreamUnicode = new MemoryStream(LowerCaseJsonBytesUnicode);

        private readonly static Stream JsonStreamUtf8ByType = new MemoryStream(JsonBytesUtf8ByType);
        private readonly static Stream JsonStreamUnicodeByType = new MemoryStream(JsonBytesUnicodeByType);
        private readonly static Stream LowerCaseJsonStreamUtf8ByType = new MemoryStream(LowerCaseJsonBytesUtf8ByType);
        private readonly static Stream LowerCaseJsonStreamUnicodeByType = new MemoryStream(LowerCaseJsonBytesUnicodeByType);

        [SetUp]
        public void CreateStreams()
        {
            JsonStreamUtf8.Reset();
            JsonStreamUnicode.Reset();
            LowerCaseJsonStreamUtf8.Reset();
            LowerCaseJsonStreamUnicode.Reset();

            JsonStreamUtf8ByType.Reset();
            JsonStreamUnicodeByType.Reset();
            LowerCaseJsonStreamUtf8ByType.Reset();
            LowerCaseJsonStreamUnicodeByType.Reset();
        }

        private static readonly object[] AsStreamCases = TestCases.Create()

            .AddFunc(() => Serialize.JsonStream(ModelInstance), JsonStreamUtf8)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, Configure), LowerCaseJsonStreamUtf8)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, Options), LowerCaseJsonStreamUtf8)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, Encoding.BigEndianUnicode), JsonStreamUnicode)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicode)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicode)

            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance), JsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance, Configure), LowerCaseJsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance, Options), LowerCaseJsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance, Encoding.BigEndianUnicode), JsonStreamUnicodeByType)
            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance, Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicodeByType)
            .AddFunc(() => Serialize.JsonStream<IModel>(ModelInstance, Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicodeByType)

            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel)), JsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel), Configure), LowerCaseJsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel), Options), LowerCaseJsonStreamUtf8ByType)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode), JsonStreamUnicodeByType)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicodeByType)
            .AddFunc(() => Serialize.JsonStream(ModelInstance, typeof(IModel), Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicodeByType)

            .All;

        [Test]
        [TestCaseSource("AsStreamCases")]
        public void should_serialize_as_stream(Func<Stream> serialize, Stream output)
        {
            serialize().ShouldEqual(output);
        }

        private static readonly object[] ToStreamCases = TestCases.Create()

            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x), JsonStreamUtf8)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x, Configure), LowerCaseJsonStreamUtf8)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x, Options), LowerCaseJsonStreamUtf8)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x, Encoding.BigEndianUnicode), JsonStreamUnicode)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x, Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicode)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, x, Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicode)

            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x), JsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x, Configure), LowerCaseJsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x, Options), LowerCaseJsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode), JsonStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.JsonStream<IModel>(ModelInstance, x, Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicodeByType)

            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x), JsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x, Configure), LowerCaseJsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x, Options), LowerCaseJsonStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode), JsonStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode, Configure), LowerCaseJsonStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.JsonStream(ModelInstance, typeof(IModel), x, Encoding.BigEndianUnicode, Options), LowerCaseJsonStreamUnicodeByType)

            .All;

        [Test]
        [TestCaseSource("ToStreamCases")]
        public void should_serialize_to_stream(Action<Stream> serialize, Stream output)
        {
            var stream = new MemoryStream();
            serialize(stream);
            stream.Seek(0, SeekOrigin.Begin);
            stream.ShouldEqual(output);
        }

        // Files

        private readonly static string JsonPathUtf8 = Path.GetTempFileName();
        private readonly static string JsonPathUnicode = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUtf8 = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUnicode = Path.GetTempFileName();

        private readonly static string JsonPathUtf8ByType = Path.GetTempFileName();
        private readonly static string JsonPathUnicodeByType = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUtf8ByType = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUnicodeByType = Path.GetTempFileName();

        [SetUp]
        public void CreateFiles()
        {
            File.WriteAllBytes(JsonPathUtf8, JsonBytesUtf8);
            File.WriteAllBytes(JsonPathUnicode, JsonBytesUnicode);
            File.WriteAllBytes(LowerCaseJsonPathUtf8, LowerCaseJsonBytesUtf8);
            File.WriteAllBytes(LowerCaseJsonPathUnicode, LowerCaseJsonBytesUnicode);

            File.WriteAllBytes(JsonPathUtf8ByType, JsonBytesUtf8ByType);
            File.WriteAllBytes(JsonPathUnicodeByType, JsonBytesUnicodeByType);
            File.WriteAllBytes(LowerCaseJsonPathUtf8ByType, LowerCaseJsonBytesUtf8ByType);
            File.WriteAllBytes(LowerCaseJsonPathUnicodeByType, LowerCaseJsonBytesUnicodeByType);
        }

        [TearDown]
        public void DeleteFiles()
        {
            File.Delete(JsonPathUtf8);
            File.Delete(JsonPathUnicode);
            File.Delete(LowerCaseJsonPathUtf8);
            File.Delete(LowerCaseJsonPathUnicode);

            File.Delete(JsonPathUtf8ByType);
            File.Delete(JsonPathUnicodeByType);
            File.Delete(LowerCaseJsonPathUtf8ByType);
            File.Delete(LowerCaseJsonPathUnicodeByType);
        }

        private static readonly object[] FileCases = TestCases.Create()

            .AddFunc(() => { Serialize.JsonFile(ModelInstance, JsonPathUtf8); 
                return File.ReadAllBytes(JsonPathUtf8); }, JsonBytesUtf8)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, LowerCaseJsonPathUtf8, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8); }, LowerCaseJsonBytesUtf8)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, LowerCaseJsonPathUtf8, Options); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8); }, LowerCaseJsonBytesUtf8)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, JsonPathUnicode, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(JsonPathUnicode); }, JsonBytesUnicode)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, LowerCaseJsonPathUnicode, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUnicode); }, LowerCaseJsonBytesUnicode)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, LowerCaseJsonPathUnicode, Encoding.BigEndianUnicode, Options); 
                return File.ReadAllBytes(LowerCaseJsonPathUnicode); }, LowerCaseJsonBytesUnicode)

            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, JsonPathUtf8ByType); 
                return File.ReadAllBytes(JsonPathUtf8ByType); }, JsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, LowerCaseJsonPathUtf8ByType, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8ByType); }, LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, LowerCaseJsonPathUtf8ByType, Options); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8ByType); }, LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, JsonPathUnicodeByType, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(JsonPathUnicodeByType); }, JsonBytesUnicodeByType)
            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, LowerCaseJsonPathUnicodeByType, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUnicodeByType); }, LowerCaseJsonBytesUnicodeByType)
            .AddFunc(() => { Serialize.JsonFile<IModel>(ModelInstance, LowerCaseJsonPathUnicodeByType, Encoding.BigEndianUnicode, Options);
 
                return File.ReadAllBytes(LowerCaseJsonPathUnicodeByType); }, LowerCaseJsonBytesUnicodeByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), JsonPathUtf8ByType); 
                return File.ReadAllBytes(JsonPathUtf8ByType); }, JsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), LowerCaseJsonPathUtf8ByType, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8ByType); }, LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), LowerCaseJsonPathUtf8ByType, Options); 
                return File.ReadAllBytes(LowerCaseJsonPathUtf8ByType); }, LowerCaseJsonBytesUtf8ByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), JsonPathUnicodeByType, Encoding.BigEndianUnicode); 
                return File.ReadAllBytes(JsonPathUnicodeByType); }, JsonBytesUnicodeByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), LowerCaseJsonPathUnicodeByType, Encoding.BigEndianUnicode, Configure); 
                return File.ReadAllBytes(LowerCaseJsonPathUnicodeByType); }, LowerCaseJsonBytesUnicodeByType)
            .AddFunc(() => { Serialize.JsonFile(ModelInstance, typeof(IModel), LowerCaseJsonPathUnicodeByType, Encoding.BigEndianUnicode, Options); 
                return File.ReadAllBytes(LowerCaseJsonPathUnicodeByType); }, LowerCaseJsonBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource("FileCases")]
        public void should_serialize_file(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }
    }
}
