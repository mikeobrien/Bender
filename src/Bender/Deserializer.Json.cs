using System;
using System.IO;
using System.Text;
using Bender.Nodes;

namespace Bender
{
    public partial class Deserializer
    {
        // String

        public T DeserializeJson<T>(string json)
        {
            return (T)DeserializeJson(json, typeof(T));
        }

        public object DeserializeJson(string json, Type type)
        {
            return Deserialize(new JsonNode(json), type);
        }

        public JsonNode DeserializeJson(string json)
        {
            return new JsonNode(json);
        }

        // Bytes

        public T DeserializeJson<T>(byte[] bytes, Encoding encoding = null)
        {
            return (T)DeserializeJson(bytes, typeof(T), encoding);
        }

        public object DeserializeJson(byte[] bytes, Type type, Encoding encoding = null)
        {
            return Deserialize(new JsonNode(bytes, encoding ?? Encoding.UTF8), type);
        }

        public JsonNode DeserializeJson(byte[] bytes, Encoding encoding = null)
        {
            return new JsonNode(bytes, encoding ?? Encoding.UTF8);
        }

        // Stream

        public T DeserializeJson<T>(Stream stream, Encoding encoding = null)
        {
            return (T)DeserializeJson(stream, typeof(T), encoding);
        }

        public object DeserializeJson(Stream stream, Type type, Encoding encoding = null)
        {
            return Deserialize(new JsonNode(stream, encoding ?? Encoding.UTF8), type);
        }

        public JsonNode DeserializeJson(Stream stream, Encoding encoding = null)
        {
            return new JsonNode(stream, encoding ?? Encoding.UTF8);
        }

        // File

        public T DeserializeJsonFile<T>(string path, Encoding encoding = null)
        {
            return (T)DeserializeJsonFile(path, typeof(T), encoding);
        }

        public object DeserializeJsonFile(string path, Type type, Encoding encoding = null)
        {
            return DeserializeJson(File.ReadAllBytes(path), type, encoding);
        }

        public JsonNode DeserializeJsonFile(string path, Encoding encoding = null)
        {
            return DeserializeJson(File.ReadAllBytes(path), encoding);
        }
    }
}
