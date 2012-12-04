using System;
using System.Reflection;

namespace Bender
{
    public class SerializerDsl
    {
        private readonly SerializerOptions _options;

        public SerializerDsl(SerializerOptions options)
        {
            _options = options;
        }

        public SerializerDsl PrettyPrint()
        {
            _options.PrettyPrint = true;
            return this;
        }

        public SerializerDsl ExcludeNullValues()
        {
            _options.ExcludeNullValues = true;
            return this;
        }

        public SerializerDsl AddFormatter<T>(Func<PropertyInfo, T, string> formatter)
        {
            _options.AddFormatter(formatter);
            return this;
        }

        public SerializerDsl ExcludeTypes(Func<Type, bool> typeFilter)
        {
            _options.ExcludedTypes.Add(typeFilter);
            return this;
        }

        public SerializerDsl ExcludeType<T>()
        {
            _options.ExcludedTypes.Add(x => x == typeof(T));
            return this;
        }

        public SerializerDsl WithDefaultGenericTypeNameFormat(string typeNameFormat)
        {
            _options.DefaultGenericTypeNameFormat = typeNameFormat;
            return this;
        }

        public SerializerDsl WithDefaultGenericListNameFormat(string listNameFormat)
        {
            _options.DefaultGenericListNameFormat = listNameFormat;
            return this;
        }
    }
}