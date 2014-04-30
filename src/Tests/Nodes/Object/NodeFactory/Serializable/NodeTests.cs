using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Tests.Nodes.Object.NodeFactory.Serializable
{
    [TestFixture]
    public class NodeTests
    {
        private readonly Context _context = new Context(Options.Create(), Mode.Serialize, "xml");

        private const string NodeName = "yada";
        private ObjectNode _parent;

        [SetUp]
        public void Setup()
        {
            _parent = new ObjectNode(_context, null,
                new SimpleValue(new object(), typeof(object).GetCachedType()), null, null);
        }

        private interface ISomeInterface { }
        private class SomeType<T> : ISomeInterface { }

        private static readonly object[] TypeCases = TestCases.Create()
            .AddType("hai", NodeType.Value)
            .AddType<IList<string>>(new List<string>(), NodeType.Array)
            .AddType<IDictionary<string, string>>(new Dictionary<string, string>(), NodeType.Object)
            .AddType<ISomeInterface>(new SomeType<string>(), NodeType.Object)
            .All;

        [Test]
        [TestCaseSource("TypeCases")]
        public void should_create_node(Type specifiedType, 
            object @object, NodeType nodeType)
        {
            var value = new SimpleValue(@object, specifiedType.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                _context).As<NodeBase>();

            node_should_be_in_valid_state(node, @object, 
                value, nodeType, specifiedType);
        }

        private class ArrayItemMember
        {
            [XmlArrayItem("item")]
            public List<string> SomeProperty { get; set; }
        }

        [Test]
        public void should_pass_memberinfo_to_enumerable_node()
        {
            var @object = new List<string> { "hai" };
            var value = new SimpleValue(@object, typeof(List<string>).GetCachedType());
            var member = new CachedMember(typeof (ArrayItemMember).GetProperty("SomeProperty"));
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                _context, member).As<NodeBase>();

            node.ShouldNotBeEmpty();
            node.ShouldAllMatch(x => x.Name == "item");

            node_should_be_in_valid_state(node, @object, 
                value, NodeType.Array, @object.GetType());
        }

        [Test]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_create_object_node_when_enumerable_implementations_are_treated_as_objects(Type type)
        {
            var @object = type.CreateInstance();
            var value = new SimpleValue(@object, type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                new Context(Options.Create(x => x.TreatEnumerableImplementationsAsObjects()), 
                    Mode.Serialize, "xml")).As<NodeBase>();

            node.ShouldBeType<ObjectNode>();

            node_should_be_in_valid_state(node, @object,
                value, NodeType.Object, type);
        }

        [Test]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_create_array_node_when_enumerable_implementations_are_treated_as_arrays(Type type)
        {
            var @object = type.CreateInstance();
            var value = new SimpleValue(@object, type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                _context).As<NodeBase>();

            node.ShouldBeType<EnumerableNode>();

            node_should_be_in_valid_state(node, @object,
                value, NodeType.Array, type);
        }

        [Test]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        public void should_create_object_node_when_dictionary_implementations_are_treated_as_objects(Type type)
        {
            var @object = type.CreateInstance();
            var value = new SimpleValue(@object, type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                new Context(Options.Create(x => x.TreatDictionaryImplementationsAsObjects()), 
                    Mode.Serialize, "xml")).As<NodeBase>();

            node.ShouldBeType<ObjectNode>();

            node_should_be_in_valid_state(node, @object,
                value, NodeType.Object, type);
        }

        [Test]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        public void should_create_array_node_when_dictionary_implementations_are_treated_as_arrays(Type type)
        {
            var @object = type.CreateInstance();
            var value = new SimpleValue(@object, type.GetCachedType());
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializable(
                NodeName, value, _parent,
                _context).As<NodeBase>();

            node.ShouldBeType<DictionaryNode>();

            node_should_be_in_valid_state(node, @object,
                value, NodeType.Object, type);
        }

        private void node_should_be_in_valid_state(NodeBase node,
            object @object, SimpleValue value, NodeType nodeType, Type specifiedType)
        {
            node.Parent.ShouldBeSameAs(_parent);
            node.Parent.HasParent.ShouldBeFalse();
            node.NodeType.ShouldEqual(nodeType);
            node.Mode.ShouldEqual(Mode.Serialize);
            node.Format.ShouldEqual(NodeBase.NodeFormat);
            node.Value.ShouldBeSameAs(value.Instance);
            node.Value.ShouldBeSameAs(@object);
            node.ActualType.Type.ShouldEqual(@object.GetType());
            node.SpecifiedType.Type.ShouldEqual(specifiedType);
            node.Name.ShouldEqual(NodeName);
        }
    }
}
