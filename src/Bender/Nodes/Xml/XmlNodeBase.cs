using System.Xml.Linq;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;

namespace Bender.Nodes.Xml
{
    public enum XmlObjectType { Element, Attribute }

    public class XObjectNotSupportedException : BenderException
    {
        public XObjectNotSupportedException(XObject @object) : base(
            "Xml object '{0}' not supported. Only XElements and XAttributes are supported."
                .ToFormat(@object.GetType().Name)) { }
    }

    public abstract class XmlNodeBase : NodeBase
    {
        public const string NodeFormat = "xml";

        protected XmlNodeBase(XObject @object, ElementNode parent, Options options) : base(parent)
        {
            Options = options;
            if (@object is XAttribute)
            {
                XmlType = XmlObjectType.Attribute;
                Attribute = @object.As<XAttribute>();
            }
            else if (@object is XElement)
            {
                XmlType = XmlObjectType.Element;
                Element = @object.As<XElement>();
            }
            else throw new XObjectNotSupportedException(@object);
        }

        protected Options Options { get; }

        public override string Format => NodeFormat;
        public override bool IsNamed => true;

        public XmlObjectType XmlType { get; private set; }
        public XObject Object { get; private set; }
        public XElement Element { get; private set; }
        public XAttribute Attribute { get; protected set; }

        public abstract void SetNamespace(XNamespace @namespace);

        public void SetNamespacePrefix(string prefix)
        {
            SetNamespace(Options.Serialization.XmlNamespaces[prefix]);
        }
    }
}
