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
            public Node Node { get; set; }
            public List<Node> Nodes { get; set; }

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
        }

        public class Node
        {
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
        public void should_expand_graph()
        {
            var graph = Expander.Expand<Graph>();

            graph.ShouldNotBeNull();
            graph.String.ShouldNotBeNull();
            graph.Boolean.ShouldBeFalse();
            graph.NullableBoolean.HasValue.ShouldBeFalse();
            graph.Byte.ShouldEqual((byte)0);
            graph.NullableByte.HasValue.ShouldBeFalse();
            graph.ByteArray.ShouldNotBeNull();
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

            graph.Node.ShouldNotBeNull();

            graph.Node.Float.ShouldEqual(0);
            graph.Node.NullableFloat.HasValue.ShouldBeFalse();
            graph.Node.Double.ShouldEqual(0);
            graph.Node.NullableDouble.HasValue.ShouldBeFalse();
            graph.Node.Decimal.ShouldEqual(0);
            graph.Node.NullableDecimal.HasValue.ShouldBeFalse();
            graph.Node.DateTime.ShouldEqual(DateTime.MinValue);
            graph.Node.NullableDateTime.HasValue.ShouldBeFalse();
            graph.Node.TimeSpan.ShouldEqual(TimeSpan.Zero);
            graph.Node.NullableTimeSpan.HasValue.ShouldBeFalse();
            graph.Node.Guid.ShouldEqual(Guid.Empty);
            graph.Node.NullableGuid.HasValue.ShouldBeFalse();
            graph.Node.Enum.ShouldEqual(Enum.Value1);
            graph.Node.NullableEnum.HasValue.ShouldBeFalse();
            graph.Node.Uri.ShouldBeNull();
            graph.Node.Object.ShouldBeNull();

            graph.Nodes.ShouldNotBeNull();
            graph.Nodes.ShouldBeEmpty();
        }
    }
}