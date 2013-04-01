using System;
using System.Reflection;
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

        public SerializerOptions AddWriter(Action<Options, PropertyInfo, object, ValueNode> writter)
        {
            _options.AddWriter(writter);
            return this;
        }

        public SerializerOptions AddWriter(Func<Options, PropertyInfo, object, ValueNode, bool> predicate, 
            Action<Options, PropertyInfo, object, ValueNode> writter)
        {
            _options.AddWriter(predicate, writter);
            return this;
        }

        public SerializerOptions AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writter)
        {
            _options.AddWriter(writter);
            return this;
        }

        public SerializerOptions AddWriter<T>(Action<Options, PropertyInfo, T, ValueNode> writer, bool handleNullable) where T : struct
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

        public SerializerOptions WithDefaultGenericTypeNameFormat(string typeNameFormat)
        {
            _options.GenericTypeNameFormat = typeNameFormat;
            return this;
        }

        public SerializerOptions WithDefaultGenericListNameFormat(string listNameFormat)
        {
            _options.GenericListNameFormat = listNameFormat;
            return this;
        }

        public SerializerOptions ValuesAsAttributes()
        {
            _options.ValueNode = ValueNodeType.Attribute;
            return this;
        }

        public SerializerOptions WithDefaultNamespace(string @namespace)
        {
            _options.DefaultNamespace = XNamespace.Get(@namespace);
            return this;
        }

        public SerializerOptions AddNamespace(string prefix, string @namespace)
        {
            _options.Namespaces.Add(prefix, XNamespace.Get(@namespace));
            return this;
        }
    }
}