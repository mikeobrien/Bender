using System;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Xml
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
        public void should_fail_when_xml_is_not_valid()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeXml<Graph>("<yada><sdfsdf></yada>")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse xml: 'sdfsdf' is expected  Line 1, position 18.");
            #else 
            message.ShouldEqual("Unable to parse xml: The 'sdfsdf' start tag on line 1 position 8 does not match the end tag of 'yada'. Line 1, position 17.");
            #endif
        }

        [Test]
        public void should_fail_when_there_are_multiple_root_elements()
        {
            var message = Assert.Throws<ParseException>(() => Bender.Deserializer.Create()
                .DeserializeXml<Graph>("<oh></oh><hai></hai>")).FriendlyMessage;
            
            #if __MonoCS__
            message.ShouldEqual("Unable to parse xml: Multiple document element was detected.  Line 1, position 11.");
            #else 
            message.ShouldEqual("Unable to parse xml: There are multiple root elements. Line 1, position 11.");
            #endif
        }

        [Test]
        public void should_deserialize_graph_with_element_values()
        {
            const string xml = @"<Graph><Value1><Value2>oh</Value2><Value3>hai</Value3></Value1></Graph>";
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_graph_with_attribute_values()
        {
            const string xml = @"<Graph><Value1 Value2=""oh"" Value3=""hai""/></Graph>";
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_graph_with_attribute_and_element_values()
        {
            const string xml = @"<Graph><Value1 Value2=""oh""><Value3>hai</Value3></Value1></Graph>";
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_types()
        {
            const string xml = @"<Graph><Value1><Value2>hai</Value2></Value1></Graph>";
            Bender.Deserializer.Create(x => x.ExcludeType<GraphNode>()).DeserializeXml<Graph>(xml).Value1.ShouldBeNull();
        }

        // Null handling

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public GraphNode Value2 { get; set; }
        }

        [Test]
        public void should_deserialize_empty_nullable_value_when_null()
        {
            const string xml = "<NullValue><Value1/></NullValue>";
            Bender.Deserializer.Create().DeserializeXml<NullValue>(xml).Value1.ShouldBeNull();
        }

        [Test]
        public void should_deserialize_empty_type()
        {
            const string xml = "<NullValue><Value2/></NullValue>";
            Bender.Deserializer.Create().DeserializeXml<NullValue>(xml).Value2.ShouldNotBeNull();
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
            var result = Bender.Deserializer.Create().DeserializeXml<GraphNode<int, string>>(xml);
            result.Value2.ShouldEqual(67);
        }
    }
}
