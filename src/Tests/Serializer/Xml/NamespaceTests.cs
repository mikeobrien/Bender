using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class NamespaceTests
    {
        public class ComplexType { public int Value { get; set; } }

        [Test]
        public void should_apply_default_namespace_with_element()
        {
            var xml = Bender.Serializer.Create(x => x.WithDefaultXmlNamespace("http://namespace.org")).SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType xmlns=\"http://namespace.org\"><Value>5</Value></ComplexType>");
        }

        [Test]
        public void should_apply_default_namespace_with_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.WithDefaultXmlNamespace("http://namespace.org").XmlValuesAsAttributes())
                .SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType Value=\"5\" xmlns=\"http://namespace.org\" />");
        }

        [Test]
        public void should_apply_default_namespace_to_element()
        {
            var xml = Bender.Serializer.Create(x => x.AddXmlNamespace("abc", "http://abc.org")
                .AddWriter(y => y.Node.Name == "Value", y => y.Node.Name = y.Options.XmlNamespaces["abc"] + y.Node.Name.LocalName))
                .SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType xmlns:abc=\"http://abc.org\"><abc:Value>5</abc:Value></ComplexType>");
        }

        [Test]
        public void should_apply_default_namespace_to_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.AddXmlNamespace("abc", "http://abc.org").XmlValuesAsAttributes()
                .AddWriter(y => y.Node.Name == "Value", y => y.Node.Name = y.Options.XmlNamespaces["abc"] + y.Node.Name.LocalName))
                .SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType abc:Value=\"5\" xmlns:abc=\"http://abc.org\" />");
        }
    }
}
