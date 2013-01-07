using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class DeserializerTests
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
            public object Object { get; set; }
            public object BoxedValue { get; set; }
        }

        [Test]
        public void should_deserialize_simple_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String>hai</String>
                      <Boolean>true</Boolean><NullableBoolean>true</NullableBoolean>
                      <Byte>1</Byte><NullableByte>1</NullableByte>
                      <ByteArray>AQID</ByteArray>
                      <UnsignedByte>2</UnsignedByte><NullableUnsignedByte>2</NullableUnsignedByte>
                      <Short>3</Short><NullableShort>3</NullableShort>
                      <UnsignedShort>4</UnsignedShort><NullableUnsignedShort>4</NullableUnsignedShort>
                      <Integer>5</Integer><NullableInteger>5</NullableInteger>
                      <UnsignedInteger>6</UnsignedInteger><NullableUnsignedInteger>6</NullableUnsignedInteger>
                      <Long>7</Long><NullableLong>7</NullableLong>
                      <UnsignedLong>8</UnsignedLong><NullableUnsignedLong>8</NullableUnsignedLong>
                      <Float>1.1</Float><NullableFloat>1.1</NullableFloat>
                      <Double>1.2</Double><NullableDouble>1.2</NullableDouble>
                      <Decimal>1.3</Decimal><NullableDecimal>1.3</NullableDecimal>
                      <DateTime>12/31/9999 11:59:59.9999999 PM</DateTime><NullableDateTime>12/31/9999 11:59:59.9999999 PM</NullableDateTime>
                      <TimeSpan>10675199.02:48:05.4775807</TimeSpan><NullableTimeSpan>10675199.02:48:05.4775807</NullableTimeSpan>
                      <Guid>00000000-0000-0000-0000-000000000000</Guid><NullableGuid>00000000-0000-0000-0000-000000000000</NullableGuid>
                      <Enum>Value2</Enum><NullableEnum>Value2</NullableEnum>
                      <Uri>http://www.google.com/</Uri>
                      <Object><Enum>Value2</Enum><Enum>Value2</Enum></Object>
                      <BoxedValue>00000000-0000-0000-0000-000000000000</BoxedValue>
                </SimpleTypes>";
            var result = Deserializer.Create(x => x.IgnoreUnmatchedElements()).Deserialize<SimpleTypes>(xml);
            result.String.ShouldEqual("hai");
            result.Boolean.ShouldBeTrue();
            result.NullableBoolean.Value.ShouldBeTrue();
            result.Byte.ShouldEqual<byte>(1);
            result.NullableByte.Value.ShouldEqual<byte>(1);
            result.ByteArray.ShouldEqual(new byte[] { 1, 2, 3 });
            result.UnsignedByte.ShouldEqual<sbyte>(2);
            result.NullableUnsignedByte.Value.ShouldEqual<sbyte>(2);
            result.Short.ShouldEqual<short>(3);
            result.NullableShort.Value.ShouldEqual<short>(3);
            result.UnsignedShort.ShouldEqual<ushort>(4);
            result.NullableUnsignedShort.Value.ShouldEqual<ushort>(4);
            result.Integer.ShouldEqual(5);
            result.NullableInteger.Value.ShouldEqual(5);
            result.UnsignedInteger.ShouldEqual<uint>(6);
            result.NullableUnsignedInteger.Value.ShouldEqual<uint>(6);
            result.Long.ShouldEqual(7);
            result.NullableLong.Value.ShouldEqual(7);
            result.UnsignedLong.ShouldEqual<ulong>(8);
            result.NullableUnsignedLong.Value.ShouldEqual<ulong>(8);
            result.Float.ShouldEqual(1.1F);
            result.NullableFloat.Value.ShouldEqual(1.1F);
            result.Double.ShouldEqual(1.2);
            result.NullableDouble.Value.ShouldEqual(1.2);
            result.Decimal.ShouldEqual(1.3M);
            result.NullableDecimal.Value.ShouldEqual(1.3M);
            result.DateTime.ShouldEqual(DateTime.MaxValue);
            result.NullableDateTime.Value.ShouldEqual(DateTime.MaxValue);
            result.TimeSpan.ShouldEqual(TimeSpan.MaxValue);
            result.NullableTimeSpan.Value.ShouldEqual(TimeSpan.MaxValue);
            result.Guid.ShouldEqual(Guid.Empty);
            result.NullableGuid.Value.ShouldEqual(Guid.Empty);
            result.Enum.ShouldEqual(Enum.Value2);
            result.NullableEnum.Value.ShouldEqual(Enum.Value2);
            result.Uri.ShouldEqual(new Uri("http://www.google.com"));
            result.Object.ShouldNotBeNull();
            result.Object.ShouldBeType<XElement>();
            result.BoxedValue.ShouldNotBeNull();
            result.BoxedValue.ShouldBeType<XElement>();
        }

        [Test]
        public void should_deserialize_simple_empty_types()
        {
            const string xml =
                @"<SimpleTypes>
                      <String></String>
                      <Boolean></Boolean><NullableBoolean></NullableBoolean>
                      <Byte></Byte><NullableByte></NullableByte>
                      <ByteArray></ByteArray>
                      <UnsignedByte></UnsignedByte><NullableUnsignedByte></NullableUnsignedByte>
                      <Short></Short><NullableShort></NullableShort>
                      <UnsignedShort></UnsignedShort><NullableUnsignedShort></NullableUnsignedShort>
                      <Integer></Integer><NullableInteger></NullableInteger>
                      <UnsignedInteger></UnsignedInteger><NullableUnsignedInteger></NullableUnsignedInteger>
                      <Long></Long><NullableLong></NullableLong>
                      <UnsignedLong></UnsignedLong><NullableUnsignedLong></NullableUnsignedLong>
                      <Float></Float><NullableFloat></NullableFloat>
                      <Double></Double><NullableDouble></NullableDouble>
                      <Decimal></Decimal><NullableDecimal></NullableDecimal>
                      <DateTime></DateTime><NullableDateTime></NullableDateTime>
                      <TimeSpan></TimeSpan><NullableTimeSpan></NullableTimeSpan>
                      <Guid></Guid><NullableGuid></NullableGuid>
                      <Enum></Enum><NullableEnum></NullableEnum>
                      <Uri></Uri>
                </SimpleTypes>";
            var result = Deserializer.Create(x => x.DefaultNonNullableTypesWhenEmpty()).Deserialize<SimpleTypes>(xml);
            result.String.ShouldBeEmpty();
            result.Boolean.ShouldBeFalse();
            result.NullableBoolean.HasValue.ShouldBeFalse();
            result.Byte.ShouldEqual<byte>(0);
            result.NullableByte.HasValue.ShouldBeFalse();
            result.ByteArray.ShouldEqual(new byte[] { });
            result.UnsignedByte.ShouldEqual<sbyte>(0);
            result.NullableUnsignedByte.HasValue.ShouldBeFalse();
            result.Short.ShouldEqual<short>(0);
            result.NullableShort.HasValue.ShouldBeFalse();
            result.UnsignedShort.ShouldEqual<ushort>(0);
            result.NullableUnsignedShort.HasValue.ShouldBeFalse();
            result.Integer.ShouldEqual(0);
            result.NullableInteger.HasValue.ShouldBeFalse();
            result.UnsignedInteger.ShouldEqual<uint>(0);
            result.NullableUnsignedInteger.HasValue.ShouldBeFalse();
            result.Long.ShouldEqual(0);
            result.NullableLong.HasValue.ShouldBeFalse();
            result.UnsignedLong.ShouldEqual<ulong>(0);
            result.NullableUnsignedLong.HasValue.ShouldBeFalse();
            result.Float.ShouldEqual(0);
            result.NullableFloat.HasValue.ShouldBeFalse();
            result.Double.ShouldEqual(0);
            result.NullableDouble.HasValue.ShouldBeFalse();
            result.Decimal.ShouldEqual(0);
            result.NullableDecimal.HasValue.ShouldBeFalse();
            result.DateTime.ShouldEqual(DateTime.MinValue);
            result.NullableDateTime.HasValue.ShouldBeFalse();
            result.TimeSpan.ShouldEqual(TimeSpan.MinValue);
            result.NullableTimeSpan.HasValue.ShouldBeFalse();
            result.Guid.ShouldEqual(Guid.Empty);
            result.NullableGuid.HasValue.ShouldBeFalse();
            result.Enum.ShouldEqual(Enum.Value1);
            result.NullableEnum.HasValue.ShouldBeFalse();
            result.Uri.ShouldEqual(new Uri("http://tempuri.org/"));
        }

        [Test]
        public void should_throw_format_exception_when_value_is_empty_and_not_set_to_use_default()
        {
            const string xml = @"<SimpleTypes><Boolean></Boolean></SimpleTypes>";
            Assert.Throws<FormatException>(() => Deserializer.Create().Deserialize<SimpleTypes>(xml));
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
        public void should_deserialize_graph_with_element_values()
        {
            const string xml = @"<Graph><Value1><Value2>hai</Value2></Value1></Graph>";
            Deserializer.Create().Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_deserialize_graph_with_attribute_values()
        {
            const string xml = @"<Graph><Value1 Value2=""hai""/></Graph>";
            Deserializer.Create().Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("hai");
        }

        public class ComplexAttribute
        {
            public Version Version { get; set; } 
        }

        [Test]
        public void should_not_traverse_attribute_with_complex_type_without_a_reader()
        {
            const string xml = @"<ComplexAttribute Version=""1.1.1.1""/>";
            Deserializer.Create().Deserialize<ComplexAttribute>(xml).Version.ShouldBeNull();
        }

        [Test]
        public void should_deserialize_empty_element()
        {
            const string xml = @"<Graph><Value1 /></Graph>";
            Deserializer.Create().Deserialize<Graph>(xml).Value1.ShouldNotBeNull();
        }

        [Test]
        public void should_deserialize_graph_case_insensitively()
        {
            const string xml = @"<graph><value1><value2>hai</value2></value1></graph>";
            Deserializer.Create(x => x.IgnoreCase()).Deserialize<Graph>(xml).Value1.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_not_deserialize_graph_case_insensitively()
        {
            const string xml = @"<graph><value1><value2>hai</value2></value1></graph>";
            Assert.Throws<UnmatchedNodeException>(() => Deserializer.Create().Deserialize<Graph>(xml));
        }

        [Test]
        public void should_exclude_types()
        {
            const string xml = @"<Graph><Value1><Value2>hai</Value2></Value1></Graph>";
            Deserializer.Create(x => x.ExcludeType<GraphNode>()).Deserialize<Graph>(xml).Value1.ShouldBeNull();
        }

        public class NullValue
        {
            public DateTime? Value1 { get; set; }
            public GraphNode Value2 { get; set; }
            public int Value3 { get; set; }
        }

        [Test]
        public void should_set_type_with_missing_element_to_null()
        {
            Deserializer.Create().Deserialize<NullValue>("<NullValue></NullValue>").Value2.ShouldBeNull();
        }

        [Test]
        public void should_set_value_with_missing_element_to_default()
        {
            Deserializer.Create().Deserialize<NullValue>("<NullValue></NullValue>").Value3.ShouldEqual(0);
        }

        [Test]
        public void should_set_nullable_value_with_missing_element_to_null()
        {
            Deserializer.Create().Deserialize<NullValue>("<NullValue></NullValue>").Value1.ShouldBeNull();
        }

        public class CustomFormat
        {
            public DateTime Timestamp { get; set; }
        }

        [Test]
        public void should_deserialize_custom_reader()
        {
            const string xml = @"<CustomFormat><Timestamp>06197805</Timestamp></CustomFormat>";
            Deserializer.Create(x => x.AddReader<DateTime>(
                    (o, p, v) => DateTime.ParseExact(v.Value, "MMyyyydd", CultureInfo.InvariantCulture)))
                .Deserialize<CustomFormat>(xml).Timestamp.ShouldEqual(DateTime.Parse("6/5/1978"));
        }

        [Test]
        public void should_deserialize_list()
        {
            const string xml =
                @"<ArrayOfGraph>
                    <Graph><Value1><Value2>hai1</Value2></Value1></Graph>
                    <Graph><Value1><Value2>hai2</Value2></Value1></Graph>
                </ArrayOfGraph>";
            var result = Deserializer.Create().Deserialize<List<Graph>>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value1.Value2.ShouldEqual("hai1");
            result[1].Value1.Value2.ShouldEqual("hai2");
        }

        public class ListProperty
        {
            public List<GraphNode> Items { get; set; }
        }

        [Test]
        public void should_deserialize_list_property()
        {
            const string xml =
                @"<ListProperty>
                    <Items>
                        <GraphNode><Value2>hai1</Value2></GraphNode>
                        <GraphNode><Value2>hai2</Value2></GraphNode>
                    </Items>
                </ListProperty>";
            var result = Deserializer.Create().Deserialize<ListProperty>(xml).Items;
            result.Count.ShouldEqual(2);
            result[0].Value2.ShouldEqual("hai1");
            result[1].Value2.ShouldEqual("hai2");
        }

        public class GraphNode<T, T2>
        {
            public T Value2 { get; set; }
        }

        [Test]
        public void should_deserialize_generic_type()
        {
            const string xml = @"<GraphNodeOfInt32String><Value2>67</Value2></GraphNodeOfInt32String>";
            var result = Deserializer.Create().Deserialize<GraphNode<int, string>>(xml);
            result.Value2.ShouldEqual(67);
        }

        public class ParentNode
        {
            public int Age { get; set; }
            public ChildNode Child { get; set; }
        }

        public class ChildNode
        {
            private readonly ParentNode _parent;
            private string _name;

            public ChildNode(ParentNode parent)
            {
                _parent = parent;
            }

            public string Name { get { return _name + " (" + _parent.Age + ")"; } set { _name = value; } }
        }

        [Test]
        public void should_deserialize_child_with_parent_passed_into_the_constructor()
        {
            const string xml = @"<ParentNode><Age>67</Age><Child><Name>Ed</Name></Child></ParentNode>";
            var result = Deserializer.Create().Deserialize<ParentNode>(xml);
            result.Child.Name.ShouldEqual("Ed (67)");
        }

        public class SomeItemsProperty
        {
            public SomeItems Items { get; set; }
        }

        [Test]
        public void should_deserialize_inherited_list_type_property()
        {
            const string xml =
                @"<ListProperty>
                    <Items>
                        <GraphNode><Value2>hai1</Value2></GraphNode>
                        <GraphNode><Value2>hai2</Value2></GraphNode>
                    </Items>
                </ListProperty>";
            var result = Deserializer.Create().Deserialize<ListProperty>(xml).Items;
            result.Count.ShouldEqual(2);
            result[0].Value2.ShouldEqual("hai1");
            result[1].Value2.ShouldEqual("hai2");
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
        public void should_deserialize_xml_type_name()
        {
            const string xml =
                @"<ArrayOfSomeType>
                    <SomeType></SomeType>
                    <SomeType></SomeType>
                </ArrayOfSomeType>";
            var result = Deserializer.Create().Deserialize<List<CustomNames>>(xml);
            result.Count.ShouldEqual(2);
        }

        [Test]
        public void should_deserialize_xml_element_name()
        {
            const string xml = @"<SomeType><SomeElement>hai</SomeElement></SomeType>";
            var result = Deserializer.Create().Deserialize<CustomNames>(xml);
            result.Value1.ShouldEqual("hai");
        }

        [Test]
        public void should_not_deserialize_xml_ignored_members()
        {
            const string xml = @"<SomeType><Value2>hai</Value2></SomeType>";
            var result = Deserializer.Create().Deserialize<CustomNames>(xml);
            result.Value2.ShouldBeNull();
        }

        public class SomeItems : List<GraphNode> { }

        [Test]
        public void should_deserialize_inherited_list_type()
        {
            const string xml =
                @"<SomeItems>
                    <GraphNode><Value2>hai1</Value2></GraphNode>
                    <GraphNode><Value2>hai2</Value2></GraphNode>
                </SomeItems>";
            var result = Deserializer.Create().Deserialize<SomeItems>(xml);
            result.Count.ShouldEqual(2);
            result[0].Value2.ShouldEqual("hai1");
            result[1].Value2.ShouldEqual("hai2");
        }

        [Test]
        public void should_not_fail_on_unmatched_elements()
        {
            const string xml = @"<GraphNode><yada>hai</yada></GraphNode>";
            GraphNode result = null;
            Assert.DoesNotThrow(() => result = Deserializer.Create(x => x.IgnoreUnmatchedElements()).Deserialize<GraphNode>(xml));
            result.ShouldNotBeNull();
            result.Value2.ShouldBeNull();
        }

        [Test]
        public void should_fail_on_unmatched_elements()
        {
            const string xml = @"<GraphNode><yada>hai</yada></GraphNode>";
            Assert.Throws<UnmatchedNodeException>(() => Deserializer.Create().Deserialize<GraphNode>(xml));
        }

        [Test]
        public void should_not_fail_on_unmatched_root_element()
        {
            const string xml = @"<root><Value2>hai</Value2></root>";
            GraphNode result = null;
            Assert.DoesNotThrow(() => result = Deserializer.Create(x => x.IgnoreTypeElementNames()).Deserialize<GraphNode>(xml));
            result.ShouldNotBeNull();
            result.Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_fail_on_unmatched_root_element()
        {
            const string xml = @"<root><Value2>hai</Value2></root>";
            Assert.Throws<UnmatchedNodeException>(() => Deserializer.Create().Deserialize<GraphNode>(xml));
        }

        [Test]
        public void should_not_fail_on_unmatched_list_element()
        {
            const string xml = @"<ArrayOfGraphNode><yada><Value2>hai</Value2></yada></ArrayOfGraphNode>";
            List<GraphNode> results = null;
            Assert.DoesNotThrow(() => results = Deserializer.Create(x => x.IgnoreTypeElementNames()).Deserialize<List<GraphNode>>(xml));
            results.ShouldNotBeNull();
            results.Count.ShouldEqual(1);
            results[0].Value2.ShouldEqual("hai");
        }

        [Test]
        public void should_fail_on_unmatched_list_element()
        {
            const string xml = @"<ArrayOfGraphNode><yada><Value2>hai</Value2></yada></ArrayOfGraphNode>";
            Assert.Throws<UnmatchedNodeException>(() => Deserializer.Create().Deserialize<List<GraphNode>>(xml));
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
        public void should_be_faster_than_the_fcl_xml_deserializer()
        {
            var document = XDocument.Parse("<ArrayOfSpeedTestCollection></ArrayOfSpeedTestCollection>");
            document.Root.Add(Enumerable.Range(0, 5).Select(x => new XElement("SpeedTestCollection",
                    new XElement("Value0", Enumerable.Range(0, 5).Select(y => new XElement("SpeedTestItem",
                            new XElement("Value0", "ssdfsfsfd"), new XElement("Value1", "sfdsfsdf"), new XElement("Value2", "adasd"), new XElement("Value3", "wqerqwe"), new XElement("Value4", "qwerqwer")))),
                    new XElement("Value1", Enumerable.Range(0, 5).Select(y => new XElement("SpeedTestItem",
                            new XElement("Value0", "ssdfsfsfd"), new XElement("Value1", "sfdsfsdf"), new XElement("Value2", "adasd"), new XElement("Value3", "wqerqwe"), new XElement("Value4", "qwerqwer")))),
                    new XElement("Value2", Enumerable.Range(0, 5).Select(y => new XElement("SpeedTestItem",
                            new XElement("Value0", "ssdfsfsfd"), new XElement("Value1", "sfdsfsdf"), new XElement("Value2", "adasd"), new XElement("Value3", "wqerqwe"), new XElement("Value4", "qwerqwer")))),
                    new XElement("Value3", Enumerable.Range(0, 5).Select(y => new XElement("SpeedTestItem",
                            new XElement("Value0", "ssdfsfsfd"), new XElement("Value1", "sfdsfsdf"), new XElement("Value2", "adasd"), new XElement("Value3", "wqerqwe"), new XElement("Value4", "qwerqwer")))),
                    new XElement("Value4", Enumerable.Range(0, 5).Select(y => new XElement("SpeedTestItem",
                            new XElement("Value0", "ssdfsfsfd"), new XElement("Value1", "sfdsfsdf"), new XElement("Value2", "adasd"), new XElement("Value3", "wqerqwe"), new XElement("Value4", "qwerqwer"))))
                )));

            var xml = document.ToString();
            
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < 100; i++) Deserializer.Create().Deserialize<List<SpeedTestCollection>>(xml);
            stopwatch.Stop();
            var benderSpeed = stopwatch.ElapsedTicks;

            var xmlSerializer = new XmlSerializer(typeof(List<SpeedTestCollection>));
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(xml));
            stopwatch.Start();
            for (var i = 0; i < 100; i++)
            {
                stream.Position = 0;
                xmlSerializer.Deserialize(stream);
            }
            stopwatch.Stop();
            var xmlSerializerSpeed = stopwatch.ElapsedTicks;

            Debug.WriteLine("Bender speed (ticks): {0:#,##0}", benderSpeed);
            Debug.WriteLine("XmlSerializer speed (ticks): {0:#,##0}", xmlSerializerSpeed);
            (benderSpeed < xmlSerializerSpeed).ShouldBeTrue();
        }
    }
}
