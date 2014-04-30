using System;
using Bender;
using Flexo.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<Model>(json,
                x => x.Deserialization(y => y.AddReader(
                    (s, t, o) => { t.Value = s.Value + "!"; },
                    (s, t, o) => s.IsNamed && s.Name.EndsWith("2"))));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        public void should_read_nodes_of_type()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<Model>(json,
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
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => t.Value = 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_nodes_of_type_when()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<Model>(json,
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
            decimal match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => (decimal)s.Value == match, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_value_nodes_of_type()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (s, t, o) => s.Value + "!")));

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(true, 2, 2)]
        [TestCase(false, 2, 1)]
        public void should_read_nullable_and_non_nullable_value_nodes_of_type(
            bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_read_value_nodes_of_type_when()
        {
            var json = "{ \"Property1\": \"oh\", \"Property2\": \"hai\" }";
            var result = Deserialize.Json<Model>(json,
                x => x.Deserialization(y => y.AddReader<string>(
                    (s, t, o) => s.Value + "!",
                    (s, t, o) => s.Name.EndsWith("2"))));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("hai!");
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_read_all_nullable_and_non_nullable_value_nodes_of_type_when(
            decimal match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddReader<int>(
                    (s, t, o) => 2,
                    (s, t, o) => (decimal)s.Value == match, includeNullable)));

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
            var result = Deserialize.Json<DateTimeConversion>("{{ \"DateTime\": \"{0}\" }}".ToFormat(datetime),
                x => x.Deserialization(y => y.TreatAllDateTimesAsUtcAndConvertToLocal()));

            result.DateTime.ShouldEqual(new DateTime(1985, 10, 26, 1, 21, 0));
        }
    }
}
