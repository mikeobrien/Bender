using System.Linq;
using System.Text;
using Bender;
using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class JsonNodeTests
    {
        // Parse Errors

        [Test]
        public void should_fail_on_unclosed_outer_object()
        {
            var exception = Assert.Throws<ParseException>(() => new JsonNode("{"));
            #if __MonoCS__
                exception.Message.ShouldEqual("1 missing end of arrays or objects (1,2)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: 1 missing end of arrays or objects (1,2)");
            #else
                exception.Message.ShouldEqual("Unexpected end of file. Following elements are not closed: root.");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: Unexpected end of file. Following elements are not closed: root.");
            #endif
        }

        [Test]
        public void should_fail_on_missing_token()
        {
            var exception = Assert.Throws<ParseException>(() => new JsonNode("{ \"yada\" }"));
            #if __MonoCS__
                exception.Message.ShouldEqual("':' is expected after a name of an object content (1,10)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: ':' is expected after a name of an object content (1,10)");
            #else
                exception.Message.ShouldEqual("The token ':' was expected but found '}'.");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: The token ':' was expected but found '}'.");
            #endif
        }

        [Test]
        public void should_fail_on_unclosed_nested_array()
        {
            var exception = Assert.Throws<ParseException>(() => new JsonNode("{ \"yada\": [ }"));
            #if __MonoCS__
                exception.Message.ShouldEqual("Unexpected end of object (1,13)");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: Unexpected end of object (1,13)");
            #else
                exception.Message.ShouldEqual("Encountered unexpected character '}'.");
                exception.FriendlyMessage.ShouldEqual("Unable to parse json: Encountered unexpected character '}'.");
            #endif
        }

        // Misc

        [Test]
        public void should_return_path()
        {
            new JsonNode(NodeType.Object).Path.ShouldEqual("$");
        }

        [Test]
        public void should_set_node_type()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType.ShouldEqual(NodeType.Object);
            node.NodeType = NodeType.Value;
            node.NodeType.ShouldEqual(NodeType.Value);
        }

        [Test]
        [TestCase(NodeType.Object, "object")]
        [TestCase(NodeType.Array, "array")]
        public void should_be_own_type_when_root(NodeType nodeType, string type)
        {
            new JsonNode(nodeType).Type.ShouldEqual(type);
        }

        [Test]
        [TestCase(NodeType.Object, "object")]
        [TestCase(NodeType.Array, "array")]
        [TestCase(NodeType.Value, "value")]
        public void should_be_own_type_when_array_item(NodeType nodeType, string type)
        {
            var node = new JsonNode(NodeType.Array);
            node.Add("item", nodeType, Metadata.Empty, x => { });
            node.First().Type.ShouldEqual(type);
        }

        [Test]
        [TestCase(NodeType.Object)]
        [TestCase(NodeType.Array)]
        [TestCase(NodeType.Value)]
        public void should_be_field_type_when_field(NodeType nodeType)
        {
            var node = new JsonNode(NodeType.Object);
            node.Add("field", nodeType, Metadata.Empty, x => {});
            node.GetNode("field").Type.ShouldEqual("field");
        }

        // Empty root object

        [Test]
        public void should_create_empty_root_object()
        {
            new JsonNode(NodeType.Object).Encode().ShouldEqual("{}");
        }

        [Test]
        public void should_read_empty_root_object()
        {
            var node = new JsonNode("{}");
            node.ShouldTotal(0);
            node.NodeType.ShouldEqual(NodeType.Object);
            node.IsNamed.ShouldBeFalse();
            node.Format.ShouldEqual("json");
        }

        // Empty root array

        [Test]
        public void should_create_empty_root_array()
        {
            new JsonNode(NodeType.Array).Encode().ShouldEqual("[]");
        }

        [Test]
        public void should_read_empty_root_array()
        {
            var node = new JsonNode("[]");
            node.ShouldTotal(0);
            node.NodeType.ShouldEqual(NodeType.Array);
            node.IsNamed.ShouldBeFalse();
            node.Format.ShouldEqual("json");
        }

        // Indexer

        [Test]
        public void should_get_node_by_name()
        {
            new JsonNode("{ \"oh\": \"hai\" }").GetNode("oh").Value.ShouldEqual("hai");
        }

        [Test]
        public void should_return_null_when_there_is_no_match()
        {
            new JsonNode("{ }").GetNode("oh").ShouldBeNull();
        }

        // Add

        [Test]
        public void should_add_node()
        {
            var node = new JsonNode(NodeType.Array);
            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add(NodeType.Value, Metadata.Empty, c),
                x => 
                {
                    x.ShouldNotBeNull();
                    x.IsNamed.ShouldBeFalse();
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Parent.ShouldBeSameAs(node);
                });
        }

        [Test]
        public void should_add_named_node()
        {
            var node = new JsonNode(NodeType.Object);
            node.ShouldExecuteCallback<INode>(
                (x, c) => x.Add("node", NodeType.Value, Metadata.Empty, c),
                x =>
                {
                    x.ShouldNotBeNull();
                    x.Name.ShouldEqual("node");
                    x.NodeType.ShouldEqual(NodeType.Value);
                    x.Parent.ShouldBeSameAs(node);
                });
        }

        // Insert

        [Test]
        public void should_insert_json_node()
        {
            var parent = new JsonNode(NodeType.Array);
            var node = new JsonNode(NodeType.Object);
            parent.ShouldNotExecuteCallback<INode>((s, c) => s.Add(node, c));
            var child = parent.Cast<JsonNode>().First();
            child.Element.ShouldBeSameAs(node.Element);
            child.Parent.ShouldBeSameAs(parent);
        }

        // String values

        [Test]
        public void should_get_field_string_value()
        {
            var node = new JsonNode("{ \"field1\": \"hai\" }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual("hai");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_string_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Add("field2", NodeType.Value, Metadata.Empty, x => x.Value = 'y');
            node.Encode().ShouldEqual("{\"field1\":\"hai\",\"field2\":\"y\"}");
        }

        [Test]
        public void should_get_array_string_value()
        {
            var node = new JsonNode("[\"hai\"]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Value.ShouldEqual("hai");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_string_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType = NodeType.Array;
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = 'y');
            node.Encode().ShouldEqual("[\"hai\",\"y\"]");
        }

        // Numeric values

        [Test]
        public void should_get_field_number_value()
        {
            var node = new JsonNode("{ \"field1\": 42 }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual(42.0m);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_number_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = (decimal)1.1);
            node.Add("field2", NodeType.Value, Metadata.Empty, x => x.Value = (float)2.2);
            node.Add("field3", NodeType.Value, Metadata.Empty, x => x.Value = (double)3.3);
            node.Add("field4", NodeType.Value, Metadata.Empty, x => x.Value = (sbyte)4);
            node.Add("field5", NodeType.Value, Metadata.Empty, x => x.Value = (byte)5);
            node.Add("field6", NodeType.Value, Metadata.Empty, x => x.Value = (short)6);
            node.Add("field7", NodeType.Value, Metadata.Empty, x => x.Value = (ushort)7);
            node.Add("field8", NodeType.Value, Metadata.Empty, x => x.Value = (int)8);
            node.Add("field9", NodeType.Value, Metadata.Empty, x => x.Value = (uint)9);
            node.Add("field10", NodeType.Value, Metadata.Empty, x => x.Value = (long)10);
            node.Add("field11", NodeType.Value, Metadata.Empty, x => x.Value = (ulong)11);
            node.Encode().ShouldEqual("{" +
                "\"field1\":1.1," +
                "\"field2\":2.2," +
                "\"field3\":3.3," +
                "\"field4\":4," +
                "\"field5\":5," +
                "\"field6\":6," +
                "\"field7\":7," +
                "\"field8\":8," +
                "\"field9\":9," +
                "\"field10\":10," +
                "\"field11\":11" +
                "}");
        }

        [Test]
        public void should_get_array_number_value()
        {
            var node = new JsonNode("[42]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Value.ShouldEqual(42.0m);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_number_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType = NodeType.Array;
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (decimal)1.1);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (float)2.2);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (double)3.3);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (sbyte)4);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (byte)5);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (short)6);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (ushort)7);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (int)8);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (uint)9);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (long)10);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = (ulong)11);
            node.Encode().ShouldEqual("[1.1,2.2,3.3,4,5,6,7,8,9,10,11]");
        }

        // Bool values

        [Test]
        public void should_get_field_bool_value()
        {
            var node = new JsonNode("{ \"field1\": true }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual(true);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_bool_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = true);
            node.Encode().ShouldEqual("{\"field1\":true}");
        }

        [Test]
        public void should_get_array_bool_value()
        {
            var node = new JsonNode("[true]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Value.ShouldEqual(true);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_bool_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = true);
            node.Encode().ShouldEqual("[true]");
        }

        // Null values

        [Test]
        public void should_get_field_null_value()
        {
            var node = new JsonNode("{ \"field1\": null }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual(null);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_null_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = null);
            node.Encode().ShouldEqual("{\"field1\":null}");
        }

        [Test]
        public void should_get_array_null_value()
        {
            var node = new JsonNode("[null]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Value.ShouldEqual(null);
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_null_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType = NodeType.Array;
            node.Add(NodeType.Value, Metadata.Empty, x => { });
            node.Encode().ShouldEqual("[null]");
        }

        // Array values

        [Test]
        public void should_get_field_array_value()
        {
            var node = new JsonNode("{ \"field1\": [] }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].NodeType.ShouldEqual(NodeType.Array);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_array_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Array, Metadata.Empty, x => { });
            node.Encode().ShouldEqual("{\"field1\":[]}");
        }

        [Test]
        public void should_get_array_array_value()
        {
            var node = new JsonNode("[[]]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].NodeType.ShouldEqual(NodeType.Array);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_array_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType = NodeType.Array;
            node.Add(NodeType.Array, Metadata.Empty, x => {});
            node.Encode().ShouldEqual("[[]]");
        }

        // Object values

        [Test]
        public void should_get_field_object_value()
        {
            var node = new JsonNode("{ \"field1\": {} }");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].Name.ShouldEqual("field1");
            children[0].NodeType.ShouldEqual(NodeType.Object);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_field_object_value()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType = NodeType.Object;
            node.Add("field1", NodeType.Object, Metadata.Empty, x => {});
            node.Encode().ShouldEqual("{\"field1\":{}}");
        }

        [Test]
        public void should_get_array_object_value()
        {
            var node = new JsonNode("[{}]");
            var children = node.ToList();
            children.ShouldTotal(1);
            children[0].NodeType.ShouldEqual(NodeType.Object);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_set_array_object_value()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType = NodeType.Array;
            node.Add(NodeType.Object, Metadata.Empty, x => {});
            node.Encode().ShouldEqual("[{}]");
        }

        // Multiple values

        [Test]
        public void should_get_multiple_fields()
        {
            var node = new JsonNode("{ \"field1\": \"oh\", \"field2\": \"hai\" }");
            var children = node.ToList();
            children.ShouldTotal(2);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual("oh");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[0].Parent.ShouldBeSameAs(node);
            children[1].Name.ShouldEqual("field2");
            children[1].Value.ShouldEqual("hai");
            children[1].NodeType.ShouldEqual(NodeType.Value);
            children[1].IsNamed.ShouldBeTrue();
            children[1].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_add_named_fields_to_objects()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType.ShouldEqual(NodeType.Object);
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = "oh");
            node.Add("field2", NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Encode().ShouldEqual("{\"field1\":\"oh\",\"field2\":\"hai\"}");
        }

        [Test]
        public void should_fail_to_add_unamed_fields_to_objects()
        {
            var node = new JsonNode(NodeType.Object);
            node.NodeType.ShouldEqual(NodeType.Object);
            Assert.Throws<UnnamedChildrenNotSupportedException>(() => 
                node.Add(NodeType.Value, Metadata.Empty, x => x.Value = "hai"));
        }

        [Test]
        public void should_get_multiple_array_items()
        {
            var node = new JsonNode("[\"oh\", \"hai\"]");
            var children = node.ToList();
            children.ShouldTotal(2);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Value.ShouldEqual("oh");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[0].Parent.ShouldBeSameAs(node);
            children[1].Value.ShouldEqual("hai");
            children[1].NodeType.ShouldEqual(NodeType.Value);
            children[1].Parent.ShouldBeSameAs(node);
        }

        [Test]
        public void should_add_unamed_fields_to_arrays()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType.ShouldEqual(NodeType.Array);
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = "oh");
            node.Add(NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Encode().ShouldEqual("[\"oh\",\"hai\"]");
        }

        [Test]
        public void should_add_named_fields_to_arrays()
        {
            var node = new JsonNode(NodeType.Array);
            node.NodeType.ShouldEqual(NodeType.Array);
            node.Add("item1", NodeType.Value, Metadata.Empty, x => x.Value = "oh");
            node.Add("item2", NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Encode().ShouldEqual("[\"oh\",\"hai\"]");
        }

        // Whitespace

        [Test]
        public void should_read_fields_with_whitespace()
        {
            var children = new JsonNode("{\r\n    \"field1\": \"oh\",\r\n\t\"field2\": \"hai\"\r\n}").ToList();
            children.ShouldTotal(2);
            children[0].Name.ShouldEqual("field1");
            children[0].Value.ShouldEqual("oh");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeTrue();
            children[1].Name.ShouldEqual("field2");
            children[1].Value.ShouldEqual("hai");
            children[1].NodeType.ShouldEqual(NodeType.Value);
            children[1].IsNamed.ShouldBeTrue();
        }

        [Test]
        public void should_read_array_items_with_whitespace()
        {
            var children = new JsonNode("[\r\n    \"oh\",\r\n\t\"hai\"\r\n]").ToList();
            children.ShouldTotal(2);
            children[0].Value.ShouldEqual("oh");
            children[0].NodeType.ShouldEqual(NodeType.Value);
            children[0].IsNamed.ShouldBeFalse();
            children[1].Value.ShouldEqual("hai");
            children[1].NodeType.ShouldEqual(NodeType.Value);
            children[1].IsNamed.ShouldBeFalse();
        }

        [Test, Ignore("Whitespace is not supported in 4.0 but is supported in 4.5.")]
        public void should_save_with_whitespace()
        {
            var node = new JsonNode(NodeType.Object);
            node.Add("field1", NodeType.Value, Metadata.Empty, x => x.Value = "hai");
            node.Encode(Encoding.UTF8, true).ShouldEqual("{\r\n  \"field1\": \"hai\"\r\n}");
        }

        // Field names

        [Test]
        public void should_set_field_name()
        {
            var node = new JsonNode(NodeType.Object);
            node.Add("field2", NodeType.Value, Metadata.Empty, x => x.Value = "");
            node.Encode().ShouldEqual("{\"field2\":\"\"}");
        }

        [Test]
        public void should_get_field_name()
        {
            new JsonNode("{\"field1\": null}").First()
                .Name.ShouldEqual("field1");
        }

        // IsNamed

        [Test]
        public void should_be_named_if_parent_is_object()
        {
            var node = new JsonNode(NodeType.Object);
            node.Add("field1", NodeType.Value, Metadata.Empty, x => { });
            node.First().IsNamed.ShouldBeTrue();
            
            new JsonNode("{\"field1\":1}").First(x => x.Name == "field1").IsNamed.ShouldBeTrue();
        }

        [Test]
        public void should_not_be_named_if_parent_is_array()
        {
            var node = new JsonNode(NodeType.Array);
            node.Add("field1", NodeType.Value, Metadata.Empty, x => { });
            node.First().IsNamed.ShouldBeFalse();

            new JsonNode("[1]").First().IsNamed.ShouldBeFalse();
        }

        [Test]
        public void should_not_be_named_if_doesnt_have_a_parent()
        {
            new JsonNode(NodeType.Object).IsNamed.ShouldBeFalse();
            new JsonNode(NodeType.Array).IsNamed.ShouldBeFalse();
            new JsonNode("{}").IsNamed.ShouldBeFalse();
        }
    }
}
