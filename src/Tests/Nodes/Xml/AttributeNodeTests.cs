using System.Xml.Linq;
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
    public class AttributeNodeTests
    {
        private const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        private static ElementNode _parent;
        private static AttributeNode _attribute;

        [SetUp]
        public void Setup()
        {
            _parent = new ElementNode(new XElement("yada"), null);
            _attribute = new AttributeNode(new XDocument(new XElement("Yada",
                new XAttribute("Oh", "Hai"))).Root.Attribute("Oh"), _parent, Options.Create());
        }

        [Test]
        public void should_return_node_type()
        {
            _attribute.NodeType.ShouldEqual(NodeType.Value);
        }

        [Test]
        public void should_be_of_type_attribute()
        {
            _attribute.Type.ShouldEqual("attribute");
        }

        [Test]
        public void should_return_element_type()
        {
            _attribute.XmlType.ShouldEqual(XmlObjectType.Attribute);
        }

        [Test]
        public void should_return_format()
        {
            _attribute.Format.ShouldEqual(XmlNodeBase.NodeFormat);
        }

        [Test]
        public void should_return_is_named()
        {
            _attribute.IsNamed.ShouldBeTrue();
        }

        [Test]
        public void should_return_path()
        {
            _attribute.Path.ShouldEqual("/Yada/@Oh");
        }

        [Test]
        public void should_return_parent()
        {
            _attribute.Parent.ShouldBeSameAs(_parent);
        }

        [Test]
        public void should_get_name()
        {
            _attribute.IsNamed.ShouldBeTrue();
            _attribute.Name.ShouldEqual("Oh");
        }

        [Test]
        public void should_set_name()
        {
            _attribute.Name = "yada";
            _attribute.Name.ShouldEqual("yada");
            _attribute.Attribute.Name.ShouldEqual("yada");
        }

        [Test]
        public void should_set_name_and_keep_namespace()
        {
            _attribute.SetNamespace("urn:abc");
            _attribute.Attribute.Name.ShouldEqual("{urn:abc}Oh");
            _attribute.Name = "yada";
            _attribute.Name.ShouldEqual("yada");
            _attribute.Attribute.Name.ShouldEqual("{urn:abc}yada");
        }

        [Test]
        public void should_get_value()
        {
            _attribute.Value.ShouldEqual("Hai");
        }

        [Test]
        public void should_set_value()
        {
            _attribute.Value = "yada";
            _attribute.Value.ShouldEqual("yada");
        }

        [Test]
        public void should_set_boolean_value_as_lowercase()
        {
            _attribute.Value = true;
            _attribute.Value.ShouldEqual("true");
        }

        [Test]
        public void should_add_and_set_namespace_prefix()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x.Serialization(y => y
                .AddXmlNamespace("abc", "http://namespace.org").XmlValuesAsAttributes())));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x =>
            {
                x.Value = "There";
                x.As<XmlNodeBase>().SetNamespacePrefix("abc");
            });
            node.Encode().ShouldEqual(Declaration + "<Oh xmlns:abc=\"http://namespace.org\" abc:Hai=\"There\" />");
        }

        [Test]
        public void should_add_and_set_namespace()
        {
            var node = ElementNode.Create("Oh", Metadata.Empty, Options.Create(x => x
                .Serialization(y => y.XmlValuesAsAttributes())));
            node.Add("Hai", NodeType.Value, Metadata.Empty, x =>
            {
                x.Value = "There";
                x.As<XmlNodeBase>().SetNamespace("http://namespace.org");
            });
            node.Encode().ShouldEqual(Declaration + "<Oh p1:Hai=\"There\" xmlns:p1=\"http://namespace.org\" />");
        }
    }
}
