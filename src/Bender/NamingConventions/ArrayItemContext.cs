using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;
using Bender.Reflection;

namespace Bender.NamingConventions
{
    public class ArrayItemContext 
    {
        public ArrayItemContext(CachedType type, CachedMember member, Context context)
        {
            Type = type;
            Member = member;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        public CachedType Type { get; private set; }
        public CachedMember Member { get; private set; }
        public bool HasMember { get { return Member != null; } }
        public Options Options { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; private set; }
        public bool IsXml { get { return Format == XmlNodeBase.NodeFormat; } }
        public bool IsJson { get { return Format == JsonNode.NodeFormat; } }
    }

    public static class ArrayItemContextExtensions
    {
        public static string GetName(this NamingConventions<ArrayItemContext> conventions,
            CachedType type, CachedMember member, Context context)
        {
            return conventions.GetName(new ArrayItemContext(type, member, context));
        }
    }
}