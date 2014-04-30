using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
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
            const string json = "{ \"ChildWithParentConstructor\": { } }";
            var parent = Bender.Deserializer.Create().DeserializeJson<Parent>(json);
            parent.ChildWithParentConstructor.ShouldNotBeNull();
            parent.ChildWithParentConstructor.Parent.ShouldBeSameAs(parent);
        }

        [Test]
        public void should_deserialize_child_with_default_constructor()
        {
            const string json = "{ \"ChildWithDefaultConstructor\": { } }";
            var parent = Bender.Deserializer.Create().DeserializeJson<Parent>(json);
            parent.ChildWithDefaultConstructor.ShouldNotBeNull();
        }

        [Test]
        public void should_not_deserialize_child_with_non_matching_constructor()
        {
            const string json = "{ \"ChildWithNonMatchingConstructor\": { } }";
            Assert.Throws<MappingException>(() => Bender.Deserializer.Create().DeserializeJson<Parent>(json))
                .InnerException.ShouldBeType<ObjectCreationException>();
        }

        [Test]
        public void should_use_custom_object_factory()
        {
            var alternativeParent = new Parent();
            const string json = "{ \"ChildWithParentConstructor\": { } }";
            var parent = Bender.Deserializer.Create(x => x.Deserialization(
                y => y.WithObjectFactory((t, d) => t.Is<Parent>() ? alternativeParent : t.Type.CreateInstance(d))))
                .DeserializeJson<Parent>(json);
            parent.ChildWithParentConstructor.ShouldNotBeNull();
            parent.ChildWithParentConstructor.Parent.ShouldBeSameAs(alternativeParent);
        }
    }
}
