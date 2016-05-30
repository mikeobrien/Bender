using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class DictionaryNodeTests
    {
        public static DictionaryNode CreateNode(Options options = null, Mode mode = Mode.Deserialize, IValue value = null)
        {
            return CreateNodeOfType<Dictionary<string, object>>(null, options, mode, value);
        }

        public static DictionaryNode CreateNodeOfType<T>(T dictionary = null,
            Options options = null, Mode mode = Mode.Deserialize, IValue value = null) where T : class 
        {
            return CreateNode(dictionary, options, mode, typeof(T), value);
        }

        public static DictionaryNode CreateNode(object dictionary,
            Options options = null, Mode mode = Mode.Deserialize, Type type = null, IValue value = null)
        {
            return new DictionaryNode(new Context(options ?? Options.Create(), mode, "xml"), type?.Name,
                value ?? new SimpleValue(dictionary, (type ?? dictionary.GetType()).ToCachedType()), null, null);
        }

        [Test]
        public void should_be_of_node_type_object()
        {
            CreateNode().NodeType.ShouldEqual(NodeType.Object);
        }

        [Test]
        public void should_be_of_type_dictionary()
        {
            CreateNode().Type.ShouldEqual("dictionary");
        }

        [Test]
        public void should_return_object_from_inner_value()
        {
            var dictionary = new Dictionary<string, object>();
            var node = CreateNode(dictionary);
            node.Value.ShouldEqual(dictionary);
        }

        // Initialization

        [Test]
        public void should_initialize_source_value()
        {
            var node = CreateNode(value: new LazyValue(
                new SimpleValue(typeof(Dictionary<string, object>).ToCachedType()),
                () => new Dictionary<string, object>()));

            node.Source.As<LazyValue>().InnerValue.Instance.ShouldBeNull();
            node.Initialize();
            node.Source.As<LazyValue>().InnerValue.Instance.ShouldNotBeNull();
        }

        // Ancestors

        [Test]
        public void should_pass_ancestors_to_children_in_serialize_mode()
        {
            var parent = CreateNode(new Dictionary<string, string> 
                {{"item1", "oh"}, {"item2", "hai"}}, mode: Mode.Serialize);
            var children = parent.Cast<NodeBase>().ToList();
            children.ShouldTotal(2);
            children.ShouldAllMatch(x =>
                x.HasParent &&
                x.Parent == parent);
        }

        [Test]
        public void should_pass_ancestors_to_children_in_deserialize_mode()
        {
            var parent = CreateNode(new Dictionary<string, string>(), mode: Mode.Deserialize);
            parent.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("item1", NodeType.Value, Metadata.Empty, c),
                x => x.As<NodeBase>().Parent.ShouldBeSameAs(parent));
            parent.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("item2", NodeType.Value, Metadata.Empty, c),
                x => x.As<NodeBase>().Parent.ShouldBeSameAs(parent));
        }

        // Getting node by name

        [Test]
        public void should_get_node_by_name()
        {
            var node = CreateNodeOfType(new Dictionary<string, string> { { "oh", "hai" } }, mode: Mode.Serialize);

            node.GetNode("oh").Value.ShouldEqual("hai");
            node.GetNode("yada").ShouldBeNull();
        }

        // Adding items

        public static object[] AddNodeCases = TestCases.Create()
            .Add(new Dictionary<string, string>(), NodeType.Value, "oh", 1)
            .Add(new Dictionary<object, string>(), NodeType.Value, "oh", 1)
            .Add(new GenericDictionaryImpl<string, string>(), NodeType.Value, "oh", 1)
            .All;

        [Test]
        [TestCaseSource(nameof(AddNodeCases))]
        public void should_add_nodes(
            object dictionary, NodeType nodeType, string value1, int value2)
        {
            var parent = CreateNode(dictionary);
            parent.Add("item1", nodeType, Metadata.Empty, x => x.Value = value1);
            parent.Add("item2", nodeType, Metadata.Empty, x => x.Value = value2);
            var backingDictionary = GenericDictionaryAdapter.Create(dictionary);

            backingDictionary.Count.ShouldEqual(2);
            backingDictionary["item1"].ShouldEqual(value1);
            backingDictionary["item2"].ShouldEqual(value2.ToString());
        }

        [Test]
        public void should_fail_to_add_item_to_non_generic_dictionary()
        {
            Assert.Throws<TypeNotSupportedException>(() =>
                CreateNodeOfType(new Hashtable())
                .Add("yada", NodeType.Value, Metadata.Empty, x => { }))
                .Message.ShouldEqual("Non generic dictionary 'System.Collections.Hashtable' " +
                    "is not supported for deserialization. Only generic dictionaries can be deserialized.");
        }

        // Inserting items

        [Test]
        [TestCase(typeof(Dictionary<string, Node>))]
        [TestCase(typeof(Dictionary<string, INode>))]
        public void should_insert_node(Type type)
        {
            var dictionary = type.CreateInstance().As<IDictionary>();
            var parent = CreateNode(dictionary);
            var child = Node.CreateValue("hai");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            dictionary.Count.ShouldEqual(1);
            dictionary["hai"].ShouldBeSameAs(child);
        }

        [Test]
        public void should_insert_node_backed_by_generic_dictionary_implementation()
        {
            var dictionary = new GenericDictionaryImpl<string, INode>();
            var parent = CreateNodeOfType(dictionary);
            var child = Node.CreateValue("hai");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            dictionary.Count.ShouldEqual(1);
            dictionary["hai"].ShouldBeSameAs(child);
        }

        [Test]
        public void should_not_insert_node_if_node_types_dont_match_but_indicate_that_it_can_insert_it()
        {
            var dictionary = new Dictionary<string, JsonNode>();
            var parent = CreateNodeOfType(dictionary);
            var child = Node.CreateValue("hai");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));

            dictionary.Count.ShouldEqual(0);
        }

        [Test]
        public void should_fail_to_insert_an_unamed_node()
        {
            var parent = CreateNodeOfType(new Dictionary<string, INode>());
            var child = Node.CreateValue();

            Assert.Throws<UnnamedChildrenNotSupportedException>(() => parent.Add(child, x => { }));
        }

        // Enumeration

        [Test]
        [TestCase(typeof(int), NodeType.Value)]
        [TestCase(typeof(List<int>), NodeType.Array)]
        [TestCase(typeof(object), NodeType.Object)]
        public void should_enumerate_nodes_backed_by_hashtable(Type type, NodeType nodeType)
        {
            var item1 = type.CreateInstance();
            var item2 = type.CreateInstance();
            var children = CreateNodeOfType(new Hashtable
            {
                { "oh", item1 }, 
                { "hai", item2 }
            }, mode: Mode.Serialize).Cast<NodeBase>().OrderBy(x => x.Name).ToList();

            children.ShouldTotal(2);

            var child = children.First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual("hai");
            child.Value.ShouldEqual(item2);

            child = children.Skip(1).First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual("oh");
            child.Value.ShouldEqual(item1);
        }

        public class Name
        {
            private readonly string _name;
            public Name(string name) { _name = name; }
            public override string ToString() { return _name; }
        }

        public static object[] GenericDictionaryStringKeysCases = TestCases.Create()
            .Add(new Dictionary<string, int> { { "oh", 1 }, { "hai", 2 } }, NodeType.Value)
            .Add(new Dictionary<string, List<int>> { { "oh", new List<int>() }, { "hai", new List<int>() } }, NodeType.Array)
            .Add(new Dictionary<string, object> { { "oh", new object() }, { "hai", new object() } }, NodeType.Object)
            .Add(new Dictionary<Name, int> { { new Name("oh"), 1 }, { new Name("hai"), 2 } }, NodeType.Value)
            .Add(new Dictionary<Name, List<int>> { { new Name("oh"), new List<int>() }, { new Name("hai"), new List<int>() } }, NodeType.Array)
            .Add(new Dictionary<Name, object> { { new Name("oh"), new object() }, { new Name("hai"), new object() } }, NodeType.Object)
            .Add(new GenericDictionaryImpl<string, string> { { "oh", "1" }, { "hai", "2" } }, NodeType.Value)
            .All;

        [Test]
        [TestCaseSource(nameof(GenericDictionaryStringKeysCases))]
        public void should_enumerate_nodes_backed_by_generic_dictionary(
            object dictionary, NodeType nodeType)
        {
            var nonGenericDictionary = GenericDictionaryAdapter.Create(dictionary);
            Func<string, object> getKey = x => nonGenericDictionary
                .Keys.Cast<object>().First(y => y.ToString() == x);

            var children = CreateNode(dictionary, mode: Mode.Serialize).Cast<NodeBase>().ToList();

            children.ShouldTotal(2);

            var child = children.First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual("oh");
            child.Value.ShouldEqual(nonGenericDictionary[getKey("oh")]);

            child = children.Skip(1).First();
            child.NodeType.ShouldEqual(nodeType);
            child.Name.ShouldEqual("hai");
            child.Value.ShouldEqual(nonGenericDictionary[getKey("hai")]);
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(Dictionary<string, INode>))]
        [TestCase(typeof(Dictionary<string, Node>))]
        public void should_enumerate_node_instances(Type type)
        {
            var dictionary = type.CreateInstance().As<IDictionary>();
            var child1 = Node.CreateValue("item1");
            var child2 = Node.CreateValue("item2");
            dictionary.Add(child1.Name, child1);
            dictionary.Add(child2.Name, child2);
            var children = CreateNodeOfType(dictionary, mode: Mode.Serialize).ToList();

            children.ShouldTotal(2);

            children.GetNode("item1").ShouldBeSameAs(child1);
            children.GetNode("item2").ShouldBeSameAs(child2);
        }

        public class CyclicRoot
        {
            public Hashtable Hashtable { get; set; }
            public Dictionary<string, CyclicRoot> Dictionary { get; set; }
            public Dictionary<string, object> ObjectDictionary { get; set; }
        }

        public static object[] CyclicCases = TestCases.Create(new CyclicRoot())
            .Add(x => x[0].As<CyclicRoot>().Hashtable = new Hashtable 
                { { "item1", x[0] }, { "item2", new CyclicRoot() } }, "Hashtable")
            .Add(x => x[0].As<CyclicRoot>().Dictionary = new Dictionary<string, CyclicRoot> 
                { { "item1", x[0].As<CyclicRoot>() }, { "item2", new CyclicRoot() } }, "Dictionary")
            .Add(x => x[0].As<CyclicRoot>().ObjectDictionary = new Dictionary<string, object> 
                { { "item1", x[0] }, { "item2", new CyclicRoot() } }, "ObjectDictionary")
            .All;
        
        [Test]
        [TestCaseSource(nameof(CyclicCases))]
        public void should_not_return_cyclic_references_in_serialize_mode(CyclicRoot root, string name)
        {
            var members = new ObjectNode(new Context(Options.Create(), Mode.Serialize, "xml"), null,
                new SimpleValue(root, typeof(CyclicRoot).ToCachedType()), null, null).ToList();

            var child = members.GetNode(name);
            child.ShouldTotal(1);
            child.ShouldNotContain(x => x.Name == "item1");
            child.ShouldContainNode("item2");
        }

        [Test]
        public void should_not_return_null_values()
        {
            var parent = CreateNode(new Dictionary<string, string> 
                { { "item1", "oh" }, { "item2", null } }, mode: Mode.Serialize);
            var children = parent.Cast<NodeBase>().ToList();
            children.ShouldTotal(1);
            children.ShouldContainNode("item1");
        }

        [Test]
        public void should_not_return_items_in_deserialize_mode()
        {
            CreateNodeOfType(new Dictionary<string, string>{{"oh", "hai"}}, 
                mode: Mode.Deserialize).ShouldBeEmpty();
        }

        [Test]
        [TestCase(typeof(Dictionary<string, INode>))]
        [TestCase(typeof(Dictionary<string, Node>))]
        public void should_return_node_value_when_a_node_implementation(Type type)
        {
            var instance = type.CreateInstance().As<IDictionary>();
            var child = new Node("name");
            instance.Add("key", child);
            var parent = CreateNode(instance, mode: Mode.Serialize);
            parent.GetNode("name").ShouldBeSameAs(child);
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

        public static object[] GenericDictionaryTypeCases = TestCases.Create()
            .AddType(new Dictionary<string, IInterface> { { "item", new ConcreteType { Member1 = "oh", Member2 = "hai" } }})
            .AddType(new Dictionary<string, BaseType> { { "item", new ConcreteType { Member1 = "oh", Member2 = "hai" } } })
            .AddType<IDictionary<string, IInterface>>(new Dictionary<string, IInterface> { { "item", new ConcreteType { Member1 = "oh", Member2 = "hai" } } })
            .AddType<IDictionary<string, IInterface>>(new Dictionary<string, BaseType> { { "item", new ConcreteType { Member1 = "oh", Member2 = "hai" } } })
            .All;

        [Test]
        [TestCaseSource(nameof(GenericDictionaryTypeCases))]
        public void should_enuerate_specified_type_when_backed_by_a_specified_generic_dictionary(
            Type type, object dictionary)
        {
            var items = CreateNode(dictionary, type: type,
                mode: Mode.Serialize).Cast<NodeBase>().ToList();

            var members = items.GetNode("item");
            members.ShouldTotal(1);
            members.ShouldContainNode("Member1");
        }

        [Test]
        [TestCaseSource(nameof(GenericDictionaryTypeCases))]
        public void should_enuerate_actual_type_when_configured_and_backed_by_a_specified_generic_dictionary(
            Type type, object dictionary)
        {
            var items = CreateNode(dictionary,
                Options.Create(x => x.Serialization(y => y.UseActualType())),
                Mode.Serialize, type).Cast<NodeBase>().ToList();

            var members = items.GetNode("item");
            members.ShouldTotal(2);
            members.ShouldContainNode("Member1");
            members.ShouldContainNode("Member2");
        }

        public static object[] NonGenericDictionaryTypeCases = TestCases.Create(
            new Hashtable { { "item1", new ConcreteType { Member1 = "", Member2 = "" } },
                            { "item2", new BaseType { Member1 = "" } } })
            .AddType<Hashtable>()
            .AddType<IDictionary>()
            .All;

        [Test]
        [TestCaseSource(nameof(NonGenericDictionaryTypeCases))]
        public void should_enumerate_actual_type_when_backed_by_a_specified_non_generic_dictionary(
            object dictionary, Type type)
        {
            var items = CreateNode(dictionary, type: type, 
                mode: Mode.Serialize).Cast<NodeBase>().ToList();

            var members = items.GetNode("item1");
            members.ShouldTotal(2);
            members.ShouldContainNode("Member1");
            members.ShouldContainNode("Member2");

            members = items.GetNode("item2");
            members.ShouldTotal(1);
            members.ShouldContainNode("Member1");
        }

        // Type Filtering

        [Test]
        public void should_exclude_type()
        {
            var children = CreateNode(
                    new Hashtable { { "item1", new Tuple<string>("") }, { "item2", 5 } },
                    Options.Create(x => x.ExcludeType<Tuple<string>>()),
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("item2");
        }

        [Test]
        public void should_exclude_type_by_filter()
        {
            var children = CreateNode(
                    new Hashtable { { "item1", new Tuple<string>("") }, { "item2", 5 } },
                    Options.Create(x => x.ExcludeTypesWhen((t, o) =>
                    {
                        o.ShouldNotBeNull();
                        return t.Type == typeof(Tuple<string>);
                    })),
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("item2");
        }

        [Test]
        public void should_include_type_by_filter()
        {
            var children = CreateNode(
                    new Hashtable { { "item1", new Tuple<string>("") }, { "item2", 5 } },
                    Options.Create(x => x.IncludeTypesWhen((t, o) =>
                    {
                        o.ShouldNotBeNull();
                        return t.Type == typeof(Tuple<string>);
                    })),
                    Mode.Serialize)
                .Cast<NodeBase>().ToList();

            children.ShouldTotal(1);
            children.First().Name.ShouldEqual("item1");
        }
    }
}
