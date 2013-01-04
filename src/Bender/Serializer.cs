using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class Serializer
    {
        private class NodeMap
        {
            public NodeMap Parent { get; set; }
            public object Instance { get; set; }
        }

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

        public XDocument SerializeAsDocument(object @object)
        {
            var document = new XDocument();
            var type = @object.GetType();
            document.Add(new XElement(type.GetXmlName(
                _options.DefaultGenericListNameFormat, _options.DefaultGenericTypeNameFormat)));
            Traverse(type, @object, document.Root, new NodeMap());
            return document;
        }

        private void Traverse(Type type, object @object, XElement element, NodeMap parent)
        {
            if (@object == null || parent.Traverse(x => x.Parent).Any(y => y.Instance == @object)) return;
            var nodeMap = new NodeMap { Parent = parent, Instance = @object };
            if (@object.GetType().IsList())
            {
                foreach (var item in (IList)@object)
                {
                    var itemElement = new XElement(item.GetType()
                        .GetXmlName(_options.DefaultGenericListNameFormat, 
                                        _options.DefaultGenericTypeNameFormat));
                    Traverse(item.GetType(), item, itemElement, nodeMap);
                    element.Add(itemElement);
                }
                return;
            }
            var properties = type.GetSerializableProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(@object, null);
                var propertyType = property.PropertyType == typeof(object) && propertyValue != null ? 
                    propertyValue.GetType() : property.PropertyType;

                if (_options.ExcludedTypes.Any(x => x(propertyType)) ||
                    property.HasCustomAttribute<XmlIgnoreAttribute>()) continue;

                if (propertyValue == null && _options.ExcludeNullValues) continue;

                var propertyName = property.GetXmlName();

                Func<Node> createNode = () => {
                    var node = Node.Create(propertyName, _options.ValueNode);
                    element.Add(node.Object);
                    return node;
                };

                if (_options.Writers.ContainsKey(propertyType))
                    _options.Writers[propertyType](_options, property, propertyValue, createNode());
                else if (propertyType.IsPrimitive || propertyType.IsValueType ||
                         propertyType == typeof (string) || propertyType == typeof (object))
                    createNode().Value = propertyValue == null ? "" : propertyValue.ToString();
                else Traverse(propertyType, propertyValue, element.AddElement(propertyName), nodeMap);
            }
        }
    }
}
