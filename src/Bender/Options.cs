using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Bender
{
    public class Options
    {
        public Options()
        {
            ExcludedTypes = new List<Func<Type, bool>>();
            DefaultGenericTypeNameFormat = "{0}Of{1}";
            DefaultGenericListNameFormat = "ArrayOf{0}";

            Readers = new Dictionary<Type, Func<Options, PropertyInfo, XElement, object>>();
            AddReader((o, p, v) => Convert.FromBase64String(v.Value));
            AddReader((o, p, v) => new Uri(o.DefaultNonNullableTypesWhenEmpty && v.Value.IsNullOrEmpty() ? "http://tempuri.org/" : v.Value));

            Writers = new Dictionary<Type, Func<Options, PropertyInfo, object, string>>();
            AddWriter<byte[]>((o, p, v) => Convert.ToBase64String(v));
            AddWriter<Uri>((o, p, v) => v != null ? v.ToString() : "");
            AddWriter<bool>((o, p, v) => v.ToString().ToLower(), true);
        }
        
        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string DefaultGenericTypeNameFormat { get; set; }
        public string DefaultGenericListNameFormat { get; set; }

        // Deserialization specific
        public bool DefaultNonNullableTypesWhenEmpty { get; set; }
        public bool IgnoreUnmatchedElements { get; set; }
        public bool IgnoreTypeElementNames { get; set; }
        public Dictionary<Type, Func<Options, PropertyInfo, XElement, object>> Readers { get; private set; }

        public void AddReader<T>(Func<Options, PropertyInfo, XElement, T> reader)
        {
            Readers.Add(typeof(T), (o, p, v) => reader(o, p, v));
        }

        public void AddReader<T>(Func<Options, PropertyInfo, XElement, T> reader, bool handleNullable) where T : struct
        {
            Readers.Add(typeof(T), (o, p, v) => reader(o, p, v));
            if (handleNullable) Readers.Add(typeof(T?), (o, p, v) => !string.IsNullOrEmpty(v.Value) ? reader(o, p, v) : (T?)null);
        } 

        // Serialization specific
        public bool PrettyPrint { get; set; }
        public bool ExcludeNullValues { get; set; }
        public Dictionary<Type, Func<Options, PropertyInfo, object, string>> Writers { get; private set; }

        public void AddWriter<T>(Func<Options, PropertyInfo, T, string> writer)
        {
            Writers.Add(typeof(T), (o, p, v) => writer(o, p, (T)v));
        }

        public void AddWriter<T>(Func<Options, PropertyInfo, T, string> writer, bool handleNullable) where T : struct
        {
            Writers.Add(typeof(T), (o, p, v) => writer(o, p, (T)v));
            if (handleNullable) Writers.Add(typeof(T?), (o, p, v) => ((T?)v).HasValue ? writer(o, p, ((T?)v).Value) : "");
        } 
    }
}