using System.Xml.Serialization;
using Bender.Collections;
using Bender.Extensions;

namespace Bender.NamingConventions
{
    public class ArrayItemConventions
    {
        public static NamingConventions<ArrayItemContext> Create()
        {
            return new NamingConventions<ArrayItemContext>(DefaultConvention);
        }

        public static string DefaultConvention(ArrayItemContext context)
        {
            return context.Member
                .PipeWhen(x => x.Type.Map(y => y.IsEnumerable && !y.IsSimpleType))
                .MapOrDefault(x => x.GetAttribute<XmlArrayItemAttribute>())
                .MapOrDefault(x => x.ElementName)
                .PipeWhen(x => !x.IsEmpty());
        }
    }
}