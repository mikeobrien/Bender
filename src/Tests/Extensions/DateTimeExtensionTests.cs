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
            new DateTime(1985, 10, 26, 1, 21, 0).ToMicrosoftJsonDateFormat().ShouldEqual("/Date(499152060000)/");
        }
    }
}
