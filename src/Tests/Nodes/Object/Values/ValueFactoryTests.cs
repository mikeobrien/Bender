using System;
using System.Collections;
using System.Collections.Generic;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object.Values
{
    [TestFixture]
    public class ValueFactoryTests
    {
        // Simple value

        [Test]
        public void should_create_empty_simple_value()
        {
            var value = ValueFactory.Create(typeof(object).ToCachedType());

            value.Instance.ShouldBeNull();
            value.SpecifiedType.Type.ShouldBe<object>();
            value.ActualType.Type.ShouldBe<object>();
            value.IsReadonly.ShouldBeFalse();
        }

        [Test]
        [TestCase(true), TestCase(false)]
        public void should_create_simple_value(bool @readonly)
        {
            var backingValue = new List<int>();
            var value = ValueFactory.Create(backingValue, typeof(IList).ToCachedType(), 
                @readonly, Options.Create());

            value.Instance.ShouldEqual(backingValue);
            value.SpecifiedType.Type.ShouldBe<IList>();
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldEqual(@readonly);
        }

        [Test]
        public void should_create_simple_value_and_use_actual_type_when_configured()
        {
            var backingValue = new List<int>();
            var value = ValueFactory.Create(backingValue, typeof(IList).ToCachedType(), Options.Create(x => x
                .Serialization(y => y.UseActualType())));

            value.Instance.ShouldEqual(backingValue);
            value.SpecifiedType.Type.ShouldBe<List<int>>();
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldBeFalse();
        }

        [Test]
        public void should_create_simple_value_and_use_actual_type_when_specified_type_is_null()
        {
            var backingValue = new List<int>();
            var value = ValueFactory.Create(backingValue, null, Options.Create());

            value.Instance.ShouldEqual(backingValue);
            value.SpecifiedType.Type.ShouldBe<List<int>>();
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldBeFalse();
        }

        [Test]
        public void should_create_simple_value_and_use_actual_type_when_specified_type_is_object()
        {
            var value = ValueFactory.Create(5, typeof(object).ToCachedType(), Options.Create());

            value.Instance.ShouldEqual(5);
            value.SpecifiedType.Type.ShouldBe<int>();
            value.ActualType.Type.ShouldBe<int>();
            value.IsReadonly.ShouldBeFalse();
        }

        // Member value

        public class Member { public IList Value { get; set; } }

        public static IValue CreateMemberValue(Mode mode, object value, Options options = null)
        {
            return ValueFactory.Create(mode,
                new SimpleValue(value, value.GetType().ToCachedType()),
                new CachedMember(value.GetType().GetProperty("Value")),
                options ?? Options.Create());
        }

        [Test]
        [TestCase(Mode.Deserialize), TestCase(Mode.Serialize)]
        public void should_create_member_value(Mode mode)
        {
            var backingValue = new Member { Value = new List<int>() };
            var value = CreateMemberValue(mode, backingValue);

            value.Instance.ShouldEqual(backingValue.Value);
            value.SpecifiedType.Type.ShouldBe<IList>();
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldEqual(false);
        }

        [Test]
        [TestCase(Mode.Deserialize, typeof(IList))] 
        [TestCase(Mode.Serialize, typeof(List<int>))]
        public void should_create_member_value_and_use_actual_type_when_configured
            (Mode mode, Type specifiedType)
        {
            var backingValue = new Member { Value = new List<int>() };
            var value = CreateMemberValue(mode, backingValue, Options.Create(
                x => x.Serialization(y => y.UseActualType())));

            value.Instance.ShouldEqual(backingValue.Value);
            value.SpecifiedType.Type.ShouldEqual(specifiedType);
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldEqual(false);
        }

        public class ObjectMember { public object Value { get; set; } }

        [Test]
        [TestCase(Mode.Deserialize, typeof(object))]
        [TestCase(Mode.Serialize, typeof(List<int>))]
        public void should_create_member_value_and_use_actual_type_when_specified_type_is_object
            (Mode mode, Type specifiedType)
        {
            var backingValue = new ObjectMember { Value = new List<int>() };
            var value = CreateMemberValue(mode, backingValue);

            value.Instance.ShouldEqual(backingValue.Value);
            value.SpecifiedType.Type.ShouldEqual(specifiedType);
            value.ActualType.Type.ShouldBe<List<int>>();
            value.IsReadonly.ShouldEqual(false);
        }

        // Lazy value

        [Test]
        public void should_create_lazy_value()
        {
            var type = typeof(int);
            var backingValue = new SimpleValue(type.ToCachedType());
            var value = ValueFactory.Create(backingValue, () => 5);

            value.Instance.ShouldEqual(5);
            value.SpecifiedType.Type.ShouldEqual(type);
            value.ActualType.Type.ShouldEqual(type);
            value.IsReadonly.ShouldBeFalse();
        }
    }
}
