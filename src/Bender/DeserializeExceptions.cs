using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Bender
{
    public class DeserializeException : Exception
    {
        public DeserializeException(PropertyInfo property, XObject node, Format format, string message, params object[] args) :
            base(propertyMessage(property, node, message, args, format)) { }
        public DeserializeException(PropertyInfo property, XObject node, Format format, Exception exception, string message, params object[] args) :
            base(propertyMessage(property, node, message, args, format), exception) { }
        public DeserializeException(Type type, string message) :
            base("Unable to deserialize type {0}: {1}".ToFormat(type, message)) { }

        private static readonly Func<PropertyInfo, XObject, string, object[], Format, string> propertyMessage = (p, n, m, a, f) =>
            "Unable to set {0}.{1} to the value in the '{2}' {3}: {4}".ToFormat(
                 p.DeclaringType.FullName, p.Name, n.GetPath(f), n.ObjectType(), m.ToFormat(a));
    }

    public abstract class SourceException : Exception
    {
        protected SourceException(string debugMessage, string friendlyMessage, Exception innerException)
            : base(debugMessage, innerException)
        {
            FriendlyMessage = friendlyMessage;
        }

        protected SourceException(string debugMessage, string friendlyMessage)
            : base(debugMessage)
        {
            FriendlyMessage = friendlyMessage;
        }

        public string FriendlyMessage { get; private set; }
    }

    public class UnmatchedNodeException : SourceException
    {
        public UnmatchedNodeException(ValueNode node) :
            base("The '{0}' {1} does not correspond to a type or property.".ToFormat(
                    node.Object.GetPath(node.Format), node.NodeType.ToFriendlyNodeType()),
                 "Unable to read {0}: The '{1}' {2} is not recognized.".ToFormat(node.Format.ToString().ToLower(),
                    node.Object.GetPath(node.Format), node.NodeType.ToFriendlyNodeType())) { }
    }

    public class SourceParseException : SourceException
    {
        public SourceParseException(XmlException exception, Format format) :
            base(exception.Message, "Unable to parse {0}: {1}".ToFormat(format.ToString().ToLower(), exception.Message), exception) { }

        public XmlException ParseException { get { return (XmlException) InnerException; } }
    }

    public class ValueParseException : SourceException
    {
        public ValueParseException(ReaderContext context, ParseException exception) :
            this(context.Property, context.Node, exception) { }

        public ValueParseException(ReaderContext context, string friendlyMessage, Exception exception) :
            this(context.Property, context.Node, friendlyMessage, exception) { }
        
        public ValueParseException(PropertyInfo property, ValueNode node, ParseException exception) :
            this(property, node, exception.FriendlyMessage, exception) { }

        public ValueParseException(PropertyInfo property, ValueNode node, string friendlyMessage, Exception exception) :
            base("Unable to parse the value {0} in the '{1}' {2} as a {3} into {4}.{5}: {6}".ToFormat(
                               GetFriendlyValue(node), node.Object.GetPath(node.Format), node.NodeType.ToFriendlyNodeType(), property.PropertyType.Name,
                               property.DeclaringType.FullName, property.Name, exception.Message),
                 "Unable to parse the value {0} in the '{1}' {2} as a {3}: {4}".ToFormat(
                               GetFriendlyValue(node), node.Object.GetPath(node.Format), node.NodeType.ToFriendlyNodeType(), 
                               property.PropertyType.ToFriendlyType(), friendlyMessage),
                 exception)
        {
            Value = node.Value;
            XPath = node.Object.GetPath(node.Format);
            Node = node;
            ParseErrorMessage = friendlyMessage;
            ClrType = property.PropertyType;
            FriendlyType = property.PropertyType.ToFriendlyType();
        }

        public string Value { get; private set; }
        public string XPath { get; private set; }
        public ValueNode Node { get; private set; }
        public string ParseErrorMessage { get; private set; }
        public Type ClrType { get; private set; }
        public string FriendlyType { get; private set; }

        private static string GetFriendlyValue(ValueNode node)
        {
            return node.Value == null ? "<null>"  : "'" + node.Value.TruncateAt(50) + "'";
        }
    }

    public static class NodeTypeExtensions
    {
        public static string ToFriendlyNodeType(this NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.JsonField: return "field";
                case NodeType.XmlAttribute: return "attribute";
                default: return "element";
            }
        }
    }
}
