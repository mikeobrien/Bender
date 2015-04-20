using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class RootTypeTests
    {
        [Test]
        public void should_fail_as_serialize_simple_type()
        {
            Assert.Throws<TypeNotSupportedException>(() => Serialize.Json("hai"))
                .Message.ShouldEqual("Simple type 'System.String' is not supported " +
                                     "for serialization. Only complex types can be serialized.");
        }

        // Json Object

        public class EmptyType { }

        [Test]
        public void should_serialize_object_as_object()
        {
            Serialize.Json(new EmptyType()).ShouldEqual("{}");
        }

        [Test]
        [TestCase(typeof(Hashtable))]
        [TestCase(typeof(Dictionary<string, int>))]
        public void should_serialize_dictionary_as_object(Type type)
        {
            Serialize.Json(type.CreateInstance()).ShouldEqual("{}");
        }

        [Test]
        public void should_serialize_collection_as_object_when_configured()
        {
            Serialize.Json(new GenericStringListImpl(), x => x.TreatEnumerableImplsAsObjects()).ShouldEqual("{}");
        }

        // Json Array

        [Test]
        public void should_serialize_array_as_array()
        {
            Serialize.Json(new int[] {}).ShouldEqual("[]");
        }

        [Test]
        [TestCase(typeof(ArrayList))]
        [TestCase(typeof(List<object>))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_serialize_collection_as_array(Type type)
        {
            Serialize.Json(type.CreateInstance()).ShouldEqual("[]");
        }
    }
}
