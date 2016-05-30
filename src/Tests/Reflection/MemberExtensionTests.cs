using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Reflection;
using NSubstitute;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Reflection
{
    [TestFixture]
    public class MemberExtensionTests
    {
        // Attributes

        public class Attributes
        {
            [XmlElement, XmlAttribute("AttributeName")]
            public string Hai { get; set; }
        }

        [Test]
        public void should_get_an_attribute_from_a_member()
        {
            typeof (Attributes).GetProperty("Hai")
                .HasAttribute<XmlElementAttribute>().ShouldBeTrue();
        }

        [Test]
        public void should_return_null_when_an_attribute_doesnt_exist_on_a_member()
        {
            typeof(Attributes).GetProperty("Hai")
                .HasAttribute<ObsoleteAttribute>().ShouldBeFalse();
        }

        // Member types

        public class PropertyOrFieldType
        {
            public string Field;
            public string Property { get; set; }
            public string Method() { return null; }
        }

        [Test]
        public void should_return_property_or_field_type()
        {
            var type = typeof(PropertyOrFieldType);
            type.GetField("Field").GetPropertyOrFieldType().ShouldEqual(typeof(string));
            type.GetProperty("Property").GetPropertyOrFieldType().ShouldEqual(typeof(string));
            Assert.Throws<OnlyPropertiesAndFieldsSupportedException>(() => type.GetMethod("Method").GetPropertyOrFieldType());
        }

        // Member visibility

        public class PropertyAndFieldVisibility
        {
            private string _privateField;
            protected string ProtectedField;
            internal string InternalField;
            protected internal string ProtectedInternalField;
            public string PublicField;

            private string PrivateProperty { get; set; }
            protected string ProtectedProperty { get; set; }
            internal string InternalProperty { get; set; }
            protected internal string ProtectedInternalProperty { get; set; }
            public string PublicProperty { get; set; }

            public string PublicPropertyPrivateSetter { get; private set; }
            public string PublicPropertyPrivateGetter { private get; set; }

            public string PublicReadonlyProperty { get { return null; } }
            public string PublicWriteonlyProperty { set { } }

            public string PublicPropertyProtectedSetter { get; protected set; }
            public string PublicPropertyProtectedGetter { protected get; set; }

            public string PublicPropertyProtectedInternalSetter { get; protected internal set; }
            public string PublicPropertyProtectedInternalGetter { protected internal get; set; }

            protected string ProtectedPropertyPrivateSetter { get; private set; }
            protected string ProtectedPropertyPrivateGetter { private get; set; }

            protected internal string ProtectedInternalPropertyPrivateSetter { get; private set; }
            protected internal string ProtectedInternalPropertyPrivateGetter { private get; set; }
        }

        public interface IPropertyVisibility
        {
            string Property { get; set; }
            string ReadonlyProperty { get; }
            string WriteonlyProperty { set; }
        }

        private static readonly IDictionary<string, MemberInfo> MemberVisibility = 
            typeof(PropertyAndFieldVisibility).GetPropertiesAndFields()
            .Union(typeof(IPropertyVisibility).GetPropertiesAndFields())
            .ToDictionary(x => x.Name, x => x);
            
        [Test]
        [TestCase("_privateField")]
        [TestCase("ProtectedField")]
        [TestCase("InternalField")]
        [TestCase("ProtectedInternalField")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("InternalProperty")]
        [TestCase("ProtectedInternalProperty")]
        [TestCase("ProtectedPropertyPrivateSetter")]
        [TestCase("ProtectedPropertyPrivateGetter")]
        [TestCase("ProtectedInternalPropertyPrivateSetter")]
        [TestCase("ProtectedInternalPropertyPrivateGetter")]
        public void should_indicate_if_properties_and_fields_are_private(string name)
        {
            MemberVisibility[name].IsPublicPropertyOrField().ShouldBeFalse();
        }

        [Test]
        [TestCase("Property")]
        [TestCase("ReadonlyProperty")]
        [TestCase("WriteonlyProperty")]
        [TestCase("PublicField")]
        [TestCase("PublicProperty")]
        [TestCase("PublicReadonlyProperty")]
        [TestCase("PublicWriteonlyProperty")]
        [TestCase("PublicPropertyPrivateSetter")]
        [TestCase("PublicPropertyPrivateGetter")]
        [TestCase("PublicPropertyProtectedSetter")]
        [TestCase("PublicPropertyProtectedGetter")]
        [TestCase("PublicPropertyProtectedInternalSetter")]
        [TestCase("PublicPropertyProtectedInternalGetter")]
        public void should_indicate_if_properties_and_fields_are_public(string name)
        {
            MemberVisibility[name].IsPublicPropertyOrField().ShouldBeTrue();
        }

        // Member enumeration

        public class PropertiesAndFieldsBase
        {
            private string _basePrivateField;
            protected string BaseProtectedField;
            public string BasePublicField;
            public static string BasePublicStaticField;
            private string BasePrivateProperty { get; set; }
            protected string BaseProtectedProperty { get; set; }
            public string BasePublicProperty { get; set; }
            public static string BasePublicStaticProperty { get; set; }
            public void BasePublicMethod() { }
        }

        public class PropertiesAndFields : PropertiesAndFieldsBase
        {
            private string _privateField;
            protected string ProtectedField;
            public string PublicField;
            public static string PublicStaticField;
            private string PrivateProperty { get; set; }
            protected string ProtectedProperty { get; set; }
            public string PublicProperty { get; set; }
            public static string PublicStaticProperty { get; set; }
            public void PublicMethod() { }
            public string this[int i] { get { return null; } set { } }
        }

        public interface IProperties
        {
            string Property { get; set; }
            string ReadonlyProperty { get; }
            string WriteonlyProperty { set; }
        }

        private static readonly IDictionary<string, MemberInfo> MemberEnumeration = 
            typeof(PropertiesAndFields).GetPropertiesAndFields()
                .Union(typeof(IProperties).GetPropertiesAndFields())
                .ToDictionary(x => x.Name, x => x);

        [Test]
        [TestCase("_basePrivateField")]
        [TestCase("BasePrivateProperty")]
        [TestCase("BasePublicStaticField")]
        [TestCase("BasePublicStaticProperty")]
        [TestCase("BasePublicMethod")]
        [TestCase("PublicStaticField")]
        [TestCase("PublicStaticProperty")]
        [TestCase("PublicMethod")]
        [TestCase("Item")]
        public void should_not_return_methods_and_private_members(string name)
        {
            MemberEnumeration.ContainsKey(name).ShouldBeFalse();
        }

        [Test]
        [TestCase("Property")]
        [TestCase("ReadonlyProperty")]
        [TestCase("WriteonlyProperty")]
        [TestCase("BasePublicField")]
        [TestCase("BaseProtectedField")]
        [TestCase("BaseProtectedProperty")]
        [TestCase("BasePublicProperty")]
        [TestCase("_privateField")]
        [TestCase("ProtectedField")]
        [TestCase("PublicField")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("PublicProperty")]
        public void should_return_public_and_non_public_instance_fields_and_properties(string name)
        {
            MemberEnumeration.ContainsKey(name).ShouldBeTrue();
        }

        [Test]
        [TestCase(typeof(IDictionary))]
        [TestCase(typeof(IDictionary<string, string>))]
        [TestCase(typeof(IEnumerable))]
        [TestCase(typeof(IEnumerable<string>))]
        [TestCase(typeof(IList))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(DictionaryImpl))]
        [TestCase(typeof(GenericStringDictionaryImpl))]
        [TestCase(typeof(EnumerableImpl))]
        [TestCase(typeof(GenericStringEnumerableImpl))]
        [TestCase(typeof(ListImpl))]
        [TestCase(typeof(GenericStringListImpl))]
        public void should_omit_interface_members(Type type)
        {
            type.GetPropertiesAndFields(
                typeof(IEnumerable), typeof(IEnumerable<>),
                typeof(IList), typeof(IList<>),
                typeof(IDictionary), typeof(IDictionary<,>),
                typeof(ICollection), typeof(ICollection<>))
                .Where(x => x.MemberType == MemberTypes.Property).ShouldTotal(0);
        }

        // Member access

        public class ReadonlyMember
        {
            public string _field;
            public readonly string _readonlyField;

            public string Property { get; set; }
            public string ReadonlyProperty { get { return ""; } }
        }

        public interface IReadonlyMembers
        {
            string InterfaceProperty { get; set; }
            string InterfaceReadonlyProperty { get; }
            string InterfaceWriteonlyProperty { set; }
        }

        private static readonly IDictionary<string, MemberInfo> ReadonlyMembers =
            typeof(ReadonlyMember).GetPropertiesAndFields()
                .Union(typeof(IReadonlyMembers).GetPropertiesAndFields())
                .ToDictionary(x => x.Name, x => x);

        [Test]
        [TestCase("_field")]
        [TestCase("Property")]
        [TestCase("InterfaceProperty")]
        [TestCase("InterfaceWriteonlyProperty")]
        public void should_indicate_a_member_is_not_readonly(string name)
        {
            ReadonlyMembers[name].IsReadonly().ShouldBeFalse();
        }

        [Test]
        [TestCase("ReadonlyProperty")]
        [TestCase("_readonlyField")]
        [TestCase("InterfaceReadonlyProperty")]
        public void should_indicate_a_member_is_readonly(string name)
        {
            ReadonlyMembers[name].IsReadonly().ShouldBeTrue();
        }

        public class GetAndSetPropertyAndFieldValues
        {
            public string _field;
            public string Property { get; set; }
        }

        [Test]
        public void should_get_field_values()
        {
            var @object = new GetAndSetPropertyAndFieldValues { _field = "field" };
            typeof(GetAndSetPropertyAndFieldValues).GetField("_field")
                .BuildGetter()(@object).ShouldEqual("field");
        }

        [Test]
        public void should_set_field_values()
        {
            var @object = new GetAndSetPropertyAndFieldValues();
            typeof(GetAndSetPropertyAndFieldValues).GetField("_field")
                .BuildSetter()(@object, "field");
            @object._field.ShouldEqual("field");
        }

        [Test]
        public void should_get_property_values()
        {
            var @object = new GetAndSetPropertyAndFieldValues { Property = "property" };
            typeof(GetAndSetPropertyAndFieldValues).GetProperty("Property")
                .BuildGetter()(@object).ShouldEqual("property");
        }

        [Test]
        public void should_set_property_values()
        {
            var @object = new GetAndSetPropertyAndFieldValues();
            typeof(GetAndSetPropertyAndFieldValues).GetProperty("Property")
                .BuildSetter()(@object, "property");
            @object.Property.ShouldEqual("property");
        }

        // Indexer

        public class Indexer
        {
            private readonly Dictionary<string, string> _values = 
                new Dictionary<string, string>();

            public string this[string name]
            {
                get { return _values[name]; } 
                set { _values[name] = value; }
            }

            public string Property { get; set; }
        }

        [Test]
        public void should_indicate_if_property_is_an_indexer()
        {
            typeof(Indexer).GetProperty("Item").IsIndexer().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_property_is_not_an_indexer()
        {
            typeof(Indexer).GetProperty("Property").IsIndexer().ShouldBeFalse();
        }

        [Test]
        public void should_get_indexer_value()
        {
            var indexer = new Indexer { ["oh"] = "hai" };
            typeof(Indexer).ToCachedType().GetIndexer()
                .GetValue(indexer, "oh").ShouldEqual("hai");
        }

        [Test]
        public void should_set_indexer_value()
        {
            var indexer = new Indexer();
            typeof(Indexer).ToCachedType().GetIndexer()
                .SetValue(indexer, "oh", "hai");
            indexer["oh"].ShouldEqual("hai");
        }

        // Create delegates

        public class Delegates
        {
            public virtual void Action() { }
            public virtual void Action(string value) { }
            public virtual void Action(string value1, int value2) { }

            public virtual string Func() { return "oh"; }
            public virtual string Func(string value) { return value; }
            public virtual string Func(string value1, int value2) { return value1 + value2; }
        }

        [Test]
        public void should_create_action_delegate()
        {
            var delegates = Substitute.For<Delegates>();
            typeof(Delegates).CreateActionDelegate("Action")
                (delegates, new object[] {});
            delegates.Received().Action();
        }

        [Test]
        public void should_create_action_delegate_with_one_parameter()
        {
            var delegates = Substitute.For<Delegates>();
            typeof(Delegates).CreateActionDelegate("Action", 
                    new [] { typeof(string) })
                (delegates, new object[] { "hai" });
            delegates.Received().Action("hai");
        }

        [Test]
        public void should_create_action_delegate_with_two_parameters()
        {
            var delegates = Substitute.For<Delegates>();
            typeof(Delegates).CreateActionDelegate("Action", 
                    new [] { typeof(string), typeof(int) })
                (delegates, new object[] { "hai", 5 });
            delegates.Received().Action("hai", 5);
        }

        [Test]
        public void should_create_func_delegate()
        {
            typeof(Delegates).CreateFuncDelegate("Func")
                (new Delegates(), new object[] { }).ShouldEqual("oh");
        }

        [Test]
        public void should_create_func_delegate_with_one_parameter()
        {
            typeof(Delegates).CreateFuncDelegate("Func",
                    new[] { typeof(string) })
                (new Delegates(), new object[] { "hai" }).ShouldEqual("hai");
        }

        [Test]
        public void should_create_func_delegate_with_two_parameters()
        {
            typeof(Delegates).CreateFuncDelegate("Func",
                    new[] { typeof(string), typeof(int) })
                (new Delegates(), new object[] { "hai", 5 }).ShouldEqual("hai5");
        }
    }
}
