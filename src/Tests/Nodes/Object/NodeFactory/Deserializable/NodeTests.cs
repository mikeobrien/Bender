using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Tests.Nodes.Object.NodeFactory.Deserializable
{
    [TestFixture]
    public class NodeTests
    {
        private readonly Context _context = new Context(Options.Create(), Mode.Deserialize, "xml");

        private const string NodeName = "yada";
        private ObjectNode _parent;

        [SetUp]
        public void Setup()
        {
            _parent = new ObjectNode(_context, null,
                new SimpleValue(new object(), typeof(object).GetCachedType()), null, null);
        }

        [Test]
        public void should_create_non_lazy_simple_type_node()
        {
            var type = typeof(string);
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(NodeName, value,
                _parent, _context).As<NodeBase>();

            node.ShouldNotBeCreatedLazily(x => x.Value, () => value.Instance);
            node.ShouldBeBackedBy(x => x.Value = "hai", () => value.Instance);
            node.Value.ShouldBeType(type);

            node_should_be_in_valid_state(node, NodeType.Value);
        }

        private class SomeType<T> { }

        private static readonly object[] LazyTypeCases = TestCases.Create()
            .AddType(new List<string> { "hai" }, NodeType.Array)
            .AddType(new Dictionary<string, string>(), NodeType.Object)
            .AddType(new SomeType<string>(), NodeType.Object)
            .All;

        public class Lazy { public string Member { get; set; } }

        [Test]
        [TestCaseSource("LazyTypeCases")]
        public void should_create_lazy_nodes_when_member_is_not_null_and_not_root(Type type, object @object, NodeType nodeType)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(NodeName, value,
                _parent, _context, new CachedMember(typeof(Lazy).GetProperty("Member"))).As<NodeBase>();

            node.ShouldBeCreatedLazily(x => x.Value, () => value.Instance);
            node.Value.ShouldBeType(type);

            node_should_be_in_valid_state(node, nodeType);
        }

        [Test]
        [TestCaseSource("LazyTypeCases")]
        public void should_not_create_lazy_nodes_when_member_is_null_and_not_root(Type type, object @object, NodeType nodeType)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(NodeName, value,
                _parent, _context).As<NodeBase>();

            node.ShouldNotBeCreatedLazily(x => x.Value, () => value.Instance);
            node.Value.ShouldBeType(type);

            node_should_be_in_valid_state(node, nodeType);
        }

        [Test]
        [TestCaseSource("LazyTypeCases")]
        public void should_not_create_lazy_nodes_when_root(Type type, object @object, NodeType nodeType)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(NodeName, value,
                null, _context, new CachedMember(typeof(Lazy).GetProperty("Member"))).As<NodeBase>();

            node.ShouldNotBeCreatedLazily(x => x.Value, () => value.Instance);
            node.Value.ShouldBeType(type);

            node_should_be_in_valid_state(node, nodeType, false);
        }

        [Test]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_create_object_node_when_enumerable_implementations_are_treated_as_objects(Type type)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(
                NodeName, value, _parent,
                new Context(Options.Create(x => x.TreatEnumerableImplsAsObjects()), 
                    Mode.Deserialize, "xml")).As<NodeBase>();

            node.ShouldBeType<ObjectNode>();

            node_should_be_in_valid_state(node, NodeType.Object);
        }

        [Test]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_create_array_node_when_enumerable_implementations_are_treated_as_arrays(Type type)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(
                NodeName, value, _parent,
                _context).As<NodeBase>();

            node.ShouldBeType<EnumerableNode>();

            node_should_be_in_valid_state(node, NodeType.Array);
        }

        [Test]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        public void should_create_object_node_when_dictionary_implementations_are_treated_as_objects(Type type)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(
                NodeName, value, _parent,
                new Context(Options.Create(x => x.TreatDictionaryImplsAsObjects()), 
                    Mode.Deserialize, "xml")).As<NodeBase>();

            node.ShouldBeType<ObjectNode>();

            node_should_be_in_valid_state(node, NodeType.Object);
        }

        [Test]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        public void should_create_array_node_when_dictionary_implementations_are_treated_as_arrays(Type type)
        {
            var value = new SimpleValue(type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(
                NodeName, value, _parent,
                _context).As<NodeBase>();

            node.ShouldBeType<DictionaryNode>();

            node_should_be_in_valid_state(node, NodeType.Object);
        }

        private void node_should_be_in_valid_state(NodeBase node, NodeType nodeType, bool hasAncestors = true)
        {
            if (hasAncestors)
            {
                node.Parent.ShouldBeSameAs(_parent);
                node.Parent.HasParent.ShouldBeFalse();
            }
            else node.HasParent.ShouldBeFalse();
        
            node.NodeType.ShouldEqual(nodeType);
            node.Mode.ShouldEqual(Mode.Deserialize);
            node.Format.ShouldEqual(NodeBase.NodeFormat);
            node.Name.ShouldEqual(NodeName);
        }

        private class ArrayItemMember
        {
            [XmlArrayItem("item")]
            public List<string> SomeProperty { get; set; }
        }

        [Test]
        public void should_pass_member_info_to_array_nodes()
        {
            var value = new SimpleValue(typeof(List<string>).GetCachedType());
            var member = new CachedMember(typeof(ArrayItemMember).GetProperty("SomeProperty"));
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(NodeName, value,
                _parent, _context, member).As<NodeBase>();

            Assert.DoesNotThrow(() => node.Add("item", NodeType.Value, Metadata.Empty, x => { }));
            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() => node.Add(NodeName, NodeType.Value, Metadata.Empty, x => { }));

            exception.Message.ShouldEqual("Name 'yada' does not match expected name of 'item'.");
            exception.FriendlyMessage.ShouldEqual("Name 'yada' does not match expected name of 'item'.");
        }

        public class Parent { }

        public class Child
        {
            public Child(Parent parent) { Parent = parent; }
            public Parent Parent { get; set; }
        }

        [Test]
        public void should_pass_parent_object_to_child_ctor()
        {
            var parent = new Parent();
            var parentNode = new ObjectNode(_context, null,
                new SimpleValue(parent, typeof(Parent).GetCachedType()), null, null);
            var value = new SimpleValue(typeof(Child).GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializable(null, value, parentNode,
                _context).As<NodeBase>();

            node.Value.As<Child>().Parent.ShouldBeSameAs(parent);
        }
    }
}
