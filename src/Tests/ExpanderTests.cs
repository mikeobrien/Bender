using System;
using System.Collections.Generic;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class ExpanderTests
    {
        public enum Enum { Value1, Value2 }

        public class Graph
        {
            public List<object> NodeList { get; set; }
            public IList<object> NodeIList { get; set; }

            public string String { get; set; }
            public bool Boolean { get; set; } public bool? NullableBoolean { get; set; }
            public byte Byte { get; set; } public byte? NullableByte { get; set; }
            public byte[] ByteArray { get; set; }
            public sbyte UnsignedByte { get; set; } public sbyte? NullableUnsignedByte { get; set; }
            public short Short { get; set; } public short? NullableShort { get; set; }
            public ushort UnsignedShort { get; set; } public ushort? NullableUnsignedShort { get; set; }
            public int Integer { get; set; } public int? NullableInteger { get; set; }
            public uint UnsignedInteger { get; set; } public uint? NullableUnsignedInteger { get; set; }
            public long Long { get; set; } public long? NullableLong { get; set; }
            public ulong UnsignedLong { get; set; } public ulong? NullableUnsignedLong { get; set; }
            public float Float { get; set; } public float? NullableFloat { get; set; }
            public double Double { get; set; } public double? NullableDouble { get; set; }
            public decimal Decimal { get; set; } public decimal? NullableDecimal { get; set; }
            public DateTime DateTime { get; set; } public DateTime? NullableDateTime { get; set; }
            public TimeSpan TimeSpan { get; set; } public TimeSpan? NullableTimeSpan { get; set; }
            public Guid Guid { get; set; } public Guid? NullableGuid { get; set; }
            public Enum Enum { get; set; } public Enum? NullableEnum { get; set; }
            public Uri Uri { get; set; }
            public object Object { get; set; }
        }

        [Test]
        public void should_not_expand_simple_types()
        {
            var graph = Expander.Expand<Graph>();

            graph.ShouldNotBeNull();
            graph.String.ShouldBeNull();
            graph.Boolean.ShouldBeFalse();
            graph.NullableBoolean.HasValue.ShouldBeFalse();
            graph.Byte.ShouldEqual((byte)0);
            graph.NullableByte.HasValue.ShouldBeFalse();
            graph.ByteArray.ShouldBeNull();
            graph.UnsignedByte.ShouldEqual((sbyte)0);
            graph.NullableUnsignedByte.HasValue.ShouldBeFalse();
            graph.UnsignedShort.ShouldEqual((ushort)0);
            graph.NullableUnsignedShort.HasValue.ShouldBeFalse();
            graph.Short.ShouldEqual((short)0);
            graph.NullableShort.HasValue.ShouldBeFalse();
            graph.UnsignedInteger.ShouldEqual((uint)0);
            graph.NullableUnsignedInteger.HasValue.ShouldBeFalse();
            graph.Integer.ShouldEqual(0);
            graph.NullableInteger.HasValue.ShouldBeFalse();
            graph.UnsignedLong.ShouldEqual((ulong)0);
            graph.NullableUnsignedLong.HasValue.ShouldBeFalse();
            graph.Long.ShouldEqual(0);
            graph.NullableLong.HasValue.ShouldBeFalse();
            graph.Float.ShouldEqual(0);
            graph.NullableFloat.HasValue.ShouldBeFalse();
            graph.Double.ShouldEqual(0);
            graph.NullableDouble.HasValue.ShouldBeFalse();
            graph.Decimal.ShouldEqual(0);
            graph.NullableDecimal.HasValue.ShouldBeFalse();
            graph.DateTime.ShouldEqual(DateTime.MinValue);
            graph.NullableDateTime.HasValue.ShouldBeFalse();
            graph.TimeSpan.ShouldEqual(TimeSpan.Zero);
            graph.NullableTimeSpan.HasValue.ShouldBeFalse();
            graph.Guid.ShouldEqual(Guid.Empty);
            graph.NullableGuid.HasValue.ShouldBeFalse();
            graph.Enum.ShouldEqual(Enum.Value1);
            graph.NullableEnum.HasValue.ShouldBeFalse();
            graph.Uri.ShouldBeNull();
            graph.Object.ShouldBeNull();

            graph.NodeList.ShouldNotBeNull();
            graph.NodeList.ShouldBeEmpty();
            graph.NodeIList.ShouldNotBeNull();
            graph.NodeIList.ShouldBeEmpty();
        }

        public class GraphWithParameterlessNode
        {
            public ParameterlessNode Node { get; set; }
        }

        public class ParameterlessNode
        {
            public ParameterlessNode() {}
            public ParameterlessNode(string value) { }
        }

        [Test]
        public void should_expand_parameterless_constructor()
        {
            Expander.Expand<GraphWithParameterlessNode>().Node.ShouldNotBeNull();
        }

        public class GraphWithParentParameterizedNode
        {
            public ParentParameterizedNode Node { get; set; }
        }

        public class ParentParameterizedNode
        {
            public ParentParameterizedNode(GraphWithParentParameterizedNode value) { }
            public ParentParameterizedNode(string value) { }
        }

        [Test]
        public void should_expand_parent_parameterized_constructor()
        {
            Expander.Expand<GraphWithParentParameterizedNode>().Node.ShouldNotBeNull();
        }

        public class GraphWithParameterizedNode
        {
            public ParameterizedNode Node { get; set; }
        }

        public class ParameterizedNode
        {
            public ParameterizedNode(string value) { }
        }

        [Test]
        public void should_not_expand_parameterized_constructor()
        {
            Expander.Expand<GraphWithParameterizedNode>().Node.ShouldBeNull();
        }
    }
}