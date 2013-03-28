using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace Bender
{
    public class Options
    {
        public Options()
        {
            ExcludedTypes = new List<Func<Type, bool>>();
            ValueNode = ValueNodeType.Element;
            IgnoreUnmatchedAttributes = true;

            Readers = new Dictionary<Type, Func<Options, PropertyInfo, ValueNode, object>>();
            AddReader((o, p, e) => Convert.FromBase64String(e.Value));
            AddReader((o, p, e) => new Uri(o.DefaultNonNullableTypesWhenEmpty && e.Value.IsNullOrEmpty() ? "http://tempuri.org/" : e.Value));
            AddReader((o, p, e) => Version.Parse(e.Value));
            AddReader((o, p, e) => new MailAddress(e.Value));
            AddReader((o, p, e) => IPAddress.Parse(e.Value));

            Writers = new Dictionary<Type, Action<Options, PropertyInfo, object, ValueNode>>();
            AddWriter<bool>((o, p, v, e) => e.Value = v.ToString().ToLower(), true);
            AddWriter<byte[]>((o, p, v, e) => e.Value = Convert.ToBase64String(v));
            AddWriter<Uri>((o, p, v, e) => e.Value = v != null ? v.ToString() : "");
            AddWriter<Version>((o, p, v, e) => e.Value = v.ToString());
            AddWriter<MailAddress>((o, p, v, e) => e.Value = v.ToString());
            AddWriter<IPAddress>((o, p, v, e) => e.Value = v.ToString());
        }
        
        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string GenericTypeNameFormat { get; set; }
        public string GenericListNameFormat { get; set; }

        // Deserialization specific
        public bool DefaultNonNullableTypesWhenEmpty { get; set; }
        public bool IgnoreUnmatchedElements { get; set; }
        public bool IgnoreUnmatchedAttributes { get; set; }
        public bool IgnoreTypeElementNames { get; set; }
        public bool IgnoreCase { get; set; }

        public Dictionary<Type, Func<Options, PropertyInfo, ValueNode, object>> Readers { get; private set; }

        public void AddReader<T>(Func<Options, PropertyInfo, ValueNode, T> reader)
        {
            Readers[typeof(T)] = (o, p, e) => reader(o, p, e);
        }

        public void AddReader<T>(Func<Options, PropertyInfo, ValueNode, T> reader, bool handleNullable) where T : struct
        {
            AddReader(reader);
            if (handleNullable) Readers[typeof(T?)] = (o, p, e) => !string.IsNullOrEmpty(e.Value) ? reader(o, p, e) : (T?)null;
        } 

        // Serialization specific
        public bool PrettyPrint { get; set; }
        public bool ExcludeNullValues { get; set; }
        public ValueNodeType ValueNode { get; set; }
        public Dictionary<Type, Action<Options, PropertyInfo, object, ValueNode>> Writers { get; private set; }

        public void AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writer)
        {
            Writers[typeof(T)] = (o, p, v, e) => writer(o, p, (T)v, e);
        }

        public void AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writer, bool handleNullable) where T : struct
        {
            AddWriter(writer);
            if (handleNullable) Writers[typeof(T?)] = (o, p, v, e) => { if (((T?)v).HasValue) writer(o, p, ((T?)v).Value, e); };
        } 
    }
}