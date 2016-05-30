using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
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

        private static readonly Model ModelInstance = new Model { Property1 = "oh", Property2 = 5, 
                Child = new Child { Property1 = "oh", Property2 = 6 } };

        private const string ModelXml = Xml.Xml.Declaration + 
            "<Model><Property1>oh</Property1><Property2>5</Property2>" +
            "<Child><Property1>oh</Property1><Property2>6</Property2></Child></Model>";

        [Test]
        public void should_visit_all_nodes()
        {
            Serialize.Json(ModelInstance,
                x => x.Serialization(y => y.AddVisitor(
                    (s, t, o) =>
                    {
                        if (s.Name == "Property2") t.Value = "orly";
                        if (s.ActualType.Is<int>()) t.Value += "!";
                    })))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly!\"," + 
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly!\"}}");
        }

        [Test]
        public void should_visit_all_nodes_when()
        {
            Serialize.Json(ModelInstance,
                x => x.Serialization(y => y.AddVisitor(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Name == "Property2")))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly\"," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");
        }

        [Test]
        public void should_visit_all_json_nodes()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddJsonVisitor(
                    (s, t, o) =>
                    {
                        if (s.Name == "Property2") t.Value = "orly";
                        if (s.ActualType.Is<int>()) t.Value += "!";
                    })));

            Serialize.Json(ModelInstance, options)
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly!\"," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly!\"}}");

            Serialize.Xml(ModelInstance, options).ShouldEqual(ModelXml);
        }

        [Test]
        public void should_visit_all_json_nodes_when()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddJsonVisitor(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Name == "Property2")));

            Serialize.Json(ModelInstance, options)
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly\"," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");

            Serialize.Xml(ModelInstance, options).ShouldEqual(ModelXml);
        }

        [Test]
        public void should_visit_all_nodes_of_type()
        {
            Serialize.Json(ModelInstance,
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = "orly")))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly\"," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");
        }

        [Test]
        public void should_visit_nullable_and_non_nullable_nodes_of_type()
        {
            Serialize.Json(new NullableModel { NonNullable = 1, Nullable = 1 },
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2)))
                .ShouldEqual("{\"NonNullable\":2,\"Nullable\":2}");
        }

        [Test]
        public void should_visit_all_json_nodes_of_type()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddJsonVisitor<int>(
                    (s, t, o) => t.Value = "orly")));

            Serialize.Json(ModelInstance, options)
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":\"orly\"," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");

            Serialize.Xml(ModelInstance, options).ShouldEqual(ModelXml);
        }

        [Test]
        public void should_visit_all_nodes_of_type_when()
        {
            Serialize.Json(ModelInstance,
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Member.DeclaringType.Type.Is<Child>())))
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":5," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_nullable_and_non_nullable_nodes_of_type_when(int match, int nonNullableCount)
        {
            Serialize.Json(new NullableModel { Nullable = 1, NonNullable = 2 },
                x => x.Serialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 3,
                    (s, t, o) => (int)s.Value == match)))
                .ShouldEqual("{{\"NonNullable\":{0},\"Nullable\":{1}}}".ToFormat(match == 2 ? 3 : 2, match == 1 ? 3 : 1));
        }

        [Test]
        public void should_visit_all_json_nodes_of_type_when()
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddJsonVisitor<int>(
                    (s, t, o) => t.Value = "orly",
                    (s, t, o) => s.Member.DeclaringType.Type.Is<Child>())));

            Serialize.Json(ModelInstance, options)
                .ShouldEqual("{\"Property1\":\"oh\",\"Property2\":5," +
                    "\"Child\":{\"Property1\":\"oh\",\"Property2\":\"orly\"}}");

            Serialize.Xml(ModelInstance, options).ShouldEqual(ModelXml);
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_nullable_and_non_nullable_json_nodes_of_type_when(int match, int nonNullableCount)
        {
            var options = Options.Create(
                x => x.Serialization(y => y.AddJsonVisitor<int>(
                    (s, t, o) => t.Value = 3,
                    (s, t, o) => (int)s.Value == match)));

            var model = new NullableModel { Nullable = 1, NonNullable = 2 };

            Serialize.Json(model, options)
                .ShouldEqual("{{\"NonNullable\":{0},\"Nullable\":{1}}}".ToFormat(match == 2 ? 3 : 2, match == 1 ? 3 : 1));

            Serialize.Xml(model, options)
                .ShouldEqual(Xml.Xml.Declaration + ("<NullableModel><NonNullable>2</NonNullable>" + 
                "<Nullable>1</Nullable></NullableModel>"));
        }
    }
}
