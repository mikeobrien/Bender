using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bender.Configuration;
using Bender.Nodes.Xml;

namespace Bender
{
    public static partial class Deserialize
    {
        // String

        public static T Xml<T>(string xml, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(xml);
        }

        public static T Xml<T>(string xml, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(xml);
        }

        public static object Xml(string xml, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(xml, type);
        }

        public static object Xml(string xml, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(xml, type);
        }

        public static XmlNodeBase Xml(string xml, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(xml);
        }

        public static XmlNodeBase Xml(string xml, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(xml);
        }

        public static T Xml<T>(string xml, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(xml, xsl);
        }

        public static T Xml<T>(string xml, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(xml, xsl);
        }

        public static object Xml(string xml, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(xml, xsl, type);
        }

        public static object Xml(string xml, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(xml, xsl, type);
        }

        public static XmlNodeBase Xml(string xml, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(xml, xsl);
        }

        public static XmlNodeBase Xml(string xml, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(xml, xsl);
        }

        // XDocument

        public static T Xml<T>(XDocument document, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(document);
        }

        public static T Xml<T>(XDocument document, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(document);
        }

        public static object Xml(XDocument document, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(document, type);
        }

        public static object Xml(XDocument document, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(document, type);
        }

        public static XmlNodeBase Xml(XDocument document, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(document);
        }

        public static XmlNodeBase Xml(XDocument document, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(document);
        }

        public static T Xml<T>(XDocument document, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(document, xsl);
        }

        public static T Xml<T>(XDocument document, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(document, xsl);
        }

        public static object Xml(XDocument document, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(document, xsl, type);
        }

        public static object Xml(XDocument document, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(document, xsl, type);
        }

        public static XmlNodeBase Xml(XDocument document, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(document, xsl);
        }

        public static XmlNodeBase Xml(XDocument document, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(document, xsl);
        }

        // XElement

        public static T Xml<T>(XElement element, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(element);
        }

        public static T Xml<T>(XElement element, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(element);
        }

        public static object Xml(XElement element, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(element, type);
        }

        public static object Xml(XElement element, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(element, type);
        }

        public static XmlNodeBase Xml(XElement element, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(element);
        }

        public static XmlNodeBase Xml(XElement element, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(element);
        }

        public static T Xml<T>(XElement element, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(element, xsl);
        }

        public static T Xml<T>(XElement element, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(element, xsl);
        }

        public static object Xml(XElement element, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(element, xsl, type);
        }

        public static object Xml(XElement element, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(element, xsl, type);
        }

        public static XmlNodeBase Xml(XElement element, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(element, xsl);
        }

        public static XmlNodeBase Xml(XElement element, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(element, xsl);
        }

        // Bytes

        public static T Xml<T>(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(bytes);
        }

        public static T Xml<T>(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(bytes);
        }

        public static T Xml<T>(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(bytes, encoding);
        }

        public static T Xml<T>(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(bytes, encoding);
        }

        public static object Xml(byte[] bytes, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, type);
        }

