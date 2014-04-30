using System.Linq;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class NodeBaseTests
    {
        // Name

        [Test]
        public void should_set_and_get_name()
        {
            new Node
            {
                IsNamed = true, 
                Name = "yada"
            }.Name.ShouldEqual("yada");
        }

        [Test]
        public void should_not_allow_name_to_be_read_if_not_supported()
        {
            Assert.Throws<NameNotSupportedException>(() => 
                { var result = new Node { IsNamed = false }.Name; });
        }

        [Test]
        public void should_not_allow_name_to_be_set_if_not_supported()
        {
            Assert.Throws<NameNotSupportedException>(() => 
                new Node { IsNamed = false }.Name = "yada");
        }

        // Indexer

        [Test, TestCase(NodeType.Variable), TestCase(NodeType.Object)]
        public void should_get_node(NodeType type)
        {
            var parent = new Node { NodeType = type };
            var child = new Node("hai");
            parent.Add(child, x => { });
            parent.GetNode("hai").ShouldEqual(child);
        }

        [Test]
        public void should_fail_to_get_node_from_node_types_that_do_not_support_children()
        {
            Assert.Throws<ChildrenNotSupportedException>(() =>
                new Node { NodeType = NodeType.Value }.GetNode("Hai").ShouldNotBeNull());
        }

        // Value

        [Test, TestCase(NodeType.Variable), TestCase(NodeType.Value)]
        public void should_set_and_get_value(NodeType type)
        {
            new Node
            {
                NodeType = type,
                IsNamed = true,
                Value = "yada"
            }.Value.ShouldEqual("yada");
        }

        [Test, TestCase(NodeType.Object), TestCase(NodeType.Array)]
        public void should_not_allow_value_to_be_read_if_node_is_an_object_or_array(NodeType type)
        {
            Assert.Throws<ValueNotSupportedException>(() =>
            { var result = new Node { NodeType = type }.Value; });
        }

        [Test, TestCase(NodeType.Object), TestCase(NodeType.Array)]
        public void should_not_allow_value_to_be_set_if_node_is_an_object_or_array(NodeType type)
        {
            Assert.Throws<ValueNotSupportedException>(() =>
                new Node { NodeType = type }.Value = "yada");
        }

        // Adding children

        [Test, TestCase(NodeType.Variable), TestCase(NodeType.Array)]
        public void should_add_and_get_unamed_node_on_an_array_and_variable(NodeType type)
        {
            var metadata = new Metadata(new object());
            var node = new Node { NodeType = type };

            node.ShouldExecuteCallback<INode>(
                (x, c) => node.Add(NodeType.Value, metadata, c), x =>
                {
                    x.IsNamed.ShouldBeFalse();
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });

            node.ShouldExecuteCallback<INode>(
                (x, c) => node.Add(NodeType.Object, metadata, c), x =>
                {
                    x.IsNamed.ShouldBeFalse();
                    x.NodeType.ShouldEqual(NodeType.Object);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });

            node.ShouldExecuteCallback<INode>(
                (x, c) => node.Add(NodeType.Array, metadata, c), x =>
                {
                    x.IsNamed.ShouldBeFalse();
                    x.NodeType.ShouldEqual(NodeType.Array);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });
        }

        [Test, TestCase(NodeType.Variable), TestCase(NodeType.Object), TestCase(NodeType.Array)]
        public void should_add_and_get_named_node_on_an_array_and_object_and_variable(NodeType type)
        {
            var metadata = new Metadata(new object());
            var node = new Node { NodeType = type };

            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("value", NodeType.Value, metadata, c),
                x =>
                {
                    x.Name.ShouldEqual("value");
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });

            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("object", NodeType.Object, metadata, c),
                x =>
                {
                    x.Name.ShouldEqual("object");
                    x.NodeType.ShouldEqual(NodeType.Object);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });

            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("array", NodeType.Array, metadata, c),
                x =>
                {
                    x.Name.ShouldEqual("array");
                    x.NodeType.ShouldEqual(NodeType.Array);
                    x.Metadata.ShouldNotBeSameAs(metadata);
                    x.Metadata.Get<object>().ShouldBeSameAs(metadata.Get<object>());
                    node.ShouldContainInstance(x);
                });
        }

        [Test]
        public void should_not_allow_an_unamed_child_to_be_added_if_node_is_an_object()
        {
            Assert.Throws<UnnamedChildrenNotSupportedException>(() =>
                Node.CreateObject().Add(NodeType.Value, Metadata.Empty, x => { }));
        }

        [Test]
        public void should_not_allow_children_to_be_queried_if_node_is_a_value()
        {
            Assert.Throws<ChildrenNotSupportedException>(() =>
            { var result = Node.CreateValue().ToList(); });
        }

        [Test]
        public void should_not_allow_an_unamed_child_to_be_added_if_node_is_a_value()
        {
            Assert.Throws<ChildrenNotSupportedException>(() =>
                Node.CreateValue().Add(NodeType.Value, Metadata.Empty, x => { }));
        }

        [Test]
        public void should_not_allow_named_child_to_be_added_if_node_is_a_value()
        {
            Assert.Throws<ChildrenNotSupportedException>(() =>
                Node.CreateValue().Add("oh", NodeType.Value, Metadata.Empty, x => { }));
        }
    }
}
