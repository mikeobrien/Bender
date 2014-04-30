using System;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class WriterTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }

        [Test]
        public void should_write_nodes_when()
        {
            Serialize.Json(new Model { Property1 = "oh",Property2 = "hai"},
                x => x.Serialization(y => y.AddWriter(
                    (v, s, t, o) => { t.Value = s.Value + "!"; },
                    (v, s, t, o) => s.IsNamed && s.Name.EndsWith("2"))))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"hai!\"}");
        }

        [Test]
        public void should_write_nodes_of_type()
        {
            Serialize.Json(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => t.Value = s.Value + "!")))
                .ShouldEqual("{\"Property1\":\"oh!\",\"Property2\":\"hai!\"}");
        }

        [Test]
        public void should_write_nodes_of_type_when()
        {
            Serialize.Json(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => t.Value = s.Value + "!",
                    (v, s, t, o) => s.Name.EndsWith("2"))))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"hai!\"}");
        }

        [Test]
        public void should_write_value_nodes_of_type()
        {
            Serialize.Json(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => s.Value + "!")))
                .ShouldEqual("{\"Property1\":\"oh!\",\"Property2\":\"hai!\"}");
        }

        [Test]
        public void should_write_value_nodes_of_type_when()
        {
            Serialize.Json(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => s.Value + "!",
                    (v, s, t, o) => s.Name.EndsWith("2"))))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"hai!\"}");
        }

        private class DateTimeConversion
        {
            public DateTime DateTime { get; set; }
        }

        [Test]
        public void should_write_datetime_as_utc_iso8601()
        {
            Serialize.Json(new DateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsUtcIso8601()))
                .ShouldEqual("{\"DateTime\":\"1985-10-26T05:21:00.0000000Z\"}");
        }

        [Test]
        public void should_write_datetime_as_microsoft_datetime()
        {
            Serialize.Json(new DateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsMicrosoftJsonDateTime()))
                .ShouldEqual("{\"DateTime\":\"\\/Date(499152060000)\\/\"}");
        }
    }
}
