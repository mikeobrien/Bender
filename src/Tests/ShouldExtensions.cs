using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bender.Extensions;
using Bender.Nodes;
using NSubstitute;
using Should;

namespace Tests
{
    public static class ShouldExtensions
    {
        public static Stream ShouldEqual(this Stream stream1, Stream stream2)
        {
            stream1.ReadAllBytes().ShouldEqual(stream2.ReadAllBytes());
            return stream1;
        }

        public static void ShouldBeWithinSeconds(this DateTime datetime, DateTime expected)
        {
            datetime.ShouldBeInRange(expected.AddSeconds(-5), expected.AddSeconds(5));
        }

        public static void ShouldBeWithinSeconds(this DateTime? datetime, DateTime expected)
        {
            datetime.ShouldNotEqual(null);
            datetime.Value.ShouldBeWithinSeconds(expected);
        }

        public static Type ShouldBe<T>(this Type type)
        {
            type.ShouldEqual(typeof (T));
            return type;
        }

        public static IEnumerable<T> ShouldContainInstance<T>(this IEnumerable<T> source, T compare) where T : class
        {
            source.ShouldContain(x => x == compare);
            return source;
        }

        public static IEnumerable<T> ShouldNotContainInstance<T>(this IEnumerable<T> source, T compare) where T : class
        {
            source.ShouldNotContain(x => x == compare);
            return source;
        }

        public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            source.Any(predicate).ShouldBeTrue();
            return source;
        }

        public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            source.Any(predicate).ShouldBeFalse();
            return source;
        }

        public static IEnumerable<T> ShouldContainNode<T>(this IEnumerable<T> source, string name) where T : INode
        {
            source.Any(x => x.Name == name).ShouldBeTrue();
            return source;
        }

        public static IEnumerable<T> ShouldNotContainNode<T>(this IEnumerable<T> source, string name) where T : INode
        {
            source.Any(x => x.Name == name).ShouldBeFalse();
            return source;
        }

        public static IDictionary ShouldContainKey(this IDictionary source, object key)
        {
            source.Contains(key).ShouldBeTrue();
            return source;
        }

        public static IDictionary ShouldNotContainKey(this IDictionary source, object key)
        {
            source.Contains(key).ShouldBeFalse();
            return source;
        }

        public static IEnumerable<T> ShouldAllMatch<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            source.All(predicate).ShouldBeTrue();
            return source;
        }

        public static IEnumerable<T> ShouldTotal<T>(this IEnumerable<T> source, int count)
        {
            source.Count().ShouldEqual(count);
            return source;
        }

        public static IEnumerable<T> ShouldTotal<T>(this IEnumerable<T> source, Func<T, bool> predicate, int count)
        {
            source.Count(predicate).ShouldEqual(count);
            return source;
        }

        public static T ShouldBeCreatedLazily<T, TResult>(this T source, Func<T, TResult> accessValue, Func<TResult> backingValue)
            where T : class
            where TResult : class
        {
            backingValue().ShouldBeNull();
            var value = accessValue(source);
            value.ShouldNotBeNull();
            value.ShouldBeSameAs(backingValue());
            return source;
        }

        public static T ShouldNotBeCreatedLazily<T, TResult>(this T source, Func<T, TResult> accessValue, Func<TResult> backingValue)
            where T : class
            where TResult : class
        {
            var value = backingValue();
            accessValue(source).ShouldBeSameAs(value);
            backingValue().ShouldBeSameAs(value);
            return source;
        }

        public static T ShouldBeBackedBy<T, TResult>(this T source, Func<T, TResult> sourceValue, Func<TResult> backingValue)
            where T : class
            where TResult : class
        {
            var value = sourceValue(source);
            backingValue().ShouldBeSameAs(value);
            return source;
        }

        public static T ShouldExecuteCallback<T>(this T source, 
            Action<T, Action<T>> act, 
            Action<T> assert)
            where T : class
        {
            var callback = Substitute.For<Action<T>>();
            act(source, callback);
            callback.Received().Invoke(Arg.Any<T>());
            var arg = (T) callback.ReceivedCalls().Last().GetArguments()[0];
            assert(arg);
            return arg;
        }

        public static void ShouldNotExecuteCallback<T>(this T source,
            Action<T, Action<T>> act)
            where T : class
        {
            var callback = Substitute.For<Action<T>>();
            act(source, callback);
            callback.DidNotReceiveWithAnyArgs().Invoke(null);
        }

        public static void ShouldEqual(this Stream stream, string expected)
        {
            stream.ReadToEnd().ShouldEqual(expected);
        }
    }
}
