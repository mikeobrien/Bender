using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Bender
{
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
        public UnmatchedNodeException(XObject node) :
            base("The '{0}' {1} does not correspond to a type or property.".ToFormat(
                    node.GetXPath(), node.ObjectType()),
                 "The '{0}' {1} is not recognized.".ToFormat(
                    node.GetXPath(), node.ObjectType())) { }
    }

    public class XmlParseException : SourceException
    {
        public XmlParseException(XmlException exception) :
            base(exception.Message, "Unable to parse xml: {0}".ToFormat(exception.Message), exception) { }

        public XmlException XmlException { get { return (XmlException) InnerException; } }
    }

    public class ValueParseException : SourceException
    {
        public ValueParseException(PropertyInfo property, XObject node, string value, ParseException exception) :
            base("Unable to parse the value {0} in the '{1}' {2} as a {3} into {4}.{5}: {6}".ToFormat(
                               GetFriendlyValue(value), node.GetXPath(), node.ObjectType(), property.PropertyType.Name, 
                               property.DeclaringType.FullName, property.Name, exception.Message),
                 "Unable to parse the value {0} in the '{1}' {2} as a {3}: {4}".ToFormat(
                               GetFriendlyValue(value), node.GetXPath(), node.ObjectType(), property.PropertyType.ToFriendlyType(), exception.FriendlyMessage),
                 exception)
        {
            Value = value;
            XPath = node.GetXPath();
            NodeType = node.NodeType;
            ParseErrorMessage = exception.FriendlyMessage;
            ClrType = property.PropertyType;
            FriendlyType = property.PropertyType.ToFriendlyType();
        }

        public string Value { get; private set; }
        public string XPath { get; private set; }
        public XmlNodeType NodeType { get; private set; }
        public string ParseErrorMessage { get; private set; }
        public Type ClrType { get; private set; }
        public string FriendlyType { get; private set; }

        public static T Wrap<T>(Options options, PropertyInfo property, ValueNode node, Func<T> parse, string friendlyErrorMessage = null)
        {
            return Exceptions.Wrap(parse, x => new ValueParseException(property, node.Object, node.Value,
                new ParseException(x, friendlyErrorMessage ?? options.FriendlyParseErrorMessages[typeof(T)])));
        }

        private static string GetFriendlyValue(string value)
        {
            return value != null ? "'" + value.TruncateAt(50) + "'" : "<null>";
        }
    }
}
