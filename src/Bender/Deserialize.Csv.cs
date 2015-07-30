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
    }
}
