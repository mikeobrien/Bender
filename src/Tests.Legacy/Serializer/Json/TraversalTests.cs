using System;
using System.Diagnostics;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Json
{
    [TestFixture]
    public class TraversalTests
    {
        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode { public string Value2 { get; set; } }

        [Test]
        public void should_serialize_graph()
        {
            var json = Bender.Serializer.Create().SerializeJson(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            Debug.WriteLine(json);
            json.ParseJson().JsonRoot().JsonObjectField("Value1").JsonStringField("Value2").Value.ShouldEqual("hai"); 
        }

        // Exclusion

        [Test]
        public void should_exclude_types()
        {
            var json = Bender.Serializer.Create(x => x.ExcludeType<GraphNode>())
                .SerializeJson(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            json.ParseJson().JsonRoot().JsonFieldExists("Value1").ShouldBeFalse();
        }

        // Null handling

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public GraphNode Value2 { get; set; }
        }

        //[Test]
        //public void should_serialize_empty_value_when_null()
        //{
        //    var json = Bender.Serializer.Create().SerializeJson(new NullValue());
        //    json.ParseJson().JsonRoot().IsJsonNullField("Value1").ShouldBeTrue();
        //}

        [Test]
        public void should_exclude_element_when_null()
        {
            var json = Bender.Serializer.Create().SerializeJson(new NullValue());
            json.ParseJson().JsonRoot().JsonFieldExists("Value1").ShouldBeFalse();
        }

        //[Test]
        //public void should_serialize_empty_type_when_null()
        //{
        //    var json = Bender.Serializer.Create().SerializeJson(new NullValue());
        //    json.ParseJson().JsonRoot().IsJsonNullField("Value2").ShouldBeTrue();
        //}

        [Test]
        public void should_exclude_element_when_type_is_null()
        {
            var json = Bender.Serializer.Create().SerializeJson(new NullValue());
            json.ParseJson().JsonRoot().JsonFieldExists("Value2").ShouldBeFalse();
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
            var json = Bender.Serializer.Create().SerializeJson(graph);
            var result = json.ParseJson().JsonRoot().JsonObjectField("Value");
            result.JsonStringField("Value1").Value.ShouldEqual("hai");
            result.JsonFieldExists("Value2").ShouldBeFalse();
        }
    }
}
