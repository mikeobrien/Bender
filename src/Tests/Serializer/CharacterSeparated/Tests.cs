using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bender;
using Bender.Collections;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.CharacterSeparated
{
    [TestFixture]
    public class CharacterSeparated
    {
        public class Record
        {
            public string Field;
            public string Property { get; set; }
        }

        [Test]
        public void should_serialize_csv()
        {
            Serialize.Csv(new List<Record>
            {
                new Record { Property = "oh", Field = "\"hai\"" }
            }, x => x.IncludePublicFields()).ShouldEqual(
                "\"Property\",\"Field\"\r\n\"oh\",\"\"\"hai\"\"\"\r\n");
        }

        [Test]
        public void should_serialize_csv_with_custom_separators()
        {
            Serialize.Csv(new List<Record>
            {
                new Record { Property = "oh,", Field = "*hai*" }
            }, x => x
                .IncludePublicFields()
                .WithCsvDelimiter("-")
                .WithCsvQualifier("*")
                .WithCsvNewLine("#"))
                .ShouldEqual("*Property*-*Field*#*oh,*-***hai***#");
        }

        [Test, Ignore("Intended to be run manually as a sanity check to make sure nothing is being buffered.")]
        public void Should_serialize_data_to_a_stream_and_not_buffer()
        {
            using (var stream = new FileStream(
                Path.Combine(Path.GetTempPath(), "bender.csv"), 
                FileMode.Create, FileAccess.Write))
            {
                Serialize.CsvStream(GetData(), typeof(IEnumerable<Model>), stream, x => x
                    .Serialization(y => y.WriteDateTimeAsUtcIso8601()));
            }
        }

        private static IEnumerable GetData()
        {
            var model = new Model();
            typeof(Model).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ForEach(x => x.SetValue(model, new string('*', 30)));
            for (var i = 0; i < 1000000; i++)
            {
                yield return model;
            }
        }

        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string Property3 { get; set; }
            public string Property4 { get; set; }
            public string Property5 { get; set; }
            public string Property6 { get; set; }
            public string Property7 { get; set; }
            public string Property8 { get; set; }
            public string Property9 { get; set; }
            public string Property10 { get; set; }
            public string Property11 { get; set; }
            public string Property12 { get; set; }
            public string Property13 { get; set; }
            public string Property14 { get; set; }
            public string Property15 { get; set; }
            public string Property16 { get; set; }
            public string Property17 { get; set; }
            public string Property18 { get; set; }
            public string Property19 { get; set; }
            public string Property20 { get; set; }
        }
    }
}
