using System;
using System.Collections.Generic;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.CharacterSeparated
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
                "\"Property\",\"Field\"\r\n" +
                "\"oh,\",\"\"\"hai\"\"\"\r\n" +
                "\"fark,\",\"\"\"farker\"\"\"\r\n",
                x => x.IncludePublicFields());
                
            results.Count.ShouldEqual(2);
            results[0].Property.ShouldEqual("oh,");
            results[0].Field.ShouldEqual("\"hai\"");
            results[1].Property.ShouldEqual("fark,");
            results[1].Field.ShouldEqual("\"farker\"");
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

        public class NestedRecordParent
        {
            public string Field;
            public string Property { get; set; }
            public NestedRecordChild Child { get; set; }
        }

        public class NestedRecordChild
        {
            public string Field;
            public string Property { get; set; }
            public Guid Guid { get; set; }
        }

        [Test]
        public void should_deserialize_nested_objects_as_csv()
        {
            var results = Deserialize.Csv<List<NestedRecordParent>>(
                "\"Property\",\"ChildProperty\",\"ChildField\",\"Field\"\r\n" +
                "\"oh,\",\"fark,\",\"\"\"farker\"\"\",\"\"\"hai\"\"\"\r\n",
                x => x.IncludePublicFields());

            results.Count.ShouldEqual(1);

            results[0].Property.ShouldEqual("oh,");
            results[0].Field.ShouldEqual("\"hai\"");

            var child = results[0].Child;
            child.ShouldNotBeNull();
            child.Property.ShouldEqual("fark,");
            child.Field.ShouldEqual("\"farker\"");
        }

        [Test]
        public void should_not_deserialize_empty_values_when_configured()
        {
            var results = Deserialize.Csv<List<NestedRecordParent>>(
                "\"Property\",\"ChildProperty\",\"ChildField\",\"Field\"\r\n" +
                "\"oh,\",,,\"\"\"hai\"\"\"\r\n",
                x => x.IncludePublicFields().Deserialization(d => d.IgnoreEmptyCsvValues()));

            results.Count.ShouldEqual(1);

            results[0].Property.ShouldEqual("oh,");
            results[0].Field.ShouldEqual("\"hai\"");

            results[0].Child.ShouldBeNull();
        }
    }
}
