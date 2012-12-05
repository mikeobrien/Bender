using System;
using System.Reflection;

namespace Bender
{
    public class SerializerOptions
    {
        private readonly Options _options;

        public SerializerOptions(Options options)
        {
            _options = options;
        }

        public SerializerOptions PrettyPrint()
        {
            _options.PrettyPrint = true;
            return this;
        }

        public SerializerOptions ExcludeNullValues()
        {
            _options.ExcludeNullValues = true;
            return this;
        }

        public SerializerOptions AddWriter<T>(Func<PropertyInfo, T, string> writter)
        {
            _options.AddWriter(writter);
            return this;
        }

        public SerializerOptions ExcludeTypes(Func<Type, bool> typeFilter)
        {
            _options.ExcludedTypes.Add(typeFilter);
            return this;
        }

        public SerializerOptions ExcludeType<T>()
        {
            _options.ExcludedTypes.Add(x => x == typeof(T));
            return this;
        }

        public SerializerOptions WithDefaultGenericTypeNameFormat(string typeNameFormat)
        {
            _options.DefaultGenericTypeNameFormat = typeNameFormat;
            return this;
        }

        public SerializerOptions WithDefaultGenericListNameFormat(string listNameFormat)
        {
            _options.DefaultGenericListNameFormat = listNameFormat;
            return this;
        }
    }
}