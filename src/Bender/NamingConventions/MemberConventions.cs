using System.Xml.Serialization;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Xml;

namespace Bender.NamingConventions
{
    public class MemberNamingConventions
    {
        public static NamingConventions<MemberContext> Create()
        {
            return new NamingConventions<MemberContext>(DefaultConvention);
        }

        public static string DefaultConvention(MemberContext context)
        {
            return context.Member
                    .GetAttribute<System.Xml.Serialization.XmlAttributeAttribute>()
                    .MapOrDefault(x => x.AttributeName)
                    .PipeWhen(x => !x.IsEmpty()) ??
                context.Member
                    .GetAttribute<Bender.Nodes.Xml.XmlAttributeAttribute>()
                    .MapOrDefault(x => x.AttributeName)
                    .PipeWhen(x => !x.IsEmpty()) ??
                context.Member.GetAttribute<XmlElementAttribute>()
                    .MapOrDefault(x => x.ElementName)
                    .PipeWhen(x => !x.IsEmpty()) ??
                context.Member.PipeWhen(x => x.Type.Map(y => y.IsEnumerable && !y.IsSimpleType))
                    .MapOrDefault(x => x.GetAttribute<XmlArrayAttribute>())
                    .MapOrDefault(x => x.ElementName)
                    .PipeWhen(x => !x.IsEmpty()) ??
                context.Member.PipeWhen(x => x.Type.Map(y => y.IsEnumerable && !y.IsSimpleType))
                    .MapOrDefault(x => x.GetAttribute<XmlSiblingsAttribute>())
                    .MapOrDefault(x => x.ElementName)
                    .PipeWhen(x => !x.IsEmpty()) ??
                context.Member.Name;
        }
    }
}