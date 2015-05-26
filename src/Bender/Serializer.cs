using System;
using System.IO;
using System.Text;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;
using UTF8Encoding = Bender.Extensions.UTF8Encoding;

namespace Bender
{
    public partial class Serializer
    {
        private readonly Options _options;

        public Serializer(Options options)
        {
            _options = options;
        }

        public static Serializer Create(Options options)
        {
            return new Serializer(options);
        }

        public static Serializer Create(Action<OptionsDsl> configure = null)
        {
            return new Serializer(Options.Create(configure));
        }

        // String

        public string SerializeString<T>(T source, INode target)
        {
            return SerializeString(source, typeof(T), target);
        }

        public string SerializeString<T>(T source, Func<INode, Options, INode> targetFactory, string format)
        {
            return SerializeString(source, typeof(T), targetFactory, format);
        }

        public string SerializeString(object source, INode target)
        {
            return SerializeString(source, source.GetType(), target);
        }

        public string SerializeString(object source, Func<INode, Options, INode> targetFactory, string format)
        {
            return SerializeString(source, source.GetType(), targetFactory, format);
        }

        public string SerializeString(object @object, Type type, INode target)
        {
            return SerializeNodes(@object, type, target)
                .Encode(pretty: _options.Serialization.PrettyPrint).ReadToEnd();
        }

        public string SerializeString(object @object, Type type, Func<INode, Options, INode> targetFactory, string format)
        {
            return SerializeNodes(@object, type, targetFactory, format)
                .Encode(pretty: _options.Serialization.PrettyPrint).ReadToEnd();
        }

        // Bytes

        public byte[] SerializeBytes<T>(T source, INode target, Encoding encoding = null)
        {
            return SerializeBytes(source, typeof(T), target, encoding);
        }

        public byte[] SerializeBytes<T>(T source, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeBytes(source, typeof(T), targetFactory, format, encoding);
        }

        public byte[] SerializeBytes(object source, INode target, Encoding encoding = null)
        {
            return SerializeBytes(source, source.GetType(), target, encoding);
        }

        public byte[] SerializeBytes(object source, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeBytes(source, source.GetType(), targetFactory, format, encoding);
        }

        public byte[] SerializeBytes(object @object, Type type, INode target, Encoding encoding = null)
        {
            return SerializeNodes(@object, type, target)
                .Encode(encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint).ReadAllBytes();
        }

        public byte[] SerializeBytes(object @object, Type type, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeNodes(@object, type, targetFactory, format)
                .Encode(encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint).ReadAllBytes();
        }

        // Return Stream

        public Stream SerializeStream<T>(T source, INode target, Encoding encoding = null)
        {
            return SerializeStream(source, typeof(T), target, encoding);
        }

        public Stream SerializeStream<T>(T source, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeStream(source, typeof(T), targetFactory, format, encoding);
        }

        public Stream SerializeStream(object source, INode target, Encoding encoding = null)
        {
            return SerializeStream(source, source.GetType(), target, encoding);
        }

        public Stream SerializeStream(object source, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeStream(source, source.GetType(), targetFactory, format, encoding);
        }

        public Stream SerializeStream(object @object, Type type, INode target, Encoding encoding = null)
        {
            return SerializeNodes(@object, type, target)
                .Encode(encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint);
        }

        public Stream SerializeStream(object @object, Type type, Func<INode, Options, INode> targetFactory, string format, Encoding encoding = null)
        {
            return SerializeNodes(@object, type, targetFactory, format)
                .Encode(encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint);
        }

        // To Stream

        public void SerializeStream<T>(T source, INode target, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, typeof(T), target, stream, encoding);
        }

        public void SerializeStream<T>(T source, Func<INode, Options, INode> targetFactory, string format, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, typeof(T), targetFactory, format, stream, encoding);
        }

        public void SerializeStream(object source, INode target, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, source.GetType(), target, stream, encoding);
        }

        public void SerializeStream(object source, Func<INode, Options, INode> targetFactory, string format, Stream stream, Encoding encoding = null)
        {
            SerializeStream(source, source.GetType(), targetFactory, format, stream, encoding);
        }

        public void SerializeStream(object @object, Type type, INode target, Stream stream, Encoding encoding = null)
        {
            SerializeNodes(@object, type, target)
                .Encode(stream, encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint);
        }

        public void SerializeStream(object @object, Type type, Func<INode, Options, INode> targetFactory, string format, Stream stream, Encoding encoding = null)
        {
            SerializeNodes(@object, type, targetFactory, format)
                .Encode(stream, encoding ?? UTF8Encoding.NoBOM, _options.Serialization.PrettyPrint);
        }

        // Nodes

        public INode SerializeNodes<T>(T @object, INode target)
        {
            return SerializeNodes(@object, typeof(T), (s, o) => target, target.Format);
        }

        public INode SerializeNodes<T>(T @object, Func<INode, Options, INode> targetFactory, string format)
        {
            return SerializeNodes(@object, typeof(T), targetFactory, format);
        }

        public INode SerializeNodes(object @object, INode target)
        {
            return SerializeNodes(@object, @object.GetType(), (s, o) => target, target.Format);
        }

        public INode SerializeNodes(object @object, Func<INode, Options, INode> targetFactory, string format)
        {
            return SerializeNodes(@object, @object.GetType(), targetFactory, format);
        }

        public INode SerializeNodes(object @object, Type type, INode target)
        {
            return SerializeNodes(@object, @object.GetType(), (s, o) => target, target.Format);
        }

        public INode SerializeNodes(object @object, Type type, Func<INode, Options, INode> targetFactory, string format)
        {
            var source = NodeFactory.CreateSerializableRoot(@object, 
                type.GetCachedType(), _options, format);
            var target = targetFactory(source, _options);
            new NodeMapper<NodeBase, INode>(
                _options.Serialization.Writers.Mapping.HasMapping,
                _options.Serialization.Writers.Mapping.Map,
                _options.Serialization.Writers.Visitors.HasVisitor,
                _options.Serialization.Writers.Visitors.Visit)
                .Map(source, target, Mode.Serialize);
            return target;
        }
    }
}
