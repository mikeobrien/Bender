using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class Serializer
    {
        private readonly Options _options;
        private readonly SaveOptions _saveOptions;

        public Serializer(Options options)
        {
            _options = options;
            _saveOptions = _options.PrettyPrintXml ? SaveOptions.None : SaveOptions.DisableFormatting;
        }

        public static Serializer Create(Action<SerializerOptions> configure = null)
        {
            var options = new Options();
            if (configure != null) configure(new SerializerOptions(options));
            return new Serializer(options);
        }

        public Stream SerializeJson(object @object, Stream stream)
        {
            var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8);
            SerializeXmlAsDocument(@object, Format.Json).Save(writer);
            writer.Flush();
            return stream;
        }

        public void SerializeJson(object @object, string path)
        {
            using (var stream = File.Create(path)) SerializeJson(@object, stream);
        }

        public string SerializeJson(object @object)
        {
            using (var stream = new MemoryStream())
            {
                SerializeJson(@object, stream).Position = 0;
                return new StreamReader(stream).ReadToEnd();
            }
        }

        public Stream SerializeXml(object @object, Stream stream)
        {
            SerializeXmlAsDocument(@object).Save(stream, _saveOptions);
            return stream;
        }

        public void SerializeXml(object @object, string path)
        {
            SerializeXmlAsDocument(@object).Save(path, _saveOptions);
        }

        public string SerializeXml(object @object)
        {
            return SerializeXmlAsDocument(@object).ToString(_saveOptions);
        }

        public XDocument SerializeXmlAsDocument(object source)
        {
            return SerializeXmlAsDocument(source, Format.Xml);
        }

        private XDocument SerializeXmlAsDocument(object source, Format format)
        {
            if (source == null) throw new ArgumentNullException("source", "Cannot serialize a null reference.");
            return new XDocument(Traverse(format, source, LinkedNode<object>.Create(source)));
        }

        private XObject Traverse(Format format, object source, LinkedNode<object> ancestors, PropertyInfo sourceProperty = null, Type itemType = null)
        {
            var type = itemType ?? (sourceProperty == null ? source.GetType() : sourceProperty.PropertyType);

            var name = format == Format.Xml ? GetXmlNodeName(type, ancestors, sourceProperty, itemType) :
                GetJsonNodeName(ancestors, sourceProperty, itemType);

            Func<object, XElement> createElement = x => new XElement(_options.DefaultNamespace == null || format == Format.Json ?
                name : _options.DefaultNamespace + name, x, format == Format.Json ? new XAttribute("type", "object") : null);

            Func<string, XObject> createValueNode = x => _options.XmlValueNodeType == XmlValueNodeType.Attribute || 
               (sourceProperty != null && sourceProperty.HasCustomAttribute<XmlAttributeAttribute>()) ?
                new XAttribute(name, x ?? "") : (XObject)createElement(x);

            XObject node;
            ValueNode valueNode = null;

            if (_options.ValueWriters.ContainsKey(type))
            {
                node = createValueNode(null);
                valueNode = new ValueNode(node, format);
                if (format == Format.Json) valueNode.JsonField.DataType = source.ToJsonValueType();
                _options.ValueWriters[type](new WriterContext(_options, sourceProperty, source, valueNode));
            }
            else if (type.IsSimpleType())
            {
                node = source == null ? createValueNode(null) : createValueNode(source.ToString());
                valueNode = new ValueNode(node, format);
                if (format == Format.Json) valueNode.JsonField.DataType = source.ToJsonValueType();
            }
            else if (source == null)
            {
                node = createElement(null);
                valueNode = new ValueNode(node, format);
                if (format == Format.Json) valueNode.JsonField.DataType = JsonDataType.Null;
            }
            else if (type.IsEnumerable())
            {
                var listItemType = type.GetGenericEnumerableType();
                node = createElement(null).WithChildren(source.AsEnumerable().Select(x =>
                    Traverse(format, x, ancestors.Add(source), sourceProperty, listItemType ?? x.GetType())));
                valueNode = new ValueNode(node, format);
                if (format == Format.Json) valueNode.JsonField.DataType = JsonDataType.Array;
            }
            else
            {
                node = createElement(null);
                var properties = type.GetSerializableProperties(_options.ExcludedTypes); 
            
                foreach (var property in properties.Where(x => !x.IsIgnored()))
                {
                    var propertyValue = property.GetValue(source, null);
                    if ((propertyValue == null && _options.ExcludeNullValues) || ancestors.Any(propertyValue)) continue;
                    ((XElement)node).Add(Traverse(format, propertyValue, ancestors.Add(propertyValue), property));
                }
            }

            valueNode = valueNode ?? new ValueNode(node, format);
            
            AddNamespaces(format, ancestors, valueNode);

            _options.NodeWriters.ForEach(x => x(new WriterContext(_options, sourceProperty, source, valueNode)));

            return valueNode.Object;
        }

        private void AddNamespaces(Format format, LinkedNode<object> ancestors, ValueNode valueNode)
        {
            if (format == Format.Xml && !ancestors.Any()) _options.XmlNamespaces.ForEach(x => 
                valueNode.XmlElement.Add(new XAttribute(XNamespace.Xmlns + x.Key, x.Value)));
        }

        private static string GetJsonNodeName(LinkedNode<object> ancestors, PropertyInfo sourceProperty, Type itemType)
        {
            return !ancestors.Any() ? "root" : (itemType != null ? "item" : sourceProperty.GetXmlName());
        }

        private string GetXmlNodeName(Type type, LinkedNode<object> ancestors, PropertyInfo sourceProperty, Type itemType)
        {
            return sourceProperty != null && itemType == null ? sourceProperty.GetXmlName() :
                (sourceProperty.GetXmlArrayItemName() ?? type.GetXmlName(_options.GenericTypeXmlNameFormat, _options.GenericListXmlNameFormat, !ancestors.Any()));
        }
    }
}