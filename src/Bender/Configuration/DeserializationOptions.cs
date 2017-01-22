using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            EnumValueComparison = StringComparison.OrdinalIgnoreCase;
            IgnoreXmlAttributes = false;

            FriendlyParseErrorMessages = new Dictionary<Type, string>
            {
                [typeof(Enum)] = "Option '{0}' is not valid.",
                [typeof(char)] = "Char '{0}' not valid, must be exactly one character.",
                [typeof(bool)] = "Boolean '{0}' not formatted correctly, must be 'true' or 'false'.",
                [typeof(sbyte)] = "Byte '{0}' not formatted correctly, must be an integer between -128 and 127.",
                [typeof(byte)] = "Unsigned byte '{0}' not formatted correctly, must be an integer between 0 and 255.",
                [typeof(short)] = "Integer '{0}' not formatted correctly, must be an integer between -32,768 and 32,767.",
                [typeof(ushort)] = "Unsigned integer '{0}' not formatted correctly, must be an integer between 0 and 65,535.",
                [typeof(int)] = "Integer '{0}' not formatted correctly, must be an integer between -2,147,483,648 and 2,147,483,647.",
                [typeof(uint)] = "Unsigned integer '{0}' not formatted correctly, must be an integer between 0 and 4,294,967,295.",
                [typeof(long)] = "Integer '{0}' not formatted correctly, must be an integer between -9,223,372,036,854,775,808 and 9,223,372,036,854,775,807.",
                [typeof(ulong)] = "Unsigned integer '{0}' not formatted correctly, must be an integer between 0 and 18,446,744,073,709,551,615.",
                [typeof(float)] = "32 bit float '{0}' not formatted correctly, must be a single-precision 32 bit float between -3.402823e38 and 3.402823e38.",
                [typeof(double)] = "64 bit float '{0}' not formatted correctly, must be a double-precision 64-bit float between -1.79769313486232e308 and 1.79769313486232e308.",
                [typeof(decimal)] = "Decimal '{0}' not formatted correctly, must be a decimal number between -79,228,162,514,264,337,593,543,950,335 and 79,228,162,514,264,337,593,543,950,335.",
                [typeof(DateTime)] = "Date '{0}' not formatted correctly, must be formatted as m/d/yyy h:m:s AM.",
                [typeof(Guid)] = "UUID '{0}' not formatted correctly, should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).",
                [typeof(TimeSpan)] = "Timespan '{0}' not formatted correctly, must be formatted as 'd.h:m:s'.",
                [typeof(Uri)] = "Uri '{0}' not formatted correctly, must be formatted as 'scheme://host'.",

                [typeof(Version)] = "Version '{0}' not formatted correctly, must be formatted as '1.2.3.4'.",
                [typeof(MailAddress)] = "Email address '{0}' not formatted correctly, must be formatted as 'username@domain.com'.",
                [typeof(IPAddress)] = "IP address '{0}' not formatted correctly, must be formatted as '1.2.3.4'."
            };

            FriendlyParseErrorMessages[typeof(IntPtr)] = FriendlyParseErrorMessages[typeof(int)];
            FriendlyParseErrorMessages[typeof(UIntPtr)] = FriendlyParseErrorMessages[typeof(ushort)];

            Readers = new ReaderConventions(options);

            IgnoreUnmatchedElements = true;
            IgnoreUnmatchedMembers = true;
            IgnoreNullsForValueTypes = false;
            IgnoreEmptyCsvValues = false;

            Readers.AddValueReader((v, s, t, o) => Version.Parse(v.ToString()));
            Readers.AddValueReader((v, s, t, o) => new MailAddress(v.ToString()));
            Readers.AddValueReader((v, s, t, o) => IPAddress.Parse(v.ToString()));
            Readers.AddValueReader((v, s, t, o) => Convert.FromBase64String(v.ToString()));
            Readers.AddValueReader((v, s, t, o) => new SqlConnectionStringBuilder(v.ToString()));
        }

        public StringComparison NameComparison { get; set; }
        public StringComparison EnumValueComparison { get; set; }
        
        public bool IgnoreUnmatchedElements { get; set; }
        public bool IgnoreUnmatchedMembers { get; set; }

        public bool IgnoreXmlAttributes { get; set; }
        public bool IgnoreRootName { get; set; }
        public bool IgnoreArrayItemNames { get; set; }
        public bool IgnoreUnmatchedArrayItems { get; set; }
        public bool IgnoreNullsForValueTypes { get; set; }
        public bool IgnoreEmptyCsvValues { get; set; }

        public Dictionary<Type, string> FriendlyParseErrorMessages { get; set; }
        public Func<CachedType, object[], object> ObjectFactory { get; set; }

        public ReaderConventions Readers { get; }
    }
}