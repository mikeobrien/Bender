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
    }
}
