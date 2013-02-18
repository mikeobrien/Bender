using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class UnmatchedNodeException : Exception { public UnmatchedNodeException(XObject node) : 
        base(string.Format("The '{0}' {1} does not correspond to a type or property.", 
        node.GetXPath(), node is XAttribute ? "attribute" : "element")) { } }

    public class SetValueException : Exception { public SetValueException(PropertyInfo property, XObject node, Exception exception) :
        base(string.Format("Unable to set {0}.{1} to the value at '{2}': {3}", 
        property.DeclaringType.FullName, property.Name, node.GetXPath(), exception.Message), exception) { } }

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
            var instance = type.IsListInterface() ? type.CreateList() : Activator.CreateInstance(type);
            ValidateTypeElementName(type, element, true);
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

            var properties = @object.GetType().GetDeserializableProperties(_options.ExcludedTypes)
                .ToDictionary(x => x.GetXmlName(), x => x, _options.IgnoreCase);
            
            foreach (var node in element.Elements().Cast<XObject>().Concat(element.Attributes()).Select(x => new Node(x)))
            {
                if (!properties.ContainsKey(node.Name.LocalName))
                {
                    if ((!_options.IgnoreUnmatchedElements && node.NodeType == NodeType.Element) || 
                        (!_options.IgnoreUnmatchedAttributes && node.NodeType == NodeType.Attribute))
                            throw new UnmatchedNodeException(node.Object);
                    continue;
                }

                var property = properties[node.Name.LocalName];
                var propertyType = property.PropertyType;

                Action<Func<object>> setValue = x => property.SetValue(
                    @object, x, y => new SetValueException(property, node.Object, y));

                if (_options.Readers.ContainsKey(propertyType)) 
                    setValue(() => _options.Readers[propertyType](_options, property, node));
                else if (propertyType.IsPrimitive || propertyType.IsValueType || propertyType == typeof (string))
                    setValue(() => node.Value.Parse(propertyType, _options.DefaultNonNullableTypesWhenEmpty));
                else if (propertyType == typeof(object)) property.SetValue(@object, node.Object, null);
                else if (node.NodeType == NodeType.Element && (propertyType.IsListInterface() || 
                    propertyType.HasParameterlessConstructor() || propertyType.HasConstructor(@object.GetType())))
                {
                    object propertyValue;
                    if (propertyType.IsList() || propertyType.IsListInterface()) propertyValue = propertyType.CreateList();
                    else propertyValue = Activator.CreateInstance(propertyType, 
                        propertyType.HasConstructor(@object.GetType()) ? new [] { @object } : null);
                    property.SetValue(@object, propertyValue, null);
                    Traverse(propertyValue, node.Element);
                }
            }
        }

        private void ValidateTypeElementName(Type type, XElement element, bool isRoot = false)
        {
            if (!_options.IgnoreTypeElementNames &&
                !type.GetXmlName(_options.DefaultGenericListNameFormat,
                        _options.DefaultGenericTypeNameFormat, isRoot).Equals(element.Name.LocalName, 
                            _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new UnmatchedNodeException(element);
        }
    }
}