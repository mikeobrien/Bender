using System;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;

namespace Tests
{
    public class TestCases
    {
        private readonly object[] _params;
        private readonly List<object[]> _cases;

        public TestCases(params object[] @params)
        {
            _cases = new List<object[]>();
            _params = @params;
        }

        public static TestCases Create(params object[] @params)
        {
            return new TestCases(@params);
        }

        public TestCases AddFunc<T>(Func<T> func, params object[] @params)
        {
            return AddCase(new List<object> { func }.Concat(@params));
        }

        public TestCases AddAction<T>(Action<T> action, params object[] @params)
        {
            return AddCase(new List<object> { action }.Concat(@params));
        }

        public TestCases AddType<T>()
        {
            return AddCase(new List<object> { typeof(T) });
        }

        public TestCases AddType<T>(T value, params object[] @params)
        {
            return AddCase(new List<object> { typeof(T), value }.Concat(@params));
        }

        public TestCases AddTypeAndValues<T>(params T[] @params)
        {
            return AddCase(new List<object> { typeof(T) }.Concat(@params.Cast<object>()));
        }

        public TestCases AddType<T>(params object[] @params)
        {
            return AddCase(new List<object> { typeof(T) }.Concat(@params));
        }

        public TestCases Add(Action<object[]> config, params object[] @params)
        {
            config(_params);
            return AddCase(@params);
        }

        public TestCases Add(params object[] @params)
        {
            return AddCase(@params);
        }

        private TestCases AddCase(IEnumerable<object> @params)
        {
            if (!_params.Any()) _cases.Add(@params.ToArray());
            else _params.ForEach(x => _cases.Add(new [] { x }.Concat(@params).ToArray()));
            return this;
        }

        public object[] All { get { return _cases.ToArray(); } }
    }
}