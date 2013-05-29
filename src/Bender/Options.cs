using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace Bender
{
    public enum XmlValueNodeType { Attribute, Element }
    public enum Format { Xml, Json }
    public enum DictionaryItemFormat { KeyValueStructure, NameValuePair }

    public class Options
    {
        public Options()
        {
            DictionaryItemFormat = DictionaryItemFormat.NameValuePair;
            ExcludedTypes = new List<Func<Type, bool>>();
            XmlValueNodeType = XmlValueNodeType.Element;
            IgnoreUnmatchedXmlAttributes = true;
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
            FriendlyParseErrorMessages[typeof(float)] = "Not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.";
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

            Readers = new Dictionary<Type, Func<ReaderContext, object>>();
            AddReader(x => Convert.FromBase64String(x.Node.Value));
            AddReader(x => new Uri(x.Options.DefaultNonNullableTypesWhenEmpty && x.Node.Value.IsNullOrEmpty() ? "http://tempuri.org/" : x.Node.Value));
            AddReader(x => Version.Parse(x.Node.Value));
            AddReader(x => new MailAddress(x.Node.Value));
            AddReader(x => IPAddress.Parse(x.Node.Value));

            XmlNamespaces = new Dictionary<string, XNamespace>();
            NodeWriters = new List<Action<WriterContext>>();
            ValueWriters = new Dictionary<Type, Action<WriterContext>>();
            AddWriter<bool>(x => x.Node.Value = x.Value.ToString().ToLower(), true);
            AddWriter<byte[]>(x => { if (x.Value != null) x.Node.Value = Convert.ToBase64String(x.Value); });
            AddWriter<Uri>(x => { if (x.Value != null) x.Node.Value = x.Value.ToString(); });
            AddWriter<Version>(x => { if (x.Value != null) x.Node.Value = x.Value.ToString(); });
            AddWriter<MailAddress>(x => { if (x.Value != null) x.Node.Value = x.Value.ToString(); });
            AddWriter<IPAddress>(x => { if (x.Value != null) x.Node.Value = x.Value.ToString(); });
        }
        
        public List<Func<Type, bool>> ExcludedTypes { get; set; }
        public string GenericTypeXmlNameFormat { get; set; }
        public string GenericListXmlNameFormat { get; set; }
        public DictionaryItemFormat DictionaryItemFormat { get; set; }

        // Deserialization specific
        public bool DefaultNonNullableTypesWhenEmpty { get; set; }
        public bool IgnoreUnmatchedNodes { get; set; }
        public bool IgnoreUnmatchedXmlAttributes { get; set; }
        public bool IgnoreTypeXmlElementNames { get; set; }
        public bool IgnoreCase { get; set; }
        public Dictionary<Type, string> FriendlyParseErrorMessages { get; set; }

        public Dictionary<Type, Func<ReaderContext, object>> Readers { get; private set; }

        public void AddReader<T>(Func<ReaderContext, T> reader)
        {
            Readers[typeof(T)] = x => reader(x);
        }

        public void AddReader<T>(Func<ReaderContext, T> reader, bool handleNullable) where T : struct
        {
            AddReader(reader);
            if (handleNullable) Readers[typeof(T?)] = x => !string.IsNullOrEmpty(x.Node.Value) ? reader(x) : (T?)null;
        } 

        // Serialization specific
        public bool PrettyPrintXml { get; set; }
        public bool ExcludeNullValues { get; set; }
        public XmlValueNodeType XmlValueNodeType { get; set; }
        public Dictionary<Type, Action<WriterContext>> ValueWriters { get; private set; }
        public List<Action<WriterContext>> NodeWriters { get; private set; }
        public XNamespace DefaultNamespace { get; set; }
        public Dictionary<string, XNamespace> XmlNamespaces { get; set; }

        public void AddWriter(Action<WriterContext> writer)
        {
            NodeWriters.Add(writer);
        }

        public void AddWriter(Func<WriterContext, bool> predicate, Action<WriterContext> writer)
        {
            NodeWriters.Add(x => { if (predicate(x)) writer(x); });
        }

        public void AddWriter<T>(Action<WriterContext<T>> writer)
        {
            ValueWriters[typeof(T)] = x => writer(new WriterContext<T>(x));
        }

        public void AddWriter<T>(Action<WriterContext<T>> writer, bool handleNullable) where T : struct
        {
            AddWriter(writer);
            if (handleNullable) ValueWriters[typeof(T?)] = x => { if (((T?)x.Value).HasValue) writer(new WriterContext<T>(x)); };
        } 
    }
}