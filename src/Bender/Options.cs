using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bender
{
    public class Options
    {
        public Options()
        {
            ExcludedTypes = new List<Func<Type, bool>>();
            DefaultGenericTypeNameFormat = "{0}Of{1}";
            DefaultGenericListNameFormat = "ArrayOf{0}";

            Readers = new Dictionary<Type, Func<Options, PropertyInfo, string, object>>();
            AddReader((o, p, v) => Convert.FromBase64String(v));
            AddReader((o, p, v) => new Uri(o.DefaultNonNullableTypesWhenEmpty && v.IsNullOrEmpty() ? "http://tempuri.org/" : v));

            Writers = new Dictionary<Type, Func<Options, PropertyInfo, object, string>>();
            AddWriter<byte[]>((o, p, v) => Convert.ToBase64String(v));
            AddWriter<Uri>((o, p, v) => v != null ? v.ToString() : "");
            AddWriter<bool>((o, p, v) => v.ToString().ToLower());
            AddWriter<bool?>((o, p, v) => v.HasValue ? v.ToString().ToLower() : "");
        }
        
        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string DefaultGenericTypeNameFormat { get; set; }
        public string DefaultGenericListNameFormat { get; set; }

        // Deserialization specific
        public bool DefaultNonNullableTypesWhenEmpty { get; set; }
        public Dictionary<Type, Func<Options, PropertyInfo, string, object>> Readers { get; private set; }

        public void AddReader<T>(Func<Options, PropertyInfo, string, T> reader)
        {
            Readers.Add(typeof(T), (o, p, v) => reader(o, p, v));
        } 

        // Serialization specific
        public bool PrettyPrint { get; set; }
        public bool ExcludeNullValues { get; set; }
        public Dictionary<Type, Func<Options, PropertyInfo, object, string>> Writers { get; private set; }

        public void AddWriter<T>(Func<Options, PropertyInfo, T, string> writer)
        {
            Writers.Add(typeof(T), (o, p, v) => writer(o, p, (T)v));
        } 
    }
}