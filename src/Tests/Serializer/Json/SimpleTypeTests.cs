using System;
using System.Diagnostics;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class SimpleTypeTests
    {
        public enum Enum { Value1, Value2 }

        public class Object {}

        public class SimpleTypeProperties
        {
            public string String { get; set; }
            public bool Boolean { get; set; } public bool? NullableBoolean { get; set; }
            public byte Byte { get; set; } public byte? NullableByte { get; set; }
            public sbyte UnsignedByte { get; set; } public sbyte? NullableUnsignedByte { get; set; }
            public short Short { get; set; } public short? NullableShort { get; set; }
            public ushort UnsignedShort { get; set; } public ushort? NullableUnsignedShort { get; set; }
            public int Integer { get; set; } public int? NullableInteger { get; set; }
            public uint UnsignedInteger { get; set; } public uint? NullableUnsignedInteger { get; set; }
            public long Long { get; set; } public long? NullableLong { get; set; }
            public ulong UnsignedLong { get; set; } public ulong? NullableUnsignedLong { get; set; }
            public float Float { get; set; } public float? NullableFloat { get; set; }
            public double Double { get; set; } public double? NullableDouble { get; set; }
            public decimal Decimal { get; set; } public decimal? NullableDecimal { get; set; }
            public DateTime DateTime { get; set; } public DateTime? NullableDateTime { get; set; }
            public TimeSpan TimeSpan { get; set; } public TimeSpan? NullableTimeSpan { get; set; }
            public Guid Guid { get; set; } public Guid? NullableGuid { get; set; }
            public Enum Enum { get; set; } public Enum? NullableEnum { get; set; }
            public object BoxedValue { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_properties()
        {
            var simpleTypes = new SimpleTypeProperties {
                    String = "hai",
                    Boolean = true, NullableBoolean = true,
                    Byte = 1, NullableByte = 1,
                    UnsignedByte = 2, NullableUnsignedByte = 2,
                    Short = 3, NullableShort = 3,
                    UnsignedShort = 4, NullableUnsignedShort = 4,
                    Integer = 5, NullableInteger = 5,
                    UnsignedInteger = 6, NullableUnsignedInteger = 6,
                    Long = 7, NullableLong = 7,
                    UnsignedLong = 8, NullableUnsignedLong = 8,
                    Float = 1.1F, NullableFloat = 1.1F,
                    Double = 1.2, NullableDouble = 1.2,
                    Decimal = 1.3M, NullableDecimal = 1.3M,
                    DateTime = DateTime.MaxValue, NullableDateTime = DateTime.MaxValue,
                    TimeSpan = TimeSpan.MaxValue, NullableTimeSpan = TimeSpan.MaxValue,
                    Guid = Guid.Empty, NullableGuid = Guid.Empty,
                    Enum = Enum.Value2, NullableEnum = Enum.Value2,
                    BoxedValue = Guid.Empty
                };

            var json = Bender.Serializer.Create().SerializeJson(simpleTypes);
            Debug.WriteLine(json);

            json.ShouldNotBeNull();
            var types = json.ParseJson().JsonRoot();
            types.ShouldNotBeNull();
            types.JsonStringField("String").Value.ShouldEqual(simpleTypes.String);
            types.JsonBooleanField("Boolean").Value.ShouldEqual(simpleTypes.Boolean.ToString().ToLower());
            types.JsonNumberField("Byte").Value.ShouldEqual(simpleTypes.Byte.ToString());
            types.JsonNumberField("UnsignedByte").Value.ShouldEqual(simpleTypes.UnsignedByte.ToString());
            types.JsonNumberField("Short").Value.ShouldEqual(simpleTypes.Short.ToString());
            types.JsonNumberField("UnsignedShort").Value.ShouldEqual(simpleTypes.UnsignedShort.ToString());
            types.JsonNumberField("UnsignedInteger").Value.ShouldEqual(simpleTypes.UnsignedInteger.ToString());
            types.JsonNumberField("Long").Value.ShouldEqual(simpleTypes.Long.ToString());
            types.JsonNumberField("UnsignedLong").Value.ShouldEqual(simpleTypes.UnsignedLong.ToString());
            types.JsonNumberField("Float").Value.ShouldEqual(simpleTypes.Float.ToString());
            types.JsonNumberField("Double").Value.ShouldEqual(simpleTypes.Double.ToString());
            types.JsonNumberField("Decimal").Value.ShouldEqual(simpleTypes.Decimal.ToString());
            types.JsonStringField("DateTime").Value.ShouldEqual(simpleTypes.DateTime.ToString());
            types.JsonStringField("TimeSpan").Value.ShouldEqual(simpleTypes.TimeSpan.ToString());
            types.JsonStringField("Guid").Value.ShouldEqual(simpleTypes.Guid.ToString());
            types.JsonStringField("Enum").Value.ShouldEqual(simpleTypes.Enum.ToString());
            types.JsonStringField("BoxedValue").Value.ShouldEqual(Guid.Empty.ToString());

            types.JsonBooleanField("NullableBoolean").Value.ShouldEqual(simpleTypes.NullableBoolean.ToString().ToLower());
            types.JsonNumberField("NullableByte").Value.ShouldEqual(simpleTypes.NullableByte.ToString());
            types.JsonNumberField("NullableUnsignedByte").Value.ShouldEqual(simpleTypes.NullableUnsignedByte.ToString());
            types.JsonNumberField("NullableShort").Value.ShouldEqual(simpleTypes.NullableShort.ToString());
            types.JsonNumberField("NullableUnsignedShort").Value.ShouldEqual(simpleTypes.NullableUnsignedShort.ToString());
            types.JsonNumberField("NullableUnsignedInteger").Value.ShouldEqual(simpleTypes.NullableUnsignedInteger.ToString());
            types.JsonNumberField("NullableLong").Value.ShouldEqual(simpleTypes.NullableLong.ToString());
            types.JsonNumberField("NullableUnsignedLong").Value.ShouldEqual(simpleTypes.NullableUnsignedLong.ToString());
            types.JsonNumberField("NullableFloat").Value.ShouldEqual(simpleTypes.NullableFloat.ToString());
            types.JsonNumberField("NullableDouble").Value.ShouldEqual(simpleTypes.NullableDouble.ToString());
            types.JsonNumberField("NullableDecimal").Value.ShouldEqual(simpleTypes.NullableDecimal.ToString());
            types.JsonStringField("NullableDateTime").Value.ShouldEqual(simpleTypes.NullableDateTime.ToString());
            types.JsonStringField("NullableTimeSpan").Value.ShouldEqual(simpleTypes.NullableTimeSpan.ToString());
            types.JsonStringField("NullableGuid").Value.ShouldEqual(simpleTypes.NullableGuid.ToString());
            types.JsonStringField("NullableEnum").Value.ShouldEqual(simpleTypes.NullableEnum.ToString());
        }

        [Test]
        public void should_serialize_simple_nullable_type_properties()
        {
            var simpleTypes = new SimpleTypeProperties
            {
                String = null,
                NullableBoolean = null,
                NullableByte = null,
                NullableUnsignedByte = null,
                NullableShort = null,
                NullableUnsignedShort = null,
                NullableInteger = null,
                NullableUnsignedInteger = null,
                NullableLong = null,
                NullableUnsignedLong = null,
                NullableFloat = null,
                NullableDouble = null,
                NullableDecimal = null,
                NullableDateTime = null,
                NullableTimeSpan = null,
                NullableGuid = null,
                NullableEnum = null
            };

            var json = Bender.Serializer.Create().SerializeJson(simpleTypes);
            Debug.WriteLine(json);

            json.ShouldNotBeNull();
            var types = json.ParseJson().JsonRoot();
            types.ShouldNotBeNull();
            types.JsonNullField("String").Value.ShouldBeEmpty();
            types.JsonNullField("NullableBoolean").Value.ShouldBeEmpty();
            types.JsonNullField("NullableByte").Value.ShouldBeEmpty();
            types.JsonNullField("NullableUnsignedByte").Value.ShouldBeEmpty();
            types.JsonNullField("NullableShort").Value.ShouldBeEmpty();
            types.JsonNullField("NullableUnsignedShort").Value.ShouldBeEmpty();
            types.JsonNullField("NullableUnsignedInteger").Value.ShouldBeEmpty();
            types.JsonNullField("NullableLong").Value.ShouldBeEmpty();
            types.JsonNullField("NullableUnsignedLong").Value.ShouldBeEmpty();
            types.JsonNullField("NullableFloat").Value.ShouldBeEmpty();
            types.JsonNullField("NullableDouble").Value.ShouldBeEmpty();
            types.JsonNullField("NullableDecimal").Value.ShouldBeEmpty();
            types.JsonNullField("NullableDateTime").Value.ShouldBeEmpty();
            types.JsonNullField("NullableTimeSpan").Value.ShouldBeEmpty();
            types.JsonNullField("NullableGuid").Value.ShouldBeEmpty();
            types.JsonNullField("NullableEnum").Value.ShouldBeEmpty();
        }
    }
}