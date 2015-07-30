using System;
using System.Collections.Generic;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using Flexo.Extensions;
using NUnit.Framework;
using Should;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Tests.Nodes.Object.NodeFactory.Serializable
{
    [TestFixture]
    public class RootNodeTests
    {
        private interface ISomeInterface { }
        private class SomeType<T> : ISomeInterface { }

        private static readonly object[] RootTypeCases = TestCases.Create()
            .AddType<IList<string>>(new List<string>(), "ArrayOfString", "ArrayOfString", NodeType.Array)
            .AddType<IDictionary<string, string>>(new Dictionary<string, string>(), "DictionaryOfString", "DictionaryOfString", NodeType.Object)
            .AddType<ISomeInterface>(new SomeType<string>(), "ISomeInterface", "SomeTypeOfString", NodeType.Object)
            .All;

        [Test]
        [TestCaseSource("RootTypeCases")]
        public void should_create_node_from_actual_type_overload(
            Type specifiedType, object value, string specifiedTypeName, string actualName, NodeType nodeType)
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializableRoot(
                value, value.ToCachedType(), Options.Create(), "xml").As<NodeBase>();

            node_should_be_in_valid_state(actualName, node, value, nodeType, value.GetType());
        }

        [Test]
        [TestCaseSource("RootTypeCases")]
        public void should_create_node_from_specified_type_overload(
            Type specifiedType, object value, string specifiedName, string actualName, NodeType nodeType)
        {
            var node = Bender.Nodes.Object.NodeFactory.CreateSerializableRoot(value, specifiedType.ToCachedType(),
                Options.Create(), "xml").As<NodeBase>();

            node_should_be_in_valid_state(specifiedName, node, value, nodeType, specifiedType);
        }

        private void node_should_be_in_valid_state(string name, NodeBase node, 
            object value, NodeType nodeType, Type specifiedType)
        {
            node.HasParent.ShouldBeFalse();
            node.NodeType.ShouldEqual(nodeType);
            node.Mode.ShouldEqual(Mode.Serialize);
            node.Format.ShouldEqual(NodeBase.NodeFormat);
            node.Value.ShouldBeSameAs(value);
            node.ActualType.Type.ShouldEqual(value.GetType());
            node.SpecifiedType.Type.ShouldEqual(specifiedType);
            node.Name.ShouldEqual(name);
        }

        [Test]
        public void should_fail_to_create_simple_type_node_from_actual_type_overload()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                Bender.Nodes.Object.NodeFactory.CreateSerializableRoot("hai", 
                    typeof(string).ToCachedType(), Options.Create(), "xml"))
                .Message.ShouldEqual("Simple type 'System.String' is not supported for " +
                                     "serialization. Only complex types can be serialized.");
        }

        [Test]
        public void should_fail_to_create_simple_type_node_from_specified_type_overload()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                Bender.Nodes.Object.NodeFactory.CreateSerializableRoot("hai", 
                    typeof(string).ToCachedType(), Options.Create(), "xml"))
                .Message.ShouldEqual("Simple type 'System.String' is not supported for " +
                                     "serialization. Only complex types can be serialized.");
        }
    }
}
