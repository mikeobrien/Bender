using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bender
{
    public class SerializerOptions
    {
        private readonly Dictionary<Type, Func<PropertyInfo, object, string>> _formatters = 
            new Dictionary<Type, Func<PropertyInfo, object, string>>();

        public SerializerOptions()
        {
            ExcludedTypes = new List<Func<Type, bool>>();
            AddFormatter<byte[]>((p, v) => Convert.ToBase64String(v));
            AddFormatter<bool>((p, v) => v.ToString().ToLower());
            AddFormatter<bool?>((p, v) => v.HasValue ? v.ToString().ToLower() : "");
            DefaultGenericTypeNameFormat = "{0}Of{1}";
            DefaultGenericListNameFormat = "ArrayOf{0}";
        }

        public bool PrettyPrint { get; set; }
        public bool ExcludeNullValues { get; set; }
        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string DefaultGenericTypeNameFormat { get; set; }
        public string DefaultGenericListNameFormat { get; set; }

        public bool HasFormatter(Type type)
        {
            return _formatters.ContainsKey(type);
        }

        public Func<PropertyInfo, object, string> GetFormatter(Type type)
        {
            return _formatters[type];
        }

        public void AddFormatter<T>(Func<PropertyInfo, T, string> formatter)
        {
            _formatters.Add(typeof(T), (p, v) => formatter(p, (T)v));
        } 
    }
}