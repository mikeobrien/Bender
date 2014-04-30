using System;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Json
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
        }

        [Test]
        public void should_deserialize_simple_types()
        {
            const string json =
                @"{
                      ""String"": ""hai"",
                      ""Char"": ""A"", ""NullableChar"": ""A"", 
                      ""Boolean"": true, ""NullableBoolean"": true,
                      ""Byte"": 1, ""NullableByte"": 1,
                      ""SignedByte"": 2, ""NullableSignedByte"": 2,
                      ""Short"": 3, ""NullableShort"": 3,
                      ""UnsignedShort"": 4, ""NullableUnsignedShort"": 4,
                      ""Integer"": 5, ""NullableInteger"": 5,
                      ""UnsignedInteger"": 6, ""NullableUnsignedInteger"": 6,
                      ""Long"": 7, ""NullableLong"": 7,
                      ""UnsignedLong"": 8, ""NullableUnsignedLong"": 8,
                      ""Float"": 1.1, ""NullableFloat"": 1.1,
                      ""Double"": 1.2, ""NullableDouble"": 1.2,
                      ""Decimal"": 1.3, ""NullableDecimal"": 1.3,
                      ""DateTime"": ""12/31/9999 11:59:59.9999999 PM"", ""NullableDateTime"": ""12/31/9999 11:59:59.9999999 PM"", 
                      ""TimeSpan"": ""10675199.02:48:05.4775807"", ""NullableTimeSpan"": ""10675199.02:48:05.4775807"", 
                      ""Guid"": ""00000000-0000-0000-0000-000000000000"", ""NullableGuid"": ""00000000-0000-0000-0000-000000000000"", 
                      ""Enum"": ""Value2"", ""NullableEnum"": ""Value2""
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<SimpleTypes>(json);
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
        public void should_deserialize_simple_empty_types_and_default_non_nullable_types()
        {
            // We dont do this anymore

//            const string json =
//                @"{
//                      ""String"": """",
//                      ""NullableChar"": """", 
//                      ""Boolean"": """", ""NullableBoolean"": """",
//                      ""Byte"": """", ""NullableByte"": """",
//                      ""SignedByte"": """", ""NullableSignedByte"": """",
//                      ""Short"": """", ""NullableShort"": """",
//                      ""UnsignedShort"": """", ""NullableUnsignedShort"": """",
//                      ""Integer"": """", ""NullableInteger"": """",
//                      ""UnsignedInteger"": """", ""NullableUnsignedInteger"": """",
//                      ""Long"": """", ""NullableLong"": """",
//                      ""UnsignedLong"": """", ""NullableUnsignedLong"": """",
//                      ""Float"": """", ""NullableFloat"": """",
//                      ""Double"": """", ""NullableDouble"": """",
//                      ""Decimal"": """", ""NullableDecimal"": """",
//                      ""DateTime"": """", ""NullableDateTime"": """", 
//                      ""TimeSpan"": """", ""NullableTimeSpan"": """", 
//                      ""Guid"": """", ""NullableGuid"": """", 
//                      ""Enum"": """", ""NullableEnum"": """"
//                }";
//            var result = Bender.Deserializer.Create().DeserializeJson<SimpleTypes>(json);
//            result.String.ShouldBeEmpty();
//            result.NullableChar.HasValue.ShouldBeFalse();
//            result.Boolean.ShouldBeFalse();
//            result.NullableBoolean.HasValue.ShouldBeFalse();
//            result.Byte.ShouldEqual<byte>(0);
//            result.NullableByte.HasValue.ShouldBeFalse();
//            result.SignedByte.ShouldEqual<sbyte>(0);
//            result.NullableSignedByte.HasValue.ShouldBeFalse();
//            result.Short.ShouldEqual<short>(0);
//            result.NullableShort.HasValue.ShouldBeFalse();
//            result.UnsignedShort.ShouldEqual<ushort>(0);
//            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
//            result.Integer.ShouldEqual(0);
//            result.NullableInteger.HasValue.ShouldBeFalse();
//            result.UnsignedInteger.ShouldEqual<uint>(0);
//            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
//            result.Long.ShouldEqual(0);
//            result.NullableLong.HasValue.ShouldBeFalse();
//            result.UnsignedLong.ShouldEqual<ulong>(0);
//            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
//            result.Float.ShouldEqual(0);
//            result.NullableFloat.HasValue.ShouldBeFalse();
//            result.Double.ShouldEqual(0);
//            result.NullableDouble.HasValue.ShouldBeFalse();
//            result.Decimal.ShouldEqual(0);
//            result.NullableDecimal.HasValue.ShouldBeFalse();
//            result.DateTime.ShouldEqual(DateTime.MinValue);
//            result.NullableDateTime.HasValue.ShouldBeFalse();
//            result.TimeSpan.ShouldEqual(TimeSpan.Zero);
//            result.NullableTimeSpan.HasValue.ShouldBeFalse();
//            result.Guid.ShouldEqual(Guid.Empty);
//            result.NullableGuid.HasValue.ShouldBeFalse();
//            result.Enum.ShouldEqual(Enum.Value1);
//            result.NullableEnum.HasValue.ShouldBeFalse();
        }

        [Test]
        public void should_deserialize_simple_nullable_empty_types()
        {
            // We dont do this anymore

//            const string json =
//                @"{
//                      ""String"": """",
//                      ""NullableChar"": """", 
//                      ""NullableBoolean"": """",
//                      ""NullableByte"": """",
//                      ""NullableSignedByte"": """",
//                      ""NullableShort"": """",
//                      ""NullableUnsignedShort"": """",
//                      ""NullableInteger"": """",
//                      ""NullableUnsignedInteger"": """",
//                      ""NullableLong"": """",
//                      ""NullableUnsignedLong"": """",
//                      ""NullableFloat"": """",
//                      ""NullableDouble"": """",
//                      ""NullableDecimal"": """",
//                      ""NullableDateTime"": """", 
//                      ""NullableTimeSpan"": """", 
//                      ""NullableGuid"": """", 
//                      ""NullableEnum"": """"
//                }";
//            var result = Bender.Deserializer.Create().DeserializeJson<SimpleTypes>(json);
//            result.String.ShouldBeEmpty();
//            result.NullableChar.HasValue.ShouldBeFalse();
//            result.NullableBoolean.HasValue.ShouldBeFalse();
//            result.NullableByte.HasValue.ShouldBeFalse();
//            result.NullableSignedByte.HasValue.ShouldBeFalse();
//            result.NullableShort.HasValue.ShouldBeFalse();
//            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
//            result.NullableInteger.HasValue.ShouldBeFalse();
//            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
//            result.NullableLong.HasValue.ShouldBeFalse();
//            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
//            result.NullableFloat.HasValue.ShouldBeFalse();
//            result.NullableDouble.HasValue.ShouldBeFalse();
//            result.NullableDecimal.HasValue.ShouldBeFalse();
//            result.NullableDateTime.HasValue.ShouldBeFalse();
//            result.NullableTimeSpan.HasValue.ShouldBeFalse();
//            result.NullableGuid.HasValue.ShouldBeFalse();
//            result.NullableEnum.HasValue.ShouldBeFalse();
        }

        [Test]
        public void should_deserialize_simple_null_types_and_default_non_nullable_types()
        {
            const string json =
                @"{
                      ""String"": null,
                      ""NullableChar"": null, 
                      ""NullableBoolean"": null,
                      ""NullableByte"": null,
                      ""NullableSignedByte"": null,
                      ""NullableShort"": null,
                      ""NullableUnsignedShort"": null,
                      ""NullableInteger"": null,
                      ""NullableUnsignedInteger"": null,
                      ""NullableLong"": null,
                      ""NullableUnsignedLong"": null,
                      ""NullableFloat"": null,
                      ""NullableDouble"": null,
                      ""NullableDecimal"": null,
                      ""NullableDateTime"": null, 
                      ""NullableTimeSpan"": null, 
                      ""NullableGuid"": null, 
                      ""NullableEnum"": null
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<SimpleTypes>(json);
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
        public void should_deserialize_simple_nullable_types()
        {
            const string json =
                @"{
                      ""String"": null,
                      ""NullableChar"": null, 
                      ""NullableBoolean"": null,
                      ""NullableByte"": null,
                      ""NullableSignedByte"": null,
                      ""NullableShort"": null,
                      ""NullableUnsignedShort"": null,
                      ""NullableInteger"": null,
                      ""NullableUnsignedInteger"": null,
                      ""NullableLong"": null,
                      ""NullableUnsignedLong"": null,
                      ""NullableFloat"": null,
                      ""NullableDouble"": null,
                      ""NullableDecimal"": null,
                      ""NullableDateTime"": null, 
                      ""NullableTimeSpan"": null, 
                      ""NullableGuid"": null, 
                      ""NullableEnum"": null
                }";
            var result = Bender.Deserializer.Create().DeserializeJson<SimpleTypes>(json);
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
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Char\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(char).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Char\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(char).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Char\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableChar\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(char).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Boolean\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(bool).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Boolean\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(bool).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Boolean\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableBoolean\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(bool).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Byte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(byte).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Byte\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(byte).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Byte\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(byte).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"SignedByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(sbyte).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"SignedByte\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(sbyte).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"SignedByte\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableSignedByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(sbyte).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Short\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(short).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Short\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(short).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Short\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(short).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ushort).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ushort).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ushort).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Integer\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(int).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Integer\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(int).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Integer\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(int).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(uint).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(uint).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(uint).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Long\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(long).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Long\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(long).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Long\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(long).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ulong).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ulong).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(ulong).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Float\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(float).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Float\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(float).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Float\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableFloat\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(float).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Double\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(double).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Double\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(double).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Double\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDouble\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(double).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Decimal\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(decimal).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Decimal\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(decimal).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Decimal\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDecimal\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(decimal).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"DateTime\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(DateTime).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"DateTime\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(DateTime).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"DateTime\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDateTime\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(DateTime).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"TimeSpan\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(TimeSpan).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"TimeSpan\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(TimeSpan).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"TimeSpan\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableTimeSpan\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(TimeSpan).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Guid\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(Guid).GetUnderlyingNullableType()].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Guid\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(Guid).GetUnderlyingNullableType()].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Guid\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableGuid\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(Guid).GetUnderlyingNullableType()].ToFormat("sfsfdds"));

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Enum\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(System.Enum)].ToFormat("sfsfdds"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Enum\": \"\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(System.Enum)].ToFormat(""));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Enum\": null }"));
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableEnum\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldContain(Options.Create().Deserialization.FriendlyParseErrorMessages[typeof(System.Enum)].ToFormat("sfsfdds"));
        }

        [Test]
        public void should_throw_value_parse_exception_with_custom_parse_message_when_failing_to_set_simple_type()
        {
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Char\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableChar\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Boolean\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableBoolean\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Byte\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"SignedByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableSignedByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Short\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Integer\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Long\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Float\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableFloat\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Double\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableDouble\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Decimal\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableDecimal\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"DateTime\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableDateTime\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"TimeSpan\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableTimeSpan\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Guid\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableGuid\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");

            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"Enum\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => x.Deserialization(y => y.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai")))
                .DeserializeJson<SimpleTypes>("{ \"NullableEnum\": \"sfsfdds\" }")).FriendlyMessage.ShouldContain("yada");
        }
    }
}
