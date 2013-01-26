using System;
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
            return elementName != null && elementName.ElementName != null ? elementName.ElementName : property.Name;
        }

        public static string GetXmlName(this Type type, string listNameFormat, string typeNameFormat, bool isRoot = false)
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
                var typeArguments = type.GetGenericArguments().Select(x => GetXmlName(x, listNameFormat, typeNameFormat)).Aggregate((a, i) => a + i);
                return typeDefinition == typeof(List<>) || typeDefinition == typeof(IList<>) ?
                    string.Format(listNameFormat, typeArguments) :
                    string.Format(typeNameFormat, typeDefinition.Name.Remove(typeDefinition.Name.IndexOf('`')), typeArguments);
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
    }
}