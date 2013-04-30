using System;
using System.Reflection;
using System.Xml.Linq;

namespace Bender
{
    public class UnmatchedNodeException : BenderException
    {
        public UnmatchedNodeException(XObject node) :
            base(string.Format("The '{0}' {1} does not correspond to a type or property.",
                    node.GetXPath(), node.ObjectType()),
                 string.Format("The '{0}' {1} is not recognized.",
                    node.GetXPath(), node.ObjectType())) { }
    }

    public class SetValueException : BenderException
    {
        public SetValueException(PropertyInfo property, XObject node, string message) :
            base(GetDebugMessage(property, node, message), GetFriendlyMessage(node, message)) { }

        public SetValueException(PropertyInfo property, XObject node, string value, Exception exception) :
            base(GetDebugMessage(property, node, exception.Message, value), GetFriendlyMessage(node, exception.Message, value), exception) { }

        private static string GetDebugMessage(PropertyInfo property, XObject node, string message, string value = null)
        {
            return string.Format("Unable to set {0}.{1} to the value {2}in the '{3}' {4}: {5}",
                property.DeclaringType.FullName, property.Name, GetFriendlyValue(value), node.GetXPath(), node.ObjectType(), message);
        }

        private static string GetFriendlyMessage(XObject node, string message, string value = null)
        {
            return string.Format("Unable to read the value {0}in the '{1}' {2}: {3}",
                GetFriendlyValue(value), node.GetXPath(), node.ObjectType(), message);
        }

        private static string GetFriendlyValue(string value)
        {
            return value != null ? "'" + value.TruncateAt(50) + "' " : "";
        }
    }

    public class DeserializeException : BenderException
    {
        public DeserializeException(Type type, string message) :
            base(string.Format("Unable to deserialize {0}: {1}", type.FullName, message),
                 string.Format("Unable to read xml.")) { }

    }
}
