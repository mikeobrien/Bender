using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Xml.Linq;

namespace Bender
{
    public enum ValueNodeType { Attribute, Element }

    public class Options
    {
        public Options()
        {
            ExcludedTypes = new List<Func<Type, bool>>();
            ValueNode = ValueNodeType.Element;
            IgnoreUnmatchedAttributes = true;
            FriendlyParseErrorMessages = new Dictionary<Type, string>();
            FriendlyParseErrorMessages[typeof(Enum)] = "Not a valid option.";
            FriendlyParseErrorMessages[typeof(char)] = "Length not valid, must be one character.";
            FriendlyParseErrorMessages[typeof(bool)] = "Not formatted correctly, must be 'true' or 'false'.";
            FriendlyParseErrorMessages[typeof(sbyte)] = "Not formatted correctly, must be an integer between -128 and 127.";
            FriendlyParseErrorMessages[typeof(byte)] = "Not formatted correctly, must be an integer between 0 and 255.";
            FriendlyParseErrorMessages[typeof(short)] = "Not formatted correctly, must be an integer between -32,768 and 32,767.";
            FriendlyParseErrorMessages[typeof(ushort)] = "Not formatted correctly, must be an integer between 0 and 65,535.";
            FriendlyParseErrorMessages[typeof(int)] = "Not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.";
            FriendlyParseErrorMessages[typeof(uint)] = "Not formatted correctly, must be an integer between 0 and 4,294,967,295.";
            FriendlyParseErrorMessages[typeof(long)] = "Not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.";
            FriendlyParseErrorMessages[typeof(ulong)] = "Not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.";
            FriendlyParseErrorMessages[typeof(float)] = "Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38";
            FriendlyParseErrorMessages[typeof(double)] = "Not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.";
            FriendlyParseErrorMessages[typeof(decimal)] = "Not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.";
            FriendlyParseErrorMessages[typeof(DateTime)] = "Not formatted correctly, must be formatted as m/d/yyy h:m:s AM.";
            FriendlyParseErrorMessages[typeof(Guid)] = "Not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
            FriendlyParseErrorMessages[typeof(TimeSpan)] = "Not formatted correctly, must be formatted as d.h:m:s.";
            FriendlyParseErrorMessages[typeof(byte[])] = "Not formatted correctly, must be formatted as base64 string.";
            FriendlyParseErrorMessages[typeof(Uri)] = "Not formatted correctly, must be formatted as 'http://domain.com'.";
            FriendlyParseErrorMessages[typeof(Version)] = "Not formatted correctly, must be formatted as '1.2.3.4'.";
            FriendlyParseErrorMessages[typeof(MailAddress)] = "Not formatted correctly, must be formatted as 'username@domain.com'.";
            FriendlyParseErrorMessages[typeof(IPAddress)] = "Not formatted correctly, must be formatted as '1.2.3.4'.";

            Readers = new Dictionary<Type, Func<Options, PropertyInfo, ValueNode, object>>();
            AddReader((o, p, e) => Exceptions.Wrap(() => Convert.FromBase64String(e.Value), x => 
                new ValueParseException(p, e.Object, e.Value, new ParseException(x, o.FriendlyParseErrorMessages[typeof(byte[])]))));
            AddReader((o, p, e) => Exceptions.Wrap(() => new Uri(o.DefaultNonNullableTypesWhenEmpty && e.Value.IsNullOrEmpty() ? "http://tempuri.org/" : e.Value), x => 
                new ValueParseException(p, e.Object, e.Value, new ParseException(x, o.FriendlyParseErrorMessages[typeof(Uri)]))));
            AddReader((o, p, e) => Exceptions.Wrap(() => Version.Parse(e.Value), x => 
                new ValueParseException(p, e.Object, e.Value, new ParseException(x, o.FriendlyParseErrorMessages[typeof(Version)]))));
            AddReader((o, p, e) => Exceptions.Wrap(() => new MailAddress(e.Value), x => 
                new ValueParseException(p, e.Object, e.Value, new ParseException(x, o.FriendlyParseErrorMessages[typeof(MailAddress)]))));
            AddReader((o, p, e) => Exceptions.Wrap(() => IPAddress.Parse(e.Value), x => 
                new ValueParseException(p, e.Object, e.Value, new ParseException(x, o.FriendlyParseErrorMessages[typeof(IPAddress)]))));

            Namespaces = new Dictionary<string, XNamespace>();
            NodeWriters = new List<Action<Options, PropertyInfo, object, ValueNode>>();
            ValueWriters = new Dictionary<Type, Action<Options, PropertyInfo, object, ValueNode>>();
            AddWriter<bool>((o, p, v, e) => e.Value = v.ToString().ToLower(), true);
            AddWriter<byte[]>((o, p, v, e) => e.Value = v != null ? Convert.ToBase64String(v) : "");
            AddWriter<Uri>((o, p, v, e) => e.Value = v != null ? v.ToString() : "");
            AddWriter<Version>((o, p, v, e) => e.Value = v != null ? v.ToString() : "");
            AddWriter<MailAddress>((o, p, v, e) => e.Value = v != null ? v.ToString() : "");
            AddWriter<IPAddress>((o, p, v, e) => e.Value = v != null ? v.ToString() : "");
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
        public Dictionary<Type, string> FriendlyParseErrorMessages { get; set; } 

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
        public Dictionary<Type, Action<Options, PropertyInfo, object, ValueNode>> ValueWriters { get; private set; }
        public List<Action<Options, PropertyInfo, object, ValueNode>> NodeWriters { get; private set; }
        public XNamespace DefaultNamespace { get; set; }
        public Dictionary<string, XNamespace> Namespaces { get; set; } 

        public void AddWriter(Action<Options, PropertyInfo, object, ValueNode> writer)
        {
            NodeWriters.Add(writer);
        }

        public void AddWriter(Func<Options, PropertyInfo, object, ValueNode, bool> predicate, Action<Options, PropertyInfo, object, ValueNode> writer)
        {
            NodeWriters.Add((o, p, v, e) => { if (predicate(o, p, v, e)) writer(o, p, v, e); });
        }

        public void AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writer)
        {
            ValueWriters[typeof(T)] = (o, p, v, e) => writer(o, p, (T)v, e);
        }

        public void AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writer, bool handleNullable) where T : struct
        {
            AddWriter(writer);
            if (handleNullable) ValueWriters[typeof(T?)] = (o, p, v, e) => { if (((T?)v).HasValue) writer(o, p, ((T?)v).Value, e); };
        } 
    }
}