using System;
using System.IO;
using System.Text;
using Bender.Nodes.CharacterSeparated;

namespace Bender
{
    public partial class Deserializer
    {
        // String

        public T DeserializeCsv<T>(string json)
        {
            return (T)DeserializeCsv(json, typeof(T));
        }

        public object DeserializeCsv(string json, Type type)
        {
            return Deserialize(new FileNode(json, _options), type);
        }

        public FileNode DeserializeCsv(string json)
        {
            return new FileNode(json, _options);
        }

        // Bytes

        public T DeserializeCsv<T>(byte[] bytes, Encoding encoding = null)
        {
            return (T)DeserializeCsv(bytes, typeof(T), encoding);
        }

        public object DeserializeCsv(byte[] bytes, Type type, Encoding encoding = null)
        {
            return Deserialize(new FileNode(bytes, _options, encoding ?? Encoding.UTF8), type);
        }

        public FileNode DeserializeCsv(byte[] bytes, Encoding encoding = null)
        {
            return new FileNode(bytes, _options, encoding ?? Encoding.UTF8);
        }

        // Stream

        public T DeserializeCsv<T>(Stream stream, Encoding encoding = null)
        {
            return (T)DeserializeCsv(stream, typeof(T), encoding);
        }

        public object DeserializeCsv(Stream stream, Type type, Encoding encoding = null)
        {
            return Deserialize(new FileNode(stream, _options, encoding ?? Encoding.UTF8), type);
        }

        public FileNode DeserializeCsv(Stream stream, Encoding encoding = null)
        {
            return new FileNode(stream, _options, encoding ?? Encoding.UTF8);
        }

        // File

        public T DeserializeCsvFile<T>(string path, Encoding encoding = null)
        {
            return (T)DeserializeCsvFile(path, typeof(T), encoding);
        }

        public object DeserializeCsvFile(string path, Type type, Encoding encoding = null)
        {
            return DeserializeCsv(File.ReadAllBytes(path), type, encoding);
        }

        public FileNode DeserializeCsvFile(string path, Encoding encoding = null)
        {
            return DeserializeCsv(File.ReadAllBytes(path), encoding);
        }
    }
}
