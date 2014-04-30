using System;
using System.IO;
using System.Text;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
        public void should_fail_when_there_are_extra_brackets()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeJson<Model>("{ \"sdfsdf\": 1 }}}")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse json: There are multiple root elements (0077) (1,13)");
            #else
            message.ShouldEqual("Unable to parse json: There are multiple root elements.");
            #endif
        }

        [Test]
        public void should_fail_when_value_is_missing()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeJson<Model>("{ \"sdfsdf\" }")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse json: ':' is expected after a name of an object content (1,12)");
            #else 
            message.ShouldEqual("Unable to parse json: The token ':' was expected but found '}'.");
            #endif
        }

        [Test]
        public void should_fail_when_element_is_not_closed()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeJson<Model>("{ \"sdfsdf\": { }")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse json: 1 missing end of arrays or objects (1,16)");
            #else 
            message.ShouldEqual("Unable to parse json: Unexpected end of file. Following elements are not closed: sdfsdf, root.");
            #endif
        }

        [Test]
        public void should_fail_when_value_not_quoted()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeJson<Model>("{ \"sdfsdf\": werwer }")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse json: Unexpected token: 'w' (0077) (1,13)");
            #else 
            message.ShouldEqual("Unable to parse json: Encountered unexpected character 'w'.");
            #endif
        }

        // String 

        public const string Json = "{ \"Oh\": \"Hai\" }";
        public const string LowerCaseJson = "{ \"oh\": \"Hai\" }";        
        
        private readonly static byte[] JsonBytesUtf8 = Encoding.UTF8.GetBytes(Json);
        private readonly static byte[] JsonBytesUnicode = Encoding.BigEndianUnicode.GetBytes(Json);
        private readonly static byte[] LowerCaseJsonBytesUtf8 = Encoding.UTF8.GetBytes(LowerCaseJson);
        private readonly static byte[] LowerCaseJsonBytesUnicode = Encoding.BigEndianUnicode.GetBytes(LowerCaseJson);

        private readonly static Stream JsonStreamUtf8 = new MemoryStream(JsonBytesUtf8);
        private readonly static Stream JsonStreamUnicode = new MemoryStream(JsonBytesUnicode);
        private readonly static Stream LowerCaseJsonStreamUtf8 = new MemoryStream(LowerCaseJsonBytesUtf8);
        private readonly static Stream LowerCaseJsonStreamUnicode = new MemoryStream(LowerCaseJsonBytesUnicode);

        private readonly static string JsonPathUtf8 = Path.GetTempFileName();
        private readonly static string JsonPathUnicode = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUtf8 = Path.GetTempFileName();
        private readonly static string LowerCaseJsonPathUnicode = Path.GetTempFileName();

        [SetUp]
        public void Setup()
        {
            JsonStreamUtf8.Reset();
            JsonStreamUnicode.Reset();
            LowerCaseJsonStreamUtf8.Reset();
            LowerCaseJsonStreamUnicode.Reset();

            File.WriteAllBytes(JsonPathUtf8, JsonBytesUtf8);
            File.WriteAllBytes(JsonPathUnicode, JsonBytesUnicode);
            File.WriteAllBytes(LowerCaseJsonPathUtf8, LowerCaseJsonBytesUtf8);
            File.WriteAllBytes(LowerCaseJsonPathUnicode, LowerCaseJsonBytesUnicode);
        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(JsonPathUtf8);
            File.Delete(JsonPathUnicode);
            File.Delete(LowerCaseJsonPathUtf8);
            File.Delete(LowerCaseJsonPathUnicode);
        }

        private static readonly object[] ToObjectCases = TestCases.Create()

            // String

            .AddFunc(() => Deserialize.Json(Json, typeof(Model)))
            .AddFunc(() => Deserialize.Json(LowerCaseJson, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Json(LowerCaseJson, typeof(Model), Options))
            .AddFunc(() => Deserialize.Json<Model>(Json))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJson, Configure))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJson, Options))

            // Bytes

            .AddFunc(() => Deserialize.Json(JsonBytesUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonBytesUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonBytesUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.Json(JsonBytesUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonBytesUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonBytesUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Json<Model>(JsonBytesUtf8))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonBytesUtf8, Options))
            .AddFunc(() => Deserialize.Json<Model>(JsonBytesUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Json(JsonStreamUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonStreamUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonStreamUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.Json(JsonStreamUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonStreamUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(LowerCaseJsonStreamUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Json<Model>(JsonStreamUtf8))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonStreamUtf8, Options))
            .AddFunc(() => Deserialize.Json<Model>(JsonStreamUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json<Model>(LowerCaseJsonStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.JsonFile(JsonPathUtf8, typeof(Model)))
            .AddFunc(() => Deserialize.JsonFile(LowerCaseJsonPathUtf8, typeof(Model), Configure))
            .AddFunc(() => Deserialize.JsonFile(LowerCaseJsonPathUtf8, typeof(Model), Options))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUnicode, typeof(Model), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.JsonFile(LowerCaseJsonPathUnicode, typeof(Model), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.JsonFile(LowerCaseJsonPathUnicode, typeof(Model), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.JsonFile<Model>(JsonPathUtf8))
            .AddFunc(() => Deserialize.JsonFile<Model>(LowerCaseJsonPathUtf8, Configure))
            .AddFunc(() => Deserialize.JsonFile<Model>(LowerCaseJsonPathUtf8, Options))
            .AddFunc(() => Deserialize.JsonFile<Model>(JsonPathUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.JsonFile<Model>(LowerCaseJsonPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.JsonFile<Model>(LowerCaseJsonPathUnicode, Encoding.BigEndianUnicode, Options))

            .All;

        [Test]
        [TestCaseSource("ToObjectCases")]
        public void should_deserialize_to_object(Func<object> deserialize)
        {
            deserialize().As<Model>().Oh.ShouldEqual("Hai");
        }

        private static readonly object[] ToNodeCases = TestCases.Create()

            // String 

            .AddFunc(() => Deserialize.Json(Json))
            .AddFunc(() => Deserialize.Json(Json, Configure))
            .AddFunc(() => Deserialize.Json(Json, Options))
            
            // Bytes

            .AddFunc(() => Deserialize.Json(JsonBytesUtf8))
            .AddFunc(() => Deserialize.Json(JsonBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(JsonBytesUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Json(JsonBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Json(JsonBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(JsonBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Json(JsonStreamUtf8))
            .AddFunc(() => Deserialize.Json(JsonStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(JsonStreamUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Json(JsonStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Json(JsonStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Json(JsonStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.JsonFile(JsonPathUtf8))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUtf8, Configure))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.JsonFile(JsonPathUnicode, Encoding.BigEndianUnicode, Options))
            
            .All;

        [Test]
        [TestCaseSource("ToNodeCases")]
        public void should_deserialize_to_node(Func<INode> deserialize)
        {
            deserialize().GetNode("Oh").Value.ShouldEqual("Hai");;
        }
    }
}
