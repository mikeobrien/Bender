using System.Diagnostics;
using System.Xml.Linq;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class FormattingTests
    {
        public class Graph { public GraphNode Value1 { get; set; } }
        public class GraphNode { public string Value2 { get; set; } }

        [Test]
        public void should_serialize_graph_with_attribute_values()
        {
            var xml = Bender.Serializer.Create(x => x.ValuesIn(ValueNodeType.Attribute)).Serialize(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            Debug.WriteLine(xml);
            XDocument.Parse(xml).Element("Graph").Element("Value1").Attribute("Value2").Value.ShouldEqual("hai");
        }

        public class PrettyPrint
        {
            public string Value { get; set; }
        }

        [Test]
        public void should_pretty_format()
        {
            var xml = Bender.Serializer.Create(x => x.PrettyPrint()).Serialize(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint>\r\n  <Value>hai</Value>\r\n</PrettyPrint>");
        }

        [Test]
        public void should_not_pretty_format()
        {
            var xml = Bender.Serializer.Create().Serialize(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint><Value>hai</Value></PrettyPrint>");
        }
    }
}
