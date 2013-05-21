using System;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
            Assert.Throws<SourceParseException>(() => Bender.Deserializer.Create().DeserializeJson<Graph>("{ \"sdfsdf\" }"))
                .FriendlyMessage.ShouldEqual("Unable to parse json: The token ':' was expected but found '}'.");

            Assert.Throws<SourceParseException>(() => Bender.Deserializer.Create().DeserializeJson<Graph>("{ \"sdfsdf\": }"))
                .FriendlyMessage.ShouldEqual("Unable to parse json: Encountered unexpected character '}'.");

            Assert.Throws<SourceParseException>(() => Bender.Deserializer.Create().DeserializeJson<Graph>("{ \"sdfsdf\": { }"))
                .FriendlyMessage.ShouldEqual("Unable to parse json: Unexpected end of file. Following elements are not closed: sdfsdf, root.");

            Assert.Throws<SourceParseException>(() => Bender.Deserializer.Create().DeserializeJson<Graph>("{ \"sdfsdf\": werwer }"))
                .FriendlyMessage.ShouldEqual("Unable to parse json: Encountered unexpected character 'w'.");
        }

        [Test]
        public void should_deserialize_graph()
        {
            const string json = "{ \"Value1\": { \"Value2\": \"oh\", \"Value3\": \"hai\" } }";
            Bender.Deserializer.Create().DeserializeJson<Graph>(json).Value1.Value2.ShouldEqual("oh");
            Bender.Deserializer.Create().DeserializeJson<Graph>(json).Value1.Value3.ShouldEqual("hai");
        }

        [Test]
        public void should_exclude_types()
        {
            const string json = "{ \"Value1\": { \"Value2\": \"hai\" } }";
            Bender.Deserializer.Create(x => x.ExcludeType<GraphNode>().IgnoreUnmatchedNodes()).DeserializeJson<Graph>(json).Value1.ShouldBeNull();
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
            const string json = "{ \"Value1\": null }";
            Bender.Deserializer.Create().DeserializeJson<NullValue>(json).Value1.ShouldBeNull();
        }

        [Test]
        public void should_deserialize_empty_type_when_null()
        {
            const string json = "{ \"Value2\": null }";
            Bender.Deserializer.Create().DeserializeJson<NullValue>(json).Value2.ShouldBeNull();
        }

        // Generic types

        public class GraphNode<T, T2>
        {
            public T Value2 { get; set; }
        }

        [Test]
        public void should_map_generic_type()
        {
            const string json = "{ \"Value2\": 67 }";
            var result = Bender.Deserializer.Create().DeserializeJson<GraphNode<int, string>>(json);
            result.Value2.ShouldEqual(67);
        }
    }
}
