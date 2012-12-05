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

            Readers = new Dictionary<Type, Func<PropertyInfo, string, object>>();
            AddReader((p, v) => Convert.FromBase64String(v));

            Writers = new Dictionary<Type, Func<PropertyInfo, object, string>>();
            AddWriter<byte[]>((p, v) => Convert.ToBase64String(v));
            AddWriter<bool>((p, v) => v.ToString().ToLower());
            AddWriter<bool?>((p, v) => v.HasValue ? v.ToString().ToLower() : "");
        }

        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string DefaultGenericTypeNameFormat { get; set; }
        public string DefaultGenericListNameFormat { get; set; }

        // Deserialization specific
        public bool DefaultNonNullableTypesWhenEmpty { get; set; }
        public Dictionary<Type, Func<PropertyInfo, string, object>> Readers { get; private set; }

        public void AddReader<T>(Func<PropertyInfo, string, T> reader)
        {
            Readers.Add(typeof(T), (p, v) => reader(p, v));
        } 

        // Serialization specific
        public bool PrettyPrint { get; set; }
        public bool ExcludeNullValues { get; set; }
        public Dictionary<Type, Func<PropertyInfo, object, string>> Writers { get; private set; }

        public void AddWriter<T>(Func<PropertyInfo, T, string> writer)
        {
            Writers.Add(typeof(T), (p, v) => writer(p, (T)v));
        } 
    }
}