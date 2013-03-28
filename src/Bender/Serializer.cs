using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

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

            Func<XElement> createElement = () => new XElement(name);
            Func<string, XObject> createValueNode = x => _options.ValueNode == ValueNodeType.Element ? 
                (x == null ? new XElement(name) : new XElement(name, x)) : 
                (x == null ? new XAttribute(name, "") : (XObject)new XAttribute(name, x));

            if (_options.Writers.ContainsKey(type))
            {
                var node = createValueNode(null);
                _options.Writers[type](_options, sourceProperty, source, new ValueNode(node));
                return node;
            }

            if (type.IsSimpleType()) return source == null ? createValueNode(null) : createValueNode(source.ToString());

            if (type.IsEnumerable())
            {
                var listItemType = type.GetListType();
                return source == null ? createElement() : createElement()
                    .WithChildren(source.AsEnumerable().Select(x => Traverse(x, ancestors.Add(source), sourceProperty, listItemType)));
            }

            var element = createElement();
            if (source == null) return element;

            var properties = type.GetSerializableProperties(_options.ExcludedTypes); 
            
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(source, null);
                if ((propertyValue == null && _options.ExcludeNullValues) || ancestors.Any(propertyValue)) continue;
                element.Add(Traverse(propertyValue, ancestors.Add(propertyValue), property));
            }

            return element;
        }
    }
}