using System;
using System.Collections.Generic;
using Bender.Collections;

namespace Bender.Nodes.CharacterSeparated
{
    public class RowNode : NodeBase
    {
        private readonly List<INode> _values;
        private readonly int _rowNumber;
        private readonly string _name;

        public RowNode(int rowNumber, string name = "", List<INode> values = null)
        {
            _rowNumber = rowNumber;
            _name = name;
            _values = values ?? new List<INode>();
        }

        public override string Type => "row";
        public override string Format => FileNode.NodeFormat;
        public override bool IsNamed => false;
        public override string Path => $"{_rowNumber}:";

        protected override NodeType GetNodeType()
        {
            return NodeType.Variable;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            if (nodeType != NodeType.Object)
                throw new BenderException("CSV rows must be object nodes.");
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is ValueNode) _values.Add((ValueNode) node);
            else if (node is RowObjectNode) _values.Add((RowObjectNode)node);
            else
            {
                var name = _name + node.Name;
                if (node.NodeType == NodeType.Value)
                {
                    _values.Add(new ValueNode(node.Name, name, this)
                        .Configure(modify).As<INode>());
                }
                else modify(new RowNode(_rowNumber, name, _values));
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return _values;
        }
    }
}