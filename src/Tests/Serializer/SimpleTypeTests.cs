using System;
using System.Diagnostics;
using System.Xml.Linq;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
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
            public byte[] ByteArray { get; set; }
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
            public Uri Uri { get; set; }
            public object Object { get; set; }
            public object BoxedValue { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_properties()
        {
            var simpleTypes = new SimpleTypeProperties {
                    String = "hai",
                    Boolean = true, NullableBoolean = true,
                    Byte = 1, NullableByte = 1,
                    ByteArray = new byte[] { 1, 2, 3 },
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
                    Uri = new Uri("http://www.google.com"),
                    Enum = Enum.Value2, NullableEnum = Enum.Value2,
                    Object = new Object(),
                    BoxedValue = Guid.Empty
                };

            var xml = Bender.Serializer.Create(x => x.PrettyPrint()).Serialize(simpleTypes);
            Debug.WriteLine(xml);

            xml.ShouldNotBeNull();
            var root = XDocument.Parse(xml).Element("SimpleTypeProperties");
            root.ShouldNotBeNull();
            root.Element("String").Value.ShouldEqual(simpleTypes.String);
            root.Element("Boolean").Value.ShouldEqual(simpleTypes.Boolean.ToString().ToLower());
            root.Element("Byte").Value.ShouldEqual(simpleTypes.Byte.ToString());
            root.Element("ByteArray").Value.ShouldEqual(Convert.ToBase64String(simpleTypes.ByteArray));
            root.Element("UnsignedByte").Value.ShouldEqual(simpleTypes.UnsignedByte.ToString());
            root.Element("Short").Value.ShouldEqual(simpleTypes.Short.ToString());
            root.Element("UnsignedShort").Value.ShouldEqual(simpleTypes.UnsignedShort.ToString());
            root.Element("UnsignedInteger").Value.ShouldEqual(simpleTypes.UnsignedInteger.ToString());
            root.Element("Long").Value.ShouldEqual(simpleTypes.Long.ToString());
            root.Element("UnsignedLong").Value.ShouldEqual(simpleTypes.UnsignedLong.ToString());
            root.Element("Float").Value.ShouldEqual(simpleTypes.Float.ToString());
            root.Element("Double").Value.ShouldEqual(simpleTypes.Double.ToString());
            root.Element("Decimal").Value.ShouldEqual(simpleTypes.Decimal.ToString());
            root.Element("DateTime").Value.ShouldEqual(simpleTypes.DateTime.ToString());
            root.Element("TimeSpan").Value.ShouldEqual(simpleTypes.TimeSpan.ToString());
            root.Element("Guid").Value.ShouldEqual(simpleTypes.Guid.ToString());
            root.Element("Enum").Value.ShouldEqual(simpleTypes.Enum.ToString());
            root.Element("Uri").Value.ShouldEqual(simpleTypes.Uri.ToString());
            root.Element("Object").Value.ShouldBeEmpty();
            root.Element("BoxedValue").Value.ShouldEqual(Guid.Empty.ToString());

            root.Element("NullableBoolean").Value.ShouldEqual(simpleTypes.NullableBoolean.ToString().ToLower());
            root.Element("NullableByte").Value.ShouldEqual(simpleTypes.NullableByte.ToString());
            root.Element("NullableUnsignedByte").Value.ShouldEqual(simpleTypes.NullableUnsignedByte.ToString());
            root.Element("NullableShort").Value.ShouldEqual(simpleTypes.NullableShort.ToString());
            root.Element("NullableUnsignedShort").Value.ShouldEqual(simpleTypes.NullableUnsignedShort.ToString());
            root.Element("NullableUnsignedInteger").Value.ShouldEqual(simpleTypes.NullableUnsignedInteger.ToString());
            root.Element("NullableLong").Value.ShouldEqual(simpleTypes.NullableLong.ToString());
            root.Element("NullableUnsignedLong").Value.ShouldEqual(simpleTypes.NullableUnsignedLong.ToString());
            root.Element("NullableFloat").Value.ShouldEqual(simpleTypes.NullableFloat.ToString());
            root.Element("NullableDouble").Value.ShouldEqual(simpleTypes.NullableDouble.ToString());
            root.Element("NullableDecimal").Value.ShouldEqual(simpleTypes.NullableDecimal.ToString());
            root.Element("NullableDateTime").Value.ShouldEqual(simpleTypes.NullableDateTime.ToString());
            root.Element("NullableTimeSpan").Value.ShouldEqual(simpleTypes.NullableTimeSpan.ToString());
            root.Element("NullableGuid").Value.ShouldEqual(simpleTypes.NullableGuid.ToString());
            root.Element("NullableEnum").Value.ShouldEqual(simpleTypes.NullableEnum.ToString());
        }
    }
}