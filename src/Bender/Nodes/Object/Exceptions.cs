using Bender.Extensions;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class TypeNotSupportedException : BenderException
    {
        public const string MessageTypeFormat =
            "{0} '{1}' is not supported for {2}. Only {3} can be {4}.";

        public TypeNotSupportedException(string typeDescription, 
            CachedType type, Mode mode, string expected) :
            base(GetMessage(typeDescription, type, mode, expected)) { }

        private static string GetMessage(string typeDescription, 
            CachedType type, Mode mode, string expected)
        {
            string modeNoun;
            string modeVerb;
            switch (mode)
            {
                case Mode.Deserialize: 
                    modeVerb = "deserialized";
                    modeNoun = "deserialization";
                    break;
                default:
                    modeVerb = "serialized";
                    modeNoun = "serialization";
                    break;
            }
            return MessageTypeFormat.ToFormat(typeDescription.ToInitialCaps(), 
                type.FriendlyFullName, modeNoun, expected, modeVerb);
        }
    }
}
