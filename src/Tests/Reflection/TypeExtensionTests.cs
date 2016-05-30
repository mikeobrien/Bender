using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Reflection
{
    [TestFixture]
    public class TypeExtensionTests
    {
        [Test]
        [TestCase(typeof(Dictionary<string, KeyValuePair<int, string>>), "Dictionary")]
        [TestCase(typeof(string), "String")]
        [TestCase(typeof(int?), "Nullable")]
        public void should_get_generic_type_base_name(Type type, string name)
        {
            type.GetGenericTypeBaseName().ShouldEqual(name);
        }

        [Test]
        [TestCase(typeof(Dictionary<string, KeyValuePair<int, string>>), "System.Collections.Generic.Dictionary")]
        [TestCase(typeof(string), "System.String")]
        [TestCase(typeof(int?), "System.Nullable")]
        public void should_get_generic_type_base_full_name(Type type, string name)
        {
            type.GetGenericTypeBaseFullName().ShouldEqual(name);
        }

        [Test]
        [TestCase(typeof(Dictionary<string, KeyValuePair<int, string>>), 
            "System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.KeyValuePair<System.Int32, System.String>>")]
        [TestCase(typeof(string), "System.String")]
        [TestCase(typeof(KeyValuePair<int?, string>?), "System.Collections.Generic.KeyValuePair<System.Int32?, System.String>?")]
        public void should_get_friendly_full_type_name(Type type, string name)
        {
            type.GetFriendlyTypeFullName().ShouldEqual(name);
        }

        [Test]
        public void should_indicate_if_type_implements_interface()
        {
            typeof(Stream).Implements<IDisposable>().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_type_implements_open_generic_interface()
        {
            typeof(GenericStringEnumerableImpl).Implements(typeof(IEnumerable<>)).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_type_does_not__implement_interface()
        {
            typeof(object).Implements<IDisposable>().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(int), TypeCode.Int32)]
        [TestCase(typeof(int?), TypeCode.Int32)]
        [TestCase(typeof(string), TypeCode.String)]
        public void should_get_type_code_for_nullable_and_non_types(Type type, TypeCode typeCode)
        {
            type.GetTypeCode().ShouldEqual(typeCode);
        }

        [Test]
        public void should_indicate_if_a_type_is_the_specified_type()
        {
            typeof(int).Is<int>().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(List<string>))]
        public void should_indicate_if_a_type_is_or_implements_the_specified_type(Type type)
        {
            type.IsOrImplements<IList<string>>().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_and_does_not_implement_the_specified_type()
        {
            typeof(ArrayList).IsOrImplements<IList<string>>().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(GenericStringListImpl))]
        [TestCase(typeof(InheritedGenericList))]
        public void should_indicate_if_a_type_can_be_cast_to_another_type(Type type)
        {
            type.CanBeCastTo<IList<string>>().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_cannot_be_cast_to_another_type()
        {
            typeof(ArrayList).CanBeCastTo<IList<string>>().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(int))]
        [TestCase(typeof(int?))]
        public void should_indicate_if_a_nullable_type_can_be_cast_to_another_type(Type type)
        {
            type.CanBeCastTo<int>(true).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_nullable_type_cannot_be_cast_to_another_type()
        {
            typeof(int?).CanBeCastTo<int>(false).ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(int))]
        [TestCase(typeof(int?))]
        public void should_indicate_if_a_type_is_the_specified_type_or_its_nullable_counterpart(Type type)
        {
            type.Is<int>(true).ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(int?))]
        public void should_indicate_if_a_type_is_not_the_specified_type(Type type)
        {
            type.Is<int>().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_a_type_is_an_open_generic_type()
        {
            typeof(IEnumerable<string>).Is(typeof(IEnumerable<>)).ShouldBeTrue();
        }

        // Type kind

        public class InheritedList : List<string> { }
        public class InheritedHashtable : Hashtable { }
        public class InheritedGenericList : List<string> { }
        public class InheritedGenericDictionary : Dictionary<string, string> { }

        [Test]
        [TestCase(typeof(int), TypeKind.Simple)]
        [TestCase(typeof(WebClient), TypeKind.Complex)]
        [TestCase(typeof(ArrayList), TypeKind.Enumerable)]
        [TestCase(typeof(IEnumerable), TypeKind.Enumerable)]
        [TestCase(typeof(IList), TypeKind.Enumerable)]
        [TestCase(typeof(List<string>), TypeKind.Enumerable)]
        [TestCase(typeof(IList<string>), TypeKind.Enumerable)]
        [TestCase(typeof(InheritedList), TypeKind.Enumerable)]
        [TestCase(typeof(EnumerableImpl), TypeKind.Enumerable)]
        [TestCase(typeof(GenericStringEnumerableImpl), TypeKind.Enumerable)]
        [TestCase(typeof(InheritedGenericList), TypeKind.Enumerable)]
        [TestCase(typeof(GenericStringListImpl), TypeKind.Enumerable)]
        [TestCase(typeof(Hashtable), TypeKind.Dictionary)]
        [TestCase(typeof(IDictionary), TypeKind.Dictionary)]
        [TestCase(typeof(Dictionary<string, int>), TypeKind.Dictionary)]
        [TestCase(typeof(IDictionary<string, int>), TypeKind.Dictionary)]
        [TestCase(typeof(InheritedHashtable), TypeKind.Dictionary)]
        [TestCase(typeof(DictionaryImpl), TypeKind.Dictionary)]
        [TestCase(typeof(InheritedGenericDictionary), TypeKind.Dictionary)]
        [TestCase(typeof(GenericDictionaryImpl<string, string>), TypeKind.Dictionary)]
        public void should_indicate_type_kind(Type type, TypeKind kind)
        {
            type.ToCachedType().GetKind(false, false).ShouldEqual(kind);
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        public void should_not_override_type_kind_of_bcl_enumerables(Type type)
        {
            type.ToCachedType().GetKind(true, false).ShouldEqual(TypeKind.Enumerable);
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(Dictionary<string, int>))]
        [TestCase(typeof(IDictionary<string, int>))]
        public void should_not_override_type_kind_of_bcl_dictionaries(Type type)
        {
            type.ToCachedType().GetKind(false, true).ShouldEqual(TypeKind.Dictionary);
        }

        [Test]
        [TestCase(typeof(InheritedList))]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(InheritedGenericList))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_override_type_kind_of_non_bcl_enumerables(Type type)
        {
            type.ToCachedType().GetKind(true, false).ShouldEqual(TypeKind.Complex);
        }

        [Test]
        [TestCase(typeof(InheritedHashtable))]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(InheritedGenericDictionary))]
        [TestCase(typeof(GenericDictionaryImpl<string, string>))]
        public void should_override_type_kind_of_non_bcl_dictionaries(Type type)
        {
            type.ToCachedType().GetKind(false, true).ShouldEqual(TypeKind.Complex);
        }

        // Enums

        [Test]
        [TestCase(typeof(TypeCode))]
        [TestCase(typeof(TypeCode?))]
        public void should_indicate_if_a_type_is_an_enum(Type type)
        {
            type.IsEnum().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_an_enum()
        {
            typeof(string).IsEnum().ShouldBeFalse();
        }

        // Constructor

        public class CtorWithNoArgs { }
        public class CtorWithArgs { public CtorWithArgs(string oh, int hai) { } }

        [Test]
        public void should_indicate_if_object_has_a_matching_default_constructor_by_type()
        {
            typeof(CtorWithNoArgs).HasMatchingConstructor().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_object_does_not_have_a_matching_default_constructor_by_type()
        {
            typeof(CtorWithArgs).HasMatchingConstructor().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_object_has_a_matching_constructor_by_type()
        {
            typeof(CtorWithArgs).HasMatchingConstructor(
                new[] { typeof(string), typeof(int) }).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_object_does_not_have_a_matching_constructor_by_type()
        {
            typeof(CtorWithNoArgs).HasMatchingConstructor(
                new[] { typeof(string), typeof(int) }).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_object_has_a_matching_default_constructor_by_parameter_instance()
        {
            typeof(CtorWithNoArgs).HasMatchingConstructor((object[])null).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_object_does_not_have_a_matching_default_constructor_by_parameter_instance()
        {
            typeof(CtorWithArgs).HasMatchingConstructor((object[])null).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_object_has_a_matching_constructor_by_parameter_instance()
        {
            typeof(CtorWithArgs).HasMatchingConstructor(
                new object[] { "", 5 }).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_object_does_not_have_a_matching_constructor_by_parameter_instance()
        {
            var signature = new object[] { "", 5 };
            typeof(CtorWithNoArgs).HasMatchingConstructor(
                new object[] { "", 5 }).ShouldBeFalse();
        }

        // Data types

        public static readonly object[] NumericValues = TestCases.Create()
            .AddType<decimal>(1).AddType<decimal?>(1)
            .AddType<float>(1).AddType<float?>(1)
            .AddType<double>(1).AddType<double?>(1)
            .AddType<sbyte>(1).AddType<sbyte?>(1)
            .AddType<byte>(1).AddType<byte?>(1)
            .AddType<short>(1).AddType<short?>(1)
            .AddType<ushort>(1).AddType<ushort?>(1)
            .AddType<int>(1).AddType<int?>(1)
            .AddType<uint>(1).AddType<uint?>(1)
            .AddType<long>(1).AddType<long?>(1)
            .AddType<ulong>(1).AddType<ulong?>(1)
            .All;

        [Test]
        [TestCaseSource(nameof(NumericValues))]
        public void should_indicate_if_a_value_is_numeric(Type type, object value)
        {
            value.IsNumeric().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_value_is_not_numeric()
        {
            "hai".IsNumeric().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(decimal)), TestCase(typeof(decimal?))]
        [TestCase(typeof(float)), TestCase(typeof(float?))]
        [TestCase(typeof(double)), TestCase(typeof(double?))]
        [TestCase(typeof(sbyte)), TestCase(typeof(sbyte?))]
        [TestCase(typeof(byte)), TestCase(typeof(byte?))]
        [TestCase(typeof(short)), TestCase(typeof(short?))]
        [TestCase(typeof(ushort)), TestCase(typeof(ushort?))]
        [TestCase(typeof(int)), TestCase(typeof(int?))]
        [TestCase(typeof(uint)), TestCase(typeof(uint?))]
        [TestCase(typeof(long)), TestCase(typeof(long?))]
        [TestCase(typeof(ulong)), TestCase(typeof(ulong?))]
        public void should_indicate_if_a_type_is_numeric(Type type)
        {
            type.IsNumeric().ShouldBeTrue();

            typeof(string).IsNumeric().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_numeric()
        {
            typeof(string).IsNumeric().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(bool)), TestCase(typeof(bool?))]
        public void should_indicate_if_a_type_is_a_bool(Type type)
        {
            type.IsBoolean().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_type_is_not_a_bool()
        {
            typeof(string).IsBoolean().ShouldBeFalse();
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Uri))]
        [TestCase(typeof(ConsoleColor)), TestCase(typeof(ConsoleColor?))]
        [TestCase(typeof(char)), TestCase(typeof(char?))]
        [TestCase(typeof(decimal)), TestCase(typeof(decimal?))]
        [TestCase(typeof(bool)), TestCase(typeof(bool?))]
        [TestCase(typeof(byte)), TestCase(typeof(byte?))]
        [TestCase(typeof(sbyte)), TestCase(typeof(sbyte?))]
        [TestCase(typeof(short)), TestCase(typeof(short?))]
        [TestCase(typeof(ushort)), TestCase(typeof(ushort?))]
        [TestCase(typeof(int)), TestCase(typeof(int?))]
        [TestCase(typeof(uint)), TestCase(typeof(uint?))]
        [TestCase(typeof(long)), TestCase(typeof(long?))]
        [TestCase(typeof(ulong)), TestCase(typeof(ulong?))]
        [TestCase(typeof(double)), TestCase(typeof(double?))]
        [TestCase(typeof(float)), TestCase(typeof(float?))]
        [TestCase(typeof(IntPtr)), TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr)), TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(DateTime)), TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan)), TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid)), TestCase(typeof(Guid?))]
        public void should_indicate_if_a_type_is_simple(Type type)
        {
            type.IsSimpleType().ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(KeyValuePair<string, int>))]
        [TestCase(typeof(Tuple))]
        [TestCase(typeof(object))]
        public void should_indicate_if_a_type_is_not_simple(Type type)
        {
            type.IsSimpleType().ShouldBeFalse();
        }

        public static readonly object[] SimpleTypeParsing = TestCases.Create()
            .AddType<string>("1")
            .AddType<Uri>(new Uri("http://www.xkcd.com"))

            .AddType<UriFormat>(UriFormat.UriEscaped).AddType<UriFormat?>(UriFormat.UriEscaped)

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
        [TestCaseSource(nameof(SimpleTypeParsing))]
        public void should_parse_simple_types(Type type, object value)
        {
            var result = value.ToString().ParseSimpleType(type.ToCachedType());
            result.ShouldEqual(value);
            result.ShouldBeType(type.GetUnderlyingNullableType());
        }

        [Test]
        public void should_parse_enum_integer()
        {
            "2".ParseSimpleType(typeof(UriFormat).ToCachedType()).ShouldEqual(UriFormat.Unescaped);
        }

        [Test]
        public void should_parse_enum_case_insensitively()
        {
            "UNESCAPED".ParseSimpleType(typeof(UriFormat).ToCachedType()).ShouldEqual(UriFormat.Unescaped);
        }

        [Test]
        public void should_parse_bool_case_insensitively()
        {
            "TRUE".ParseSimpleType(typeof(bool).ToCachedType()).ShouldEqual(true);
        }

        [Test]
        public void should_return_empty_string_when_source_is_empty_string()
        {
            "".ParseSimpleType(typeof(string).ToCachedType()).ShouldEqual("");
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Uri))]
        [TestCase(typeof(UriFormat)), TestCase(typeof(UriFormat?))]
        [TestCase(typeof(DateTime)), TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan)), TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid)), TestCase(typeof(Guid?))]
        [TestCase(typeof(Boolean)), TestCase(typeof(Boolean?))]
        [TestCase(typeof(Byte)), TestCase(typeof(Byte?))]
        [TestCase(typeof(SByte)), TestCase(typeof(SByte?))]
        [TestCase(typeof(Int16)), TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16)), TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32)), TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32)), TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64)), TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64)), TestCase(typeof(UInt64?))]
        [TestCase(typeof(IntPtr)), TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr)), TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(Char)), TestCase(typeof(Char?))]
        [TestCase(typeof(Double)), TestCase(typeof(Double?))]
        [TestCase(typeof(Single)), TestCase(typeof(Single?))]
        [TestCase(typeof(Decimal)), TestCase(typeof(Decimal?))]
        public void should_return_null_when_source_is_null(Type type)
        {
            ((string)null).ParseSimpleType(type.ToCachedType()).ShouldEqual(null);
        }

        [Test]
        [TestCase(typeof(Uri))]
        [TestCase(typeof(UriFormat)), TestCase(typeof(UriFormat?))]
        [TestCase(typeof(DateTime)), TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan)), TestCase(typeof(TimeSpan?))]
        [TestCase(typeof(Guid)), TestCase(typeof(Guid?))]
        [TestCase(typeof(Boolean)), TestCase(typeof(Boolean?))]
        [TestCase(typeof(Byte)), TestCase(typeof(Byte?))]
        [TestCase(typeof(SByte)), TestCase(typeof(SByte?))]
        [TestCase(typeof(Int16)), TestCase(typeof(Int16?))]
        [TestCase(typeof(UInt16)), TestCase(typeof(UInt16?))]
        [TestCase(typeof(Int32)), TestCase(typeof(Int32?))]
        [TestCase(typeof(UInt32)), TestCase(typeof(UInt32?))]
        [TestCase(typeof(Int64)), TestCase(typeof(Int64?))]
        [TestCase(typeof(UInt64)), TestCase(typeof(UInt64?))]
        [TestCase(typeof(IntPtr)), TestCase(typeof(IntPtr?))]
        [TestCase(typeof(UIntPtr)), TestCase(typeof(UIntPtr?))]
        [TestCase(typeof(Char)), TestCase(typeof(Char?))]
        [TestCase(typeof(Double)), TestCase(typeof(Double?))]
        [TestCase(typeof(Single)), TestCase(typeof(Single?))]
        [TestCase(typeof(Decimal)), TestCase(typeof(Decimal?))]
        public void should_fail_when_source_is_empty_string(Type type)
        {
            Assert.That(() => "".ParseSimpleType(type.ToCachedType()), Throws.Exception);
            Assert.That(() => "yada".ParseSimpleType(type.ToCachedType()), Throws.Exception);
        }

        // Nullablity

        [Test]
        public void should_get_underlying_nullable_type()
        {
            typeof(int).GetUnderlyingNullableType().ShouldBe<int>();
            typeof(int?).GetUnderlyingNullableType().ShouldBe<int>();
        }

        [Test]
        public void should_indicate_if_a_type_is_nullable()
        {
            typeof(int).IsNullable().ShouldBeFalse();
            typeof(int?).IsNullable().ShouldBeTrue();
        }

        [Test]
        public void should_return_default_for_value_types_and_null_for_reference_types()
        {
            TypeExtensions.NullOrDefault<string>().ShouldBeNull(); // Normally default(string) is ""
            TypeExtensions.NullOrDefault<object>().ShouldBeNull(); // Object
            TypeExtensions.NullOrDefault<int>().ShouldEqual(0); // Primitive/Struct
            TypeExtensions.NullOrDefault<TypeCode>().ShouldEqual(TypeCode.Empty); // Enum
            TypeExtensions.NullOrDefault<TimeSpan>().ShouldEqual(TimeSpan.Zero); // Struct
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
        public void should_convert_numeric_to_enum(object value)
        {
            value.ConvertToEnum(typeof(UriFormat)).ShouldEqual(UriFormat.SafeUnescaped);
        }

        [Test]
        public void should_convert_decimal_to_enum()
        {
            3.0m.ConvertToEnum(typeof(UriFormat)).ShouldEqual(UriFormat.SafeUnescaped);
        }
    }
}
