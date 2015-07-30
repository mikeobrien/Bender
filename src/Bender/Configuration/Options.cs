using System;
using System.Web.UI;
using Bender.NamingConventions;
using Bender.Reflection;

namespace Bender.Configuration
{
    public class Options
    {
        private static readonly Options Empty = new Options();

        public Options()
        {
            Serialization = new SerializationOptions(this);
            Deserialization = new DeserializationOptions(this);
            PropertyNameConventions = MemberNamingConventions.Create();
            FieldNameConventions = MemberNamingConventions.Create();
            ArrayItemNameConventions = ArrayItemConventions.Create();
            TypeNameConventions = TypeNamingConvention.Create();
        }

        public bool TreatEnumerableImplsAsObjects { get; set; }
        public bool TreatDictionaryImplsAsObjects { get; set; }

        public string EnumerableTypeNameFormat { get; set; }
        public string DictionaryTypeNameFormat { get; set; }
        public string DefaultItemTypeName { get; set; }
        public string GenericTypeNameFormat { get; set; }

        public string CsvDelimiter { get; set; } = ",";
        public string CsvQualifier { get; set; } = "\"";
        public string CsvNewLine { get; set; } = "\r\n";

        public NamingConventions<MemberContext> FieldNameConventions { get; set; }
        public NamingConventions<MemberContext> PropertyNameConventions { get; set; }
        public NamingConventions<ArrayItemContext> ArrayItemNameConventions { get; set; }
        public NamingConventions<TypeContext> TypeNameConventions { get; set; }

        public Func<CachedType, Options, bool> TypeFilter { get; set; }
        public Func<CachedMember, Options, bool> MemberFilter { get; set; }

        public bool IncludeNonPublicProperties { get; set; }
        public bool IncludePublicFields { get; set; }
        public bool IncludeNonPublicFields { get; set; } 

        public DeserializationOptions Deserialization { get; private set; }
        public SerializationOptions Serialization { get; private set; }

        public static Options Create(Action<OptionsDsl> configure = null)
        {
            if (configure != null)
            {
                var options = new Options();
                configure(new OptionsDsl(options));
                return options;
            }
            return Empty;
        }
    }
}