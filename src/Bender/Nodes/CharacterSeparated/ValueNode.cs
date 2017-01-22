using System.Linq;
using Bender.Collections;

namespace Bender.Nodes.CharacterSeparated
{
    public class ValueNode : NodeBase
    {
        private object _value;

        public ValueNode(string name, string columnName, 
            INode parent, object value = null) : base(parent)
        {
            Name = name;
            ColumnName = columnName;
            _value = value;
        }

        public string ColumnName { get; }
        public override string Type => "csv value";
        public override string Format => FileNode.NodeFormat;
        public override bool IsNamed => true;
        public override string Path => Parent.Walk(x => x.Parent).Last().Path + ColumnName;

        protected override NodeType GetNodeType()
        {
            return NodeType.Value;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            if (nodeType != NodeType.Value)
                throw new BenderException("CSV values must be value nodes.");
        }

        protected override object GetValue()
        {
            return _value;
        }

        protected override void SetValue(object value)
        {
            _value = value ?? "";
        }
    }
}