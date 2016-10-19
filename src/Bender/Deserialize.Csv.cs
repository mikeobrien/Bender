using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes.CharacterSeparated;

namespace Bender
{
    public static partial class Deserialize
    {
        // String 

        public static T Csv<T>(string csv, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv<T>(csv);
        }

        public static T Csv<T>(string csv, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv<T>(csv);
        }

        public static object Csv(string csv, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(csv, type);
        }

        public static object Csv(string csv, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(csv, type);
        }

        public static FileNode Csv(string csv, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(csv);
        }

        public static FileNode Csv(string csv, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(csv);
        }

        // String Extensions

        public static T DeserializeCsv<T>(this string csv, Options options)
        {
            return Csv<T>(csv, options);
        }

        public static T DeserializeCsv<T>(this string csv, Action<OptionsDsl> configure = null)
        {
            return Csv<T>(csv, configure);
        }

        public static object DeserializeCsv(this string csv, Type type, Options options)
        {
            return Csv(csv, type, options);
        }

        public static object DeserializeCsv(this string csv, Type type, Action<OptionsDsl> configure = null)
        {
            return Csv(csv, type, configure);
        }

        public static FileNode DeserializeCsv(this string csv, Options options)
        {
            return Csv(csv, options);
        }

        public static FileNode DeserializeCsv(this string csv, Action<OptionsDsl> configure = null)
        {
            return Csv(csv, configure);
        }

        // Bytes

        public static T Csv<T>(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv<T>(bytes);
        }

        public static T Csv<T>(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv<T>(bytes);
        }

        public static T Csv<T>(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv<T>(bytes, encoding);
        }

        public static T Csv<T>(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv<T>(bytes, encoding);
        }

        public static object Csv(byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(bytes, type);
        }

        public static object Csv(byte[] bytes, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(bytes, type);
        }

        public static FileNode Csv(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(bytes);
        }

        public static FileNode Csv(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(bytes);
        }

        public static object Csv(byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(bytes, type, encoding);
        }

        public static object Csv(byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(bytes, type, encoding);
        }

        public static FileNode Csv(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(bytes, encoding);
        }

        public static FileNode Csv(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(bytes, encoding);
        }

        // Bytes extensions

        public static T DeserializeCsv<T>(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Csv<T>(bytes, configure);
        }

        public static T DeserializeCsv<T>(this byte[] bytes, Options options)
        {
            return Csv<T>(bytes, options);
        }

        public static T DeserializeCsv<T>(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv<T>(bytes, encoding, configure);
        }

        public static T DeserializeCsv<T>(this byte[] bytes, Encoding encoding, Options options)
        {
            return Csv<T>(bytes, encoding, options);
        }

        public static object DeserializeCsv(this byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Csv(bytes, type, configure);
        }

        public static object DeserializeCsv(this byte[] bytes, Type type, Options options)
        {
            return Csv(bytes, type, options);
        }

        public static FileNode DeserializeCsv(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Csv(bytes, configure);
        }

        public static FileNode DeserializeCsv(this byte[] bytes, Options options)
        {
            return Csv(bytes, options);
        }

        public static object DeserializeCsv(this byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv(bytes, type, encoding, configure);
        }

        public static object DeserializeCsv(this byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Csv(bytes, type, encoding, options);
        }

        public static FileNode DeserializeCsv(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv(bytes, encoding, configure);
        }

        public static FileNode DeserializeCsv(this byte[] bytes, Encoding encoding, Options options)
        {
            return Csv(bytes, encoding, options);
        }

        // Stream

        public static T Csv<T>(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv<T>(stream);
        }

        public static T Csv<T>(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv<T>(stream);
        }

        public static T Csv<T>(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv<T>(stream, encoding);
        }

        public static T Csv<T>(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv<T>(stream, encoding);
        }

        public static object Csv(Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(stream, type);
        }

        public static object Csv(Stream stream, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(stream, type);
        }

        public static FileNode Csv(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(stream);
        }

        public static FileNode Csv(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(stream);
        }

