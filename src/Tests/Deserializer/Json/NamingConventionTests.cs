using System;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class NamingConventionTests
    {
        public class PascalModel
        {
            public string FarkFarker { get; set; }
        }

        public class CamelModel
        {
            public string farkFarker { get; set; }
        }

        [Test]
        public void Should_deserialze_camel_case_to_pascal_when_using_camel_case_naming_convention()
        {
            var result = "{\"farkFarker\":\"fark\"}"
                .DeserializeJson<PascalModel>(x => x
                    .UseCamelCaseNaming()
                    .Deserialization(d => d.FailOnUnmatchedElements()));

            result.FarkFarker.ShouldEqual("fark");
        }

        [Test]
        public void Should_deserialze_camel_case_to_camel_when_using_camel_case_naming_convention()
        {
            var result = "{\"farkFarker\":\"fark\"}"
                .DeserializeJson<CamelModel>(x => x
                    .UseCamelCaseNaming()
                    .Deserialization(d => d.FailOnUnmatchedElements()));

            result.farkFarker.ShouldEqual("fark");
        }

        [Test]
        public void Should_fail_to_deserialze_camel_case_to_pascal_when_using_pascal_case_naming_convention()
        {
            Assert.Throws<FriendlyMappingException>(() => 
                "{\"farkFarker\":\"fark\"}"
                    .DeserializeJson<PascalModel>(x => x
                        .UsePascalCaseNaming()
                        .Deserialization(d => d.FailOnUnmatchedElements())));
        }

        [Test]
        public void Should_fail_to_deserialze_camel_case_to_camel_when_using_pascal_case_naming_convention()
        {
            Assert.Throws<FriendlyMappingException>(() =>
                "{\"farkFarker\":\"fark\"}"
                    .DeserializeJson<CamelModel>(x => x
                        .UsePascalCaseNaming()
                        .Deserialization(d => d.FailOnUnmatchedElements())));
        }

        [Test]
        public void Should_deserialze_pascal_case_to_pascal_when_using_pascal_case_naming_convention()
        {
            var result = "{\"FarkFarker\":\"fark\"}"
                .DeserializeJson<PascalModel>(x => x
                    .UsePascalCaseNaming()
                    .Deserialization(d => d.FailOnUnmatchedElements()));

            result.FarkFarker.ShouldEqual("fark");
        }

        [Test]
        public void Should_deserialze_pascal_case_to_camel_when_using_pascal_case_naming_convention()
        {
            var result = "{\"FarkFarker\":\"fark\"}"
                .DeserializeJson<CamelModel>(x => x
                    .UsePascalCaseNaming()
                    .Deserialization(d => d.FailOnUnmatchedElements()));

            result.farkFarker.ShouldEqual("fark");
        }

        [Test]
        public void Should_fail_to_deserialze_pascal_case_to_camel_when_using_camel_case_naming_convention()
        {
            Assert.Throws<FriendlyMappingException>(() =>
                "{\"FarkFarker\":\"fark\"}"
                    .DeserializeJson<CamelModel>(x => x
                        .UseCamelCaseNaming()
                        .Deserialization(d => d.FailOnUnmatchedElements())));
        }

        [Test]
        public void Should_fail_to_deserialze_pascal_case_to_pascal_when_using_camel_case_naming_convention()
        {
            Assert.Throws<FriendlyMappingException>(() =>
                "{\"FarkFarker\":\"fark\"}"
                    .DeserializeJson<PascalModel>(x => x
                        .UseCamelCaseNaming()
                        .Deserialization(d => d.FailOnUnmatchedElements())));
        }
    }
}
