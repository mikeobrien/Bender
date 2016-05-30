using System.Xml.Linq;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Xml;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void should_use_serialization_mapping_exception_message()
        {
            new MappingException(new BenderException("exception message"), new Node("object"), 
                    new ElementNode(new XElement("oh", new XElement("hai")).Element("hai"), Options.Create()), 
                    Mode.Serialize)
                .Message.ShouldEqual(MappingException.SerializationMessageFormat
                    .ToFormat("object", "xml", "element", "/oh/hai") + "exception message");
        }

        [Test]
        public void should_use_deserialization_mapping_exception_message()
        {
            new MappingException(new BenderException("exception message"),
                    new ElementNode(new XElement("oh", new XElement("hai")).Element("hai"), Options.Create()),
                    new Node("object"), Mode.Deserialize)
                .Message.ShouldEqual(MappingException.DeserializationMessageFormat
                    .ToFormat("xml", "element", "/oh/hai", "object") + "exception message");
        }

        [Test]
        public void should_use_serialization_friendly_mapping_exception_message()
        {
            var exception = new FriendlyMappingException(new FriendlyBenderException("exception message", "friendly message"), new Node("object"),
                    new ElementNode(new XElement("oh", new XElement("hai")).Element("hai"), Options.Create()),
                    Mode.Serialize);
            
            exception.Message.ShouldEqual(MappingException.SerializationMessageFormat
                    .ToFormat("object", "xml", "element", "/oh/hai") + "exception message");

            exception.FriendlyMessage.ShouldEqual(FriendlyMappingException.FriendlyMessageFormat
                    .ToFormat("write", "xml", "element", "/oh/hai") + "friendly message");
        }

        [Test]
        public void should_use_deserialization_friendly_mapping_exception_message()
        {
            var exception = new FriendlyMappingException(new FriendlyBenderException("exception message", "friendly message"),
                    new ElementNode(new XElement("oh", new XElement("hai")).Element("hai"), Options.Create()),
                    new Node("object"), Mode.Deserialize);

            exception.Message.ShouldEqual(MappingException.DeserializationMessageFormat
                    .ToFormat("xml", "element", "/oh/hai", "object") + "exception message");

            exception.FriendlyMessage.ShouldEqual(FriendlyMappingException.FriendlyMessageFormat
                    .ToFormat("read", "xml", "element", "/oh/hai") + "friendly message");
        }
    }
}
