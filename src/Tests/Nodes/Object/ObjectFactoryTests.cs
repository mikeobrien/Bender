using System;
using System.Collections;
using System.Collections.Generic;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ObjectFactoryTests
    {
        private static readonly object[] SimpleTypes = TestCases.Create()
            .AddType<string>()
            .AddType<Uri>()

            .AddType<UriFormat>().AddType<UriFormat?>()

            .AddType<DateTime>().AddType<DateTime?>()
            .AddType<TimeSpan>().AddType<TimeSpan?>()
            .AddType<Guid>().AddType<Guid?>()

            .AddType<Boolean>().AddType<Boolean?>()
            .AddType<Byte>().AddType<Byte?>()
            .AddType<SByte>().AddType<SByte?>()
            .AddType<Int16>().AddType<Int16?>()
            .AddType<UInt16>().AddType<UInt16?>()
            .AddType<Int32>().AddType<Int32?>()
            .AddType<UInt32>().AddType<UInt32?>()
            .AddType<Int64>().AddType<Int64?>()
            .AddType<UInt64>().AddType<UInt64?>()
            .AddType<IntPtr>().AddType<IntPtr?>()
            .AddType<UIntPtr>().AddType<UIntPtr?>()
            .AddType<Char>().AddType<Char?>()
            .AddType<Double>().AddType<Double?>()
            .AddType<Single>().AddType<Single?>()
            .AddType<Decimal>().AddType<Decimal?>()

            .All;

        [Test]
        [TestCaseSource("SimpleTypes")]
        public void should_fail_to_create_simple_types(Type type)
        {
            Assert.Throws<SimpleTypeInstantiationNotSupportedException>(() => ObjectFactory.CreateInstance(type.GetCachedType()))
                .Message.ShouldEqual(SimpleTypeInstantiationNotSupportedException.MessageFormat.ToFormat(type.GetFriendlyTypeFullName()));
        }

        [Test]
        [TestCase(typeof(string[]), typeof(string[]))]
        [TestCase(typeof(IList<string>), typeof(List<string>))]
        [TestCase(typeof(IList), typeof(List<object>))]
        [TestCase(typeof(IDictionary<string, int>), typeof(Dictionary<string, int>))]
        [TestCase(typeof(IDictionary), typeof(Dictionary<object, object>))]
        [TestCase(typeof(IEnumerable<string>), typeof(List<string>))]
        [TestCase(typeof(IEnumerable), typeof(List<object>))]
        public void should_create_collection_interfaces(Type specifiedType, Type resultingType)
        {
            var result = ObjectFactory.CreateInstance(specifiedType.GetCachedType());
            result.ShouldNotBeNull();
            result.ShouldBeType(resultingType);
        }

        public class ComplexType { }

        [Test]
        public void should_create_complex_type()
        {
            var result = ObjectFactory.CreateInstance(typeof(ComplexType).GetCachedType());
            result.ShouldNotBeNull();
            result.ShouldBeType<ComplexType>();
        }

        public class ComplexTypeWithDependency
        {
            public ComplexTypeWithDependency() { }

            public ComplexTypeWithDependency(ComplexType dependency)
            {
                Dependency = dependency;
            }

            public ComplexType Dependency { get; private set; }
        }

        [Test]
        public void should_create_complex_type_with_dependency()
        {
            var dependency = new ComplexType();
            var result = ObjectFactory.CreateInstance(typeof(ComplexTypeWithDependency).GetCachedType(), null, dependency);
            result.ShouldNotBeNull();
            result.ShouldBeType<ComplexTypeWithDependency>();
            ((ComplexTypeWithDependency)result).Dependency.ShouldEqual(dependency);
        }

        [Test]
        public void should_create_complex_type_with_no_dependency()
        {
            var dependency = new ComplexType();
            var result = ObjectFactory.CreateInstance(typeof(ComplexType).GetCachedType(), null, dependency);
            result.ShouldNotBeNull();
            result.ShouldBeType<ComplexType>();
        }

        [Test]
        public void should_fail_create_complex_type_with_unresolvable_dependency()
        {
            Assert.Throws<ObjectCreationException>(() => ObjectFactory.CreateInstance(typeof(ComplexType).GetCachedType(), null, null))
                .Message.ShouldEqual(ObjectCreationException.MessageFormat.ToFormat(
                    "Tests.Nodes.Object.ObjectFactoryTests.ComplexType", 
                    "Value cannot be null. Parameter name: source"));
        }

        [Test]
        public void should_create_complex_type_with_structure_map_factory()
        {
            var dependency = new ComplexType();
            var result = ObjectFactory.CreateInstance(typeof(ComplexTypeWithDependency).GetCachedType(),
                (t, d) =>
                {
                    var container = StructureMap.ObjectFactory.Container.GetNestedContainer();
                    d.ForEach(x => container.Inject(x.GetType(), x));
                    return container.GetInstance(t.Type);
                }, dependency);
            result.ShouldNotBeNull();
            result.ShouldBeType<ComplexTypeWithDependency>();
            ((ComplexTypeWithDependency)result).Dependency.ShouldEqual(dependency);
        }
    }
}
