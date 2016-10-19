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

        // String extensions

        public static T DeserializeXml<T>(this string xml, Options options)
        {
            return Xml<T>(xml, options);
        }

        public static T DeserializeXml<T>(this string xml, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(xml, configure);
        }

        public static object DeserializeXml(this string xml, Type type, Options options)
        {
            return Xml(xml, type, options);
        }

        public static object DeserializeXml(this string xml, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(xml, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this string xml, Options options)
        {
            return Xml(xml, options);
        }

        public static XmlNodeBase DeserializeXml(this string xml, Action<OptionsDsl> configure = null)
        {
            return Xml(xml, configure);
        }

        public static T DeserializeXml<T>(this string xml, string xsl, Options options)
        {
            return Xml<T>(xml, xsl, options);
        }

        public static T DeserializeXml<T>(this string xml, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(xml, xsl, configure);
        }

        public static object DeserializeXml(this string xml, string xsl, Type type, Options options)
        {
            return Xml(xml, xsl, type, options);
        }

        public static object DeserializeXml(this string xml, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(xml, xsl, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this string xml, string xsl, Options options)
        {
            return Xml(xml, xsl, options);
        }

        public static XmlNodeBase DeserializeXml(this string xml, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml(xml, xsl, configure);
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

        // XDocument extensions

        public static T DeserializeXml<T>(this XDocument document, Options options)
        {
            return Xml<T>(document, options);
        }

        public static T DeserializeXml<T>(this XDocument document, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(document, configure);
        }

        public static object DeserializeXml(this XDocument document, Type type, Options options)
        {
            return Xml(document, type, options);
        }

        public static object DeserializeXml(this XDocument document, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(document, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this XDocument document, Options options)
        {
            return Xml(document, options);
        }

        public static XmlNodeBase DeserializeXml(this XDocument document, Action<OptionsDsl> configure = null)
        {
            return Xml(document, configure);
        }

        public static T DeserializeXml<T>(this XDocument document, string xsl, Options options)
        {
            return Xml<T>(document, xsl, options);
        }

        public static T DeserializeXml<T>(this XDocument document, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(document, xsl, configure);
        }

        public static object DeserializeXml(this XDocument document, string xsl, Type type, Options options)
        {
            return Xml(document, xsl, type, options);
        }

        public static object DeserializeXml(this XDocument document, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(document, xsl, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this XDocument document, string xsl, Options options)
        {
            return Xml(document, xsl, options);
        }

        public static XmlNodeBase DeserializeXml(this XDocument document, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml(document, xsl, configure);
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

        // XElement extensions

        public static T DeserializeXml<T>(this XElement element, Options options)
        {
            return Xml<T>(element, options);
        }

        public static T DeserializeXml<T>(this XElement element, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(element, configure);
        }

        public static object DeserializeXml(this XElement element, Type type, Options options)
        {
            return Xml(element, type, options);
        }

        public static object DeserializeXml(this XElement element, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(element, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this XElement element, Options options)
        {
            return Xml(element, options);
        }

        public static XmlNodeBase DeserializeXml(this XElement element, Action<OptionsDsl> configure = null)
        {
            return Xml(element, configure);
        }

        public static T DeserializeXml<T>(this XElement element, string xsl, Options options)
        {
            return Xml<T>(element, xsl, options);
        }

        public static T DeserializeXml<T>(this XElement element, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(element, xsl, configure);
        }

        public static object DeserializeXml(this XElement element, string xsl, Type type, Options options)
        {
            return Xml(element, xsl, type, options);
        }

        public static object DeserializeXml(this XElement element, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(element, xsl, type, configure);
        }

        public static XmlNodeBase DeserializeXml(this XElement element, string xsl, Options options)
        {
            return Xml(element, xsl, options);
        }

        public static XmlNodeBase DeserializeXml(this XElement element, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml(element, xsl, configure);
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

        // Bytes extensions

        public static T DeserializeXml<T>(this byte[] bytes, Options options)
        {
            return Xml<T>(bytes, options);
        }

        public static T DeserializeXml<T>(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(bytes, configure);
        }

        public static T DeserializeXml<T>(this byte[] bytes, Encoding encoding, Options options)
        {
            return Xml<T>(bytes, encoding, options);
        }

        public static T DeserializeXml<T>(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(bytes, encoding, configure);
        }

        public static object DeserializeXml(this byte[] bytes, Type type, Options options)
        {
            return Xml(bytes, type, options);
        }

        public static object DeserializeXml(this byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, type, configure);
        }

        public static object DeserializeXml(this byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Xml(bytes, type, encoding, options);
        }

        public static object DeserializeXml(this byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, Options options)
        {
            return Xml(bytes, options);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, configure);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, Encoding encoding, Options options)
        {
            return Xml(bytes, encoding, options);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, encoding, configure);
        }

        public static T DeserializeXml<T>(this byte[] bytes, string xsl, Options options)
        {
            return Xml<T>(bytes, xsl, options);
        }

        public static T DeserializeXml<T>(this byte[] bytes, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(bytes, xsl, configure);
        }

        public static T DeserializeXml<T>(this byte[] bytes, string xsl, Encoding encoding, Options options)
        {
            return Xml<T>(bytes, xsl, encoding, options);
        }

        public static T DeserializeXml<T>(this byte[] bytes, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(bytes, xsl, encoding, configure);
        }

        public static object DeserializeXml(this byte[] bytes, string xsl, Type type, Options options)
        {
            return Xml(bytes, xsl, type, options);
        }

        public static object DeserializeXml(this byte[] bytes, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, xsl, type, configure);
        }

        public static object DeserializeXml(this byte[] bytes, string xsl, Type type, Encoding encoding, Options options)
        {
            return Xml(bytes, xsl, type, encoding, options);
        }

        public static object DeserializeXml(this byte[] bytes, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, xsl, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, string xsl, Options options)
        {
            return Xml(bytes, xsl, options);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, xsl, configure);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, string xsl, Encoding encoding, Options options)
        {
            return Xml(bytes, xsl, encoding, options);
        }

        public static XmlNodeBase DeserializeXml(this byte[] bytes, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(bytes, xsl, encoding, configure);
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

        // Stream extensions

        public static T DeserializeXml<T>(this Stream stream, Options options)
        {
            return Xml<T>(stream, options);
        }

        public static T DeserializeXml<T>(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(stream, configure);
        }

        public static T DeserializeXml<T>(this Stream stream, Encoding encoding, Options options)
        {
            return Xml<T>(stream, encoding, options);
        }

        public static T DeserializeXml<T>(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(stream, encoding, configure);
        }

        public static object DeserializeXml(this Stream stream, Type type, Options options)
        {
            return Xml(stream, type, options);
        }

        public static object DeserializeXml(this Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, type, configure);
        }

        public static object DeserializeXml(this Stream stream, Type type, Encoding encoding, Options options)
        {
            return Xml(stream, type, encoding, options);
        }

        public static object DeserializeXml(this Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, Options options)
        {
            return Xml(stream, options);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, configure);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, Encoding encoding, Options options)
        {
            return Xml(stream, encoding, options);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, encoding, configure);
        }

        public static T DeserializeXml<T>(this Stream stream, string xsl, Options options)
        {
            return Xml<T>(stream, xsl, options);
        }

        public static T DeserializeXml<T>(this Stream stream, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(stream, xsl, configure);
        }

        public static T DeserializeXml<T>(this Stream stream, string xsl, Encoding encoding, Options options)
        {
            return Xml<T>(stream, xsl, encoding, options);
        }

        public static T DeserializeXml<T>(this Stream stream, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml<T>(stream, xsl, encoding, configure);
        }

        public static object DeserializeXml(this Stream stream, string xsl, Type type, Options options)
        {
            return Xml(stream, xsl, type, options);
        }

        public static object DeserializeXml(this Stream stream, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, xsl, type, configure);
        }

        public static object DeserializeXml(this Stream stream, string xsl, Type type, Encoding encoding, Options options)
        {
            return Xml(stream, xsl, type, encoding, options);
        }

        public static object DeserializeXml(this Stream stream, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, xsl, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, string xsl, Options options)
        {
            return Xml(stream, xsl, options);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, string xsl, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, xsl, configure);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, string xsl, Encoding encoding, Options options)
        {
            return Xml(stream, xsl, encoding, options);
        }

        public static XmlNodeBase DeserializeXml(this Stream stream, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Xml(stream, xsl, encoding, configure);
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

        // File extensions

        public static T DeserializeXmlFile<T>(this string path, Options options)
        {
            return XmlFile<T>(path, options);
        }

        public static T DeserializeXmlFile<T>(this string path, Action<OptionsDsl> configure = null)
        {
            return XmlFile<T>(path, configure);
        }

        public static T DeserializeXmlFile<T>(this string path, Encoding encoding, Options options)
        {
            return XmlFile<T>(path, encoding, options);
        }

        public static T DeserializeXmlFile<T>(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile<T>(path, encoding, configure);
        }

        public static object DeserializeXmlFile(this string path, Type type, Options options)
        {
            return XmlFile(path, type, options);
        }

        public static object DeserializeXmlFile(this string path, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, type, configure);
        }

        public static object DeserializeXmlFile(this string path, Type type, Encoding encoding, Options options)
        {
            return XmlFile(path, type, encoding, options);
        }

        public static object DeserializeXmlFile(this string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, Options options)
        {
            return XmlFile(path, options);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, configure);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, Encoding encoding, Options options)
        {
            return XmlFile(path, encoding, options);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, encoding, configure);
        }

        public static T DeserializeXmlFile<T>(this string path, string xsl, Options options)
        {
            return XmlFile<T>(path, xsl, options);
        }

        public static T DeserializeXmlFile<T>(this string path, string xsl, Action<OptionsDsl> configure = null)
        {
            return XmlFile<T>(path, xsl, configure);
        }

        public static T DeserializeXmlFile<T>(this string path, string xsl, Encoding encoding, Options options)
        {
            return XmlFile<T>(path, xsl, encoding, options);
        }

        public static T DeserializeXmlFile<T>(this string path, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile<T>(path, xsl, encoding, configure);
        }

        public static object DeserializeXmlFile(this string path, string xsl, Type type, Options options)
        {
            return XmlFile(path, xsl, type, options);
        }

        public static object DeserializeXmlFile(this string path, string xsl, Type type, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, xsl, type, configure);
        }

        public static object DeserializeXmlFile(this string path, string xsl, Type type, Encoding encoding, Options options)
        {
            return XmlFile(path, xsl, type, encoding, options);
        }

        public static object DeserializeXmlFile(this string path, string xsl, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, xsl, type, encoding, configure);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, string xsl, Options options)
        {
            return XmlFile(path, xsl, options);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, string xsl, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, xsl, configure);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, string xsl, Encoding encoding, Options options)
        {
            return XmlFile(path, xsl, encoding, options);
        }

        public static XmlNodeBase DeserializeXmlFile(this string path, string xsl, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return XmlFile(path, xsl, encoding, configure);
        }
    }
}
