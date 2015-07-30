using System;
using System.IO;
using System.Text;
using Bender.Extensions;
using Bender.Nodes.FormUrlEncoded;

namespace Bender
{
    public partial class Serializer
    {
        // String 

        public string SerializeFormUrlEncoded(object source)
        {
            return SerializeFormUrlEncoded(source, source.GetType());
        }

        public string SerializeFormUrlEncoded<T>(T source)
        {
            return SerializeFormUrlEncoded(source, typeof(T));
        }

        public string SerializeFormUrlEncoded(object source, Type type)
        {
            return SerializeString(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat);
        }

        // Bytes

        public byte[] SerializeFormUrlEncodedBytes(object source, Encoding encoding = null)
        {
            return SerializeFormUrlEncodedBytes(source, source.GetType(), encoding);
        }

        public byte[] SerializeFormUrlEncodedBytes<T>(T source, Encoding encoding = null)
        {
            return SerializeFormUrlEncodedBytes(source, typeof(T), encoding);
        }

        public byte[] SerializeFormUrlEncodedBytes(object source, Type type, Encoding encoding = null)
        {
            return SerializeBytes(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat, encoding);
        }

        // Return Stream

        public Stream SerializeFormUrlEncodedStream(object source, Encoding encoding = null)
        {
            return SerializeFormUrlEncodedStream(source, source.GetType(), encoding);
        }

        public Stream SerializeFormUrlEncodedStream<T>(T source, Encoding encoding = null)
        {
            return SerializeFormUrlEncodedStream(source, typeof(T), encoding);
        }

        public Stream SerializeFormUrlEncodedStream(object source, Type type, Encoding encoding = null)
        {
            return SerializeStream(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat, encoding);
        }

        // To Stream

        public void SerializeFormUrlEncodedStream(object source, Stream stream, Encoding encoding = null)
        {
            SerializeFormUrlEncodedStream(source, source.GetType(), stream, encoding);
        }

        public void SerializeFormUrlEncodedStream<T>(T source, Stream stream, Encoding encoding = null)
        {
            SerializeFormUrlEncodedStream(source, typeof(T), stream, encoding);
        }

        public void SerializeFormUrlEncodedStream(object source, Type type, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat, stream, encoding);
        }

        // File

        public void SerializeFormUrlEncodedFile(object source, string path, Encoding encoding = null)
        {
            SerializeFormUrlEncodedFile(source, source.GetType(), path, encoding);
        }

        public void SerializeFormUrlEncodedFile<T>(T source, string path, Encoding encoding = null)
        {
            SerializeFormUrlEncodedFile(source, typeof(T), path, encoding);
        }

        public void SerializeFormUrlEncodedFile(object source, Type type, string path, Encoding encoding = null)
        {
            SerializeStream(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat, encoding).SaveToFile(path);
        }

        // Nodes

        public FormNode SerializeFormUrlEncodedNodes(object source)
        {
            return SerializeFormUrlEncodedNodes(source, source.GetType());
        }

        public FormNode SerializeFormUrlEncodedNodes<T>(T source)
        {
            return SerializeFormUrlEncodedNodes(source, typeof(T));
        }

        public FormNode SerializeFormUrlEncodedNodes(object source, Type type)
        {
            return (FormNode)SerializeNodes(source, type, (s, o) => new FormNode(s.NodeType), FormNode.NodeFormat);
        }
    }
}
