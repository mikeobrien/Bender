using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bender.Extensions;
using Bender.Nodes.Xml;

namespace Bender
{
    public partial class Serializer
    {
        // XElement
        public XElement SerializeXmlElement(object source)
        {
            return SerializeXmlElement(source, source.GetType());
        }

        public XElement SerializeXmlElement<T>(T source)
        {
            return SerializeXmlElement(source, typeof(T));
        }

        public XElement SerializeXmlElement(object source, Type type)
        {
            return SerializeXmlNodes(source, type).Element;
        }

        // XDocument

        public XDocument SerializeXmlDocument(object source)
        {
            return SerializeXmlDocument(source, source.GetType());
        }

        public XDocument SerializeXmlDocument<T>(T source)
        {
            return SerializeXmlDocument(source, typeof(T));
        }

        public XDocument SerializeXmlDocument(object source, Type type)
        {
            return SerializeXmlNodes(source, type).Element.Document;
        }

        // String

        public string SerializeXml(object source)
        {
            return SerializeString(source, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat);
        }

        public string SerializeXml<T>(T source)
        {
            return SerializeXml(source, typeof(T));
        }

        public string SerializeXml(object source, Type type)
        {
            return SerializeString(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat);
        }

        // Bytes

        public byte[] SerializeXmlBytes(object source, Encoding encoding = null)
        {
            return SerializeBytes(source, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, encoding);
        }

        public byte[] SerializeXmlBytes<T>(T source, Encoding encoding = null)
        {
            return SerializeXmlBytes(source, typeof(T), encoding);
        }

        public byte[] SerializeXmlBytes(object source, Type type, Encoding encoding = null)
        {
            return SerializeBytes(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, encoding);
        }

        // Return Stream

        public Stream SerializeXmlStream(object source, Encoding encoding = null)
        {
            return SerializeStream(source, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, encoding);
        }

        public Stream SerializeXmlStream<T>(T source, Encoding encoding = null)
        {
            return SerializeXmlStream(source, typeof(T), encoding);
        }

        public Stream SerializeXmlStream(object source, Type type, Encoding encoding = null)
        {
            return SerializeStream(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, encoding);
        }

        // To Stream

        public void SerializeXmlStream(object source, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, stream, encoding);
        }

        public void SerializeXmlStream<T>(T source, Stream stream, Encoding encoding = null)
        {
            SerializeXmlStream(source, typeof(T), stream, encoding);
        }

        public void SerializeXmlStream(object source, Type type, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, stream, encoding);
        }

        // File

        public void SerializeXmlFile(object source, string path, Encoding encoding = null)
        {
            SerializeXmlFile(source, source.GetType(), path, encoding);
        }

        public void SerializeXmlFile<T>(T source, string path, Encoding encoding = null)
        {
            SerializeXmlFile(source, typeof(T), path, encoding);
        }

        public void SerializeXmlFile(object source, Type type, string path, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat, encoding).SaveToFile(path);
        }

        // Nodes

        public ElementNode SerializeXmlNodes(object source)
        {
            return (ElementNode)SerializeNodes(source, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat);
        }

        public ElementNode SerializeXmlNodes<T>(T source)
        {
            return SerializeXmlNodes(source, typeof(T));
        }

        public ElementNode SerializeXmlNodes(object source, Type type)
        {
            return (ElementNode)SerializeNodes(source, type, (s, o) => ElementNode.Create(s.Name, s.Metadata, o), XmlNodeBase.NodeFormat);
        }
    }
}
