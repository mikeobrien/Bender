using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ObjectNodeTests
    {
        public class MemberAccessModifierEnumeration
        {
            private string _privateField;
            protected string ProtectedField;
            public string PublicField;

            private string PrivateProperty { get; set; }
            protected string ProtectedProperty { get; set; }
            public string PublicProperty { get; set; }
        }

        public class MemberAccessibilityEnumeration
        {
            public readonly string ReadonlyField = "yada";
            public string ReadonlyProperty => "yada";
            public string WriteonlyProperty { set { } }
        }

        public static ObjectNode CreateAccessModifierNode(Options options = null, IValue value = null, Mode mode = Mode.Deserialize)
        {
            return new ObjectNode(new Context(options ?? Options.Create(), mode, "xml"), null,
                value ?? new SimpleValue(typeof(MemberAccessModifierEnumeration).ToCachedType()), null, null);
        }

        public static ObjectNode CreateAccessibilityNode(Options options, Mode mode)
        {
            var value = new SimpleValue(new MemberAccessibilityEnumeration(), 
                typeof(MemberAccessibilityEnumeration).ToCachedType());
            return new ObjectNode(new Context(options ?? Options.Create(), 
                mode, "xml"), null, value, null, null);
        }

        [Test]
        public void should_be_of_node_type_object()
        {
            CreateAccessModifierNode().NodeType.ShouldEqual(NodeType.Object);
        }

        [Test]
        public void should_be_of_type_object()
        {
            CreateAccessModifierNode().Type.ShouldEqual("object");
        }

        [Test]
        public void should_return_object_from_inner_value()
        {
            var @object = new MemberAccessModifierEnumeration();
            var node = CreateAccessModifierNode(value: new SimpleValue(@object, typeof(MemberAccessModifierEnumeration).ToCachedType()));
            node.Value.ShouldBeSameAs(@object);
        }

        [Test]
        public void should_pass_ancestors_to_children()
        {
            var parent = CreateAccessModifierNode();
            var children = parent.Cast<NodeBase>().ToList();
            children.ShouldTotal(1);
            children.ShouldAllMatch(x =>
                x.HasParent &&
                x.Parent == parent);
        }

        // Initialization

        public class InitializeSource { }

        [Test]
        public void should_initialize_source_value()
        {
            var node = CreateAdditionNode(value: new LazyValue(
                new SimpleValue(typeof(InitializeSource).ToCachedType()),
                () => new InitializeSource()));

            node.Source.As<LazyValue>().InnerValue.Instance.ShouldBeNull();
            node.Initialize();
            node.Source.As<LazyValue>().InnerValue.Instance.ShouldNotBeNull();
        }

        // Enumeration

        [Test]
        public void should_enumerate_only_public_properties_by_default()
        {
            var nodes = CreateAccessModifierNode().ToList();
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("PublicProperty");
        }

        [Test]
        public void should_enumerate_all_properties_when_configured()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.IncludeNonPublicProperties())).ToList();
            nodes.ShouldTotal(3);
            nodes.ShouldContainNode("PublicProperty");
            nodes.ShouldContainNode("ProtectedProperty");
            nodes.ShouldContainNode("PrivateProperty");
        }

        [Test]
        public void should_enumerate_public_properties_and_public_fields_when_configured()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.IncludePublicFields())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("PublicProperty");
            nodes.ShouldContainNode("PublicField");
        }

        [Test]
        public void should_enumerate_public_properties_and_non_public_fields_when_configured()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.IncludeNonPublicFields())).ToList();
            nodes.ShouldTotal(3);
            nodes.ShouldContainNode("PublicProperty");
            nodes.ShouldContainNode("ProtectedField");
            nodes.ShouldContainNode("_privateField");
        }

        [Test]
        public void should_enumerate_all_properties_and_fields_when_configured()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x
                .IncludePublicFields()
                .IncludeNonPublicFields()
                .IncludeNonPublicProperties())).ToList();
            nodes.ShouldTotal(6);
            nodes.ShouldContainNode("PublicProperty");
            nodes.ShouldContainNode("ProtectedProperty");
            nodes.ShouldContainNode("PrivateProperty");
            nodes.ShouldContainNode("PublicField");
            nodes.ShouldContainNode("ProtectedField");
            nodes.ShouldContainNode("_privateField");
        }

        [Test]
        public void should_include_members_with_a_custom_filter()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => 
                x.IncludeMembersWhen((m, o) => m.Name.Length == 14)
                 .IncludeNonPublicFields())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("PublicProperty");
            nodes.ShouldContainNode("ProtectedField");
        }

        [Test]
        public void should_exclude_readonly_members_when_deserializing()
        {
            var nodes = CreateAccessibilityNode(Options.Create(x => x
                .IncludePublicFields()), mode: Mode.Deserialize).ToList();

            nodes.ShouldNotContainNode("ReadonlyField");
            nodes.ShouldNotContainNode("ReadonlyProperty");

            nodes.ShouldContainNode("WriteonlyProperty");
        }

        [Test]
        public void should_exclude_writeonly_members_when_serializing()
        {
            var nodes = CreateAccessibilityNode(Options.Create(x => x
                .IncludePublicFields()), mode: Mode.Serialize).ToList();

            nodes.ShouldNotContainNode("WriteonlyProperty");

            nodes.ShouldContainNode("ReadonlyField");
            nodes.ShouldContainNode("ReadonlyProperty");
        }

        [Test]
        public void should_exclude_members_with_a_custom_filter()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x =>
                x.ExcludeMembersWhen((m, o) => m.Name.Contains("Pr"))
                 .IncludePublicFields())).ToList();
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("PublicField");
        }

        [Test]
        public void should_enumerate_value_on_anonymous_type()
        {
            var @object = new { Oh = "hai" };
            var nodes = CreateAccessModifierNode(
                value: new SimpleValue(@object, @object
                    .GetType().ToCachedType()), 
                mode: Mode.Serialize).ToList();
            nodes.ShouldTotal(1);
            nodes[0].Name.ShouldEqual("Oh");
            nodes[0].NodeType.ShouldEqual(NodeType.Value);
            nodes[0].Value.ShouldEqual("hai");
        }

        public class ExcludedMember
        {
            [XmlIgnore]
            public string Excluded { get; set; }
            public string Included { get; set; }
        }

        [Test]
        public void should_exclude_members_with_xml_ignore_attribute_applied()
        {
            var nodes = CreateAccessModifierNode(value: new SimpleValue(new ExcludedMember(), typeof(ExcludedMember).ToCachedType())).ToList();
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("Included");
        }

        public class IndexerMember
        {
            public string Property { get; set; }
            public string Field;
            public string this[int i] { get { return null; } set { } }
        }

        [Test]
        public void should_exclude_indexers()
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.IncludePublicFields()),
                new SimpleValue(new IndexerMember(), typeof(IndexerMember).ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        [Test]
        [TestCase(typeof(Hashtable), typeof(IDictionary))]
        [TestCase(typeof(Dictionary<string, string>), typeof(IDictionary<string, string>))]
        [TestCase(typeof(ArrayList), typeof(IEnumerable))]
        [TestCase(typeof(List<string>), typeof(IEnumerable<string>))]
        [TestCase(typeof(List<string>), typeof(IList))]
        [TestCase(typeof(List<string>), typeof(IList<string>))]
        public void should_exclude_collection_interface_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(0);
        }

        public class Dictionary : DictionaryImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedDictionary : Dictionary { }

        [Test]
        [TestCase(typeof(InheritedDictionary), typeof(InheritedDictionary))]
        [TestCase(typeof(Dictionary), typeof(Dictionary))]
        public void should_exclude_idictionary_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class GenericDictionary : GenericStringDictionaryImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedGenericDictionary : GenericDictionary { }

        [Test]
        [TestCase(typeof(InheritedGenericDictionary), typeof(InheritedGenericDictionary))]
        [TestCase(typeof(GenericDictionary), typeof(GenericDictionary))]
        public void should_exclude_generic_idictionary_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatDictionaryImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class Enumerable : EnumerableImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedEnumerable : Enumerable { }

        [Test]
        [TestCase(typeof(InheritedEnumerable), typeof(InheritedEnumerable))]
        [TestCase(typeof(Enumerable), typeof(Enumerable))]
        public void should_exclude_ienumerable_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatEnumerableImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class GenericEnumerable : GenericStringEnumerableImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedGenericEnumerable : GenericEnumerable { }

        [Test]
        [TestCase(typeof(InheritedGenericEnumerable), typeof(InheritedGenericEnumerable))]
        [TestCase(typeof(GenericEnumerable), typeof(GenericEnumerable))]
        public void should_exclude_generic_ienumerable_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatEnumerableImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class List : ListImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedList : List { }

        [Test]
        [TestCase(typeof(InheritedList), typeof(InheritedList))]
        [TestCase(typeof(List), typeof(List))]
        public void should_exclude_ilist_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatEnumerableImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class GenericList : GenericStringListImpl
        {
            public string Property { get; set; }
            public string Field;
        }

        public class InheritedGenericList : GenericList { }

        [Test]
        [TestCase(typeof(InheritedGenericList), typeof(InheritedGenericList))]
        [TestCase(typeof(GenericList), typeof(GenericList))]
        public void should_exclude_generic_ilist_members(Type actualType, Type specifiedType)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.TreatEnumerableImplsAsObjects().IncludePublicFields()),
                new SimpleValue(actualType.CreateInstance(), specifiedType.ToCachedType())).ToList();
            nodes.ShouldTotal(2);
            nodes.ShouldContainNode("Property");
            nodes.ShouldContainNode("Field");
        }

        public class NodeMembers
        {
            public INode NodeInterface { get; set; }
            public Node NodeImplementation { get; set; }
        }

        [Test]
        [TestCase("NodeInterface")]
        [TestCase("NodeImplementation")]
        public void should_return_node_value_when_a_node_implementation(string member)
        {
            var instance = new NodeMembers();
            var child = new Node("yada");
            instance.SetPropertyOrFieldValue(member, child);
            var parent = CreateAccessModifierNode(Options.Create(),
                new SimpleValue(instance, typeof (NodeMembers).ToCachedType()));
            parent.GetNode("yada").ShouldBeSameAs(child);
        }

        // Indexer

        [Test]
        public void should_get_node_by_name()
        {
            var @object = new { Oh = "hai" };
            CreateAccessModifierNode(value: new SimpleValue(@object,
                @object.GetType().ToCachedType()), mode: Mode.Serialize)
                .GetNode("Oh").Value.ShouldEqual("hai");
        }

        [Test]
        public void should_return_null_when_there_is_no_match()
        {
            var @object = new { Oh = "hai" };
            CreateAccessModifierNode(value: new SimpleValue(@object, 
                @object.GetType().ToCachedType()), mode: Mode.Serialize)
                .GetNode("yada").ShouldBeNull();
        }

        [Test]
        public void should_get_node_by_name_when_case_does_no_match_and_configured()
        {
            var @object = new { Oh = "hai" };
            CreateAccessModifierNode(Options.Create(x => x.Deserialization(y => y.IgnoreNameCase())),
                new SimpleValue(@object, @object.GetType().ToCachedType()), Mode.Serialize)
                .GetNode("Oh").Value.ShouldEqual("hai");
        }

        [Test]
        public void should_fail_to_get_node_by_name_when_case_does_no_match()
        {
            var @object = new { Oh = "hai" };
            CreateAccessModifierNode(value: new SimpleValue(@object, @object.GetType().ToCachedType())).GetNode("oh").ShouldBeNull();
        }

        // Metadata

        public class MemberMetadata
        {
            public string EmptyProperty { get; set; }
            [XmlAttribute]
            public string Property { get; set; }

            public string EmptyField;
            [XmlAttribute]
            public string Field { get; set; }
        }

        [Test]
        [TestCase("EmptyProperty", false)]
        [TestCase("Property", true)]
        [TestCase("EmptyField", false)]
        [TestCase("Field", true)]
        public void should_return_attribute_metadata(string name, bool exists)
        {
            var children = new ObjectNode(new Context(Options.Create(x => x.IncludePublicFields()), 
                Mode.Serialize, "xml"), null, new SimpleValue(new MemberMetadata 
                { EmptyProperty = "", Property = "", Field = "", EmptyField = "" }, 
                typeof(MemberMetadata).ToCachedType()), null, null);
                
            children.GetNode(name).Metadata.Contains<XmlAttributeAttribute>().ShouldEqual(exists);
        }

        // Actual vs specified type enumeration

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

        private static readonly ConcreteType ConcreteTypeInstance = 
            new ConcreteType { Member1 = "", Member2 = "" };

        [Test]
        [TestCase(typeof(IInterface), Mode.Deserialize, 1)]
        [TestCase(typeof(BaseType), Mode.Deserialize, 1)]
        [TestCase(typeof(ConcreteType), Mode.Deserialize, 2)]
        [TestCase(typeof(IInterface), Mode.Serialize, 1)]
        [TestCase(typeof(BaseType), Mode.Serialize, 1)]
        [TestCase(typeof(ConcreteType), Mode.Serialize, 2)]
        public void should_enumerate_members_on_specified_type(Type type, Mode mode, int count)
        {
            var nodes = CreateAccessModifierNode(value: new SimpleValue(ConcreteTypeInstance, type.ToCachedType()), 
                mode: mode).ToList();
            
            nodes.ShouldTotal(count);
        }

        public class Model
        {
            public IInterface Interface { get; set; }
            public BaseType BaseType { get; set; }
            public ConcreteType ConcreteType { get; set; }
        }

        private static readonly Model ModelInstance = new Model
        {
            Interface = ConcreteTypeInstance,
            BaseType = ConcreteTypeInstance,
            ConcreteType = ConcreteTypeInstance
        };

        [Test]
        [TestCase("Interface")]
        [TestCase("BaseType")]
        [TestCase("ConcreteType")]
        public void should_enumerate_member_actual_type_in_serialize_mode_when_configured(
            string name)
        {
            var nodes = CreateAccessModifierNode(
                    Options.Create(x => x.Serialization(y => y.UseActualType())),
                    new SimpleValue(ModelInstance, typeof(Model).ToCachedType()),
                    Mode.Serialize
                );

            nodes.GetNode(name).ShouldTotal(2);
        }

        [Test]
        [TestCase("Interface", Mode.Serialize, SerializationType.SpecifiedType, 1)]
        [TestCase("BaseType", Mode.Serialize, SerializationType.SpecifiedType, 1)]
        [TestCase("ConcreteType", Mode.Serialize, SerializationType.SpecifiedType, 2)]
        [TestCase("Interface", Mode.Deserialize, SerializationType.ActualType, 1)]
        [TestCase("BaseType", Mode.Deserialize, SerializationType.ActualType, 1)]
        [TestCase("ConcreteType", Mode.Deserialize, SerializationType.ActualType, 2)]
        [TestCase("Interface", Mode.Deserialize, SerializationType.SpecifiedType, 1)]
        [TestCase("BaseType", Mode.Deserialize, SerializationType.SpecifiedType, 1)]
        [TestCase("ConcreteType", Mode.Deserialize, SerializationType.SpecifiedType, 2)]
        public void should_enumerate_member_specified_type_when_in_serialize_mode_and_configured_or_when_in_deserialize_mode(
            string name, Mode mode, SerializationType type, int count)
        {
            var nodes = CreateAccessModifierNode(Options.Create(x => x.Serialization(y => 
                { if (type == SerializationType.ActualType) y.UseActualType(); })),
                new SimpleValue(ModelInstance, typeof(Model).ToCachedType()), mode).ToList();

            nodes.GetNode(name).ShouldTotal(count);
        }

        // Serialize mode enumeration

        public class NullMembers
        {
            public string Property { get; set; }
            public string NullProperty { get; set; }
        }

        [Test]
        public void should_not_return_null_members_in_serialize_mode()
        {
            var @object = new NullMembers { Property = "hai" };
            var members = CreateAccessModifierNode(value: new SimpleValue(@object, typeof(NullMembers).ToCachedType()), 
                mode: Mode.Serialize).ToList();

            members.ShouldTotal(1);
            members.ShouldNotContainNode("NullProperty");
            members.ShouldContainNode("Property");
        }

        public class CyclicRoot
        {
            public CyclicChild NonCyclicProperty { get; set; }
            public CyclicRoot CyclicProperty { get; set; }
        }

        public class CyclicChild
        {
            public string NonCyclicProperty { get; set; }
            public CyclicRoot CyclicProperty { get; set; }
        }

        [Test]
        public void should_not_return_cyclic_references_in_serialize_mode()
        {
            var @object = new CyclicRoot();
            @object.CyclicProperty = @object;
            @object.NonCyclicProperty = new CyclicChild
            {
                NonCyclicProperty = "hai", 
                CyclicProperty = @object
            };

            var members = CreateAccessModifierNode(value: new SimpleValue(@object, typeof(CyclicRoot).ToCachedType()), 
                mode: Mode.Serialize).ToList();

            members.ShouldTotal(1);
            members.ShouldNotContainNode("CyclicProperty");

            var child = members.GetNode("NonCyclicProperty");
            child.ShouldNotBeNull();

            members = child.ToList();

            members.ShouldTotal(1);
            members.ShouldNotContainNode("CyclicProperty");

            child = members.GetNode("NonCyclicProperty");
            child.Value.ShouldEqual("hai");
        }

        // Type filtering

        public class TypeFiltering
        {
            public string String { get; set; }
            public int Int { get; set; }
        }

        private static readonly TypeFiltering TypeFilteringInstance =
            new TypeFiltering {String = ""};

        [Test]
        [TestCase(Mode.Deserialize), TestCase(Mode.Serialize)]
        public void should_exclude_type(Mode mode)
        {
            var nodes = CreateAccessModifierNode(
                Options.Create(x => x.ExcludeType<string>()),
                new SimpleValue(TypeFilteringInstance, typeof(TypeFiltering).ToCachedType()), mode).ToList();
        
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("Int");
        }

        [Test]
        [TestCase(Mode.Deserialize), TestCase(Mode.Serialize)]
        public void should_exclude_type_by_filter(Mode mode)
        {
            var nodes = CreateAccessModifierNode(
                Options.Create(x => x.ExcludeTypesWhen((t, o) =>
                {
                    o.ShouldNotBeNull();
                    return t.Type == typeof(string);
                })),
                new SimpleValue(TypeFilteringInstance, typeof(TypeFiltering).ToCachedType()), mode).ToList();
                
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("Int");
        }

        [Test]
        [TestCase(Mode.Deserialize), TestCase(Mode.Serialize)]
        public void should_include_type_by_filter(Mode mode)
        {
            var nodes = CreateAccessModifierNode(
                Options.Create(x => x.IncludeTypesWhen((t, o) =>
                {
                    o.ShouldNotBeNull();
                    return t.Type == typeof(string);
                })),
                new SimpleValue(TypeFilteringInstance, typeof(TypeFiltering).ToCachedType()), mode).ToList();
                
            nodes.ShouldTotal(1);
            nodes.ShouldContainNode("String");
        }

        // Adding nodes

        public class NodeAddition
        {
            public string Oh { get; set; }
            public string Hai { get; set; }
        }

        public static ObjectNode CreateAdditionNode(Options options = null, IValue value = null, Mode mode = Mode.Deserialize)
        {
            return new ObjectNode(new Context(options ?? Options.Create(), mode, "xml"), null,
                value ?? new SimpleValue(typeof(NodeAddition).ToCachedType()), null, null);
        }

        [Test]
        public void should_add_nodes()
        {
            var node = CreateAdditionNode();

            node.ShouldExecuteCallback<INode>(
                (n, c) => n.Add("Oh", NodeType.Value, Metadata.Empty, c), 
                c => c.ShouldBeType<ValueNode>());

            node.ShouldExecuteCallback<INode>(
                (n, c) => n.Add("Hai", NodeType.Value, Metadata.Empty, c),
                c => c.ShouldBeType<ValueNode>());
        }

        [Test]
        public void should_successfully_validate_node()
        {
            var node = CreateAdditionNode();

            node.Add("Oh", NodeType.Value, Metadata.Empty, x => { });
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => { });

            Assert.DoesNotThrow(node.Validate);
        }

        [Test]
        public void should_not_fail_to_add_element_unmatched_by_default()
        {
            Assert.DoesNotThrow(() =>
                CreateAdditionNode().Add("yada", NodeType.Value, Metadata.Empty, x => { }));
        }

        [Test]
        public void should_fail_to_add_unmatched_element_if_configured()
        {
            var node = CreateAdditionNode(Options.Create(
                x => x.Deserialization(y => y.FailOnUnmatchedElements())));
            var exception = Assert.Throws<UnrecognizedNodeDeserializationException>(() => 
                node.Add(new Node("yada") { NodeType = NodeType.Value, Type = "element" }, x => { }));

            const string message = "Element 'yada' is not recognized.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_not_add_case_insensitively_when_configured()
        {
            var exception = Assert.Throws<UnrecognizedNodeDeserializationException>(() =>
                CreateAdditionNode(Options.Create(x =>
                    x.Deserialization(y => y.FailOnUnmatchedElements())))
                        .Add(new Node("yada") { NodeType = NodeType.Value, Type = "element" }, x => { }));

            const string message = "Element 'yada' is not recognized.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_successfully_add_and_validate_case_insensitively_when_configured()
        {
            var node = CreateAdditionNode(Options.Create(x =>
                x.Deserialization(y => y.IgnoreNameCase().FailOnUnmatchedElements())));
            node.Add("oh", NodeType.Value, Metadata.Empty, x => { });
            node.Add("hai", NodeType.Value, Metadata.Empty, x => { });

            Assert.DoesNotThrow(node.Validate);
        }

        [Test]
        public void should_fail_if_not_all_nodes_were_added_and_configured_to_do_so()
        {
            var node = CreateAdditionNode(Options.Create(x => x.Deserialization(y => y.FailOnUnmatchedMembers())));
            node.Add(new Node("yada") { NodeType = NodeType.Value, Type = "element" }, x => { });

            var exception = Assert.Throws<MissingNodeDeserializationException>(node.Validate);

            const string message = "The following children were expected but not found: 'Oh', 'Hai'.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual(message);
        }

        [Test]
        public void should_fail_if_all_nodes_were_not_added_and_in_serialize_mode()
        {
            var node = CreateAdditionNode(Options.Create(x => 
                x.Deserialization(y => y.FailOnUnmatchedMembers())),
                mode: Mode.Serialize);

            Assert.DoesNotThrow(node.Validate);
        }

        [Test]
        public void should_not_fail_if_all_nodes_were_not_added_and_configured_to_do_so()
        {
            var node = CreateAdditionNode();
            node.Add("Oh", NodeType.Value, Metadata.Empty, x => { });

            Assert.DoesNotThrow(node.Validate);
        }

        // Inserting nodes

        public static ObjectNode CreateInsertionNode(Options options = null, IValue value = null, Mode mode = Mode.Deserialize)
        {
            return new ObjectNode(new Context(options ?? Options.Create(), mode, "xml"), null,
                value ?? new SimpleValue(typeof(MemberAccessModifierEnumeration).ToCachedType()), null, null);
        }

        public class NodeInsertion
        {
            public int NotANode { get; set; }
            public INode NodeInterface { get; set; }
            public Node Node { get; set; }
            public JsonNode JsonNode { get; set; }
        }

        [Test]
        public void should_insert_any_node_into_an_inode_member()
        {
            var instance = new NodeInsertion();
            var parent = CreateAccessModifierNode(value: new SimpleValue(instance, typeof(NodeInsertion).ToCachedType()));
            var child = new Node("NodeInterface");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));
            parent.GetNode("NodeInterface").ShouldBeSameAs(child);
            instance.NodeInterface.ShouldBeSameAs(child);
        }

        [Test]
        [TestCase(StringComparison.Ordinal, "NodeInterface")]
        [TestCase(StringComparison.OrdinalIgnoreCase, "NODEINTERFACE")]
        public void should_insert_a_node_matching_on_configured_case_comparison(
            StringComparison comparison, string name)
        {
            var instance = new NodeInsertion();
            var parent = CreateAccessModifierNode(Options.Create(x => x.Deserialization(y => y.WithNameComparison(comparison))),
                new SimpleValue(instance, typeof(NodeInsertion).ToCachedType()));
            var child = new Node(name);
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));
            parent.GetNode(name).ShouldBeSameAs(child);
            instance.NodeInterface.ShouldBeSameAs(child);
        }

        [Test]
        public void should_insert_a_node_into_a_member_of_the_same_type()
        {
            var instance = new NodeInsertion();
            var parent = CreateAccessModifierNode(value: new SimpleValue(instance, typeof(NodeInsertion).ToCachedType())); 
            var child = new Node("Node");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));
            parent.GetNode("Node").ShouldBeSameAs(child);
            instance.Node.ShouldBeSameAs(child);
        }

        [Test]
        public void should_ignore_a_node_that_implements_inode_but_not_the_same_type_as_the_member()
        {
            var instance = new NodeInsertion();
            var parent = CreateAccessModifierNode(value: new SimpleValue(instance, typeof(NodeInsertion).ToCachedType()));
            var child = new Node("JsonNode");
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(child, c));
            var jsonNode = parent.GetNode("JsonNode");
            jsonNode.ShouldNotBeNull();
            jsonNode.ShouldNotBeSameAs(child);
            jsonNode.NodeType.ShouldEqual(NodeType.Value);
            jsonNode.Value.ShouldBeNull();
            instance.JsonNode.ShouldBeNull();
        }
    }
}
