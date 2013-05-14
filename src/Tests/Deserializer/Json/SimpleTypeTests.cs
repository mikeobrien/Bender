using System;
using System.Xml.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
                      ""Enum"": ""Value2"", ""NullableEnum"": ""Value2"",
                      ""Object"": { },
                      ""BoxedValue"": ""00000000-0000-0000-0000-000000000000""
                }";
            var result = Bender.Deserializer.Create(x => x.IgnoreUnmatchedNodes()).DeserializeJson<SimpleTypes>(json);
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
            result.Object.ShouldNotBeNull();
            result.Object.ShouldBeType<XElement>();
            result.BoxedValue.ShouldNotBeNull();
            result.BoxedValue.ShouldBeType<XElement>();
        }

        [Test]
        public void should_deserialize_simple_empty_types()
        {
            const string json =
                @"{
                      ""String"": """",
                      ""NullableChar"": """", 
                      ""Boolean"": """", ""NullableBoolean"": """",
                      ""Byte"": """", ""NullableByte"": """",
                      ""SignedByte"": """", ""NullableSignedByte"": """",
                      ""Short"": """", ""NullableShort"": """",
                      ""UnsignedShort"": """", ""NullableUnsignedShort"": """",
                      ""Integer"": """", ""NullableInteger"": """",
                      ""UnsignedInteger"": """", ""NullableUnsignedInteger"": """",
                      ""Long"": """", ""NullableLong"": """",
                      ""UnsignedLong"": """", ""NullableUnsignedLong"": """",
                      ""Float"": """", ""NullableFloat"": """",
                      ""Double"": """", ""NullableDouble"": """",
                      ""Decimal"": """", ""NullableDecimal"": """",
                      ""DateTime"": """", ""NullableDateTime"": """", 
                      ""TimeSpan"": """", ""NullableTimeSpan"": """", 
                      ""Guid"": """", ""NullableGuid"": """", 
                      ""Enum"": """", ""NullableEnum"": """"
                }";
            var result = Bender.Deserializer.Create(x => x.DefaultNonNullableTypesWhenEmpty()).DeserializeJson<SimpleTypes>(json);
            result.String.ShouldBeEmpty();
            result.NullableChar.HasValue.ShouldBeFalse();
            result.Boolean.ShouldBeFalse();
            result.NullableBoolean.HasValue.ShouldBeFalse();
            result.Byte.ShouldEqual<byte>(0);
            result.NullableByte.HasValue.ShouldBeFalse();
            result.SignedByte.ShouldEqual<sbyte>(0);
            result.NullableSignedByte.HasValue.ShouldBeFalse();
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
        }

        [Test]
        public void should_throw_value_parse_exception_when_failing_to_set_simple_type()
        {
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Char\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Char' field as a char: Length not valid, must be one character.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableChar\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableChar' field as a char: Length not valid, must be one character.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Boolean\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Boolean' field as a boolean: Not formatted correctly, must be 'true' or 'false'.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableBoolean\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableBoolean' field as a boolean: Not formatted correctly, must be 'true' or 'false'.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Byte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Byte' field as a byte: Not formatted correctly, must be an integer between 0 and 255.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableByte' field as a byte: Not formatted correctly, must be an integer between 0 and 255.");
           
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"SignedByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'SignedByte' field as a signedByte: Not formatted correctly, must be an integer between -128 and 127.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableSignedByte\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableSignedByte' field as a signedByte: Not formatted correctly, must be an integer between -128 and 127.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Short\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Short' field as a word: Not formatted correctly, must be an integer between -32,768 and 32,767.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableShort' field as a word: Not formatted correctly, must be an integer between -32,768 and 32,767.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedShort' field as a usignedWord: Not formatted correctly, must be an integer between 0 and 65,535.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedShort\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedShort' field as a usignedWord: Not formatted correctly, must be an integer between 0 and 65,535.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Integer\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Integer' field as a integer: Not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableInteger' field as a integer: Not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedInteger' field as a usignedInteger: Not formatted correctly, must be an integer between 0 and 4,294,967,295.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedInteger\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedInteger' field as a usignedInteger: Not formatted correctly, must be an integer between 0 and 4,294,967,295.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Long\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Long' field as a long: Not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableLong' field as a long: Not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedLong' field as a usignedLong: Not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableUnsignedLong\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedLong' field as a usignedLong: Not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Float\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Float' field as a singleFloat: Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableFloat\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableFloat' field as a singleFloat: Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Double\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Double' field as a doubleFloat: Not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDouble\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDouble' field as a doubleFloat: Not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Decimal\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Decimal' field as a decimal: Not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDecimal\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDecimal' field as a decimal: Not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"DateTime\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'DateTime' field as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableDateTime\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDateTime' field as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"TimeSpan\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'TimeSpan' field as a duration: Not formatted correctly, must be formatted as d.h:m:s.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableTimeSpan\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableTimeSpan' field as a duration: Not formatted correctly, must be formatted as d.h:m:s.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Guid\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Guid' field as a guid: Not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableGuid\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableGuid' field as a guid: Not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"Enum\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Enum' field as a enumeration: Not a valid option.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<SimpleTypes>("{ \"NullableEnum\": \"sfsfdds\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableEnum' field as a enumeration: Not a valid option.");
        }

        [Test]
        public void should_throw_value_parse_exception_with_custom_parse_message_when_failing_to_set_simple_type()
        {
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Char\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Char' field as a char: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableChar\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableChar' field as a char: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Boolean\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Boolean' field as a boolean: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableBoolean\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableBoolean' field as a boolean: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Byte\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Byte' field as a byte: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableByte' field as a byte: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"SignedByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'SignedByte' field as a signedByte: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableSignedByte\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableSignedByte' field as a signedByte: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Short\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Short' field as a word: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableShort' field as a word: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedShort' field as a usignedWord: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedShort\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedShort' field as a usignedWord: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Integer\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Integer' field as a integer: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableInteger' field as a integer: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedInteger' field as a usignedInteger: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedInteger\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedInteger' field as a usignedInteger: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Long\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Long' field as a long: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableLong' field as a long: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"UnsignedLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'UnsignedLong' field as a usignedLong: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableUnsignedLong\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableUnsignedLong' field as a usignedLong: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Float\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Float' field as a singleFloat: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableFloat\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableFloat' field as a singleFloat: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Double\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Double' field as a doubleFloat: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableDouble\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDouble' field as a doubleFloat: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Decimal\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Decimal' field as a decimal: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableDecimal\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDecimal' field as a decimal: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"DateTime\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'DateTime' field as a datetime: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableDateTime\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableDateTime' field as a datetime: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"TimeSpan\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'TimeSpan' field as a duration: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableTimeSpan\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableTimeSpan' field as a duration: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Guid\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Guid' field as a guid: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableGuid\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableGuid' field as a guid: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"Enum\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'Enum' field as a enumeration: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .DeserializeJson<SimpleTypes>("{ \"NullableEnum\": \"sfsfdds\" }")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the 'NullableEnum' field as a enumeration: yada");
        }
    }
}
