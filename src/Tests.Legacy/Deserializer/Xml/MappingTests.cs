using System;
using System.Collections.Generic;
using Bender.Nodes;
using Bender.Nodes.Object;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Xml
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
            const string xml = @"<graph><value1><value2>hai</value2></value1></graph>";
            Bender.Deserializer.Create(x => x.Deserialization(y => y.IgnoreNameCase())).DeserializeXml<Graph>(xml).Value1.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_not_map_elements_and_values_case_insensitively()
        {
            const string xml = @"<graph><value1><value2>hai</value2></value1></graph>";
            Assert.Throws<InvalidRootNameDeserializationException>(() => Bender.Deserializer.Create().DeserializeXml<Graph>(xml))
                .FriendlyMessage.ShouldEqual("Xml root name 'graph' does not match expected name of 'Graph'.");
        }

        // Missing elements

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public TraversalTests.GraphNode Value2 { get; set; }
            public int Value3 { get; set; }
        }

        [Test]
        public void should_deserialize_empty_complex_type_element_with_missing_child_elements()
        {
            const string xml = @"<Graph><Value1></Value1></Graph>";
            Bender.Deserializer.Create().DeserializeXml<Graph>(xml).Value1.ShouldNotBeNull();
        }

        [Test]
        public void should_set_type_with_missing_element_to_null()
        {
            Bender.Deserializer.Create().DeserializeXml<NullValue>("<NullValue></NullValue>").Value2.ShouldBeNull();
        }

        [Test]
        public void should_set_value_with_missing_element_to_default()
        {
            Bender.Deserializer.Create().DeserializeXml<NullValue>("<NullValue></NullValue>").Value3.ShouldEqual(0);
        }

        [Test]
        public void should_set_nullable_value_with_missing_element_to_null()
        {
            Bender.Deserializer.Create().DeserializeXml<NullValue>("<NullValue></NullValue>").Value1.ShouldBeNull();
        }

        // Unmapped elements

        [Test]
        public void should_not_fail_on_unmapped_elements()
        {
            const string xml = "<ComplexType blarg=\"hai\"><yada>hai</yada></ComplexType>";
            ComplexType result = null;
            Assert.DoesNotThrow(() => result = Bender.Deserializer.Create().DeserializeXml<ComplexType>(xml));
            result.ShouldNotBeNull();
            result.StringValue.ShouldBeNull();
        }

        [Test]
        public void should_fail_on_unmapped_elements()
        {
            const string xml = @"<ComplexType><yada>hai</yada></ComplexType>";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => 
                x.Deserialization(y => y.FailOnUnmatchedElements())).DeserializeXml<ComplexType>(xml))
                .InnerException.ShouldBeType<UnrecognizedNodeDeserializationException>();
        }

        [Test]
        public void should_not_fail_on_unmapped_attributes()
        {
            const string xml = "<ComplexType blarg=\"hai\"></ComplexType>";
            ComplexType result = null;
            Assert.DoesNotThrow(() => result = Bender.Deserializer.Create().DeserializeXml<ComplexType>(xml));
            result.ShouldNotBeNull();
            result.StringValue.ShouldBeNull();
        }

        [Test]
        public void should_fail_on_unmapped_attributes()
        {
            const string xml = "<ComplexType blarg=\"hai\"></ComplexType>";
            Assert.Throws<FriendlyMappingException>(() => Bender.Deserializer.Create(x => 
                x.Deserialization(y => y.FailOnUnmatchedElements())).DeserializeXml<ComplexType>(xml))
                .InnerException.ShouldBeType<UnrecognizedNodeDeserializationException>();
        }

        [Test]
        public void should_not_fail_on_unmapped_root_element()
        {
            const string xml = @"<root><StringValue>5</StringValue></root>";
            ComplexType result = null;
            Assert.DoesNotThrow(() => result = Bender.Deserializer.Create(x => x.Deserialization(y => y.IgnoreRootName())).DeserializeXml<ComplexType>(xml));
            result.ShouldNotBeNull();
            result.StringValue.ShouldEqual("5");
        }

        [Test]
        public void should_fail_on_unmapped_root_element()
        {
            const string xml = @"<root><Value>hai</Value></root>";
            Assert.Throws<InvalidRootNameDeserializationException>(() => Bender.Deserializer.Create().DeserializeXml<ComplexType>(xml))
                .FriendlyMessage.ShouldEqual("Xml root name 'root' does not match expected name of 'ComplexType'.");
        }

        [Test]
        public void should_not_fail_on_unmapped_list_element()
        {
            const string xml = @"<ArrayOfComplexType><yada><StringValue>5</StringValue></yada></ArrayOfComplexType>";
            List<ComplexType> results = null;
            Assert.DoesNotThrow(() => results = Bender.Deserializer.Create(x => x.Deserialization(y => y.IgnoreArrayItemNames())).DeserializeXml<List<ComplexType>>(xml));
            results.ShouldNotBeNull();
            results.Count.ShouldEqual(1);
            results[0].StringValue.ShouldEqual("5");
        }

        [Test]
        public void should_fail_on_unmapped_list_element()
        {
            const string xml = @"<ArrayOfComplexType><yada><Value>hai</Value></yada></ArrayOfComplexType>";
            var exception = Assert.Throws<FriendlyMappingException>(() => 
                Bender.Deserializer.Create().DeserializeXml<List<ComplexType>>(xml));

            exception.Message.ShouldEqual("Error deserializing xml element '/ArrayOfComplexType/yada' to " +
                "'System.Collections.Generic.List<Tests.Legacy.Deserializer.Xml.MappingTests.ComplexType>': " +
                "Name 'yada' does not match expected name of 'ComplexType'.");

            exception.FriendlyMessage.ShouldEqual("Could not read xml element '/ArrayOfComplexType/yada': " +
                                                  "Name 'yada' does not match expected name of 'ComplexType'.");

            exception.InnerException.ShouldBeType<InvalidItemNameDeserializationException>();
        }
    }
}
