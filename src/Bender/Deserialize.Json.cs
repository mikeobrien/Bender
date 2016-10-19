using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;

namespace Bender
{
    public static partial class Deserialize
    {
        // String 

        public static T Json<T>(string json, Options options)
        {
            return Deserializer.Create(options).DeserializeJson<T>(json);
        }

        public static T Json<T>(string json, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(json);
        }

        public static object Json(string json, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(json, type);
        }

        public static object Json(string json, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(json, type);
        }

        public static JsonNode Json(string json, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(json);
        }

        public static JsonNode Json(string json, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(json);
        }

        // String extensions

        public static T DeserializeJson<T>(this string json, Options options)
        {
            return Json<T>(json, options);
        }

        public static T DeserializeJson<T>(this string json, Action<OptionsDsl> configure = null)
        {
            return Json<T>(json, configure);
        }

        public static object DeserializeJson(this string json, Type type, Options options)
        {
            return Json(json, type, options);
        }

        public static object DeserializeJson(this string json, Type type, Action<OptionsDsl> configure = null)
        {
            return Json(json, type, configure);
        }

        public static JsonNode DeserializeJson(this string json, Options options)
        {
            return Json(json, options);
        }

        public static JsonNode DeserializeJson(this string json, Action<OptionsDsl> configure = null)
        {
            return Json(json, configure);
        }

        // Bytes

        public static T Json<T>(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(bytes);
        }

        public static T Json<T>(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeJson<T>(bytes);
        }

        public static T Json<T>(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(bytes, encoding);
        }

        public static T Json<T>(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson<T>(bytes, encoding);
        }

        public static object Json(byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(bytes, type);
        }

        public static object Json(byte[] bytes, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(bytes, type);
        }

        public static JsonNode Json(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(bytes);
        }

        public static JsonNode Json(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(bytes);
        }

        public static object Json(byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(bytes, type, encoding);
        }

        public static object Json(byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(bytes, type, encoding);
        }

        public static JsonNode Json(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(bytes, encoding);
        }

        public static JsonNode Json(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(bytes, encoding);
        }

        // Bytes extensions

        public static T DeserializeJson<T>(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Json<T>(bytes, configure);
        }

        public static T DeserializeJson<T>(this byte[] bytes, Options options)
        {
            return Json<T>(bytes, options);
        }

        public static T DeserializeJson<T>(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json<T>(bytes, encoding, configure);
        }

        public static T DeserializeJson<T>(this byte[] bytes, Encoding encoding, Options options)
        {
            return Json<T>(bytes, encoding, options);
        }

        public static object DeserializeJson(this byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Json(bytes, type, configure);
        }

        public static object DeserializeJson(this byte[] bytes, Type type, Options options)
        {
            return Json(bytes, type, options);
        }

        public static JsonNode DeserializeJson(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Json(bytes, configure);
        }

        public static JsonNode DeserializeJson(this byte[] bytes, Options options)
        {
            return Json(bytes, options);
        }

        public static object DeserializeJson(this byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json(bytes, type, encoding, configure);
        }

        public static object DeserializeJson(this byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Json(bytes, type, encoding, options);
        }

        public static JsonNode DeserializeJson(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json(bytes, encoding, configure);
        }

        public static JsonNode DeserializeJson(this byte[] bytes, Encoding encoding, Options options)
        {
            return Json(bytes, encoding, options);
        }

        // Stream

        public static T Json<T>(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(stream);
        }

        public static T Json<T>(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeJson<T>(stream);
        }

        public static T Json<T>(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson<T>(stream, encoding);
        }

        public static T Json<T>(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson<T>(stream, encoding);
        }

        public static object Json(Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(stream, type);
        }

        public static object Json(Stream stream, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(stream, type);
        }

        public static JsonNode Json(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(stream);
        }

        public static JsonNode Json(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(stream);
        }

        public static object Json(Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(stream, type, encoding);
        }

