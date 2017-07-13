using System;

namespace Bender.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToMicrosoftJsonDateFormat(this DateTime date)
        {
            return "/Date({0})/".ToFormat((long)(date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }

        public static DateTime? TryParseMicrosoftJsonDateFormat(this object value)
        {
            var source = (value as string)?.Trim();

            if (source == null) return null;

            var hasOpen = source.StartsWith("/Date(",
                StringComparison.OrdinalIgnoreCase);
            var hasClose = source.EndsWith(")/");

            if (!hasOpen && !hasClose) return null;

            long epoch;
            if (hasOpen && hasClose && source.Length > 8 &&
                long.TryParse(source.Substring(6, source.Length - 8), out epoch))
            {
                try
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0, 0)
                        .AddMilliseconds(epoch).ToLocalTime();
                }
                catch
                {
                    throw new FormatException($"Datetime '{source}' is out of range.");
                }
            }
            throw new FormatException($"Datetime '{source}' is not formatted correctly. " +
                                      "Should be formatted as /Date(499797261000)/.");
        }
    }
}
