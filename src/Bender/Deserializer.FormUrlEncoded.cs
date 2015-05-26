using System;
using System.IO;
using System.Text;
using Bender.Nodes;
using Bender.Nodes.FormUrlEncoded;

namespace Bender
{
    public partial class Deserializer
    {
        // String

        public T DeserializeFormUrlEncoded<T>(string form)
        {
            return (T)DeserializeFormUrlEncoded(form, typeof(T));
        }

        public object DeserializeFormUrlEncoded(string form, Type type)
        {
            return Deserialize(new FormNode(form), type);
        }

        public FormNode DeserializeFormUrlEncoded(string form)
        {
            return new FormNode(form);
        }

        // Bytes

        public T DeserializeFormUrlEncoded<T>(byte[] bytes, Encoding encoding = null)
        {
            return (T)DeserializeFormUrlEncoded(bytes, typeof(T), encoding);
        }

        public object DeserializeFormUrlEncoded(byte[] bytes, Type type, Encoding encoding = null)
        {
            return Deserialize(new FormNode(bytes, encoding ?? Encoding.UTF8), type);
        }

        public FormNode DeserializeFormUrlEncoded(byte[] bytes, Encoding encoding = null)
        {
            return new FormNode(bytes, encoding ?? Encoding.UTF8);
        }

        // Stream

        public T DeserializeFormUrlEncoded<T>(Stream stream, Encoding encoding = null)
        {
            return (T)DeserializeFormUrlEncoded(stream, typeof(T), encoding);
        }

        public object DeserializeFormUrlEncoded(Stream stream, Type type, Encoding encoding = null)
        {
            return Deserialize(new FormNode(stream, encoding ?? Encoding.UTF8), type);
        }

        public FormNode DeserializeFormUrlEncoded(Stream stream, Encoding encoding = null)
        {
            return new FormNode(stream, encoding ?? Encoding.UTF8);
        }

        // File

        public T DeserializeFormUrlEncodedFile<T>(string path, Encoding encoding = null)
        {
            return (T)DeserializeFormUrlEncodedFile(path, typeof(T), encoding);
        }

        public object DeserializeFormUrlEncodedFile(string path, Type type, Encoding encoding = null)
        {
            return DeserializeFormUrlEncoded(File.ReadAllBytes(path), type, encoding);
        }

        public FormNode DeserializeFormUrlEncodedFile(string path, Encoding encoding = null)
        {
            return DeserializeFormUrlEncoded(File.ReadAllBytes(path), encoding);
        }
    }
}
