using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bender;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer
{
    [TestFixture]
    public class PerformanceTests
    {
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
            var document = "<ArrayOfSpeedTestCollection></ArrayOfSpeedTestCollection>".ParseXml();
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
            for (var i = 0; i < 100; i++) Bender.Deserializer.Create().Deserialize<List<SpeedTestCollection>>(xml);
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
