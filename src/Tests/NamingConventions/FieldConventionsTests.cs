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
    public class FieldConventionsTests
    {
        public class Model
        {
            [XmlElement("SomeElement")]
            public string Field;
        }

        private static readonly Options Options = Options.Create();
        private static readonly CachedMember Field = new CachedMember(typeof(Model).GetField("Field"));

        private static string GetFieldName(Action<NamingConventions<MemberContext>> config)
        {
            var convention = MemberNamingConventions.Create();
            config(convention);
            return convention.GetName(new MemberContext(Field, new Context(Options, Mode.Deserialize, "xml")));
        }

        [Test]
        public void should_add_field_name_convention()
        {
            GetFieldName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Member.ShouldEqual(Field);
                c.Options.ShouldEqual(Options);
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_field_name_convention()
        {
            GetFieldName(x => x.Add(
            c => {
                c.Member.ShouldEqual(Field);
                c.Options.ShouldEqual(Options);
                return "yada";
            },
            c => {
                c.Member.ShouldEqual(Field);
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetFieldName(x => x.Add((MemberContext c) => "yada", 
                c => false)).ShouldEqual("SomeElement");
        }

        [Test]
        public void should_add_conditional_field_name_modification_convention()
        {
            GetFieldName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Member.ShouldEqual(Field);
                c.Options.ShouldEqual(Options);
                return "yada";
            },
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Member.ShouldEqual(Field);
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetFieldName(x => x.Add((n, c) => "yada", 
                (n, c) => false)).ShouldEqual("SomeElement");
        }

        [Test]
        public void should_add_generic_field_name_convention()
        {
            GetFieldName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Options.ShouldEqual(Options);
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_generic_field_convention()
        {
            GetFieldName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Options.ShouldEqual(Options);
                return "yada";
            },
            (n, c) => {
                n.ShouldEqual("SomeElement");
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetFieldName(x => x.Add((n, c) => "yada", 
                (n, c) => false)).ShouldEqual("SomeElement");
        }
    }
}
