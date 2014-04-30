using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;

namespace Bender
{
    public static partial class Serialize
    {
        // String

        public static string String<T>(T source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeString(source, target);
        }

        public static string String<T>(T source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeString(source, target);
        }

        public static string String(object source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeString(source, target);
        }

        public static string String(object source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeString(source, target);
        }

        public static string String(object @object, Type type, INode target, Options options)
        {
            return Serializer.Create(options).SerializeString(@object, type, target);
        }

        public static string String(object @object, Type type, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeString(@object, type, target);
        }

        // Bytes

        public static byte[] Bytes<T>(T source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeBytes(source, target);
        }

        public static byte[] Bytes<T>(T source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(source, target);
        }

        public static byte[] Bytes<T>(T source, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeBytes(source, target, encoding);
        }

        public static byte[] Bytes<T>(T source, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(source, target, encoding);
        }

        public static byte[] Bytes(object source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeBytes(source, target);
        }

        public static byte[] Bytes(object source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(source, target);
        }

        public static byte[] Bytes(object source, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeBytes(source, target, encoding);
        }

        public static byte[] Bytes(object source, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(source, target, encoding);
        }

        public static byte[] Bytes(object @object, Type type, INode target, Options options)
        {
            return Serializer.Create(options).SerializeBytes(@object, type, target);
        }

        public static byte[] Bytes(object @object, Type type, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(@object, type, target);
        }

        public static byte[] Bytes(object @object, Type type, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeBytes(@object, type, target, encoding);
        }

        public static byte[] Bytes(object @object, Type type, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeBytes(@object, type, target, encoding);
        }

        // Return Stream

        public static Stream Stream<T>(T source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeStream(source, target);
        }

        public static Stream Stream<T>(T source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(source, target);
        }

        public static Stream Stream<T>(T source, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeStream(source, target, encoding);
        }

        public static Stream Stream<T>(T source, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(source, target, encoding);
        }

        public static Stream Stream(object source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeStream(source, target);
        }

        public static Stream Stream(object source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(source, target);
        }

        public static Stream Stream(object source, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeStream(source, target, encoding);
        }

        public static Stream Stream(object source, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(source, target, encoding);
        }

        public static Stream Stream(object @object, Type type, INode target, Options options)
        {
            return Serializer.Create(options).SerializeStream(@object, type, target);
        }

        public static Stream Stream(object @object, Type type, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(@object, type, target);
        }

        public static Stream Stream(object @object, Type type, INode target, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeStream(@object, type, target, encoding);
        }

        public static Stream Stream(object @object, Type type, INode target, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeStream(@object, type, target, encoding);
        }

        // To Stream

        public static void Stream<T>(T source, INode target, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeStream(source, target, stream);
        }

        public static void Stream<T>(T source, INode target, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(source, target, stream);
        }

        public static void Stream<T>(T source, INode target, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeStream(source, target, stream, encoding);
        }

        public static void Stream<T>(T source, INode target, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(source, target, stream, encoding);
        }

        public static void Stream(object source, INode target, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeStream(source, target, stream);
        }

        public static void Stream(object source, INode target, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(source, target, stream);
        }

        public static void Stream(object source, INode target, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeStream(source, target, stream, encoding);
        }

        public static void Stream(object source, INode target, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(source, target, stream, encoding);
        }

        public static void Stream(object @object, Type type, INode target, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeStream(@object, type, target, stream);
        }

        public static void Stream(object @object, Type type, INode target, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(@object, type, target, stream);
        }

        public static void Stream(object @object, Type type, INode target, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeStream(@object, type, target, stream, encoding);
        }

        public static void Stream(object @object, Type type, INode target, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeStream(@object, type, target, stream, encoding);
        }

        // Nodes

        public static INode Nodes<T>(T source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeNodes(source, target);
        }

        public static INode Nodes<T>(T source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeNodes(source, target);
        }

        public static INode Nodes(object source, INode target, Options options)
        {
            return Serializer.Create(options).SerializeNodes(source, target);
        }

        public static INode Nodes(object source, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeNodes(source, target);
        }

        public static INode Nodes(object @object, Type type, INode target, Options options)
        {
            return Serializer.Create(options).SerializeNodes(@object, type, target);
        }

        public static INode Nodes(object @object, Type type, INode target, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeNodes(@object, type, target);
        }
    }
}
