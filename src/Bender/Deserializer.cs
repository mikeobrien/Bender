using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Bender
{
    public class UnmatchedNodeException : Exception { public UnmatchedNodeException(XObject node) : 
        base(string.Format("The '{0}' {1} does not correspond to a type or property.", 
        node.GetXPath(), node is XAttribute ? "attribute" : "element")) { } }

    public class SetValueException : Exception 
    { 
        public SetValueException(PropertyInfo property, XObject node, Exception exception) :
            base(GetMessage(property, node, exception.Message), exception) { }

        public SetValueException(PropertyInfo property, XObject node, string message) :
            base(GetMessage(property, node, message)) { } 

        private static string GetMessage(PropertyInfo property, XObject node, string message)
        {
            return string.Format("Unable to set {0}.{1} to the value at '{2}': {3}",
                property.DeclaringType.FullName, property.Name, node.GetXPath(), message);
        }
    }

    public class DeserializeException : Exception
    {
        public DeserializeException(Type type, string message) :
            base(string.Format("Unable to deserialize {0}: {1}", type.FullName, message)) { }
    }

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

        public T Deserialize<T>(XDocument document)
        {
            return (T)Deserialize(typeof (T), document);
        }

        public object Deserialize(Type type, XDocument document)
        {
            return Deserialize(type, document.Root);
        }

        public T Deserialize<T>(XElement element)
        {
            return (T)Deserialize(typeof(T), element);
        }

        public object Deserialize(Type type, XElement element)
        {
            return Traverse(type, new ValueNode(element));
        }

        private object Traverse(Type type, ValueNode node, object parent = null, PropertyInfo sourceProperty = null)
        {
            if (parent == null || parent.GetType().IsGenericEnumerable())
                ValidateTypeElementName(type, node.Element, parent == null, sourceProperty);

            if (_options.Readers.ContainsKey(type)) 
                return _options.Readers[type](_options, sourceProperty, node);

            if (type == typeof (object)) return node.Object;
            if (type.IsSimpleType()) return node.Value.Parse(type, _options.DefaultNonNullableTypesWhenEmpty);

            if (node.NodeType == ValueNodeType.Attribute)
                throw new SetValueException(sourceProperty, node.Object, "Cannot deserialize attribute value as complex type.");

            if (type.IsGenericEnumerable())
            {
                var list = type.CreateListOfEnumerableType();
                var itemType = type.GetGenericEnumerableType();
                node.Element.Elements().ForEach(x => list.Add(Traverse(itemType, new ValueNode(x), parent, sourceProperty)));
                return type.IsArray ? list.ToArray(itemType) : list;
            }

            if (type.IsEnumerable())
            {
                if (sourceProperty != null) throw new SetValueException(sourceProperty, node.Object, "Cannot deserialize IEnumerable and ArrayList types.");
                throw new DeserializeException(type, "Cannot deserialize IEnumerable and ArrayList types.");
            }

            var instance = Activator.CreateInstance(type, parent != null && type.HasConstructor(parent.GetType()) ? new[] { parent } : null);

            var properties = type.GetDeserializableProperties(_options.ExcludedTypes)
                .ToDictionary(x => x.GetXmlName(), x => x, _options.IgnoreCase);
            
            foreach (var childNode in node.Element.Elements().Cast<XObject>().Concat(node.Element.Attributes()).Select(x => new ValueNode(x)))
            {
                if (!properties.ContainsKey(childNode.Name.LocalName))
                {
                    if ((!_options.IgnoreUnmatchedElements && childNode.NodeType == ValueNodeType.Element) || 
                        (!_options.IgnoreUnmatchedAttributes && childNode.NodeType == ValueNodeType.Attribute))
                            throw new UnmatchedNodeException(childNode.Object);
                    continue;
                }

                var property = properties[childNode.Name.LocalName];
                var propertyType = property.PropertyType;

                if (property.IsIgnored()) continue;

                if (propertyType.IsSimpleType() || propertyType.HasParameterlessConstructor() || 
                    propertyType.HasConstructor(instance.GetType()) || propertyType.IsEnumerable())
                    property.SetValue(instance, () => Traverse(property.PropertyType, childNode, instance, property), 
                        x => new SetValueException(property, childNode.Object, x));
            }

            return instance;
        }

        private void ValidateTypeElementName(Type type, XElement element, bool isRoot = false, PropertyInfo property = null)
        {
            if (!_options.IgnoreTypeElementNames &&
                !(property.GetXmlArrayItemName() ?? type.GetXmlName(_options.GenericTypeNameFormat, _options.GenericListNameFormat, isRoot))
                     .Equals(element.Name.LocalName, _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new UnmatchedNodeException(element);
        }
    }
}