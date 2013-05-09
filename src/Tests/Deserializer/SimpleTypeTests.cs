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
            var result = Bender.Deserializer.Create(x => x.IgnoreUnmatchedElements()).Deserialize<SimpleTypes>(xml);
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
            const string xml =
                @"<SimpleTypes>
                      <String></String>
                      <NullableChar></NullableChar>
                      <Boolean></Boolean><NullableBoolean></NullableBoolean>
                      <Byte></Byte><NullableByte></NullableByte>
                      <SignedByte></SignedByte><NullableSignedByte></NullableSignedByte>
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
                </SimpleTypes>";
            var result = Bender.Deserializer.Create(x => x.DefaultNonNullableTypesWhenEmpty()).Deserialize<SimpleTypes>(xml);
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
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Char>sfsfdds</Char></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Char' element as a char: Length not valid, must be one character.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableChar>sfsfdds</NullableChar></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableChar' element as a char: Length not valid, must be one character.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Boolean>sfsfdds</Boolean></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Boolean' element as a boolean: Not formatted correctly, must be 'true' or 'false'.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableBoolean>sfsfdds</NullableBoolean></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableBoolean' element as a boolean: Not formatted correctly, must be 'true' or 'false'.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Byte>sfsfdds</Byte></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Byte' element as a byte: Not formatted correctly, must be an integer between 0 and 255.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableByte>sfsfdds</NullableByte></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableByte' element as a byte: Not formatted correctly, must be an integer between 0 and 255.");
           
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><SignedByte>sfsfdds</SignedByte></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/SignedByte' element as a signedByte: Not formatted correctly, must be an integer between -128 and 127.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableSignedByte>sfsfdds</NullableSignedByte></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableSignedByte' element as a signedByte: Not formatted correctly, must be an integer between -128 and 127.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Short>sfsfdds</Short></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Short' element as a word: Not formatted correctly, must be an integer between -32,768 and 32,767.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableShort>sfsfdds</NullableShort></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableShort' element as a word: Not formatted correctly, must be an integer between -32,768 and 32,767.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><UnsignedShort>sfsfdds</UnsignedShort></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedShort' element as a usignedWord: Not formatted correctly, must be an integer between 0 and 65,535.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedShort>sfsfdds</NullableUnsignedShort></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedShort' element as a usignedWord: Not formatted correctly, must be an integer between 0 and 65,535.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Integer>sfsfdds</Integer></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Integer' element as a integer: Not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableInteger>sfsfdds</NullableInteger></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableInteger' element as a integer: Not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><UnsignedInteger>sfsfdds</UnsignedInteger></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedInteger' element as a usignedInteger: Not formatted correctly, must be an integer between 0 and 4,294,967,295.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedInteger>sfsfdds</NullableUnsignedInteger></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedInteger' element as a usignedInteger: Not formatted correctly, must be an integer between 0 and 4,294,967,295.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Long>sfsfdds</Long></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Long' element as a long: Not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableLong>sfsfdds</NullableLong></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableLong' element as a long: Not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><UnsignedLong>sfsfdds</UnsignedLong></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedLong' element as a usignedLong: Not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedLong>sfsfdds</NullableUnsignedLong></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedLong' element as a usignedLong: Not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Float>sfsfdds</Float></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Float' element as a singleFloat: Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableFloat>sfsfdds</NullableFloat></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableFloat' element as a singleFloat: Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Double>sfsfdds</Double></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Double' element as a doubleFloat: Not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableDouble>sfsfdds</NullableDouble></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDouble' element as a doubleFloat: Not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Decimal>sfsfdds</Decimal></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Decimal' element as a decimal: Not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableDecimal>sfsfdds</NullableDecimal></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDecimal' element as a decimal: Not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><DateTime>sfsfdds</DateTime></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/DateTime' element as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableDateTime>sfsfdds</NullableDateTime></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDateTime' element as a datetime: Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><TimeSpan>sfsfdds</TimeSpan></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/TimeSpan' element as a duration: Not formatted correctly, must be formatted as d.h:m:s.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableTimeSpan>sfsfdds</NullableTimeSpan></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableTimeSpan' element as a duration: Not formatted correctly, must be formatted as d.h:m:s.");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Guid>sfsfdds</Guid></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Guid' element as a guid: Not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableGuid>sfsfdds</NullableGuid></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableGuid' element as a guid: Not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
            
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><Enum>sfsfdds</Enum></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Enum' element as a enumeration: Not a valid option.");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().Deserialize<SimpleTypes>("<SimpleTypes><NullableEnum>sfsfdds</NullableEnum></SimpleTypes>"))
                .FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableEnum' element as a enumeration: Not a valid option.");
        }

        [Test]
        public void should_throw_value_parse_exception_with_custom_parse_message_when_failing_to_set_simple_type()
        {
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Char>sfsfdds</Char></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Char' element as a char: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<char>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableChar>sfsfdds</NullableChar></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableChar' element as a char: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Boolean>sfsfdds</Boolean></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Boolean' element as a boolean: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<bool>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableBoolean>sfsfdds</NullableBoolean></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableBoolean' element as a boolean: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Byte>sfsfdds</Byte></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Byte' element as a byte: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<byte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableByte>sfsfdds</NullableByte></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableByte' element as a byte: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><SignedByte>sfsfdds</SignedByte></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/SignedByte' element as a signedByte: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<sbyte>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableSignedByte>sfsfdds</NullableSignedByte></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableSignedByte' element as a signedByte: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Short>sfsfdds</Short></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Short' element as a word: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<short>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableShort>sfsfdds</NullableShort></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableShort' element as a word: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><UnsignedShort>sfsfdds</UnsignedShort></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedShort' element as a usignedWord: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ushort>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedShort>sfsfdds</NullableUnsignedShort></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedShort' element as a usignedWord: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Integer>sfsfdds</Integer></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Integer' element as a integer: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<int>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableInteger>sfsfdds</NullableInteger></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableInteger' element as a integer: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><UnsignedInteger>sfsfdds</UnsignedInteger></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedInteger' element as a usignedInteger: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<uint>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedInteger>sfsfdds</NullableUnsignedInteger></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedInteger' element as a usignedInteger: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Long>sfsfdds</Long></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Long' element as a long: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<long>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableLong>sfsfdds</NullableLong></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableLong' element as a long: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><UnsignedLong>sfsfdds</UnsignedLong></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/UnsignedLong' element as a usignedLong: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<ulong>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableUnsignedLong>sfsfdds</NullableUnsignedLong></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableUnsignedLong' element as a usignedLong: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Float>sfsfdds</Float></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Float' element as a singleFloat: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<float>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableFloat>sfsfdds</NullableFloat></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableFloat' element as a singleFloat: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Double>sfsfdds</Double></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Double' element as a doubleFloat: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<double>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableDouble>sfsfdds</NullableDouble></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDouble' element as a doubleFloat: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Decimal>sfsfdds</Decimal></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Decimal' element as a decimal: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<decimal>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableDecimal>sfsfdds</NullableDecimal></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDecimal' element as a decimal: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><DateTime>sfsfdds</DateTime></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/DateTime' element as a datetime: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<DateTime>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableDateTime>sfsfdds</NullableDateTime></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableDateTime' element as a datetime: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><TimeSpan>sfsfdds</TimeSpan></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/TimeSpan' element as a duration: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<TimeSpan>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableTimeSpan>sfsfdds</NullableTimeSpan></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableTimeSpan' element as a duration: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Guid>sfsfdds</Guid></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Guid' element as a guid: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<Guid>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableGuid>sfsfdds</NullableGuid></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableGuid' element as a guid: yada");

            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><Enum>sfsfdds</Enum></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/Enum' element as a enumeration: yada");
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create(x => x.WithFriendlyParseErrorMessage<System.Enum>("yada").WithFriendlyParseErrorMessage<string>("ohhai"))
                .Deserialize<SimpleTypes>("<SimpleTypes><NullableEnum>sfsfdds</NullableEnum></SimpleTypes>")).FriendlyMessage.ShouldEqual("Unable to parse the value 'sfsfdds' in the '/SimpleTypes/NullableEnum' element as a enumeration: yada");
        }
    }
}
