using System;
using System.Collections.Generic;
using Bender.Collections;

namespace Bender.Nodes.CharacterSeparated
{
    public class RowNode : NodeBase
    {
        private readonly List<ValueNode> _values = new List<ValueNode>();
        private readonly int _rowNumber;

        public RowNode(int rowNumber)
        {
            _rowNumber = rowNumber;
        }
        
        public override string Type => "row";
        public override string Format => FileNode.NodeFormat;
        public override bool IsNamed => false;
        public override string Path => $"[{_rowNumber}]";

        protected override NodeType GetNodeType()
        {
            return NodeType.Object;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            if (nodeType != NodeType.Object)
                throw new BenderException("CSV rows must be object nodes.");
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is ValueNode) _values.Add((ValueNode)node);
            else _values.Add(new ValueNode(node.Name, this).Configure(modify).As<ValueNode>());
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return _values;
        }
    }
}