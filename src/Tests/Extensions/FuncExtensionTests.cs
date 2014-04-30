using System;
using NSubstitute;
using NUnit.Framework;
using Bender.Extensions;
using Should;

namespace Tests.Extensions
{
    [TestFixture]
    public class FuncExtensionTests
    {
        [Test]
        public void should_or()
        {
            ((Func<bool>)null).Or(() => true)().ShouldBeTrue();
            new Func<bool>(() => true).Or(null)().ShouldBeTrue();
            new Func<bool>(() => false).Or(() => true)().ShouldBeTrue();
            new Func<bool>(() => false).Or(() => false)().ShouldBeFalse();

            ((Func<string, bool>)null).Or(a => a == "a")("a").ShouldBeTrue();
            new Func<string, bool>(a => a == "a").Or(null)("a").ShouldBeTrue();
            new Func<string, bool>(a => a != "a").Or(a => a == "a")("a").ShouldBeTrue();
            new Func<string, bool>(a => false).Or(a => false)("a").ShouldBeFalse();

            ((Func<string, string, bool>)null)
                .Or((a, b) => a == "a" && b == "b")("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => a == "a" && b == "b")
                .Or(null)("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => a != "a" && b != "b")
                .Or((a, b) => a == "a" && b == "b")("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => false)
                .Or((a, b) => false)("a", "b").ShouldBeFalse();

            ((Func<string, string, string, bool>)null)
                .Or((a, b, c) => a == "a" && b == "b" && c == "c")("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => a == "a" && b == "b" && c == "c")
                .Or(null)("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => a != "a" && b != "b" && c != "c")
                .Or((a, b, c) => a == "a" && b == "b" && c == "c")("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => false)
                .Or((a, b, c) => false)("a", "b", "c").ShouldBeFalse();

            ((Func<string, string, string, string, bool>)null)
                .Or((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")
                .Or(null)("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => a != "a" && b != "b" && c != "c" && d != "d")
                .Or((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => false)
                .Or((a, b, c, d) => false)("a", "b", "c", "d").ShouldBeFalse();
        }

        [Test]
        public void should_and()
        {
            ((Func<bool>)null).And(() => true)().ShouldBeTrue();
            new Func<bool>(() => true).And(null)().ShouldBeTrue();
            new Func<bool>(() => true).And(() => true)().ShouldBeTrue();
            new Func<bool>(() => true).And(() => false)().ShouldBeFalse();

            ((Func<string, bool>)null).And(a => a == "a")("a").ShouldBeTrue();
            new Func<string, bool>(a => a == "a").And(null)("a").ShouldBeTrue();
            new Func<string, bool>(a => a == "a").And(a => a == "a")("a").ShouldBeTrue();
            new Func<string, bool>(a => true).And(a => false)("a").ShouldBeFalse();

            ((Func<string, string, bool>)null)
                .And((a, b) => a == "a" && b == "b")("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => a == "a" && b == "b")
                .And(null)("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => a == "a" && b == "b")
                .And((a, b) => a == "a" && b == "b")("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => true)
                .And((a, b) => false)("a", "b").ShouldBeFalse();

            ((Func<string, string, string, bool>)null)
                .And((a, b, c) => a == "a" && b == "b" && c == "c")("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => a == "a" && b == "b" && c == "c")
                .And(null)("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => a == "a" && b == "b" && c == "c")
                .And((a, b, c) => a == "a" && b == "b" && c == "c")("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => true)
                .And((a, b, c) => false)("a", "b", "c").ShouldBeFalse();

            ((Func<string, string, string, string, bool>)null)
                .And((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")
                .And(null)("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")
                .And((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => true)
                .And((a, b, c, d) => false)("a", "b", "c", "d").ShouldBeFalse();
        }

        [Test]
        public void should_when_not()
        {
            ((Func<bool>)null).WhenNot().ShouldBeFalse();
            new Func<bool>(() => false).WhenNot().ShouldBeTrue();
            new Func<bool>(() => true).WhenNot().ShouldBeFalse();

            ((Func<string, bool>)null).WhenNot("a").ShouldBeFalse();
            new Func<string, bool>(a => a != "a").WhenNot("a").ShouldBeTrue();
            new Func<string, bool>(a => a == "a").WhenNot("a").ShouldBeFalse();

            ((Func<string, string, bool>)null)
                .WhenNot("a", "b").ShouldBeFalse();
            new Func<string, string, bool>((a, b) => a != "a" && b != "b")
                .WhenNot("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => a == "a" && b == "b")
                .WhenNot("a", "b").ShouldBeFalse();

            ((Func<string, string, string, bool>)null)
                .WhenNot("a", "b", "c").ShouldBeFalse();
            new Func<string, string, string, bool>((a, b, c) => a != "a" && b != "b" && c != "c")
                .WhenNot("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => a == "a" && b == "b" && c == "c")
                .WhenNot("a", "b", "c").ShouldBeFalse();

            ((Func<string, string, string, string, bool>)null)
                .WhenNot("a", "b", "c", "d").ShouldBeFalse();
            new Func<string, string, string, string, bool>((a, b, c, d) => a != "a" && b != "b" && c != "c" && d != "d")
                .WhenNot("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")
                .WhenNot("a", "b", "c", "d").ShouldBeFalse();
        }

        [Test]
        public void should_and_not()
        {
            new Func<bool>(() => true).AndNot(() => false)().ShouldBeTrue();
            new Func<bool>(() => true).AndNot(() => true)().ShouldBeFalse();

            new Func<string, bool>(a => a == "a").AndNot(a => false)("a").ShouldBeTrue();
            new Func<string, bool>(a => true).AndNot(a => a == "a")("a").ShouldBeFalse();

            new Func<string, string, bool>((a, b) => a == "a" && b == "b")
                .AndNot((a, b) => false)("a", "b").ShouldBeTrue();
            new Func<string, string, bool>((a, b) => true)
                .AndNot((a, b) => a == "a" && b == "b")("a", "b").ShouldBeFalse();

            new Func<string, string, string, bool>((a, b, c) => a == "a" && b == "b" && c == "c")
                .AndNot((a, b, c) => false)("a", "b", "c").ShouldBeTrue();
            new Func<string, string, string, bool>((a, b, c) => true)
                .AndNot((a, b, c) => a == "a" && b == "b" && c == "c")("a", "b", "c").ShouldBeFalse();

            new Func<string, string, string, string, bool>((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")
                .AndNot((a, b, c, d) => false)("a", "b", "c", "d").ShouldBeTrue();
            new Func<string, string, string, string, bool>((a, b, c, d) => true)
                .AndNot((a, b, c, d) => a == "a" && b == "b" && c == "c" && d == "d")("a", "b", "c", "d").ShouldBeFalse();
        }

        [Test]
        public void should_when()
        {
            var @do = Substitute.For<Action>();
            @do.When(() => true)();
            @do.Received().Invoke();

            @do.ClearReceivedCalls();
            @do.When(() => false)();
            @do.DidNotReceive().Invoke();

            var @do1 = Substitute.For<Action<string>>();
            @do1.When(a => true)("a");
            @do1.Received().Invoke("a");

            @do1.ClearReceivedCalls();
            @do1.When(a => false)("a");
            @do1.DidNotReceiveWithAnyArgs().Invoke(null);

            var @do2 = Substitute.For<Action<string, string>>();
            @do2.When((a, b) => true)("a", "b");
            @do2.Received().Invoke("a", "b");

            @do2.ClearReceivedCalls();
            @do2.When((a, b) => false)("a", "b");
            @do2.DidNotReceiveWithAnyArgs().Invoke(null, null);

            var @do3 = Substitute.For<Action<string, string, string>>();
            @do3.When((a, b, c) => true)("a", "b", "c");
            @do3.Received().Invoke("a", "b", "c");

            @do3.ClearReceivedCalls();
            @do3.When((a, b, c) => false)("a", "b", "c");
            @do3.DidNotReceiveWithAnyArgs().Invoke(null, null, null);

            var @do4 = Substitute.For<Action<string, string, string, string>>();
            @do4.When((a, b, c, d) => true)("a", "b", "c", "d");
            @do4.Received().Invoke("a", "b", "c", "d");

            @do4.ClearReceivedCalls();
            @do4.When((a, b, c, d) => false)("a", "b", "c", "d");
            @do4.DidNotReceiveWithAnyArgs().Invoke(null, null, null, null);
        }

        [Test]
        public void should_when_else_func()
        {
            new Func<string>(() => "1").WhenElse(() => true, () => "2")().ShouldEqual("1");
            new Func<string>(() => "1").WhenElse(() => false, () => "2")().ShouldEqual("2");

            new Func<string, string>(a => "1" + a).WhenElse(a => true, a => "2" + a)("a").ShouldEqual("1a");
            new Func<string, string>(a => "1" + a).WhenElse(a => false, a => "2" + a)("a").ShouldEqual("2a");

            new Func<string, string, string>((a, b) => "1" + a + b)
                .WhenElse((a, b) => true, (a, b) => "2" + a + b)("a", "b").ShouldEqual("1ab");
            new Func<string, string, string>((a, b) => "1" + a + b)
                .WhenElse((a, b) => false, (a, b) => "2" + a + b)("a", "b").ShouldEqual("2ab");

            new Func<string, string, string, string>((a, b, c) => "1" + a + b + c)
                .WhenElse((a, b, c) => true, (a, b, c) => "2" + a + b + c)("a", "b", "c").ShouldEqual("1abc");
            new Func<string, string, string, string>((a, b, c) => "1" + a + b + c)
                .WhenElse((a, b, c) => false, (a, b, c) => "2" + a + b + c)("a", "b", "c").ShouldEqual("2abc");

            new Func<string, string, string, string, string>((a, b, c, d) => "1" + a + b + c + d)
                .WhenElse((a, b, c, d) => true, (a, b, c, d) => "2" + a + b + c + d)("a", "b", "c", "d").ShouldEqual("1abcd");
            new Func<string, string, string, string, string>((a, b, c, d) => "1" + a + b + c + d)
                .WhenElse((a, b, c, d) => false, (a, b, c, d) => "2" + a + b + c + d)("a", "b", "c", "d").ShouldEqual("2abcd");
        }

        [Test]
        public void should_when_else_action()
        {
            var @do = Substitute.For<Action>(); var @else = Substitute.For<Action>();
            @do.WhenElse(() => true, @else)();
            @do.Received().Invoke(); @else.DidNotReceive().Invoke();

            @do.ClearReceivedCalls(); @else.ClearReceivedCalls();
            @do.WhenElse(() => false, @else)();
            @do.DidNotReceive().Invoke(); @else.Received().Invoke();

            var @do1 = Substitute.For<Action<string>>(); var @else1 = Substitute.For<Action<string>>();
            @do1.WhenElse(a => true, @else1)("a");
            @do1.Received().Invoke("a"); @else1.DidNotReceiveWithAnyArgs().Invoke(null);

            @do1.ClearReceivedCalls(); @else1.ClearReceivedCalls();
            @do1.WhenElse(a => false, @else1)("a");
            @do1.DidNotReceiveWithAnyArgs().Invoke(null); @else1.Received().Invoke("a");

            var @do2 = Substitute.For<Action<string, string>>(); var @else2 = Substitute.For<Action<string, string>>();
            @do2.WhenElse((a, b) => true, @else2)("a", "b");
            @do2.Received().Invoke("a", "b"); @else2.DidNotReceiveWithAnyArgs().Invoke(null, null);

            @do2.ClearReceivedCalls(); @else2.ClearReceivedCalls();
            @do2.WhenElse((a, b) => false, @else2)("a", "b");
            @do2.DidNotReceiveWithAnyArgs().Invoke(null, null); @else2.Received().Invoke("a", "b");

            var @do3 = Substitute.For<Action<string, string, string>>(); var @else3 = Substitute.For<Action<string, string, string>>();
            @do3.WhenElse((a, b, c) => true, @else3)("a", "b", "c");
            @do3.Received().Invoke("a", "b", "c"); @else3.DidNotReceiveWithAnyArgs().Invoke(null, null, null);

            @do3.ClearReceivedCalls(); @else3.ClearReceivedCalls();
            @do3.WhenElse((a, b, c) => false, @else3)("a", "b", "c");
            @do3.DidNotReceiveWithAnyArgs().Invoke(null, null, null); @else3.Received().Invoke("a", "b", "c");

            var @do4 = Substitute.For<Action<string, string, string, string>>(); var @else4 = Substitute.For<Action<string, string, string, string>>();
            @do4.WhenElse((a, b, c, d) => true, @else4)("a", "b", "c", "d");
            @do4.Received().Invoke("a", "b", "c", "d"); @else4.DidNotReceiveWithAnyArgs().Invoke(null, null, null, null);

            @do4.ClearReceivedCalls(); @else4.ClearReceivedCalls();
            @do4.WhenElse((a, b, c, d) => false, @else4)("a", "b", "c", "d");
            @do4.DidNotReceiveWithAnyArgs().Invoke(null, null, null, null); @else4.Received().Invoke("a", "b", "c", "d");
        }

        [Test]
        public void should_pipe()
        {
            new Func<string>(() => "a").Pipe<string>(a => a + "b")().ShouldEqual("ab");

            new Func<string, string>(a => a).Pipe((a, b) => a + b)("a").ShouldEqual("aa");

            new Func<string, string, string>((a, b) => a + b)
                .Pipe((a, b, c) => a + c + b)("a", "b").ShouldEqual("abba");

            new Func<string, string, string, string>((a, b, c) => a + b + c)
                .Pipe((a, b, c, d) => a + d + c + b)("a", "b", "c").ShouldEqual("abccba");

            new Func<string, string, string, string, string>((a, b, c, d) => a + b + c + d)
                .Pipe((a, b, c, d, e) => a + e + d + c + b)("a", "b", "c", "d").ShouldEqual("abcddcba");
        }

        [Test]
        public void should_pipe_conditionally()
        {
            new Func<string, string>(a => a)
                .PipeWhen((a, b) => a + b,
                      (a, b) => true)("a").ShouldEqual("aa");
            new Func<string, string>(a => a)
                .PipeWhen((a, b) => a + b,
                      (a, b) => false)("a").ShouldEqual("a");

            new Func<string, string, string>((a, b) => a + b)
                .PipeWhen((a, b, c) => a + c + b,
                      (a, b, c) => true)("a", "b").ShouldEqual("abba");
            new Func<string, string, string>((a, b) => a + b)
                .PipeWhen((a, b, c) => a + c + b,
                      (a, b, c) => false)("a", "b").ShouldEqual("ab");

            new Func<string, string, string, string>((a, b, c) => a + b + c)
                .PipeWhen((a, b, c, d) => a + d + c + b, 
                      (a, b, c, d) => true)("a", "b", "c").ShouldEqual("abccba");
            new Func<string, string, string, string>((a, b, c) => a + b + c)
                .PipeWhen((a, b, c, d) => a + d + c + b,
                      (a, b, c, d) => false)("a", "b", "c").ShouldEqual("abc");

            new Func<string, string, string, string, string>((a, b, c, d) => a + b + c + d)
                .PipeWhen((a, b, c, d, e) => a + e + d + c + b,
                      (a, b, c, d, e) => true)("a", "b", "c", "d").ShouldEqual("abcddcba");
            new Func<string, string, string, string, string>((a, b, c, d) => a + b + c + d)
                .PipeWhen((a, b, c, d, e) => a + e + d + c + b,
                      (a, b, c, d, e) => false)("a", "b", "c", "d").ShouldEqual("abcd");
        }

        [Test]
        public void should_then_do()
        {
            var @do = Substitute.For<Action>(); var @then = Substitute.For<Action>();
            @do.ThenDo(@then)();
            @do.Received().Invoke(); @then.Received().Invoke();

            @then.ClearReceivedCalls();
            ((Action)null).ThenDo(@then)();
            @then.Received().Invoke();

            var @do1 = Substitute.For<Action<string>>(); var @then1 = Substitute.For<Action<string>>();
            @do1.ThenDo(@then1)("a");
            @do1.Received().Invoke("a"); @then1.Received().Invoke("a");

            @then1.ClearReceivedCalls();
            ((Action<string>)null).ThenDo(@then1)("a");
            @then1.Received().Invoke("a");

            var @do2 = Substitute.For<Action<string, string>>(); var @then2 = Substitute.For<Action<string, string>>();
            @do2.ThenDo(@then2)("a", "b");
            @do2.Received().Invoke("a", "b"); @then2.Received().Invoke("a", "b");

            @then2.ClearReceivedCalls();
            ((Action<string, string>)null).ThenDo(@then2)("a", "b");
            @then2.Received().Invoke("a", "b");

            var @do3 = Substitute.For<Action<string, string, string>>(); var @then3 = Substitute.For<Action<string, string, string>>();
            @do3.ThenDo(@then3)("a", "b", "c");
            @do3.Received().Invoke("a", "b", "c"); @then3.Received().Invoke("a", "b", "c");

            @then3.ClearReceivedCalls();
            ((Action<string, string, string>)null).ThenDo(@then3)("a", "b", "c");
            @then3.Received().Invoke("a", "b", "c");

            var @do4 = Substitute.For<Action<string, string, string, string>>(); var @then4 = Substitute.For<Action<string, string, string, string>>();
            @do4.ThenDo(@then4)("a", "b", "c", "d");
            @do4.Received().Invoke("a", "b", "c", "d"); @then4.Received().Invoke("a", "b", "c", "d");

            @then4.ClearReceivedCalls();
            ((Action<string, string, string, string>)null).ThenDo(@then4)("a", "b", "c", "d");
            @then4.Received().Invoke("a", "b", "c", "d");
        }

        [Test]
        public void should_then_do_conditionally()
        {
            var @do = Substitute.For<Action>(); var @then = Substitute.For<Action>();
            @do.ThenDoWhen(@then, () => true)();
            @do.Received().Invoke(); @then.Received().Invoke();

            @do.ClearReceivedCalls(); @then.ClearReceivedCalls();
            @do.ThenDoWhen(@then, () => false)();
            @do.Received().Invoke(); @then.DidNotReceive().Invoke();

            @then.ClearReceivedCalls();
            ((Action)null).ThenDoWhen(@then, () => true)();
            @then.Received().Invoke();

            var @do1 = Substitute.For<Action<string>>(); var @then1 = Substitute.For<Action<string>>();
            @do1.ThenDoWhen(@then1, a => true)("a");
            @do1.Received().Invoke("a"); @then1.Received().Invoke("a");

            @do1.ClearReceivedCalls(); @then1.ClearReceivedCalls();
            @do1.ThenDoWhen(@then1, a => false)("a");
            @do1.Received().Invoke("a"); @then1.DidNotReceiveWithAnyArgs().Invoke(null);

            @then1.ClearReceivedCalls();
            ((Action<string>)null).ThenDoWhen(@then1, a => true)("a");
            @then1.Received().Invoke("a");

            var @do2 = Substitute.For<Action<string, string>>(); var @then2 = Substitute.For<Action<string, string>>();
            @do2.ThenDoWhen(@then2, (a, b) => true)("a", "b");
            @do2.Received().Invoke("a", "b"); @then2.Received().Invoke("a", "b");

            @do2.ClearReceivedCalls(); @then2.ClearReceivedCalls();
            @do2.ThenDoWhen(@then2, (a, b) => false)("a", "b");
            @do2.Received().Invoke("a", "b"); @then2.DidNotReceiveWithAnyArgs().Invoke(null, null);

            @then2.ClearReceivedCalls();
            ((Action<string, string>)null).ThenDoWhen(@then2, (a, b) => true)("a", "b");
            @then2.Received().Invoke("a", "b");

            var @do3 = Substitute.For<Action<string, string, string>>(); var @then3 = Substitute.For<Action<string, string, string>>();
            @do3.ThenDoWhen(@then3, (a, b, c) => true)("a", "b", "c");
            @do3.Received().Invoke("a", "b", "c"); @then3.Received().Invoke("a", "b", "c");

            @do3.ClearReceivedCalls(); @then3.ClearReceivedCalls();
            @do3.ThenDoWhen(@then3, (a, b, c) => false)("a", "b", "c");
            @do3.Received().Invoke("a", "b", "c"); @then3.DidNotReceiveWithAnyArgs().Invoke(null, null, null);

            @then3.ClearReceivedCalls();
            ((Action<string, string, string>)null).ThenDoWhen(@then3, (a, b, c) => true)("a", "b", "c");
            @then3.Received().Invoke("a", "b", "c");

            var @do4 = Substitute.For<Action<string, string, string, string>>(); var @then4 = Substitute.For<Action<string, string, string, string>>();
            @do4.ThenDoWhen(@then4, (a, b, c, d) => true)("a", "b", "c", "d");
            @do4.Received().Invoke("a", "b", "c", "d"); @then4.Received().Invoke("a", "b", "c", "d");

            @do4.ClearReceivedCalls(); @then4.ClearReceivedCalls();
            @do4.ThenDoWhen(@then4, (a, b, c, d) => false)("a", "b", "c", "d");
            @do4.Received().Invoke("a", "b", "c", "d"); @then4.DidNotReceiveWithAnyArgs().Invoke(null, null, null, null);

            @then4.ClearReceivedCalls();
            ((Action<string, string, string, string>)null).ThenDoWhen(@then4, (a, b, c, d) => true)("a", "b", "c", "d");
            @then4.Received().Invoke("a", "b", "c", "d");
        }
    }
}
