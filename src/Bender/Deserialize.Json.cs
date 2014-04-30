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
    }
}
