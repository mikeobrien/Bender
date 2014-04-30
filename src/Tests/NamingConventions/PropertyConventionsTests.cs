using System;
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
    public class PropertyConventionsTests
    {
        public class Model
        {
            [XmlElement("SomeElement")]
            public string Property { get; set; }
        }

        private static readonly Options Options = Options.Create();
        private static readonly CachedMember Property = 
            new CachedMember(typeof(Model).GetProperty("Property"));

        private static string GetPropertyName(Action<NamingConventions<MemberContext>> config)
        {
            var convention = MemberNamingConventions.Create();
            config(convention);
            return convention.GetName(new MemberContext(Property, new Context(Options, Mode.Deserialize, "xml")));
        }

        // Property configuration

        [Test]
        public void should_add_property_name_convention()
        {
            GetPropertyName(x => x.Add(
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Member.ShouldEqual(Property);
                    c.Options.ShouldEqual(Options);
                    return "yada";
                })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_property_name_convention()
        {
            GetPropertyName(x => x.Add(
                (p, c) =>
                {
                    c.Member.ShouldEqual(Property);
                    c.Options.ShouldEqual(Options);
                    return "yada";
                },
                (p, c) =>
                {
                    c.Member.ShouldEqual(Property);
                    c.Options.ShouldEqual(Options);
                    return true;
                })).ShouldEqual("yada");

            GetPropertyName(x => x.Add((MemberContext c) => "yada",
                c => false)).ShouldEqual("SomeElement");
        }

        [Test]
        public void should_add_conditional_property_name_modification_convention()
        {
            GetPropertyName(x => x.Add(
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Member.ShouldEqual(Property);
                    c.Options.ShouldEqual(Options);
                    return "yada";
                },
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Member.ShouldEqual(Property);
                    c.Options.ShouldEqual(Options);
                    return true;
                })).ShouldEqual("yada");

            GetPropertyName(x => x.Add((n, c) => "yada",
                (n, c) => false)).ShouldEqual("SomeElement");
        }

        [Test]
        public void should_add_generic_property_name_convention()
        {
            GetPropertyName(x => x.Add(
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Options.ShouldEqual(Options);
                    return "yada";
                })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_generic_property_convention()
        {
            GetPropertyName(x => x.Add(
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Options.ShouldEqual(Options);
                    return "yada";
                },
                (n, c) =>
                {
                    n.ShouldEqual("SomeElement");
                    c.Options.ShouldEqual(Options);
                    return true;
                })).ShouldEqual("yada");

            GetPropertyName(x => x.Add((n, c) => "yada",
                (n, c) => false)).ShouldEqual("SomeElement");
        }
    }
}
