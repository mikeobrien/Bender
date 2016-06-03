using System;
using System.Diagnostics;
using System.Text;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Reflection;

namespace Bender.Configuration
{
    public class OptionsDsl
    {
        private readonly Options _options;

        public OptionsDsl(Options options)
        {
            _options = options;
        }

        public OptionsDsl WithGenericTypeNameFormat(string format)
        {
            _options.GenericTypeNameFormat = format;
            return this;
        }

        public OptionsDsl WithEnumerableTypeNameFormat(string format)
        {
            _options.EnumerableTypeNameFormat = format;
            return this;
        }

        public OptionsDsl WithDictionaryTypeNameFormat(string format)
        {
            _options.DictionaryTypeNameFormat = format;
            return this;
        }

        public OptionsDsl WithDefaultItemTypeName(string name)
        {
            _options.DefaultItemTypeName = name;
            return this;
        }

        public OptionsDsl TreatEnumerableImplsAsObjects()
        {
            _options.TreatEnumerableImplsAsObjects = true;
            return this;
        }

        public OptionsDsl TreatDictionaryImplsAsObjects()
        {
            _options.TreatDictionaryImplsAsObjects = true;
            return this;
        }

        // CSV Options

        public OptionsDsl WithCsvDelimiter(string delimiter)
        {
            _options.CsvDelimiter = delimiter;
            return this;
        }

        public OptionsDsl TabSeperated()
        {
            _options.CsvDelimiter = "\t";
            return this;
        }

        public OptionsDsl WithCsvQualifier(string qualifier)
        {
            _options.CsvQualifier = qualifier;
            return this;
        }

        public OptionsDsl WithCsvNewLine(string newLine)
        {
            _options.CsvNewLine = newLine;
            return this;
        }

        // Member Filtering

        public OptionsDsl IncludeNonPublicProperties()
        {
            _options.IncludeNonPublicProperties = true;
            return this;
        }

        public OptionsDsl IncludePublicFields()
        {
            _options.IncludePublicFields = true;
            return this;
        }

        public OptionsDsl IncludeNonPublicFields()
        {
            _options.IncludeNonPublicFields = true;
            return this;
        }

        // Type and member filters

        public OptionsDsl IncludeTypesWhen(Func<CachedType, Options, bool> filter)
        {
            _options.TypeFilter = _options.TypeFilter.And(filter);
            return this;
        }

        public OptionsDsl ExcludeTypesWhen(Func<CachedType, Options, bool> filter)
        {
            _options.TypeFilter = _options.TypeFilter.AndNot(filter);
            return this;
        }

        public OptionsDsl ExcludeType<T>()
        {
            return ExcludeTypesWhen((t, o) => t.Is<T>());
        }

        public OptionsDsl IncludeMembersWhen(Func<CachedMember, Options, bool> filter)
        {
            _options.MemberFilter = _options.MemberFilter.And(filter);
            return this;
        }

        public OptionsDsl ExcludeMembersWhen(Func<CachedMember, Options, bool> filter)
        {
            _options.MemberFilter = _options.MemberFilter.AndNot(filter);
            return this;
        }

        // Enum naming conventions

        public OptionsDsl WithEnumNamingConvention(
            Func<string, string> convention)
        {
            _options.EnumValueNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention)
        {
            _options.EnumValueNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention,
            Func<string, EnumContext, bool> when)
        {
            _options.EnumValueNameConventions.Add(convention, when);
            return this;
        }

        public OptionsDsl UseEnumSnakeCaseNaming(bool lower = false)
        {
            return WithEnumNamingConvention(
                (v, c) => v.Replace("_", ""),
                (v, c) => c.Mode == Mode.Deserialize)
            .WithEnumNamingConvention(
                (v, c) => v.ToSeparatedCase(lower, "_"),
                (v, c) => c.Mode == Mode.Serialize);
        }

        // Convenience naming conventions

        public OptionsDsl UseSnakeCaseNaming()
        {
            return WithNamingConvention(x => x.ToSeparatedCase(true, "_"));
        }

        public OptionsDsl UseCamelCaseNaming()
        {
            return WithNamingConvention(x => x.ToCamelCase());
        }

