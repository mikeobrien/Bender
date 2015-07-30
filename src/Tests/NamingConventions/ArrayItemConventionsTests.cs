using System;
using System.Collections.Generic;
using System.Reflection;
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
    public class ArrayItemConventionsTests
    {
        public class Model
        {
            [XmlArrayItem("SomeItem")]
            public List<string> Field;

            [XmlArrayItem("SomeItem")]
            public List<string> Property { get; set; }
        }

        private static readonly Options Options = Options.Create();
        private static readonly CachedType ModelType = typeof(Model).ToCachedType();
        private static readonly CachedMember Field = new CachedMember(ModelType.Type.GetField("Field"));
        private static readonly CachedMember Property = new CachedMember(ModelType.Type.GetProperty("Property"));

        private static string GetName(CachedMember member, Action<NamingConventions<ArrayItemContext>> config)
        {
            var convention = ArrayItemConventions.Create();
            config(convention);
            return convention.GetName(new ArrayItemContext(ModelType, member, 
                new Context(Options, Mode.Deserialize, "xml")));
        }

        private static readonly object[] ArrayItemCases =
            TestCases.Create()
            .Add(Property, true, "SomeItem")
            .Add(Field, true, "SomeItem")
            .Add((MemberInfo)null, false, null)
            .All;

        [Test]
        [TestCaseSource("ArrayItemCases")]
        public void should_add_field_name_convention(
            CachedMember member, bool hasMember, string itemName)
        {
            GetName(member, x => x.Add(
            (n, c) => {
                n.ShouldEqual(itemName);
                c.HasMember.ShouldEqual(hasMember);
                c.Member.ShouldEqual(member);
                c.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        [TestCaseSource("ArrayItemCases")]
        public void should_add_conditional_field_name_modification_convention(
            CachedMember member, bool hasMember, string itemName)
        {
            GetName(member, x => x.Add(
            (n, c) => {
                n.ShouldEqual(itemName);
                c.HasMember.ShouldEqual(hasMember);
                c.Member.ShouldEqual(member);
                c.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                return "yada";
            },
            (n, c) => {
                n.ShouldEqual(itemName);
                c.HasMember.ShouldEqual(hasMember);
                c.Member.ShouldEqual(member);
                c.Type.ShouldEqual(ModelType);
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetName(member, x => x.Add((n, p) => "yada", 
                (n, p) => false)).ShouldEqual(itemName);
        }

        [Test]
        [TestCaseSource("ArrayItemCases")]
        public void should_add_generic_field_name_convention(
            CachedMember member, bool hasMember, string itemName)
        {
            GetName(member, x => x.Add(
            (n, c) => {
                n.ShouldEqual(itemName);
                c.Options.ShouldEqual(Options);
                return "yada";
            })).ShouldEqual("yada");
        }

        [Test]
        [TestCaseSource("ArrayItemCases")]
        public void should_add_conditional_generic_field_convention(
            CachedMember member, bool hasMember, string itemName)
        {
            GetName(member, x => x.Add(
            (n, c) => {
                n.ShouldEqual(itemName);
                c.Options.ShouldEqual(Options);
                return "yada";
            },
            (n, c) => {
                n.ShouldEqual(itemName);
                c.Options.ShouldEqual(Options);
                return true;
            })).ShouldEqual("yada");

            GetName(member, x => x.Add((string n) => "yada", 
                n => false)).ShouldEqual(itemName);
        }
    }
}
