using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Xml
{
    [TestFixture]
    public class VisitorTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public Child Child { get; set; }
        }

        public class Child
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
        }

        public class NullableModel
        {
            public int NonNullable { get; set; }
            public int? Nullable { get; set; }
        }

        private readonly static Model ModelInstance = new Model { Property1 = "oh", Property2 = 5, 
                Child = new Child { Property1 = "oh", Property2 = 6 } };

        private const string ModelJson = "{\"Property1\":\"oh\",\"Property2\":5," +
                                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":6}}";

        [Test]
        public void should_visit_all_nodes()
        {
            Serialize.Xml(ModelInstance,
                x => x.Serialization(y => y.AddVisitor(
                    (s, t, o) =>
                    {
                        if (s.Name == "Property2") t.Value = "orly";
                        if (s.ActualType.Is<int>()) t.Value += "!";
                    })))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly!</Property2>" + 
                    "<Child><Property1>oh</Property1><Property2>orly!</Property2></Child></Model>");
        }

        [Test]
        public void should_visit_all_nodes_when()
        {
            Serialize.Xml(ModelInstance,
                x => x.Serialization(y => y.AddVisitor(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Name == "Property2")))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");
        }

        [Test]
        public void should_visit_all_json_nodes()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddXmlVisitor(
                    (s, t, o) =>
                    {
                        if (s.Name == "Property2") t.Value = "orly";
                        if (s.ActualType.Is<int>()) t.Value += "!";
                    })));

            Serialize.Xml(ModelInstance, options)
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly!</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly!</Property2></Child></Model>");

            Serialize.Json(ModelInstance, options).ShouldEqual(ModelJson);
        }

        [Test]
        public void should_visit_all_json_nodes_when()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddXmlVisitor(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Name == "Property2")));

            Serialize.Xml(ModelInstance, options)
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");

            Serialize.Json(ModelInstance, options).ShouldEqual(ModelJson);
        }

        [Test]
        public void should_visit_all_nodes_of_type()
        {
            Serialize.Xml(ModelInstance,
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = "orly")))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");
        }

        [Test]
        public void should_visit_nullable_and_non_nullable_nodes_of_type()
        {
            Serialize.Xml(new NullableModel { NonNullable = 1, Nullable = 1 },
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2)))
                .ShouldEqual(Xml.Declaration + "<NullableModel><NonNullable>2</NonNullable><Nullable>2</Nullable></NullableModel>");
        }

        [Test]
        public void should_visit_all_json_nodes_of_type()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddXmlVisitor<int>(
                    (s, t, o) => t.Value = "orly")));

            Serialize.Xml(ModelInstance, options)
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>orly</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");

            Serialize.Json(ModelInstance, options).ShouldEqual(ModelJson);
        }

        [Test]
        public void should_visit_all_nodes_of_type_when()
        {
            Serialize.Xml(ModelInstance,
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Member.DeclaringType.Type.Is<Child>())))
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>5</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_nullable_and_non_nullable_nodes_of_type_when(int match, int nonNullableCount)
        {
            Serialize.Xml(new NullableModel { Nullable = 1, NonNullable = 2 },
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 3,
                    (s, t, o) => (int)s.Value == match)))
                .ShouldEqual(Xml.Declaration + ("<NullableModel><NonNullable>{0}</NonNullable>" + 
                "<Nullable>{1}</Nullable></NullableModel>").ToFormat(match == 2 ? 3 : 2, match == 1 ? 3 : 1));
        }

        [Test]
        public void should_visit_all_json_nodes_of_type_when()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddXmlVisitor<int>(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Member.DeclaringType.Type.Is<Child>())));

            Serialize.Xml(ModelInstance, options)
                .ShouldEqual(Xml.Declaration + "<Model><Property1>oh</Property1><Property2>5</Property2>" +
                    "<Child><Property1>oh</Property1><Property2>orly</Property2></Child></Model>");

            Serialize.Json(ModelInstance, options).ShouldEqual(ModelJson);
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_nullable_and_non_nullable_json_nodes_of_type_when(int match, int nonNullableCount)
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddXmlVisitor<int>(
                    (s, t, o) => t.Value = 3,
                    (s, t, o) => (int)s.Value == match)));

            var model = new NullableModel { Nullable = 1, NonNullable = 2 };

            Serialize.Xml(model, options)
                .ShouldEqual(Xml.Declaration + ("<NullableModel><NonNullable>{0}</NonNullable>" + 
                "<Nullable>{1}</Nullable></NullableModel>").ToFormat(match == 2 ? 3 : 2, match == 1 ? 3 : 1));

            Serialize.Json(model, options)
                .ShouldEqual("{\"NonNullable\":2,\"Nullable\":1}");
        }
    }
}
