using System.Xml.Serialization;
using Bender.Collections;
using Bender.Extensions;
using Bender.Reflection;

namespace Bender.NamingConventions
{
    public class TypeNamingConvention
    {
        public static NamingConventions<TypeContext> Create()
        {
            return new NamingConventions<TypeContext>(DefaultConvention);
        }

        public const string DefaultItemTypeName = "AnyType";
        public const string DefaultEnumerableNameFormat = "ArrayOf{0}";
        public const string DefaultDictionaryNameFormat = "DictionaryOf{0}";
        public const string DefaultGenericTypeNameFormat = "{0}Of{1}";

        public static string DefaultConvention(TypeContext context)
        {
            var type = context.Type.IsNullable ? context.Type.UnderlyingType : context.Type;
            var kind = type.GetKind(
                context.Options.TreatEnumerableImplsAsObjects,
                context.Options.TreatDictionaryImplsAsObjects);

            if (kind.IsSimple()) return type.Name;

            var name = type.GetAttribute<XmlRootAttribute>().PipeWhen(context.IsRoot)
                    .MapOrDefault(x => x.ElementName).PipeWhen(x => !x.IsEmpty()) ??
                type.GetAttribute<XmlTypeAttribute>()
                    .MapOrDefault(x => x.TypeName).PipeWhen(x => !x.IsEmpty());
            if (name != null) return name;

            if (kind.IsDictionary())
            {
                var itemTypeName = context.Options.DefaultItemTypeName ?? (!type.IsGenericDictionary ? DefaultItemTypeName :
                    DefaultConvention(context.OfType(type.GenericDictionaryTypes.Value)));
                return (context.Options.DictionaryTypeNameFormat ?? DefaultDictionaryNameFormat).ToFormat(itemTypeName);
            }

            if (kind.IsEnumerable())
            {
                var itemTypeName = DefaultItemTypeName;
                if (context.Options.DefaultItemTypeName != null) itemTypeName = context.Options.DefaultItemTypeName;
                else if (type.IsArray) itemTypeName = DefaultConvention(context.OfType(type.ElementType));
                else if (type.IsGenericEnumerable) itemTypeName = DefaultConvention(
                    context.OfType(type.GenericEnumerableType));
                return (context.Options.EnumerableTypeNameFormat ?? DefaultEnumerableNameFormat).ToFormat(itemTypeName);
            }

            if (type.IsGenericType)
                return (context.Options.GenericTypeNameFormat ?? DefaultGenericTypeNameFormat)
                    .ToFormat(type.GenericBaseName, type.GenericArguments.Aggregate(x =>
                        DefaultConvention(context.OfType(x))));

            return type.Name;
        }
    }
}