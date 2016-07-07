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
        private object[] _columns;
        private readonly Lazy<StreamWriter> _writer;
        private bool _firstRow = true;
        private readonly bool _header;
        private readonly string _qualifier;
        private readonly string _delimiter;
        private readonly string _newLine;

        public Encoder(Stream output, Encoding encoding, 
            Options options, string format, CachedType type)
        {
            if (type.IsGenericEnumerable && options.Serialization
                .SerializationType == SerializationType.SpecifiedType)
            {
                var header = new SimpleValue(type.GenericEnumerableType);
                var context = new Context(options, Mode.Deserialize, format);
                _columns = new ObjectNode(context, null, header, null, null)
                    .Select(x => x.Name).ToArray();
            }

            _header = options.CsvHeader;
            _qualifier = options.CsvQualifier;
            _delimiter = options.CsvDelimiter;
            _newLine = options.CsvNewLine;
            _writer = new Lazy<StreamWriter>(() => new StreamWriter(output, encoding));
        }

        public void Write(RowNode node)
        {
            if (_columns == null) _columns = node.Select(x => x.Name).ToArray();

            if (!_columns.Any()) return;

            Action<object[]> writeLine = x => _writer.Value.Write(x.Select(Encode)
                .Aggregate((a, i) => a + _delimiter + i) + _newLine);

            if (_header && _firstRow) writeLine(_columns);

            var fields = new object[_columns.Length];
            node.Where(x => _columns.Contains(x.Name))
                .ForEach(x => fields[_columns.IndexOf(x.Name)] = x.Value);
            writeLine(fields);

            _writer.Value.Flush();

            _firstRow = false;
        }

        private string Encode(object value)
        {
            if (value == null) return "";
            if (value is bool) return value.ToString().ToLower();
            if (value.IsNumeric()) return value.ToString();
            return _qualifier + value.ToString().Replace(_qualifier, 
                _qualifier + _qualifier) + _qualifier;
        }

        public void Dispose()
        {
            if (_writer.IsValueCreated) _writer.Value.Close();
        }
    }
}
