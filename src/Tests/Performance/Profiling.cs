using System.IO;
using System.Linq;
using Bender.Collections;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Xml;
using NUnit.Framework;

namespace Tests.Performance
{
    [TestFixture]
    public class Profiling
    {
        private object _object;
        private INode _jsonNode;
        private INode _xmlNode;

        [SetUp]
        public void Setup()
        {
            var json = File.ReadAllText(@"../../Performance/model.json");
            _jsonNode = new JsonNode(json);

            Enumerable.Range(0, 10).ForEach(x =>
            {
                _object = Bender.Deserializer.Create().Deserialize(_jsonNode, typeof(Model<string>));
                Bender.Serializer.Create().SerializeNodes(_object, (n, o) =>
                    new JsonNode(n.NodeType, new Options()), JsonNode.NodeFormat);
            });

            var xml = File.ReadAllText(@"../../Performance/model.xml");
            _xmlNode = ElementNode.Parse(xml, Options.Create());

            Enumerable.Range(0, 10).ForEach(x =>
            {
                _object = Bender.Deserializer.Create().Deserialize(_xmlNode, typeof(Model<string>));
                Bender.Serializer.Create().SerializeNodes(_object, (n, o) =>
                    ElementNode.Create(n.Name, Metadata.Empty, Options.Create()), XmlNodeBase.NodeFormat);
            });
        }

        [Test]
        public void profile_deserialize_json()
        {
            _object = Bender.Deserializer.Create().Deserialize(_jsonNode, typeof(Model<string>));
        }

        [Test]
        public void profile_serialize_json()
        {
            _jsonNode = Bender.Serializer.Create().SerializeNodes(_object, (n, o) =>
                new JsonNode(n.NodeType, new Options()), JsonNode.NodeFormat);
        }

        [Test]
        public void profile_deserialize_xml()
        {
            _object = Bender.Deserializer.Create().Deserialize(_xmlNode, typeof(Model<string>));
        }

        [Test]
        public void profile_serialize_xml()
        {
            _xmlNode = Bender.Serializer.Create().SerializeNodes(_object, (n, o) =>
                ElementNode.Create(n.Name, Metadata.Empty, Options.Create()), XmlNodeBase.NodeFormat);
        }
    }
}
