using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class UnmatchedNodeException : Exception { public UnmatchedNodeException(XObject node) : base(node.GetXPath()) {} }

    public class Deserializer
    {
        private readonly Options _options;

        public Deserializer(Options options)
        {
            _options = options;
        }

        public static Deserializer Create(Action<DeserializerOptions> configure = null)
        {
            var options = new Options();
            if (configure != null) configure(new DeserializerOptions(options));
            return new Deserializer(options);
        }

        public T DeserializeFile<T>(string path)
        {
            return (T)DeserializeFile(typeof(T), path);
        }

        public object DeserializeFile(Type type, string path)
        {
            return Deserialize(type, XDocument.Load(path));
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T)Deserialize(typeof(T), stream);
        }

        public object Deserialize(Type type, Stream stream)
        {
            return Deserialize(type, XDocument.Load(stream));
        }

        public T Deserialize<T>(string source)
        {
            return (T)Deserialize(typeof(T), source);
        }

        public object Deserialize(Type type, string source)
        {
            return Deserialize(type, XDocument.Parse(source));
        }

        public object Deserialize<T>(XDocument document)
        {
            return Deserialize(typeof (T), document);
        }

        public object Deserialize(Type type, XDocument document)
        {
            return Deserialize(type, document.Root);
        }

        public object Deserialize<T>(XElement element)
        {
            return Deserialize(typeof(T), element);
        }

        public object Deserialize(Type type, XElement element)
        {
            var instance = Activator.CreateInstance(type);
            ValidateTypeElementName(type, element);
            Traverse(instance, element);
            return instance;
        }

        private void Traverse(object @object, XElement element)
        {
            if (@object.GetType().IsList())
            {
                var type = @object.GetType().GetListType();
                foreach (var itemElement in element.Elements())
                {
                    ValidateTypeElementName(type, itemElement);
                    var item = Activator.CreateInstance(type);
                    Traverse(item, itemElement);
                    ((IList)@object).Add(item);
                }
                return;
            }
            var properties = @object.GetType().GetSerializableProperties()
                .ToDictionary(x => x.GetXmlName(), x => x, _options.IgnoreCase);
            foreach (var node in element.Elements().Cast<XObject>().Concat(element.Attributes()).Select(x => new Node(x)))
            {
                if (!properties.ContainsKey(node.Name.LocalName))
                {
                    if (_options.IgnoreUnmatchedElements) continue;
                    throw new UnmatchedNodeException(node.Object);
                }
                var property = properties[node.Name.LocalName];
                var propertyType = property.PropertyType;
                if (_options.ExcludedTypes.Any(x => x(propertyType)) || 
                    property.HasCustomAttribute<XmlIgnoreAttribute>()) continue;

                if (_options.Readers.ContainsKey(propertyType)) 
                    property.SetValue(@object, _options.Readers[propertyType](_options, property, node), null);
                else if (propertyType.IsPrimitive || propertyType.IsValueType || propertyType == typeof(string))
                    property.SetValue(@object, node.Value.Parse(propertyType, _options.DefaultNonNullableTypesWhenEmpty), null);
                else if (propertyType == typeof(object)) property.SetValue(@object, node.Object, null);
                else
                {
                    var propertyValue = Activator.CreateInstance(propertyType);
                    property.SetValue(@object, propertyValue, null);
                    Traverse(propertyValue, node.Element);
                }
            }
        }

        private void ValidateTypeElementName(Type type, XElement element)
        {
            if (!_options.IgnoreTypeElementNames &&
                !type.GetXmlName(_options.DefaultGenericListNameFormat,
                        _options.DefaultGenericTypeNameFormat).Equals(element.Name.LocalName, 
                            _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new UnmatchedNodeException(element);
        }
    }
}