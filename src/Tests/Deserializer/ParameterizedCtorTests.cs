using NUnit.Framework;
using Should;

namespace Tests.Deserializer
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
            const string xml = @"<ParentNode><Age>67</Age><Child><Name>Ed</Name></Child></ParentNode>";
            var result = Bender.Deserializer.Create().Deserialize<ParentNode>(xml);
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
        public void should_not_deserialize_child_with_parameterized_constructor()
        {
            const string xml = @"<ParentNodeWithParameterizedChild><Child><Name>Ed</Name></Child></ParentNodeWithParameterizedChild>";
            Bender.Deserializer.Create().Deserialize<ParentNodeWithParameterizedChild>(xml).Child.ShouldBeNull();
        }
    }
}
