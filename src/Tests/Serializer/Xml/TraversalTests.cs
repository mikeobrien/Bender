using System;
using System.Diagnostics;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class TraversalTests
    {
        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode { public string Value2 { get; set; } }

        [Test]
        public void should_serialize_graph_with_element_values()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            Debug.WriteLine(xml);
            xml.ParseXml().Element("Graph").Element("Value1").Element("Value2").Value.ShouldEqual("hai"); 
        }

        [Test]
        public void should_serialize_graph_with_attribute_values()
        {
            var xml = Bender.Serializer.Create(x => x.XmlValuesAsAttributes()).SerializeXml(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            Debug.WriteLine(xml);
            xml.ParseXml().Element("Graph").Element("Value1").Attribute("Value2").Value.ShouldEqual("hai");
        }

        // Exclusion

        [Test]
        public void should_exclude_types()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeType<GraphNode>())
                .SerializeXml(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            xml.ParseXml().Element("Graph").Element("Value1").ShouldBeNull();
        }

        // Null handling

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public GraphNode Value2 { get; set; }
        }

        [Test]
        public void should_serialize_empty_value_when_null()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new NullValue());
            xml.ParseXml().Element("NullValue").Element("Value1").Value.ShouldBeEmpty(); 
        }

        [Test]
        public void should_exclude_element_when_null()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeNullValues()).SerializeXml(new NullValue());
            xml.ParseXml().Element("NullValue").Element("Value1").ShouldBeNull();
        }

        [Test]
        public void should_serialize_empty_type_when_null()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new NullValue());
            xml.ParseXml().Element("NullValue").Element("Value2").Value.ShouldBeEmpty();
        }

        [Test]
        public void should_exclude_element_when_type_is_null()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeNullValues()).SerializeXml(new NullValue());
            xml.ParseXml().Element("NullValue").Element("Value2").ShouldBeNull();
        }

        // Circular references

        public class CircularReference
        {
            public CircularReferenceNode Value { get; set; }
        }

        public class CircularReferenceNode
        {
            public string Value1 { get; set; }
            public CircularReference Value2 { get; set; }
        }

        [Test]
        public void should_not_serialize_circular_references()
        {
            var graph = new CircularReference { Value = new CircularReferenceNode { Value1 = "hai" } };
            graph.Value.Value2 = graph;
            var xml = Bender.Serializer.Create().SerializeXml(graph);
            var root = xml.ParseXml().Element("CircularReference").Element("Value");
            root.Element("Value1").Value.ShouldEqual("hai");
            root.Element("Value2").ShouldBeNull();
        }
    }
}
