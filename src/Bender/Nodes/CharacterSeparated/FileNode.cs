using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bender.Collections;
using Bender.Configuration;
using Bender.Reflection;
using UTF8Encoding = Bender.Extensions.UTF8Encoding;

namespace Bender.Nodes.CharacterSeparated
{
    public class FileNode : NodeBase
    {
        public const string NodeFormat = "character separated";
        public static readonly Encoding DefaultEncoding = UTF8Encoding.NoBOM;
        
        private readonly CsvWriter _writer;
        private readonly CsvReader _reader;

        public FileNode(NodeType nodeType, Type actualType, 
            Type specifiedType, Options options, TextWriter writer)
        {
            var type = options.Serialization.SerializationType
                .IsActual() ? actualType : specifiedType;
            var cachedType = type.ToCachedType();
            _writer = new CsvWriter(writer, options, cachedType);
            if (nodeType != NodeType.Array || !cachedType.IsEnumerable)
                throw new BenderException("Only arrays can be serialized.");
        }

        public FileNode(string data, Options options, Type type = null) : 
            this(new StringReader(data), options, type) { }

        public FileNode(byte[] bytes, Options options, Type type = null, Encoding encoding = null) : 
            this(new MemoryStream(bytes), options, type, encoding) { }

        public FileNode(Stream stream, Options options, Type type = null, Encoding encoding = null)
            : this(new StreamReader(stream, encoding ?? DefaultEncoding), options, type) { }

        private FileNode(TextReader reader, Options options, Type type = null)
        {
            _reader = new CsvReader(type.ToCachedType(), reader, options);
        }

        public override string Format => NodeFormat;
        public override string Type => "file";
        protected override NodeType GetNodeType() => NodeType.Array;
        protected override IEnumerable<INode> GetNodes() => _reader.Read();

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            var rowNode = node is RowNode ? (RowNode)node : 
                new RowNode(_writer.RowNumber).Configure(modify).As<RowNode>();
            _writer.Write(rowNode);
        }

        public static string SerializeString(Type actualType, Type specifiedType, 
            Action<Func<INode, Options, INode>> serialize)
        {
            var result = new StringBuilder();
            var writer = new StringWriter(result);
            serialize((s, o) => new FileNode(
                s.NodeType, actualType, specifiedType, o, writer));
            writer.Flush();
            return result.ToString();
        }

        public static Stream SerializeStream(Type actualType, Type specifiedType,
            Action<Func<INode, Options, INode>> serialize, 
            Encoding encoding = null)
        {
            var stream = new MemoryStream();
            SerializeStream(stream, actualType, specifiedType, serialize, encoding);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static void SerializeStream(Stream stream, Type actualType, 
            Type specifiedType, Action<Func<INode, Options, INode>> serialize,
            Encoding encoding = null)
        {
            var writer = new StreamWriter(stream, encoding ?? DefaultEncoding);
            serialize((s, o) => new FileNode(
                s.NodeType, actualType, specifiedType, o, writer));
            writer.Flush();
        }

        public static void SerializeFile(string path, Type actualType, 
            Type specifiedType, Action<Func<INode, Options, INode>> serialize,
            Encoding encoding = null)
        {
            using (var target = File.Create(path))
            {
                SerializeStream(target, actualType, specifiedType, serialize, encoding);
            }
        }
    }
}
