using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            _saveOptions = _options.PrettyPrint ? SaveOptions.None : SaveOptions.DisableFormatting;
        }

        public static Serializer Create(Action<SerializerOptions> configure = null)
        {
            var options = new Options();
            if (configure != null) configure(new SerializerOptions(options));
            return new Serializer(options);
        }

        public void Serialize(object @object, Stream stream)
        {
            SerializeAsDocument(@object).Save(stream, _saveOptions);
        }

        public void Serialize(object @object, string path)
        {
            SerializeAsDocument(@object).Save(path, _saveOptions);
        }

        public string Serialize(object @object)
        {
            return SerializeAsDocument(@object).ToString(_saveOptions);
        }

        public XDocument SerializeAsDocument(object source)
        {
            if (source == null) throw new ArgumentNullException("source", "Cannot serialize a null reference.");
            return new XDocument(Traverse(source, LinkedNode<object>.Create(source)));
        }

        private XObject Traverse(object source, LinkedNode<object> ancestors, PropertyInfo sourceProperty = null, Type itemType = null)
        {
            var type = itemType ?? (sourceProperty == null || sourceProperty.PropertyType == typeof(object) ? 
                source.GetType() : sourceProperty.PropertyType);

            var name = sourceProperty != null && itemType == null ? sourceProperty.GetXmlName() :
                (sourceProperty.GetXmlArrayItemName() ?? 
                 type.GetXmlName(_options.GenericTypeNameFormat, _options.GenericListNameFormat, !ancestors.Any()));

            Func<object, XElement> createElement = x => new XElement(_options.DefaultNamespace == null ? 
                name : _options.DefaultNamespace + name, x);

            Func<string, XObject> createValueNode = x => _options.ValueNode == ValueNodeType.Attribute || 
               (sourceProperty != null && sourceProperty.HasCustomAttribute<XmlAttributeAttribute>()) ?
                new XAttribute(name, x ?? "") : (XObject)createElement(x);

            XObject node;

            if (_options.ValueWriters.ContainsKey(type))
            {
                node = createValueNode(null);
                _options.ValueWriters[type](_options, sourceProperty, source, new ValueNode(node));
            }
            else if (type.IsSimpleType()) node = source == null ? createValueNode(null) : createValueNode(source.ToString());
            else if (source == null) node = createElement(null);
            else if (type.IsEnumerable())
            {
                var listItemType = type.GetGenericEnumerableType();
                node = createElement(null).WithChildren(source.AsEnumerable().Select(x => 
                    Traverse(x, ancestors.Add(source), sourceProperty, listItemType ?? x.GetType())));
            }
            else
            {
                node = createElement(null);
                var properties = type.GetSerializableProperties(_options.ExcludedTypes); 
            
                foreach (var property in properties.Where(x => !x.IsIgnored()))
                {
                    var propertyValue = property.GetValue(source, null);
                    if ((propertyValue == null && _options.ExcludeNullValues) || ancestors.Any(propertyValue)) continue;
                    ((XElement)node).Add(Traverse(propertyValue, ancestors.Add(propertyValue), property));
                }
            }

            var valueNode = new ValueNode(node);
            if (!ancestors.Any()) _options.Namespaces.ForEach(x => 
                valueNode.Element.Add(new XAttribute(XNamespace.Xmlns + x.Key, x.Value)));
            _options.NodeWriters.ForEach(x => x(_options, sourceProperty, source, valueNode));
            return valueNode.Object;
        }
    }
}