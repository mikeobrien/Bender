using Bender;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
            var json = "{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }";
            var result = Deserialize.Json<Model>(json,
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
            var json = "{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }";
            var result = Deserialize.Json<Model>(json,
                x => x.Deserialization(y => y.AddVisitor(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly", 
                    (s, t, o) => t.ActualType.Is<Model>())));

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");
        }

        [Test]
        public void should_visit_all_json_nodes()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor(
                    (s, t, o) =>
                    {
                        if (t.ActualType.Is<Model>()) t.Value.As<Model>().Property2 = "orly";
                        if (t.ActualType.Is<string>()) t.Value += "!";
                    })));

            var result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh!");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai!");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_json_nodes_when()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly",
                    (s, t, o) => t.ActualType.Is<Model>())));

            var result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_nodes_of_type()
        {
            var json = "{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }";
            var result = Deserialize.Json<Model>(json,
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
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
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
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_visit_all_json_nodes_of_type()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly")));

            var result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldEqual("orly");

            result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        public void should_visit_all_nodes_of_type_when()
        {
            var json = "{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }";
            var result = Deserialize.Json<Model>(json,
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
        public void should_visit_all_non_nullable_nodes_of_type_when(decimal match, int nonNullableCount)
        {
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => (decimal)s.Value == match)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(1);
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_visit_all_nullable_and_non_nullable_nodes_of_type_when(
            decimal match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }",
                x => x.Deserialization(y => y.AddVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => (decimal)s.Value == match, includeNullable)));

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);
        }

        [Test]
        public void should_visit_all_json_nodes_of_type_when()
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor<Model>(
                    (s, t, o) => t.Value.As<Model>().Property2 = "orly",
                    (s, t, o) => t.Value.As<Model>().Child != null)));

            var result = Deserialize.Json<Model>("{ \"Property1\": \"oh\", \"Child\": { \"Property1\": \"hai\" } }", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldEqual("orly");

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();

            result = Deserialize.Xml<Model>("<Model><Property1>oh</Property1><Child><Property1>hai</Property1></Child></Model>", options);

            result.Property1.ShouldEqual("oh");
            result.Property2.ShouldBeNull();

            result.Child.Property1.ShouldEqual("hai");
            result.Child.Property2.ShouldBeNull();
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void should_visit_all_non_nullable_json_nodes_of_type_when(decimal match, int nonNullableCount)
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => (decimal)s.Value == match)));

            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }", options);

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(1);

            result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>", options);

            result.NonNullable.ShouldEqual(1);
            result.Nullable.ShouldEqual(1);
        }

        [Test]
        [TestCase(1, true, 2, 2)]
        [TestCase(2, true, 1, 1)]
        [TestCase(1, false, 2, 1)]
        [TestCase(2, false, 1, 1)]
        public void should_visit_all_nullable_and_non_nullable_json_nodes_of_type_when(
            decimal match, bool includeNullable, int nonNullableCount, int nullableCount)
        {
            var options = Options.Create(
                x => x.Deserialization(y => y.AddJsonVisitor<int>(
                    (s, t, o) => t.Value = 2,
                    (s, t, o) => (decimal)s.Value == match, includeNullable)));

            var result = Deserialize.Json<Model>("{ \"NonNullable\": 1, \"Nullable\": 1 }", options);

            result.NonNullable.ShouldEqual(nonNullableCount);
            result.Nullable.ShouldEqual(nullableCount);

            result = Deserialize.Xml<Model>("<Model><NonNullable>1</NonNullable><Nullable>1</Nullable></Model>", options);

            result.NonNullable.ShouldEqual(1);
            result.Nullable.ShouldEqual(1);
        }
    }
}
