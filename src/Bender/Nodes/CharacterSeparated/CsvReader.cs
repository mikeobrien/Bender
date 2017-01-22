using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;
using Microsoft.VisualBasic.FileIO;

namespace Bender.Nodes.CharacterSeparated
{
    public class CsvReader
    {
        private readonly CachedType _type;
        private readonly TextReader _reader;
        private readonly Options _options;
        private int _rowNumber = 1;

        public CsvReader(CachedType type, TextReader reader, Options options)
        {
            _type = type;
            _reader = reader;
            _options = options;
        }

        public IEnumerable<RowNode> Read()
        {
            var parser = new TextFieldParser(_reader);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_options.CsvDelimiter);

            if (parser.EndOfData) yield break;

            var columns = parser.ReadFields();
            if (columns == null || columns.Length == 0) yield break;

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields == null || fields.Length == 0) continue;

                var rowNode = new RowNode(_rowNumber++);

                var values = 0.To(fields.Length).Where(i => i < fields.Length)
                    .Select(i => new CsvValue(columns[i], columns[i], 
                        fields[i] == "" ? null : fields[i]))
                    .Where(x => !_options.Deserialization.IgnoreEmptyCsvValues || 
                        x.Value != null).ToList();

                var rowNodes = _type?.GenericEnumerableType != null ? 
                    MapNodes(rowNode, _type.GenericEnumerableType, values) :
                    values.Select(x => new ValueNode(x.Name, x.Name, rowNode, x.Value));

                rowNodes.ForEach(x => rowNode.Add(x, y => { }));

                if (!_options.Deserialization.IgnoreEmptyCsvValues || 
                    rowNodes.Any()) yield return rowNode;
            }
        }

        private IEnumerable<INode> MapNodes(INode parent, 
            CachedType type, List<CsvValue> values)
        {
            Func<string, string, bool> matches = (n1, n2) => n1
                .Equals(n2, _options.Deserialization.NameComparison);
            Func<string, string, bool> startsWith = (n1, n2) => n1
                .StartsWith(n2, _options.Deserialization.NameComparison);
            var nodes = new List<INode>();
            var mapping = values.Select(x => new
            {
                x.Name, x.ColumnName, x.Value,
                Member = type.Members.FirstOrDefault(m => startsWith(x.Name, m.Name))
            }).Where(x => x.Member != null).ToList();

            nodes.AddRange(mapping.Where(x => matches(x.Member.Name, x.Name))
                .Select(x => new ValueNode(x.Name, x.ColumnName, parent, x.Value)));

            mapping.Where(x => !matches(x.Member.Name, x.Name))
                .GroupBy(x => x.Member)
                .ForEach(x =>
                {
                    var childNodes = MapNodes(parent, x.Key.Type,
                        x.Select(v => new CsvValue(v.Name.Substring(x.Key.Name.Length),
                            v.Name, v.Value)).ToList()).ToList();
                    nodes.Add(new RowObjectNode(x.Key.Name, childNodes, parent));
                });

            return nodes;
        }

        private class CsvValue
        {
            public CsvValue(string name, string columnName, string value)
            {
                Name = name;
                ColumnName = columnName;
                Value = value;
            }

            public string Name { get; }
            public string ColumnName { get; }
            public string Value { get; }
        }
    }
}
