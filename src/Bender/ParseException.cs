using System;
using System.Collections.Generic;
using System.Xml;
using Bender.Collections;
using Bender.Extensions;

namespace Bender
{
    public class ParseException : FriendlyBenderException
    {
        public ParseException(Exception exception, string format, int line = 0, int column = 0) :
            base(exception, exception.Message, BuildMessage(exception, format, line, column)) { }

        private static string BuildMessage(Exception exception, string format, int line = 0, int column = 0)
        {
            return "Unable to parse {0}{1}: {2}".ToFormat(
                format.ToLower(),
                new List<string>()
                    .Add("Line: " + line, line > 0)
                    .Add("Column: " + column, column > 0)
                    .Aggregate(" (", ", ", ")"),
                exception.Message);
        }
    }

    public static class ParseExceptionExtensions
    {
        public static ParseException ToParseException(this XmlException exception, string format)
        {
            return new ParseException(exception, format, exception.LineNumber, exception.LinePosition);
        }
    }
}
