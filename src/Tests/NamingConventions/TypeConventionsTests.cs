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
    public class TypeConventionsTests
    {
        [XmlType("SomeModel")]
        public class Model { }

        private static readonly Options Options = Options.Create();
        private static readonly Type ModelType = typeof(Model);

        private static string GetTypeName(Action<NamingConventions<TypeContext>> config)
        {
            var convention = TypeNamingConvention.Create();
            config(convention);
            return convention.GetName(new TypeContext(ModelType.GetCachedType(), new Context(Options, Mode.Deserialize, "xml"), true));
        }

        [Test]
        public void should_add_type_name_convention()
        {
            GetTypeName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Type.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                c.IsRoot.ShouldBeTrue();
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_type_name_convention()
        {
            GetTypeName(x => x.Add(
            (t, c) => {
                c.Type.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                c.IsRoot.ShouldBeTrue();
                return "yada";
            },
            (t, c) => {
                c.Type.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                c.IsRoot.ShouldBeTrue();
                return true;
            })).ShouldEqual("yada");

            GetTypeName(x => x.Add((t, c) => "yada", 
                (t, c) => false)).ShouldEqual("SomeModel");
        }

        [Test]
        public void should_add_conditional_type_name_modification_convention()
        {
            GetTypeName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Type.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                c.IsRoot.ShouldBeTrue();
                return "yada";
            }, 
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Type.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                c.IsRoot.ShouldBeTrue();
                return true;
            })).ShouldEqual("yada");

            GetTypeName(x => x.Add((n, c) => "yada", 
                (n, c) => false)).ShouldEqual("SomeModel");
        }

        [Test]
        public void should_add_generic_type_name_convention()
        {
            GetTypeName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Options.ShouldEqual(Options);
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        public void should_add_conditional_generic_type_convention()
        {
            GetTypeName(x => x.Add(
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Options.ShouldEqual(Options);
                return "yada";
            }, 
            (n, c) => {
                n.ShouldEqual("SomeModel");
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetTypeName(x => x.Add((n, c) => "yada", 
                (n, c) => false)).ShouldEqual("SomeModel");
        }
    }
}
