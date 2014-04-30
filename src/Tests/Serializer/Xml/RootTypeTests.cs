using System;
using System.Collections;
using System.Collections.Generic;
using Bender;
using Bender.Extensions;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class RootTypeTests
    {
        [Test]
        public void should_fail_as_serialize_simple_type()
        {
            Assert.Throws<TypeNotSupportedException>(() => Serialize.Xml("hai"))
                .Message.ShouldEqual("Simple type 'System.String' is not supported " +
                                     "for serialization. Only complex types can be serialized.");
        }

        public class EmptyType { }

        [Test]
        [TestCase(typeof(EmptyType), "EmptyType")]
        [TestCase(typeof(ArrayList), "ArrayOfAnyType")]
        [TestCase(typeof(List<object>), "ArrayOfObject")]
        [TestCase(typeof(GenericStringListImpl), "ArrayOfString")]
        [TestCase(typeof(Hashtable), "DictionaryOfAnyType")]
        [TestCase(typeof(Dictionary<string, int>), "DictionaryOfInt32")]
        public void should_serialize_object(Type type, string name)
        {
            Serialize.Xml(type.CreateInstance()).ShouldEqual(Xml.Declaration + "<{0} />".ToFormat(name));
        }

        [Test]
        public void should_serialize_array()
        {
            Serialize.Xml(new int[] { }).ShouldEqual(Xml.Declaration + "<ArrayOfInt32 />");
        }
    }
}
