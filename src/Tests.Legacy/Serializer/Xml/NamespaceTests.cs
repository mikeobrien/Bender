using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Xml
{
    [TestFixture]
    public class NamespaceTests
    {
        public class ComplexType { public int Value { get; set; } }

        [Test]
        public void should_apply_default_namespace_with_element()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.WithDefaultXmlNamespace("http://namespace.org"))).SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><ComplexType xmlns=\"http://namespace.org\"><Value>5</Value></ComplexType>");
        }

        [Test]
        public void should_apply_default_namespace_with_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.WithDefaultXmlNamespace("http://namespace.org").XmlValuesAsAttributes()))
                .SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><ComplexType Value=\"5\" xmlns=\"http://namespace.org\" />");
        }

        [Test]
        public void should_apply_namespace_to_element()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddXmlNamespace("abc", "http://abc.org")
                .AddXmlVisitor((s, t, o) => t.SetNamespacePrefix("abc"), (s, t, o) => t.Name == "Value")))
                .SerializeXml(new ComplexType { Value = 5 });
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><ComplexType xmlns:abc=\"http://abc.org\"><abc:Value>5</abc:Value></ComplexType>");
        }

        [Test]
        public void should_apply_namespace_to_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.Serialization(y => y.AddXmlNamespace("abc", "http://abc.org").XmlValuesAsAttributes()
                .AddXmlVisitor((s, t, o) => t.SetNamespacePrefix("abc"), (s, t, o) => t.Name == "Value")))
                .SerializeXml(new ComplexType { Value = 5 });
            
            #if __MonoCS__
            // This looks right locally but not on travis-ci. Different version of mono maybe?
            xml.ShouldEqual("<ComplexType d1p1:Value=\"5\" xmlns:abc=\"http://abc.org\" xmlns:d1p1=\"http://abc.org\" />");
            #else 
            xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><ComplexType xmlns:abc=\"http://abc.org\" abc:Value=\"5\" />");
            #endif
        }
    }
}
