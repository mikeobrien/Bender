﻿using System;
using System.Collections.Generic;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using Flexo.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ValueNodeTests
    {
        public static Context CreateContext(Mode mode, Options options = null)
        {
            return new Context(options ?? Options.Create(), mode, "xml");
        }

        [Test]
        public void should_be_of_node_type_value()
        {
            new ValueNode(CreateContext(Mode.Serialize), null, new SimpleValue(null, null), null, null)
                .NodeType.ShouldEqual(NodeType.Value);
        }

        [Test]
        public void should_be_of_type_value()
        {
            new ValueNode(CreateContext(Mode.Serialize), null, new SimpleValue(null, null), null, null)
                .Type.ShouldEqual("value");
        }

        [Test]
        public void should_get_the_name()
        {
            new ValueNode(CreateContext(Mode.Serialize), "hai", new SimpleValue(null, null), null, null)
                .Name.ShouldEqual("hai");
        }

        [Test]
        public void should_return_the_value_as_the_object()
        {
            new ValueNode(CreateContext(Mode.Serialize), null, new SimpleValue("hai", 
                typeof(string).ToCachedType()), null, null)
                    .Value.ShouldEqual("hai");
        }

        [Test]
        public void should_get_the_value()
        {
            new ValueNode(CreateContext(Mode.Serialize), null, 
                new SimpleValue("hai", typeof(string).ToCachedType()), null, null)
                    .Value.ShouldEqual("hai");
        }

        public static readonly object[] SimpleTypes = TestCases.Create()
            .AddType<string>("1")
            .AddType<Uri>(new Uri("http://www.xkcd.com"))

            .AddType<UriFormat>(UriFormat.SafeUnescaped).AddType<UriFormat?>(UriFormat.SafeUnescaped)

            .AddType<DateTime>(DateTime.Today).AddType<DateTime?>(DateTime.Today)
            .AddType<TimeSpan>(TimeSpan.MaxValue).AddType<TimeSpan?>(TimeSpan.MaxValue)
            .AddType<Guid>(Guid.Empty).AddType<Guid?>(Guid.Empty)

            .AddType<Boolean>(true).AddType<Boolean?>(true)
            .AddType<Byte>(5).AddType<Byte?>(55)
            .AddType<SByte>(6).AddType<SByte?>(66)
            .AddType<Int16>(7).AddType<Int16?>(77)
            .AddType<UInt16>(8).AddType<UInt16?>(88)
            .AddType<Int32>(9).AddType<Int32?>(99)
            .AddType<UInt32>(10).AddType<UInt32?>(110)
            .AddType<Int64>(11).AddType<Int64?>(111)
            .AddType<UInt64>(12).AddType<UInt64?>(120)
            .AddType<IntPtr>(new IntPtr(13)).AddType<IntPtr?>(new IntPtr(130))
            .AddType<UIntPtr>(new UIntPtr(14)).AddType<UIntPtr?>(new UIntPtr(140))
            .AddType<Char>('a').AddType<Char?>('b')
            .AddType<Double>(15).AddType<Double?>(150)
            .AddType<Single>(16).AddType<Single?>(160)
            .AddType<Decimal>(17).AddType<Decimal?>(170)

            .All;

        [Test]
        [TestCaseSource(nameof(SimpleTypes))]
        public void should_set_simple_types(Type type, object value)
        {
            var result = new SimpleValue(type.ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = value;
            result.Instance.ShouldEqual(value);
        }

        [Test]
        [TestCase((sbyte)3)]
        [TestCase((byte)3)]
        [TestCase((short)3)]
        [TestCase((ushort)3)]
        [TestCase((int)3)]
        [TestCase((uint)3)]
        [TestCase((long)3)]
        [TestCase((ulong)3)]
        [TestCase((float)3)]
        [TestCase((double)3)]
        public void should_set_numeric_enum_type(object value)
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = value;
            result.Instance.ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        public void should_set_decimal_enum_type()
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = 3.0;
            result.Instance.ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        [TestCaseSource(nameof(SimpleTypes))]
        public void should_parse_simple_types_from_string(Type type, object value)
        {
            var result = new SimpleValue(type.ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = value.ToString();
            result.Instance.ShouldEqual(value);
        }

        [Test]
        [TestCaseSource(nameof(SimpleTypes))]
        public void should_fail_to_parse_invalid_string_values_and_return_friendly_message(Type type, object value)
        {
            if (type.Is<string>()) return;
            var messageType = type.GetUnderlyingNullableType();
            messageType = messageType.IsEnum ? typeof(Enum) : messageType;
            Assert.Throws<ValueParseException>(() => new ValueNode(CreateContext(Mode.Deserialize), 
                    null, new SimpleValue(type.ToCachedType()), null, null).Value = "yada")
                .FriendlyMessage.ShouldEqual(Options.Create().Deserialization
                    .FriendlyParseErrorMessages[messageType].ToFormat("yada"));
        }

        [Test]
        public void should_fail_to_convert_incompatible_types()
        {
            Assert.Throws<ValueConversionException>(() =>
                new ValueNode(CreateContext(Mode.Deserialize), null, 
                    new SimpleValue(typeof(KeyValuePair<string, int>)
                        .ToCachedType()), null, null).Value = new KeyValuePair<int, string>())
            .Message.ShouldEqual("Value '[0, ]' of type 'System.Collections.Generic.KeyValuePair<System.Int32, System.String>' " +
                "cannot be converted to type 'System.Collections.Generic.KeyValuePair<System.String, System.Int32>': " +
                "Object must implement IConvertible.");
        }

        [Test]
        [TestCaseSource(nameof(SimpleTypes))]
        public void should_convert_simple_types_to_string(Type type, object value)
        {
            var result = new SimpleValue(typeof(string).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = value;
            result.Instance.ShouldEqual(value.ToString());
        }

        public static readonly object[] SimpleNumericTypes = TestCases.Create()
            .AddType<Boolean>(true).AddType<Boolean?>(true)
            .AddType<Byte>(5).AddType<Byte?>(55)
            .AddType<SByte>(6).AddType<SByte?>(66)
            .AddType<Int16>(7).AddType<Int16?>(77)
            .AddType<UInt16>(8).AddType<UInt16?>(88)
            .AddType<Int32>(9).AddType<Int32?>(99)
            .AddType<UInt32>(10).AddType<UInt32?>(110)
            .AddType<Int64>(11).AddType<Int64?>(111)
            .AddType<UInt64>(12).AddType<UInt64?>(120)
            .AddType<Double>(15).AddType<Double?>(150)
            .AddType<Single>(16).AddType<Single?>(160)
            .AddType<Decimal>(17).AddType<Decimal?>(170)

            .All;

        [Test]
        [TestCaseSource(nameof(SimpleNumericTypes))]
        public void should_convert_simple_numeric_types_to_decimal(Type type, object value)
        {
            var result = new SimpleValue(typeof(decimal).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result, null, null).Value = value;
            result.Instance.ShouldEqual(Convert.ChangeType(value, typeof(decimal)));
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Uri))]
        [TestCase(typeof(UriFormat?))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid?))]
        [TestCase(typeof(Boolean?))]
        [TestCase(typeof(Byte?))]
        [TestCase(typeof(SByte?))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64?))]
        [TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(Char?))]
        [TestCase(typeof(Double?))]
        [TestCase(typeof(Single?))]
        [TestCase(typeof(Decimal?))]
        public void should_set_null_on_reference_types(Type type)
        {
            var value = new SimpleValue(typeof(string).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, value, null, null).Value = null;
            value.Instance.ShouldBeNull();
        }

        [Test]
        [TestCase(typeof(UriFormat?))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid?))]
        [TestCase(typeof(Boolean?))]
        [TestCase(typeof(Byte?))]
        [TestCase(typeof(SByte?))]
        [TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64?))]
        [TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(Char?))]
        [TestCase(typeof(Double?))]
        [TestCase(typeof(Single?))]
        [TestCase(typeof(Decimal?))]
        public void should_set_null_on_nullable_types_when_empty_string_is_passed(Type type)
        {
            var value = new SimpleValue(type.ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, 
                value, null, null).Value = "";
            value.Instance.ShouldBeNull();
        }

        [Test]            
        [TestCase(typeof(UriFormat))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(Boolean))]
        [TestCase(typeof(Byte))]
        [TestCase(typeof(SByte))]
        [TestCase(typeof(Int16))]
        [TestCase(typeof(UInt16))]
        [TestCase(typeof(Int32))]
        [TestCase(typeof(UInt32))]
        [TestCase(typeof(Int64))]
        [TestCase(typeof(UInt64))]
        [TestCase(typeof(IntPtr))]
        [TestCase(typeof(UIntPtr))]
        [TestCase(typeof(Char))]
        [TestCase(typeof(Double))]
        [TestCase(typeof(Single))]
        [TestCase(typeof(Decimal))]
        public void should_fail_to_set_null_on_value_types(Type type)
        {
            Assert.Throws<ValueCannotBeNullDeserializationException>(() =>
                new ValueNode(CreateContext(Mode.Deserialize), null,
                    new SimpleValue(type.ToCachedType()), null, null).Value = null);
        }

        [Test]
        public void should_ignore_nulls_for_value_types_when_configured()
        {
            var options = Options.Create(x => x.Deserialization(
                d => d.IgnoreNullsForValueTypes()));
            var value = new SimpleValue(typeof(Guid).ToCachedType());
            value.Instance = Guid.Empty;

            new ValueNode(CreateContext(Mode.Deserialize, options), 
                null, value, null, null).Value = null;

            value.Instance.ShouldEqual(Guid.Empty);
        }

        [Test]
        public void should_set_enum_with_custom_naming_convention()
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize, Options.Create(
                x => x.UseEnumSnakeCaseNaming())), null, result, 
                null, null).Value = "Safe_Unescaped";
            result.Instance.ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        public void should_get_enum_with_custom_naming_convention()
        {
            var result = new SimpleValue(UriFormat.SafeUnescaped, 
                typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Serialize, Options.Create(
                x => x.UseEnumSnakeCaseNaming())), null, result,
                null, null).Value.ShouldEqual("Safe_Unescaped");
        }

        [Test]
        public void should_fail_to_set_enum_value_case_sensitively_when_configured()
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            Assert.Throws<ValueParseException> (() => new ValueNode(CreateContext(Mode.Deserialize, 
                Options.Create(x => x.Deserialization(y => y.WithCaseSensitiveEnumValues()))), 
                    null, result, null, null).Value = "safeunescaped")
                .FriendlyMessage.ShouldEqual("Option 'safeunescaped' is not valid.");
        }

        [Test]
        public void should_set_enum_value_case_insensitively_when_not_configured()
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), null, result,
                null, null).Value = "safeunescaped";
            result.Instance.ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        public void should_get_enum_numeric_value()
        {
            var result = new SimpleValue(UriFormat.SafeUnescaped,
                typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Serialize, Options.Create(x => 
                x.Serialization(y => y.EnumValuesAsNumeric()))), null, result,
                null, null).Value.ShouldEqual(3);
        }

        [Test]
        public void should_set_enum_numeric_value_when_configured(
            [Values(3, "3")] object enumValue)
        {
            var result = new SimpleValue(typeof(UriFormat).ToCachedType());
            new ValueNode(CreateContext(Mode.Deserialize), 
                null, result, null, null).Value = enumValue;
            result.Instance.ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        public void should_return_raw_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity,
                double.NaN, double.NegativeInfinity, double.PositiveInfinity)] object value)
        {
            var result = new SimpleValue(value, value.GetType().ToCachedType());
            new ValueNode(CreateContext(Mode.Serialize), null, result,
                null, null).Value.ShouldEqual(value);
        }

        [Test]
        public void should_return_name_of_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity,
                double.NaN, double.NegativeInfinity, double.PositiveInfinity)] object value)
        {
            var result = new SimpleValue(value, value.GetType().ToCachedType());
            new ValueNode(CreateContext(Mode.Serialize, Options.Create(
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsName()))), null, result,
                null, null).Value.ShouldEqual(value.ToString());
        }

        [Test]
        public void should_return_zero_non_numeric_float_when_configured(
            [Values(float.NaN, float.NegativeInfinity, float.PositiveInfinity,
                double.NaN, double.NegativeInfinity, double.PositiveInfinity)] object value)
        {
            var result = new SimpleValue(value, value.GetType().ToCachedType());
            new ValueNode(CreateContext(Mode.Serialize, Options.Create(
                    x => x.Serialization(s => s.WriteNonNumericFloatsAsZero()))), null, result,
                null, null).Value.ShouldEqual(0);
        }
    }
}
