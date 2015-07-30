using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using Microsoft.VisualBasic.FileIO;
using UTF8Encoding = Bender.Extensions.UTF8Encoding;

namespace Bender.Nodes.CharacterSeparated
{
    public class FileNode : NodeBase
    {
        public const string NodeFormat = "character separated";

        private readonly List<RowNode> _rows;
        private readonly CachedType _type;
        private readonly Options _options;
        private int _rowIndex = 1;

        public FileNode(NodeType nodeType, Type type, Options options)
        {
            _options = options;
            _type = type.ToCachedType();
            if (nodeType != NodeType.Array || !_type.IsEnumerable)
                throw new BenderException("Only arrays can be serialized.");
            _rows = new List<RowNode>();
        }

        public FileNode(string data, Options options)
        {
            _options = options;
            _rows = ParseFile(data);
        }

        public FileNode(byte[] bytes, Options options, 
            Encoding encoding = null)
        {
            _options = options;
            _rows = ParseFile(bytes, encoding);
        }

        public FileNode(Stream stream, Options options, 
            Encoding encoding = null)
        {
            _options = options;
            _rows = ParseFile(stream, encoding);
        }

        public override string Format => NodeFormat;
        public override string Type => "file";
        protected override NodeType GetNodeType() => NodeType.Array;
        protected override IEnumerable<INode> GetNodes() => _rows;

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is RowNode) _rows.Add(((RowNode) node));
            else _rows.Add(new RowNode(_rowIndex++).Configure(modify).As<RowNode>());
        }

        public override void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            var context = new Context(_options, Mode.Deserialize, NodeFormat);
            var header = new SimpleValue(_type.GenericEnumerableType);
            var columns = new ObjectNode(context, null, header, null, null)
                .Select(x => x.Name).ToArray();

            if (!columns.Any()) return;

            var writer = new StreamWriter(stream, encoding ?? UTF8Encoding.NoBOM);
            var qualifier = _options.CsvQualifier;

            Action<string[]> writeLine = x => writer
                .Write(x.Select(y =>
                    $"{qualifier}{y.Replace(qualifier, qualifier + qualifier)}{qualifier}")
                .Aggregate((a, i) => $"{a}{_options.CsvDelimiter}{i}") + _options.CsvNewLine);

            writeLine(columns);

            foreach (var row in _rows)
            {
                var fields = new string[columns.Length];
                row.Where(x => columns.Contains(x.Name))
                    .ForEach(x => fields[columns.IndexOf(x.Name)] = x.Value?.ToString());
                writeLine(fields);
            }
                
            writer.Flush();
        }

        private List<RowNode> ParseFile(Stream stream, Encoding encoding) => 
            ParseFile(new StreamReader(stream, encoding ?? Encoding.UTF8));

        private List<RowNode> ParseFile(byte[] bytes, Encoding encoding) =>
            ParseFile(new MemoryStream(bytes), encoding);

        private List<RowNode> ParseFile(string data) =>
            ParseFile(new StringReader(data));

        private List<RowNode> ParseFile(TextReader reader)
        {
            var nodes = new List<RowNode>();
            using (var parser = new TextFieldParser(reader))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(_options.CsvDelimiter);
                if (parser.EndOfData) return nodes;
                var columns = parser.ReadFields();
                if (columns == null || columns.Length == 0) return nodes;
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (fields == null || fields.Length == 0) continue;
                    var rowNode = new RowNode(_rowIndex++);
                    nodes.Add(rowNode);
                    for (var i = 0; i < fields.Length; i++)
                    {
                        rowNode.Add(new ValueNode(columns[i], fields[i]), x => {});
                    }
                }
            }
            return nodes;
        }
    }
}
