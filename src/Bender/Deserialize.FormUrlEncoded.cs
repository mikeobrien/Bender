using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.FormUrlEncoded;

namespace Bender
{
    public static partial class Deserialize
    {
        // String 

        public static T FormUrlEncoded<T>(string form, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded<T>(form);
        }

        public static T FormUrlEncoded<T>(string form, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded<T>(form);
        }

        public static object FormUrlEncoded(string form, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(form, type);
        }

        public static object FormUrlEncoded(string form, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(form, type);
        }

        public static FormNode FormUrlEncoded(string form, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(form);
        }

        public static FormNode FormUrlEncoded(string form, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(form);
        }

        // Bytes

        public static T FormUrlEncoded<T>(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded<T>(bytes);
        }

        public static T FormUrlEncoded<T>(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded<T>(bytes);
        }

        public static T FormUrlEncoded<T>(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded<T>(bytes, encoding);
        }

        public static T FormUrlEncoded<T>(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded<T>(bytes, encoding);
        }

        public static object FormUrlEncoded(byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(bytes, type);
        }

        public static object FormUrlEncoded(byte[] bytes, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(bytes, type);
        }

        public static FormNode FormUrlEncoded(byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(bytes);
        }

        public static FormNode FormUrlEncoded(byte[] bytes, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(bytes);
        }

        public static object FormUrlEncoded(byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(bytes, type, encoding);
        }

        public static object FormUrlEncoded(byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(bytes, type, encoding);
        }

        public static FormNode FormUrlEncoded(byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(bytes, encoding);
        }

        public static FormNode FormUrlEncoded(byte[] bytes, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(bytes, encoding);
        }

        // Stream

        public static T FormUrlEncoded<T>(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded<T>(stream);
        }

        public static T FormUrlEncoded<T>(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded<T>(stream);
        }

        public static T FormUrlEncoded<T>(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded<T>(stream, encoding);
        }

        public static T FormUrlEncoded<T>(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded<T>(stream, encoding);
        }

        public static object FormUrlEncoded(Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(stream, type);
        }

        public static object FormUrlEncoded(Stream stream, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(stream, type);
        }

        public static FormNode FormUrlEncoded(Stream stream, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(stream);
        }

        public static FormNode FormUrlEncoded(Stream stream, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(stream);
        }

        public static object FormUrlEncoded(Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(stream, type, encoding);
        }

        public static object FormUrlEncoded(Stream stream, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(stream, type, encoding);
        }

        public static FormNode FormUrlEncoded(Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncoded(stream, encoding);
        }

        public static FormNode FormUrlEncoded(Stream stream, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncoded(stream, encoding);
        }

        // File

        public static T FormUrlEncodedFile<T>(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile<T>(path);
        }

        public static T FormUrlEncodedFile<T>(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile<T>(path);
        }

        public static T FormUrlEncodedFile<T>(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile<T>(path, encoding);
        }

        public static T FormUrlEncodedFile<T>(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile<T>(path, encoding);
        }

        public static object FormUrlEncodedFile(string path, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile(path, type);
        }

        public static object FormUrlEncodedFile(string path, Type type, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile(path, type);
        }

        public static FormNode FormUrlEncodedFile(string path, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile(path);
        }

        public static FormNode FormUrlEncodedFile(string path, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile(path);
        }

        public static object FormUrlEncodedFile(string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile(path, type, encoding);
        }

        public static object FormUrlEncodedFile(string path, Type type, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile(path, type, encoding);
        }

        public static FormNode FormUrlEncodedFile(string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).DeserializeFormUrlEncodedFile(path, encoding);
        }

        public static FormNode FormUrlEncodedFile(string path, Encoding encoding, Options options)
        {
            return Deserializer.Create(options).DeserializeFormUrlEncodedFile(path, encoding);
        }
    }
}
