using System;
using System.IO;
using System.Text;
using Bender.Configuration;
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

        // String extensions

        public static T DeserializeFormUrlEncoded<T>(this string form, Options options)
        {
            return FormUrlEncoded<T>(form, options);
        }

        public static T DeserializeFormUrlEncoded<T>(this string form, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded<T>(form, configure);
        }

        public static object DeserializeFormUrlEncoded(this string form, Type type, Options options)
        {
            return FormUrlEncoded(form, type, options);
        }

        public static object DeserializeFormUrlEncoded(this string form, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(form, type, configure);
        }

        public static FormNode DeserializeFormUrlEncoded(this string form, Options options)
        {
            return FormUrlEncoded(form, options);
        }

        public static FormNode DeserializeFormUrlEncoded(this string form, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(form, configure);
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

        // Bytes extensions

        public static T DeserializeFormUrlEncoded<T>(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded<T>(bytes, configure);
        }

        public static T DeserializeFormUrlEncoded<T>(this byte[] bytes, Options options)
        {
            return FormUrlEncoded<T>(bytes, options);
        }

        public static T DeserializeFormUrlEncoded<T>(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded<T>(bytes, encoding, configure);
        }

        public static T DeserializeFormUrlEncoded<T>(this byte[] bytes, Encoding encoding, Options options)
        {
            return FormUrlEncoded<T>(bytes, encoding, options);
        }

        public static object DeserializeFormUrlEncoded(this byte[] bytes, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(bytes, type, configure);
        }

        public static object DeserializeFormUrlEncoded(this byte[] bytes, Type type, Options options)
        {
            return FormUrlEncoded(bytes, type, options);
        }

        public static FormNode DeserializeFormUrlEncoded(this byte[] bytes, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(bytes, configure);
        }

        public static FormNode DeserializeFormUrlEncoded(this byte[] bytes, Options options)
        {
            return FormUrlEncoded(bytes, options);
        }

        public static object DeserializeFormUrlEncoded(this byte[] bytes, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(bytes, type, encoding, configure);
        }

        public static object DeserializeFormUrlEncoded(this byte[] bytes, Type type, Encoding encoding, Options options)
        {
            return FormUrlEncoded(bytes, type, encoding, options);
        }

        public static FormNode DeserializeFormUrlEncoded(this byte[] bytes, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(bytes, encoding, configure);
        }

        public static FormNode DeserializeFormUrlEncoded(this byte[] bytes, Encoding encoding, Options options)
        {
            return FormUrlEncoded(bytes, encoding, options);
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

        // Stream extensions

        public static T DeserializeFormUrlEncoded<T>(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded<T>(stream, configure);
        }

        public static T DeserializeFormUrlEncoded<T>(this Stream stream, Options options)
        {
            return FormUrlEncoded<T>(stream, options);
        }

        public static T DeserializeFormUrlEncoded<T>(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded<T>(stream, encoding, configure);
        }

        public static T DeserializeFormUrlEncoded<T>(this Stream stream, Encoding encoding, Options options)
        {
            return FormUrlEncoded<T>(stream, encoding, options);
        }

        public static object DeserializeFormUrlEncoded(this Stream stream, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(stream, type, configure);
        }

        public static object DeserializeFormUrlEncoded(this Stream stream, Type type, Options options)
        {
            return FormUrlEncoded(stream, type, options);
        }

        public static FormNode DeserializeFormUrlEncoded(this Stream stream, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(stream, configure);
        }

        public static FormNode DeserializeFormUrlEncoded(this Stream stream, Options options)
        {
            return FormUrlEncoded(stream, options);
        }

        public static object DeserializeFormUrlEncoded(this Stream stream, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(stream, type, encoding, configure);
        }

        public static object DeserializeFormUrlEncoded(this Stream stream, Type type, Encoding encoding, Options options)
        {
            return FormUrlEncoded(stream, type, encoding, options);
        }

        public static FormNode DeserializeFormUrlEncoded(this Stream stream, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncoded(stream, encoding, configure);
        }

        public static FormNode DeserializeFormUrlEncoded(this Stream stream, Encoding encoding, Options options)
        {
            return FormUrlEncoded(stream, encoding, options);
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

        // File extensions

        public static T DeserializeFormUrlEncodedFile<T>(this string path, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile<T>(path, configure);
        }

        public static T DeserializeFormUrlEncodedFile<T>(this string path, Options options)
        {
            return FormUrlEncodedFile<T>(path, options);
        }

        public static T DeserializeFormUrlEncodedFile<T>(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile<T>(path, encoding, configure);
        }

        public static T DeserializeFormUrlEncodedFile<T>(this string path, Encoding encoding, Options options)
        {
            return FormUrlEncodedFile<T>(path, encoding, options);
        }

        public static object DeserializeFormUrlEncodedFile(this string path, Type type, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile(path, type, configure);
        }

        public static object DeserializeFormUrlEncodedFile(this string path, Type type, Options options)
        {
            return FormUrlEncodedFile(path, type, options);
        }

        public static FormNode DeserializeFormUrlEncodedFile(this string path, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile(path, configure);
        }

        public static FormNode DeserializeFormUrlEncodedFile(this string path, Options options)
        {
            return FormUrlEncodedFile(path, options);
        }

        public static object DeserializeFormUrlEncodedFile(this string path, Type type, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile(path, type, encoding, configure);
        }

        public static object DeserializeFormUrlEncodedFile(this string path, Type type, Encoding encoding, Options options)
        {
            return FormUrlEncodedFile(path, type, encoding, options);
        }

        public static FormNode DeserializeFormUrlEncodedFile(this string path, Encoding encoding, Action<OptionsDsl> configure = null)
        {
            return FormUrlEncodedFile(path, encoding, configure);
        }

        public static FormNode DeserializeFormUrlEncodedFile(this string path, Encoding encoding, Options options)
        {
            return FormUrlEncodedFile(path, encoding, options);
        }
    }
}
