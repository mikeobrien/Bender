using System;
using Bender.Configuration;
using Bender.Extensions;

namespace Bender.Nodes
{
    public class MapConventions<TSource, TTarget>
        where TSource : INode
        where TTarget : INode
    {
        private Action<TSource, TTarget, Options> _map;
        private Func<TSource, TTarget, Options, bool> _hasMap;
        private readonly Options _options;
        private readonly Func<Exception, TSource, TTarget, Exception> _exceptionHandler;

        public MapConventions(Options options, Func<Exception, TSource, TTarget, Exception> exceptionHandler)
        {
            _options = options;
            _exceptionHandler = exceptionHandler;
        }

        public MapConventions<TSource, TTarget> Add(Action<TSource, TTarget, Options> map,
            Func<TSource, TTarget, Options, bool> when)
        {
            _map = _map == null ? map.When(when) : map.WhenElse(when, _map);
            _hasMap = _hasMap.Or(when);
            return this;
        }

        public bool HasMapping(TSource source, TTarget target)
        {
            try
            {
                return _map != null && _hasMap(source, target, _options);
            }
            catch (Exception exception)
            {
                throw _exceptionHandler(exception, source, target);
            }
        }

        public void Map(TSource source, TTarget target)
        {
            try
            {
                if (_map != null) _map(source, target, _options);
            }
            catch (Exception exception)
            {
                throw _exceptionHandler(exception, source, target);
            }
        }
    }
}
