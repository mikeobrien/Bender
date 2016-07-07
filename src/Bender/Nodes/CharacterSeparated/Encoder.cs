using System;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;

namespace Bender.Nodes.CharacterSeparated
{
    public class Encoder : IDisposable
    {
        private readonly string[] _columns;
        private readonly Stream _output;
        private readonly Encoding _encoding;
        private readonly Lazy<StreamWriter> _writer;
        private bool _firstRow = true;
        private readonly bool _header;
        private readonly string _qualifier;
        private readonly string _delimiter;
        private readonly string _newLine;

        public Encoder(Stream output, Encoding encoding, 
            Options options, string format, CachedType type)
        {
            var header = new SimpleValue(type.GenericEnumerableType);
            var context = new Context(options, Mode.Deserialize, format);
            _columns = new ObjectNode(context, null, header, null, null)
                .Select(x => x.Name).ToArray();
            _output = output;
            _encoding = encoding;
            _header = options.CsvHeader;
            _qualifier = options.CsvQualifier;
            _delimiter = options.CsvDelimiter;
            _newLine = options.CsvNewLine;
            _writer = new Lazy<StreamWriter>(() => new StreamWriter(_output, _encoding));
        }

        public void Write(RowNode node)
        {
            if (!_columns.Any()) return;

            Action<string[]> writeLine = x => _writer.Value
                .Write(x.Select(y => $"{_qualifier}{y?.Replace(_qualifier, _qualifier + _qualifier)}{_qualifier}")
                .Aggregate((a, i) => $"{a}{_delimiter}{i}") + _newLine);

            if (_header && _firstRow) writeLine(_columns);

            var fields = new string[_columns.Length];
            node.Where(x => _columns.Contains(x.Name))
                .ForEach(x => fields[_columns.IndexOf(x.Name)] = x.Value?.ToString());
            writeLine(fields);

            _writer.Value.Flush();

            _firstRow = false;
        }

        public void Dispose()
        {
            if (_writer.IsValueCreated) _writer.Value.Close();
        }
    }
}
