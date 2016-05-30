using System;
using Bender.Configuration;
using Bender.Extensions;

namespace Bender.Nodes
{
    public class VisitConventions<TSource, TTarget> 
        where TSource : INode
        where TTarget : INode
    {
        private Action<TSource, TTarget, Options> _visit;
        private Func<TSource, TTarget, Options, bool> _hasVistor;
        private bool _alwaysVisit;
        private readonly Options _options;
        private readonly Func<Exception, TSource, TTarget, Exception> _exceptionHandler;

        public VisitConventions(Options options, Func<Exception, TSource, TTarget, Exception> exceptionHandler)
        {
            _options = options;
            _exceptionHandler = exceptionHandler;
        }

        public VisitConventions<TSource, TTarget> Add(Action<TSource, TTarget, Options> visit)
        {
            _visit = _visit.ThenDo(visit);
            _alwaysVisit = true;
            return this;
        }

        public VisitConventions<TSource, TTarget> Add(Action<TSource, TTarget, Options> visit,
            Func<TSource, TTarget, Options, bool> when)
        {
            _visit = _visit.ThenDoWhen(visit, when);
            if (!_alwaysVisit) _hasVistor = _hasVistor.Or(when);
            return this;
        }

        public bool HasVisitor(TSource source, TTarget target)
        {
            try
            {
                return _visit != null && (_alwaysVisit ||
                    _hasVistor(source, target, _options));
            }
            catch (Exception exception)
            {
                throw _exceptionHandler(exception, source, target);
            }
        }

        public void Visit(TSource source, TTarget target)
        {
            try
            {
                _visit?.Invoke(source, target, _options);
            }
            catch (Exception exception)
            {
                throw _exceptionHandler(exception, source, target);
            }
        }
    }
}
