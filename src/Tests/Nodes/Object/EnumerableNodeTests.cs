using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class EnumerableNodeTests
    {
        private class ArrayItemMember
        {
            [XmlArrayItem("item")]
            public List<string> SomeProperty { get; set; }
        }

        public static EnumerableNode CreateNode(Options options = null, 
            Mode mode = Mode.Deserialize)
        {
            return CreateNodeOfType<List<string>>(null, options, mode);
        }

        public static EnumerableNode CreateNodeOfType<T>(T enumerable = null,
            Options options = null, Mode mode = Mode.Deserialize,
            CachedMember member = null) where T : class
        {
            return CreateNode(enumerable, options, mode, typeof(T), member);
        }

        public static EnumerableNode CreateNode(object enumerable,
            Options options = null, Mode mode = Mode.Deserialize,
            Type type = null, CachedMember member = null)
        {
            return CreateNode(new SimpleValue(enumerable, (type ?? enumerable.GetType()).ToCachedType()),
                options, mode, type, member);
        }

        public static EnumerableNode CreateNode(IValue value,
            Options options = null, Mode mode = Mode.Deserialize,
            Type type = null, CachedMember member = null)
        {
            return new EnumerableNode(new Context(options ?? Options.Create(), mode, "xml"), null,
                value, member, null);
        }

        [Test]
        public void should_be_of_node_type_array()
        {
            CreateNode().NodeType.ShouldEqual(NodeType.Array);
        }

        [Test]
        public void should_be_of_type_enumerable()
        {
            CreateNode().Type.ShouldEqual("list");
        }

        [Test]
        public void should_return_object_from_inner_value()
        {
            var enumerable = new List<string>();
            var node = CreateNodeOfType(enumerable);
            node.Value.ShouldEqual(enumerable);
        }

        // Initialization

        [Test]
        public void should_initialize_source_value()
        {
            var node = CreateNode(new LazyValue(
                new SimpleValue(typeof(List<string>).ToCachedType()),
                () => new List<string>()));

            node.Source.As<LazyValue>().InnerValue.Instance.ShouldBeNull();
            node.Initialize();
            node.Source.As<LazyValue>().InnerValue.Instance.ShouldNotBeNull();
        }

        // Ancestors

        [Test]
        public void should_pass_ancestors_to_children_in_serialize_mode()
        {
            var parent = CreateNodeOfType(new List<string> { "oh", "hai" }, mode: Mode.Serialize);
            var children = parent.Cast<NodeBase>().ToList();
            children.ShouldTotal(2);
            children.ShouldAllMatch(x =>
                x.HasParent &&
                x.Parent == parent);
        }

        [Test]
        public void should_pass_ancestors_to_children_in_deserialize_mode()
        {
            var parent = CreateNodeOfType(new List<string>(), mode: Mode.Deserialize);
            parent.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("String", NodeType.Value, Metadata.Empty, c),
                x => x.As<NodeBase>().Parent.ShouldBeSameAs(parent));
            parent.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("String", NodeType.Value, Metadata.Empty, c),
                x => x.As<NodeBase>().Parent.ShouldBeSameAs(parent));
        }

        // Adding items

        [Test]
        [TestCase(typeof(string[]))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(GenericListImpl<string>))]
        public void should_add_nodes(Type type)
        {
            var value = new SimpleValue(type.IsArray ? type.CreateArray() :
                type.CreateInstance(), type.ToCachedType());
            var node = CreateNode(value);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = 1);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = "hai");

            var list = value.Instance.As<IList<string>>();
            list.Count.ShouldEqual(2);
            list[0].ShouldEqual("1");
            list[1].ShouldEqual("hai");
        }

        [Test]
        [TestCase(typeof(string[]))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(GenericListImpl<string>))]
        public void should_add_named_nodes(Type type)
        {
            var value = new SimpleValue(type.IsArray ? type.CreateArray() :
                type.CreateInstance(), type.ToCachedType());
            var node = CreateNode(value);
            node.Add("String", NodeType.Value, Metadata.Empty, x => x.Value = 1);
            node.Add("String", NodeType.Value, Metadata.Empty, x => x.Value = "hai");

            var list = value.Instance.As<IList<string>>();
            list.Count.ShouldEqual(2);
            list[0].ShouldEqual("1");
            list[1].ShouldEqual("hai");
        }

        [Test]
        public void should_fail_to_add_case_sensitive_nodes_by_default()
        {
            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() => CreateNode(new SimpleValue(new List<String>(),
                typeof(List<String>).ToCachedType())).Add("string", NodeType.Value, Metadata.Empty, x => { }));

            var message = "Name 'string' does not match expected name of 'String'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_not_fail_to_add_case_sensitive_nodes_when_configured()
        {
            Assert.DoesNotThrow(() => CreateNode(new SimpleValue(new List<String>(), typeof(List<String>).ToCachedType()), 
                Options.Create(x => x.Deserialization(y => y.IgnoreNameCase())))
                .Add("string", NodeType.Value, Metadata.Empty, x => { })); ;
        }

        [Test]
        public void should_pass_member_info_to_array_item_name_convention_when_adding()
        {
            var member = new CachedMember(typeof(ArrayItemMember).GetProperty("SomeProperty"));
            var node = CreateNodeOfType(new List<string>(), member: member);
            Assert.DoesNotThrow(() => node.Add("item", NodeType.Value, Metadata.Empty, x => { }));
            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() => 
                node.Add("yada", NodeType.Value, Metadata.Empty, x => { }));

            var message = "Name 'yada' does not match expected name of 'item'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_add_named_nodes_with_custom_array_item_name_convention()
        {
            var options = Options.Create(x => x.WithArrayItemNamingConvention((c, o) => "hai"));

            Assert.DoesNotThrow(() => CreateNodeOfType(new List<string>(), options)
                .Add("hai", NodeType.Value, Metadata.Empty, x => x.Value = 1));

            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() => CreateNodeOfType(new List<string>(), options)
                .Add("String", NodeType.Value, Metadata.Empty, x => x.Value = 1));

            var message = "Name 'String' does not match expected name of 'hai'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_add_named_nodes_with_custom_type_name_convention()
        {
            var options = Options.Create(x => x.WithTypeNamingConvention((c, o) => "hai"));

            Assert.DoesNotThrow(() => CreateNodeOfType(new List<string>(), options)
                .Add("hai", NodeType.Value, Metadata.Empty, x => x.Value = 1));

            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() => CreateNodeOfType(new List<string>(), options)
                .Add("String", NodeType.Value, Metadata.Empty, x => x.Value = 1));

            var message = "Name 'String' does not match expected name of 'hai'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_fail_to_add_invalidly_named_items()
        {
            var exception = Assert.Throws<InvalidItemNameDeserializationException>(() =>
                CreateNodeOfType(new List<string>())
                .Add(new ElementNode(new XElement("root", new XElement("yada")).Element("yada"), Options.Create()), x => { }));

            var message = "Name 'yada' does not match expected name of 'String'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_not_fail_to_add_invalidly_named_items_when_configured()
        {
            Assert.DoesNotThrow(() => CreateNodeOfType(new List<string>(), 
                Options.Create(x => x.Deserialization(y => y.IgnoreArrayItemNames())))
                .Add("yada", NodeType.Value, Metadata.Empty, x => { }));
        }

        [Test]
        [TestCase(typeof(EnumerableImpl), "enumerable")]
        [TestCase(typeof(ArrayList), "list")]
        public void should_fail_to_add_item_to_non_generic_enumerable(Type type, string kind)
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                CreateNode(type.CreateInstance())
                .Add("yada", NodeType.Value, Metadata.Empty, x => { }))
                .Message.ShouldEqual(("Non generic {0} '{1}' is not supported for " +
                                     "deserialization. Only generic lists and generic enumerable interfaces " +
                                     "can be deserialized.").ToFormat(kind, type.GetFriendlyTypeFullName()));
        }

        [Test]
        public void should_fail_to_add_item_to_non_list()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                CreateNodeOfType(new GenericEnumerableImpl<string>())
                .Add(NodeType.Value, Metadata.Empty, x => { }))
                .Message.ShouldEqual("Enumerable 'Tests.Collections.Implementations.GenericEnumerableImpl<System.String>' " +
                                     "is not supported for deserialization. Only generic lists and generic enumerable " +
                                     "interfaces can be deserialized.");
        }

        // Inserting items

        [Test]
        [TestCase(typeof(INode[]))]
        [TestCase(typeof(List<Node>))]
        [TestCase(typeof(List<INode>))]
        public void should_insert_node(Type type)
        {
            var value = new SimpleValue(type.IsArray ? type.CreateArray() :
                type.CreateInstance(), type.ToCachedType());
            var parent = CreateNode(value);
            var child = Node.CreateValue();
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            var list = value.Instance.As<IList>();
            list.Count.ShouldEqual(1);
            list[0].ShouldBeSameAs(child);
        }

        [Test]
        public void should_insert_node_backed_by_generic_list_implementation()
        {
            var list = new GenericListImpl<INode>();
            var parent = CreateNodeOfType(list);
            var child = Node.CreateValue();
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            list.Count.ShouldEqual(1);
            list[0].ShouldBeSameAs(child);
        }

        [Test]
        public void should_not_insert_node_if_node_types_dont_match_but_indicate_that_it_can_insert_it()
        {
            var list = new List<JsonNode>();
            var parent = CreateNodeOfType(list);
            var child = Node.CreateValue();
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            list.Count.ShouldEqual(0);
        }

        // Enumeration

        [Test]
        [TestCase(typeof(int), NodeType.Value, "Int32")]
        [TestCase(typeof(List<int>), NodeType.Array, "ArrayOfInt32")]
        [TestCase(typeof(object), NodeType.Object, "Object")]
        public void should_enumerate_nodes_backed_by_array_list(Type type, 
            NodeType nodeType, string name)
        {
            var item1 = type.CreateInstance();
            var item2 = type.CreateInstance();
            var children = CreateNodeOfType(
                    new ArrayList { item1 ,  item2 }, 
                    mode: Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(2);

            children.ShouldAllMatch(x => x.NodeType == nodeType);
            children.ShouldAllMatch(x => x.Name == name);

            children.ShouldTotal(x => x.Value == item1, 1);
            children.ShouldTotal(x => x.Value == item2, 1);
        }

        public static object[] GenericEnumerableCases = TestCases.Create()
            .Add(new List<int> { 1, 2 }, NodeType.Value, "Int32")
            .Add(new List<List<int>> { new List<int>(), new List<int>() }, NodeType.Array, "ArrayOfInt32")
            .Add(new List<object> { new object(), new object() }, NodeType.Object, "Object")
            .All;

        [Test]
        [TestCaseSource(nameof(GenericEnumerableCases))]
        public void should_enumerate_nodes_backed_by_generic_list(
            object enumerable, NodeType nodeType, string name)
        {
            var children = CreateNode(enumerable, mode: Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(2);

            var child = children.First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual(name);
            child.Value.ShouldEqual(((IList)enumerable)[0]);

            child = children.Skip(1).First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual(name);
            child.Value.ShouldEqual(((IList)enumerable)[1]);
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(List<INode>))]
        [TestCase(typeof(List<Node>))]
        public void should_enumerate_node_instances(Type type)
        {
            var list = type.CreateInstance().As<IList>();
            var child1 = Node.CreateValue();
            var child2 = Node.CreateValue();
            list.Add(child1);
            list.Add(child2);
            var children = CreateNodeOfType(list, mode: Mode.Serialize).ToList();

            children.ShouldTotal(2);

            children[0].ShouldBeSameAs(child1);
            children[1].ShouldBeSameAs(child2);
        }

        [Test]
        public void should_pass_member_info_to_array_item_name_convention_when_enumerating()
        {
            var member = new CachedMember(typeof(ArrayItemMember).GetProperty("SomeProperty"));
            var node = CreateNodeOfType(new List<string> { "oh", "hai" },
                mode: Mode.Serialize, member: member);
            node.ShouldNotBeEmpty();
            node.ShouldAllMatch(x => x.Name == "item");
        }

        public class CyclicRoot
        {
            public ArrayList ArrayList { get; set; }
            public List<CyclicRoot> List { get; set; }
            public List<object> ObjectList { get; set; }
        }

        public static object[] CyclicCases = TestCases.Create(new CyclicRoot())
            .Add(x => x[0].As<CyclicRoot>().ArrayList = new ArrayList { x[0], new CyclicRoot() }, "ArrayList")
            .Add(x => x[0].As<CyclicRoot>().List = new List<CyclicRoot> { x[0].As<CyclicRoot>(), new CyclicRoot() }, "List")
            .Add(x => x[0].As<CyclicRoot>().ObjectList = new List<object> { x[0], new CyclicRoot() }, "ObjectList")
            .All;

        [Test]
        [TestCaseSource(nameof(CyclicCases))]
        public void should_not_return_cyclic_references_in_serialize_mode(CyclicRoot root, string name)
        {
            var members = new ObjectNode(new Context(Options.Create(), Mode.Serialize, "xml"), null,
                new SimpleValue(root, typeof(CyclicRoot).ToCachedType()), null, null).ToList();

            var child = members.GetNode(name);
            child.ShouldTotal(1);
            child.Cast<NodeBase>().ShouldAllMatch(x => x.Value != root);
        }

        [Test]
        public void should_not_return_null_values()
        {
            var parent = CreateNodeOfType(new List<string> { "oh", null }, mode: Mode.Serialize);
            var children = parent.Cast<NodeBase>().ToList();
            children.ShouldTotal(1);
            children.ShouldContain(x => (string)x.Value == "oh");
        }

        [Test]
        public void should_not_return_items_in_deserialize_mode()
        {
            CreateNodeOfType(new List<string> { "hai" }, 
                mode: Mode.Deserialize).ShouldBeEmpty();
        }

        [Test]
        public void should_enumerate_nodes_with_custom_array_item_name_convention()
        {
            var options = Options.Create(x => x.WithArrayItemNamingConvention((c, o) => "hai"));
            CreateNodeOfType(new List<string> { "oh", "hai" }, options, Mode.Serialize)
                .ShouldAllMatch(x => x.Name == "hai");
        }

        [Test]
        public void should_enumerate_nodes_with_custom_type_name_convention()
        {
            var options = Options.Create(x => x.WithTypeNamingConvention((c, o) => "hai"));
            CreateNodeOfType(new List<string> { "oh", "hai" }, options, Mode.Serialize)
                .ShouldAllMatch(x => x.Name == "hai");
        }

        [Test]
        [TestCase(typeof(List<INode>))]
        [TestCase(typeof(List<Node>))]
        public void should_return_node_value_when_a_node_implementation(Type type)
        {
            var instance = type.CreateInstance().As<IList>();
            var child = new Node("yada");
            instance.Add(child);
            var parent = CreateNode(new SimpleValue(instance, type.ToCachedType()), mode: Mode.Serialize);
            parent.GetNode("yada").ShouldBeSameAs(child);
        }

        // Actual vs generically specified type enumeration

        public interface IInterface
        {
            string Member1 { get; set; }
        }

        public class ConcreteType : BaseType, IInterface
        {
            public string Member2 { get; set; }
        }

        public class BaseType
        {
            public string Member1 { get; set; }
        }

        public static object[] GenericEnumerableTypeCases = TestCases.Create()
            .AddType(new List<IInterface> { new ConcreteType { Member1 = "", Member2 = "" } })
            .AddType(new List<BaseType> { new ConcreteType { Member1 = "", Member2 = "" } })
            .AddType<IList<IInterface>>(new List<IInterface> { new ConcreteType { Member1 = "", Member2 = "" } })
            .AddType<IList<BaseType>>(new List<BaseType> { new ConcreteType { Member1 = "", Member2 = "" } })
            .AddType<IEnumerable<IInterface>>(new List<IInterface> { new ConcreteType { Member1 = "", Member2 = "" } })
            .AddType<IEnumerable<BaseType>>(new List<BaseType> { new ConcreteType { Member1 = "", Member2 = "" } })
            .All;

        [Test]
        [TestCaseSource(nameof(GenericEnumerableTypeCases))]
        public void should_enuerate_specified_type_when_backed_by_a_specified_generic_enumerable(
            Type type, object enumerable)
        {
            var items = CreateNode(enumerable, type: type,
                mode: Mode.Serialize).Cast<NodeBase>().ToList();

            var members = items.First();
            members.ShouldTotal(1);
            members.ShouldContainNode("Member1");
        }

        [Test]
        [TestCaseSource(nameof(GenericEnumerableTypeCases))]
        public void should_enuerate_actual_type_when_configured_and_backed_by_a_specified_generic_enumerable(
            Type type, object enumerable)
        {
            var items = CreateNode(enumerable,
                Options.Create(x => x.Serialization(y => y.UseActualType())), 
                Mode.Serialize, type).Cast<NodeBase>().ToList();

            var members = items.First();
            members.ShouldTotal(2);
            members.ShouldContainNode("Member1");
            members.ShouldContainNode("Member2");
        }

        public static object[] NonGenericEnumerableTypeCases = TestCases.Create(
            new ArrayList {
                new ConcreteType { Member1 = "", Member2 = "" },  
                new BaseType { Member1 = "" }
            })
            .AddType<ArrayList>()
            .AddType<IEnumerable>()
            .All;

        [Test]
        [TestCaseSource(nameof(NonGenericEnumerableTypeCases))]
        public void should_enumerate_actual_type_when_backed_by_a_specified_non_generic_enumerable(
            object enumerable, Type type)
        {
            var items = CreateNode(enumerable, type: type, 
                mode: Mode.Serialize).Cast<NodeBase>().ToList();

            var members = items.GetNode("ConcreteType");
            members.ShouldTotal(2);
            members.ShouldContainNode("Member1");
            members.ShouldContainNode("Member2");

            members = items.GetNode("BaseType");
            members.ShouldTotal(1);
            members.ShouldContainNode("Member1");
        }
        
        // Type Filtering

        [Test]
        public void should_exclude_type()
        {
            var children = CreateNode(
                    new ArrayList { new Tuple<string>(""), 5 },
                    Options.Create(x => x.ExcludeType<Tuple<string>>()), 
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("Int32");
        }

        [Test]
        public void should_exclude_type_by_filter()
        {
            var children = CreateNode(
                    new ArrayList { new Tuple<string>(""), 5 },
                    Options.Create(x => x.ExcludeTypesWhen((t, o) =>
                    {
                        o.ShouldNotBeNull();
                        return t.Type == typeof (Tuple<string>);
                    })),
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("Int32");
        }

        [Test]
        public void should_include_type_by_filter()
        {
            var children = CreateNode(
                    new ArrayList { new Tuple<string>(""), 5 },
                    Options.Create(x => x.IncludeTypesWhen((t, o) =>
                    {
                        o.ShouldNotBeNull();
                        return t.Type == typeof(Tuple<string>);
                    })),
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("TupleOfString");
        }
    }
}
