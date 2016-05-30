using System.Xml.Serialization;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Xml;

namespace Bender.NamingConventions
{
    public class ArrayItemNamingConventions
    {
        public static NamingConventions<ArrayItemContext> Create()
        {
            return new NamingConventions<ArrayItemContext>(DefaultConvention);
        }

        public static string DefaultConvention(ArrayItemContext context)
        {
            return context.Member
                .PipeWhen(x => x.Type.Map(y => y.IsEnumerable && !y.IsSimpleType))
                .MapOrDefault(x => 
                    x.MapOrDefault(y => y.GetAttribute<XmlArrayItemAttribute>())
                     .MapOrDefault(y => y.ElementName) ??
                    x.MapOrDefault(y => y.GetAttribute<XmlSiblingsAttribute>())
                     .MapOrDefault(y => y.ElementName))
                .PipeWhen(x => !x.IsEmpty());
        }
    }
}