        public OptionsDsl UseJsonCamelCaseNaming()
        {
            return WithMemberNamingConvention((n, c) => n.ToCamelCase(), (n, c) => c.IsJson);
        }

        public OptionsDsl UseXmlSpinalCaseNaming()
        {
            return UseXmlDashedCaseNaming(true);
        }

        public OptionsDsl UseXmlTrainCaseNaming()
        {
            return UseXmlDashedCaseNaming(false);
        }

        private OptionsDsl UseXmlDashedCaseNaming(bool lower)
        {
            Func<string, string> toSnakeCase = x => x.ToSeparatedCase(lower, "-");
            WithTypeNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            WithArrayItemNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            WithMemberNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            return this;
        }

        // Member naming conventions

        public OptionsDsl WithMemberNamingConvention(
            Func<string, MemberContext, string> convention)
        {
            _options.FieldNameConventions.Add(convention);
            _options.PropertyNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithMemberNamingConvention(
            Func<string, MemberContext, string> convention,
            Func<string, MemberContext, bool> when)
        {
            _options.FieldNameConventions.Add(convention, when);
            _options.PropertyNameConventions.Add(convention, when);
            return this;
        }

        // Field naming conventions

        public OptionsDsl WithFieldNamingConvention(
            Func<string, MemberContext, string> convention)
        {
            _options.FieldNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithFieldNamingConvention(
            Func<string, MemberContext, string> convention,
            Func<string, MemberContext, bool> when)
        {
            _options.FieldNameConventions.Add(convention, when);
            return this;
        }

        // Property naming conventions

        public OptionsDsl WithPropertyNamingConvention(Func<string,
            MemberContext, string> convention)
        {
            _options.PropertyNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithPropertyNamingConvention(Func<string, 
            MemberContext, string> convention,
            Func<string, MemberContext, bool> when)
        {
            _options.PropertyNameConventions.Add(convention, when);
            return this;
        }

        // Array item naming conventions

        public OptionsDsl WithArrayItemNamingConvention(Func<string,
            ArrayItemContext, string> convention)
        {
            _options.ArrayItemNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithArrayItemNamingConvention(Func<string,
            ArrayItemContext, string> convention,
            Func<string, ArrayItemContext, bool> when)
        {
            _options.ArrayItemNameConventions.Add(convention, when);
            return this;
        }

        // Type naming conventions

        public OptionsDsl WithTypeNamingConvention(
            Func<string, TypeContext, string> convention)
        {
            _options.TypeNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithTypeNamingConvention(
            Func<string, TypeContext, string> convention,
            Func<string, TypeContext, bool> when)
        {
            _options.TypeNameConventions.Add(convention, when);
            return this;
        }

        // Global naming conventions

        public OptionsDsl WithNamingConvention(Func<string, string> convention)
        {
            _options.TypeNameConventions.Add(convention);
            _options.ArrayItemNameConventions.Add(convention);
            _options.PropertyNameConventions.Add(convention);
            _options.FieldNameConventions.Add(convention);
            return this;
        }

        public OptionsDsl WithNamingConvention(Func<string, string> convention,
            Func<string, bool> when)
        {
            _options.TypeNameConventions.Add(convention, when);
            _options.ArrayItemNameConventions.Add(convention, when);
            _options.PropertyNameConventions.Add(convention, when);
            _options.FieldNameConventions.Add(convention, when);
            return this;
        }

        // De/serialization specific

        public OptionsDsl Serialization(Action<SerializerOptionsDsl> configure)
        {
            configure(new SerializerOptionsDsl(_options));
            return this;
        }

        public OptionsDsl Deserialization(Action<DeserializerOptionsDsl> configure)
        {
            configure(new DeserializerOptionsDsl(_options));
            return this;
        }

        public OptionsDsl DoItForTheLulz()
        {
            Process.Start(Encoding.UTF8.GetString(Convert.FromBase64String(
                "aHR0cDovL2ltZ3VyLmNvbS9nYWxsZXJ5LzVNYnE1")));
            return this;
        }
    }
}