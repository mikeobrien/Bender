using System;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;

namespace Bender.NamingConventions
{
    public class EnumContext 
    {
        public EnumContext(object value, Type type, Context context)
        {
            Value = value;
            Type = type;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        public object Value { get; private set; }
        public Type Type { get; private set; }
        public Options Options { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; }
        public bool IsXml => Format == XmlNodeBase.NodeFormat;
        public bool IsJson => Format == JsonNode.NodeFormat;
    }

    public static class EnumContextContextExtensions
    {
        public static string GetName(this NamingConventions<EnumContext> conventions,
            object value, Type type, Context context)
        {
            return conventions.GetName(new EnumContext(value, type, context));
        }
    }
}