using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void should_correctly_format_type_not_supported_deserialization_exception_message()
        {
            new TypeNotSupportedException("dictionary", typeof(string).ToCachedType(), 
                    Mode.Deserialize, "dictionaries")
                .Message.ShouldEqual("Dictionary 'System.String' is not supported for " +
                                     "deserialization. Only dictionaries can be deserialized.");
        }

        [Test]
        public void should_correctly_format_type_not_supported_serialization_exception_message()
        {
            new TypeNotSupportedException("dictionary", typeof(string).ToCachedType(),
                    Mode.Serialize, "dictionaries")
                .Message.ShouldEqual("Dictionary 'System.String' is not supported for " +
                                     "serialization. Only dictionaries can be serialized.");
        }
    }
}
