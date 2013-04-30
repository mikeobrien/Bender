using System;

namespace Bender
{
    public class BenderException : Exception
    {
        public BenderException(string debugMessage, string friendlyMessage, Exception innerException) : base(debugMessage, innerException)
        {
            FriendlyMessage = friendlyMessage;
        }

        public BenderException(string debugMessage, string friendlyMessage) : base(debugMessage)
        {
            FriendlyMessage = friendlyMessage;
        }

        public string FriendlyMessage { get; private set; }
    }
}
