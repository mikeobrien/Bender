using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;

namespace Bender
{
    public static partial class Serialize
    {
        // String

        public static string Json(object source, Options options)
        {
            return Serializer.Create(options).SerializeJson(source);
        }

        public static string Json(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJson(source);
        }

        public static string Json<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeJson(source);
        }

        public static string Json<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJson(source);
        }

        public static string Json(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeJson(source, type);
        }

        public static string Json(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJson(source, type);
        }

        // Bytes

        public static byte[] JsonBytes(object source, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source);
        }

        public static byte[] JsonBytes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source);
        }

        public static byte[] JsonBytes(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source, encoding);
        }

        public static byte[] JsonBytes(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source, encoding);
        }

        public static byte[] JsonBytes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source);
        }

        public static byte[] JsonBytes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source);
        }

        public static byte[] JsonBytes<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source, encoding);
        }

        public static byte[] JsonBytes<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source, encoding);
        }

        public static byte[] JsonBytes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source, type);
        }

        public static byte[] JsonBytes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source, type);
        }

        public static byte[] JsonBytes(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonBytes(source, type, encoding);
        }

        public static byte[] JsonBytes(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonBytes(source, type, encoding);
        }

        // Return Stream

        public static Stream JsonStream(object source, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source);
        }

        public static Stream JsonStream(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source);
        }

        public static Stream JsonStream(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source, encoding);
        }

        public static Stream JsonStream(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source, encoding);
        }

        public static Stream JsonStream<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source);
        }

        public static Stream JsonStream<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source);
        }

        public static Stream JsonStream<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source, encoding);
        }

        public static Stream JsonStream<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source, encoding);
        }

        public static Stream JsonStream(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source, type);
        }

        public static Stream JsonStream(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source, type);
        }

        public static Stream JsonStream(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeJsonStream(source, type, encoding);
        }

        public static Stream JsonStream(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonStream(source, type, encoding);
        }

        // To Stream

        public static void JsonStream(object source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, stream);
        }

        public static void JsonStream(object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, stream);
        }

        public static void JsonStream(object source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, stream, encoding);
        }

        public static void JsonStream(object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, stream, encoding);
        }

        public static void JsonStream<T>(T source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, stream);
        }

        public static void JsonStream<T>(T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, stream);
        }

        public static void JsonStream<T>(T source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, stream, encoding);
        }

        public static void JsonStream<T>(T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, stream, encoding);
        }

        public static void JsonStream(object source, Type type, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, type, stream);
        }

        public static void JsonStream(object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, type, stream);
        }

        public static void JsonStream(object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonStream(source, type, stream, encoding);
        }

        public static void JsonStream(object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonStream(source, type, stream, encoding);
        }

        // File

        public static void JsonFile(object source, string path, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, path);
        }

        public static void JsonFile(object source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, path);
        }

        public static void JsonFile(object source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, path, encoding);
        }

        public static void JsonFile(object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, path, encoding);
        }

        public static void JsonFile<T>(T source, string path, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, path);
        }

        public static void JsonFile<T>(T source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, path);
        }

        public static void JsonFile<T>(T source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, path, encoding);
        }

        public static void JsonFile<T>(T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, path, encoding);
        }

        public static void JsonFile(object source, Type type, string path, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, type, path);
        }

        public static void JsonFile(object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, type, path);
        }

        public static void JsonFile(object source, Type type, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeJsonFile(source, type, path, encoding);
        }

        public static void JsonFile(object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeJsonFile(source, type, path, encoding);
        }

        // Nodes

        public static JsonNode JsonNodes(object source, Options options)
        {
            return Serializer.Create(options).SerializeJsonNodes(source);
        }

        public static JsonNode JsonNodes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonNodes(source);
        }

        public static JsonNode JsonNodes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeJsonNodes(source);
        }

        public static JsonNode JsonNodes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonNodes(source);
        }

        public static JsonNode JsonNodes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeJsonNodes(source, type);
        }

        public static JsonNode JsonNodes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeJsonNodes(source, type);
        }
    }
}
