using System;
using System.Diagnostics;
using Bender.Nodes;
using Flexo.Extensions;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Performance
{
    public class BenchmarkSerializer : GenericListImpl<BenchmarkSerializer.Timing>
    {
        private readonly Func<object, string> _serializeString;
        private readonly Func<string, Type, object> _deserializeString;
        private readonly Func<string, INode> _parse;
        private readonly Func<INode, Type, object> _deserializeNodes;
        private readonly Func<object, INode> _serializeNodes;
        private readonly Func<INode, string> _encode;

        public BenchmarkSerializer(string name, Format format,
            Func<string, Type, object> deserializeString,
            Func<object, string> serializeString)
        {
            Name = name;
            Format = format;
            _serializeString = serializeString;
            _deserializeString = deserializeString;
            Summary = true;
        }

        public BenchmarkSerializer(string name, Format format,
            Func<string, INode> parse,
            Func<INode, Type, object> deserializeNodes,
            Func<object, INode> serializeNodes,
            Func<INode, string> encode)
        {
            _parse = parse;
            _deserializeNodes = deserializeNodes;
            _serializeNodes = serializeNodes;
            _encode = encode;
            Name = name;
            Format = format;
        }

        public string Name { get; set; }
        public Format Format { get; private set; }
        public bool Summary { get; private set; }

        public void Run(Type type, string source, bool warm = false, bool dryRun = false)
        {
            var results = Summary ? RunSummary(type, source, warm, dryRun) :
                                    RunDetail(type, source, warm, dryRun);

            results.Item1.NormalizeFormatting().ShouldEqual(source.NormalizeFormatting(),
                "{0} {1} output does not match input.".ToFormat(Name, Format.ToString().ToLower()));

            if (!dryRun) Add(results.Item2);
        }

        private Tuple<string, Timing> RunDetail(Type type, string source, bool warm = false, bool dryRun = false)
        {
            var timing = new Timing(source.Length, warm);
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var node = _parse(source);
            stopwatch.Stop();
            timing.Parse = stopwatch.ElapsedTicks;

            stopwatch.Start();
            var @object = _deserializeNodes(node, type);
            stopwatch.Stop();
            timing.Deserialize = stopwatch.ElapsedTicks;

            stopwatch.Restart();
            node = _serializeNodes(@object);
            stopwatch.Stop();
            timing.Serialize = stopwatch.ElapsedTicks;

            stopwatch.Restart();
            var encoded = _encode(node);
            stopwatch.Stop();
            timing.Encode = stopwatch.ElapsedTicks;

            return new Tuple<string, Timing>(encoded, timing);
        }

        private Tuple<string, Timing> RunSummary(Type type, string source, bool warm = false, bool dryRun = false)
        {
            var timing = new Timing(source.Length, warm);
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var @object = _deserializeString(source, type);
            stopwatch.Stop();
            timing.Deserialize = stopwatch.ElapsedTicks;

            stopwatch.Restart();
            var encoded = _serializeString(@object);
            stopwatch.Stop();
            timing.Serialize = stopwatch.ElapsedTicks;

            return new Tuple<string, Timing>(encoded, timing);
        }

        public class Timing
        {
            public Timing(long bytes, bool warm)
            {
                Bytes = bytes;
                Warm = warm;
            }

            public long Bytes { get; private set; }
            public bool Warm { get; private set; }
            public long Parse { get; set; }
            public long Deserialize { get; set; }
            public long Serialize { get; set; }
            public long Encode { get; set; }
        }
    }
}