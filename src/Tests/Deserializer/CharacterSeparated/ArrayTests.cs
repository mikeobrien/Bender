using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Deserializer.CharacterSeparated
{
    [TestFixture]
    public class ArrayTests
    {
        public class SimpleTypeModel<T>
        {
            public T Value { get; set; }
        }

        // Simple types 

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleArrayTypes = TestCases.Create()
            .AddTypeAndValues<string>("1", null)
            .AddTypeAndValues<Uri>(new Uri("http://www.xkcd.com"), null)

            .AddTypeAndValues<UriFormat>(UriFormat.UriEscaped, UriFormat.UriEscaped)
            .AddTypeAndValues<UriFormat?>(UriFormat.UriEscaped, null)

            .AddTypeAndValues<DateTime>(DateTime.Today, DateTime.MinValue).AddTypeAndValues<DateTime?>(DateTime.Today, null)
            .AddTypeAndValues<TimeSpan>(TimeSpan.MaxValue, TimeSpan.Zero).AddTypeAndValues<TimeSpan?>(TimeSpan.MaxValue, null)
            .AddTypeAndValues<Guid>(RandomGuid, Guid.Empty).AddTypeAndValues<Guid?>(RandomGuid, null)

            .AddTypeAndValues<Boolean>(true, false).AddTypeAndValues<Boolean?>(true, null)
            .AddTypeAndValues<Byte>(5, 0).AddTypeAndValues<Byte?>(55, null)
            .AddTypeAndValues<SByte>(6, 0).AddTypeAndValues<SByte?>(66, null)
            .AddTypeAndValues<Int16>(7, 0).AddTypeAndValues<Int16?>(77, null)
            .AddTypeAndValues<UInt16>(8, 0).AddTypeAndValues<UInt16?>(88, null)
            .AddTypeAndValues<Int32>(9, 0).AddTypeAndValues<Int32?>(99, null)
            .AddTypeAndValues<UInt32>(10, 0).AddTypeAndValues<UInt32?>(110, null)
            .AddTypeAndValues<Int64>(11, 0).AddTypeAndValues<Int64?>(111, null)
            .AddTypeAndValues<UInt64>(12, 0).AddTypeAndValues<UInt64?>(120, null)
            .AddTypeAndValues<IntPtr>(new IntPtr(13), IntPtr.Zero).AddTypeAndValues<IntPtr?>(new IntPtr(130), null)
            .AddTypeAndValues<UIntPtr>(new UIntPtr(14), UIntPtr.Zero).AddTypeAndValues<UIntPtr?>(new UIntPtr(140), null)
            .AddTypeAndValues<Char>('a', Char.MinValue).AddTypeAndValues<Char?>('b', null)
            .AddTypeAndValues<Double>(15, 0).AddTypeAndValues<Double?>(150, null)
            .AddTypeAndValues<Single>(16, 0).AddTypeAndValues<Single?>(160, null)
            .AddTypeAndValues<Decimal>(17, Decimal.MinValue).AddTypeAndValues<Decimal?>(170, null)

            .All;

        [Test]
        [TestCaseSource(nameof(SimpleArrayTypes))]
        public void should_deserialize_simple_types(Type type, object value, object defaultValue)
        {
            var Csv = "\"Value\"\r\n{0}\r\n".ToFormat(type.IsNumeric() || type.IsBoolean() ? value.ToString().ToLower() : "\"" + value + "\"");

            var listType = typeof(SimpleTypeModel<>).MakeGenericType(type).MakeGenericListType();
            var result = Deserialize.Csv(Csv, listType).As<IList>();

            result.ShouldNotBeNull();
            result.ShouldBeType(listType);
            result.Count.ShouldEqual(1);
            result[0].GetPropertyOrFieldValue("Value").ShouldEqual(value);
        }


        [Test]
        public void should_fail_to_deserialize_null_value()
        {
            var results = Deserialize.Csv<List<SimpleTypeModel<int?>>>("\"Value\",\"Fark\"\r\n,\r\n");
            results.Count.ShouldEqual(1);
            results.First().Value.ShouldBeNull();
        }

        // Complex types

        public class ComplexType
        {
            public string Property { get; set; }
        }

        [Test]
        [TestCase(typeof(ComplexType[]))]
        [TestCase(typeof(IEnumerable<ComplexType>))]
        [TestCase(typeof(List<ComplexType>))]
        [TestCase(typeof(IList<ComplexType>))]
        [TestCase(typeof(GenericListImpl<ComplexType>))]
        public void should_deserialize_array_of_complex_type(Type type)
        {
            var Csv = "\"Property\"\r\n\"hai\"";

            var result = Deserialize.Csv(Csv, type).As<IList<ComplexType>>();

            result.ShouldNotBeNull();
            result.Count.ShouldEqual(1);
            result[0].Property.ShouldEqual("hai");
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(EnumerableImpl))]
        public void should_fail_to_deserialize_non_generic_arrays_of_complex_type(Type type)
        {
            var exception = Assert.Throws<MappingException>(() => 
                Deserialize.Csv("\"Property\"\r\n\"hai\"", type));

            exception.Message.ShouldEqual(("Error deserializing character separated row '1:' to " +
                "'{0}': Non generic {1} '{0}' is not supported for deserialization. Only " +
                "generic lists and generic enumerable interfaces can be deserialized.")
                    .ToFormat(type.GetFriendlyTypeFullName(), type.IsList() ? "list" : "enumerable"));
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }

        [Test]
        public void should_fail_to_deserialize_generic_enumerable_impls_of_complex_type()
        {
            var exception = Assert.Throws<MappingException>(() =>
                Deserialize.Csv<GenericEnumerableImpl<ComplexType>>(
                    "\"Property\"\r\n\"hai\""));

            exception.Message.ShouldEqual("Error deserializing character separated row '1:' to " +
                "'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.CharacterSeparated.ArrayTests.ComplexType>': " +
                "Enumerable 'Tests.Collections.Implementations.GenericEnumerableImpl<Tests.Deserializer.CharacterSeparated.ArrayTests.ComplexType>' " +
                "is not supported for deserialization. Only generic lists and generic enumerable interfaces can be deserialized.");
            exception.InnerException.ShouldBeType<TypeNotSupportedException>();
        }
    }
}
