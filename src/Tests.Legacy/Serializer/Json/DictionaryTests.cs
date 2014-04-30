using System.Collections.Generic;
using NUnit.Framework;

namespace Tests.Legacy.Serializer.Json
{
    [TestFixture]
    public class DictionaryTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class InheritedListOfComplexTypes : List<ComplexType> { }
        public class InheritedListOfSimpleTypes : List<int> { }

        // simple -> object
        // simple -> simple
        // object -> object
        // object -> simple

        [Test]
        public void should_serialize_list_of_complex_types()
        {
            //var json = Bender.Serializer.Create().SerializeJson(new List<ComplexType> {
            //    new ComplexType { Value = 1 },
            //    new ComplexType { Value = 2 }});
            //var items = json.ParseJson().JsonRootObjectArray();
            //items.Count().ShouldEqual(2);
            //items.JsonObjectItem(1).JsonNumberField("Value").Value.ShouldEqual("1");
            //items.JsonObjectItem(2).JsonNumberField("Value").Value.ShouldEqual("2"); 
        }
    }
}
