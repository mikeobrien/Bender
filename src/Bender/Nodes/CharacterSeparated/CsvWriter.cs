using System;
using System.IO;
using System.Linq;
using Bender.Collections;
using Bender.Configuration;
using Bender.NamingConventions;
using Bender.Nodes.Object;
using Bender.Reflection;

namespace Bender.Nodes.CharacterSeparated
{
    public class CsvWriter
    {
        private readonly TextWriter _writer;
        private readonly CachedType _type;
        private readonly Lazy<string[]> _columns;
        private bool _firstRow = true;
        private readonly bool _header;
        private readonly string _qualifier;
        private readonly string _delimiter;
        private readonly string _newLine;

        public CsvWriter(TextWriter writer, Options options, CachedType type)
        {
            _columns = new Lazy<string[]>(() => 
                LoadColumns(type.GenericEnumerableType, options));
            _header = options.CsvHeader;
            _qualifier = options.CsvQualifier;
            _delimiter = options.CsvDelimiter;
            _newLine = options.CsvNewLine;
            _writer = writer;
            _type = type;
        }

        public int RowNumber { get; private set; } = 1;

        public void Write(RowNode node)
        {
            Action<object[]> writeLine = x => _writer.Write(x.Select(Encode)
                .Aggregate((a, i) => a + _delimiter + i) + _newLine);

            if (!_type.IsGenericEnumerable)
            {
                writeLine(node.Select(x => x.Value?.ToString()).ToArray());
            }
            else
            {
                if (_header && _firstRow) writeLine(_columns.Value);
                var nodes = node.Cast<ValueNode>();
                var fields = new object[_columns.Value.Length];
                nodes.Where(x => _columns.Value.Contains(x.ColumnName))
                    .ForEach(x => fields[_columns.Value
                        .IndexOf(x.ColumnName)] = x.Value);
                writeLine(fields);
            }

            _writer.Flush();

            _firstRow = false;
            RowNumber++;
        }

        public string[] LoadColumns(CachedType type, Options options, string baseName = "")
        {
            var members = type.Members.Where(x => 
                    (x.Type.IsSimpleType || !x.Type.IsEnumerable) && 
                    (options.MemberFilter == null || options.MemberFilter(x, options)))
                .Select(x => new {
                    Member = x,
                    Name = baseName + GetMemberName(x, options)
                }).ToList();
            var simpleTypes = members.Where(x => x.Member.Type.IsSimpleType)
                .Select(x => x.Name).ToList();
            var complexTypes = members.Where(x => !x.Member.Type.IsSimpleType)
                .SelectMany(x => LoadColumns(x.Member.Type, options, x.Name));
            return simpleTypes.Concat(complexTypes).ToArray();
        }

        private string GetMemberName(CachedMember member, Options options)
        {
            var context = new Context(options, Mode.Serialize, FileNode.NodeFormat);
            var memberContext = new MemberContext(member, context);
            return member.IsField ? options.FieldNameConventions.GetName(memberContext) :
                options.PropertyNameConventions.GetName(memberContext);
        }

        private string Encode(object value)
        {
            if (value == null) return "";
            if (value is bool) return value.ToString().ToLower();
            if (value.IsNumeric()) return value.ToString();
            return _qualifier + value.ToString().Replace(_qualifier, 
                _qualifier + _qualifier) + _qualifier;
        }
    }
}
