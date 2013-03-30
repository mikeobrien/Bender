using System;
using System.Xml.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class SimpleTypeTests
    {
        public enum Enum { Value1, Value2 }
        public class SimpleTypes
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
        public void should_deserialize_simple_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String>hai</String>
                      <Boolean>true</Boolean><NullableBoolean>true</NullableBoolean>
                      <Byte>1</Byte><NullableByte>1</NullableByte>
                      <ByteArray>AQID</ByteArray>
                      <UnsignedByte>2</UnsignedByte><NullableUnsignedByte>2</NullableUnsignedByte>
                      <Short>3</Short><NullableShort>3</NullableShort>
                      <UnsignedShort>4</UnsignedShort><NullableUnsignedShort>4</NullableUnsignedShort>
                      <Integer>5</Integer><NullableInteger>5</NullableInteger>
                      <UnsignedInteger>6</UnsignedInteger><NullableUnsignedInteger>6</NullableUnsignedInteger>
                      <Long>7</Long><NullableLong>7</NullableLong>
                      <UnsignedLong>8</UnsignedLong><NullableUnsignedLong>8</NullableUnsignedLong>
                      <Float>1.1</Float><NullableFloat>1.1</NullableFloat>
                      <Double>1.2</Double><NullableDouble>1.2</NullableDouble>
                      <Decimal>1.3</Decimal><NullableDecimal>1.3</NullableDecimal>
                      <DateTime>12/31/9999 11:59:59.9999999 PM</DateTime><NullableDateTime>12/31/9999 11:59:59.9999999 PM</NullableDateTime>
                      <TimeSpan>10675199.02:48:05.4775807</TimeSpan><NullableTimeSpan>10675199.02:48:05.4775807</NullableTimeSpan>
                      <Guid>00000000-0000-0000-0000-000000000000</Guid><NullableGuid>00000000-0000-0000-0000-000000000000</NullableGuid>
                      <Enum>Value2</Enum><NullableEnum>Value2</NullableEnum>
                      <Uri>http://www.google.com/</Uri>
                      <Object><Enum>Value2</Enum><Enum>Value2</Enum></Object>
                      <BoxedValue>00000000-0000-0000-0000-000000000000</BoxedValue>
                </SimpleTypes>";
            var result = Bender.Deserializer.Create(x => x.IgnoreUnmatchedElements()).Deserialize<SimpleTypes>(xml);
            result.String.ShouldEqual("hai");
            result.Boolean.ShouldBeTrue();
            result.NullableBoolean.Value.ShouldBeTrue();
            result.Byte.ShouldEqual<byte>(1);
            result.NullableByte.Value.ShouldEqual<byte>(1);
            result.ByteArray.ShouldEqual(new byte[] { 1, 2, 3 });
            result.UnsignedByte.ShouldEqual<sbyte>(2);
            result.NullableUnsignedByte.Value.ShouldEqual<sbyte>(2);
            result.Short.ShouldEqual<short>(3);
            result.NullableShort.Value.ShouldEqual<short>(3);
            result.UnsignedShort.ShouldEqual<ushort>(4);
            result.NullableUnsignedShort.Value.ShouldEqual<ushort>(4);
            result.Integer.ShouldEqual(5);
            result.NullableInteger.Value.ShouldEqual(5);
            result.UnsignedInteger.ShouldEqual<uint>(6);
            result.NullableUnsignedInteger.Value.ShouldEqual<uint>(6);
            result.Long.ShouldEqual(7);
            result.NullableLong.Value.ShouldEqual(7);
            result.UnsignedLong.ShouldEqual<ulong>(8);
            result.NullableUnsignedLong.Value.ShouldEqual<ulong>(8);
            result.Float.ShouldEqual(1.1F);
            result.NullableFloat.Value.ShouldEqual(1.1F);
            result.Double.ShouldEqual(1.2);
            result.NullableDouble.Value.ShouldEqual(1.2);
            result.Decimal.ShouldEqual(1.3M);
            result.NullableDecimal.Value.ShouldEqual(1.3M);
            result.DateTime.ShouldEqual(DateTime.MaxValue);
            result.NullableDateTime.Value.ShouldEqual(DateTime.MaxValue);
            result.TimeSpan.ShouldEqual(TimeSpan.MaxValue);
            result.NullableTimeSpan.Value.ShouldEqual(TimeSpan.MaxValue);
            result.Guid.ShouldEqual(Guid.Empty);
            result.NullableGuid.Value.ShouldEqual(Guid.Empty);
            result.Enum.ShouldEqual(Enum.Value2);
            result.NullableEnum.Value.ShouldEqual(Enum.Value2);
            result.Uri.ShouldEqual(new Uri("http://www.google.com"));
            result.Object.ShouldNotBeNull();
            result.Object.ShouldBeType<XElement>();
            result.BoxedValue.ShouldNotBeNull();
            result.BoxedValue.ShouldBeType<XElement>();
        }

        [Test]
        public void should_deserialize_simple_empty_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String></String>
                      <Boolean></Boolean><NullableBoolean></NullableBoolean>
                      <Byte></Byte><NullableByte></NullableByte>
                      <ByteArray></ByteArray>
                      <UnsignedByte></UnsignedByte><NullableUnsignedByte></NullableUnsignedByte>
                      <Short></Short><NullableShort></NullableShort>
                      <UnsignedShort></UnsignedShort><NullableUnsignedShort></NullableUnsignedShort>
                      <Integer></Integer><NullableInteger></NullableInteger>
                      <UnsignedInteger></UnsignedInteger><NullableUnsignedInteger></NullableUnsignedInteger>
                      <Long></Long><NullableLong></NullableLong>
                      <UnsignedLong></UnsignedLong><NullableUnsignedLong></NullableUnsignedLong>
                      <Float></Float><NullableFloat></NullableFloat>
                      <Double></Double><NullableDouble></NullableDouble>
                      <Decimal></Decimal><NullableDecimal></NullableDecimal>
                      <DateTime></DateTime><NullableDateTime></NullableDateTime>
                      <TimeSpan></TimeSpan><NullableTimeSpan></NullableTimeSpan>
                      <Guid></Guid><NullableGuid></NullableGuid>
                      <Enum></Enum><NullableEnum></NullableEnum>
                      <Uri></Uri>
                </SimpleTypes>";
            var result = Bender.Deserializer.Create(x => x.DefaultNonNullableTypesWhenEmpty()).Deserialize<SimpleTypes>(xml);
            result.String.ShouldBeEmpty();
            result.Boolean.ShouldBeFalse();
            result.NullableBoolean.HasValue.ShouldBeFalse();
            result.Byte.ShouldEqual<byte>(0);
            result.NullableByte.HasValue.ShouldBeFalse();
            result.ByteArray.ShouldEqual(new byte[] { });
            result.UnsignedByte.ShouldEqual<sbyte>(0);
            result.NullableUnsignedByte.HasValue.ShouldBeFalse();
            result.Short.ShouldEqual<short>(0);
            result.NullableShort.HasValue.ShouldBeFalse();
            result.UnsignedShort.ShouldEqual<ushort>(0);
            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
            result.Integer.ShouldEqual(0);
            result.NullableInteger.HasValue.ShouldBeFalse();
            result.UnsignedInteger.ShouldEqual<uint>(0);
            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
            result.Long.ShouldEqual(0);
            result.NullableLong.HasValue.ShouldBeFalse();
            result.UnsignedLong.ShouldEqual<ulong>(0);
            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
            result.Float.ShouldEqual(0);
            result.NullableFloat.HasValue.ShouldBeFalse();
            result.Double.ShouldEqual(0);
            result.NullableDouble.HasValue.ShouldBeFalse();
            result.Decimal.ShouldEqual(0);
            result.NullableDecimal.HasValue.ShouldBeFalse();
            result.DateTime.ShouldEqual(DateTime.MinValue);
            result.NullableDateTime.HasValue.ShouldBeFalse();
            result.TimeSpan.ShouldEqual(TimeSpan.MinValue);
            result.NullableTimeSpan.HasValue.ShouldBeFalse();
            result.Guid.ShouldEqual(Guid.Empty);
            result.NullableGuid.HasValue.ShouldBeFalse();
            result.Enum.ShouldEqual(Enum.Value1);
            result.NullableEnum.HasValue.ShouldBeFalse();
            result.Uri.ShouldEqual(new Uri("http://tempuri.org/"));
        }

        [Test]
        public void should_throw_set_value_exception_when_failing_to_set_simple_type()
        {
            const string xml = @"<SimpleTypes Integer=""sdafasdf""/>";
            Assert.Throws<SetValueException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>(xml));
        }
    }
}
