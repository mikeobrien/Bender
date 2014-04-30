using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;

namespace Bender.Nodes.Xml
{
    public class ElementNode : XmlNodeBase
    {
        private readonly XNamespace _namespace;
        private NodeType _type = NodeType.Variable;

        public ElementNode(XDocument document, Options options, XNamespace @namespace = null) :
            this(document.Root, options, @namespace) { }

        public ElementNode(XElement element, Options options, XNamespace @namespace = null, ElementNode parent = null)
            : base(element, parent, options)
        {
            _namespace = @namespace;
        }

        private ElementNode(XElement element, XNamespace @namespace, NodeType type, ElementNode parent, Options options)
            : base(element, parent, options)
        {
            _namespace = @namespace;
            _type = type;
        }

        public static XmlNodeBase Create(string name, Metadata metadata, Options options)
        {
            var @namespace = !options.Serialization.DefaultXmlNamespace.IsNullOrEmpty() ?
                options.Serialization.DefaultXmlNamespace : null;
            var element = new XElement(@namespace != null ? @namespace + name : name);
            if (options.Serialization.XmlNamespaces.Any())
                options.Serialization.XmlNamespaces.ForEach(x => 
                    element.Add(new XAttribute(XNamespace.Xmlns + x.Key, x.Value)));
            return new ElementNode(new XDocument(element), options, @namespace);
        }

        public static XmlNodeBase Parse(string xml, Options options)
        {
            return new ElementNode(Exception<XmlException>.Map(
                () => XDocument.Parse(xml),
                x => new ParseException(x, NodeFormat)), options);
        }

        public static XmlNodeBase Parse(byte[] bytes, Options options, Encoding encoding = null)
        {
            return Parse(new MemoryStream(bytes), options, encoding);
        }

        public static XmlNodeBase Parse(Stream stream, Options options, Encoding encoding = null)
        {
            return new ElementNode(Exception<XmlException>.Map(
                () => XDocument.Load(new StreamReader(stream, encoding ?? Encoding.UTF8)),
                x => new ParseException(x, NodeFormat)), options);
        }

        public override string Type { get { return "element"; } }
        public override string Path { get { return Element.GetPath(); } }

        protected override NodeType GetNodeType()
        {
            return _type;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            _type = nodeType;
        }

        protected override string GetName()
        {
            return Element.Name.LocalName;
        }

        protected override void SetName(string name)
        {
            Element.Name = name;
        }

        protected override object GetValue()
        {
            return Element.IsEmpty ? null : Element.Value;
        }

        protected override void SetValue(object value)
        {
            if (value != null) Element.Value = (value is bool ?
                value.ToString().ToLower() : value.ToString());
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is ElementNode) Element.Add(((ElementNode)node).Element);
            else if (node is AttributeNode) Element.Add(((AttributeNode)node).Attribute);
            else if (!named) throw new UnnamedChildrenNotSupportedException();
            else
            {
                var nodeName = _namespace != null ? _namespace + node.Name : node.Name;
                ((Options.Serialization.XmlValueNodeType == XmlValueNodeType.Attribute ||
                    node.Metadata.Contains<XmlAttributeAttribute>()) && node.NodeType.IsValue() ?
                    (XmlNodeBase)new AttributeNode(Element.CreateAttribute(node.Name), this, Options) :
                    new ElementNode(Element.CreateElement(nodeName), _namespace, node.NodeType, this, Options)).Configure(modify);
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            var elements = Element.Elements().Select(x => new ElementNode(x, Options, _namespace, this)).Cast<XmlNodeBase>();
            return Options.Deserialization.IgnoreXmlAttributes ? elements : elements
                .Union(Element.Attributes().Select(x => new AttributeNode(x, this, Options)));
        }

        public override void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            Element.Save(new StreamWriter(stream, encoding ?? Encoding.UTF8), 
                pretty ? SaveOptions.None : SaveOptions.DisableFormatting);
        }

        public override void SetNamespace(XNamespace @namespace)
        {
            Element.Name = @namespace + Element.Name.LocalName;
        }
    }
}
