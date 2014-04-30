using System;
using Bender.Extensions;
using Bender.NamingConventions;
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

        public OptionsDsl TreatEnumerableImplementationsAsObjects()
        {
            _options.TreatEnumerableImplementationsAsObjects = true;
            return this;
        }

        public OptionsDsl TreatDictionaryImplementationsAsObjects()
        {
            _options.TreatDictionaryImplementationsAsObjects = true;
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

        // Convenience naming conventions

        public OptionsDsl UseSnakeCaseNaming()
        {
            return WithNamingConvention(x => x.ToSeperatedCase(true, "_"));
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
            Func<string, string> toSnakeCase = x => x.ToSeperatedCase(lower, "-");
            WithTypeNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            WithArrayItemNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            WithMemberNamingConvention((n, c) => toSnakeCase(n), (t, c) => c.IsXml);
            return this;
        }

        // Member naming conventions

        public OptionsDsl WithMemberNamingConvention(
            Func<MemberContext, string> convention)
        {
            _options.FieldNameConventions.SetDefault(convention);
            _options.PropertyNameConventions.SetDefault(convention);
            return this;
        }

        public OptionsDsl WithMemberNamingConvention(
            Func<MemberContext, string> convention,
            Func<MemberContext, bool> when)
        {
            // Mono 2.10.8 build fails when lambdas passed in directly.
            _options.FieldNameConventions.Add(x => convention(x), x => when(x));
            _options.PropertyNameConventions.Add(x => convention(x), x => when(x));
            return this;
        }

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
            // Mono 2.10.8 build fails when lambdas passed in directly.
            _options.FieldNameConventions.Add((n, c) => convention(n, c), (n, c) => when(n, c));
            _options.PropertyNameConventions.Add((n, c) => convention(n, c), (n, c) => when(n, c));
            return this;
        }

        // Field naming conventions

        public OptionsDsl WithFieldNamingConvention(
            Func<MemberContext, string> convention)
        {
            _options.FieldNameConventions.SetDefault(convention);
            return this;
        }

        public OptionsDsl WithFieldNamingConvention(
            Func<MemberContext, string> convention,
            Func<MemberContext, bool> when)
        {
            _options.FieldNameConventions.Add(convention, when);
            return this;
        }

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

        public OptionsDsl WithPropertyNamingConvention(
            Func<MemberContext, string> convention)
        {
            _options.PropertyNameConventions.SetDefault(convention);
            return this;
        }

        public OptionsDsl WithPropertyNamingConvention(
            Func<MemberContext, string> convention,
            Func<MemberContext, bool> when)
        {
            _options.PropertyNameConventions.Add(convention, when);
            return this;
        }

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

        public OptionsDsl WithArrayItemNamingConvention(
            Func<ArrayItemContext, string> convention)
        {
            _options.ArrayItemNameConventions.SetDefault(convention);
            return this;
        }

        public OptionsDsl WithArrayItemNamingConvention(
            Func<ArrayItemContext, string> convention,
            Func<ArrayItemContext, bool> when)
        {
            _options.ArrayItemNameConventions.Add(convention, when);
            return this;
        }

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
            Func<TypeContext, string> convention)
        {
            _options.TypeNameConventions.SetDefault(convention);
            return this;
        }

        public OptionsDsl WithTypeNamingConvention(
            Func<TypeContext, string> convention,
            Func<TypeContext, bool> when)
        {
            _options.TypeNameConventions.Add(convention, when);
            return this;
        }

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
            configure(new SerializerOptionsDsl(_options.Serialization));
            return this;
        }

        public OptionsDsl Deserialization(Action<DeserializerOptionsDsl> configure)
        {
            configure(new DeserializerOptionsDsl(_options.Deserialization));
            return this;
        }
    }
}