using Bender.Nodes;
using Bender.Nodes.Object;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Deserializer.Json
{
    [TestFixture]
    public class ParameterizedCtorTests
    {
        public class ParentNode
        {
            public int Age { get; set; }
            public ChildNode Child { get; set; }
        }

        public class ChildNode
        {
            private readonly ParentNode _parent;
            private string _name;

            public ChildNode(ParentNode parent)
            {
                _parent = parent;
            }

            public string Name { get { return _name + " (" + _parent.Age + ")"; } set { _name = value; } }
        }

        [Test]
        public void should_deserialize_child_with_parent_passed_into_the_constructor()
        {
            const string json = "{ \"Age\": 67, \"Child\": { \"Name\": \"Ed\" } }";
            var result = Bender.Deserializer.Create().DeserializeJson<ParentNode>(json);
            result.Child.Name.ShouldEqual("Ed (67)");
        }

        public class ParentNodeWithParameterizedChild
        {
            public ChildNodeParameterized Child { get; set; }
        }

        public class ChildNodeParameterized
        {
            public ChildNodeParameterized(string value) { }
        }

        [Test]
        public void should_fail_deserializing_child_with_parameterized_constructor()
        {
            const string json = "{ \"Child\": { \"Name\": \"Ed\" } }";
            Assert.Throws<MappingException>(() => Bender.Deserializer.Create().DeserializeJson<ParentNodeWithParameterizedChild>(json).Child.ShouldBeNull())
                .InnerException.ShouldBeType<ObjectCreationException>();
        }
    }
}
