using System;

namespace Bender.Nodes.Xml
{
    public class WithAttributeAttribute : Attribute
    {
        public WithAttributeAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; }
    }
}