using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Extensions;

namespace Tests.Performance
{
    public static class Extensions
    {
        public static string Serialize(this DataContractJsonSerializer serializer, object source)
        {
            return ReadStream(x => serializer.WriteObject(x, source));
        }

        public static object Deserialize(this DataContractJsonSerializer serializer, string source, Type type)
        {
            return serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }

        public static string Serialize(this DataContractSerializer serializer, object source)
        {
            return ReadStream(x => serializer.WriteObject(x, source));
        }

        public static object Deserialize(this DataContractSerializer serializer, string source, Type type)
        {
            return serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }

        public static string Serialize(this XmlSerializer serializer, object source)
        {
            return ReadStream(x => serializer.Serialize(x, source));
        }

        private static string ReadStream(Action<Stream> write)
        {
            using (var stream = new MemoryStream())
            {
                write(stream);
                return stream.ReadToEnd();
            }
        }

        public static object Deserialize(this XmlSerializer serializer, string source, Type type)
        {
            return serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }

        public static string NormalizeFormatting(this string source)
        {
            return source
                .Replace(" encoding=\"utf-8\"", "")
                .Replace("<?xml version=\"1.0\"?>", "")
                .Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                .Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "")
                .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace(": ", ":")
                .Replace("  ", "");
        }

        public static Type[] GenerateTypes(this AppDomain domain, int count)
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("Assembly"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");
            var types = new List<Type>();

            Enumerable.Range(1, count).ForEach(x => types.Add(moduleBuilder
                .DefineType("Tests.Type" + x, TypeAttributes.Public).CreateType()));

            return types.ToArray();
        }
    }
}