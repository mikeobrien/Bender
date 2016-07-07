using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bender.Collections;
using Bender.Configuration;
using Bender.Reflection;
using Microsoft.VisualBasic.FileIO;
using UTF8Encoding = Bender.Extensions.UTF8Encoding;

namespace Bender.Nodes.CharacterSeparated
{
    public class FileNode : NodeBase
    {
        public const string NodeFormat = "character separated";
        public static readonly Encoding DefaultEncoding = UTF8Encoding.NoBOM;

        private readonly CachedType _type;
        private readonly Options _options;
        private readonly Encoding _encoding;
        private readonly List<RowNode> _writeRows;
        private readonly Encoder _writeEncoder;
        private readonly Lazy<List<RowNode>> _readRows;
        private readonly Stream _readStream;
        private int _rowNumber = 1;

        public FileNode(NodeType nodeType, Type type, Options options,
            Stream stream, Encoding encoding)
            : this(nodeType, type, encoding, options)
        {
            _writeEncoder = new Encoder(stream, _encoding, options, NodeFormat, _type);
        }

        public FileNode(NodeType nodeType, Type type, Options options)
            : this(nodeType, type, null, options)
        {
            _writeRows = new List<RowNode>();
        }

        private FileNode(NodeType nodeType, Type type, Encoding encoding, Options options)
        {
            _type = type.ToCachedType();
            if (nodeType != NodeType.Array || !_type.IsEnumerable)
                throw new BenderException("Only arrays can be serialized.");
            _encoding = encoding ?? DefaultEncoding;
            _options = options;
        }

        public FileNode(string data, Options options, Encoding encoding = null) : 
            this(new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(data)), 
                options, encoding) { }

        public FileNode(byte[] bytes, Options options, Encoding encoding = null) : 
            this(new MemoryStream(bytes), options, encoding) { }

        public FileNode(Stream stream, Options options, Encoding encoding = null)
        {
            _options = options;
            _readStream = stream;
            _encoding = encoding ?? DefaultEncoding;
            _readRows = new Lazy<List<RowNode>>(ParseFile);
        }

        public override string Format => NodeFormat;
        public override string Type => "file";
        protected override NodeType GetNodeType() => NodeType.Array;
        protected override IEnumerable<INode> GetNodes() => _readRows.Value;

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            var rowNode = node is RowNode ? (RowNode)node : 
                new RowNode(_rowNumber++).Configure(modify).As<RowNode>();
            if (_writeEncoder != null) _writeEncoder.Write(rowNode);
            else _writeRows.Add(rowNode);
        }
        
        public override void Encode(Stream stream, Encoding encoding = null)
        {
            if (_writeEncoder != null) return;

            var encoder = new Encoder(stream, encoding ?? 
                _encoding, _options, NodeFormat, _type);
            _writeRows.ForEach(encoder.Write);
        }

        private List<RowNode> ParseFile()
        {
            var nodes = new List<RowNode>();
            var parser = new TextFieldParser(new StreamReader(_readStream, _encoding));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_options.CsvDelimiter);
            if (parser.EndOfData) return nodes;
            var columns = parser.ReadFields();
            if (columns == null || columns.Length == 0) return nodes;
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                if (fields == null || fields.Length == 0) continue;
                var rowNode = new RowNode(_rowNumber++);
                nodes.Add(rowNode);
                for (var i = 0; i < fields.Length; i++)
                {
                    rowNode.Add(new ValueNode(columns[i], rowNode, 
                        fields[i] == "" ? null : fields[i]), x => {});
                }
            }
            return nodes;
        }
    }
}
