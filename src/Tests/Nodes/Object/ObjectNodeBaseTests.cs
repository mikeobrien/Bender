using System;
using System.Collections.Generic;
using System.Linq;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ObjectNodeBaseTests
    {
        private static readonly Context Context = 
            new Context(Options.Create(), Mode.Serialize, "xml");

        public class Node : ObjectNodeBase
        {
            private readonly List<INode> _nodes = new List<INode>();
            private NodeType _nodeType;

            public Node(
                NodeType nodeType, 
                ObjectNodeBase parent = null) :
                base(null, null, null, parent, ObjectNodeBaseTests.Context)
            {
                _nodeType = nodeType;
            }

            public Node(string name, NodeType? type = NodeType.Object,
                ObjectNodeBase parent = null) :
                base(name, null, null, parent, ObjectNodeBaseTests.Context)
            {
                if (type.HasValue) _nodeType = type.Value;
            }

            public Node(IValue value) :
                base(null, value, null, null, ObjectNodeBaseTests.Context) { }

            public Node(
                ObjectNodeBase parent = null) :
                base(null, null, null, parent, ObjectNodeBaseTests.Context) { }

            protected override NodeType GetNodeType()
            {
                return _nodeType;
            }

            protected override void SetNodeType(NodeType nodeType)
            {
                _nodeType = nodeType;
            }

            protected override void AddNode(INode node, bool named, Action<INode> modify)
            {
                var child = new Node(named ? node.Name : null, node.NodeType, this);
                modify(child);
                _nodes.Add(child);
            }

            protected override IEnumerable<INode> GetNodes()
            {
                return _nodes;
            }
        }

        [Test]
        public void should_be_object_format()
        {
            new Node().Format.ShouldEqual(ObjectNodeBase.NodeFormat);
        }

        [Test]
        public void should_get_node_type()
        {
            new Node().NodeType.ShouldEqual(NodeType.Object);
        }

        [Test]
        public void should_fail_to_set_type()
        {
            Assert.Throws<NodeTypeReadonlyException>(() => new Node().NodeType = NodeType.Value);
        }

        [Test]
        public void should_be_named()
        {
            new Node().IsNamed.ShouldBeTrue();
        }

        [Test]
        public void should_get_name()
        {
            new Node("hai").Name.ShouldEqual("hai");
        }

        [Test]
        public void should_get_mode()
        {
            new Node("hai").Mode.ShouldEqual(Mode.Serialize);
        }

        [Test]
        public void should_get_inner_value()
        {
            var value = new SimpleValue("hai", typeof(string).ToCachedType());
            var node = new Node(value);
            node.Value.ShouldBeSameAs(value.Instance);
            node.SpecifiedType.Type.ShouldBe<String>();
            node.ActualType.Type.ShouldBe<String>();
        }

        [Test]
        public void should_get_ancestors_when_root()
        {
            var node = new Node("hai");
            node.HasParent.ShouldBeFalse();
        }

        [Test]
        public void should_get_ancestors()
        {
            var ancestor = new Node("oh");
            var node = new Node(ancestor);
            node.Parent.ShouldBeSameAs(ancestor);
            node.Parent.HasParent.ShouldBeFalse();
        }

        public class Object
        {
            public string ValueNode { get; set; }
            public List<object> EnumerableNode { get; set; }
            public Dictionary<string, object> DictionaryNode { get; set; }
            public Object ObjectNode { get; set; }
        }

        [Test]
        public void should_return_path_for_object()
        {
            var instance = new SimpleValue(new Object
            {
                ValueNode = "hai",
                EnumerableNode = new List<object>
                {
                    "hai",
                    new List<string>(),
                    new Dictionary<string, object>(),
                    new object()
                },
                DictionaryNode = new Dictionary<string, object>
                {
                    { "one", "hai" },
                    { "two", new List<string>() },
                    { "three", new Dictionary<string, string>() },
                    { "four", new object() }
                },
                ObjectNode = new Object
                {
                    ValueNode = "hai",
                    EnumerableNode = new List<object>(),
                    DictionaryNode = new Dictionary<string, object>(),
                    ObjectNode = new Object()
                }
            }, typeof (Object).ToCachedType());

            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable("yada", instance, 
                null, new Context(Options.Create(), Mode.Serialize, "yada"));

            const string baseName = "Tests.Nodes.Object.ObjectNodeBaseTests.Object";

            node.Path.ShouldEqual(baseName);

            var nodes = node.ToList();

            var valueNode = nodes.GetNode("ValueNode");
            valueNode.ShouldBeType<ValueNode>();
            valueNode.Path.ShouldEqual(baseName + ".ValueNode");

            var enumerableNode = nodes.GetNode("EnumerableNode");
            enumerableNode.ShouldBeType<EnumerableNode>();
            enumerableNode.Path.ShouldEqual(baseName + ".EnumerableNode");
            var childNodes = enumerableNode.ToList();
            childNodes.First().Path.ShouldEqual(baseName + ".EnumerableNode[0]");
            childNodes.Skip(1).First().Path.ShouldEqual(baseName + ".EnumerableNode[1]");
            childNodes.Skip(2).First().Path.ShouldEqual(baseName + ".EnumerableNode[2]");
            childNodes.Skip(3).First().Path.ShouldEqual(baseName + ".EnumerableNode[3]");

            var dictionaryNode = nodes.GetNode("DictionaryNode");
            dictionaryNode.ShouldBeType<DictionaryNode>();
            dictionaryNode.Path.ShouldEqual(baseName + ".DictionaryNode");
            dictionaryNode.GetNode("one").Path.ShouldEqual(baseName + ".DictionaryNode[\"one\"]");
            dictionaryNode.GetNode("two").Path.ShouldEqual(baseName + ".DictionaryNode[\"two\"]");
            dictionaryNode.GetNode("three").Path.ShouldEqual(baseName + ".DictionaryNode[\"three\"]");
            dictionaryNode.GetNode("four").Path.ShouldEqual(baseName + ".DictionaryNode[\"four\"]");

            var objectNode = nodes.GetNode("ObjectNode");
            objectNode.ShouldBeType<ObjectNode>();
            objectNode.Path.ShouldEqual(baseName + ".ObjectNode");
            objectNode.GetNode("ValueNode").Path.ShouldEqual(baseName + ".ObjectNode.ValueNode");
            objectNode.GetNode("EnumerableNode").Path.ShouldEqual(baseName + ".ObjectNode.EnumerableNode");
            objectNode.GetNode("DictionaryNode").Path.ShouldEqual(baseName + ".ObjectNode.DictionaryNode");
            objectNode.GetNode("ObjectNode").Path.ShouldEqual(baseName + ".ObjectNode.ObjectNode");
        }

        [Test]
        public void should_return_path_for_array()
        {
            var instance = new SimpleValue(new List<object>
            {
                "hai",
                new List<object>
                {
                    "hai",
                    new List<string>(),
                    new Dictionary<string, object>(),
                    new object()
                },
                new Dictionary<string, object>
                {
                    { "one", "hai" },
                    { "two", new List<string>() },
                    { "three", new Dictionary<string, string>() },
                    { "four", new object() }
                },
                new Object
                {
                    ValueNode = "hai",
                    EnumerableNode = new List<object>(),
                    DictionaryNode = new Dictionary<string, object>(),
                    ObjectNode = new Object()
                }
            }, typeof(List<object>).ToCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable("yada", instance,
                null, new Context(Options.Create(), Mode.Serialize, "yada"));

            const string baseName = "System.Collections.Generic.List<System.Object>";

            node.Path.ShouldEqual(baseName);

            var nodes = node.ToList();

            var valueNode = nodes.First();
            valueNode.ShouldBeType<ValueNode>();
            valueNode.Path.ShouldEqual(baseName + "[0]");

            var enumerableNode = nodes.Skip(1).First();
            enumerableNode.ShouldBeType<EnumerableNode>();
            enumerableNode.Path.ShouldEqual(baseName + "[1]");
            var childNodes = enumerableNode.ToList();
            childNodes.First().Path.ShouldEqual(baseName + "[1][0]");
            childNodes.Skip(1).First().Path.ShouldEqual(baseName + "[1][1]");
            childNodes.Skip(2).First().Path.ShouldEqual(baseName + "[1][2]");
            childNodes.Skip(3).First().Path.ShouldEqual(baseName + "[1][3]");

            var dictionaryNode = nodes.Skip(2).First();
            dictionaryNode.ShouldBeType<DictionaryNode>();
            dictionaryNode.Path.ShouldEqual(baseName + "[2]");
            dictionaryNode.GetNode("one").Path.ShouldEqual(baseName + "[2][\"one\"]");
            dictionaryNode.GetNode("two").Path.ShouldEqual(baseName + "[2][\"two\"]");
            dictionaryNode.GetNode("three").Path.ShouldEqual(baseName + "[2][\"three\"]");
            dictionaryNode.GetNode("four").Path.ShouldEqual(baseName + "[2][\"four\"]");

            var objectNode = nodes.Skip(3).First();
            objectNode.ShouldBeType<ObjectNode>();
            objectNode.Path.ShouldEqual(baseName + "[3]");
            objectNode.GetNode("ValueNode").Path.ShouldEqual(baseName + "[3].ValueNode");
            objectNode.GetNode("EnumerableNode").Path.ShouldEqual(baseName + "[3].EnumerableNode");
            objectNode.GetNode("DictionaryNode").Path.ShouldEqual(baseName + "[3].DictionaryNode");
            objectNode.GetNode("ObjectNode").Path.ShouldEqual(baseName + "[3].ObjectNode");
        }

        [Test]
        public void should_return_path_for_dictionary()
        {
            var instance = new SimpleValue(new Dictionary<string, object>
            {
                { "one", "hai" },
                { "two", new List<object>
                {
                    "hai",
                    new List<string>(),
                    new Dictionary<string, object>(),
                    new object()
                } },
                { "three", new Dictionary<string, object>
                {
                    { "one", "hai" },
                    { "two", new List<string>() },
                    { "three", new Dictionary<string, string>() },
                    { "four", new object() }
                } },
                { "four", new Object
                {
                    ValueNode = "hai",
                    EnumerableNode = new List<object>(),
                    DictionaryNode = new Dictionary<string, object>(),
                    ObjectNode = new Object()
                } }
            }, typeof(Dictionary<string, object>).ToCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable("yada", instance,
                null, new Context(Options.Create(), Mode.Serialize, "yada"));

            const string baseName = "System.Collections.Generic.Dictionary<System.String, System.Object>";

            node.Path.ShouldEqual(baseName);

            var nodes = node.ToList();

            var valueNode = nodes.First();
            valueNode.ShouldBeType<ValueNode>();
            valueNode.Path.ShouldEqual(baseName + "[\"one\"]");

            var enumerableNode = nodes.Skip(1).First();
            enumerableNode.ShouldBeType<EnumerableNode>();
            enumerableNode.Path.ShouldEqual(baseName + "[\"two\"]");
            var childNodes = enumerableNode.ToList();
            childNodes.First().Path.ShouldEqual(baseName + "[\"two\"][0]");
            childNodes.Skip(1).First().Path.ShouldEqual(baseName + "[\"two\"][1]");
            childNodes.Skip(2).First().Path.ShouldEqual(baseName + "[\"two\"][2]");
            childNodes.Skip(3).First().Path.ShouldEqual(baseName + "[\"two\"][3]");

            var dictionaryNode = nodes.Skip(2).First();
            dictionaryNode.ShouldBeType<DictionaryNode>();
            dictionaryNode.Path.ShouldEqual(baseName + "[\"three\"]");
            dictionaryNode.GetNode("one").Path.ShouldEqual(baseName + "[\"three\"][\"one\"]");
            dictionaryNode.GetNode("two").Path.ShouldEqual(baseName + "[\"three\"][\"two\"]");
            dictionaryNode.GetNode("three").Path.ShouldEqual(baseName + "[\"three\"][\"three\"]");
            dictionaryNode.GetNode("four").Path.ShouldEqual(baseName + "[\"three\"][\"four\"]");

            var objectNode = nodes.Skip(3).First();
            objectNode.ShouldBeType<ObjectNode>();
            objectNode.Path.ShouldEqual(baseName + "[\"four\"]");
            objectNode.GetNode("ValueNode").Path.ShouldEqual(baseName + "[\"four\"].ValueNode");
            objectNode.GetNode("EnumerableNode").Path.ShouldEqual(baseName + "[\"four\"].EnumerableNode");
            objectNode.GetNode("DictionaryNode").Path.ShouldEqual(baseName + "[\"four\"].DictionaryNode");
            objectNode.GetNode("ObjectNode").Path.ShouldEqual(baseName + "[\"four\"].ObjectNode");
        }
    }
}
