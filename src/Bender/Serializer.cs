using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public class Serializer
    {
        private class Node
        {
            public Node Parent { get; set; }
            public object Instance { get; set; }
        }

        private readonly SerializerOptions _options;

        public Serializer(SerializerOptions options)
        {
            _options = options;
        }

        public static Serializer Create(Action<SerializerDsl> configure = null)
        {
            var options = new SerializerOptions();
            if (configure != null) configure(new SerializerDsl(options));
            return new Serializer(options);
        }

        public string Serialize(object @object)
        {
            var document = new XDocument();
            var type = @object.GetType();
            document.Add(new XElement(GetTypeName(type)));
            TraverseGraph(type, @object, document.Root, new Node());
            return document.ToString(_options.PrettyPrint ? SaveOptions.None : SaveOptions.DisableFormatting);
        }

        private void TraverseGraph(Type type, object @object, XElement element, Node parent)
        {
            if (@object == null || parent.Traverse(x => x.Parent).Any(y => y.Instance == @object)) return;
            var node = new Node { Parent = parent, Instance = @object };
            if (@object is IList)
            {
                foreach (var item in (IList)@object)
                {
                    var itemElement = new XElement(GetTypeName(item.GetType()));
                    TraverseGraph(item.GetType(), item, itemElement, node);
                    element.Add(itemElement);
                }
                return;
            }
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (_options.ExcludedTypes.Any(x => x(propertyType))) continue;
                var propertyValue = property.GetValue(@object);
                if (propertyValue == null && _options.ExcludeNullValues) continue;
                var elementName = property.GetCustomAttribute<XmlElementAttribute>();
                var propertyElement = new XElement(
                    elementName != null && elementName.ElementName != null ? elementName.ElementName : property.Name);
                element.Add(propertyElement);
                var hasFormatter = _options.HasFormatter(propertyType);
                if (propertyType.IsPrimitive || propertyType.IsValueType || 
                    propertyType == typeof(string) || propertyType == typeof(Uri) ||
                    propertyType == typeof(byte[]) || hasFormatter)
                {
                    propertyElement.Value = hasFormatter ? 
                        _options.GetFormatter(propertyType)(property, propertyValue) : 
                        (propertyValue == null ? "" : propertyValue.ToString());
                }
                else TraverseGraph(propertyType, propertyValue, propertyElement, node);
            }
        }

        private string GetTypeName(Type type)
        {
            var xmlType = type.GetCustomAttribute<XmlTypeAttribute>();
            if (xmlType != null && xmlType.TypeName != null) return xmlType.TypeName;
            if (type.IsGenericType)
            {
                var typeDefinition = type.GetGenericTypeDefinition();
                var typeArguments = type.GetGenericArguments().Select(GetTypeName).Aggregate((a, i) => a + i);
                return typeDefinition == typeof(List<>) ? 
                    string.Format(_options.DefaultGenericListNameFormat, typeArguments) :
                    string.Format(_options.DefaultGenericTypeNameFormat, typeDefinition.Name.Remove(typeDefinition.Name.IndexOf('`')), typeArguments);
            }
            return type.Name;
        }
    }
}
