using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bender
{
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

        public T DeserializeJsonFile<T>(string path)
        {
            return (T)DeserializeJson(typeof(T), File.ReadAllText(path));
        }

        public object DeserializeJsonFile(Type type, string path)
        {
            return DeserializeJson(type, File.ReadAllText(path));
        }

        public T DeserializeJson<T>(string source)
        {
            return (T)DeserializeJson(typeof(T), new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }

        public object DeserializeJson(Type type, string source)
        {
            return DeserializeJson(type, new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }

        public T DeserializeJson<T>(Stream stream)
        {
            return (T)DeserializeJson(typeof(T), stream);
        }

        public object DeserializeJson(Type type, Stream stream)
        {
            return Traverse(Format.Json, type, new ValueNode(Exceptions.Wrap<XmlException, XDocument>(() => 
                XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(stream, new XmlDictionaryReaderQuotas())), x => new SourceParseException(x, Format.Json)).Root, Format.Json));
        }

        public T DeserializeXmlFile<T>(string path)
        {
            return (T)DeserializeXmlFile(typeof(T), path);
        }

        public object DeserializeXmlFile(Type type, string path)
        {
            return DeserializeXml(type, Exceptions.Wrap<XmlException, XDocument>(() => XDocument.Load(path), x => new SourceParseException(x, Format.Xml)));
        }

        public T DeserializeXml<T>(Stream stream)
        {
            return (T)DeserializeXml(typeof(T), stream);
        }

        public object DeserializeXml(Type type, Stream stream)
        {
            return DeserializeXml(type, Exceptions.Wrap<XmlException, XDocument>(() => XDocument.Load(stream), x => new SourceParseException(x, Format.Xml)));
        }

        public T DeserializeXml<T>(string source)
        {
            return (T)DeserializeXml(typeof(T), source);
        }

        public object DeserializeXml(Type type, string source)
        {
            return DeserializeXml(type, Exceptions.Wrap<XmlException, XDocument>(source.ParseXml, x => new SourceParseException(x, Format.Xml)));
        }

        public T DeserializeXml<T>(XDocument document)
        {
            return (T)DeserializeXml(typeof (T), document);
        }

        public object DeserializeXml(Type type, XDocument document)
        {
            return DeserializeXml(type, document.Root);
        }

        public T DeserializeXml<T>(XElement element)
        {
            return (T)DeserializeXml(typeof(T), element);
        }

        public object DeserializeXml(Type type, XElement element)
        {
            return Traverse(Format.Xml, type, new ValueNode(element, Format.Xml));
        }

        private object Traverse(Format format, Type type, ValueNode node, object parent = null, PropertyInfo sourceProperty = null)
        {
            if (format == Format.Xml && (parent == null || parent.GetType().IsGenericEnumerable()))
                ValidateTypeElementName(type, node.XmlElement, parent == null, sourceProperty);

            if (_options.Readers.ContainsKey(type))
            {
                var readerContext = new ReaderContext(_options, sourceProperty, node);
                try
                {
                    return _options.Readers[type](readerContext);
                }
                catch (Exception exception)
                {
                    if (exception is SourceException) throw;
                    var messageType = type.GetUnderlyingNullableType();
                    throw new ValueParseException(readerContext, _options.FriendlyParseErrorMessages.ContainsKey(messageType) ?
                        _options.FriendlyParseErrorMessages[messageType] : "Parse error.", exception);
                }
            }

            if (type == typeof (object)) return node.Object;
            if (type.IsSimpleType()) return node.Value.ParseSimpleType(type, _options.DefaultNonNullableTypesWhenEmpty, _options.FriendlyParseErrorMessages);

            if ((node.NodeType == NodeType.JsonField && node.JsonField.DataType == JsonDataType.Null) ||
                (node.NodeType == NodeType.XmlElement && node.XmlElement.IsEmpty && !node.XmlElement.Attributes().Any())) return null;

            if (node.NodeType == NodeType.XmlAttribute)
                throw new DeserializeException(sourceProperty, node.Object, format, "Cannot deserialize attribute value as complex type.");

            if (type.IsGenericEnumerable())
            {
                var list = type.CreateListOfEnumerableType();
                var itemType = type.GetGenericEnumerableType();
                node.XmlElement.Elements().ForEach(x => list.Add(Traverse(format, itemType, new ValueNode(x, format), parent, sourceProperty)));
                return type.IsArray ? list.ToArray(itemType) : list;
            }

            if (type.IsEnumerable())
            {
                const string message = "Cannot deserialize IEnumerable and ArrayList types.";
                if (sourceProperty != null) throw new DeserializeException(sourceProperty, node.Object, format, message);
                throw new DeserializeException(type, message);
            }

            var instance = Activator.CreateInstance(type, parent != null && type.HasConstructor(parent.GetType()) ? new[] { parent } : null);

            var properties = type.GetDeserializableProperties(_options.ExcludedTypes)
                .ToDictionary(x => x.GetXmlName(), x => x, _options.IgnoreCase);
            
            foreach (var childNode in node.XmlElement.Elements().Cast<XObject>()
                .ConcatIf(format == Format.Xml, node.XmlElement.Attributes())
                .Select(x => new ValueNode(x, format)))
            {
                if (!properties.ContainsKey(childNode.Name.LocalName))
                {
                    if ((!_options.IgnoreUnmatchedNodes && childNode.Object.NodeType == XmlNodeType.Element) || 
                        (!_options.IgnoreUnmatchedXmlAttributes && childNode.Object.NodeType == XmlNodeType.Attribute))
                            throw new UnmatchedNodeException(childNode);
                    continue;
                }

                var property = properties[childNode.Name.LocalName];
                var propertyType = property.PropertyType;

                if (property.IsIgnored()) continue;

                if (propertyType.IsSimpleType() || propertyType.HasParameterlessConstructor() ||
                    propertyType.HasConstructor(instance.GetType()) || propertyType.IsEnumerable() ||
                    _options.Readers.ContainsKey(propertyType))
                {
                    var value = Exceptions.Wrap<ParseException>(() => Traverse(format, property.PropertyType, childNode, instance, property), 
                        x => new ValueParseException(property, childNode, x));
                    property.SetValue(instance, value, null);
                }
            }

            return instance;
        }

        private void ValidateTypeElementName(Type type, XElement element, bool isRoot = false, PropertyInfo property = null)
        {
            if (!_options.IgnoreTypeXmlElementNames &&
                !(property.GetXmlArrayItemName() ?? type.GetXmlName(_options.GenericTypeXmlNameFormat, _options.GenericListXmlNameFormat, isRoot))
                     .Equals(element.Name.LocalName, _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new UnmatchedNodeException(new ValueNode(element, Format.Xml));
        }
    }
}