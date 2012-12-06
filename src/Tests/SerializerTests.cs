using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class SerializerTests
    {
        public enum Enum { Value1, Value2 }
        public class SimpleTypes
        {
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
        }

        [Test]
        public void should_serialize_simple_types()
        {
            var simpleTypes = new SimpleTypes {
                    String = "hai",
                    Boolean = true, NullableBoolean = true,
                    Byte = 1, NullableByte = 1,
                    ByteArray = new byte[] { 1, 2, 3 },
                    UnsignedByte = 2, NullableUnsignedByte = 2,
                    Short = 3, NullableShort = 3,
                    UnsignedShort = 4, NullableUnsignedShort = 4,
                    Integer = 5, NullableInteger = 5,
                    UnsignedInteger = 6, NullableUnsignedInteger = 6,
                    Long = 7, NullableLong = 7,
                    UnsignedLong = 8, NullableUnsignedLong = 8,
                    Float = 1.1F, NullableFloat = 1.1F,
                    Double = 1.2, NullableDouble = 1.2,
                    Decimal = 1.3M, NullableDecimal = 1.3M,
                    DateTime = DateTime.MaxValue, NullableDateTime = DateTime.MaxValue,
                    TimeSpan = TimeSpan.MaxValue, NullableTimeSpan = TimeSpan.MaxValue,
                    Guid = Guid.Empty, NullableGuid = Guid.Empty,
                    Uri = new Uri("http://www.google.com"),
                    Enum = Enum.Value2, NullableEnum = Enum.Value2
                };

            var xml = Serializer.Create(x => x.PrettyPrint()).Serialize(simpleTypes);
            Debug.WriteLine(xml);

            xml.ShouldNotBeNull();
            var root = XDocument.Parse(xml).Element("SimpleTypes");
            root.ShouldNotBeNull();
            root.Element("String").Value.ShouldEqual(simpleTypes.String);
            root.Element("Boolean").Value.ShouldEqual(simpleTypes.Boolean.ToString().ToLower());
            root.Element("Byte").Value.ShouldEqual(simpleTypes.Byte.ToString());
            root.Element("ByteArray").Value.ShouldEqual(Convert.ToBase64String(simpleTypes.ByteArray));
            root.Element("UnsignedByte").Value.ShouldEqual(simpleTypes.UnsignedByte.ToString());
            root.Element("Short").Value.ShouldEqual(simpleTypes.Short.ToString());
            root.Element("UnsignedShort").Value.ShouldEqual(simpleTypes.UnsignedShort.ToString());
            root.Element("UnsignedInteger").Value.ShouldEqual(simpleTypes.UnsignedInteger.ToString());
            root.Element("Long").Value.ShouldEqual(simpleTypes.Long.ToString());
            root.Element("UnsignedLong").Value.ShouldEqual(simpleTypes.UnsignedLong.ToString());
            root.Element("Float").Value.ShouldEqual(simpleTypes.Float.ToString());
            root.Element("Double").Value.ShouldEqual(simpleTypes.Double.ToString());
            root.Element("Decimal").Value.ShouldEqual(simpleTypes.Decimal.ToString());
            root.Element("DateTime").Value.ShouldEqual(simpleTypes.DateTime.ToString());
            root.Element("TimeSpan").Value.ShouldEqual(simpleTypes.TimeSpan.ToString());
            root.Element("Guid").Value.ShouldEqual(simpleTypes.Guid.ToString());
            root.Element("Enum").Value.ShouldEqual(simpleTypes.Enum.ToString());
            root.Element("Uri").Value.ShouldEqual(simpleTypes.Uri.ToString());

            root.Element("NullableBoolean").Value.ShouldEqual(simpleTypes.NullableBoolean.ToString().ToLower());
            root.Element("NullableByte").Value.ShouldEqual(simpleTypes.NullableByte.ToString());
            root.Element("NullableUnsignedByte").Value.ShouldEqual(simpleTypes.NullableUnsignedByte.ToString());
            root.Element("NullableShort").Value.ShouldEqual(simpleTypes.NullableShort.ToString());
            root.Element("NullableUnsignedShort").Value.ShouldEqual(simpleTypes.NullableUnsignedShort.ToString());
            root.Element("NullableUnsignedInteger").Value.ShouldEqual(simpleTypes.NullableUnsignedInteger.ToString());
            root.Element("NullableLong").Value.ShouldEqual(simpleTypes.NullableLong.ToString());
            root.Element("NullableUnsignedLong").Value.ShouldEqual(simpleTypes.NullableUnsignedLong.ToString());
            root.Element("NullableFloat").Value.ShouldEqual(simpleTypes.NullableFloat.ToString());
            root.Element("NullableDouble").Value.ShouldEqual(simpleTypes.NullableDouble.ToString());
            root.Element("NullableDecimal").Value.ShouldEqual(simpleTypes.NullableDecimal.ToString());
            root.Element("NullableDateTime").Value.ShouldEqual(simpleTypes.NullableDateTime.ToString());
            root.Element("NullableTimeSpan").Value.ShouldEqual(simpleTypes.NullableTimeSpan.ToString());
            root.Element("NullableGuid").Value.ShouldEqual(simpleTypes.NullableGuid.ToString());
            root.Element("NullableEnum").Value.ShouldEqual(simpleTypes.NullableEnum.ToString());
        }

        public class Graph
        {
            public GraphNode Value1 { get; set; }
        }

        public class GraphNode
        {
            public string Value2 { get; set; } 
        }

        [Test]
        public void should_serialize_graph()
        {
            var xml = Serializer.Create().Serialize(new Graph { Value1 = new GraphNode { Value2 = "hai" }});
            XDocument.Parse(xml).Element("Graph").Element("Value1").Element("Value2").Value.ShouldEqual("hai"); 
        }

        [Test]
        public void should_exclude_types()
        {
            var xml = Serializer.Create(x => x.ExcludeType<GraphNode>())
                .Serialize(new Graph { Value1 = new GraphNode { Value2 = "hai" } });
            XDocument.Parse(xml).Element("Graph").Element("Value1").ShouldBeNull();
        }

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public GraphNode Value2 { get; set; }
        }

        [Test]
        public void should_serialize_empty_value_when_null()
        {
            var xml = Serializer.Create().Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value1").Value.ShouldBeEmpty(); 
        }

        [Test]
        public void should_exclude_element_when_null()
        {
            var xml = Serializer.Create(x => x.ExcludeNullValues()).Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value1").ShouldBeNull();
        }

        [Test]
        public void should_serialize_empty_type_when_null()
        {
            var xml = Serializer.Create().Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value2").Value.ShouldBeEmpty();
        }

        [Test]
        public void should_exclude_element_when_type_is_null()
        {
            var xml = Serializer.Create(x => x.ExcludeNullValues()).Serialize(new NullValue());
            XDocument.Parse(xml).Element("NullValue").Element("Value2").ShouldBeNull();
        }

        public class CustomFormat
        {
            public DateTime Timestamp { get; set; }
        }

        [Test]
        public void should_serialize_custom_writer()
        {
            var xml = Serializer.Create(x => x.AddWriter<DateTime>((o, p, v) => v.ToString("hh-mm")))
                .Serialize(new CustomFormat { Timestamp = DateTime.MaxValue });
            XDocument.Parse(xml).Element("CustomFormat").Element("Timestamp").Value.ShouldEqual("11-59");   
        }

        [Test]
        public void should_serialize_list()
        {
            var xml = Serializer.Create().Serialize(new List<Graph> {
                new Graph { Value1 = new GraphNode { Value2 = "hai1" } },
                new Graph { Value1 = new GraphNode { Value2 = "hai2" } }});
            var root = XDocument.Parse(xml).Element("ArrayOfGraph").Elements("Graph");
            root.ShouldNotBeNull();
            root.First().Element("Value1").Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value1").Element("Value2").Value.ShouldEqual("hai2"); 
        }

        [Test]
        public void should_serialize_list_with_custom_name_format()
        {
            var xml = Serializer.Create(x => x.WithDefaultGenericListNameFormat("{0}s"))
                .Serialize(new List<Graph> { new Graph(), new Graph()});
            var root = XDocument.Parse(xml).Element("Graphs");
            root.ShouldNotBeNull();
        }

        public class ListProperty
        {
            public List<GraphNode> Items { get; set; }  
        }

        [Test]
        public void should_serialize_list_property()
        {
            var xml = Serializer.Create().Serialize(new ListProperty { 
                Items = new List<GraphNode> {
                    new GraphNode { Value2 = "hai1" },
                    new GraphNode { Value2 = "hai2" }}});
            var root = XDocument.Parse(xml).Element("ListProperty").Element("Items").Elements("GraphNode");
            root.ShouldNotBeNull();
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2"); 
        }

        public class GraphNode<T, T2>
        {
            public T Value2 { get; set; } 
        }

        [Test]
        public void should_serialize_generic_type_name()
        {
            var xml = Serializer.Create().Serialize(new GraphNode<string, int> { Value2 = "hai" });
            XDocument.Parse(xml).Element("GraphNodeOfStringInt32").Element("Value2").Value.ShouldEqual("hai"); 
        }

        public class GenericGraphNodes
        {
            public List<GraphNode<string, int>> Items { get; set; } 
        }

        [Test]
        public void should_serialize_generic_list_item_type_name()
        {
            var xml = Serializer.Create().Serialize(new GenericGraphNodes { Items = new List<GraphNode<string, int>> {
                    new GraphNode<string, int> { Value2 = "hai1" },
                    new GraphNode<string, int> { Value2 = "hai2" }
                } });
            var root = XDocument.Parse(xml).Element("GenericGraphNodes").Element("Items").Elements("GraphNodeOfStringInt32");
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2");
        }

        public class SomeItemsProperty
        {
            public SomeItems Items { get; set; }
        }

        [Test]
        public void should_serialize_inherited_list_type_property()
        {
            var xml = Serializer.Create().Serialize(new SomeItemsProperty
            {
                Items = new SomeItems {
                new GraphNode { Value2 = "hai1" },
                new GraphNode { Value2 = "hai2" }}
            });
            var root = XDocument.Parse(xml).Element("SomeItemsProperty").Element("Items").Elements("GraphNode");
            root.ShouldNotBeNull();
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2");
        }

        public class CircularReference
        {
            public CircularReferenceNode Value { get; set; }
        }

        public class CircularReferenceNode
        {
            public string Value1 { get; set; }
            public CircularReference Value2 { get; set; }
        }

        [Test]
        public void should_not_serialize_circular_references()
        {
            var graph = new CircularReference { Value = new CircularReferenceNode { Value1 = "hai" } };
            graph.Value.Value2 = graph;
            var xml = Serializer.Create().Serialize(graph);
            var root = XDocument.Parse(xml).Element("CircularReference").Element("Value");
            root.Element("Value1").Value.ShouldEqual("hai");
            root.Element("Value2").Value.ShouldBeEmpty();
        }

        [XmlType("SomeType")]
        public class CustomNames
        {
            [XmlElement("SomeElement")]
            public string Value1 { get; set; }
            [XmlIgnore]
            public string Value2 { get; set; }
        }

        [Test]
        public void should_serialize_xml_type_name()
        {
            var xml = Serializer.Create().Serialize(new CustomNames { Value1 = "oh", Value2 = "hai" });
            XDocument.Parse(xml).Element("SomeType").ShouldNotBeNull();
        }

        [Test]
        public void should_serialize_xml_element_name()
        {
            var xml = Serializer.Create().Serialize(new CustomNames { Value1 = "oh", Value2 = "hai" });
            XDocument.Parse(xml).Element("SomeType").Element("SomeElement").ShouldNotBeNull();
        }

        [Test]
        public void should_not_serialize_xml_ignored_members()
        {
            var xml = Serializer.Create().Serialize(new CustomNames { Value1 = "oh", Value2 = "hai" });
            XDocument.Parse(xml).Element("SomeType").Element("Value2").ShouldBeNull();
        }

        public class SomeItems : List<GraphNode> {}

        [Test]
        public void should_serialize_inherited_list_type()
        {
            var xml = Serializer.Create().Serialize(new SomeItems {
                new GraphNode { Value2 = "hai1" },
                new GraphNode { Value2 = "hai2" }});
            var root = XDocument.Parse(xml).Element("SomeItems").Elements("GraphNode");
            root.ShouldNotBeNull();
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2"); 
        }

        [XmlType("SomeCustomName")]
        public class SomeItemsCustomName : List<GraphNode> { }

        [Test]
        public void should_serialize_inherited_list_type_xml_type_name()
        {
            var xml = Serializer.Create().Serialize(new SomeItemsCustomName {
                new GraphNode { Value2 = "hai1" },
                new GraphNode { Value2 = "hai2" }});
            var root = XDocument.Parse(xml).Element("SomeCustomName").Elements("GraphNode");
            root.ShouldNotBeNull();
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2"); 
        }

        public class SomeItemsPropertyCustomName
        {
            [XmlElement("SomeCustomName")]
            public SomeItemsCustomName Items { get; set; }
        }

        [Test]
        public void should_serialize_inherited_list_type_item_xml_type_name()
        {
            var xml = Serializer.Create().Serialize(new SomeItemsPropertyCustomName { Items = new SomeItemsCustomName {
                new GraphNode { Value2 = "hai1" },
                new GraphNode { Value2 = "hai2" }}});
            var root = XDocument.Parse(xml).Element("SomeItemsPropertyCustomName").Element("SomeCustomName").Elements("GraphNode");
            root.ShouldNotBeNull();
            root.First().Element("Value2").Value.ShouldEqual("hai1");
            root.Skip(1).First().Element("Value2").Value.ShouldEqual("hai2"); 
        }

        public class PrettyPrint
        {
            public string Value { get; set; }
        }

        [Test]
        public void should_pretty_format()
        {
            var xml = Serializer.Create(x => x.PrettyPrint()).Serialize(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint>\r\n  <Value>hai</Value>\r\n</PrettyPrint>");
        }

        [Test]
        public void should_not_pretty_format()
        {
            var xml = Serializer.Create().Serialize(new PrettyPrint { Value = "hai" });
            xml.ShouldEqual("<PrettyPrint><Value>hai</Value></PrettyPrint>");
        }

        public class SpeedTestCollection
        {
            public List<SpeedTestItem> Value0 { get; set; } public List<SpeedTestItem> Value1 { get; set; }
            public List<SpeedTestItem> Value2 { get; set; } public List<SpeedTestItem> Value3 { get; set; }
            public List<SpeedTestItem> Value4 { get; set; }
        }

        public class SpeedTestItem
        {
            public string Value0 { get; set; } public string Value1 { get; set; }
            public string Value2 { get; set; } public string Value3 { get; set; }
            public string Value4 { get; set; }
        }

        [Test]
        public void should_be_faster_than_the_fcl_xml_serializer()
        {
            var collection = new List<SpeedTestCollection>();
            collection.AddRange(Enumerable.Range(0, 5).Select(x => new SpeedTestCollection
                {
                    Value0 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value1 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value2 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value3 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList(),
                    Value4 = Enumerable.Range(0, 5).Select(y => new SpeedTestItem {
                            Value0 = "ssdfsfsfd", Value1 = "sfdsfsdf", Value2 = "adasd", Value3 = "wqerqwe", Value4 = "qwerqwer"}).ToList()
                }));

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < 100; i++) Serializer.Create().Serialize(collection);
            stopwatch.Stop();
            var benderSpeed = stopwatch.ElapsedTicks;

            var xmlSerializer = new XmlSerializer(typeof(List<SpeedTestCollection>));
            stopwatch.Start();
            for (var i = 0; i < 100; i++) xmlSerializer.Serialize(new MemoryStream(), collection);
            stopwatch.Stop();
            var xmlSerializerSpeed = stopwatch.ElapsedTicks;

            Debug.WriteLine("Bender speed (ticks): {0:#,##0}", benderSpeed);
            Debug.WriteLine("XmlSerializer speed (ticks): {0:#,##0}", xmlSerializerSpeed);
            (benderSpeed < xmlSerializerSpeed).ShouldBeTrue();
        }
    }
}
