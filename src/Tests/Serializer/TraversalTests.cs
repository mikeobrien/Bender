using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class TraversalTests
    {
        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode { public string Value2 { get; set; } }

        [Test]
        public void should_serialize_graph_with_element_values()
        {
            var xml = Bender.Serializer.Create().Serialize(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            Debug.WriteLine(xml);
            XDocument.Parse(xml).Element("Graph").Element("Value1").Element("Value2").Value.ShouldEqual("hai"); 
        }

        // Exclusion

        [Test]
        public void should_exclude_types()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeType<GraphNode>())
                .Serialize(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            XDocument.Parse(xml).Element("Graph").Element("Value1").ShouldBeNull();
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
            var xml = Bender.Serializer.Create().Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value1").Value.ShouldBeEmpty(); 
        }

        [Test]
        public void should_exclude_element_when_null()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeNullValues()).Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value1").ShouldBeNull();
        }

        [Test]
        public void should_serialize_empty_type_when_null()
        {
            var xml = Bender.Serializer.Create().Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value2").Value.ShouldBeEmpty();
        }

        [Test]
        public void should_exclude_element_when_type_is_null()
        {
            var xml = Bender.Serializer.Create(x => x.ExcludeNullValues()).Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value2").ShouldBeNull();
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
            var xml = Bender.Serializer.Create().Serialize(graph);
            var root = XDocument.Parse(xml).Element("CircularReference").Element("Value");
            root.Element("Value1").Value.ShouldEqual("hai");
            root.Element("Value2").ShouldBeNull();
        }
    }
}
