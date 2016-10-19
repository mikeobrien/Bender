using System;
using Bender.Configuration;
using Bender.Nodes;

namespace Bender
{
    public static partial class Deserialize
    {
        public static T Node<T>(INode node, Options options)
        {
            return Deserializer.Create(options).Deserialize<T>(node);
        }

        public static T DeserializeNode<T>(this INode node, Options options)
        {
            return Deserializer.Create(options).Deserialize<T>(node);
        }

        public static T Node<T>(INode node, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).Deserialize<T>(node);
        }

        public static T DeserializeNode<T>(this INode node, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).Deserialize<T>(node);
        }

        public static object Node(INode node, Type type, Options options)
        {
            return Deserializer.Create(options).Deserialize(node, type);
        }

        public static object DeserializeNode(this INode node, Type type, Options options)
        {
            return Deserializer.Create(options).Deserialize(node, type);
        }

        public static object Node(INode node, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).Deserialize(node, type);
        }

        public static object DeserializeNode(this INode node, Type type, Action<OptionsDsl> configure = null)
        {
            return Deserializer.Create(configure).Deserialize(node, type);
        }
    }
}
