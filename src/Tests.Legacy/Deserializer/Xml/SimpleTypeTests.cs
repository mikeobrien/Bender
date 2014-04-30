using System;
using System.Xml.Linq;
using Bender;
using Bender.Nodes;
using Bender.Nodes.Object;
using NUnit;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Xml
{
    [TestFixture]
    public class SimpleTypeTests
    {
        public enum Enum { Value1, Value2 }
        public class SimpleTypes
        {
            public string String { get; set; }
            public char Char { get; set; } public char? NullableChar { get; set; }
            public bool Boolean { get; set; } public bool? NullableBoolean { get; set; }
            public byte Byte { get; set; } public byte? NullableByte { get; set; }
            public sbyte SignedByte { get; set; } public sbyte? NullableSignedByte { get; set; }
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
            public object Object { get; set; }
            public object BoxedValue { get; set; }
        }

        [Test]
        public void should_deserialize_simple_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String>hai</String>
                      <Char>A</Char><NullableChar>A</NullableChar>
                      <Boolean>true</Boolean><NullableBoolean>true</NullableBoolean>
                      <Byte>1</Byte><NullableByte>1</NullableByte>
                      <SignedByte>2</SignedByte><NullableSignedByte>2</NullableSignedByte>
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
                      <Object><Enum>Value2</Enum><Enum>Value2</Enum></Object>
                      <BoxedValue>00000000-0000-0000-0000-000000000000</BoxedValue>
                </SimpleTypes>";
            var result = Bender.Deserializer.Create().DeserializeXml<SimpleTypes>(xml);
            result.String.ShouldEqual("hai");
            result.Char.ShouldEqual('A');
            result.NullableChar.Value.ShouldEqual('A');
            result.Boolean.ShouldBeTrue();
            result.NullableBoolean.Value.ShouldBeTrue();
            result.Byte.ShouldEqual<byte>(1);
            result.NullableByte.Value.ShouldEqual<byte>(1);
            result.SignedByte.ShouldEqual<sbyte>(2);
            result.NullableSignedByte.Value.ShouldEqual<sbyte>(2);
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
        }

        [Test]
        public void should_deserialize_simple_empty_elements_and_default_non_nullable_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String/>
                      <NullableChar/>
                      <NullableBoolean/>
                      <NullableByte/>
                      <NullableSignedByte/>
                      <NullableShort/>
                      <NullableUnsignedShort/>
                      <NullableInteger/>
                      <NullableUnsignedInteger/>
                      <NullableLong/>
                      <NullableUnsignedLong/>
                      <NullableFloat/>
                      <NullableDouble/>
                      <NullableDecimal/>
                      <NullableDateTime/>
                      <NullableTimeSpan/>
                      <NullableGuid/>
                      <NullableEnum/>
                </SimpleTypes>";
            var result = Bender.Deserializer.Create().DeserializeXml<SimpleTypes>(xml);
            result.String.ShouldBeNull();
            result.NullableChar.HasValue.ShouldBeFalse();
            result.NullableBoolean.HasValue.ShouldBeFalse();
            result.NullableByte.HasValue.ShouldBeFalse();
            result.NullableSignedByte.HasValue.ShouldBeFalse();
            result.NullableShort.HasValue.ShouldBeFalse();
            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
            result.NullableInteger.HasValue.ShouldBeFalse();
            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
            result.NullableLong.HasValue.ShouldBeFalse();
            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
            result.NullableFloat.HasValue.ShouldBeFalse();
            result.NullableDouble.HasValue.ShouldBeFalse();
            result.NullableDecimal.HasValue.ShouldBeFalse();
            result.NullableDateTime.HasValue.ShouldBeFalse();
            result.NullableTimeSpan.HasValue.ShouldBeFalse();
            result.NullableGuid.HasValue.ShouldBeFalse();
            result.NullableEnum.HasValue.ShouldBeFalse();
        }

        [Test]
        public void should_deserialize_simple_nullable_empty_elements()
        {
            const string xml =
                @"<SimpleTypes>
                      <String/>
                      <NullableChar/>
                      <NullableBoolean/>
                      <NullableByte/>
                      <NullableSignedByte/>
                      <NullableShort/>
                      <NullableUnsignedShort/>
                      <NullableInteger/>
                      <NullableUnsignedInteger/>
                      <NullableLong/>
                      <NullableUnsignedLong/>
                      <NullableFloat/>
                      <NullableDouble/>
                      <NullableDecimal/>
                      <NullableDateTime/>
                      <NullableTimeSpan/>
                      <NullableGuid/>
                      <NullableEnum/>
                </SimpleTypes>";
            var result = Bender.Deserializer.Create().DeserializeXml<SimpleTypes>(xml);
            result.String.ShouldBeNull();
            result.NullableChar.HasValue.ShouldBeFalse();
            result.NullableBoolean.HasValue.ShouldBeFalse();
            result.NullableByte.HasValue.ShouldBeFalse();
            result.NullableSignedByte.HasValue.ShouldBeFalse();
            result.NullableShort.HasValue.ShouldBeFalse();
            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
            result.NullableInteger.HasValue.ShouldBeFalse();
            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
            result.NullableLong.HasValue.ShouldBeFalse();
            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
            result.NullableFloat.HasValue.ShouldBeFalse();
            result.NullableDouble.HasValue.ShouldBeFalse();
            result.NullableDecimal.HasValue.ShouldBeFalse();
            result.NullableDateTime.HasValue.ShouldBeFalse();
            result.NullableTimeSpan.HasValue.ShouldBeFalse();
            result.NullableGuid.HasValue.ShouldBeFalse();
            result.NullableEnum.HasValue.ShouldBeFalse();
        }

        [Test]
        public void should_throw_value_parse_exception_when_failing_to_set_simple_type()
        {
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Char>sfsfdds</Char></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Char></Char></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Char/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableChar>sfsfdds</NullableChar></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Boolean>sfsfdds</Boolean></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Boolean></Boolean></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Boolean/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableBoolean>sfsfdds</NullableBoolean></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Byte>sfsfdds</Byte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Byte></Byte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Byte/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableByte>sfsfdds</NullableByte></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><SignedByte>sfsfdds</SignedByte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><SignedByte></SignedByte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><SignedByte/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableSignedByte>sfsfdds</NullableSignedByte></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Short>sfsfdds</Short></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Short></Short></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Short/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableShort>sfsfdds</NullableShort></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedShort>sfsfdds</UnsignedShort></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedShort></UnsignedShort></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedShort/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedShort>sfsfdds</NullableUnsignedShort></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Integer>sfsfdds</Integer></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Integer></Integer></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Integer/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableInteger>sfsfdds</NullableInteger></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedInteger>sfsfdds</UnsignedInteger></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedInteger></UnsignedInteger></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedInteger/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedInteger>sfsfdds</NullableUnsignedInteger></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Long>sfsfdds</Long></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Long></Long></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Long/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableLong>sfsfdds</NullableLong></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedLong>sfsfdds</UnsignedLong></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedLong></UnsignedLong></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedLong/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedLong>sfsfdds</NullableUnsignedLong></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Float>sfsfdds</Float></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Float></Float></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Float/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableFloat>sfsfdds</NullableFloat></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Double>sfsfdds</Double></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Double></Double></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Double/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDouble>sfsfdds</NullableDouble></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Decimal>sfsfdds</Decimal></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Decimal></Decimal></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Decimal/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDecimal>sfsfdds</NullableDecimal></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><DateTime>sfsfdds</DateTime></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><DateTime></DateTime></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><DateTime/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDateTime>sfsfdds</NullableDateTime></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><TimeSpan>sfsfdds</TimeSpan></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><TimeSpan></TimeSpan></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><TimeSpan/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableTimeSpan>sfsfdds</NullableTimeSpan></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Guid>sfsfdds</Guid></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Guid></Guid></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Guid/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableGuid>sfsfdds</NullableGuid></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Enum>sfsfdds</Enum></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Enum></Enum></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><Enum/></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeXml<SimpleTypes>("<SimpleTypes><NullableEnum>sfsfdds</NullableEnum></SimpleTypes>"));
        }

        [Test]
        public void should_throw_value_parse_exception_with_custom_parse_message_when_failing_to_set_simple_type()
        {
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Char>sfsfdds</Char></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableChar>sfsfdds</NullableChar></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Boolean>sfsfdds</Boolean></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableBoolean>sfsfdds</NullableBoolean></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Byte>sfsfdds</Byte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableByte>sfsfdds</NullableByte></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><SignedByte>sfsfdds</SignedByte></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableSignedByte>sfsfdds</NullableSignedByte></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Short>sfsfdds</Short></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableShort>sfsfdds</NullableShort></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedShort>sfsfdds</UnsignedShort></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedShort>sfsfdds</NullableUnsignedShort></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Integer>sfsfdds</Integer></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableInteger>sfsfdds</NullableInteger></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedInteger>sfsfdds</UnsignedInteger></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedInteger>sfsfdds</NullableUnsignedInteger></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Long>sfsfdds</Long></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableLong>sfsfdds</NullableLong></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><UnsignedLong>sfsfdds</UnsignedLong></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableUnsignedLong>sfsfdds</NullableUnsignedLong></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Float>sfsfdds</Float></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableFloat>sfsfdds</NullableFloat></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Double>sfsfdds</Double></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDouble>sfsfdds</NullableDouble></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Decimal>sfsfdds</Decimal></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDecimal>sfsfdds</NullableDecimal></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><DateTime>sfsfdds</DateTime></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableDateTime>sfsfdds</NullableDateTime></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><TimeSpan>sfsfdds</TimeSpan></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableTimeSpan>sfsfdds</NullableTimeSpan></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Guid>sfsfdds</Guid></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableGuid>sfsfdds</NullableGuid></SimpleTypes>"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><Enum>sfsfdds</Enum></SimpleTypes>"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeXml<SimpleTypes>("<SimpleTypes><NullableEnum>sfsfdds</NullableEnum></SimpleTypes>"));
        }
    }
}
