using System;
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
            Mode = context.Mode;
            Type = member.Type.Type;
        }

        public CachedMember Member { get; private set; }
        public Type Type { get; private set; }
        public Mode Mode { get; private set; }
        public string Format { get; }
        public bool IsXml => Format == XmlNodeBase.NodeFormat;
        public bool IsJson => Format == JsonNode.NodeFormat;
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