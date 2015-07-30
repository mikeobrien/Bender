using System;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object.Values
{
    [TestFixture]
    public class SimpleValueTests
    {
        [Test]
        public void should_set_correct_defaults_in_type_overload()
        {
            var type = typeof(object);
            var value = new SimpleValue(type.ToCachedType());

            value.Instance.ShouldBeNull();
            value.SpecifiedType.Type.ShouldEqual(type);
            value.ActualType.Type.ShouldEqual(type);
            value.IsReadonly.ShouldBeFalse();
        }

        [Test]
        [TestCase(true), TestCase(false)]
        public void should_set_correct_defaults_in_value_overload(bool @readonly)
        {
            var type = typeof(object);
            var @object = new Tuple<string>("yada");
            var value = new SimpleValue(@object, type.ToCachedType(), @readonly);

            value.Instance.ShouldEqual(@object);
            value.SpecifiedType.Type.ShouldEqual(type);
            value.ActualType.Type.ShouldBe<Tuple<string>>();
            value.IsReadonly.ShouldEqual(@readonly);
        }
    }
}