        public static object Json(Stream stream, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(stream, type, encoding);
        }

        public static JsonNode Json(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJson(stream, encoding);
        }

        public static JsonNode Json(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJson(stream, encoding);
        }

        // Stream extensions

        public static T DeserializeJson<T>(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Json<T>(stream, configure);
        }

        public static T DeserializeJson<T>(this Stream stream, Options options)
        {
            return Json<T>(stream, options);
        }

        public static T DeserializeJson<T>(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json<T>(stream, encoding, configure);
        }

        public static T DeserializeJson<T>(this Stream stream, Encoding encoding, Options options)
        {
            return Json<T>(stream, encoding, options);
        }

        public static object DeserializeJson(this Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Json(stream, type, configure);
        }

        public static object DeserializeJson(this Stream stream, Type type, Options options)
        {
            return Json(stream, type, options);
        }

        public static JsonNode DeserializeJson(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Json(stream, configure);
        }

        public static JsonNode DeserializeJson(this Stream stream, Options options)
        {
            return Json(stream, options);
        }

        public static object DeserializeJson(this Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json(stream, type, encoding, configure);
        }

        public static object DeserializeJson(this Stream stream, Type type, Encoding encoding, Options options)
        {
            return Json(stream, type, encoding, options);
        }

        public static JsonNode DeserializeJson(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Json(stream, encoding, configure);
        }

        public static JsonNode DeserializeJson(this Stream stream, Encoding encoding, Options options)
        {
            return Json(stream, encoding, options);
        }

        // File

        public static T JsonFile<T>(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile<T>(path);
        }

        public static T JsonFile<T>(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile<T>(path);
        }

        public static T JsonFile<T>(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile<T>(path, encoding);
        }

        public static T JsonFile<T>(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile<T>(path, encoding);
        }

        public static object JsonFile(string path, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile(path, type);
        }

        public static object JsonFile(string path, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile(path, type);
        }

        public static JsonNode JsonFile(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile(path);
        }

        public static JsonNode JsonFile(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile(path);
        }

        public static object JsonFile(string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile(path, type, encoding);
        }

        public static object JsonFile(string path, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile(path, type, encoding);
        }

        public static JsonNode JsonFile(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeJsonFile(path, encoding);
        }

        public static JsonNode JsonFile(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeJsonFile(path, encoding);
        }

        // File extensions

        public static T DeserializeJsonFile<T>(this string path, Action<OptionsDsl> configure = null)
        {
            return JsonFile<T>(path, configure);
        }

        public static T DeserializeJsonFile<T>(this string path, Options options)
        {
            return JsonFile<T>(path, options);
        }

        public static T DeserializeJsonFile<T>(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonFile<T>(path, encoding, configure);
        }

        public static T DeserializeJsonFile<T>(this string path, Encoding encoding, Options options)
        {
            return JsonFile<T>(path, encoding, options);
        }

        public static object DeserializeJsonFile(this string path, Type type, Action<OptionsDsl> configure = null)
        {
            return JsonFile(path, type, configure);
        }

        public static object DeserializeJsonFile(this string path, Type type, Options options)
        {
            return JsonFile(path, type, options);
        }

        public static JsonNode DeserializeJsonFile(this string path, Action<OptionsDsl> configure = null)
        {
            return JsonFile(path, configure);
        }

        public static JsonNode DeserializeJsonFile(this string path, Options options)
        {
            return JsonFile(path, options);
        }

        public static object DeserializeJsonFile(this string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonFile(path, type, encoding, configure);
        }

        public static object DeserializeJsonFile(this string path, Type type, Encoding encoding, Options options)
        {
            return JsonFile(path, type, encoding, options);
        }

        public static JsonNode DeserializeJsonFile(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return JsonFile(path, encoding, configure);
        }

        public static JsonNode DeserializeJsonFile(this string path, Encoding encoding, Options options)
        {
            return JsonFile(path, encoding, options);
        }
    }
}
