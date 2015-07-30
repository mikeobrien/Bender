using System;
using System.IO;
using System.Text;
using Bender.Extensions;
using Bender.Nodes.CharacterSeparated;

namespace Bender
{
    public partial class Serializer
    {
        // String 

        public string SerializeCsv(object source)
        {
            return SerializeCsv(source, source.GetType());
        }

        public string SerializeCsv<T>(T source)
        {
            return SerializeCsv(source, typeof(T));
        }

        public string SerializeCsv(object source, Type type)
        {
            return SerializeString(source, type, (s, o) => 
                new FileNode(s.NodeType, type, o), FileNode.NodeFormat);
        }

        // Bytes

        public byte[] SerializeCsvBytes(object source, Encoding encoding = null)
        {
            return SerializeCsvBytes(source, source.GetType(), encoding);
        }

        public byte[] SerializeCsvBytes<T>(T source, Encoding encoding = null)
        {
            return SerializeCsvBytes(source, typeof(T), encoding);
        }

        public byte[] SerializeCsvBytes(object source, Type type, Encoding encoding = null)
        {
            return SerializeBytes(source, type, (s, o) => new FileNode(
                s.NodeType, type, o), FileNode.NodeFormat, encoding);
        }

        // Return Stream

        public Stream SerializeCsvStream(object source, Encoding encoding = null)
        {
            return SerializeCsvStream(source, source.GetType(), encoding);
        }

        public Stream SerializeCsvStream<T>(T source, Encoding encoding = null)
        {
            return SerializeCsvStream(source, typeof(T), encoding);
        }

        public Stream SerializeCsvStream(object source, Type type, Encoding encoding = null)
        {
            return SerializeStream(source, type, (s, o) => new FileNode(
                s.NodeType, type, o), FileNode.NodeFormat, encoding);
        }

        // To Stream

        public void SerializeCsvStream(object source, Stream stream, Encoding encoding = null)
        {
            SerializeCsvStream(source, source.GetType(), stream, encoding);
        }

        public void SerializeCsvStream<T>(T source, Stream stream, Encoding encoding = null)
        {
            SerializeCsvStream(source, typeof(T), stream, encoding);
        }

        public void SerializeCsvStream(object source, Type type, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new FileNode(s.NodeType, type, o), 
                FileNode.NodeFormat, stream, encoding);
        }

        // File

        public void SerializeCsvFile(object source, string path, Encoding encoding = null)
        {
            SerializeCsvFile(source, source.GetType(), path, encoding);
        }

        public void SerializeCsvFile<T>(T source, string path, Encoding encoding = null)
        {
            SerializeCsvFile(source, typeof(T), path, encoding);
        }

        public void SerializeCsvFile(object source, Type type, string path, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new FileNode(s.NodeType, type, o), 
                FileNode.NodeFormat, encoding).SaveToFile(path);
        }

        // Nodes

        public FileNode SerializeCsvNodes(object source)
        {
            return SerializeCsvNodes(source, source.GetType());
        }

        public FileNode SerializeCsvNodes<T>(T source)
        {
            return SerializeCsvNodes(source, typeof(T));
        }

        public FileNode SerializeCsvNodes(object source, Type type)
        {
            return (FileNode)SerializeNodes(source, type, (s, o) => 
                new FileNode(s.NodeType, type, o), FileNode.NodeFormat);
        }
    }
}
