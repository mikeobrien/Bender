using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Flexo.Extensions;
using Tests.Collections.Implementations;

namespace Tests.Performance
{
    public enum Format { Json, Xml }

    [DataContract(Namespace = "")]
    public class Dummy
    {
        [DataMember]
        public string Oh { get; set; }
    }

    public class Benchmarks : GenericListImpl<BenchmarkSerializer>
    {
        public const string BenderSerializer = "Bender";

        public void Run<T>(Format format, string path)
        {
            Run(typeof(T), format, path);
        }

        public void Run(Type type, Format format, string path)
        {
            var types = new Stack<Type>(AppDomain.CurrentDomain.GenerateTypes(200)
                .Select(x => type.MakeGenericType(x)));
            var benchmarks = this.Where(x => x.Format == format).ToList();
            var source = File.ReadAllText(path);
            var random = new Random();

            Func<string> generateSource = () => Enumerable.Range(1, 100).Select(x => "%" + x)
                 .Aggregate(source, (a, i) => a.Replace(i, Guid.NewGuid().ToString().Substring(0, random.Next(10, 32))));

            Action<BenchmarkSerializer> dummyRun = x =>
            {
                var dummySource = format == Format.Xml ? "<Dummy><Oh>Hai</Oh></Dummy>" : "{\"Oh\":\"Hai\"}";
                x.Run(typeof (Dummy), dummySource, dryRun: true);
            };

            Enumerable.Range(1, 20).ForEach(x => benchmarks.ForEach(y =>
            {
                var newType = types.Pop();
                dummyRun(y);
                y.Run(newType, generateSource()); // Cold
                Enumerable.Range(1, 20).ForEach(z =>
                {
                    dummyRun(y);
                    y.Run(newType, generateSource(), true); // Warm
                }); 
            }));
        }

        public IEnumerable<Benchmark> GetResults()
        {
            return this
                .SelectMany(x => x.Select(y => new
                {
                    x.Name,
                    x.Format,
                    x.Summary,
                    y.Bytes,
                    y.Warm,
                    y.Parse,
                    y.Deserialize,
                    y.Serialize,
                    y.Encode
                }))
                .GroupBy(x => "{0}-{1}".ToFormat(x.Name, x.Format))
                .Select(x => new Benchmark(
                    x.First().Name,
                    x.First().Format,
                    x.First().Summary)
                {
                    Bytes = (long)x.Average(y => y.Bytes),
                    ColdSerialize = (long)x.Where(y => !y.Warm).Average(y => y.Serialize),
                    ColdDeserialize = (long)x.Where(y => !y.Warm).Average(y => y.Deserialize),
                    WarmSerialize = (long)x.Where(y => y.Warm).Average(y => y.Serialize),
                    WarmDeserialize = (long)x.Where(y => y.Warm).Average(y => y.Deserialize),
                    ColdEncode = (long)x.Where(y => !y.Warm).Average(y => y.Encode),
                    ColdParse = (long)x.Where(y => !y.Warm).Average(y => y.Parse),
                    WarmEncode = (long)x.Where(y => y.Warm).Average(y => y.Encode),
                    WarmParse = (long)x.Where(y => y.Warm).Average(y => y.Parse)
                });
        }

        public void SaveSummaryGraph(string path, bool cold = true, bool warm = true)
        {
            BenchmarkChart.SaveSummary(GetResults(), Path.Combine("../../", path), cold, warm);
        }

        public void SaveJsonSummaryReport(string path)
        {
            File.WriteAllText(Path.Combine("../../", path), GetJsonSummaryReport());
        }

        public void SaveTextSummaryReport(string path)
        {
            File.WriteAllText(Path.Combine("../../", path), GetTextSummaryReport());
        }

        public void SaveTextDetailReport(string path)
        {
            File.WriteAllText(Path.Combine("../../", path), GetTextDetailReport());
        }

        public string GetTextDetailReport(string path)
        {
            path = Path.Combine("../../", path);
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }

