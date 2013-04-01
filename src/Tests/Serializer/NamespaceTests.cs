using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class NamespaceTests
    {
        public class ComplexType { public int Value { get; set; } }

        [Test]
        public void should_apply_default_namespace_with_element()
        {
            var xml = Bender.Serializer.Create(x => x.WithDefaultNamespace("http://namespace.org")).Serialize(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType xmlns=\"http://namespace.org\"><Value>5</Value></ComplexType>");
        }

        [Test]
        public void should_apply_default_namespace_with_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.WithDefaultNamespace("http://namespace.org").ValuesAsAttributes())
                .Serialize(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType Value=\"5\" xmlns=\"http://namespace.org\" />");
        }

        [Test]
        public void should_apply_default_namespace_to_element()
        {
            var xml = Bender.Serializer.Create(x => x.AddNamespace("abc", "http://abc.org")
                .AddWriter((o, p, v, e) => e.Name == "Value", (o, p, v, e) => e.Name = o.Namespaces["abc"] + e.Name.LocalName))
                .Serialize(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType xmlns:abc=\"http://abc.org\"><abc:Value>5</abc:Value></ComplexType>");
        }

        [Test]
        public void should_apply_default_namespace_to_attribute()
        {
            var xml = Bender.Serializer.Create(x => x.AddNamespace("abc", "http://abc.org").ValuesAsAttributes()
                .AddWriter((o, p, v, e) => e.Name == "Value", (o, p, v, e) => e.Name = o.Namespaces["abc"] + e.Name.LocalName))
                .Serialize(new ComplexType { Value = 5 });
            xml.ShouldEqual("<ComplexType abc:Value=\"5\" xmlns:abc=\"http://abc.org\" />");
        }
    }
}
