using System;
using Bender.Extensions;

namespace Bender.NamingConventions
{
    public class NamingConventions<T>
    {
        private Func<T, string> _convention;

        public NamingConventions(Func<T, string> @default)
        {
            _convention = @default;
        }

        public string GetName(T context)
        {
            return _convention(context);
        }

        public NamingConventions<T> Add(
            Func<string, T, string> convention)
        {
            _convention = _convention.Pipe(convention);
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, T, string> convention,
            Func<string, T, bool> when)
        {
            _convention = _convention.PipeWhen(convention, when);
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, string> convention)
        {
            _convention = _convention.Pipe((n, x) => convention(n));
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, string> convention,
            Func<string, bool> when)
        {
            _convention = _convention.PipeWhen((n, x) => convention(n), (n, x) => when(n));
            return this;
        }
    }
}
