using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Bender.Nodes.Object;
using Bender.Reflection;

namespace Bender.Configuration
{
    public class DeserializationOptions
    {
        public DeserializationOptions(Options options)
        {
            NameComparison = StringComparison.Ordinal;
            IgnoreXmlAttributes = false;

            FriendlyParseErrorMessages = new Dictionary<Type, string>();
            FriendlyParseErrorMessages[typeof(Enum)] = "Option '{0}' is not valid.";
            FriendlyParseErrorMessages[typeof(char)] = "Char '{0}' not valid, must be exactly one character.";
            FriendlyParseErrorMessages[typeof(bool)] = "Boolean '{0}' not formatted correctly, must be 'true' or 'false'.";
            FriendlyParseErrorMessages[typeof(sbyte)] = "Byte '{0}' not formatted correctly, must be an integer between -128 and 127.";
            FriendlyParseErrorMessages[typeof(byte)] = "Unsigned byte '{0}' not formatted correctly, must be an integer between 0 and 255.";
            FriendlyParseErrorMessages[typeof(short)] = "Integer '{0}' not formatted correctly, must be an integer between -32,768 and 32,767.";
            FriendlyParseErrorMessages[typeof(ushort)] = "'Unsigned integer {0}' not formatted correctly, must be an integer between 0 and 65,535.";
            FriendlyParseErrorMessages[typeof(int)] = "Integer '{0}' not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.";
            FriendlyParseErrorMessages[typeof(IntPtr)] = FriendlyParseErrorMessages[typeof(int)];
            FriendlyParseErrorMessages[typeof(UIntPtr)] = FriendlyParseErrorMessages[typeof(ushort)];
            FriendlyParseErrorMessages[typeof(uint)] = "Unsigned integer '{0}' not formatted correctly, must be an integer between 0 and 4,294,967,295.";
            FriendlyParseErrorMessages[typeof(long)] = "Integer '{0}' not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.";
            FriendlyParseErrorMessages[typeof(ulong)] = "Unsigned integer '{0}' not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.";
            FriendlyParseErrorMessages[typeof(float)] = "32 bit float '{0}' not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.";
            FriendlyParseErrorMessages[typeof(double)] = "64 bit float '{0}' not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.";
            FriendlyParseErrorMessages[typeof(decimal)] = "Decimal '{0}' not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.";
            FriendlyParseErrorMessages[typeof(DateTime)] = "Date '{0}' not formatted correctly, must be formatted as m/d/yyy h:m:s AM.";
            FriendlyParseErrorMessages[typeof(Guid)] = "UUID '{0}' not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
            FriendlyParseErrorMessages[typeof(TimeSpan)] = "Timespan'{0}' not formatted correctly, must be formatted as 'd.h:m:s'.";
            FriendlyParseErrorMessages[typeof(Uri)] = "Uri '{0}' not formatted correctly, must be formatted as 'scheme://host'.";

            FriendlyParseErrorMessages[typeof(Version)] = "Version '{0}' not formatted correctly, must be formatted as '1.2.3.4'.";
            FriendlyParseErrorMessages[typeof(MailAddress)] = "Email address '{0}' not formatted correctly, must be formatted as 'username@domain.com'.";
            FriendlyParseErrorMessages[typeof(IPAddress)] = "IP address '{0}' not formatted correctly, must be formatted as '1.2.3.4'.";

            Readers = new ReaderConventions(options);

            IgnoreUnmatchedElements = true;
            IgnoreUnmatchedMembers = true;

            Readers.AddValueReader((s, t, o) => Version.Parse(s.Value.ToString()));
            Readers.AddValueReader((s, t, o) => new MailAddress(s.Value.ToString()));
            Readers.AddValueReader((s, t, o) => IPAddress.Parse(s.Value.ToString()));
        }

        public StringComparison NameComparison { get; set; }
        public StringComparison EnumNameComparison { get; set; }

        public bool IgnoreUnmatchedElements { get; set; }
        public bool IgnoreUnmatchedMembers { get; set; }

        public bool IgnoreXmlAttributes { get; set; }
        public bool IgnoreRootName { get; set; }
        public bool IgnoreArrayItemNames { get; set; }
        public Dictionary<Type, string> FriendlyParseErrorMessages { get; set; }
        public Func<CachedType, object[], object> ObjectFactory { get; set; }

        public ReaderConventions Readers { get; private set; }
    }
}