using System;
using System.IO;
using System.Text;
using Bender.Configuration;
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

        // String extensions

        public static string SerializeFormUrlEncoded(this object source, Options options)
        {
            return FormUrlEncoded(source, options);
        }

        public static string SerializeFormUrlEncoded(this object source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(source, configure);
        }

        public static string SerializeFormUrlEncoded<T>(this T source, Options options)
        {
            return FormUrlEncoded(source, options);
        }

        public static string SerializeFormUrlEncoded<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(source, configure);
        }

        public static string SerializeFormUrlEncoded(this object source, Type type, Options options)
        {
            return FormUrlEncoded(source, type, options);
        }

        public static string SerializeFormUrlEncoded(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(source, type, configure);
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

        // Bytes extensions

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Options options)
        {
            return FormUrlEncodedBytes(source, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, configure);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Encoding encoding, Options options)
        {
            return FormUrlEncodedBytes(source, encoding, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, encoding, configure);
        }

        public static byte[] SerializeFormUrlEncodedBytes<T>(this T source, Options options)
        {
            return FormUrlEncodedBytes(source, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, configure);
        }

        public static byte[] SerializeFormUrlEncodedBytes<T>(this T source, Encoding encoding, Options options)
        {
            return FormUrlEncodedBytes(source, encoding, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, encoding, configure);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Type type, Options options)
        {
            return FormUrlEncodedBytes(source, type, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, type, configure);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Type type, Encoding encoding, Options options)
        {
            return FormUrlEncodedBytes(source, type, encoding, options);
        }

        public static byte[] SerializeFormUrlEncodedBytes(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedBytes(source, type, encoding, configure);
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

        // Return Stream extensions

        public static Stream SerializeFormUrlEncodedStream(this object source, Options options)
        {
            return FormUrlEncodedStream(source, options);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, configure);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Encoding encoding, Options options)
        {
            return FormUrlEncodedStream(source, encoding, options);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, encoding, configure);
        }

        public static Stream SerializeFormUrlEncodedStream<T>(this T source, Options options)
        {
            return FormUrlEncodedStream(source, options);
        }

        public static Stream SerializeFormUrlEncodedStream<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, configure);
        }

        public static Stream SerializeFormUrlEncodedStream<T>(this T source, Encoding encoding, Options options)
        {
            return FormUrlEncodedStream(source, encoding, options);
        }

        public static Stream SerializeFormUrlEncodedStream<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, encoding, configure);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Type type, Options options)
        {
            return FormUrlEncodedStream(source, type, options);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, type, configure);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Type type, Encoding encoding, Options options)
        {
            return FormUrlEncodedStream(source, type, encoding, options);
        }

        public static Stream SerializeFormUrlEncodedStream(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedStream(source, type, encoding, configure);
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

        // To Stream extensions

        public static void SerializeFormUrlEncodedStream(this object source, Stream stream, Options options)
        {
            FormUrlEncodedStream(source, stream, options);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, stream, configure);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Stream stream, Encoding encoding, Options options)
        {
            FormUrlEncodedStream(source, stream, encoding, options);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, stream, encoding, configure);
        }

        public static void SerializeFormUrlEncodedStream<T>(this T source, Stream stream, Options options)
        {
            FormUrlEncodedStream(source, stream, options);
        }

        public static void SerializeFormUrlEncodedStream<T>(this T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, stream, configure);
        }

        public static void SerializeFormUrlEncodedStream<T>(this T source, Stream stream, Encoding encoding, Options options)
        {
            FormUrlEncodedStream(source, stream, encoding, options);
        }

        public static void SerializeFormUrlEncodedStream<T>(this T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, stream, encoding, configure);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Type type, Stream stream, Options options)
        {
            FormUrlEncodedStream(source, type, stream, options);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, type, stream, configure);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            FormUrlEncodedStream(source, type, stream, encoding, options);
        }

        public static void SerializeFormUrlEncodedStream(this object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedStream(source, type, stream, encoding, configure);
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

        // File extensions

        public static void SerializeFormUrlEncodedFile(this object source, string path, Options options)
        {
            FormUrlEncodedFile(source, path, options);
        }

        public static void SerializeFormUrlEncodedFile(this object source, string path, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, path, configure);
        }

        public static void SerializeFormUrlEncodedFile(this object source, string path, Encoding encoding, Options options)
        {
            FormUrlEncodedFile(source, path, encoding, options);
        }

        public static void SerializeFormUrlEncodedFile(this object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, path, encoding, configure);
        }

        public static void SerializeFormUrlEncodedFile<T>(this T source, string path, Options options)
        {
            FormUrlEncodedFile(source, path, options);
        }

        public static void SerializeFormUrlEncodedFile<T>(this T source, string path, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, path, configure);
        }

        public static void SerializeFormUrlEncodedFile<T>(this T source, string path, Encoding encoding, Options options)
        {
            FormUrlEncodedFile(source, path, encoding, options);
        }

        public static void SerializeFormUrlEncodedFile<T>(this T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, path, encoding, configure);
        }

        public static void SerializeFormUrlEncodedFile(this object source, Type type, string path, Options options)
        {
            FormUrlEncodedFile(source, type, path, options);
        }

        public static void SerializeFormUrlEncodedFile(this object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, type, path, configure);
        }

        public static void SerializeFormUrlEncodedFile(this object source, Type type, string path, Encoding encoding, Options options)
        {
            FormUrlEncodedFile(source, type, path, encoding, options);
        }

        public static void SerializeFormUrlEncodedFile(this object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            FormUrlEncodedFile(source, type, path, encoding, configure);
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

        // Nodes extensions

        public static FormNode SerializeFormUrlEncodedNodes(this object source, Options options)
        {
            return FormUrlEncodedNodes(source, options);
        }

        public static FormNode SerializeFormUrlEncodedNodes(this object source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedNodes(source, configure);
        }

        public static FormNode SerializeFormUrlEncodedNodes<T>(this T source, Options options)
        {
            return FormUrlEncodedNodes(source, options);
        }

        public static FormNode SerializeFormUrlEncodedNodes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedNodes(source, configure);
        }

        public static FormNode SerializeFormUrlEncodedNodes(this object source, Type type, Options options)
        {
            return FormUrlEncodedNodes(source, type, options);
        }

        public static FormNode SerializeFormUrlEncodedNodes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedNodes(source, type, configure);
        }
    }
}
