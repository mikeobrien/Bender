using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Bender.Extensions;
using Bender.Nodes.Xml;
using Flexo;
using NUnit.Framework;
using Should;

namespace Tests.Legacy
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void should_generate_xml_xpath()
        {
            var document = XDocument.Parse("<oh><hai><yada yada=\"\"/></hai></oh>");
            document.Root.XPathSelectElement("/oh").GetPath().ShouldEqual("/oh");
            document.Root.XPathSelectElement("/oh/hai/yada").GetPath().ShouldEqual("/oh/hai/yada");
            document.Root.XPathSelectElement("/oh/hai/yada").Attribute("yada").GetPath().ShouldEqual("/oh/hai/yada/@yada");
        }

        [Test]
        public void should_generate_json_path()
        {
            const string json = "{ \"oh\": { \"hai\": 55, \"blarg\": [ 1, 2, 3 ], \"yada\": [ { \"blarg\": 33 }, { \"blarg\": 44 } ] } }";
            var document = JElement.Load(json);

            document["oh"].Path.ShouldEqual("$.oh");
            document["oh"]["hai"].Path.ShouldEqual("$.oh.hai");
            document["oh"]["yada"].Skip(1).First()["blarg"].Path.ShouldEqual("$.oh.yada[2].blarg");
            document["oh"]["blarg"].Skip(1).First().Path.ShouldEqual("$.oh.blarg[2]");
        }
    }
}
