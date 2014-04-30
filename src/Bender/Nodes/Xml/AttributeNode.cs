using System.Xml.Linq;
using Bender.Configuration;

namespace Bender.Nodes.Xml
{
    public class AttributeNode : XmlNodeBase
    {
        private XNamespace _namespace;

        public AttributeNode(XAttribute attribute, ElementNode parent, Options options) : 
            base(attribute, parent, options) { }

        public override string Type { get { return "attribute"; } }
        public override string Path { get { return Attribute.GetPath(); } }

        protected override NodeType GetNodeType()
        {
            return NodeType.Value;
        }

        protected override string GetName()
        {
            return Attribute.Name.LocalName;
        }

        protected override void SetName(string name)
        {
            var parent = Attribute.Parent;
            Attribute.Remove();
            Attribute = new XAttribute(_namespace != null ? 
                _namespace + name : name, Attribute.Value);
            parent.Add(Attribute);
        }

        protected override object GetValue()
        {
            return Attribute.Value;
        }

        protected override void SetValue(object value)
        {
            Attribute.Value = value == null ? "" : (value is bool ? 
                value.ToString().ToLower() : value.ToString());
        }

        public override void SetNamespace(XNamespace @namespace)
        {
            _namespace = @namespace;
            SetName(Attribute.Name.LocalName);
        }
    }
}
