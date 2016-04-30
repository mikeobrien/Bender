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
using UTF8Encoding = Bender.Extensions.UTF8Encoding;
using XmlAttributeAttribute = Bender.Nodes.Xml.Microsoft.XmlAttributeAttribute;

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

        public static XmlNodeBase Create(string name, Metadata metadata, Options options)
        {
            var @namespace = GetNamespace(name, metadata, 
                options.Serialization.DefaultXmlNamespace, options);
            var element = new XElement(GetNodeName(name, @namespace));
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

        public override string Type => "element";
        public override string Path => Element.GetPath();

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
                var @namespace = GetNamespace(node.Name, node.Metadata, _namespace, Options);
                XmlNodeBase xmlNode;
                if ((Options.Serialization.XmlValueNodeType == XmlValueNodeType.Attribute ||
                    node.Metadata.Contains<System.Xml.Serialization.XmlAttributeAttribute>() ||
                    node.Metadata.Contains<XmlAttributeAttribute>()) && 
                    node.NodeType.IsValue())
                {
                    xmlNode = new AttributeNode(Element.CreateAttribute(node.Name), this, Options);
                }
                else if (node.Metadata.Contains<XmlSiblingsAttribute>()) xmlNode = this;
                else
                {
                    xmlNode = new ElementNode(Element.CreateElement(GetNodeName(node.Name, 
                        @namespace)), Options, @namespace, this);
                }
                xmlNode.Configure(modify);
            }
        }

        private static XNamespace GetNamespace(string name, Metadata metadata,
            XNamespace defaultNamespace, Options options)
        {
            var @namespace =
                GetAttributeValue<XmlRootAttribute>(metadata, x => x.Namespace) ??
                GetAttributeValue<XmlTypeAttribute>(metadata, x => x.Namespace) ??
                GetAttributeValue<XmlElementAttribute>(metadata, x => x.Namespace) ??
                defaultNamespace;
            var lookup = options.Serialization.XmlNamespaces.FirstOrDefault(x => x.Key == @namespace);
            return lookup.Key != null ? lookup.Value : @namespace;
        }

        private static XName GetNodeName(string name, XNamespace @namespace)
        {
            return @namespace != null ? @namespace + name : name;
        }

        private static string GetAttributeValue<T>(Metadata metadata, Func<T, string> map)
        {
            if (!metadata.Contains<T>()) return null;
            var attribute = metadata.Get<T>();
            var value = map(attribute);
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        protected override IEnumerable<INode> GetNodes()
        {
            var elements = Element.Elements().Select(x => new ElementNode(x, Options, _namespace, this)).Cast<XmlNodeBase>();
            return Options.Deserialization.IgnoreXmlAttributes ? elements : elements
                .Union(Element.Attributes().Select(x => new AttributeNode(x, this, Options)));
        }

        public override void Encode(Stream stream, Encoding encoding = null)
        {
            var writer = XmlWriter.Create(stream, new XmlWriterSettings
            {
                OmitXmlDeclaration = Options.Serialization.OmitXmlDeclaration,
                Encoding = encoding ?? UTF8Encoding.NoBOM,
                IndentChars = "\t",
                Indent = Options.Serialization.PrettyPrint
            });
            Element.Save(writer);
            writer.Flush();
        }

        public override void SetNamespace(XNamespace @namespace)
        {
            Element.Name = @namespace + Element.Name.LocalName;
        }
    }
}
