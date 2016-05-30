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
        public bool IsRoot { get; }
        public Options Options { get; }
        public Mode Mode { get; }
        public string Format { get; }
        public bool IsXml => Format == XmlNodeBase.NodeFormat;
        public bool IsJson => Format == JsonNode.NodeFormat;

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