using System.IO;
using System.Linq;
using Bender.Nodes;
using Bender.Nodes.FormUrlEncoded;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class FormUrlEncodedTests
    {
        [Test]
        public void should_parse_form()
        {
            var form = new FormNode("oh=hai&john=zoidberg");
            form.NodeType.ShouldEqual(NodeType.Object);
            var values = form.ToList();
            values.Count.ShouldEqual(2);

            var value = values.First();
            value.Name.ShouldEqual("oh");
            value.Value.ShouldEqual("hai");
            value.NodeType.ShouldEqual(NodeType.Value);

            value = values[1];
            value.Name.ShouldEqual("john");
            value.Value.ShouldEqual("zoidberg");
            value.NodeType.ShouldEqual(NodeType.Value);
        }

        [Test]
        public void should_encode_form()
        {
            var form = new FormNode(NodeType.Object)
            {
                { "oh", NodeType.Value, Metadata.Empty, x => x.Value = "hai" },
                { new FormValueNode("john", "zoidberg"), x => { } }
            };
            var stream = new MemoryStream();
            form.Encode(stream);
            stream.Position = 0;
            new StreamReader(stream).ReadToEnd().ShouldEqual("oh=hai&john=zoidberg");
        }
    }
}
