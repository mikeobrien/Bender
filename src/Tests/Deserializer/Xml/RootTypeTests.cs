using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Extensions;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class RootTypeTests
    {
        // Xml Object

        public class EmptyType { }

        [Test]
        public void should_deserialize_object_to_object()
        {
            var result = Deserialize.Xml<EmptyType>("<EmptyType></EmptyType>");
            result.ShouldNotBeNull();
            result.ShouldBeType<EmptyType>();
        }

        [Test]
        [TestCase(typeof(IDictionary), typeof(Dictionary<object, object>))]
        [TestCase(typeof(Hashtable), typeof(Hashtable))]
        public void should_deserialize_object_to_non_generic_dictionary(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Xml("<DictionaryOfAnyType></DictionaryOfAnyType>", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(IDictionary<string, int>), typeof(Dictionary<string, int>))]
        [TestCase(typeof(Dictionary<string, int>), typeof(Dictionary<string, int>))]
        public void should_deserialize_object_to_generic_dictionary(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Xml("<DictionaryOfInt32></DictionaryOfInt32>", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(IList), typeof(List<object>))]
        [TestCase(typeof(ArrayList), typeof(ArrayList))]
        [TestCase(typeof(IEnumerable), typeof(List<object>))]
        public void should_deserialize_object_as_non_generic_collection(Type specifiedType, Type resultingType)
        {
            var result = Deserialize.Xml("<ArrayOfAnyType></ArrayOfAnyType>", specifiedType);
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
            var result = Deserialize.Xml("<ArrayOfInt32></ArrayOfInt32>", specifiedType);
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        [Test]
        [TestCase(typeof(string), "String")]
        [TestCase(typeof(Guid), "Guid")]
        [TestCase(typeof(DateTime), "DateTime")]
        [TestCase(typeof(Uri), "Uri")]
        public void should_fail_to_deserialize_object_as_simple_type(Type type, string name)
        {
            Assert.Throws<TypeNotSupportedException>(() => 
                Deserialize.Xml("<{0}></{0}>".ToFormat(name), type))
                .Message.ShouldEqual(("Simple type '{0}' is not supported for deserialization. " +
                    "Only complex types can be deserialized.").ToFormat(type.GetFriendlyTypeFullName()));
        }
    }
}
