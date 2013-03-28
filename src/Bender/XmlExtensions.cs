using System;
using System.Collections;
using System.Collections.Generic;
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
            var elementName = property.GetCustomAttribute<XmlElementAttribute>();
            if (elementName != null && elementName.ElementName != null) return elementName.ElementName;
            var arrayName = property.GetCustomAttribute<XmlArrayAttribute>();
            return arrayName != null && arrayName.ElementName != null && property.PropertyType.IsEnumerable() ? arrayName.ElementName : property.Name;
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
            if (type.IsGenericType)
            {
                var typeDefinition = type.GetGenericTypeDefinition();
                var typeArguments = type.GetGenericArguments().Select(x => GetXmlName(x, listNameFormat, genericTypeNameFormat)).Aggregate((a, i) => a + i);
                return typeDefinition == typeof(List<>) || typeDefinition == typeof(IList<>) ?
                    string.Format(listNameFormat ?? "ArrayOf{0}", typeArguments) :
                    string.Format(genericTypeNameFormat ?? "{0}Of{1}", typeDefinition.Name.Remove(typeDefinition.Name.IndexOf('`')), typeArguments);
            }
            return type.Name;
        }

        public static string GetXPath(this XObject node)
        {
            var attribute = node as XAttribute;
            var element = node as XElement ?? attribute.Parent;
            return (element.Ancestors().Any() ? "/" + element.Ancestors().Select(x => x.Name.LocalName)
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
    }
}