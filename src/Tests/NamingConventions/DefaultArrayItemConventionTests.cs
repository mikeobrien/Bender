using System.Xml.Serialization;
using Bender.Configuration;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.NamingConventions
{
    [TestFixture]
    public class DefaultArrayItemConventionTests
    {
        private string GetName<T>(string member, Options options = null)
        {
            return ArrayItemConventions.Create()
                .GetName(new ArrayItemContext(typeof(T).GetCachedType(), new CachedMember(typeof(T).GetMember(member)[0]), 
                    new Context(options ?? Options.Create(), Mode.Deserialize, "xml")));
        }

        public class ArrayItemNameConvention
        {
            [XmlArrayItem]
            public string[] NoNameArrayItemProperty { get; set; }
            [XmlArrayItem("SomeItem")]
            public string[] ArrayItemProperty { get; set; }
            [XmlArrayItem("SomeItem")]
            public string NotAnArrayProperty { get; set; }
        }

        [Test]
        public void should_return_null_if_name_not_set_in_xml_array()
        {
            GetName<ArrayItemNameConvention>("NoNameArrayItemProperty")
                .ShouldBeNull();
        }

        [Test]
        public void should_use_xml_array_item_name()
        {
            GetName<ArrayItemNameConvention>("ArrayItemProperty")
                .ShouldEqual("SomeItem");
        }

        [Test]
        public void should_return_null_if_xml_array_not_on_an_enumerable_property()
        {
            GetName<ArrayItemNameConvention>("NotAnArrayProperty")
                .ShouldBeNull();
        }
    }
}
