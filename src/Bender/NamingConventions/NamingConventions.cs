using System;
using Bender.Extensions;

namespace Bender.NamingConventions
{
    public class NamingConventions<T>
    {
        private Func<T, string> _source;
        private Func<T, string> _override;

        public NamingConventions(Func<T, string> @default)
        {
            _source = @default;
            _override = t => _source(t);
        }

        public string GetName(T context)
        {
            return _override(context);
        }

        public NamingConventions<T> SetDefault(Func<T, string> convention)
        {
            _source = convention;
            return this;
        }

        public NamingConventions<T> Add(
            Func<T, string> convention,
            Func<T, bool> when)
        {
            _source = convention.WhenElse(when, _source);
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, T, string> convention)
        {
            _override = _override.Pipe(convention);
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, T, string> convention,
            Func<string, T, bool> when)
        {
            _override = _override.PipeWhen(convention, when);
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, string> convention)
        {
            _override = _override.Pipe((n, x) => convention(n));
            return this;
        }

        public NamingConventions<T> Add(
            Func<string, string> convention,
            Func<string, bool> when)
        {
            _override = _override.PipeWhen((n, x) => convention(n), (n, x) => when(n));
            return this;
        }
    }
}
