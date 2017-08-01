using System;
using System.IO;
using System.Text;
using Bender.Extensions;
using NUnit.Framework;
using Should;

namespace Tests.Extensions
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void should_indicate_if_string_cast_as_object_is_null_or_empty(object value)
        {
            value.IsNullOrEmpty().ShouldBeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void should_indicate_if_string_is_null_or_empty(string value)
        {
            value.IsNullOrEmpty().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_if_string_is_not_null_or_empty()
        {
            "oh hai".IsNullOrEmpty().ShouldBeFalse();
        }

        [Test]
        public void should_indicate_if_string_is_empty()
        {
            "".IsEmpty().ShouldBeTrue();
        }

        [Test]
        [TestCase(null)]
        [TestCase("oh hai")]
        public void should_indicate_if_string_is_not_empty(string value)
        {
            value.IsEmpty().ShouldBeFalse();
        }

        [Test]
        [TestCase("oh {0}", "oh hai")]
        [TestCase("oh", "oh")]
        public void should_format(string format, string result)
        {
            format.ToFormat("hai").ShouldEqual(result);
        }

        [Test]
        public void should_remove_new_lines()
        {
            ("oh" + Environment.NewLine + "hai").RemoveNewLines().ShouldEqual("oh hai");
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("h", "H")]
        [TestCase("hai", "Hai")]
        public void should_initial_cap(string value, string result)
        {
            value.ToInitialCap().ShouldEqual(result);
        }

        [Test]
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("URLValue", "urlValue")]
        [TestCase("URL", "url")]
        [TestCase("ID", "id")]
        [TestCase("I", "i")]
        [TestCase("Person", "person")]
        [TestCase("iPhone", "iPhone")]
        [TestCase("IPhone", "iPhone")]
        [TestCase("I Phone", "i Phone")]
        [TestCase("I  Phone", "i  Phone")]
        [TestCase(" IPhone", " IPhone")]
        [TestCase(" IPhone ", " IPhone ")]
        [TestCase("IsCIA", "isCIA")]
        [TestCase("VmQ", "vmQ")]
        [TestCase("Xml2Json", "xml2Json")]
        [TestCase("SnAkEcAsE", "snAkEcAsE")]
        [TestCase("SnA__kEcAsE", "snA__kEcAsE")]
        [TestCase("SnA__ kEcAsE", "snA__ kEcAsE")]
        [TestCase("already_snake_case_ ", "already_snake_case_ ")]
        [TestCase("IsJSONProperty", "isJSONProperty")]
        [TestCase("SHOUTING_CASE", "shoutinG_CASE")]
        [TestCase("9999-12-31T23:59:59.9999999Z", "9999-12-31T23:59:59.9999999Z")]
        [TestCase("Hi!! This is text. Time to test.", "hi!! This is text. Time to test.")]
        public void should_camel_case(string source, string result)
        {
            source.ToCamelCase().ShouldEqual(result);
        }

        [Test]
        [TestCase("OhHai", "Oh_Hai", false, "_")]
        [TestCase("OhHai", "Oh-Hai", false, "-")]
        [TestCase("OhHai", "oh_hai", true, "_")]
        [TestCase("OhHai", "oh-hai", true, "-")]
        [TestCase("ohHai", "oh_Hai", false, "_")]
        [TestCase("ohHai", "oh-Hai", false, "-")]
        [TestCase("ohHai", "oh_hai", true, "_")]
        [TestCase("ohHai", "oh-hai", true, "-")]
        [TestCase("ABC", "abc", true, "-")]
        [TestCase("ABC", "ABC", false, "-")]
        [TestCase("IAm", "i-am", true, "-")]
        [TestCase("IAm", "I-Am", false, "-")]
        [TestCase("O", "O", false, "-")]
        [TestCase("O", "o", true, "-")]
        [TestCase("Oh", "oh", true, "-")]
        [TestCase("Oh", "Oh", false, "-")]
        [TestCase("", "", true, "-")]
        public void should_snake_case(string source, string result, bool lower, string seperator)
        {
            source.ToSeparatedCase(lower, seperator).ShouldEqual(result);
        }

        [Test]
        public void should_convert_string_to_stream()
        {
            new StreamReader("oh hai".ToStream()).ReadToEnd().ShouldEqual("oh hai");
        }

        [Test]
        public void should_convert_stream_to_string()
        {
            "oh hai".ToStream().ReadToEnd().ShouldEqual("oh hai");
        }

        [Test]
        public void should_convert_memory_stream_to_byte_array()
        {
            "oh hai".ToStream().ReadAllBytes().ShouldEqual(Encoding.UTF8.GetBytes("oh hai"));
        }

        [Test]
        [TestCase(null, "a")]
        [TestCase("", "a")]
        [TestCase("h", "a")]
        [TestCase("hai", "a")]
        [TestCase("a", "an")]
        [TestCase("e", "an")]
        [TestCase("i", "an")]
        [TestCase("o", "an")]
        [TestCase("u", "a")]
        [TestCase("A", "an")]
        [TestCase("E", "an")]
        [TestCase("I", "an")]
        [TestCase("O", "an")]
        [TestCase("U", "a")]
        public void should_get_article(string value, string article)
        {
            value.GetArticle().ShouldEqual(article);
        }


        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("ohhai", "ohhai")]
        [TestCase("oh hai", "oh ha...")]
        public void should_truncate(object source, string truncated)
        {
            source.Truncate(5).ShouldEqual(truncated);
        }
    }
}
