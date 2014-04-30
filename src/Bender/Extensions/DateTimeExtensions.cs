using System;

namespace Bender.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToMicrosoftJsonDateFormat(this DateTime date)
        {
            return "/Date({0})/".ToFormat((long)(date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }
    }
}
