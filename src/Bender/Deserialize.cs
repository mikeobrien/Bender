using System;
using System.IO;
using System.Xml.Linq;

namespace Bender
{
    public static class Deserialize
    {

        public static T JsonFile<T>(string path, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile<T>(path);
        }

        public static object JsonFile(Type type, string path, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile(type, path);
        }

        public static T Json<T>(string source, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(source);
        }

        public static object Json(Type type, string source, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(type, source);
        }

        public static T Json<T>(Stream stream, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(stream);
        }

        public static object Json(Type type, Stream stream, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(type, stream);
        }

        public static T XmlFile<T>(string path, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile<T>(path);
        }

        public static object XmlFile(Type type, string path, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXmlFile(type, path);
        }

        public static T Xml<T>(Stream stream, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(stream);
        }

        public static object Xml(Type type, Stream stream, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(type, stream);
        }

        public static T Xml<T>(string source, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(source);
        }

        public static object Xml(Type type, string source, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(type, source);
        }

        public static T Xml<T>(XDocument document, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(document);
        }

        public static object Xml(Type type, XDocument document, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(type, document);
        }

        public static T Xml<T>(XElement element, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml<T>(element);
        }

        public static object Xml(Type type, XElement element, Action<DeserializerOptions> configure = null)
        {
            return Deserializer.Create(configure).DeserializeXml(type, element);
        }
    }
}
