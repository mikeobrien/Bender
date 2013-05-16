using System;
using System.IO;
using System.Xml.Linq;

namespace Bender
{
    public static class Serialize
    {
        public static Stream Json(object @object, Stream stream, Action<SerializerOptions> configure = null)
        {
            return Serializer.Create(configure).SerializeJson(@object, stream);
        }

        public static void Json(object @object, string path, Action<SerializerOptions> configure = null)
        {
            Serializer.Create(configure).SerializeJson(@object, path);
        }

        public static string Json(object @object, Action<SerializerOptions> configure = null)
        {
            return Serializer.Create(configure).SerializeJson(@object);
        }

        public static Stream Xml(object @object, Stream stream, Action<SerializerOptions> configure = null)
        {
            return Serializer.Create(configure).SerializeXml(@object, stream);
        }

        public static void Xml(object @object, string path, Action<SerializerOptions> configure = null)
        {
            Serializer.Create(configure).SerializeXml(@object, path);
        }

        public static string Xml(object @object, Action<SerializerOptions> configure = null)
        {
            return Serializer.Create(configure).SerializeXml(@object);
        }

        public static XDocument XmlAsDocument(object source, Action<SerializerOptions> configure = null)
        {
            return Serializer.Create(configure).SerializeXmlAsDocument(source);
        }
    }
}
