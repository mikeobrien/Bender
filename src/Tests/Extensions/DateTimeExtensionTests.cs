using System;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Extensions
{
    [TestFixture]
    public class DateTimeExtensionTests
    {
        [Test]
        public void should_indicate_if_string_cast_as_object_is_null_or_empty()
        {
            new DateTime(1985, 10, 26, 5, 21, 0).SubtractUtcOffset()
                .ToMicrosoftJsonDateFormat().ShouldEqual("/Date(499152060000)/");
        }

        [Test]
        public void Should_parse_microsoft_json_datetime()
        {
            "/Date(499800861000)/".TryParseMicrosoftJsonDateFormat()
                .ShouldBeWithinSeconds(new DateTime(1985, 11, 2, 12, 34, 21, DateTimeKind.Local));
        }

        [TestCase(null)]
        [TestCase(5)]
        [TestCase("")]
        [TestCase("fark")]
        public void Should_return_null_if_not_a_microsoft_json_datetime(object input)
        {
            input.TryParseMicrosoftJsonDateFormat().ShouldBeNull();
        }
        
        [TestCase("/Date(")]
        [TestCase(")/")]
        [TestCase("/Date()/")]
        [TestCase("/Date(fark)/")]
        [TestCase("/Date(499797261000499797261000499797261000499797261000)/")]
        public void Should_throw_format_exception_if_a_microsoft_json_datetime_and_not_formatted_correctly(object input)
        {
            Assert.Throws<FormatException>(() => input.TryParseMicrosoftJsonDateFormat())
                .Message.ShouldContain("formatted");
        }

        [TestCase(long.MaxValue)]
        [TestCase(long.MinValue)]
        public void Should_throw_format_exception_if_a_microsoft_json_datetime_and_out_of_range(long input)
        {
            Assert.Throws<FormatException>(() => $"/Date({input})/".TryParseMicrosoftJsonDateFormat())
                .Message.ShouldContain("range");
        }
    }
}
