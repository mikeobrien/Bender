using System;
using System.IO;
using System.Text;
using Bender.Extensions;
using Bender.Nodes;

namespace Bender
{
    public partial class Serializer
    {
        // String 

        public string SerializeJson(object source)
        {
            return SerializeJson(source, source.GetType());
        }

        public string SerializeJson<T>(T source)
        {
            return SerializeJson(source, typeof(T));
        }

        public string SerializeJson(object source, Type type)
        {
            return SerializeString(source, type, (s, o) => 
                new JsonNode(s.NodeType, o), JsonNode.NodeFormat);
        }

        // Bytes

        public byte[] SerializeJsonBytes(object source, Encoding encoding = null)
        {
            return SerializeJsonBytes(source, source.GetType(), encoding);
        }

        public byte[] SerializeJsonBytes<T>(T source, Encoding encoding = null)
        {
            return SerializeJsonBytes(source, typeof(T), encoding);
        }

        public byte[] SerializeJsonBytes(object source, Type type, Encoding encoding = null)
        {
            return SerializeBytes(source, type, (s, o) => 
                new JsonNode(s.NodeType, o), JsonNode.NodeFormat, encoding);
        }

        // Return Stream

        public Stream SerializeJsonStream(object source, Encoding encoding = null)
        {
            return SerializeJsonStream(source, source.GetType(), encoding);
        }

        public Stream SerializeJsonStream<T>(T source, Encoding encoding = null)
        {
            return SerializeJsonStream(source, typeof(T), encoding);
        }

        public Stream SerializeJsonStream(object source, Type type, Encoding encoding = null)
        {
            return SerializeStream(source, type, (s, o) => new JsonNode(s.NodeType, o),  
                JsonNode.NodeFormat, encoding);
        }

        // To Stream

        public void SerializeJsonStream(object source, Stream stream, Encoding encoding = null)
        {
            SerializeJsonStream(source, source.GetType(), stream, encoding);
        }

        public void SerializeJsonStream<T>(T source, Stream stream, Encoding encoding = null)
        {
            SerializeJsonStream(source, typeof(T), stream, encoding);
        }

        public void SerializeJsonStream(object source, Type type, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new JsonNode(s.NodeType, o), 
                JsonNode.NodeFormat, stream, encoding);
        }

        // File

        public void SerializeJsonFile(object source, string path, Encoding encoding = null)
        {
            SerializeJsonFile(source, source.GetType(), path, encoding);
        }

        public void SerializeJsonFile<T>(T source, string path, Encoding encoding = null)
        {
            SerializeJsonFile(source, typeof(T), path, encoding);
        }

        public void SerializeJsonFile(object source, Type type, string path, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new JsonNode(s.NodeType, o), 
                JsonNode.NodeFormat, encoding).SaveToFile(path);
        }

        // Nodes

        public JsonNode SerializeJsonNodes(object source)
        {
            return SerializeJsonNodes(source, source.GetType());
        }

        public JsonNode SerializeJsonNodes<T>(T source)
        {
            return SerializeJsonNodes(source, typeof(T));
        }

        public JsonNode SerializeJsonNodes(object source, Type type)
        {
            return (JsonNode)SerializeNodes(source, type, (s, o) => 
                new JsonNode(s.NodeType, o), JsonNode.NodeFormat);
        }
    }
}