        public string GetJsonSummaryReport()
        {
            var report = new StringBuilder();
            report.AppendLine("[");
            GetResults().OrderBy(x => x.Name).ForEach(x => report.AppendLine((
                "\t{{ \"Name\": \"{0}\", " + 
                    "\"Cold\": {{ \"Deserialization\": {1}, \"Serialization\": {2} }}, " + 
                    "\"Warm\": {{ \"Deserialization\": {3}, \"Serialization\": {4} }} }}").ToFormat(
                "{0} {1}".ToFormat(x.Name, x.Format),
                x.ColdDeserialize + x.ColdParse,
                x.ColdSerialize + x.ColdEncode,
                x.WarmDeserialize + x.WarmParse,
                x.WarmSerialize + x.WarmEncode)));
            report.AppendLine("]");
            return report.ToString();
        }

        public string GetTextSummaryReport()
        {
            var results = GetResults().OrderBy(x => x.WarmDeserialize + x.WarmParse + x.WarmEncode + x.WarmSerialize).Select(x => 
                new { x.Format, Results = "{0} | {1} | {2} | {3} | {4} |".ToFormat(
                "{0} {1}".ToFormat(x.Name, x.Format).PadRight(31),
                (x.ColdDeserialize + x.ColdParse).ToString("#,###").PadLeft(11),
                (x.ColdSerialize + x.ColdEncode).ToString("#,###").PadLeft(11),
                (x.WarmDeserialize + x.WarmParse).ToString("#,###").PadLeft(11),
                (x.WarmSerialize + x.WarmEncode).ToString("#,###").PadLeft(11))});
            var report = new StringBuilder();
            report.AppendLine("                                ---------------------------------------------------------");
            report.AppendLine("                                |           Cold            |           Warm            |");
            report.AppendLine("                                | Deserialize |  Serialize  | Deserialize |  Serialize  |");
            report.AppendLine("-----------------------------------------------------------------------------------------");
            results.Where(x => x.Format == Format.Json).ForEach(x => report.AppendLine(x.Results));
            report.AppendLine("-----------------------------------------------------------------------------------------");
            results.Where(x => x.Format == Format.Xml).ForEach(x => report.AppendLine(x.Results));
            report.AppendLine("-----------------------------------------------------------------------------------------");
            return report.ToString();
        }

        public string GetTextDetailReport()
        {
            var report = new StringBuilder();
            const string detailFormat = "{0:#,###} -> {1:#,###}";
            report.AppendLine("            -------------------------------------------------------------------------------------------");
            report.AppendLine("            |                    Cold                    |                    Warm                    |");
            report.AppendLine("            | Parse -> Deserialize | Serialize -> Encode | Parse -> Deserialize | Serialize -> Encode |");
            report.AppendLine("-------------------------------------------------------------------------------------------------------");
            GetResults().Where(x => !x.Summary).OrderBy(x => x.Name).ForEach(x => report.AppendLine("{0} | {1} | {2} | {3} | {4} |".ToFormat(
                "{0} {1}".ToFormat(x.Name, x.Format).PadRight(11),
                detailFormat.ToFormat(x.ColdParse, x.ColdDeserialize).PadLeft(20),
                detailFormat.ToFormat(x.ColdSerialize, x.ColdEncode).PadLeft(19),
                detailFormat.ToFormat(x.WarmParse, x.WarmDeserialize).PadLeft(20),
                detailFormat.ToFormat(x.WarmSerialize, x.WarmEncode).PadLeft(19))));
            report.AppendLine("-------------------------------------------------------------------------------------------------------");
            return report.ToString();
        }

        public class Benchmark
        {
            public Benchmark(string name, Format format, bool summary)
            {
                Name = name;
                Format = format;
                Summary = summary;
            }

            public string Name { get; private set; }
            public Format Format { get; private set; }
            public bool Summary { get; private set; }
            public long Bytes { get; set; }
            public bool IsBender { get { return Name == BenderSerializer; } }

            public long ColdSerialize { get; set; }
            public long ColdDeserialize { get; set; }
            public long WarmSerialize { get; set; }
            public long WarmDeserialize { get; set; }

            public long ColdEncode { get; set; }
            public long ColdParse { get; set; }
            public long WarmEncode { get; set; }
            public long WarmParse { get; set; }
        }
    }
}