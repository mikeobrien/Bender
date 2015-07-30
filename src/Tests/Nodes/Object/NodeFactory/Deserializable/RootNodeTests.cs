using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using Flexo.Extensions;
using NUnit.Framework;
using Should;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Tests.Nodes.Object.NodeFactory.Deserializable
{
    [TestFixture]
    public class RootNodeTests
    {
        private class SomeType<T> { }

        private static readonly object[] TypeCases = TestCases.Create()
            .AddType(new List<string>(), "ArrayOfString", NodeType.Array)
            .AddType(new Dictionary<string, string>(), "DictionaryOfString", NodeType.Object)
            .AddType(new SomeType<string>(), "SomeTypeOfString", NodeType.Object)
            .All;

        [Test]
        [TestCaseSource("TypeCases")]
        public void should_create_node_with_name(
            Type type, object @object, string name, NodeType nodeType)
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot(name, type.ToCachedType(), "xml",
                Options.Create()).As<NodeBase>();

            node.Name.ShouldEqual(name);
            node_should_be_in_valid_state(node, type, nodeType);
        }

        [Test]
        [TestCaseSource("TypeCases")]
        public void should_create_node_without_name(
            Type type, object @object, string name, NodeType nodeType)
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot(type.ToCachedType(), "xml",
                Options.Create()).As<NodeBase>();

            node.Name.ShouldBeNull();
            node_should_be_in_valid_state(node, type, nodeType);
        }

        [Test]
        public void should_create_root_node_case_insensitively_name()
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot(
                "sometypeofstring", typeof(SomeType<string>).ToCachedType(), "xml", Options.Create(x => x.Deserialization(y => y.IgnoreNameCase())));

            node.Name.ShouldEqual("sometypeofstring");
        }

        [XmlRoot("Yada")]
        private class SomeCustomNamedType { }

        [Test]
        public void should_create_root_node_with_custom_name()
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot(
                "Yada", typeof(SomeCustomNamedType).ToCachedType(), "xml", Options.Create());

            node.Name.ShouldEqual("Yada");
        }

        private void node_should_be_in_valid_state(
            NodeBase node, Type type, NodeType nodeType)
        {
            node.NodeType.ShouldEqual(nodeType);
            node.Mode.ShouldEqual(Mode.Deserialize);
            node.Format.ShouldEqual(NodeBase.NodeFormat);
            node.Value.ShouldNotBeNull();
            node.Value.ShouldBeType(type);
        }

        [Test]
        public void should_fail_to_create_node_with_invalid_name()
        {
            var exception = Assert.Throws<InvalidRootNameDeserializationException>(() =>
                Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot("hai", 
                typeof(SomeType<string>).ToCachedType(), "xml", Options.Create()));

            exception.FriendlyMessage.ShouldEqual("Xml root name 'hai' does not match expected " +
                                                  "name of 'SomeTypeOfString'.");

            exception.Message.ShouldEqual("Xml root name 'hai' does not match expected " +
                "name of 'SomeTypeOfString'. Deserializing 'Tests.Nodes.Object.NodeFactory." +
                "Deserializable.RootNodeTests.SomeType<System.String>'.");
        }

        [Test]
        public void should_create_node_with_invalid_name_when_configured()
        {
            Assert.DoesNotThrow(() =>
                Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot("hai", 
                        typeof(SomeType<string>).ToCachedType(), "xml",
                    Options.Create(x => x.Deserialization(y => y.IgnoreRootName()))));
        }

        [Test]
        public void should_fail_to_create_simple_type_node_without_name()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot("String", 
                    typeof(string).ToCachedType(), "xml", Options.Create()))
                .Message.ShouldEqual("Simple type 'System.String' is not supported for " +
                                     "deserialization. Only complex types can be deserialized.");
        }

        [Test]
        public void should_fail_to_create_simple_type_node_with_name()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                Bender.Nodes.Object.NodeFactory.CreateDeserializableRoot(
                    typeof(string).ToCachedType(), "xml", Options.Create()))
                .Message.ShouldEqual("Simple type 'System.String' is not supported for " +
                                     "deserialization. Only complex types can be deserialized.");
        }
    }
}
