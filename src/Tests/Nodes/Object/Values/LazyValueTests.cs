using System;
using System.Collections;
using System.Collections.Generic;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object.Values
{
    [TestFixture]
    public class LazyValueTests
    {
        [Test]
        public void should_lazily_init_value()
        {
            var factory = Substitute.For<Func<string>>();
            factory.Invoke().Returns("hai");
            var value = new SimpleValue(typeof(string).GetCachedType());
            var lazy = new LazyValue(value, factory);

            value.Instance.ShouldBeNull();

            for (var i = 0; i < 5; i++)
            {
                lazy.Instance.ShouldEqual("hai");
                lazy.SpecifiedType.Type.ShouldBe<string>();
                lazy.ActualType.Type.ShouldBe<string>();
                value.Instance.ShouldEqual("hai");
            }

            factory.Received(1);
        }

        [Test]
        public void should_deterministically_create_value()
        {
            var factory = Substitute.For<Func<string>>();
            factory.Invoke().Returns("hai");
            var value = new SimpleValue(typeof(string).GetCachedType());
            var lazy = new LazyValue(value, factory);

            value.Instance.ShouldBeNull();

            for (var i = 0; i < 5; i++)
            {
                lazy.EnsureValue();
                lazy.SpecifiedType.Type.ShouldBe<string>();
                lazy.ActualType.Type.ShouldBe<string>();
                value.Instance.ShouldEqual("hai");
            }

            factory.Received(1);
        }

        [Test]
        public void should_return_specified_type_as_actual_type_when_not_initialized()
        {
            var factory = Substitute.For<Func<IList>>();
            var value = new SimpleValue(typeof(IList).GetCachedType());
            var lazy = new LazyValue(value, factory);

            lazy.SpecifiedType.Type.ShouldBe<IList>();
            lazy.ActualType.Type.ShouldBe<IList>();
            factory.DidNotReceiveWithAnyArgs().Invoke();
        }

        [Test]
        public void should_get_actual_and_specified_type_when_initialized()
        {
            var factory = Substitute.For<Func<IList>>();
            factory.Invoke().Returns(new List<string>());
            var value = new SimpleValue(typeof(IList).GetCachedType());
            var lazy = new LazyValue(value, factory);

            var instance = lazy.Instance;

            lazy.SpecifiedType.Type.ShouldBe<IList>();
            lazy.ActualType.Type.ShouldBe<List<string>>();
            factory.Received(1);
        }

        [Test]
        public void should_allow_value_override()
        {
            var factory = Substitute.For<Func<string>>();
            factory.Invoke().Returns("oh");
            var value = new SimpleValue(typeof(string).GetCachedType());
            var lazy = new LazyValue(value, factory);

            value.Instance.ShouldBeNull();

            lazy.Instance = "hai";

            for (var i = 0; i < 5; i++)
            {
                lazy.Instance.ShouldEqual("hai");
                lazy.SpecifiedType.Type.ShouldBe<string>();
                lazy.ActualType.Type.ShouldBe<string>();
                value.Instance.ShouldEqual("hai");
            }

            factory.DidNotReceiveWithAnyArgs();
        }

        [Test]
        public void should_pull_type_and_readonly_from_inner_value()
        {
            var type = typeof(string).GetCachedType();
            var lazy = new LazyValue(new SimpleValue(type), () => null);
            lazy.SpecifiedType.Type.ShouldBe<string>();
            lazy.IsReadonly.ShouldBeFalse();

            lazy = new LazyValue(new SimpleValue(null, type, true), () => null);
            lazy.SpecifiedType.Type.ShouldBe<string>();
            lazy.IsReadonly.ShouldBeTrue();
        }
    }
}
