using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class RootTypeTests
    {
        // Json Object

        public class EmptyType { }

        [Test]
        public void should_deserialize_object_to_object()
        {
            var result = Deserialize.Json<EmptyType>("{}");
            result.ShouldNotBeNull();
            result.ShouldBeType<EmptyType>();
        }

        [Test]
        [TestCase(typeof(IDictionary), typeof(Dictionary<object, object>))]
        [TestCase(typeof(Hashtable), typeof(Hashtable))]
        public void should_deserialize_object_to_non_generic_dictionary(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("{}", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(IDictionary<string, int>), typeof(Dictionary<string, int>))]
        [TestCase(typeof(Dictionary<string, int>), typeof(Dictionary<string, int>))]
        public void should_deserialize_object_to_generic_dictionary(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("{}", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(IList), typeof(List<object>))]
        [TestCase(typeof(ArrayList), typeof(ArrayList))]
        [TestCase(typeof(IEnumerable), typeof(List<object>))]
        public void should_deserialize_object_as_non_generic_collection(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("{}", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(int[]), typeof(int[]))]
        [TestCase(typeof(IList<int>), typeof(List<int>))]
        [TestCase(typeof(List<int>), typeof(List<int>))]
        [TestCase(typeof(IEnumerable<int>), typeof(List<int>))]
        public void should_deserialize_object_as_generic_collection_or_array(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("{}", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(Uri))]
        public void should_fail_to_deserialize_object_as_simple_type(Type type)
        {
            Assert.Throws<TypeNotSupportedException>(() => Deserialize.Json("{}", type))
                .Message.ShouldEqual(("Simple type '{0}' is not supported for deserialization. " +
                    "Only complex types can be deserialized.").ToFormat(type.GetFriendlyTypeFullName()));
        }

        // Json Array

        [Test]
        [TestCase(typeof(IList), typeof(List<object>))]
        [TestCase(typeof(ArrayList), typeof(ArrayList))]
        [TestCase(typeof(IEnumerable), typeof(List<object>))]
        public void should_deserialize_array_as_non_generic_collection(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("[]", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(int[]), typeof(int[]))]
        [TestCase(typeof(IList<int>), typeof(List<int>))]
        [TestCase(typeof(List<int>), typeof(List<int>))]
        [TestCase(typeof(IEnumerable<int>), typeof(List<int>))]
        public void should_deserialize_array_as_generic_collection_or_array(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Json("[]", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(IDictionary<string, int>))]
        [TestCase(typeof(Dictionary<string, int>))]
        [TestCase(typeof(EmptyType))]
        public void should_fail_to_deserialize_array_to_object_type(Type type)
        {
            var exception = Assert.Throws<FriendlyMappingException>(() => Deserialize.Json("[]", type));
            exception.InnerException.ShouldBeType<NodeTypeMismatchException>();
            exception.Message.ShouldEqual(("Error deserializing json array '$' to '{0}': Cannot map an array " +
                                          "node to an object node.").ToFormat(type.GetFriendlyTypeFullName()));
            exception.FriendlyMessage.ShouldEqual("Could not read json array '$': Should be an object but was an array.");
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(Uri))]
        public void should_fail_to_deserialize_array_as_simple_type(Type type)
        {
            Assert.Throws<TypeNotSupportedException>(() => Deserialize.Json("[]", type))
                .Message.ShouldEqual(("Simple type '{0}' is not supported for deserialization. " +
                    "Only complex types can be deserialized.").ToFormat(type.GetFriendlyTypeFullName()));
        }
    }
}
