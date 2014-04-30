using System;
using Bender.Extensions;

namespace Bender
{
    public class BenderException : Exception
    {
        public BenderException(string message) :
            base(message.RemoveNewLines()) { }

        public BenderException(string message, params object[] args) :
            base(message.ToFormat(args).RemoveNewLines()) { }

        public BenderException(Exception innerException) :
            base(innerException.Message.RemoveNewLines(), innerException) { }

        public BenderException(Exception innerException, params object[] args) :
            base(innerException.Message.ToFormat(args).RemoveNewLines(), innerException) { }

        public BenderException(Exception innerException, string message) :
            base(message.RemoveNewLines(), innerException) { }

        public BenderException(Exception innerException, string message, params object[] args) :
            base(message.ToFormat(args).RemoveNewLines(), innerException) { }
    }

    public class FriendlyBenderException : BenderException
    {
        public FriendlyBenderException(string message) :
            base(message)
        {
            FriendlyMessage = message.RemoveNewLines();
        }

        public FriendlyBenderException(string message, string friendlyMessage) :
            base(message)
        {
            FriendlyMessage = friendlyMessage.RemoveNewLines();
        }

        public FriendlyBenderException(string message, string friendlyMessage, params object[] args) :
            base(message, args)
        {
            FriendlyMessage = friendlyMessage.ToFormat(args).RemoveNewLines();
        }

        public FriendlyBenderException(Exception innerException, string friendlyMessage) :
            base(innerException)
        {
            FriendlyMessage = friendlyMessage.RemoveNewLines();
        }

        public FriendlyBenderException(Exception innerException, string friendlyMessage, params object[] args) :
            base(innerException, args)
        {
            FriendlyMessage = friendlyMessage.ToFormat(args).RemoveNewLines();
        }

        public FriendlyBenderException(Exception innerException, string message, string friendlyMessage) :
            base(innerException, message)
        {
            FriendlyMessage = friendlyMessage.RemoveNewLines();
        }

        public FriendlyBenderException(Exception innerException, string message, string friendlyMessage, params object[] args) :
            base(innerException, message, args)
        {
            FriendlyMessage = friendlyMessage.ToFormat(args).RemoveNewLines();
        }

        public string FriendlyMessage { get; private set; }
    }
}
