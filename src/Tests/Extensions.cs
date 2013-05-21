using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Should;

namespace Tests
{
    public static class Extensions
    {
        public static XDocument ParseJson(this string json)
        {
            return XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(new MemoryStream(Encoding.UTF8.GetBytes(json)), new XmlDictionaryReaderQuotas()));
        }

        public static XElement JsonRoot(this XDocument document)
        {
            document.Root.Name.ShouldEqual("root");
            document.Root.Attribute("type").Value.ShouldEqual("object");
            return document.Root;
        }

        public static IEnumerable<XElement> JsonRootObjectArray(this XDocument document)
        {
            return document.JsonRootArray("object");
        }

        public static IEnumerable<XElement> JsonRootNumberArray(this XDocument document)
        {
            return document.JsonRootArray("number");
        }

        public static IEnumerable<XElement> JsonRootBooleanArray(this XDocument document)
        {
            return document.JsonRootArray("boolean");
        }

        public static IEnumerable<XElement> JsonRootStringArray(this XDocument document)
        {
            return document.JsonRootArray("string");
        }

        private static IEnumerable<XElement> JsonRootArray(this XDocument document, string type)
        {
            document.Root.Name.ShouldEqual("root");
            document.Root.Attribute("type").Value.ShouldEqual("array");
            var elements = document.Root.Elements("item");
            elements.All(x => x.Attribute("type").Value == type).ShouldBeTrue();
            return elements;
        }

        public static IEnumerable<XElement> JsonObjectArrayField(this XElement element, string name)
        {
            return element.JsonArrayField(name, "object");
        }

        public static IEnumerable<XElement> JsonNumberArrayField(this XElement element, string name)
        {
            return element.JsonArrayField(name, "number");
        }

        public static IEnumerable<XElement> JsonBooleanArrayField(this XElement element, string name)
        {
            return element.JsonArrayField(name, "boolean");
        }

        public static IEnumerable<XElement> JsonStringArrayField(this XElement element, string name)
        {
            return element.JsonArrayField(name, "string");
        }

        private static IEnumerable<XElement> JsonArrayField(this XElement element, string name, string type)
        {
            var elements = element.JsonField(name, "array").Elements("item");
            elements.All(x => x.Attribute("type").Value == type).ShouldBeTrue();
            return elements;
        }

        public static bool IsJsonNullField(this XElement element, string name)
        {
            return element.IsJsonField(name, "null");
        }

        public static bool IsJsonObjectField(this XElement element, string name)
        {
            return element.IsJsonField(name, "object");
        }

        public static bool IsJsonStringField(this XElement element, string name)
        {
            return element.IsJsonField(name, "string");
        }

        public static bool IsJsonBooleanField(this XElement element, string name)
        {
            return element.IsJsonField(name, "boolean");
        }

        public static bool IsJsonNumberField(this XElement element, string name)
        {
            return element.IsJsonField(name, "number");
        }

        private static bool IsJsonField(this XElement element, string name, string type)
        {
            var child = element.Element(name);
            return child.Attribute("type").Value == type;
        }

        public static XElement JsonNullField(this XElement element, string name)
        {
            return element.JsonField(name, "null");
        }

        public static XElement JsonObjectField(this XElement element, string name)
        {
            return element.JsonField(name, "object");
        }

        public static XElement JsonStringField(this XElement element, string name)
        {
            return element.JsonField(name, "string");
        }

        public static XElement JsonBooleanField(this XElement element, string name)
        {
            return element.JsonField(name, "boolean");
        }

        public static XElement JsonNumberField(this XElement element, string name)
        {
            return element.JsonField(name, "number");
        }

        private static XElement JsonField(this XElement element, string name, string type)
        {
            var child = element.Element(name);
            child.Attribute("type").Value.ShouldEqual(type);
            return child;
        }

        public static XElement JsonObjectItem(this IEnumerable<XElement> elements, int index)
        {
            return elements.JsonItem(index, "object");
        }

        public static XElement JsonStringItem(this IEnumerable<XElement> elements, int index)
        {
            return elements.JsonItem(index, "string");
        }

        public static XElement JsonBooleanItem(this IEnumerable<XElement> elements, int index)
        {
            return elements.JsonItem(index, "boolean");
        }

        public static XElement JsonNumberItem(this IEnumerable<XElement> elements, int index)
        {
            return elements.JsonItem(index, "number");
        }

        private static XElement JsonItem(this IEnumerable<XElement> elements, int index, string type)
        {
            var child = elements.Where(x => x.Name == "item").Skip(index - 1).First();
            child.Attribute("type").Value.ShouldEqual(type);
            return child;
        }

        public static bool JsonFieldExists(this XElement element, string name)
        {
            return element.Element(name) != null;
        }
    }
}
