using System.Collections;
using System.Collections.Generic;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object.Values
{
    [TestFixture]
    public class MemberValueTests
    {
        public class Model
        {
            public string Field;
            public string Property { get; set; }

            public readonly string ReadonlyField;
            public string ReadonlyProperty { get { return "hai"; } }

            public IList List { get { return new List<string>(); } }

            public IList NullList { get { return null; } }
        }

        private static readonly CachedType ModelType = typeof(Model).GetCachedType(); 

        [Test]
        public void should_set_and_get_property_value()
        {
            var model = new Model();
            var value = new SimpleValue(model, ModelType);
            var member = new MemberValue(value, new CachedMember(ModelType.Type.GetProperty("Property")), x => false);

            member.Instance.ShouldBeNull();
            member.SpecifiedType.Type.ShouldBe<string>();
            member.ActualType.Type.ShouldBe<string>();

            member.Instance = "oh";
            member.Instance.ShouldEqual("oh");
            member.ActualType.Type.ShouldBe<string>();
            member.SpecifiedType.Type.ShouldBe<string>();
            model.Property.ShouldEqual("oh");

            model.Property = "hai";
            member.Instance.ShouldEqual("hai");
            member.ActualType.Type.ShouldBe<string>();
            member.SpecifiedType.Type.ShouldBe<string>();
            model.Property.ShouldEqual("hai");
        }

        [Test]
        public void should_set_and_get_field_value()
        {
            var model = new Model();
            var value = new SimpleValue(model, ModelType);
            var member = new MemberValue(value, new CachedMember(ModelType.Type.GetField("Field")), x => false);

            member.Instance.ShouldBeNull();
            member.SpecifiedType.Type.ShouldBe<string>();
            member.SpecifiedType.Type.ShouldBe<string>();
            member.ActualType.Type.ShouldBe<string>();

            member.Instance = "oh";
            member.Instance.ShouldEqual("oh");
            member.ActualType.Type.ShouldBe<string>();
            member.SpecifiedType.Type.ShouldBe<string>();
            model.Field.ShouldEqual("oh");

            model.Field = "hai";
            member.Instance.ShouldEqual("hai");
            member.ActualType.Type.ShouldBe<string>();
            member.SpecifiedType.Type.ShouldBe<string>();
            model.Field.ShouldEqual("hai");
        }

        [Test]
        public void should_get_actual_and_specified_type()
        {
            var member = new MemberValue(new SimpleValue(new Model(), ModelType),
                new CachedMember(ModelType.Type.GetProperty("List")), x => false);

            member.SpecifiedType.Type.ShouldBe<IList>();
            member.ActualType.Type.ShouldBe<List<string>>();
        }

        [Test]
        public void should_override_specified_type_with_actual_type()
        {
            var member = new MemberValue(new SimpleValue(new Model(), ModelType),
                new CachedMember(ModelType.Type.GetProperty("List")), x =>
                {
                    x.Type.ShouldBe<IList>();
                    return true;
                });

            member.SpecifiedType.Type.ShouldBe<List<string>>();
            member.ActualType.Type.ShouldBe<List<string>>();
        }

        [Test]
        public void should_not_override_specified_type_with_actual_type_when_value_is_null()
        {
            var member = new MemberValue(new SimpleValue(new Model(), ModelType),
                new CachedMember(ModelType.Type.GetProperty("NullList")), x =>
                {
                    x.Type.ShouldBe<IList>();
                    return true;
                });

            member.SpecifiedType.Type.ShouldBe<IList>();
            member.ActualType.Type.ShouldBe<IList>();
        }

        [Test]
        public void should_indicate_if_property_is_readonly()
        {
            new MemberValue(null, new CachedMember(ModelType.Type.GetProperty("Property")), x => false)
                .IsReadonly.ShouldBeFalse();

            new MemberValue(null, new CachedMember(ModelType.Type.GetProperty("ReadonlyProperty")), x => false)
                .IsReadonly.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_field_is_readonly()
        {
            new MemberValue(null, new CachedMember(ModelType.Type.GetField("Field")), x => false)
                .IsReadonly.ShouldBeFalse();

            new MemberValue(null, new CachedMember(ModelType.Type.GetField("ReadonlyField")), x => false)
                .IsReadonly.ShouldBeTrue();
        }
    }
}
