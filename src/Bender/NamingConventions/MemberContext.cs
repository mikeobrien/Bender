using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Xml;
using Bender.Reflection;

namespace Bender.NamingConventions
{
    public class MemberContext
    {
        public MemberContext(CachedMember member, Context context)
        {
            Member = member;
            Format = context.Format;
            Options = context.Options;
            Mode = context.Mode;
        }

        public CachedMember Member { get; private set; }
        public Options Options { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; private set; }
        public bool IsXml { get { return Format == XmlNodeBase.NodeFormat; } }
        public bool IsJson { get { return Format == JsonNode.NodeFormat; } }
    }

    public static class MemberContextExtensions
    {
        public static string GetName(this NamingConventions<MemberContext> conventions,
            CachedMember member, Context context)
        {
            return conventions.GetName(new MemberContext(member, context));
        }
    }
}