using System.Xml.Serialization;
using Bender.Extensions;

namespace Bender.NamingConventions
{
    public class EnumConventions
    {
        public static NamingConventions<EnumContext> Create()
        {
            return new NamingConventions<EnumContext>(DefaultConvention);
        }

        public static string DefaultConvention(EnumContext context)
        {
            return context.Value.ToString();
                //.PipeWhen(x => x.GetPropertyOrFieldType().Map(y => y.IsEnumerable() && !y.IsSimpleType()))
                //.MapOrDefault(x => x.GetAttribute<XmlArrayItemAttribute>())
                //.MapOrDefault(x => x.ElementName)
                //.PipeWhen(x => !x.IsEmpty());
        }
    }
}