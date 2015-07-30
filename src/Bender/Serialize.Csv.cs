using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes.CharacterSeparated;

namespace Bender
{
    public static partial class Serialize
    {
        // String

        public static string Csv(object source, Options options)
        {
            return Serializer.Create(options).SerializeCsv(source);
        }

        public static string Csv(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsv(source);
        }

        public static string Csv<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeCsv(source);
        }

        public static string Csv<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsv(source);
        }

        public static string Csv(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeCsv(source, type);
        }

        public static string Csv(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsv(source, type);
        }

        // Bytes

        public static byte[] CsvBytes(object source, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source);
        }

        public static byte[] CsvBytes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source);
        }

        public static byte[] CsvBytes(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source, encoding);
        }

        public static byte[] CsvBytes(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source, encoding);
        }

        public static byte[] CsvBytes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source);
        }

        public static byte[] CsvBytes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source);
        }

        public static byte[] CsvBytes<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source, encoding);
        }

        public static byte[] CsvBytes<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source, encoding);
        }

        public static byte[] CsvBytes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source, type);
        }

        public static byte[] CsvBytes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source, type);
        }

        public static byte[] CsvBytes(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvBytes(source, type, encoding);
        }

        public static byte[] CsvBytes(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvBytes(source, type, encoding);
        }

        // Return Stream

        public static Stream CsvStream(object source, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source);
        }

        public static Stream CsvStream(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source);
        }

        public static Stream CsvStream(object source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source, encoding);
        }

        public static Stream CsvStream(object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source, encoding);
        }

        public static Stream CsvStream<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source);
        }

        public static Stream CsvStream<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source);
        }

        public static Stream CsvStream<T>(T source, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source, encoding);
        }

        public static Stream CsvStream<T>(T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source, encoding);
        }

        public static Stream CsvStream(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source, type);
        }

        public static Stream CsvStream(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source, type);
        }

        public static Stream CsvStream(object source, Type type, Encoding encoding, Options options)
        {
            return Serializer.Create(options).SerializeCsvStream(source, type, encoding);
        }

        public static Stream CsvStream(object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvStream(source, type, encoding);
        }

        // To Stream

        public static void CsvStream(object source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, stream);
        }

        public static void CsvStream(object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, stream);
        }

        public static void CsvStream(object source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, stream, encoding);
        }

        public static void CsvStream(object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, stream, encoding);
        }

        public static void CsvStream<T>(T source, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, stream);
        }

        public static void CsvStream<T>(T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, stream);
        }

        public static void CsvStream<T>(T source, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, stream, encoding);
        }

        public static void CsvStream<T>(T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, stream, encoding);
        }

        public static void CsvStream(object source, Type type, Stream stream, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, type, stream);
        }

        public static void CsvStream(object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, type, stream);
        }

        public static void CsvStream(object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvStream(source, type, stream, encoding);
        }

        public static void CsvStream(object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvStream(source, type, stream, encoding);
        }

        // File

        public static void CsvFile(object source, string path, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, path);
        }

        public static void CsvFile(object source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, path);
        }

        public static void CsvFile(object source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, path, encoding);
        }

        public static void CsvFile(object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, path, encoding);
        }

        public static void CsvFile<T>(T source, string path, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, path);
        }

        public static void CsvFile<T>(T source, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, path);
        }

        public static void CsvFile<T>(T source, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, path, encoding);
        }

        public static void CsvFile<T>(T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, path, encoding);
        }

        public static void CsvFile(object source, Type type, string path, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, type, path);
        }

        public static void CsvFile(object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, type, path);
        }

        public static void CsvFile(object source, Type type, string path, Encoding encoding, Options options)
        {
            Serializer.Create(options).SerializeCsvFile(source, type, path, encoding);
        }

        public static void CsvFile(object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            Serializer.Create(configure).SerializeCsvFile(source, type, path, encoding);
        }

        // Nodes

        public static FileNode CsvNodes(object source, Options options)
        {
            return Serializer.Create(options).SerializeCsvNodes(source);
        }

        public static FileNode CsvNodes(object source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvNodes(source);
        }

        public static FileNode CsvNodes<T>(T source, Options options)
        {
            return Serializer.Create(options).SerializeCsvNodes(source);
        }

        public static FileNode CsvNodes<T>(T source, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvNodes(source);
        }

        public static FileNode CsvNodes(object source, Type type, Options options)
        {
            return Serializer.Create(options).SerializeCsvNodes(source, type);
        }

        public static FileNode CsvNodes(object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Serializer.Create(configure).SerializeCsvNodes(source, type);
        }
    }
}
