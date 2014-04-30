using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bender.Nodes.Xml;

namespace Bender
{
    public partial class Deserializer
    {
        // String

        public T DeserializeXml<T>(string xml)
        {
            return (T)DeserializeXml(xml, typeof(T));
        }

        public object DeserializeXml(string xml, Type type)
        {
            return Deserialize(ElementNode.Parse(xml, _options), type);
        }

        public XmlNodeBase DeserializeXml(string xml)
        {
            return ElementNode.Parse(xml, _options);
        }

        // XDocument

        public T DeserializeXml<T>(XDocument document)
        {
            return (T)DeserializeXml(document, typeof(T));
        }

        public object DeserializeXml(XDocument document, Type type)
        {
            return Deserialize(new ElementNode(document, _options), type);
        }

        public ElementNode DeserializeXml(XDocument document)
        {
            return new ElementNode(document, _options);
        }

        // XElement

        public T DeserializeXml<T>(XElement element)
        {
            return (T)DeserializeXml(element, typeof(T));
        }

        public object DeserializeXml(XElement element, Type type)
        {
            return Deserialize(new ElementNode(element, _options), type);
        }

        public ElementNode DeserializeXml(XElement element)
        {
            return new ElementNode(element, _options);
        }

        // Bytes

        public T DeserializeXml<T>(byte[] bytes, Encoding encoding = null)
        {
            return (T)DeserializeXml(bytes, typeof(T), encoding);
        }

        public object DeserializeXml(byte[] bytes, Type type, Encoding encoding = null)
        {
            return Deserialize(ElementNode.Parse(bytes, _options, encoding ?? Encoding.UTF8), type);
        }

        public XmlNodeBase DeserializeXml(byte[] bytes, Encoding encoding = null)
        {
            return ElementNode.Parse(bytes, _options, encoding ?? Encoding.UTF8);
        }

        // Stream

        public T DeserializeXml<T>(Stream stream, Encoding encoding = null)
        {
            return (T)DeserializeXml(stream, typeof(T), encoding);
        }

        public object DeserializeXml(Stream stream, Type type, Encoding encoding = null)
        {
            return Deserialize(ElementNode.Parse(stream, _options, encoding ?? Encoding.UTF8), type);
        }

        public XmlNodeBase DeserializeXml(Stream stream, Encoding encoding = null)
        {
            return ElementNode.Parse(stream, _options, encoding ?? Encoding.UTF8);
        }

        // File

        public T DeserializeXmlFile<T>(string path, Encoding encoding = null)
        {
            return (T)DeserializeXmlFile(path, typeof(T), encoding);
        }

        public object DeserializeXmlFile(string path, Type type, Encoding encoding = null)
        {
            return DeserializeXml(File.ReadAllBytes(path), type, encoding);
        }

        public XmlNodeBase DeserializeXmlFile(string path, Encoding encoding = null)
        {
            return DeserializeXml(File.ReadAllBytes(path), encoding);
        }
    }
}
