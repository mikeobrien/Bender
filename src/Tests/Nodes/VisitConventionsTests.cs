using System;
using Bender.Configuration;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class VisitConventionsTests
    {
        private class VisitException : Exception
        {
            public VisitException(string message, Exception exception) :
                base(message, exception) { }
        }

        private INode _source;
        private INode _target;
        private Options _options;
        private VisitConventions<INode, INode> _conventions;

        [SetUp]
        public void Setup()
        {
            _options = Options.Create();
            _source = new Node { NodeType = NodeType.Value };
            _target = new Node { NodeType = NodeType.Value };
            _conventions = new VisitConventions<INode, INode>(_options,
                (e, s, t) => new VisitException("The visit failed yo!", e));
        }

        [Test]
        public void should_not_visit_nodes_if_there_are_no_visitors()
        {
            _conventions.Visit(_source, _target);

            _source.Value.ShouldBeNull();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_visit_nodes_in_the_order_the_visitor_was_added()
        {
            _conventions
                .Add((s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        t.Value += "1";
                    })
                .Add((s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        t.Value += "2";
                    }, (s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        return true;
                    })
                .Visit(_source, _target);

            _target.Value.ShouldEqual("12");
        }

        [Test]
        public void should_not_visit_nodes_if_visitor_does_not_match()
        {
            _conventions
                .Add((s, t, o) =>
                {
                    s.Value += "a";
                    t.Value += "1";
                })
                .Add((s, t, o) =>
                {
                    s.Value += "b";
                    t.Value += "2";
                }, (s, t, o) => false)
                .Visit(_source, _target);

            _source.Value.ShouldEqual("a");
            _target.Value.ShouldEqual("1");
        }

        [Test]
        public void should_indicate_there_is_not_a_visitor_when_none_have_been_added()
        {
            _conventions.HasVisitor(null, null).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_when_there_is_a_visitor()
        {
            _conventions
                .Add((s, t, o) => { }, (s, t, o) => 
                     (string)s.Value == "a" && (string)t.Value == "b")
                .Add((s, t, o) => { }, (s, t, o) => 
                     (string)s.Value == "c" && (string)t.Value == "d");

            _source.Value = "a";
            _target.Value = "b";
            _conventions.HasVisitor(_source, _target).ShouldBeTrue();

            _target.Value = "c";
            _conventions.HasVisitor(_source, _target).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_there_is_a_visitor_when_a_visitor_is_added_without_a_predicate()
        {
            _conventions.Add((s, t, o) => { });
            _conventions.HasVisitor(null, null).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_there_is_a_visitor_when_a_visitor_is_added_without_a_predicate_and_there_are_non_matching_visitors()
        {
            _conventions
                .Add((s, t, o) => { })
                .Add((s, t, o) => { }, (s, t, o) => false);
            _conventions.HasVisitor(null, null).ShouldBeTrue();
        }

        [Test]
        public void should_handle_visit_exceptions()
        {
            var innerException = new Exception("Oh snap visit");
            _conventions.Add((s, t, o) => { throw innerException; },
                (s, t, o) => true);

            var exception = Assert.Throws<VisitException>(() => _conventions.Visit(null, null));

            exception.Message.ShouldEqual("The visit failed yo!");
            exception.ShouldBeType<VisitException>();
            exception.InnerException.Message.ShouldEqual("Oh snap visit");
            exception.InnerException.ShouldBeSameAs(innerException);
        }

        [Test]
        public void should_handle_where_exceptions()
        {
            var innerException = new Exception("Oh snap where");
            _conventions.Add((s, t, o) => { },
                (s, t, o) => { throw innerException; });

            var exception = Assert.Throws<VisitException>(() => _conventions.HasVisitor(null, null));

            exception.Message.ShouldEqual("The visit failed yo!");
            exception.ShouldBeType<VisitException>();
            exception.InnerException.Message.ShouldEqual("Oh snap where");
            exception.InnerException.ShouldBeSameAs(innerException);
        }
    }
}
