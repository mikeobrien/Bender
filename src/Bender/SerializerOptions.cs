using System;
using System.Xml.Linq;

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

        public SerializerOptions AddWriter(Action<WriterContext> writter)
        {
            _options.AddWriter(writter);
            return this;
        }

        public SerializerOptions AddWriter(Func<WriterContext, bool> predicate,
            Action<WriterContext> writter)
        {
            _options.AddWriter(predicate, writter);
            return this;
        }

        public SerializerOptions AddWriter<T>(Action<WriterContext<T>> writter)
        {
            _options.AddWriter(writter);
            return this;
        }

        public SerializerOptions AddWriter<T>(Action<WriterContext<T>> writer, bool handleNullable) where T : struct
        {
            _options.AddWriter(writer, handleNullable);
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

        public SerializerOptions WithDefaultGenericTypeXmlNameFormat(string typeNameFormat)
        {
            _options.GenericTypeXmlNameFormat = typeNameFormat;
            return this;
        }

        public SerializerOptions WithDefaultGenericListXmlNameFormat(string listNameFormat)
        {
            _options.GenericListXmlNameFormat = listNameFormat;
            return this;
        }

        public SerializerOptions XmlValuesAsAttributes()
        {
            _options.XmlValueNode = XmlValueNodeType.Attribute;
            return this;
        }

        public SerializerOptions WithDefaultXmlNamespace(string @namespace)
        {
            _options.DefaultNamespace = XNamespace.Get(@namespace);
            return this;
        }

        public SerializerOptions AddXmlNamespace(string prefix, string @namespace)
        {
            _options.Namespaces.Add(prefix, XNamespace.Get(@namespace));
            return this;
        }
    }
}