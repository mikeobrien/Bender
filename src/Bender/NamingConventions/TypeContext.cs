using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;
using Bender.Reflection;

namespace Bender.NamingConventions
{
    public class TypeContext
    {
        public TypeContext(CachedType type, Context context, bool isRoot = false)
        {
            Type = type;
            IsRoot = isRoot;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        private TypeContext(CachedType type, TypeContext context)
        {
            Type = type;
            IsRoot = context.IsRoot;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        public CachedType Type { get; private set; }
        public bool IsRoot { get; private set; }
        public Options Options { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; private set; }
        public bool IsXml { get { return Format == XmlNodeBase.NodeFormat; } }
        public bool IsJson { get { return Format == JsonNode.NodeFormat; } }

        public TypeContext OfType(CachedType type)
        {
            return new TypeContext(type, this);
        }
    }

    public static class TypeContextExtensions
    {
        public static string GetName(this NamingConventions<TypeContext> conventions,
            CachedType type, Context context, bool isRoot = false)
        {
            return conventions.GetName(new TypeContext(type, context, isRoot));
        }
    }
}