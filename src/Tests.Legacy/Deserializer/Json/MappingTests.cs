using System;
using Bender.Nodes;
using Bender.Nodes.Object;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Json
{
    [TestFixture]
    public class MappingTests
    {
        public class ComplexType
        {
            public string StringValue { get; set; }
            public int IntegerValue { get; set; }
        }

        // Case sensitivity

        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode { public string Value2 { get; set; } }

        [Test]
        public void should_map_elements_and_values_case_insensitively()
        {
            const string json = @"{ ""value1"": { ""value2"": ""hai"" } }";
            Bender.Deserializer.Create(x => x.Deserialization(y => y.IgnoreNameCase())).DeserializeJson<Graph>(json).Value1.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_not_map_elements_and_values_case_insensitively()
        {
            const string json = @"{ ""value1"": { ""value2"": ""hai"" } }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserialize
                .Json<Graph>(json, x => x.Deserialization(y => y.FailOnUnmatchedElements())))
                .InnerException.ShouldBeType<UnrecognizedNodeDeserializationException>();
        }

        // Missing elements

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public TraversalTests.GraphNode Value2 { get; set; }
            public int Value3 { get; set; }
        }

        [Test]
        public void should_set_type_with_missing_element_to_null()
        {
            Bender.Deserializer.Create().DeserializeJson<NullValue>("{}").Value2.ShouldBeNull();
        }

        [Test]
        public void should_set_value_with_missing_element_to_default()
        {
            Bender.Deserializer.Create().DeserializeJson<NullValue>("{}").Value3.ShouldEqual(0);
        }

        [Test]
        public void should_set_nullable_value_with_missing_element_to_null()
        {
            Bender.Deserializer.Create().DeserializeJson<NullValue>("{}").Value1.ShouldBeNull();
        }

        // Unmapped elements

        [Test]
        public void should_not_fail_on_unmapped_elements()
        {
            const string json = "{ \"yada\": \"hai\" }";
            ComplexType result = null;
            Assert.DoesNotThrow(() => result = Bender.Deserialize.Json<ComplexType>(json));
            result.ShouldNotBeNull();
            result.StringValue.ShouldBeNull();
        }

        [Test]
        public void should_fail_on_unmapped_elements()
        {
            const string json = "{ \"yada\": \"hai\" }";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(
                x => x.Deserialization(y => y.FailOnUnmatchedElements()))
                .DeserializeJson<ComplexType>(json))
                .InnerException.ShouldBeType<UnrecognizedNodeDeserializationException>();
        }
    }
}
