using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Xml;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Xml
{
    [TestFixture]
    public class ElementNodeTests
    {
        private const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        // Parse Errors

        [Test]
        public void should_fail_on_unclosed_outer_object()
        {
            var exception = Assert.Throws<ParseException>(() => ElementNode.Parse("<yada>", Options));
#if __MonoCS__
                exception.Message.ShouldEqual("1 missing end of arrays or objects (1,2)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse xml: 1 missing end of arrays or objects (1,2)");
#else
            const string message = "Unexpected end of file has occurred. The following elements are not closed: yada. Line 1, position 7.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual("Unable to parse xml: " + message);
#endif
        }

        [Test]
        public void should_fail_on_missing_token()
        {
            var exception = Assert.Throws<ParseException>(() => ElementNode.Parse("<oh><hai></oh> }", Options));
#if __MonoCS__
                exception.Message.ShouldEqual("':' is expected after a name of an object content (1,10)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse xml: ':' is expected after a name of an object content (1,10)");
#else
            const string message = "The 'hai' start tag on line 1 position 6 does not match the end tag of 'oh'. Line 1, position 12.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual("Unable to parse xml: " + message);
#endif
        }

        [Test]
        public void should_fail_on_unclosed_nested_array()
        {
            var exception = Assert.Throws<ParseException>(() => ElementNode.Parse("<yada", Options));
#if __MonoCS__
                exception.Message.ShouldEqual("Unexpected end of object (1,13)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse xml: Unexpected end of object (1,13)");
#else
            const string message = "Unexpected end of file while parsing Name has occurred. Line 1, position 6.";
            exception.Message.ShouldEqual(message);
            exception.FriendlyMessage.ShouldEqual("Unable to parse xml: " + message);
#endif
        }

        // Misc

        private static readonly Options Options = Options.Create();
        private static readonly XmlNodeBase Node = ElementNode.Create("Yada", Metadata.Empty, Options);

        [Test]
        public void should_return_element_type()
        {
            Node.XmlType.ShouldEqual(XmlObjectType.Element);
        }

        [Test]
        public void should_return_format()
        {
            Node.Format.ShouldEqual(XmlNodeBase.NodeFormat);
        }

        [Test]
        public void should_be_of_type_attribute()
        {
            Node.Type.ShouldEqual("element");
        }

        [Test]
        public void should_return_is_named()
        {
            Node.IsNamed.ShouldBeTrue();
        }

        [Test]
        public void should_return_path()
        {
            Node.Path.ShouldEqual("/Yada");
        }

        [Test]
        public void should_return_name()
        {
            var node = Node;
            node.IsNamed.ShouldBeTrue();
            node.Name.ShouldEqual("Yada");
        }

        [Test]
        public void should_return_node_type()
        {
            Node.NodeType.ShouldEqual(NodeType.Variable);
        }

        [Test]
        public void should_set_node_type()
        {
            var node = Node;
            node.HasFixedNodeType.ShouldBeFalse();
            node.NodeType.ShouldEqual(NodeType.Variable);
            node.NodeType = NodeType.Value;
            node.NodeType.ShouldEqual(NodeType.Value);
        }

        // Empty root object

        [Test]
        public void should_create_empty_root_object()
        {
            Node.Encode().ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Yada />");
        }

        [Test]
        public void should_read_empty_root_object()
        {
            var node = ElementNode.Parse("<Yada/>", Options);
            node.ShouldTotal(0);
            node.NodeType.ShouldEqual(NodeType.Variable);
            node.IsNamed.ShouldBeTrue();
            node.Name.ShouldEqual("Yada");
            node.Format.ShouldEqual("xml");
        }

        // Indexer

        [Test]
        public void should_get_node_by_name()
        {
            ElementNode.Parse("<Yada><Oh>hai</Oh></Yada>", Options).GetNode("Oh").Value.ShouldEqual("hai");
        }

        [Test]
        public void should_return_null_when_there_is_no_match()
        {
            ElementNode.Parse("<Yada></Yada>", Options).GetNode("Oh").ShouldBeNull();
        }

        // Add

        [Test]
        public void should_add_named_node_as_element()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options);
            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("Hai", NodeType.Value, Metadata.Empty, c),
                x =>
                {
                    x.ShouldBeType<ElementNode>();
                    x.Name.ShouldEqual("Hai");
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Parent.ShouldBeSameAs(node);
                });
        }

        [Test]
        public void should_add_named_node_as_attribute_when_passed_xml_attribute_attribute()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options);
            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("Hai", NodeType.Value, new Metadata(new XmlAttributeAttribute()), c),
                x =>
                {
                    x.ShouldBeType<AttributeNode>();
                    x.Name.ShouldEqual("Hai");
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Parent.ShouldBeSameAs(node);
                });
        }

        [Test]
        public void should_fail_to_add_unnamed_node()
        {
            Assert.Throws<UnnamedChildrenNotSupportedException>(() => 
                Node.Add(NodeType.Value, Metadata.Empty, x => { }));
        }

        // Insert

        [Test]
        public void should_insert_xml_element()
        {
            var parent = ElementNode.Create("Oh", Metadata.Empty, Options);
            var node = ElementNode.Create("Hai", Metadata.Empty, Options);
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(node, c));
            var child = parent.Cast<ElementNode>().First();
            child.Element.Name.ShouldEqual(node.Element.Name);
            child.Parent.ShouldBeSameAs(parent);
        }

        [Test]
        public void should_insert_xml_attribute()
        {
            var parent = ElementNode.Create("Oh", Metadata.Empty, Options);
            var node = new AttributeNode(new XAttribute("Hai", "yada"), (ElementNode)parent, Options);
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(node, c));
            var child = parent.Cast<AttributeNode>().First();
            child.Attribute.Name.ShouldEqual(node.Attribute.Name);
            child.Attribute.Value.ShouldEqual(node.Attribute.Value);
            child.Parent.ShouldBeSameAs(parent);
        }

        // Values

        [Test]
        [TestCase("<Hai/>", null)]
        [TestCase("<Hai></Hai>", "")]
        [TestCase("<Hai>yada</Hai>", "yada")]
        [TestCase("<Hai></Hai>", "")]
        [TestCase("<Hai><![CDATA[yada]]></Hai>", "yada")]
        public void should_get_value(string element, string expectedValue)
        {
            var node = ElementNode.Parse("<Oh>{0}</Oh>".ToFormat(element), Options);
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("Hai");
            children[0].Value.ShouldEqual(expectedValue);
            children[0].NodeType.ShouldEqual(NodeType.Variable);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        [TestCase(null, "<Hai />")]
        [TestCase("", "<Hai></Hai>")]
        [TestCase("yada", "<Hai>yada</Hai>")]
        [TestCase("<yada>", "<Hai>&lt;yada&gt;</Hai>")]
        public void should_set_value(string value, string expectedElement)
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options);
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => x.Value = value);
            node.Encode().ShouldEqual(Declaration + "<Oh>{0}</Oh>".ToFormat(expectedElement));
        }

        [Test]
        public void should_set_boolean_value_as_lower_case()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options);
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => x.Value = true);
            node.Encode().ShouldEqual(Declaration + "<Oh><Hai>true</Hai></Oh>");
        }

        [Test]
        public void should_get_attribute_value()
        {
            var node = ElementNode.Parse("<Oh Hai=\"There\" />", Options);
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("Hai");
            children[0].Value.ShouldEqual("There");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_attribute_value_when_a_value_and_configured()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y.XmlValuesAsAttributes())));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => x.Value = "There");
            node.Encode().ShouldEqual(Declaration + "<Oh Hai=\"There\" />");
        }

        [Test]
        [TestCase(NodeType.Object)]
        [TestCase(NodeType.Array)]
        [TestCase(NodeType.Variable)]
        public void should_not_set_attribute_value_when_not_a_value_and_configured(NodeType type)
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y.XmlValuesAsAttributes())));
            node.Add("Hai", type, Metadata.Empty, x => { });
            node.Encode().ShouldEqual(Declaration + "<Oh><Hai /></Oh>");
        }

        // Children

        [Test]
        public void should_parse_and_return_children()
        {
            var node = ElementNode.Parse("<Yada O=\"rly\"><Oh>Hai</Oh></Yada>", Options);
            var children = node.Cast<XmlNodeBase>().ToList();
            children.ShouldTotal(2);

            var child = children.First();
            child.Name.ShouldEqual("Oh");
            child.NodeType.ShouldEqual(NodeType.Variable);
            child.XmlType.ShouldEqual(XmlObjectType.Element);
            child.Value.ShouldEqual("Hai");
            child.Parent.ShouldBeSameAs(node);

            child = children.Skip(1).First();
            child.Name.ShouldEqual("O");
            child.NodeType.ShouldEqual(NodeType.Value);
            child.XmlType.ShouldEqual(XmlObjectType.Attribute);
            child.Value.ShouldEqual("rly");
            child.Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_exclude_attributes_when_configured()
        {
            var nodes = ElementNode.Parse("<Yada O=\"rly\"><Oh>Hai</Oh></Yada>", 
                Options.Create(x => x.Deserialization(y => y.IgnoreXmlAttributes())))
                .Cast<XmlNodeBase>().ToList();

            nodes.ShouldTotal(1);

            var child = nodes.First();
            child.Name.ShouldEqual("Oh");
            child.NodeType.ShouldEqual(NodeType.Variable);
            child.XmlType.ShouldEqual(XmlObjectType.Element);
            child.Value.ShouldEqual("Hai");
        }

        // Namespaces

        [Test]
        public void should_set_default_namespace()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y
                .WithDefaultXmlNamespace("http://namespace.org"))));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => x.Value = "There");
            node.Encode().ShouldEqual(Declaration + "<Oh xmlns=\"http://namespace.org\"><Hai>There</Hai></Oh>");
        }

        [Test]
        public void should_set_default_namespace_with_value_attributes()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y
                .WithDefaultXmlNamespace("http://namespace.org").XmlValuesAsAttributes())));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x => x.Value = "There");
            node.Encode().ShouldEqual(Declaration + "<Oh Hai=\"There\" xmlns=\"http://namespace.org\" />");
        }

        [Test]
        public void should_add_and_set_namespace_prefix()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y
                .AddXmlNamespace("abc", "http://namespace.org"))));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x =>
                {
                    x.Value = "There";
                    x.As<XmlNodeBase>().SetNamespacePrefix("abc");
                });
            node.Encode().ShouldEqual(Declaration + "<Oh xmlns:abc=\"http://namespace.org\"><abc:Hai>There</abc:Hai></Oh>");
        }

        [Test]
        public void should_add_and_set_namespace()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create());
            node.Add("Hai", NodeType.Value, Metadata.Empty, x =>
            {
                x.Value = "There";
                x.As<XmlNodeBase>().SetNamespace("http://namespace.org");
            });
            node.Encode().ShouldEqual(Declaration + "<Oh><Hai xmlns=\"http://namespace.org\">There</Hai></Oh>");
        }
    }
}
