using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class TraversalTests
    {
        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode
        {
            public string Value2 { get; set; }
            public string Value3 { get; set; }
        }

        [Test]
        public void should_deserialize_graph_with_element_values()
        {
            const string xml = @"<Graph><Value1><Value2>oh</Value2><Value3>hai</Value3></Value1></Graph>";
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_graph_with_attribute_values()
        {
            const string xml = @"<Graph><Value1 Value2=""oh"" Value3=""hai""/></Graph>";
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_graph_with_attribute_and_element_values()
        {
            const string xml = @"<Graph><Value1 Value2=""oh""><Value3>hai</Value3></Value1></Graph>";
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().Deserialize<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_types()
        {
            const string xml = @"<Graph><Value1><Value2>hai</Value2></Value1></Graph>";
            Bender.Deserializer.Create(x => x.ExcludeType<GraphNode>().IgnoreUnmatchedElements()).Deserialize<Graph>(xml).Value1.ShouldBeNull();
        }

        // Generic types

        public class GraphNode<T, T2>
        {
            public T Value2 { get; set; }
        }

        [Test]
        public void should_map_generic_type()
        {
            const string xml = @"<GraphNodeOfInt32String><Value2>67</Value2></GraphNodeOfInt32String>";
            var result = Bender.Deserializer.Create().Deserialize<GraphNode<int, string>>(xml);
            result.Value2.ShouldEqual(67);
        }
    }
}
