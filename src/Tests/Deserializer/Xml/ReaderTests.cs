using System;
using Bender;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class ReaderTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public int NonNullable { get; set; }
            public int? Nullable { get; set; }
        }

        [Test]
        public void should_read_nodes_when()
        {
            var json = "<Model><Property1>oh</Property1><Property2>hai</Property2></Model>";
            var result = Deserialize.Xml<Model>(json,
                x => x.Deserialization(y => y.AddReader(
                    (s, t, o) => { t.Value = s.Value + "!"; },
                    (s, t, o) => s.IsNamed && s.Name.EndsWith("2"))));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        public void should_read_nodes_of_type()
        {
            var json = "<Model><Property1>oh</Property1><Property2>hai</Property2></Model>";
            var result = Deserialize.Xml<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (s, t, o) => t.Value = s.Value + "!")));

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(true, 2, 2)]
        [TestCase(false, 2, 1)]
        public void should_read_nullable_and_non_nullable_nodes_of_type(
            bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => t.Value = 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_nodes_of_type_when()
        {
            var json = "<Model><Property1>oh</Property1><Property2>hai</Property2></Model>";
            var result = Deserialize.Xml<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (s, t, o) => t.Value = s.Value + "!",
                    (s, t, o) => s.Name.EndsWith("2"))));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_read_all_nullable_and_non_nullable_nodes_of_type_when(
            int match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => int.Parse(s.Value.ToString()) == match, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_value_nodes_of_type()
        {
            var json = "<Model><Property1>oh</Property1><Property2>hai</Property2></Model>";
            var result = Deserialize.Xml<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (v, s, t, o) => s.Value + "!")));

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(true, 2, 2)]
        [TestCase(false, 2, 1)]
        public void should_read_nullable_and_non_nullable_value_nodes_of_type(
            bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddReader<int>(
                    (v, s, t, o) => 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_value_nodes_of_type_when()
        {
            var json = "<Model><Property1>oh</Property1><Property2>hai</Property2></Model>";
            var result = Deserialize.Xml<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (v, s, t, o) => s.Value + "!",
                    (v, s, t, o) => s.Name.EndsWith("2"))));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_read_all_nullable_and_non_nullable_value_nodes_of_type_when(
            int match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddReader<int>(
                    (v, s, t, o) => 2,
                    (v, s, t, o) => int.Parse(s.Value.ToString()) == match, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        private class DateTimeConversion
        {
            public DateTime DateTime { get; set; }
        }

        [Test]
        [TestCase("1985-10-26T05:21:00.0000000Z")]
        [TestCase("1985-10-26T05:21:00.0000000")]
        public void should_read_datetime_as_local(string datetime)
        {
            var result = Deserialize.Xml<DateTimeConversion>("<DateTimeConversion><DateTime>{0}</DateTime></DateTimeConversion>".ToFormat(datetime),
                x => x.Deserialization(y => y.TreatDatesAsUtcAndConvertToLocal()));

            result.DateTime.ShouldEqual(new DateTime(1985, 10, 26, 5, 21, 0).SubtractUtcOffset());
        }

        private class NullableDateTimeConversion
        {
            public DateTime? DateTime { get; set; }
        }

        [Test]
        [TestCase("<DateTime>1985-10-26T05:21:00.0000000Z</DateTime>", "10/26/1985 5:21")]
        [TestCase("<DateTime>1985-10-26T05:21:00.0000000</DateTime>", "10/26/1985 5:21")]
        [TestCase("<DateTime/>", null)]
        public void should_read_nullable_datetime_as_local(string datetime, string result)
        {
            var @object = Deserialize.Xml<NullableDateTimeConversion>("<NullableDateTimeConversion>{0}</NullableDateTimeConversion>".ToFormat(datetime),
                x => x.Deserialization(y => y.TreatDatesAsUtcAndConvertToLocal()));

            @object.DateTime.ShouldEqual(result == null ? (DateTime?)null : DateTime.Parse(result).SubtractUtcOffset());
        }
    }
}
