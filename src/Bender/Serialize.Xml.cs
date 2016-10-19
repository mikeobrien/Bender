using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bender.Configuration;
using Bender.Nodes.Xml;

namespace Bender
{
    public static partial class Serialize
    {
        // XElement

        public static XElement XmlElement(object source, Options options)
        {
            return Serializer.Create(options).SerializeXmlElement(source);
        }

        public static XElement XmlElement(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlElement(source);
        }

        public static XElement XmlElement<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXmlElement(source);
        }

        public static XElement XmlElement<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlElement(source);
        }

        public static XElement XmlElement(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXmlElement(source, type);
        }

        public static XElement XmlElement(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlElement(source, type);
        }

        // XElement extensions

        public static XElement SerializeXmlElement(this object source, Options options)
        {
            return XmlElement(source, options);
        }

        public static XElement SerializeXmlElement(this object source, Action<OptionsDsl> configure = null)
        {
            return XmlElement(source, configure);
        }

        public static XElement SerializeXmlElement<T>(this T source, Options options)
        {
            return XmlElement(source, options);
        }

        public static XElement SerializeXmlElement<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return XmlElement(source, configure);
        }

        public static XElement SerializeXmlElement(this object source, Type type, Options options)
        {
            return XmlElement(source, type, options);
        }

        public static XElement SerializeXmlElement(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlElement(source, type, configure);
        }

        // XDocument

        public static XDocument XmlDocument(object source, Options options)
        {
            return Serializer.Create(options).SerializeXmlDocument(source);
        }

        public static XDocument XmlDocument(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlDocument(source);
        }

        public static XDocument XmlDocument<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXmlDocument(source);
        }

        public static XDocument XmlDocument<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlDocument(source);
        }

        public static XDocument XmlDocument(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXmlDocument(source, type);
        }

        public static XDocument XmlDocument(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlDocument(source, type);
        }

        // XDocument extensions

        public static XDocument SerializeXmlDocument(this object source, Options options)
        {
            return XmlDocument(source, options);
        }

        public static XDocument SerializeXmlDocument(this object source, Action<OptionsDsl> configure = null)
        {
            return XmlDocument(source, configure);
        }

        public static XDocument SerializeXmlDocument<T>(this T source, Options options)
        {
            return XmlDocument(source, options);
        }

        public static XDocument SerializeXmlDocument<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return XmlDocument(source, configure);
        }

        public static XDocument SerializeXmlDocument(this object source, Type type, Options options)
        {
            return XmlDocument(source, type, options);
        }

        public static XDocument SerializeXmlDocument(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlDocument(source, type, configure);
        }

        // String

        public static string Xml(object source, Options options)
        {
            return Serializer.Create(options).SerializeXml(source);
        }

        public static string Xml(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXml(source);
        }

        public static string Xml<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXml(source);
        }

        public static string Xml<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXml(source);
        }

        public static string Xml(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXml(source, type);
        }

        public static string Xml(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXml(source, type);
        }

        // String extensions

        public static string SerializeXml(this object source, Options options)
        {
            return Xml(source, options);
        }

        public static string SerializeXml(this object source, Action<OptionsDsl> configure = null)
        {
            return Xml(source, configure);
        }

        public static string SerializeXml<T>(this T source, Options options)
        {
            return Xml(source, options);
        }

        public static string SerializeXml<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return Xml(source, configure);
        }

        public static string SerializeXml(this object source, Type type, Options options)
        {
            return Xml(source, type, options);
        }

        public static string SerializeXml(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(source, type, configure);
        }

        // Bytes

        public static byte[] XmlBytes(object source, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source);
        }

        public static byte[] XmlBytes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source);
        }

        public static byte[] XmlBytes(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source, encoding);
        }

        public static byte[] XmlBytes(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source, encoding);
        }

        public static byte[] XmlBytes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source);
        }

        public static byte[] XmlBytes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source);
        }

        public static byte[] XmlBytes<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source, encoding);
        }

        public static byte[] XmlBytes<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source, encoding);
        }

        public static byte[] XmlBytes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source, type);
        }

        public static byte[] XmlBytes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source, type);
        }

        public static byte[] XmlBytes(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlBytes(source, type, encoding);
        }

        public static byte[] XmlBytes(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlBytes(source, type, encoding);
        }

        // Bytes extensions

        public static byte[] SerializeXmlBytes(this object source, Options options)
        {
            return XmlBytes(source, options);
        }

        public static byte[] SerializeXmlBytes(this object source, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, configure);
        }

        public static byte[] SerializeXmlBytes(this object source, Encoding encoding, Options options)
        {
            return XmlBytes(source, encoding, options);
        }

        public static byte[] SerializeXmlBytes(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, encoding, configure);
        }

        public static byte[] SerializeXmlBytes<T>(this T source, Options options)
        {
            return XmlBytes(source, options);
        }

        public static byte[] SerializeXmlBytes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, configure);
        }

        public static byte[] SerializeXmlBytes<T>(this T source, Encoding encoding, Options options)
        {
            return XmlBytes(source, encoding, options);
        }

        public static byte[] SerializeXmlBytes<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, encoding, configure);
        }

        public static byte[] SerializeXmlBytes(this object source, Type type, Options options)
        {
            return XmlBytes(source, type, options);
        }

        public static byte[] SerializeXmlBytes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, type, configure);
        }

        public static byte[] SerializeXmlBytes(this object source, Type type, Encoding encoding, Options options)
        {
            return XmlBytes(source, type, encoding, options);
        }

        public static byte[] SerializeXmlBytes(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlBytes(source, type, encoding, configure);
        }

        // Return Stream

        public static Stream XmlStream(object source, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source);
        }

        public static Stream XmlStream(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source);
        }

        public static Stream XmlStream(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source, encoding);
        }

        public static Stream XmlStream(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source, encoding);
        }

        public static Stream XmlStream<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source);
        }

        public static Stream XmlStream<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source);
        }

        public static Stream XmlStream<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source, encoding);
        }

        public static Stream XmlStream<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source, encoding);
        }

        public static Stream XmlStream(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source, type);
        }

        public static Stream XmlStream(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source, type);
        }

        public static Stream XmlStream(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeXmlStream(source, type, encoding);
        }

        public static Stream XmlStream(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlStream(source, type, encoding);
        }

        // Return Stream extensions

        public static Stream SerializeXmlStream(this object source, Options options)
        {
            return XmlStream(source, options);
        }

        public static Stream SerializeXmlStream(this object source, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, configure);
        }

        public static Stream SerializeXmlStream(this object source, Encoding encoding, Options options)
        {
            return XmlStream(source, encoding, options);
        }

        public static Stream SerializeXmlStream(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, encoding, configure);
        }

        public static Stream SerializeXmlStream<T>(this T source, Options options)
        {
            return XmlStream(source, options);
        }

        public static Stream SerializeXmlStream<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, configure);
        }

        public static Stream SerializeXmlStream<T>(this T source, Encoding encoding, Options options)
        {
            return XmlStream(source, encoding, options);
        }

        public static Stream SerializeXmlStream<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, encoding, configure);
        }

        public static Stream SerializeXmlStream(this object source, Type type, Options options)
        {
            return XmlStream(source, type, options);
        }

        public static Stream SerializeXmlStream(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, type, configure);
        }

        public static Stream SerializeXmlStream(this object source, Type type, Encoding encoding, Options options)
        {
            return XmlStream(source, type, encoding, options);
        }

        public static Stream SerializeXmlStream(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlStream(source, type, encoding, configure);
        }

        // To Stream

        public static void XmlStream(object source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, stream);
        }

        public static void XmlStream(object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, stream);
        }

        public static void XmlStream(object source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, stream, encoding);
        }

        public static void XmlStream(object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, stream, encoding);
        }

        public static void XmlStream<T>(T source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, stream);
        }

        public static void XmlStream<T>(T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, stream);
        }

        public static void XmlStream<T>(T source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, stream, encoding);
        }

        public static void XmlStream<T>(T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, stream, encoding);
        }

        public static void XmlStream(object source, Type type, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, type, stream);
        }

        public static void XmlStream(object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, type, stream);
        }

        public static void XmlStream(object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlStream(source, type, stream, encoding);
        }

        public static void XmlStream(object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlStream(source, type, stream, encoding);
        }

        // To Stream extensions

        public static void SerializeXmlStream(this object source, Stream stream, Options options)
        {
            XmlStream(source, stream, options);
        }

        public static void SerializeXmlStream(this object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, stream, configure);
        }

        public static void SerializeXmlStream(this object source, Stream stream, Encoding encoding, Options options)
        {
            XmlStream(source, stream, encoding, options);
        }

        public static void SerializeXmlStream(this object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, stream, encoding, configure);
        }

        public static void SerializeXmlStream<T>(this T source, Stream stream, Options options)
        {
            XmlStream(source, stream, options);
        }

        public static void SerializeXmlStream<T>(this T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, stream, configure);
        }

        public static void SerializeXmlStream<T>(this T source, Stream stream, Encoding encoding, Options options)
        {
            XmlStream(source, stream, encoding, options);
        }

        public static void SerializeXmlStream<T>(this T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, stream, encoding, configure);
        }

        public static void SerializeXmlStream(this object source, Type type, Stream stream, Options options)
        {
            XmlStream(source, type, stream, options);
        }

        public static void SerializeXmlStream(this object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, type, stream, configure);
        }

        public static void SerializeXmlStream(this object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            XmlStream(source, type, stream, encoding, options);
        }

        public static void SerializeXmlStream(this object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlStream(source, type, stream, encoding, configure);
        }

        // File

        public static void XmlFile(object source, string path, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, path);
        }

        public static void XmlFile(object source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, path);
        }

        public static void XmlFile(object source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, path, encoding);
        }

        public static void XmlFile(object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, path, encoding);
        }

        public static void XmlFile<T>(T source, string path, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, path);
        }

        public static void XmlFile<T>(T source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, path);
        }

        public static void XmlFile<T>(T source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, path, encoding);
        }

        public static void XmlFile<T>(T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, path, encoding);
        }

        public static void XmlFile(object source, Type type, string path, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, type, path);
        }

        public static void XmlFile(object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, type, path);
        }

        public static void XmlFile(object source, Type type, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeXmlFile(source, type, path, encoding);
        }

        public static void XmlFile(object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeXmlFile(source, type, path, encoding);
        }

        // File extensions

        public static void SerializeXmlFile(this object source, string path, Options options)
        {
            XmlFile(source, path, options);
        }

        public static void SerializeXmlFile(this object source, string path, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, path, configure);
        }

        public static void SerializeXmlFile(this object source, string path, Encoding encoding, Options options)
        {
            XmlFile(source, path, encoding, options);
        }

        public static void SerializeXmlFile(this object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, path, encoding, configure);
        }

        public static void SerializeXmlFile<T>(this T source, string path, Options options)
        {
            XmlFile(source, path, options);
        }

        public static void SerializeXmlFile<T>(this T source, string path, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, path, configure);
        }

        public static void SerializeXmlFile<T>(this T source, string path, Encoding encoding, Options options)
        {
            XmlFile(source, path, encoding, options);
        }

        public static void SerializeXmlFile<T>(this T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, path, encoding, configure);
        }

        public static void SerializeXmlFile(this object source, Type type, string path, Options options)
        {
            XmlFile(source, type, path, options);
        }

        public static void SerializeXmlFile(this object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, type, path, configure);
        }

        public static void SerializeXmlFile(this object source, Type type, string path, Encoding encoding, Options options)
        {
            XmlFile(source, type, path, encoding, options);
        }

        public static void SerializeXmlFile(this object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            XmlFile(source, type, path, encoding, configure);
        }

        // Nodes

        public static ElementNode XmlNodes(object source, Options options)
        {
            return Serializer.Create(options).SerializeXmlNodes(source);
        }

        public static ElementNode XmlNodes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlNodes(source);
        }

        public static ElementNode XmlNodes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeXmlNodes(source);
        }

        public static ElementNode XmlNodes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlNodes(source);
        }

        public static ElementNode XmlNodes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeXmlNodes(source, type);
        }

        public static ElementNode XmlNodes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlNodes(source, type);
        }

        // Nodes extensions

        public static ElementNode SerializeXmlNodes(this object source, Options options)
        {
            return XmlNodes(source, options);
        }

        public static ElementNode SerializeXmlNodes(this object source, Action<OptionsDsl> configure = null)
        {
            return XmlNodes(source, configure);
        }

        public static ElementNode SerializeXmlNodes<T>(this T source, Options options)
        {
            return XmlNodes(source, options);
        }

        public static ElementNode SerializeXmlNodes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return XmlNodes(source, configure);
        }

        public static ElementNode SerializeXmlNodes(this object source, Type type, Options options)
        {
            return XmlNodes(source, type, options);
        }

        public static ElementNode SerializeXmlNodes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlNodes(source, type, configure);
        }
    }
}
