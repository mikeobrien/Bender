using System;
using System.Reflection;

namespace Bender
{
    public class DeserializerOptions
    {
        private readonly Options _options;

        public DeserializerOptions(Options options)
        {
            _options = options;
        }

        public DeserializerOptions AddReader<T>(Func<Options, PropertyInfo, string, T> reader)
        {
            _options.AddReader(reader);
            return this;
        }

        public DeserializerOptions DefaultNonNullableTypesWhenEmpty()
        {
            _options.DefaultNonNullableTypesWhenEmpty = true;
            return this;
        }

        public DeserializerOptions ExcludeTypes(Func<Type, bool> typeFilter)
        {
            _options.ExcludedTypes.Add(typeFilter);
            return this;
        }

        public DeserializerOptions ExcludeType<T>()
        {
            _options.ExcludedTypes.Add(x => x == typeof(T));
            return this;
        }

        public DeserializerOptions WithDefaultGenericTypeNameFormat(string typeNameFormat)
        {
            _options.DefaultGenericTypeNameFormat = typeNameFormat;
            return this;
        }

        public DeserializerOptions WithDefaultGenericListNameFormat(string listNameFormat)
        {
            _options.DefaultGenericListNameFormat = listNameFormat;
            return this;
        }
    }
}