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
            return FileNode.SerializeString(source?.GetType(), type, factory => SerializeNodes(
                source, type, factory, FileNode.NodeFormat));
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
            return SerializeCsvStream(source, type, encoding).ReadAllBytes();
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
            return FileNode.SerializeStream(source?.GetType(), type, factory => SerializeNodes(
                source, type, factory, FileNode.NodeFormat), encoding);
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
            FileNode.SerializeStream(stream, source?.GetType(), type, factory => SerializeNodes(
                source, type, factory, FileNode.NodeFormat), encoding);
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
            FileNode.SerializeFile(path, source?.GetType(), type, factory => SerializeNodes(
                source, type, factory, FileNode.NodeFormat), encoding);
        }
    }
}
