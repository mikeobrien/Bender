using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
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
            private Dictionary<string, string> _items = new Dictionary<string, string>();

            public string this[string index]
            {
                get { return _items[index]; }
                set { _items[index] = value; }
            } 

            [XmlIgnore, XmlArray]
            public string Field;
            [XmlIgnore, XmlArray]
            public string Property { get; set; }
            public string Readonly { get { return null; } }
            public void Method() { }
        }

        private static readonly PropertyInfo PropertyInfo = typeof(Members).GetProperty("Property");
        private static readonly FieldInfo FieldInfo = typeof(Members).GetField("Field");
        private static readonly CachedMember Property = new CachedMember(PropertyInfo);
        private static readonly CachedMember Field = new CachedMember(FieldInfo);

        public object[] Cases = TestCases.Create()
            .Add(Property, PropertyInfo, PropertyInfo.PropertyType)
            .Add(Field, FieldInfo, FieldInfo.FieldType)
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
        [TestCaseSource("Cases")]
        public void should_return_name(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.Name.ShouldEqual(memberInfo.Name);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_type(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.Type.Type.ShouldEqual(type);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_declaring_type(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.DeclaringType.Type.ShouldEqual(memberInfo.DeclaringType);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_member_type(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.MemberType.ShouldEqual(memberInfo.MemberType);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_is_property(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            var isProperty = memberInfo.MemberType == MemberTypes.Property;
            cachedMember.IsProperty.ShouldEqual(isProperty);
            cachedMember.IsField.ShouldEqual(!isProperty);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_is_field(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            var isField = memberInfo.MemberType == MemberTypes.Field;
            cachedMember.IsField.ShouldEqual(isField);
            cachedMember.IsProperty.ShouldEqual(!isField);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_is_property_or_field(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.IsPublicPropertyOrField.ShouldBeTrue();
        }

        [Test]
        public void should_return_is_not_property_or_field()
        {
            new CachedMember(typeof(Members).GetMethod("Method"))
                .IsPublicPropertyOrField.ShouldBeFalse();
        }

        [Test]
        public void should_return_is_readonly()
        {
            new CachedMember(typeof(Members).GetProperty("Readonly"))
                .IsReadonly.ShouldBeTrue();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_is_not_readonly(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.IsReadonly.ShouldBeFalse();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_attributes(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.Attributes.ShouldContain(x => x is XmlIgnoreAttribute);
            cachedMember.Attributes.ShouldContain(x => x is XmlArrayAttribute);
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_attribute(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.GetAttribute<XmlIgnoreAttribute>().ShouldNotBeNull();
            cachedMember.GetAttribute<XmlArrayAttribute>().ShouldNotBeNull();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_return_null_when_attribute_not_applied_attribute(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.GetAttribute<XmlArrayItemAttribute>().ShouldBeNull();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_indicate_if_member_has_attributes(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.HasAttribute<XmlIgnoreAttribute>().ShouldBeTrue();
            cachedMember.HasAttribute<XmlArrayAttribute>().ShouldBeTrue();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_indicate_if_member_does_not_have_an_attributes(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            cachedMember.HasAttribute<XmlArrayItemAttribute>().ShouldBeFalse();
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_get_value(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            var @object = new Members();
            @object.SetPropertyOrFieldValue(memberInfo.Name, "hai");
            cachedMember.GetValue(@object).ShouldEqual("hai");
        }

        [Test]
        [TestCaseSource("Cases")]
        public void should_set_value(CachedMember cachedMember, MemberInfo memberInfo, Type type)
        {
            var @object = new Members();
            cachedMember.SetValue(@object, "hai");
            @object.GetPropertyOrFieldValue(memberInfo.Name).ShouldEqual("hai");
        }

        [Test]
        public void should_get_named_value()
        {
            var @object = new Members();
            @object["oh"] = "hai";
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
