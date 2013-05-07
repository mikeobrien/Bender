using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bender
{
    public static class XmlExtensions
    {
        public static string GetXmlName(this PropertyInfo property)
        {
            var attributeName = property.GetCustomAttribute<XmlAttributeAttribute>();
            if (attributeName != null && !string.IsNullOrEmpty(attributeName.AttributeName)) return attributeName.AttributeName;
            var elementName = property.GetCustomAttribute<XmlElementAttribute>();
            if (elementName != null && !string.IsNullOrEmpty(elementName.ElementName)) return elementName.ElementName;
            var arrayName = property.GetCustomAttribute<XmlArrayAttribute>();
            return arrayName != null && !string.IsNullOrEmpty(arrayName.ElementName) && property.PropertyType.IsEnumerable() ? 
                arrayName.ElementName : property.Name;
        }

        public static string GetXmlArrayItemName(this PropertyInfo property)
        {
            if (property == null) return null;
            var arrayItemAttribute = property.GetCustomAttribute<XmlArrayItemAttribute>();
            return arrayItemAttribute != null && arrayItemAttribute.ElementName != null && property.PropertyType.IsEnumerable() ? arrayItemAttribute.ElementName : null;
        }

        public static string GetXmlName(this Type type, string genericTypeNameFormat = null, string listNameFormat = null, bool isRoot = false)
        {
            if (isRoot)
            {
                var xmlRoot = type.GetCustomAttribute<XmlRootAttribute>();
                if (xmlRoot != null && xmlRoot.ElementName != null) return xmlRoot.ElementName;
            }
            var xmlType = type.GetCustomAttribute<XmlTypeAttribute>();
            if (xmlType != null && xmlType.TypeName != null) return xmlType.TypeName;
            const string defaultArrayNameFormat = "ArrayOf{0}";
            if (type.IsArray) return (listNameFormat ?? defaultArrayNameFormat).ToFormat(type.GetElementType().GetXmlName());
            if (type.IsGenericEnumerable() && type.IsClrCollectionType()) return (listNameFormat ?? defaultArrayNameFormat).ToFormat(type.GetGenericEnumerableType().GetXmlName());
            if (type.IsEnumerable() && type.IsClrCollectionType()) return (listNameFormat ?? defaultArrayNameFormat).ToFormat("Object");
            if (type.IsGenericType)
            {
                var typeDefinition = type.GetGenericTypeDefinition();
                var typeArguments = type.GetGenericArguments().Select(x => GetXmlName(x, listNameFormat, genericTypeNameFormat)).Aggregate((a, i) => a + i);
                return (genericTypeNameFormat ?? "{0}Of{1}").ToFormat(typeDefinition.Name.Remove(typeDefinition.Name.IndexOf('`')), typeArguments);
            }
            return type.Name;
        }

        public static string ObjectType(this XObject node)
        {
            return node is XAttribute ? "attribute" : (node is XElement ? "element" : "object");
        }

        public static bool IsIgnored(this PropertyInfo property)
        {
            return property.HasCustomAttribute<XmlIgnoreAttribute>();
        }

        public static string GetXPath(this XObject node)
        {
            var attribute = node as XAttribute;
            var element = node as XElement ?? attribute.Parent;
            return (element.Ancestors().Any() ? "/" + element.Ancestors().Select(x => x.Name.LocalName).Reverse()
                .Aggregate((a, i) => a + "/" + i) : "") + "/" + element.Name.LocalName + (attribute != null ? "/@" + attribute.Name : "");
        }

        public static XElement AddElement(this XElement element, string name)
        {
            var childElement = new XElement(name);
            element.Add(childElement);
            return childElement;
        } 

        public static XElement WithValue(this XElement element, string value)
        {
            element.Value = value;
            return element;
        }

        public static XElement WithChildren(this XElement element, IEnumerable children)
        {
            children.ForEach(element.Add);
            return element;
        }

        public static XDocument ParseXml(this string xml)
        {
            return XDocument.Parse(xml);
        }
    }
}