        public static object Xml(byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, type);
        }

        public static object Xml(byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, type, encoding);
        }

        public static object Xml(byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, type, encoding);
        }

        public static XmlNodeBase Xml(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes);
        }

        public static XmlNodeBase Xml(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes);
        }

        public static XmlNodeBase Xml(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, encoding);
        }

        public static XmlNodeBase Xml(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, encoding);
        }

        public static T Xml<T>(byte[] bytes, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(bytes, xsl);
        }

        public static T Xml<T>(byte[] bytes, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(bytes, xsl);
        }

        public static T Xml<T>(byte[] bytes, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(bytes, xsl, encoding);
        }

        public static T Xml<T>(byte[] bytes, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(bytes, xsl, encoding);
        }

        public static object Xml(byte[] bytes, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, xsl, type);
        }

        public static object Xml(byte[] bytes, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, xsl, type);
        }

        public static object Xml(byte[] bytes, string xsl, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, xsl, type, encoding);
        }

        public static object Xml(byte[] bytes, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, xsl, type, encoding);
        }

        public static XmlNodeBase Xml(byte[] bytes, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, xsl);
        }

        public static XmlNodeBase Xml(byte[] bytes, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, xsl);
        }

        public static XmlNodeBase Xml(byte[] bytes, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(bytes, xsl, encoding);
        }

        public static XmlNodeBase Xml(byte[] bytes, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(bytes, xsl, encoding);
        }

        // Stream

        public static T Xml<T>(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(stream);
        }

        public static T Xml<T>(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(stream);
        }

        public static T Xml<T>(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(stream, encoding);
        }

        public static T Xml<T>(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(stream, encoding);
        }

        public static object Xml(Stream stream, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, type);
        }

        public static object Xml(Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, type);
        }

        public static object Xml(Stream stream, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, type, encoding);
        }

        public static object Xml(Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, type, encoding);
        }

        public static XmlNodeBase Xml(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream);
        }

        public static XmlNodeBase Xml(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream);
        }

        public static XmlNodeBase Xml(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, encoding);
        }

        public static XmlNodeBase Xml(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, encoding);
        }

        public static T Xml<T>(Stream stream, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(stream, xsl);
        }

        public static T Xml<T>(Stream stream, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(stream, xsl);
        }

        public static T Xml<T>(Stream stream, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml<T>(stream, xsl, encoding);
        }

        public static T Xml<T>(Stream stream, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(stream, xsl, encoding);
        }

        public static object Xml(Stream stream, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, xsl, type);
        }

        public static object Xml(Stream stream, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, xsl, type);
        }

        public static object Xml(Stream stream, string xsl, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, xsl, type, encoding);
        }

        public static object Xml(Stream stream, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, xsl, type, encoding);
        }

        public static XmlNodeBase Xml(Stream stream, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, xsl);
        }

        public static XmlNodeBase Xml(Stream stream, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, xsl);
        }

        public static XmlNodeBase Xml(Stream stream, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXml(stream, xsl, encoding);
        }

        public static XmlNodeBase Xml(Stream stream, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(stream, xsl, encoding);
        }

        // File

        public static T XmlFile<T>(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile<T>(path);
        }

        public static T XmlFile<T>(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile<T>(path);
        }

        public static T XmlFile<T>(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile<T>(path, encoding);
        }

        public static T XmlFile<T>(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile<T>(path, encoding);
        }

        public static object XmlFile(string path, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, type);
        }

        public static object XmlFile(string path, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, type);
        }

        public static object XmlFile(string path, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, type, encoding);
        }

        public static object XmlFile(string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, type, encoding);
        }

        public static XmlNodeBase XmlFile(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path);
        }

        public static XmlNodeBase XmlFile(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path);
        }

        public static XmlNodeBase XmlFile(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, encoding);
        }

        public static XmlNodeBase XmlFile(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, encoding);
        }

        public static T XmlFile<T>(string path, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile<T>(path, xsl);
        }

        public static T XmlFile<T>(string path, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile<T>(path, xsl);
        }

        public static T XmlFile<T>(string path, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile<T>(path, xsl, encoding);
        }

        public static T XmlFile<T>(string path, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile<T>(path, xsl, encoding);
        }

        public static object XmlFile(string path, string xsl, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, xsl, type);
        }

        public static object XmlFile(string path, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, xsl, type);
        }

        public static object XmlFile(string path, string xsl, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, xsl, type, encoding);
        }

        public static object XmlFile(string path, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, xsl, type, encoding);
        }

        public static XmlNodeBase XmlFile(string path, string xsl, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, xsl);
        }

        public static XmlNodeBase XmlFile(string path, string xsl, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, xsl);
        }

        public static XmlNodeBase XmlFile(string path, string xsl, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeXmlFile(path, xsl, encoding);
        }

        public static XmlNodeBase XmlFile(string path, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(path, xsl, encoding);
        }
    }
}
