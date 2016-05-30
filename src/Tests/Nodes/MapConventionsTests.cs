using System;
using Bender.Configuration;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class MapConventionsTests
    {
        private class MapException : Exception
        {
            public MapException(string message, Exception exception) :
                base(message, exception) { }
        }

        private INode _source;
        private INode _target;
        private Options _options;
        private MapConventions<INode, INode> _conventions;

        [SetUp]
        public void Setup()
        {
            _options = Options.Create();
            _source = new Node { NodeType = NodeType.Value };
            _target = new Node { NodeType = NodeType.Value };
            _conventions = new MapConventions<INode, INode>(_options,
                (e, s, t) => new MapException("The map failed yo!", e));
        }

        [Test]
        public void should_not_map_nodes_if_there_are_no_mappings()
        {
            _conventions.Map(_source, _target);

            _source.Value.ShouldBeNull();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_map_nodes_in_the_order_the_map_was_added()
        {
            _conventions
                .Add((s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        t.Value = 5;
                    }, (s, t, o) => 
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        return ((int)s.Value) > 5;
                    })
                .Add((s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        t.Value = 6;
                    }, (s, t, o) =>
                    {
                        s.ShouldBeSameAs(_source);
                        t.ShouldBeSameAs(_target);
                        o.ShouldBeSameAs(_options);
                        return ((int)s.Value) > 6;
                    });

            _source.Value = 7;
            _conventions.Map(_source, _target);
            _target.Value.ShouldEqual(6);

            _source.Value = 6;
            _conventions.Map(_source, _target);
            _target.Value.ShouldEqual(5);
        }

        [Test]
        public void should_not_map_nodes_if_map_does_not_match()
        {
            _conventions
                .Add((s, t, o) =>
                {
                    s.Value = "a";
                    t.Value = "1";
                }, (s, t, o) => false)
                .Map(_source, _target);

            _source.Value.ShouldBeNull();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_indicate_there_is_not_a_map_when_none_have_been_added()
        {
            _conventions.HasMapping(null, null).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_when_there_is_a_map()
        {
            _conventions
                .Add((s, t, o) => { }, (s, t, o) => 
                     t.Value != null && (string)s.Value == "a" && (string)t.Value == "b")
                .Add((s, t, o) => { }, (s, t, o) => 
                     t.Value != null && (string)s.Value == "c" && (string)t.Value == "d");

            _source.Value = "a";
            _target.Value = "b";
            _conventions.HasMapping(_source, _target).ShouldBeTrue();

            _target.Value = "c";
            _conventions.HasMapping(_source, _target).ShouldBeFalse();
        }

        [Test]
        public void should_handle_map_exceptions()
        {
            var innerException = new Exception("Oh snap map");
            _conventions.Add((s, t, o) => { throw innerException; },
                (s, t, o) => true);

            var exception = Assert.Throws<MapException>(() => _conventions.Map(null, null));

            exception.Message.ShouldEqual("The map failed yo!");
            exception.ShouldBeType<MapException>();
            exception.InnerException.Message.ShouldEqual("Oh snap map");
            exception.InnerException.ShouldBeSameAs(innerException);
        }

        [Test]
        public void should_handle_where_exceptions()
        {
            var innerException = new Exception("Oh snap where");
            _conventions.Add((s, t, o) => { },
                (s, t, o) => { throw innerException; });

            var exception = Assert.Throws<MapException>(() => _conventions.HasMapping(null, null));

            exception.Message.ShouldEqual("The map failed yo!");
            exception.ShouldBeType<MapException>();
            exception.InnerException.Message.ShouldEqual("Oh snap where");
            exception.InnerException.ShouldBeSameAs(innerException);
        }
    }
}
