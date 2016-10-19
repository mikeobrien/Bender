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

        // String extensions

        public static string SerializeJson(this object source, Options options)
        {
            return Json(source, options);
        }

        public static string SerializeJson(this object source, Action<OptionsDsl> configure = null)
        {
            return Json(source, configure);
        }

        public static string SerializeJson<T>(this T source, Options options)
        {
            return Json(source, options);
        }

        public static string SerializeJson<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return Json(source, configure);
        }

        public static string SerializeJson(this object source, Type type, Options options)
        {
            return Json(source, type, options);
        }

        public static string SerializeJson(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Json(source, type, configure);
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

        // Bytes extensions

        public static byte[] SerializeJsonBytes(this object source, Options options)
        {
            return JsonBytes(source, options);
        }

        public static byte[] SerializeJsonBytes(this object source, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, configure);
        }

        public static byte[] SerializeJsonBytes(this object source, Encoding encoding, Options options)
        {
            return JsonBytes(source, encoding, options);
        }

        public static byte[] SerializeJsonBytes(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, encoding, configure);
        }

        public static byte[] SerializeJsonBytes<T>(this T source, Options options)
        {
            return JsonBytes(source, options);
        }

        public static byte[] SerializeJsonBytes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, configure);
        }

        public static byte[] SerializeJsonBytes<T>(this T source, Encoding encoding, Options options)
        {
            return JsonBytes(source, encoding, options);
        }

        public static byte[] SerializeJsonBytes<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, encoding, configure);
        }

        public static byte[] SerializeJsonBytes(this object source, Type type, Options options)
        {
            return JsonBytes(source, type, options);
        }

        public static byte[] SerializeJsonBytes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, type, configure);
        }

        public static byte[] SerializeJsonBytes(this object source, Type type, Encoding encoding, Options options)
        {
            return JsonBytes(source, type, encoding, options);
        }

        public static byte[] SerializeJsonBytes(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonBytes(source, type, encoding, configure);
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

        // Return Stream extensions

        public static Stream SerializeJsonStream(this object source, Options options)
        {
            return JsonStream(source, options);
        }

        public static Stream SerializeJsonStream(this object source, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, configure);
        }

        public static Stream SerializeJsonStream(this object source, Encoding encoding, Options options)
        {
            return JsonStream(source, encoding, options);
        }

        public static Stream SerializeJsonStream(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, encoding, configure);
        }

        public static Stream SerializeJsonStream<T>(this T source, Options options)
        {
            return JsonStream(source, options);
        }

        public static Stream SerializeJsonStream<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, configure);
        }

        public static Stream SerializeJsonStream<T>(this T source, Encoding encoding, Options options)
        {
            return JsonStream(source, encoding, options);
        }

        public static Stream SerializeJsonStream<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, encoding, configure);
        }

        public static Stream SerializeJsonStream(this object source, Type type, Options options)
        {
            return JsonStream(source, type, options);
        }

        public static Stream SerializeJsonStream(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, type, configure);
        }

        public static Stream SerializeJsonStream(this object source, Type type, Encoding encoding, Options options)
        {
            return JsonStream(source, type, encoding, options);
        }

        public static Stream SerializeJsonStream(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonStream(source, type, encoding, configure);
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

        // To Stream extensions

        public static void SerializeJsonStream(this object source, Stream stream, Options options)
        {
            JsonStream(source, stream, options);
        }

        public static void SerializeJsonStream(this object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, stream, configure);
        }

        public static void SerializeJsonStream(this object source, Stream stream, Encoding encoding, Options options)
        {
            JsonStream(source, stream, encoding, options);
        }

        public static void SerializeJsonStream(this object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, stream, encoding, configure);
        }

        public static void SerializeJsonStream<T>(this T source, Stream stream, Options options)
        {
            JsonStream(source, stream, options);
        }

        public static void SerializeJsonStream<T>(this T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, stream, configure);
        }

        public static void SerializeJsonStream<T>(this T source, Stream stream, Encoding encoding, Options options)
        {
            JsonStream(source, stream, encoding, options);
        }

        public static void SerializeJsonStream<T>(this T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, stream, encoding, configure);
        }

        public static void SerializeJsonStream(this object source, Type type, Stream stream, Options options)
        {
            JsonStream(source, type, stream, options);
        }

        public static void SerializeJsonStream(this object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, type, stream, configure);
        }

        public static void SerializeJsonStream(this object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            JsonStream(source, type, stream, encoding, options);
        }

        public static void SerializeJsonStream(this object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonStream(source, type, stream, encoding, configure);
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

        // File extensions

        public static void SerializeJsonFile(this object source, string path, Options options)
        {
            JsonFile(source, path, options);
        }

        public static void SerializeJsonFile(this object source, string path, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, path, configure);
        }

        public static void SerializeJsonFile(this object source, string path, Encoding encoding, Options options)
        {
            JsonFile(source, path, encoding, options);
        }

        public static void SerializeJsonFile(this object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, path, encoding, configure);
        }

        public static void SerializeJsonFile<T>(this T source, string path, Options options)
        {
            JsonFile(source, path, options);
        }

        public static void SerializeJsonFile<T>(this T source, string path, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, path, configure);
        }

        public static void SerializeJsonFile<T>(this T source, string path, Encoding encoding, Options options)
        {
            JsonFile(source, path, encoding, options);
        }

        public static void SerializeJsonFile<T>(this T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, path, encoding, configure);
        }

        public static void SerializeJsonFile(this object source, Type type, string path, Options options)
        {
            JsonFile(source, type, path, options);
        }

        public static void SerializeJsonFile(this object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, type, path, configure);
        }

        public static void SerializeJsonFile(this object source, Type type, string path, Encoding encoding, Options options)
        {
            JsonFile(source, type, path, encoding, options);
        }

        public static void SerializeJsonFile(this object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            JsonFile(source, type, path, encoding, configure);
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

        // Nodes extensions

        public static JsonNode SerializeJsonNodes(this object source, Options options)
        {
            return JsonNodes(source, options);
        }

        public static JsonNode SerializeJsonNodes(this object source, Action<OptionsDsl> configure = null)
        {
            return JsonNodes(source, configure);
        }

        public static JsonNode SerializeJsonNodes<T>(this T source, Options options)
        {
            return JsonNodes(source, options);
        }

        public static JsonNode SerializeJsonNodes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return JsonNodes(source, configure);
        }

        public static JsonNode SerializeJsonNodes(this object source, Type type, Options options)
        {
            return JsonNodes(source, type, options);
        }

        public static JsonNode SerializeJsonNodes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return JsonNodes(source, type, configure);
        }
    }
}
