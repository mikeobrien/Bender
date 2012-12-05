using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

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

        public T Deserialize<T>(string source)
        {
            return (T)Deserialize(typeof(T), source);
        }

        public object Deserialize(Type type, string source)
        {
            var document = XDocument.Parse(source);
            var instance = Activator.CreateInstance(type);
            Traverse(instance, document.Root);
            return instance;
        }

        public void Traverse(object @object, XElement element)
        {
            if (@object.GetType().IsList())
            {
                var type = @object.GetType().GetListType();
                foreach (var itemElement in element.Elements(
                    type.GetXmlName(_options.DefaultGenericListNameFormat, 
                                        _options.DefaultGenericTypeNameFormat)))
                {
                    var item = Activator.CreateInstance(type);
                    Traverse(item, itemElement);
                    ((IList)@object).Add(item);
                }
                return;
            }
            var properties = @object.GetType().GetSerializableProperties()
                .ToDictionary(x => x.GetXmlName(), x => x);
            foreach (var propertyElement in element.Elements())
            {
                if (!properties.ContainsKey(propertyElement.Name.LocalName)) continue;
                var property = properties[propertyElement.Name.LocalName];
                var propertyType = property.PropertyType;
                if (_options.ExcludedTypes.Any(x => x(propertyType)) || 
                    property.HasCustomAttribute<XmlIgnoreAttribute>()) continue;

                if (_options.Readers.ContainsKey(propertyType)) 
                    property.SetValue(@object, _options.Readers[propertyType](property, propertyElement.Value));
                else if (propertyType.IsPrimitive || propertyType.IsValueType || 
                         propertyType == typeof(string) || propertyType == typeof(Uri))
                    property.SetValue(@object, propertyElement.Value.Parse(propertyType, _options.DefaultNonNullableTypesWhenEmpty));
                else
                {
                    var propertyValue = Activator.CreateInstance(propertyType);
                    property.SetValue(@object, propertyValue);
                    Traverse(propertyValue, propertyElement);
                }
            }
        }
    }
}