        public static object Csv(Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(stream, type, encoding);
        }

        public static object Csv(Stream stream, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(stream, type, encoding);
        }

        public static FileNode Csv(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsv(stream, encoding);
        }

        public static FileNode Csv(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsv(stream, encoding);
        }

        // Stream extensions

        public static T DeserializeCsv<T>(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Csv<T>(stream, configure);
        }

        public static T DeserializeCsv<T>(this Stream stream, Options options)
        {
            return Csv<T>(stream, options);
        }

        public static T DeserializeCsv<T>(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv<T>(stream, encoding, configure);
        }

        public static T DeserializeCsv<T>(this Stream stream, Encoding encoding, Options options)
        {
            return Csv<T>(stream, encoding, options);
        }

        public static object DeserializeCsv(this Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Csv(stream, type, configure);
        }

        public static object DeserializeCsv(this Stream stream, Type type, Options options)
        {
            return Csv(stream, type, options);
        }

        public static FileNode DeserializeCsv(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return Csv(stream, configure);
        }

        public static FileNode DeserializeCsv(this Stream stream, Options options)
        {
            return Csv(stream, options);
        }

        public static object DeserializeCsv(this Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv(stream, type, encoding, configure);
        }

        public static object DeserializeCsv(this Stream stream, Type type, Encoding encoding, Options options)
        {
            return Csv(stream, type, encoding, options);
        }

        public static FileNode DeserializeCsv(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Csv(stream, encoding, configure);
        }

        public static FileNode DeserializeCsv(this Stream stream, Encoding encoding, Options options)
        {
            return Csv(stream, encoding, options);
        }

        // File

        public static T CsvFile<T>(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile<T>(path);
        }

        public static T CsvFile<T>(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile<T>(path);
        }

        public static T CsvFile<T>(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile<T>(path, encoding);
        }

        public static T CsvFile<T>(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile<T>(path, encoding);
        }

        public static object CsvFile(string path, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile(path, type);
        }

        public static object CsvFile(string path, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile(path, type);
        }

        public static FileNode CsvFile(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile(path);
        }

        public static FileNode CsvFile(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile(path);
        }

        public static object CsvFile(string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile(path, type, encoding);
        }

        public static object CsvFile(string path, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile(path, type, encoding);
        }

        public static FileNode CsvFile(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeCsvFile(path, encoding);
        }

        public static FileNode CsvFile(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeCsvFile(path, encoding);
        }

        // File extensions

        public static T DeserializeCsvFile<T>(this string path, Action<OptionsDsl> configure = null)
        {
            return CsvFile<T>(path, configure);
        }

        public static T DeserializeCsvFile<T>(this string path, Options options)
        {
            return CsvFile<T>(path, options);
        }

        public static T DeserializeCsvFile<T>(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvFile<T>(path, encoding, configure);
        }

        public static T DeserializeCsvFile<T>(this string path, Encoding encoding, Options options)
        {
            return CsvFile<T>(path, encoding, options);
        }

        public static object DeserializeCsvFile(this string path, Type type, Action<OptionsDsl> configure = null)
        {
            return CsvFile(path, type, configure);
        }

        public static object DeserializeCsvFile(this string path, Type type, Options options)
        {
            return CsvFile(path, type, options);
        }

        public static FileNode DeserializeCsvFile(this string path, Action<OptionsDsl> configure = null)
        {
            return CsvFile(path, configure);
        }

        public static FileNode DeserializeCsvFile(this string path, Options options)
        {
            return CsvFile(path, options);
        }

        public static object DeserializeCsvFile(this string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvFile(path, type, encoding, configure);
        }

        public static object DeserializeCsvFile(this string path, Type type, Encoding encoding, Options options)
        {
            return CsvFile(path, type, encoding, options);
        }

        public static FileNode DeserializeCsvFile(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return CsvFile(path, encoding, configure);
        }

        public static FileNode DeserializeCsvFile(this string path, Encoding encoding, Options options)
        {
            return CsvFile(path, encoding, options);
        }
    }
}
