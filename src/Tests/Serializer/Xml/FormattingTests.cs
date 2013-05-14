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
