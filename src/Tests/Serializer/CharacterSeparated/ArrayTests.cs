using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Collections;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.CharacterSeparated
{
    [TestFixture]
    public class ArrayTests
    {
        // Simple types 

        public class SimpleTypeModel<T>
        {
            public T Value { get; set; }
        }

        private static readonly Guid RandomGuid = Guid.NewGuid();

        private static readonly object[] SimpleArrayTypes = TestCases.Create()
            .AddTypeAndValues<string>("1")
            .AddTypeAndValues<Uri>(new Uri("http://www.xkcd.com"))

            .AddTypeAndValues<UriFormat>(UriFormat.UriEscaped)
            .AddTypeAndValues<UriFormat?>(UriFormat.UriEscaped)

            .AddTypeAndValues<DateTime>(DateTime.Today).AddTypeAndValues<DateTime?>(DateTime.Today)
            .AddTypeAndValues<TimeSpan>(TimeSpan.MaxValue).AddTypeAndValues<TimeSpan?>(TimeSpan.MaxValue)
            .AddTypeAndValues<Guid>(RandomGuid).AddTypeAndValues<Guid?>(RandomGuid)

            .AddTypeAndValues<Boolean>(true).AddTypeAndValues<Boolean?>(true)
            .AddTypeAndValues<Byte>(5).AddTypeAndValues<Byte?>(55)
            .AddTypeAndValues<SByte>(6).AddTypeAndValues<SByte?>(66)
            .AddTypeAndValues<Int16>(7).AddTypeAndValues<Int16?>(77)
            .AddTypeAndValues<UInt16>(8).AddTypeAndValues<UInt16?>(88)
            .AddTypeAndValues<Int32>(9).AddTypeAndValues<Int32?>(99)
            .AddTypeAndValues<UInt32>(10).AddTypeAndValues<UInt32?>(110)
            .AddTypeAndValues<Int64>(11).AddTypeAndValues<Int64?>(111)
            .AddTypeAndValues<UInt64>(12).AddTypeAndValues<UInt64?>(120)
            .AddTypeAndValues<IntPtr>(new IntPtr(13)).AddTypeAndValues<IntPtr?>(new IntPtr(130))
            .AddTypeAndValues<UIntPtr>(new UIntPtr(14)).AddTypeAndValues<UIntPtr?>(new UIntPtr(140))
            .AddTypeAndValues<Char>('a').AddTypeAndValues<Char?>('b')
            .AddTypeAndValues<Double>(15).AddTypeAndValues<Double?>(150)
            .AddTypeAndValues<Single>(16).AddTypeAndValues<Single?>(160)
            .AddTypeAndValues<Decimal>(17).AddTypeAndValues<Decimal?>(170)

            .All;

        [Test]
        [TestCaseSource(nameof(SimpleArrayTypes))]
        public void should_serialize_simple_types(Type type, object value)
        {
            var model = Activator.CreateInstance(typeof(SimpleTypeModel<>).MakeGenericType(type));
            model.SetPropertyOrFieldValue("Value", value);
            var list = model.GetType().MakeGenericListType().CreateInstance();
            list.As<IList>().Add(model);
            Serialize.Csv(list).ShouldEqual("\"Value\"\r\n" + 
                (type.IsNumeric() || type.IsBoolean()
                ? value.ToString().ToLower() : $"\"{value}\"") + "\r\n");
        }

        // Complex types

        public class ComplexType
        {
            public string Property {get; set; }
        }

        private static readonly ComplexType ComplexTypeInstance = new ComplexType { Property = "hai" };

        private static readonly object[] ComplexTypeArrays = TestCases.Create()
            .AddTypeAndValues(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<IEnumerable<ComplexType>>(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<IList<ComplexType>>(new List<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues(new GenericListImpl<ComplexType> { ComplexTypeInstance })
            .AddTypeAndValues<ComplexType[]>(new[] { ComplexTypeInstance })
            .AddTypeAndValues(new ArrayList { ComplexTypeInstance })
            .AddTypeAndValues<IEnumerable>(new ArrayList { ComplexTypeInstance })
            .AddTypeAndValues(new EnumerableImpl(ComplexTypeInstance))
            .AddTypeAndValues(new GenericEnumerableImpl<ComplexType>(ComplexTypeInstance))

        .All;

        [Test]
        [TestCaseSource(nameof(ComplexTypeArrays))]
        public void should_serialize_list_of_complex_type(Type type, object list)
        {
            Serialize.Csv(list, type).ShouldEqual((type.IsGenericEnumerable() ? 
                "\"Property\"\r\n" : "") + "\"hai\"\r\n");
        }

        // Actual vs specified type

        public class ActualType : ISpecifiedType
        {
            public string Actual { get; set; }
            public string Specified { get; set; }
        }

        public interface ISpecifiedType
        {
            string Specified { get; set; }
        }

        [Test]
        public void should_serialize_specified_type_by_default()
        {
            Serialize.Csv(new List<ActualType> { new ActualType
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>))
                .ShouldEqual("\"Specified\"\r\n\"hai\"\r\n");
        }

        [Test]
        public void should_serialize_actual_type_when_configured()
        {
            Serialize.Csv(new List<ActualType> { new ActualType
                { Actual = "oh", Specified = "hai" } }, typeof(List<ISpecifiedType>),
                x => x.Serialization(y => y.UseActualType()))
                .ShouldEqual("\"Actual\",\"Specified\"\r\n\"oh\",\"hai\"\r\n");
        }
    }
}
