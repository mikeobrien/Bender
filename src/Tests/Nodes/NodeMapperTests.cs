using System;
using System.Linq;
using Bender.Collections;
using Bender.Nodes;
using Bender.Extensions;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class NodeMapperTests
    {
        private readonly Func<INode, INode, bool> _noMatch = (s, t) => false;
        private readonly Func<INode, INode, bool> _match = (s, t) => true;
        private readonly Action<INode, INode> _empty = (s, t) => { };
        private Node _source;
        private Node _target;

        [SetUp]
        public void Setup()
        {
            _source = new Node();
            _target = Substitute.ForPartsOf<Node>();
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Value)]
        [TestCase(NodeType.Value, NodeType.Variable)]

        [TestCase(NodeType.Object, NodeType.Object)]
        [TestCase(NodeType.Object, NodeType.Variable)]
        [TestCase(NodeType.Object, NodeType.Array)]

        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]

        [TestCase(NodeType.Variable, NodeType.Value)]
        [TestCase(NodeType.Variable, NodeType.Object)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_map_all_node_type_combinations(
            NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            _target.NodeType = targetType;

            var map = Substitute.For<Action<INode, INode>>();
            var canMap = Substitute.For<Func<INode, INode, bool>>();
            canMap.Invoke(Arg.Any<INode>(), Arg.Any<INode>()).Returns(true);

            new NodeMapper<Node, Node>(canMap, map,
                _noMatch, null).Map(_source, _target, Mode.Serialize);

            canMap.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            map.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Value)]
        [TestCase(NodeType.Value, NodeType.Variable)]

        [TestCase(NodeType.Object, NodeType.Object)]
        [TestCase(NodeType.Object, NodeType.Variable)]
        [TestCase(NodeType.Object, NodeType.Array)]

        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]

        [TestCase(NodeType.Variable, NodeType.Value)]
        [TestCase(NodeType.Variable, NodeType.Object)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_visit_all_node_type_combinations(
            NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            _target.NodeType = targetType;

            var visit = Substitute.For<Action<INode, INode>>();
            var canVisit = Substitute.For<Func<INode, INode, bool>>();
            canVisit.Invoke(Arg.Any<INode>(), Arg.Any<INode>()).Returns(true);

            new NodeMapper<Node, Node>(_noMatch, null, 
                canVisit, visit).Map(_source, _target, Mode.Serialize);

            canVisit.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            visit.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(true, true)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void should_map_and_visit_if_exists(bool hasMapper, bool hasVisitor)
        {
            _source.NodeType = NodeType.Object;
            _target.NodeType = NodeType.Object;

            var map = Substitute.For<Action<INode, INode>>();
            var canMap = Substitute.For<Func<INode, INode, bool>>();
            canMap.Invoke(Arg.Any<INode>(), Arg.Any<INode>()).Returns(hasMapper);

            var visit = Substitute.For<Action<INode, INode>>();
            var canVisit = Substitute.For<Func<INode, INode, bool>>();
            canVisit.Invoke(Arg.Any<INode>(), Arg.Any<INode>()).Returns(hasVisitor);

            new NodeMapper<Node, Node>(canMap, map,
                canVisit, visit).Map(_source, _target, Mode.Serialize);

            canMap.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            map.ReceivedWithAnyArgs(hasMapper ? 1 : 0).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            canVisit.ReceivedWithAnyArgs(1).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            visit.ReceivedWithAnyArgs(hasVisitor ? 1 : 0).Invoke(
                Arg.Is<INode>(x => x == _source),
                Arg.Is<INode>(x => x == _target));

            if (hasMapper)
            {
                _target.DidNotReceiveWithAnyArgs().Initialize();
                _target.DidNotReceiveWithAnyArgs().Validate();
            }
            else
            {
                _target.ReceivedWithAnyArgs(1).Initialize();
                _target.ReceivedWithAnyArgs(1).Validate();
            }
        }

        [Test]
        public void should_not_throw_array_to_object_mapping_error_if_source_is_array_and_target_is_object_and_mapping_exists()
        {
            _source.NodeType = NodeType.Array;
            _target.NodeType = NodeType.Object;

            Assert.DoesNotThrow(() =>
                new NodeMapper<Node, Node>(_match, _empty, _noMatch, null)
                    .Map(_source, _target, Mode.Serialize));
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Object)]
        [TestCase(NodeType.Value, NodeType.Array)]
        [TestCase(NodeType.Object, NodeType.Value)]
        [TestCase(NodeType.Array, NodeType.Value)]
        [TestCase(NodeType.Array, NodeType.Object)]
        public void should_throw_node_type_mismatch_error_if_invalid_and_mapping_does_not_exist(
            NodeType sourceType, NodeType targetType)
        {
            var source = new Node("Source", "xml") { Type = "element", NodeType = sourceType };
            if (sourceType == NodeType.Value) source.Value = "";
            var target = new Node("Target") { NodeType = targetType };

            var exception = Assert.Throws<FriendlyMappingException>(() =>
                new NodeMapper<Node, Node>(_noMatch, null, _noMatch, null)
                    .Map(source, target, Mode.Deserialize));

            var message = "Cannot map {0} {1} node to {2} {3} node.".ToFormat(
                source.NodeType.GetArticle(), source.NodeType.ToLower(),
                target.NodeType.GetArticle(), target.NodeType.ToLower());

            var friendlyMessage = "Should be {0} {1} but was {2} {3}.".ToFormat(
                target.NodeType.GetArticle(), target.NodeType.ToLower(),
                source.NodeType.GetArticle(), source.NodeType.ToLower());

            exception.Message.ShouldEqual("Error deserializing xml element 'Source' to 'Target': " + message);
            exception.FriendlyMessage.ShouldEqual("Could not read xml element 'Source': " + friendlyMessage);
            exception.InnerException.ShouldBeType<NodeTypeMismatchException>();

            var innerException = exception.InnerException.As<NodeTypeMismatchException>();
            innerException.Message.ShouldEqual(message);
            innerException.FriendlyMessage.ShouldEqual(friendlyMessage);
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Object)]
        [TestCase(NodeType.Value, NodeType.Array)]
        [TestCase(NodeType.Object, NodeType.Value)]
        [TestCase(NodeType.Array, NodeType.Value)]
        public void should_not_throw_node_type_mismatch_error_if_invalid_and_mapping_exists(
            NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            if (sourceType == NodeType.Value) _source.Value = "";
            _target.NodeType = targetType;

            Assert.DoesNotThrow(() =>
                new NodeMapper<Node, Node>(_match, _empty, _noMatch, null)
                    .Map(_source, _target, Mode.Serialize));
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Value)]
        [TestCase(NodeType.Variable, NodeType.Value)]
        [TestCase(NodeType.Value, NodeType.Variable)]
        public void should_set_value_when_no_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            _source.Value = "hai";
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_noMatch, null, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.Value.ShouldEqual("hai");
            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
        }

        [Test]
        [TestCase(NodeType.Value, NodeType.Value)]
        [TestCase(NodeType.Variable, NodeType.Value)]
        [TestCase(NodeType.Value, NodeType.Variable)]
        public void should_not_set_value_when_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            _source.Value = "hai";
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_match, _empty, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.Value.ShouldBeNull();
            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
        }

        [Test]
        [TestCase(NodeType.Object, NodeType.Object)]
        [TestCase(NodeType.Object, NodeType.Variable)]
        [TestCase(NodeType.Object, NodeType.Array)]

        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]

        [TestCase(NodeType.Variable, NodeType.Object)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_not_insert_child_when_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            _source.NodeType = sourceType;
            _source.Add("child", NodeType.Value, Metadata.Empty, x => { });
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_match, _empty, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
            _target.ShouldBeEmpty();
        }

        [Test]
        [TestCase(NodeType.Object, NodeType.Object)]
        [TestCase(NodeType.Object, NodeType.Variable)]

        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]

        [TestCase(NodeType.Variable, NodeType.Object)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_add_named_child_when_no_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            var config = Substitute.For<Action<INode>>();
            _source.NodeType = sourceType;
            _source.Add("child", NodeType.Value, new Metadata("hai"), config);
            _source.First().Value = "hai";
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_noMatch, null, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.ShouldTotal(1);
            var result = _target.First();
            result.Name.ShouldEqual("child");
            result.Value.ShouldEqual("hai");
            result.NodeType.ShouldEqual(NodeType.Value);
            result.Metadata.Get<string>().ShouldEqual("hai");
            config.ReceivedWithAnyArgs(1).Invoke(Arg.Any<INode>());

            _target.ReceivedWithAnyArgs(1).Initialize();
            _target.ReceivedWithAnyArgs(1).Validate();
        }

        [Test]
        [TestCase(NodeType.Object, NodeType.Object)]
        [TestCase(NodeType.Object, NodeType.Variable)]

        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]

        [TestCase(NodeType.Variable, NodeType.Object)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_not_add_named_child_when_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            var config = Substitute.For<Action<INode>>();
            _source.NodeType = sourceType;
            _source.Add("child", NodeType.Value, new Metadata("hai"), config);
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_match, _empty, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.DidNotReceive().Add(
                Arg.Any<INode>(), 
                Arg.Any<Action<INode>>());
            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
            _target.ShouldBeEmpty();
        }

        [Test]
        [TestCase(NodeType.Object, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_add_unnamed_child_when_no_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            var config = Substitute.For<Action<INode>>();
            _source.NodeType = sourceType;
            if (sourceType == NodeType.Object)
                _source.Add("child", NodeType.Value, new Metadata("hai"), config);
            else _source.Add(NodeType.Value, new Metadata("hai"), config);
            _source.First().Value = "hai";
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_noMatch, null, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.ShouldTotal(1);
            var result = _target.First();
            result.IsNamed.ShouldBeFalse();
            result.Value.ShouldEqual("hai");
            result.NodeType.ShouldEqual(NodeType.Value);
            result.Metadata.Get<string>().ShouldEqual("hai");
            config.ReceivedWithAnyArgs(1).Invoke(Arg.Any<INode>());

            _target.ReceivedWithAnyArgs(1).Initialize();
            _target.ReceivedWithAnyArgs(1).Validate();
        }

        [Test]
        [TestCase(NodeType.Object, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Array)]
        [TestCase(NodeType.Array, NodeType.Variable)]
        [TestCase(NodeType.Variable, NodeType.Array)]
        public void should_not_add_unnamed_child_when_mapping_exists(NodeType sourceType, NodeType targetType)
        {
            var config = Substitute.For<Action<INode>>();
            _source.NodeType = sourceType;
            if (sourceType == NodeType.Object)
                _source.Add("child", NodeType.Value, new Metadata("hai"), config);
            else _source.Add(NodeType.Value, new Metadata("hai"), config);
            _target.NodeType = targetType;

            new NodeMapper<Node, Node>(_match, _empty, _noMatch, null).Map(_source, _target, Mode.Serialize);

            _target.DidNotReceive().Add(
                Arg.Any<INode>(),
                Arg.Any<Action<INode>>());
            _target.DidNotReceiveWithAnyArgs().Initialize();
            _target.DidNotReceiveWithAnyArgs().Validate();
            _target.ShouldBeEmpty();
        }
    }
}
