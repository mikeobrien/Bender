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

        // String extensions

        public static string SerializeCsv(this object source, Options options)
        {
            return Csv(source, options);
        }

        public static string SerializeCsv(this object source, Action<OptionsDsl> configure = null)
        {
            return Csv(source, configure);
        }

        public static string SerializeCsv<T>(this T source, Options options)
        {
            return Csv(source, options);
        }

        public static string SerializeCsv<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return Csv(source, configure);
        }

        public static string SerializeCsv(this object source, Type type, Options options)
        {
            return Csv(source, type, options);
        }

        public static string SerializeCsv(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return Csv(source, type, configure);
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

        // Bytes extensions

        public static byte[] SerializeCsvBytes(this object source, Options options)
        {
            return CsvBytes(source, options);
        }

        public static byte[] SerializeCsvBytes(this object source, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, configure);
        }

        public static byte[] SerializeCsvBytes(this object source, Encoding encoding, Options options)
        {
            return CsvBytes(source, encoding, options);
        }

        public static byte[] SerializeCsvBytes(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, encoding, configure);
        }

        public static byte[] SerializeCsvBytes<T>(this T source, Options options)
        {
            return CsvBytes(source, options);
        }

        public static byte[] SerializeCsvBytes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, configure);
        }

        public static byte[] SerializeCsvBytes<T>(this T source, Encoding encoding, Options options)
        {
            return CsvBytes(source, encoding, options);
        }

        public static byte[] SerializeCsvBytes<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, encoding, configure);
        }

        public static byte[] SerializeCsvBytes(this object source, Type type, Options options)
        {
            return CsvBytes(source, type, options);
        }

        public static byte[] SerializeCsvBytes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, type, configure);
        }

        public static byte[] SerializeCsvBytes(this object source, Type type, Encoding encoding, Options options)
        {
            return CsvBytes(source, type, encoding, options);
        }

        public static byte[] SerializeCsvBytes(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvBytes(source, type, encoding, configure);
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

        // Return Stream extensions

        public static Stream SerializeCsvStream(this object source, Options options)
        {
            return CsvStream(source, options);
        }

        public static Stream SerializeCsvStream(this object source, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, configure);
        }

        public static Stream SerializeCsvStream(this object source, Encoding encoding, Options options)
        {
            return CsvStream(source, encoding, options);
        }

        public static Stream SerializeCsvStream(this object source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, encoding, configure);
        }

        public static Stream SerializeCsvStream<T>(this T source, Options options)
        {
            return CsvStream(source, options);
        }

        public static Stream SerializeCsvStream<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, configure);
        }

        public static Stream SerializeCsvStream<T>(this T source, Encoding encoding, Options options)
        {
            return CsvStream(source, encoding, options);
        }

        public static Stream SerializeCsvStream<T>(this T source, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, encoding, configure);
        }

        public static Stream SerializeCsvStream(this object source, Type type, Options options)
        {
            return CsvStream(source, type, options);
        }

        public static Stream SerializeCsvStream(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, type, configure);
        }

        public static Stream SerializeCsvStream(this object source, Type type, Encoding encoding, Options options)
        {
            return CsvStream(source, type, encoding, options);
        }

        public static Stream SerializeCsvStream(this object source, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvStream(source, type, encoding, configure);
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

        // To Stream extensions

        public static void SerializeCsvStream(this object source, Stream stream, Options options)
        {
            CsvStream(source, stream, options);
        }

        public static void SerializeCsvStream(this object source, Stream stream, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, stream, configure);
        }

        public static void SerializeCsvStream(this object source, Stream stream, Encoding encoding, Options options)
        {
            CsvStream(source, stream, encoding, options);
        }

        public static void SerializeCsvStream(this object source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, stream, encoding, configure);
        }

        public static void SerializeCsvStream<T>(this T source, Stream stream, Options options)
        {
            CsvStream(source, stream, options);
        }

        public static void SerializeCsvStream<T>(this T source, Stream stream, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, stream, configure);
        }

        public static void SerializeCsvStream<T>(this T source, Stream stream, Encoding encoding, Options options)
        {
            CsvStream(source, stream, encoding, options);
        }

        public static void SerializeCsvStream<T>(this T source, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, stream, encoding, configure);
        }

        public static void SerializeCsvStream(this object source, Type type, Stream stream, Options options)
        {
            CsvStream(source, type, stream, options);
        }

        public static void SerializeCsvStream(this object source, Type type, Stream stream, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, type, stream, configure);
        }

        public static void SerializeCsvStream(this object source, Type type, Stream stream, Encoding encoding, Options options)
        {
            CsvStream(source, type, stream, encoding, options);
        }

        public static void SerializeCsvStream(this object source, Type type, Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvStream(source, type, stream, encoding, configure);
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

        // File extensions

        public static void SerializeCsvFile(this object source, string path, Options options)
        {
            CsvFile(source, path, options);
        }

        public static void SerializeCsvFile(this object source, string path, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, path, configure);
        }

        public static void SerializeCsvFile(this object source, string path, Encoding encoding, Options options)
        {
            CsvFile(source, path, encoding, options);
        }

        public static void SerializeCsvFile(this object source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, path, encoding, configure);
        }

        public static void SerializeCsvFile<T>(this T source, string path, Options options)
        {
            CsvFile(source, path, options);
        }

        public static void SerializeCsvFile<T>(this T source, string path, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, path, configure);
        }

        public static void SerializeCsvFile<T>(this T source, string path, Encoding encoding, Options options)
        {
            CsvFile(source, path, encoding, options);
        }

        public static void SerializeCsvFile<T>(this T source, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, path, encoding, configure);
        }

        public static void SerializeCsvFile(this object source, Type type, string path, Options options)
        {
            CsvFile(source, type, path, options);
        }

        public static void SerializeCsvFile(this object source, Type type, string path, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, type, path, configure);
        }

        public static void SerializeCsvFile(this object source, Type type, string path, Encoding encoding, Options options)
        {
            CsvFile(source, type, path, encoding, options);
        }

        public static void SerializeCsvFile(this object source, Type type, string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            CsvFile(source, type, path, encoding, configure);
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

        // Nodes extensions

        public static FileNode SerializeCsvNodes(this object source, Options options)
        {
            return CsvNodes(source, options);
        }

        public static FileNode SerializeCsvNodes(this object source, Action<OptionsDsl> configure = null)
        {
            return CsvNodes(source, configure);
        }

        public static FileNode SerializeCsvNodes<T>(this T source, Options options)
        {
            return CsvNodes(source, options);
        }

        public static FileNode SerializeCsvNodes<T>(this T source, Action<OptionsDsl> configure = null)
        {
            return CsvNodes(source, configure);
        }

        public static FileNode SerializeCsvNodes(this object source, Type type, Options options)
        {
            return CsvNodes(source, type, options);
        }

        public static FileNode SerializeCsvNodes(this object source, Type type, Action<OptionsDsl> configure = null)
        {
            return CsvNodes(source, type, configure);
        }
    }
}
