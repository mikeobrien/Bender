using System;
using Bender;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
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
            Serialize.Xml(new Model { Property1 = "oh",Property2 = "hai"},
                x => x.Serialization(y => y.AddWriter(
                    (v, s, t, o) => { t.Value = s.Value + "!"; },
                    (v, s, t, o) => s.IsNamed && s.Name.EndsWith("2"))))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>hai!</Property2></Model>");
        }

        [Test]
        public void should_write_nodes_of_type()
        {
            Serialize.Xml(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => t.Value = s.Value + "!")))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh!</Property1><Property2>hai!</Property2></Model>");
        }

        [Test]
        public void should_write_nodes_of_type_when()
        {
            Serialize.Xml(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => t.Value = s.Value + "!",
                    (v, s, t, o) => s.Name.EndsWith("2"))))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>hai!</Property2></Model>");
        }

        [Test]
        public void should_write_value_nodes_of_type()
        {
            Serialize.Xml(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => s.Value + "!")))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh!</Property1><Property2>hai!</Property2></Model>");
        }

        [Test]
        public void should_write_value_nodes_of_type_when()
        {
            Serialize.Xml(new Model { Property1 = "oh", Property2 = "hai" },
                x => x.Serialization(y => y.AddWriter<string>(
                    (v, s, t, o) => s.Value + "!",
                    (v, s, t, o) => s.Name.EndsWith("2"))))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>hai!</Property2></Model>");
        }

        private class DateTimeConversion
        {
            public DateTime DateTime { get; set; }
        }

        private class NullableDateTimeConversion
        {
            public DateTime? DateTime { get; set; }
        }

        [Test]
        public void should_write_datetime_as_utc_iso8601()
        {
            Serialize.Xml(new DateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsUtcIso8601()))
                .ShouldEqual(Xml.Declaration + "<DateTimeConversion><DateTime>1985-10-26T05:21:00.0000000Z</DateTime></DateTimeConversion>");
        }

        [Test]
        public void should_write_nullable_datetime_as_utc_iso8601()
        {
            Serialize.Xml(new NullableDateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsUtcIso8601()))
                .ShouldEqual(Xml.Declaration + "<NullableDateTimeConversion><DateTime>1985-10-26T05:21:00.0000000Z</DateTime></NullableDateTimeConversion>");
        }

        [Test]
        public void should_write_datetime_as_microsoft_datetime()
        {
            Serialize.Xml(new DateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsMicrosoftJsonDateTime()))
                .ShouldEqual(Xml.Declaration + "<DateTimeConversion><DateTime>/Date(499152060000)/</DateTime></DateTimeConversion>");
        }

        [Test]
        public void should_write_nullable_datetime_as_microsoft_datetime()
        {
            Serialize.Xml(new NullableDateTimeConversion { DateTime = new DateTime(1985, 10, 26, 1, 21, 0) },
                x => x.Serialization(y => y.WriteDateTimeAsMicrosoftJsonDateTime()))
                .ShouldEqual(Xml.Declaration + "<NullableDateTimeConversion><DateTime>/Date(499152060000)/</DateTime></NullableDateTimeConversion>");
        }
    }
}
