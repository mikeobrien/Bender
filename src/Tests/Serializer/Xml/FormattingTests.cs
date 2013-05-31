using System;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
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
            var xml = Bender.Serializer.Create(x => x.PrettyPrintXml()).SerializeXml(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint>" + Environment.NewLine + "<Value>hai</Value>" + Environment.NewLine + "</PrettyPrint>");
        }

        [Test]
        public void should_not_pretty_format()
        {
            var xml = Bender.Serializer.Create().SerializeXml(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint><Value>hai</Value></PrettyPrint>");
        }
    }
}
