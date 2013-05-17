using System;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
            Bender.Deserializer.Create(x => x.IgnoreCase()).DeserializeJson<Graph>(json).Value1.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_not_map_elements_and_values_case_insensitively()
        {
            const string json = @"{ ""value1"": { ""value2"": ""hai"" } }";
            Assert.Throws<UnmatchedNodeException>(() => Bender.Deserializer.Create().DeserializeJson<Graph>(json))
                .FriendlyMessage.ShouldEqual("Unable to read json: The 'value1' field is not recognized.");
        }

        // Empty elements

        [Test]
        public void should_deserialize_empty_complex_type_element()
        {
            const string json = @"{ ""Value1"": """" }";
            Bender.Deserializer.Create().DeserializeJson<Graph>(json).Value1.ShouldNotBeNull();
        }

        [Test]
        public void should_throw_value_parse_exception_when_simple_type_is_empty_and_not_set_to_use_default()
        {
            const string json = @"{ ""IntegerValue"": """" }";
            Assert.Throws<ValueParseException>(() => Bender.Deserializer.Create().DeserializeJson<ComplexType>(json));
        }

        [Test]
        public void should_not_throw_value_parse_exception_when_simple_type_is_empty_and_set_to_use_default()
        {
            const string json = @"{ ""StringValue"": """" }";
            Assert.DoesNotThrow(() => Bender.Deserializer.Create(x => x.DefaultNonNullableTypesWhenEmpty()).DeserializeJson<ComplexType>(json));
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
            Assert.DoesNotThrow(() => result = Bender.Deserializer.Create(x => x.IgnoreUnmatchedNodes()).DeserializeJson<ComplexType>(json));
            result.ShouldNotBeNull();
            result.StringValue.ShouldBeNull();
        }

        [Test]
        public void should_fail_on_unmapped_elements()
        {
            const string json = "{ \"yada\": \"hai\" }";
            Assert.Throws<UnmatchedNodeException>(() => Bender.Deserializer.Create().DeserializeJson<ComplexType>(json))
                .FriendlyMessage.ShouldEqual("Unable to read json: The 'yada' field is not recognized.");
        }
    }
}
