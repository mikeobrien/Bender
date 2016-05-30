using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Reflection
{   
    [TestFixture]
    public class CachedMemberTests
    {
        public class Members
        {
            private readonly Dictionary<string, string> _items = new Dictionary<string, string>();

            public string this[string index]
            {
                get { return _items[index]; }
                set { _items[index] = value; }
            } 

            [XmlIgnore, XmlArray]
            public string Field;
            [XmlIgnore, XmlArray]
            public Optional<string> OptionalField;
            [XmlIgnore, XmlArray]
            public string Property { get; set; }
            public string ReadonlyProperty => null;
            public string WriteonlyProperty { set { } }
            [XmlIgnore, XmlArray]
            public Optional<string> OptionalProperty { get; set; }
            public readonly string ReadonlyField;
            public void Method() { }
        }

        private static readonly PropertyInfo PropertyInfo = typeof(Members).GetProperty("Property");
        private static readonly FieldInfo FieldInfo = typeof(Members).GetField("Field");
        private static readonly CachedMember Property = new CachedMember(PropertyInfo);
        private static readonly CachedMember Field = new CachedMember(FieldInfo);

        private static readonly PropertyInfo OptionalPropertyInfo = typeof(Members).GetProperty("OptionalProperty");
        private static readonly FieldInfo OptionalFieldInfo = typeof(Members).GetField("OptionalField");
        private static readonly CachedMember OptionalProperty = new CachedMember(OptionalPropertyInfo);
        private static readonly CachedMember OptionalField = new CachedMember(OptionalFieldInfo);

        public object[] Cases = TestCases.Create()
            .Add(Property, PropertyInfo)
            .Add(Field, FieldInfo)
            .Add(OptionalProperty, OptionalPropertyInfo)
            .Add(OptionalField, OptionalFieldInfo)
            .All;

        [Test]
        public void should_return_property_info()
        {
            Property.MemberInfo.ShouldBeSameAs(PropertyInfo);
            Property.PropertyInfo.ShouldBeSameAs(PropertyInfo);
            Property.FieldInfo.ShouldBeNull();
        }

        [Test]
        public void should_return_field_info()
        {
            Field.MemberInfo.ShouldBeSameAs(FieldInfo);
            Field.FieldInfo.ShouldBeSameAs(FieldInfo);
            Field.PropertyInfo.ShouldBeNull();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_name(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.Name.ShouldEqual(memberInfo.Name);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_type(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.Type.Type.ShouldEqual(typeof(string));
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_declaring_type(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.DeclaringType.Type.ShouldEqual(memberInfo.DeclaringType);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_is_a_property(CachedMember cachedMember, MemberInfo memberInfo)
        {
            var isProperty = memberInfo.MemberType == MemberTypes.Property;
            cachedMember.IsProperty.ShouldEqual(isProperty);
            cachedMember.IsField.ShouldEqual(!isProperty);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_is_a_field(CachedMember cachedMember, MemberInfo memberInfo)
        {
            var isField = memberInfo.MemberType == MemberTypes.Field;
            cachedMember.IsField.ShouldEqual(isField);
            cachedMember.IsProperty.ShouldEqual(!isField);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_property_or_field(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.IsPublicPropertyOrField.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_not_property_or_field()
        {
            new CachedMember(typeof(Members).GetMethod("Method"))
                .IsPublicPropertyOrField.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_a_field_is_optional()
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.OptionalField)))
                .IsOptional.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_property_is_optional()
        {
            new CachedMember(typeof(Members).GetProperty(nameof(Members.OptionalProperty)))
                .IsOptional.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_member_is_not_optional()
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.Field)))
                .IsOptional.ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_an_optional_field_has_a_value(
            [Values(null, "", "fark")] string value)
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.OptionalField)))
                .HasValue(new Members{ OptionalField = value }).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_an_optional_field_does_not_have_a_value()
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.OptionalField)))
                .HasValue(new Members()).ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_an_optional_property_has_a_value(
            [Values(null, "", "fark")] string value)
        {
            new CachedMember(typeof(Members).GetProperty(nameof(Members.OptionalProperty)))
                .HasValue(new Members { OptionalProperty = value }).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_an_optional_property_does_not_have_a_value()
        {
            new CachedMember(typeof(Members).GetProperty(nameof(Members.OptionalProperty)))
                .HasValue(new Members()).ShouldBeFalse();
        }

        [Test]
        public void should_always_indicate_that_a_non_optional_member_has_a_value()
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.ReadonlyField)))
                .HasValue(new Members()).ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_field_is_readonly()
        {
            new CachedMember(typeof(Members).GetField(nameof(Members.ReadonlyField)))
                .IsReadonly.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_property_is_readonly()
        {
            new CachedMember(typeof(Members).GetProperty(nameof(Members.ReadonlyProperty)))
                .IsReadonly.ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_a_property_is_writeonly()
        {
            new CachedMember(typeof(Members).GetProperty("WriteonlyProperty"))
                .HasGetter.ShouldBeFalse();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_a_member_is_not_readonly(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.IsReadonly.ShouldBeFalse();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_attributes(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.Attributes.ShouldContain(x => x is XmlIgnoreAttribute);
            cachedMember.Attributes.ShouldContain(x => x is XmlArrayAttribute);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_attribute(CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.GetAttribute<XmlIgnoreAttribute>().ShouldNotBeNull();
            cachedMember.GetAttribute<XmlArrayAttribute>().ShouldNotBeNull();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_return_null_when_attribute_not_applied_attribute(
            CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.GetAttribute<XmlArrayItemAttribute>().ShouldBeNull();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_member_has_attributes(
            CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.HasAttribute<XmlIgnoreAttribute>().ShouldBeTrue();
            cachedMember.HasAttribute<XmlArrayAttribute>().ShouldBeTrue();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_indicate_if_member_does_not_have_an_attributes(
            CachedMember cachedMember, MemberInfo memberInfo)
        {
            cachedMember.HasAttribute<XmlArrayItemAttribute>().ShouldBeFalse();
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_get_value(CachedMember cachedMember, MemberInfo memberInfo)
        {
            var @object = new Members();
            @object.SetPropertyOrFieldValue(memberInfo.Name,
                memberInfo.GetPropertyOrFieldType() == typeof(string) ?
                    "hai" : (object)(Optional<string>)"hai");
            cachedMember.GetValue(@object).As<string>().ShouldEqual("hai");
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void should_set_value(CachedMember cachedMember, MemberInfo memberInfo)
        {
            var @object = new Members();
            cachedMember.SetValue(@object, "hai");
            var value = @object.GetPropertyOrFieldValue(memberInfo.Name);
            (memberInfo.GetPropertyOrFieldType() == typeof(string) ? "hai" : 
                (string)(Optional<string>)value).ShouldEqual("hai");
        }

        [Test]
        public void should_get_named_value()
        {
            var @object = new Members { ["oh"] = "hai"} ;
            new CachedMember(typeof(Members).GetProperty("Item"))
                .GetValue(@object, "oh").ShouldEqual("hai");
        }

        [Test]
        public void should_set_named_value()
        {
            var @object = new Members();
            new CachedMember(typeof(Members).GetProperty("Item"))
                .SetValue(@object, "oh", "hai");
            @object["oh"].ShouldEqual("hai");
        }
    }
}
