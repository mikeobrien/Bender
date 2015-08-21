using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Xml;
using fastJSON;
using Newtonsoft.Json;
using NUnit.Framework;
using JsonSerializer = ServiceStack.Text.JsonSerializer;

namespace Tests.Performance
{
    [TestFixture, Ignore]
    public class PerformanceTests
    {
        private IEnumerable<Benchmarks.Benchmark> _xmlResults;
        private IEnumerable<Benchmarks.Benchmark> _jsonResults;

        [TestFixtureSetUp]
        public void Setup()
        {
            var benchmarks = new Benchmarks
            {
                // Json

                new BenchmarkSerializer(Benchmarks.BenderSerializer, Format.Json, 
                    x => new JsonNode(x),
                    (n, t) => Bender.Deserializer.Create().Deserialize(n, t), 
                    x => Bender.Serializer.Create().SerializeNodes(x, (n, o) => 
                        new JsonNode(n.NodeType, new Options()), JsonNode.NodeFormat),
                    x => x.Encode().ReadToEnd()),

                new BenchmarkSerializer("JavaScriptSerializer", Format.Json, 
                    (s, t) => new JavaScriptSerializer().Deserialize(s, t), 
                    x => new JavaScriptSerializer().Serialize(x)),

                new BenchmarkSerializer("DataContractJsonSerializer", Format.Json, 
                    (s, t) => new DataContractJsonSerializer(t).Deserialize(s, t), 
                    x => new DataContractJsonSerializer(x.GetType()).Serialize(x)),

                new BenchmarkSerializer("JSON.NET", Format.Json, 
                    JsonConvert.DeserializeObject, 
                    JsonConvert.SerializeObject),

                new BenchmarkSerializer("ServiceStack", Format.Json, 
                    JsonSerializer.DeserializeFromString, 
                    JsonSerializer.SerializeToString),

                new BenchmarkSerializer("fastJSON", Format.Json, 
                    (s, t) => JSON.Instance.ToObject(s, t), 
                    x => JSON.Instance.ToJSON(x, new JSONParameters { UseExtensions = false })),

                // Xml
                
                new BenchmarkSerializer(Benchmarks.BenderSerializer, Format.Xml,
                    x => ElementNode.Parse(x, Options.Create()),
                    (n, t) => Bender.Deserializer.Create().Deserialize(n, t), 
                    x => Bender.Serializer.Create().SerializeNodes(x, (n, o) => 
                        ElementNode.Create(n.Name, Metadata.Empty, Options.Create()), XmlNodeBase.NodeFormat),
                    x => x.Encode().ReadToEnd()),

                new BenchmarkSerializer("XmlSerializer", Format.Xml, 
                    (s, t) => new XmlSerializer(t).Deserialize(s, t), 
                    x => new XmlSerializer(x.GetType()).Serialize(x)),

                new BenchmarkSerializer("DataContractSerializer", Format.Xml, 
                    (s, t) => new DataContractSerializer(t).Deserialize(s, t), 
                    x => new DataContractSerializer(x.GetType()).Serialize(x)),

                new BenchmarkSerializer("ServiceStack", Format.Xml, 
                    ServiceStack.Text.XmlSerializer.DeserializeFromString, 
                    ServiceStack.Text.XmlSerializer.SerializeToString)
            };

            benchmarks.Run(typeof(Model<>), Format.Xml, "Performance/model.xml");
            benchmarks.Run(typeof(Model<>), Format.Json, "Performance/model.json");

            const string benchmarkPath = "Performance/Benchmarks/";

            Console.Write(benchmarks.GetTextSummaryReport());
            Console.WriteLine();
            Console.Write(benchmarks.GetTextDetailReport());
            Console.WriteLine();
            Console.Write(benchmarks.GetTextDetailReport(benchmarkPath + "Report.Detail.txt"));

            benchmarks.SaveSummaryGraph(benchmarkPath + "All.png");
            benchmarks.SaveSummaryGraph(benchmarkPath + "Cold.png", warm: false);
            benchmarks.SaveSummaryGraph(benchmarkPath + "Warm.png", false);

            benchmarks.SaveJsonSummaryReport(benchmarkPath + "Report.json");
            benchmarks.SaveTextSummaryReport(benchmarkPath + "Report.txt");
            benchmarks.SaveTextDetailReport(benchmarkPath + "Report.Detail.txt");

            var results = benchmarks.GetResults().OrderBy(x => x.Name).ToList();

            _xmlResults = results.Where(x => x.Format == Format.Xml).ToList();
            _jsonResults = results.Where(x => x.Format == Format.Json).ToList();
        }

        [Test]
        public void should_exceed_xml_serializer_benchmark()
        {
            Console.WriteLine(TimeSpan.TicksPerMillisecond);
            //_xmlResults.ForEach(x => Console.WriteLine("{0} {1}: {2} ({3})",
            //    x.Name, x.Format, x.WarmSerialize, x.Bytes));

            //_xmlResults.First(x => x.IsBender).WarmSerialization.ShouldBeLessThan(
            //    _xmlResults.Where(x => !x.IsBender).Min(x => x.WarmSerialization));
        }

        [Test]
        public void should_exceed_xml_deserializer_benchmark()
        {
            _xmlResults.ForEach(x => Console.WriteLine("{0} {1}: {2} ({3})",
                x.Name, x.Format, x.WarmDeserialize, x.Bytes));

            //_xmlResults.First(x => x.IsBender).WarmDeserialization.ShouldBeLessThan(
            //    _xmlResults.Where(x => !x.IsBender).Min(x => x.WarmDeserialization));
        }

        [Test]
        public void should_exceed_json_serializer_benchmark()
        {
            _jsonResults.ForEach(x => Console.WriteLine("{0} {1}: {2} ({3})",
                x.Name, x.Format, x.WarmSerialize, x.Bytes));

            //_jsonResults.First(x => x.IsBender).WarmSerialization.ShouldBeLessThan(
            //    _jsonResults.Where(x => !x.IsBender).Min(x => x.WarmSerialization));
        }

        [Test]
        public void should_exceed_json_deserializer_benchmark()
        {
            _jsonResults.ForEach(x => Console.WriteLine("{0} {1}: {2} ({3})",
                x.Name, x.Format, x.WarmDeserialize, x.Bytes));

            //_jsonResults.First(x => x.IsBender).WarmDeserialization.ShouldBeLessThan(
            //    _jsonResults.Where(x => !x.IsBender).Min(x => x.WarmDeserialization));
        }
    }
}
