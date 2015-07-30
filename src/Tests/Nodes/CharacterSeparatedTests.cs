using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.CharacterSeparated;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class CharacterSeparatedTests
    {
        public class Record
        {
            public string Field;
            public string Property { get; set; }
        }

        private const string Data = "\"Property\",\"Field\"\r\n\"oh,\",\"\"\"hai\"\"\"\r\n";

        [Test]
        public void should_encode_csv()
        {
            var file = new FileNode(NodeType.Array, 
                typeof(List<Record>), Options.Create(x => 
                x.IncludePublicFields()));
            var row = new RowNode(1)
            {
                { new ValueNode("Property", "oh,"), x => {} },
                { new ValueNode("Field", "\"hai\""), x => {} }
            };
            file.Add(row, x => {});
            file.EncodeToString().ShouldEqual(Data);
        }

        [Test]
        public void should_parse_csv()
        {
            var fileNode = new FileNode(new MemoryStream(
                Encoding.ASCII.GetBytes(Data)), Options.Create());

            fileNode.NodeType.ShouldEqual(NodeType.Array);
            var rowNodes = fileNode.ToList();
            rowNodes.Count.ShouldEqual(1);
            var rowNode = rowNodes.First();
            rowNode.NodeType.ShouldEqual(NodeType.Object);
            var rowValues = rowNode.ToList();
            rowValues.Count.ShouldEqual(2);
            var rowValue = rowValues[0];
            rowValue.NodeType.ShouldEqual(NodeType.Value);
            rowValue.Value.ShouldEqual("oh,");
            rowValue = rowValues[1];
            rowValue.NodeType.ShouldEqual(NodeType.Value);
            rowValue.Value.ShouldEqual("\"hai\"");
        }
    }
}
