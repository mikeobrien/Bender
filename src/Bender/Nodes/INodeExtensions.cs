using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Xml;

namespace Bender.Nodes
{
    public static class INodeExtensions
    {
        public static bool IsValue(this INode node)
        {
            return node != null && node.NodeType == NodeType.Value;
        }

        public static bool IsNotValue(this INode node)
        {
            return node == null || node.NodeType != NodeType.Value;
        }

        public static bool IsObject(this INode node)
        {
            return node != null && node.NodeType == NodeType.Object;
        }

        public static bool IsArray(this INode node)
        {
            return node != null && node.NodeType == NodeType.Array;
        }

        public static bool IsVariable(this INode node)
        {
            return node != null && node.NodeType == NodeType.Variable;
        }

        public static bool IsObjectOrVariable(this INode node)
        {
            return node != null && (node.NodeType == NodeType.Object || node.NodeType == NodeType.Variable);
        }

        public static bool IsValueOrVariable(this INode node)
        {
            return node != null && (node.NodeType == NodeType.Value || node.NodeType == NodeType.Variable);
        }

        public static bool IsXml(this INode node)
        {
            return node != null && node.Format == XmlNodeBase.NodeFormat;
        }

        public static bool IsJson(this INode node)
        {
            return node != null && node.Format == JsonNode.NodeFormat;
        }

        public static INode Configure(this INode node, Action<INode> configure)
        {
            configure(node);
            return node;
        }

        public static bool IsNonNullValue(this INode node)
        {
            return node != null && node.IsValue() && node.Value != null;
        }

        public static bool IsNullValue(this INode node)
        {
            return node != null && node.IsValue() && node.Value == null;
        }

        public static IEnumerable<INode> GetUnmatchedNodes(this IEnumerable<INode> target, IEnumerable<INode> source,
            StringComparison compare)
        {
            return target.Exclude(source, (t, s) => t.IsNamed && s.IsNamed && t.Name.Equals(s.Name, compare)).ToList();
        } 

        public static T GetNode<T>(this IEnumerable<T> nodes, string name, 
            StringComparison comparison = StringComparison.Ordinal) where T : INode
        {
            return nodes.FirstOrDefault(x => x.IsNamed && x.Name.Equals(name, comparison));
        }

        public static Stream Encode(this INode node, Encoding encoding = null)
        {
            var stream = new MemoryStream();
            node.Encode(stream, encoding);
            stream.Position = 0;
            return stream;
        }
    }
}
