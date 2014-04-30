using System;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;
using Bender.Reflection;

namespace Bender.NamingConventions
{
    public class EnumContext 
    {
        public EnumContext(object value, Type type, CachedMember member, Context context)
        {
            Value = value;
            Type = type;
            Member = member;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        public object Value { get; private set; }
        public Type Type { get; private set; }
        public CachedMember Member { get; private set; }
        public bool HasMember { get { return Member != null; } }
        public Options Options { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; private set; }
        public bool IsXml { get { return Format == XmlNodeBase.NodeFormat; } }
        public bool IsJson { get { return Format == JsonNode.NodeFormat; } }
    }

    public static class EnumContextContextExtensions
    {
        public static string GetName(this NamingConventions<EnumContext> conventions,
            object value, Type type, CachedMember member, Context context)
        {
            return conventions.GetName(new EnumContext(value, type, member, context));
        }
    }
}