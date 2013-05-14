using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;
using System.Xml.XPath;
using Bender;
using Should;

namespace Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void should_generate_xml_xpath()
        {
            var document = XDocument.Parse("<oh><hai><yada yada=\"\"/></hai></oh>");
            document.Root.XPathSelectElement("/oh").GetPath(Format.Xml).ShouldEqual("/oh");
            document.Root.XPathSelectElement("/oh/hai/yada").GetPath(Format.Xml).ShouldEqual("/oh/hai/yada");
            document.Root.XPathSelectElement("/oh/hai/yada").Attribute("yada").GetPath(Format.Xml).ShouldEqual("/oh/hai/yada/@yada");
        }

        [Test]
        public void should_generate_json_path()
        {
            const string json = "{ \"oh\": { \"hai\": 55, \"blarg\": [ 1, 2, 3 ], \"yada\": [ { \"blarg\": 33 }, { \"blarg\": 44 } ] } }";
            var document = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(
                new MemoryStream(Encoding.UTF8.GetBytes(json)), new XmlDictionaryReaderQuotas()));

            document.Root.XPathSelectElement("/root/oh").GetPath(Format.Json).ShouldEqual("oh");
            document.Root.XPathSelectElement("/root/oh/hai").GetPath(Format.Json).ShouldEqual("oh.hai");
            document.Root.XPathSelectElement("/root/oh/yada/item[2]/blarg").GetPath(Format.Json).ShouldEqual("oh.yada[2].blarg");
            document.Root.XPathSelectElement("/root/oh/blarg/item[2]").GetPath(Format.Json).ShouldEqual("oh.blarg[2]");
        }
    }
}
