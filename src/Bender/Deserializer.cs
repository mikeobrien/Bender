using System;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Bender
{
    public partial class Deserializer
    {
        private readonly Options _options;

        public Deserializer(Options options)
        {
            _options = options;
        }

        public static Deserializer Create(Options options)
        {
            return new Deserializer(options);
        }

        public static Deserializer Create(Action<OptionsDsl> configure = null)
        {
            return new Deserializer(Options.Create(configure));
        }

        public T Deserialize<T>(INode source)
        {
            return (T)Deserialize(source, typeof(T));
        }

        public object Deserialize(INode source, Type type)
        {
            var target = source.IsNamed ?
                NodeFactory.CreateDeserializableRoot(source.Name, type.ToCachedType(), source.Format, _options) :
                NodeFactory.CreateDeserializableRoot(type.ToCachedType(), source.Format, _options);
            new NodeMapper<INode, NodeBase>(
                _options.Deserialization.Readers.Mapping.HasMapping,
                _options.Deserialization.Readers.Mapping.Map,
                _options.Deserialization.Readers.Visitors.HasVisitor,
                _options.Deserialization.Readers.Visitors.Visit)
                .Map(source, target, Mode.Deserialize);
            return target.Value;
        }
    }
}
