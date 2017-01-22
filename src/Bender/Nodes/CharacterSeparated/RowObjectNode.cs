using System.Collections.Generic;
using System.Linq;
using Bender.Collections;

namespace Bender.Nodes.CharacterSeparated
{
    public class RowObjectNode : NodeBase
    {
        private readonly List<INode> _values;

        public RowObjectNode(string name, List<INode> values, 
            INode parent) : base(parent)
        {
            _values = values;
            Name = name;
        }
        
        public override string Type => "row";
        public override string Format => FileNode.NodeFormat;
        public override bool IsNamed => true;

        protected override NodeType GetNodeType()
        {
            return NodeType.Object;
        }
        
        protected override IEnumerable<INode> GetNodes()
        {
            return _values;
        }
    }
}