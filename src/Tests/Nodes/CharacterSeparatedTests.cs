﻿using System.Collections.Generic;
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
            public string Null { get; set; }
        }

        private const string Data = "\"Property\",\"Null\"," + 
            "\"Field\"\r\n\"oh,\",,\"\"\"hai\"\"\"\r\n";

        [Test]
        public void should_encode_csv()
        {
            var result = new StringBuilder();
            var writer = new StringWriter(result);
            var file = new FileNode(NodeType.Array, 
                typeof(List<Record>), typeof(List<Record>), 
                Options.Create(x => x.IncludePublicFields()), writer);
            var row = new RowNode(1)
            {
                { new ValueNode("Property", "Property", null, "oh,"), x => {} },
                { new ValueNode("Field", "Field", null, "\"hai\""), x => {} },
                { new ValueNode("Null", "Null", null), x => {} }
            };
            file.Add(row, x => {});
            writer.Flush();
            result.ToString().ShouldEqual(Data);
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
            rowNode.NodeType.ShouldEqual(NodeType.Variable);
            var rowValues = rowNode.ToList();
            rowValues.Count.ShouldEqual(3);
            var rowValue = rowValues[0];
            rowValue.NodeType.ShouldEqual(NodeType.Value);
            rowValue.Value.ShouldEqual("oh,");
            rowValue = rowValues[1];
            rowValue.NodeType.ShouldEqual(NodeType.Value);
            rowValue.Value.ShouldBeNull();
            rowValue = rowValues[2];
            rowValue.NodeType.ShouldEqual(NodeType.Value);
            rowValue.Value.ShouldEqual("\"hai\"");
        }
    }
}
