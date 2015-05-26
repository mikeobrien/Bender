using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.FormUrlEncoded;

namespace Bender
{
    public static partial class Serialize
    {
        // String

        public static string FormUrlEncoded(object source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncoded(source);
        }

        public static string FormUrlEncoded(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncoded(source);
        }

        public static string FormUrlEncoded<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncoded(source);
        }

        public static string FormUrlEncoded<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncoded(source);
        }

        public static string FormUrlEncoded(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncoded(source, type);
        }

        public static string FormUrlEncoded(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncoded(source, type);
        }

        // Bytes

        public static byte[] FormUrlEncodedBytes(object source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source);
        }

        public static byte[] FormUrlEncodedBytes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source);
        }

        public static byte[] FormUrlEncodedBytes(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source, encoding);
        }

        public static byte[] FormUrlEncodedBytes(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source, encoding);
        }

        public static byte[] FormUrlEncodedBytes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source);
        }

        public static byte[] FormUrlEncodedBytes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source);
        }

        public static byte[] FormUrlEncodedBytes<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source, encoding);
        }

        public static byte[] FormUrlEncodedBytes<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source, encoding);
        }

        public static byte[] FormUrlEncodedBytes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source, type);
        }

        public static byte[] FormUrlEncodedBytes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source, type);
        }

        public static byte[] FormUrlEncodedBytes(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedBytes(source, type, encoding);
        }

        public static byte[] FormUrlEncodedBytes(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedBytes(source, type, encoding);
        }

        // Return Stream

        public static Stream FormUrlEncodedStream(object source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source);
        }

        public static Stream FormUrlEncodedStream(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source);
        }

        public static Stream FormUrlEncodedStream(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source, encoding);
        }

        public static Stream FormUrlEncodedStream(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source, encoding);
        }

        public static Stream FormUrlEncodedStream<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source);
        }

        public static Stream FormUrlEncodedStream<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source);
        }

        public static Stream FormUrlEncodedStream<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source, encoding);
        }

        public static Stream FormUrlEncodedStream<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source, encoding);
        }

        public static Stream FormUrlEncodedStream(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source, type);
        }

        public static Stream FormUrlEncodedStream(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source, type);
        }

        public static Stream FormUrlEncodedStream(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedStream(source, type, encoding);
        }

        public static Stream FormUrlEncodedStream(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedStream(source, type, encoding);
        }

        // To Stream

        public static void FormUrlEncodedStream(object source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, stream);
        }

        public static void FormUrlEncodedStream(object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, stream);
        }

        public static void FormUrlEncodedStream(object source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, stream, encoding);
        }

        public static void FormUrlEncodedStream(object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, stream, encoding);
        }

        public static void FormUrlEncodedStream<T>(T source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, stream);
        }

        public static void FormUrlEncodedStream<T>(T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, stream);
        }

        public static void FormUrlEncodedStream<T>(T source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, stream, encoding);
        }

        public static void FormUrlEncodedStream<T>(T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, stream, encoding);
        }

        public static void FormUrlEncodedStream(object source, Type type, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, type, stream);
        }

        public static void FormUrlEncodedStream(object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, type, stream);
        }

        public static void FormUrlEncodedStream(object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedStream(source, type, stream, encoding);
        }

        public static void FormUrlEncodedStream(object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedStream(source, type, stream, encoding);
        }

        // File

        public static void FormUrlEncodedFile(object source, string path, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, path);
        }

        public static void FormUrlEncodedFile(object source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, path);
        }

        public static void FormUrlEncodedFile(object source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, path, encoding);
        }

        public static void FormUrlEncodedFile(object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, path, encoding);
        }

        public static void FormUrlEncodedFile<T>(T source, string path, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, path);
        }

        public static void FormUrlEncodedFile<T>(T source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, path);
        }

        public static void FormUrlEncodedFile<T>(T source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, path, encoding);
        }

        public static void FormUrlEncodedFile<T>(T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, path, encoding);
        }

        public static void FormUrlEncodedFile(object source, Type type, string path, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, type, path);
        }

        public static void FormUrlEncodedFile(object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, type, path);
        }

        public static void FormUrlEncodedFile(object source, Type type, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeFormUrlEncodedFile(source, type, path, encoding);
        }

        public static void FormUrlEncodedFile(object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeFormUrlEncodedFile(source, type, path, encoding);
        }

        // Nodes

        public static FormNode FormUrlEncodedNodes(object source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedNodes(source);
        }

        public static FormNode FormUrlEncodedNodes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedNodes(source);
        }

        public static FormNode FormUrlEncodedNodes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedNodes(source);
        }

        public static FormNode FormUrlEncodedNodes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedNodes(source);
        }

        public static FormNode FormUrlEncodedNodes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeFormUrlEncodedNodes(source, type);
        }

        public static FormNode FormUrlEncodedNodes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeFormUrlEncodedNodes(source, type);
        }
    }
}
