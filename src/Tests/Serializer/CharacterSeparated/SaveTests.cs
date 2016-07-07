using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes.CharacterSeparated;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.CharacterSeparated
{
    [TestFixture]
    public class SaveTests
    {
        public static Action<OptionsDsl> Configure = x => x.UseCamelCaseNaming();
        public static Options Options = Options.Create(x => x.UseCamelCaseNaming());

        public interface IModel
        {
            string Fark { get; set; }
        }

        public class Model : IModel
        {
            public string Fark { get; set; }
            public string Farker { get; set; }
        }

        public static List<Model> ModelInstance = new List<Model> { new Model { Fark = "Hai", Farker = "Oh" } };
        public static List<IModel> InterfaceInstance = new List<IModel> { new Model { Fark = "Hai", Farker = "Oh" } };

        // String

        public const string Csv = "\"Fark\",\"Farker\"\r\n\"Hai\",\"Oh\"\r\n";
        public const string LowerCaseCsv = "\"fark\",\"farker\"\r\n\"Hai\",\"Oh\"\r\n";

        public const string CsvByType = "\"Fark\"\r\n\"Hai\"\r\n";
        public const string LowerCaseCsvByType = "\"fark\"\r\n\"Hai\"\r\n";

        private static readonly object[] StringCases = TestCases.Create()

            .AddFunc(() => Serialize.Csv(ModelInstance), Csv)
            .AddFunc(() => Serialize.Csv(ModelInstance, Configure), LowerCaseCsv)
            .AddFunc(() => Serialize.Csv(ModelInstance, Options), LowerCaseCsv)

            .AddFunc(() => Serialize.Csv(InterfaceInstance), CsvByType)
            .AddFunc(() => Serialize.Csv(InterfaceInstance, Configure), LowerCaseCsvByType)
            .AddFunc(() => Serialize.Csv(InterfaceInstance, Options), LowerCaseCsvByType)

            .AddFunc(() => Serialize.Csv(ModelInstance, typeof(List<IModel>)), CsvByType)
            .AddFunc(() => Serialize.Csv(ModelInstance, typeof(List<IModel>), Configure), LowerCaseCsvByType)
            .AddFunc(() => Serialize.Csv(ModelInstance, typeof(List<IModel>), Options), LowerCaseCsvByType)

            .All;

        [Test]
        [TestCaseSource(nameof(StringCases))]
        public void should_serialize_string(Func<string> serialize, string output)
        {
            serialize().ShouldEqual(output);
        }

        // Bytes

        private static readonly UnicodeEncoding BigEndianUnicode = new UnicodeEncoding(true, false);

        private static readonly byte[] CsvBytesUtf8 = FileNode.DefaultEncoding.GetBytes(Csv);
        private static readonly byte[] CsvBytesUnicode = BigEndianUnicode.GetBytes(Csv);
        private static readonly byte[] LowerCaseCsvBytesUtf8 = FileNode.DefaultEncoding.GetBytes(LowerCaseCsv);
        private static readonly byte[] LowerCaseCsvBytesUnicode = BigEndianUnicode.GetBytes(LowerCaseCsv);

        private static readonly byte[] CsvBytesUtf8ByType = FileNode.DefaultEncoding.GetBytes(CsvByType);
        private static readonly byte[] CsvBytesUnicodeByType = BigEndianUnicode.GetBytes(CsvByType);
        private static readonly byte[] LowerCaseCsvBytesUtf8ByType = FileNode.DefaultEncoding.GetBytes(LowerCaseCsvByType);
        private static readonly byte[] LowerCaseCsvBytesUnicodeByType = BigEndianUnicode.GetBytes(LowerCaseCsvByType);

        private static readonly object[] ByteCases = TestCases.Create()

            .AddFunc(() => Serialize.CsvBytes(ModelInstance), CsvBytesUtf8)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, Configure), LowerCaseCsvBytesUtf8)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, Options), LowerCaseCsvBytesUtf8)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, BigEndianUnicode), CsvBytesUnicode)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, BigEndianUnicode, Configure), LowerCaseCsvBytesUnicode)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, BigEndianUnicode, Options), LowerCaseCsvBytesUnicode)

            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance), CsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance, Configure), LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance, Options), LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance, BigEndianUnicode), CsvBytesUnicodeByType)
            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance, BigEndianUnicode, Configure), LowerCaseCsvBytesUnicodeByType)
            .AddFunc(() => Serialize.CsvBytes(InterfaceInstance, BigEndianUnicode, Options), LowerCaseCsvBytesUnicodeByType)

            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>)), CsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>), Configure), LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>), Options), LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>), BigEndianUnicode), CsvBytesUnicodeByType)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>),
                BigEndianUnicode, Configure), LowerCaseCsvBytesUnicodeByType)
            .AddFunc(() => Serialize.CsvBytes(ModelInstance, typeof(List<IModel>),
                BigEndianUnicode, Options), LowerCaseCsvBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(ByteCases))]
        public void should_serialize_bytes(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }

        // Stream

        private static readonly Stream CsvStreamUtf8 = new MemoryStream(CsvBytesUtf8);
        private static readonly Stream CsvStreamUnicode = new MemoryStream(CsvBytesUnicode);
        private static readonly Stream LowerCaseCsvStreamUtf8 = new MemoryStream(LowerCaseCsvBytesUtf8);
        private static readonly Stream LowerCaseCsvStreamUnicode = new MemoryStream(LowerCaseCsvBytesUnicode);

        private static readonly Stream CsvStreamUtf8ByType = new MemoryStream(CsvBytesUtf8ByType);
        private static readonly Stream CsvStreamUnicodeByType = new MemoryStream(CsvBytesUnicodeByType);
        private static readonly Stream LowerCaseCsvStreamUtf8ByType = new MemoryStream(LowerCaseCsvBytesUtf8ByType);
        private static readonly Stream LowerCaseCsvStreamUnicodeByType = new MemoryStream(LowerCaseCsvBytesUnicodeByType);

        [SetUp]
        public void CreateStreams()
        {
            CsvStreamUtf8.Reset();
            CsvStreamUnicode.Reset();
            LowerCaseCsvStreamUtf8.Reset();
            LowerCaseCsvStreamUnicode.Reset();

            CsvStreamUtf8ByType.Reset();
            CsvStreamUnicodeByType.Reset();
            LowerCaseCsvStreamUtf8ByType.Reset();
            LowerCaseCsvStreamUnicodeByType.Reset();
        }

        private static readonly object[] AsStreamCases = TestCases.Create()

            .AddFunc(() => Serialize.CsvStream(ModelInstance), CsvStreamUtf8)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, Configure), LowerCaseCsvStreamUtf8)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, Options), LowerCaseCsvStreamUtf8)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, BigEndianUnicode), CsvStreamUnicode)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, BigEndianUnicode, Configure), LowerCaseCsvStreamUnicode)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, BigEndianUnicode, Options), LowerCaseCsvStreamUnicode)

            .AddFunc(() => Serialize.CsvStream(InterfaceInstance), CsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(InterfaceInstance, Configure), LowerCaseCsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(InterfaceInstance, Options), LowerCaseCsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(InterfaceInstance, BigEndianUnicode), CsvStreamUnicodeByType)
            .AddFunc(() => Serialize.CsvStream(InterfaceInstance, BigEndianUnicode, Configure), LowerCaseCsvStreamUnicodeByType)
            .AddFunc(() => Serialize.CsvStream(InterfaceInstance, BigEndianUnicode, Options), LowerCaseCsvStreamUnicodeByType)

            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>)), CsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), Configure), LowerCaseCsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), Options), LowerCaseCsvStreamUtf8ByType)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), BigEndianUnicode), CsvStreamUnicodeByType)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), BigEndianUnicode, Configure), LowerCaseCsvStreamUnicodeByType)
            .AddFunc(() => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), BigEndianUnicode, Options), LowerCaseCsvStreamUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(AsStreamCases))]
        public void should_serialize_as_stream(Func<Stream> serialize, Stream output)
        {
            serialize().ShouldEqual(output);
        }

        private static readonly object[] ToStreamCases = TestCases.Create()

            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x), CsvStreamUtf8)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x, Configure), LowerCaseCsvStreamUtf8)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x, Options), LowerCaseCsvStreamUtf8)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x, BigEndianUnicode), CsvStreamUnicode)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x, BigEndianUnicode, Configure), LowerCaseCsvStreamUnicode)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, x, BigEndianUnicode, Options), LowerCaseCsvStreamUnicode)

            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x), CsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x, Configure), LowerCaseCsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x, Options), LowerCaseCsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x, BigEndianUnicode), CsvStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x, BigEndianUnicode, Configure), LowerCaseCsvStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.CsvStream(InterfaceInstance, x, BigEndianUnicode, Options), LowerCaseCsvStreamUnicodeByType)

            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x), CsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x, Configure), LowerCaseCsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x, Options), LowerCaseCsvStreamUtf8ByType)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x, BigEndianUnicode), CsvStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x, BigEndianUnicode, Configure), LowerCaseCsvStreamUnicodeByType)
            .AddAction<Stream>(x => Serialize.CsvStream(ModelInstance, typeof(List<IModel>), x, BigEndianUnicode, Options), LowerCaseCsvStreamUnicodeByType)

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

        private static readonly string CsvPathUtf8 = Path.GetTempFileName();
        private static readonly string CsvPathUnicode = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUtf8 = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUnicode = Path.GetTempFileName();

        private static readonly string CsvPathUtf8ByType = Path.GetTempFileName();
        private static readonly string CsvPathUnicodeByType = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUtf8ByType = Path.GetTempFileName();
        private static readonly string LowerCaseCsvPathUnicodeByType = Path.GetTempFileName();

        [SetUp]
        public void CreateFiles()
        {
            File.WriteAllBytes(CsvPathUtf8, CsvBytesUtf8);
            File.WriteAllBytes(CsvPathUnicode, CsvBytesUnicode);
            File.WriteAllBytes(LowerCaseCsvPathUtf8, LowerCaseCsvBytesUtf8);
            File.WriteAllBytes(LowerCaseCsvPathUnicode, LowerCaseCsvBytesUnicode);

            File.WriteAllBytes(CsvPathUtf8ByType, CsvBytesUtf8ByType);
            File.WriteAllBytes(CsvPathUnicodeByType, CsvBytesUnicodeByType);
            File.WriteAllBytes(LowerCaseCsvPathUtf8ByType, LowerCaseCsvBytesUtf8ByType);
            File.WriteAllBytes(LowerCaseCsvPathUnicodeByType, LowerCaseCsvBytesUnicodeByType);
        }

        [TearDown]
        public void DeleteFiles()
        {
            File.Delete(CsvPathUtf8);
            File.Delete(CsvPathUnicode);
            File.Delete(LowerCaseCsvPathUtf8);
            File.Delete(LowerCaseCsvPathUnicode);

            File.Delete(CsvPathUtf8ByType);
            File.Delete(CsvPathUnicodeByType);
            File.Delete(LowerCaseCsvPathUtf8ByType);
            File.Delete(LowerCaseCsvPathUnicodeByType);
        }

        private static readonly object[] FileCases = TestCases.Create()

            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, CsvPathUtf8);
                return File.ReadAllBytes(CsvPathUtf8);
            }, CsvBytesUtf8)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, LowerCaseCsvPathUtf8, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8);
            }, LowerCaseCsvBytesUtf8)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, LowerCaseCsvPathUtf8, Options);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8);
            }, LowerCaseCsvBytesUtf8)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, CsvPathUnicode, BigEndianUnicode);
                return File.ReadAllBytes(CsvPathUnicode);
            }, CsvBytesUnicode)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, LowerCaseCsvPathUnicode, BigEndianUnicode, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUnicode);
            }, LowerCaseCsvBytesUnicode)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, LowerCaseCsvPathUnicode, BigEndianUnicode, Options);
                return File.ReadAllBytes(LowerCaseCsvPathUnicode);
            }, LowerCaseCsvBytesUnicode)

            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, CsvPathUtf8ByType);
                return File.ReadAllBytes(CsvPathUtf8ByType);
            }, CsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, LowerCaseCsvPathUtf8ByType, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8ByType);
            }, LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, LowerCaseCsvPathUtf8ByType, Options);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8ByType);
            }, LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, CsvPathUnicodeByType, BigEndianUnicode);
                return File.ReadAllBytes(CsvPathUnicodeByType);
            }, CsvBytesUnicodeByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, LowerCaseCsvPathUnicodeByType, BigEndianUnicode, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUnicodeByType);
            }, LowerCaseCsvBytesUnicodeByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(InterfaceInstance, LowerCaseCsvPathUnicodeByType, BigEndianUnicode, Options);

                return File.ReadAllBytes(LowerCaseCsvPathUnicodeByType);
            }, LowerCaseCsvBytesUnicodeByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), CsvPathUtf8ByType);
                return File.ReadAllBytes(CsvPathUtf8ByType);
            }, CsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), LowerCaseCsvPathUtf8ByType, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8ByType);
            }, LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), LowerCaseCsvPathUtf8ByType, Options);
                return File.ReadAllBytes(LowerCaseCsvPathUtf8ByType);
            }, LowerCaseCsvBytesUtf8ByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), CsvPathUnicodeByType, BigEndianUnicode);
                return File.ReadAllBytes(CsvPathUnicodeByType);
            }, CsvBytesUnicodeByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), LowerCaseCsvPathUnicodeByType, BigEndianUnicode, Configure);
                return File.ReadAllBytes(LowerCaseCsvPathUnicodeByType);
            }, LowerCaseCsvBytesUnicodeByType)
            .AddFunc(() =>
            {
                Serialize.CsvFile(ModelInstance, typeof(List<IModel>), LowerCaseCsvPathUnicodeByType, BigEndianUnicode, Options);
                return File.ReadAllBytes(LowerCaseCsvPathUnicodeByType);
            }, LowerCaseCsvBytesUnicodeByType)

            .All;

        [Test]
        [TestCaseSource(nameof(FileCases))]
        public void should_serialize_file(Func<byte[]> serialize, byte[] output)
        {
            serialize().ShouldEqual(output);
        }
    }
}
