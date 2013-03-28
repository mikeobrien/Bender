using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Should;

namespace Tests.Serializer
{
    [TestFixture]
    public class WriterTests
    {
        public class ComplexType { public int Value { get; set; } }

        public class CustomWriter
        {
            public DateTime SimpleType { get; set; }
            public ComplexType ComplexType { get; set; }
            public List<int> ListOfSimpleTypes { get; set; }
            public List<ComplexType> ListOfComplexTypes { get; set; }
            public IList<int> ListInterfaceOfSimpleTypes { get; set; }
            public IList<ComplexType> ListInterfaceOfComplexTypes { get; set; }
        }

        [Test]
        public void should_serialize_simple_type_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<DateTime>((o, p, v, e) => e.Value = v.ToString("hh-mm")))
                .Serialize(new CustomWriter { SimpleType = DateTime.MaxValue });
            XDocument.Parse(xml).Element("CustomWriter").Element("SimpleType").Value.ShouldEqual("11-59");   
        }

        [Test]
        public void should_serialize_complex_type_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>((o, p, v, e) => e.Value = v.Value.ToString()))
                .Serialize(new CustomWriter { ComplexType = new ComplexType { Value = 5 } });
            XDocument.Parse(xml).Element("CustomWriter").Element("ComplexType").Value.ShouldEqual("5");
        }

        [Test]
        public void should_serialize_complex_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<List<ComplexType>>((o, p, v, e) => 
                    e.Value = string.Join(",", v.Select(y => y.Value.ToString()).ToArray())))
                .Serialize(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            XDocument.Parse(xml).Element("CustomWriter").Element("ListOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<List<int>>((o, p, v, e) => 
                    e.Value = string.Join(",", v.Select(y => y.ToString()).ToArray())))
                .Serialize(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            XDocument.Parse(xml).Element("CustomWriter").Element("ListOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>((o, p, v, e) => { if (v != null) e.Value = string.Format("[{0}]", v.Value); }))
                .Serialize(new CustomWriter { ListOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = XDocument.Parse(xml).Element("CustomWriter").Element("ListOfComplexTypes").Elements("ComplexType");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<int>((o, p, v, e) => e.Value = string.Format("[{0}]", v)))
                .Serialize(new CustomWriter { ListOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = XDocument.Parse(xml).Element("CustomWriter").Element("ListOfSimpleTypes").Elements("Int32");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<IList<ComplexType>>((o, p, v, e) => 
                    e.Value = string.Join(",", v.Select(y => y.Value.ToString()).ToArray())))
                .Serialize(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            XDocument.Parse(xml).Element("CustomWriter").Element("ListInterfaceOfComplexTypes").Value.ShouldEqual("1,2,3");   
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<IList<int>>((o, p, v, e) =>
                    e.Value = string.Join(",", v.Select(y => y.ToString()).ToArray())))
                .Serialize(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            XDocument.Parse(xml).Element("CustomWriter").Element("ListInterfaceOfSimpleTypes").Value.ShouldEqual("1,2,3"); 
        }

        [Test]
        public void should_serialize_complex_type_list_interface_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<ComplexType>((o, p, v, e) => { if (v != null) e.Value = string.Format("[{0}]", v.Value); }))
                .Serialize(new CustomWriter { ListInterfaceOfComplexTypes = new List<ComplexType>
                    {
                        new ComplexType { Value = 1 }, new ComplexType { Value = 2 }, new ComplexType { Value = 3 }
                    } });
            var results = XDocument.Parse(xml).Element("CustomWriter").Element("ListInterfaceOfComplexTypes").Elements("ComplexType");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }

        [Test]
        public void should_serialize_simple_type_list_interface_property_item_with_custom_writer()
        {
            var xml = Bender.Serializer.Create(x => x.AddWriter<int>((o, p, v, e) => e.Value = string.Format("[{0}]", v)))
                .Serialize(new CustomWriter { ListInterfaceOfSimpleTypes = new List<int> { 1, 2, 3 } });
            var results = XDocument.Parse(xml).Element("CustomWriter").Element("ListInterfaceOfSimpleTypes").Elements("Int32");
            results.First().Value.ShouldEqual("[1]");
            results.Skip(1).First().Value.ShouldEqual("[2]");
            results.Skip(2).First().Value.ShouldEqual("[3]"); 
        }
    }
}
