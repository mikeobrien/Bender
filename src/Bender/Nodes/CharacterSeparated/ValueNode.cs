namespace Bender.Nodes.CharacterSeparated
{
    public class ValueNode : NodeBase
    {
        private string _name;
        private string _value;

        public ValueNode(string name, string value = null)
        {
            _name = name;
            _value = value;
        }

        public override string Type => "csv value";
        public override string Format => FileNode.NodeFormat;
        public override bool IsNamed => true;
        public override string Path => $"{Parent.Path}.{_name}";

        protected override NodeType GetNodeType()
        {
            return NodeType.Value;
        }

        protected override void SetNodeType(NodeType nodeType)
        {
            if (nodeType != NodeType.Value)
                throw new BenderException("CSV values must be value nodes.");
        }

        protected override string GetName()
        {
            return _name;
        }

        protected override void SetName(string name)
        {
            _name = name;
        }

        protected override object GetValue()
        {
            return _value;
        }

        protected override void SetValue(object value)
        {
            _value = value?.ToString() ?? "";
        }
    }
}