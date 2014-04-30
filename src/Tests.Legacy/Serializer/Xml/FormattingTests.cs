using System;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Xml
{
    [TestFixture]
    public class FormattingTests
    {
        public class PrettyPrint
        {
            public string Value { get; set; }
        }

        [Test]
        public void should_pretty_format()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.PrettyPrint())).SerializeXml(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<PrettyPrint>" + 
                Environment.NewLine + "  <Value>hai</Value>" + Environment.NewLine + "</PrettyPrint>");
        }

        [Test]
        public void should_not_pretty_format()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><PrettyPrint><Value>hai</Value></PrettyPrint>");
        }
    }
}
