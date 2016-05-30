using System.Collections.Generic;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
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
        public void should_deserialize_csv()
        {
            var results = Deserialize.Csv<List<Record>>(
                "\"Property\",\"Field\"\r\n\"oh,\",\"\"\"hai\"\"\"\r\n",
                x => x.IncludePublicFields());
                
            results.Count.ShouldEqual(1);
            results[0].Property.ShouldEqual("oh,");
            results[0].Field.ShouldEqual("\"hai\"");
        }

        [Test]
        public void should_deserialize_csv_with_custom_seperators()
        {
            var results = Deserialize.Csv<List<Record>>(
                "\"Property\"-\"Field\"\r\n\"oh\"-\"\"\"hai\"\"\"\r\n",
                x => x.IncludePublicFields().WithCsvDelimiter("-"));

            results.Count.ShouldEqual(1);
            results[0].Property.ShouldEqual("oh");
            results[0].Field.ShouldEqual("\"hai\"");
        }
    }
}
