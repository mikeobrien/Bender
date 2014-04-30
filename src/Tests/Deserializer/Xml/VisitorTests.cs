using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class VisitorTests
    {
        public class Model
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public Model Child { get; set; }
            public int NonNullable { get; set; }
            public int? Nullable { get; set; }
        }

        [Test]
        public void should_visit_all_nodes()
        {
            var xml = "<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>";
            var result = Deserialize.Xml<Model>(xml,
                x => x.Deserialization(y => y.AddVisitor(
                    (s, t, o) =>
                    {
                        if (t.ActualType.Is<Model>()) t.Value.As<Model>().Property2 = "orly";
                        if (t.ActualType.Is<string>()) t.Value += "!";
                    })));

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai!");
            result.Child.Property2.ShouldEqual("orly");
        }

        [Test]
        public void should_visit_all_nodes_when()
        {
            var xml = "<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>";
            var result = Deserialize.Xml<Model>(xml,
                x => x.Deserialization(y => y.AddVisitor(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly", 
                    (s, t, o) => t.ActualType.Is<Model>())));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");
        }

        [Test]
        public void should_visit_all_xml_nodes()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor(
                    (s, t, o) =>
                    {
                        if (t.ActualType.Is<Model>()) t.Value.As<Model>().Property2 = "orly";
                        if (t.ActualType.Is<string>()) t.Value += "!";
                    })));

            var result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai!");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_xml_nodes_when()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly",
                    (s, t, o) => t.ActualType.Is<Model>())));

            var result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_nodes_of_type()
        {
            var xml = "<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>";
            var result = Deserialize.Xml<Model>(xml,
                x => x.Deserialization(y => y.AddVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly")));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");
        }

        [Test]
        public void should_visit_all_non_nullable_nodes_of_type()
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2)));

            result.NonNullable.ShouldEqual(2);
            result.Nullable.ShouldEqual(1);
        }

        [Test]
        [TestCase(true, 2, 2)]
        [TestCase(false, 2, 1)]
        public void should_visit_all_nullable_and_non_nullable_nodes_of_type(
            bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_visit_all_xml_nodes_of_type()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly")));

            var result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_nodes_of_type_when()
        {
            var xml = "<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>";
            var result = Deserialize.Xml<Model>(xml,
                x => x.Deserialization(y => y.AddVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly",
                    (s, t, o) => t.Value.As<Model>().Child != null)));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_non_nullable_nodes_of_type_when(int match, int nonNullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => int.Parse(s.Value.ToString()) == match)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(1);
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_visit_all_nullable_and_non_nullable_nodes_of_type_when(
            int match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => int.Parse(s.Value.ToString()) == match, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_visit_all_xml_nodes_of_type_when()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly",
                    (s, t, o) => t.Value.As<Model>().Child != null)));

            var result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();

            result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_non_nullable_xml_nodes_of_type_when(int match, int nonNullableCount)
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => int.Parse(s.Value.ToString()) == match)));

            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>", options);

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(1);

            result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }", options);

            result.NonNullable.ShouldEqual(1);
            result.Nullable.ShouldEqual(1);
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_visit_all_nullable_and_non_nullable_xml_nodes_of_type_when(
            int match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddXmlVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => int.Parse(s.Value.ToString()) == match, includeNullable)));

            var result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>", options);

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);

            result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }", options);

            result.NonNullable.ShouldEqual(1);
            result.Nullable.ShouldEqual(1);
        }
    }
}
