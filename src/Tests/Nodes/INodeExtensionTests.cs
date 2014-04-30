using System;
using System.Linq;
using Bender.Nodes;
using Bender.Nodes.Xml;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class INodeExtensionTests
    {
        [Test]
        [TestCase(NodeType.Value, true)]
        [TestCase(NodeType.Object, false)]
        [TestCase(NodeType.Array, false)]
        [TestCase(NodeType.Variable, false)]
        public void should_indicate_if_node_is_a_value(NodeType type, bool isValue)
        {
            new Node { NodeType = type }.IsValue().ShouldEqual(isValue);
            ((INode)null).IsValue().ShouldBeFalse();

            new Node { NodeType = type }.IsNotValue().ShouldEqual(!isValue);
            ((INode)null).IsNotValue().ShouldBeTrue();
        }

        [Test]
        [TestCase(NodeType.Value, true)]
        [TestCase(NodeType.Object, false)]
        [TestCase(NodeType.Array, false)]
        [TestCase(NodeType.Variable, true)]
        public void should_indicate_if_node_is_a_value_or_variable(NodeType type, bool isValueOrVariable)
        {
            new Node { NodeType = type }.IsValueOrVariable().ShouldEqual(isValueOrVariable);
            ((INode)null).IsValueOrVariable().ShouldBeFalse();
        }

        [Test]
        [TestCase(NodeType.Value, false)]
        [TestCase(NodeType.Object, true)]
        [TestCase(NodeType.Array, false)]
        [TestCase(NodeType.Variable, false)]
        public void should_indicate_if_node_is_an_object(NodeType type, bool isObject)
        {
            new Node { NodeType = type }.IsObject().ShouldEqual(isObject);
            ((INode)null).IsObject().ShouldBeFalse();
        }

        [Test]
        [TestCase(NodeType.Value, false)]
        [TestCase(NodeType.Object, true)]
        [TestCase(NodeType.Array, false)]
        [TestCase(NodeType.Variable, true)]
        public void should_indicate_if_node_is_an_object_or_variable(NodeType type, bool isObjectOrVariable)
        {
            new Node { NodeType = type }.IsObjectOrVariable().ShouldEqual(isObjectOrVariable);
            ((INode)null).IsObjectOrVariable().ShouldBeFalse();
        }

        [Test]
        [TestCase(NodeType.Value, false)]
        [TestCase(NodeType.Object, false)]
        [TestCase(NodeType.Array, true)]
        [TestCase(NodeType.Variable, false)]
        public void should_indicate_if_node_is_an_array(NodeType type, bool isArray)
        {
            new Node { NodeType = type }.IsArray().ShouldEqual(isArray);
            ((INode)null).IsArray().ShouldBeFalse();
        }

        [Test]
        [TestCase(NodeType.Value, false)]
        [TestCase(NodeType.Object, false)]
        [TestCase(NodeType.Array, false)]
        [TestCase(NodeType.Variable, true)]
        public void should_indicate_if_node_is_variable(NodeType type, bool isVariable)
        {
            new Node { NodeType = type }.IsVariable().ShouldEqual(isVariable);
            ((INode)null).IsVariable().ShouldBeFalse();
        }

        [Test]
        [TestCase(JsonNode.NodeFormat, false)]
        [TestCase(XmlNodeBase.NodeFormat, true)]
        public void should_indicate_if_node_is_xml(string format, bool isXml)
        {
            new Node(format: format).IsXml().ShouldEqual(isXml);
            ((INode)null).IsXml().ShouldBeFalse();
        }

        [Test]
        [TestCase(JsonNode.NodeFormat, true)]
        [TestCase(XmlNodeBase.NodeFormat, false)]
        public void should_indicate_if_node_is_json(string format, bool isJson)
        {
            new Node(format: format).IsJson().ShouldEqual(isJson);
            ((INode)null).IsJson().ShouldBeFalse();
        }

        [Test]
        [TestCase(null, true)]
        [TestCase("", false)]
        public void should_indicate_if_node_has_null_value(string value, bool isNull)
        {
            var node = new Node { NodeType = NodeType.Value, Value = value };

            node.IsNullValue().ShouldEqual(isNull);
            ((INode)null).IsNullValue().ShouldBeFalse();

            node.IsNonNullValue().ShouldEqual(!isNull);
            ((INode)null).IsNonNullValue().ShouldBeFalse();
        }

        [Test]
        [TestCase("oh", StringComparison.Ordinal, 1)]
        [TestCase("OH", StringComparison.Ordinal, 2)]
        [TestCase("OH", StringComparison.OrdinalIgnoreCase, 1)]
        public void should_get_unmatched_nodes(string name, StringComparison comparison, int count)
        {
            var results = new[] { new Node("oh"), new Node("hai") }
                .GetUnmatchedNodes(new[] { new Node(name), new Node("crap") },
                                    comparison);

            results.ShouldTotal(count);

            if (count == 1) results.First().Name.ShouldEqual("hai");
            else
            {
                results.First().Name.ShouldEqual("oh");
                results.Skip(1).First().Name.ShouldEqual("hai");
            }
        }

        [Test]
        [TestCase("oh", StringComparison.Ordinal, true)]
        [TestCase("OH", StringComparison.Ordinal, false)]
        [TestCase("OH", StringComparison.OrdinalIgnoreCase, true)]
        public void should_get_node_by_name(string name, StringComparison comparison, bool hasResult)
        {
            var node = new Node("oh");
            var result = new[] { node, new Node("hai") }.GetNode(name, comparison);

            if (!hasResult) result.ShouldBeNull();
            else result.ShouldBeSameAs(node);
        }

        [Test]
        public void should_encode_to_stream()
        {
            new JsonNode(NodeType.Object).Encode().ShouldEqual("{}");
        }
    }
}
