using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        public class Parent
        {
            public ChildWithDefaultConstructor ChildWithDefaultConstructor { get; set; }
            public ChildWithParentConstructor ChildWithParentConstructor { get; set; }
            public ChildWithNonMatchingConstructor ChildWithNonMatchingConstructor { get; set; }
        }

        public class ChildWithParentConstructor
        {
            public ChildWithParentConstructor(Parent parent)
            {
                Parent = parent;
            }

            public Parent Parent { get; set; }
        }

        public class ChildWithDefaultConstructor { }

        public class ChildWithNonMatchingConstructor
        {
            public ChildWithNonMatchingConstructor(string value) { }
        }

        [Test]
        public void should_deserialize_child_with_parent_passed_into_the_constructor()
        {
            const string xml = "<Parent><ChildWithParentConstructor /></Parent>";
            var parent = Bender.Deserializer.Create().DeserializeXml<Parent>(xml);
            parent.ChildWithParentConstructor.ShouldNotBeNull();
            parent.ChildWithParentConstructor.Parent.ShouldBeSameAs(parent);
        }

        [Test]
        public void should_deserialize_child_with_default_constructor()
        {
            const string xml = "<Parent><ChildWithDefaultConstructor /></Parent>";
            var parent = Bender.Deserializer.Create().DeserializeXml<Parent>(xml);
            parent.ChildWithDefaultConstructor.ShouldNotBeNull();
        }

        [Test]
        public void should_not_deserialize_child_with_non_matching_constructor()
        {
            const string xml = "<Parent><ChildWithNonMatchingConstructor /></Parent>";
            var exception = Assert.Throws<MappingException>(
                () => Bender.Deserializer.Create().DeserializeXml<Parent>(xml));

            exception.Message.ShouldEqual("Error deserializing xml element '/Parent/ChildWithNonMatchingConstructor' " +
                "to 'Tests.Deserializer.Xml.DependencyInjectionTests.Parent.ChildWithNonMatchingConstructor': Could not " +
                "instantiate type 'Tests.Deserializer.Xml.DependencyInjectionTests.ChildWithNonMatchingConstructor'. " +
                "Constructor on type 'Tests.Deserializer.Xml.DependencyInjectionTests+ChildWithNonMatchingConstructor' not found.");
            exception.InnerException.ShouldBeType<ObjectCreationException>();
        }

        [Test]
        public void should_use_custom_object_factory()
        {
            var alternativeParent = new Parent();
            const string xml = "<Parent><ChildWithParentConstructor /></Parent>";
            var parent = Bender.Deserializer.Create(x => x.Deserialization(
                y => y.WithObjectFactory((t, d) => t.Is<Parent>() ? alternativeParent : t.Type.CreateInstance(d))))
                .DeserializeXml<Parent>(xml);
            parent.ChildWithParentConstructor.ShouldNotBeNull();
            parent.ChildWithParentConstructor.Parent.ShouldBeSameAs(alternativeParent);
        }
    }
}
