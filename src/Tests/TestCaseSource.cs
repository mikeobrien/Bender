using System;
using System.Collections.Generic;

namespace Tests
{
    public class TestCaseSource
    {
        public static object[][] Create(Action<CaseDsl> config)
        {
            var cases = new List<object[]>();
            config(new CaseDsl(cases));
            return cases.ToArray();
        }

        public class CaseDsl
        {
            private readonly List<object[]> _cases;

            public CaseDsl(List<object[]> cases)
            {
                _cases = cases;
            }

            public CaseDsl Add(params object[] parameters)
            {
                _cases.Add(parameters);
                return this;
            }
        }
    }
}
