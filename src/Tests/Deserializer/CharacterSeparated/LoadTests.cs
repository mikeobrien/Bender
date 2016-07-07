using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.CharacterSeparated
{
    [TestFixture]
    public class LoadTests
    {
        public static Action<OptionsDsl> Configure = x => x.Deserialization(y => y.IgnoreNameCase());
        public static Options Options = Options.Create(x => x.Deserialization(y => y.IgnoreNameCase()));
        private static readonly UnicodeEncoding BigEndianUnicode = new UnicodeEncoding(true, false);

        public class Model
        {
            public string Fark { get; set; }
        }

        // String 

        public const string Csv = "\"Fark\"\r\n\"Hai\"\r\n";
        public const string LowerCaseCsv = "\"fark\"\r\n\"Hai\"\r\n";

        private static readonly byte[] CsvBytesUtf8 = Encoding.UTF8.GetBytes(Csv);
        private static readonly byte[] CsvBytesUnicode = Encoding.BigEndianUnicode.GetBytes(Csv);
        private static readonly byte[] LowerCaseCsvBytesUtf8 = Encoding.UTF8.GetBytes(LowerCaseCsv);
        private static readonly byte[] LowerCaseCsvBytesUnicode = Encoding.BigEndianUnicode.GetBytes(LowerCaseCsv);

        private static readonly Stream CsvStreamUtf8 = new MemoryStream(CsvBytesUtf8);
        private static readonly Stream CsvStreamUnicode = new MemoryStream(CsvBytesUnicode);
        private static readonly Stream LowerCaseCsvStreamUtf8 = new MemoryStream(LowerCaseCsvBytesUtf8);
        private static readonly Stream LowerCaseCsvStreamUnicode = new MemoryStream(LowerCaseCsvBytesUnicode);

        private static readonly string CsvPathUtf8 = Path.GetTempFileName();
        private static readonly string CsvPathUnicode = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUtf8 = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUnicode = Path.GetTempFileName();

        [SetUp]
        public void Setup()
        {
            CsvStreamUtf8.Reset();
            CsvStreamUnicode.Reset();
            LowerCaseCsvStreamUtf8.Reset();
            LowerCaseCsvStreamUnicode.Reset();

            File.WriteAllBytes(CsvPathUtf8, CsvBytesUtf8);
            File.WriteAllBytes(CsvPathUnicode, CsvBytesUnicode);
            File.WriteAllBytes(LowerCaseCsvPathUtf8, LowerCaseCsvBytesUtf8);
            File.WriteAllBytes(LowerCaseCsvPathUnicode, LowerCaseCsvBytesUnicode);
        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(CsvPathUtf8);
            File.Delete(CsvPathUnicode);
            File.Delete(LowerCaseCsvPathUtf8);
            File.Delete(LowerCaseCsvPathUnicode);
        }

        private static readonly object[] ToObjectCases = TestCases.Create()

            // String

            .AddFunc(() => Deserialize.Csv(Csv, typeof(List<Model>)))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsv, typeof(List<Model>), Configure))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsv, typeof(List<Model>), Options))
            .AddFunc(() => Deserialize.Csv<List<Model>>(Csv))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsv, Configure))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsv, Options))

            // Bytes

            .AddFunc(() => Deserialize.Csv(CsvBytesUtf8, typeof(List<Model>)))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvBytesUtf8, typeof(List<Model>), Configure))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvBytesUtf8, typeof(List<Model>), Options))
            .AddFunc(() => Deserialize.Csv(CsvBytesUnicode, typeof(List<Model>), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvBytesUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvBytesUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Csv<List<Model>>(CsvBytesUtf8))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvBytesUtf8, Options))
            .AddFunc(() => Deserialize.Csv<List<Model>>(CsvBytesUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Csv(CsvStreamUtf8, typeof(List<Model>)))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvStreamUtf8, typeof(List<Model>), Configure))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvStreamUtf8, typeof(List<Model>), Options))
            .AddFunc(() => Deserialize.Csv(CsvStreamUnicode, typeof(List<Model>), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvStreamUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(LowerCaseCsvStreamUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Csv<List<Model>>(CsvStreamUtf8))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvStreamUtf8, Options))
            .AddFunc(() => Deserialize.Csv<List<Model>>(CsvStreamUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv<List<Model>>(LowerCaseCsvStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.CsvFile(CsvPathUtf8, typeof(List<Model>)))
            .AddFunc(() => Deserialize.CsvFile(LowerCaseCsvPathUtf8, typeof(List<Model>), Configure))
            .AddFunc(() => Deserialize.CsvFile(LowerCaseCsvPathUtf8, typeof(List<Model>), Options))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUnicode, typeof(List<Model>), Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.CsvFile(LowerCaseCsvPathUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.CsvFile(LowerCaseCsvPathUnicode, typeof(List<Model>), Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(CsvPathUtf8))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(LowerCaseCsvPathUtf8, Configure))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(LowerCaseCsvPathUtf8, Options))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(CsvPathUnicode, Encoding.BigEndianUnicode))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(LowerCaseCsvPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.CsvFile<List<Model>>(LowerCaseCsvPathUnicode, Encoding.BigEndianUnicode, Options))

            .All;

        [Test]
        [TestCaseSource(nameof(ToObjectCases))]
        public void should_deserialize_to_object(Func<object> deserialize)
        {
            var results = deserialize().As<List<Model>>();
            results.Count.ShouldEqual(1);
            results.First().Fark.ShouldEqual("Hai");
        }

        private static readonly object[] ToNodeCases = TestCases.Create()

            // String 

            .AddFunc(() => Deserialize.Csv(Csv))
            .AddFunc(() => Deserialize.Csv(Csv, Configure))
            .AddFunc(() => Deserialize.Csv(Csv, Options))
            
            // Bytes

            .AddFunc(() => Deserialize.Csv(CsvBytesUtf8))
            .AddFunc(() => Deserialize.Csv(CsvBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(CsvBytesUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Csv(CsvBytesUtf8, Configure))
            .AddFunc(() => Deserialize.Csv(CsvBytesUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(CsvBytesUnicode, Encoding.BigEndianUnicode, Options))

            // Stream

            .AddFunc(() => Deserialize.Csv(CsvStreamUtf8))
            .AddFunc(() => Deserialize.Csv(CsvStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(CsvStreamUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.Csv(CsvStreamUtf8, Configure))
            .AddFunc(() => Deserialize.Csv(CsvStreamUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.Csv(CsvStreamUnicode, Encoding.BigEndianUnicode, Options))

            // File

            .AddFunc(() => Deserialize.CsvFile(CsvPathUtf8))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUnicode, Encoding.BigEndianUnicode, Options))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUtf8, Configure))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUnicode, Encoding.BigEndianUnicode, Configure))
            .AddFunc(() => Deserialize.CsvFile(CsvPathUnicode, Encoding.BigEndianUnicode, Options))

            .All;
        
        [Test]
        [TestCaseSource(nameof(ToNodeCases))]
        public void should_deserialize_to_node(Func<INode> deserialize)
        {
            var nodes = deserialize().ToList();
            nodes.Count.ShouldEqual(1);
            nodes.First().GetNode("Fark").Value.ShouldEqual("Hai");;
        }
    }
}
