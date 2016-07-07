using System.Collections.Generic;
using Bender;
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
        public void should_serialize_csv_with_custom_seperators()
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
    }
}
