using System;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes.Object.Values;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class InvalidRootNameDeserializationException : FriendlyBenderException
    {
        public const string FriendlyMessageFormat = "{0} root name '{1}' does not match expected name of '{2}'.";
        public const string MessageFormat = FriendlyMessageFormat + " Deserializing '{3}'.";

        public InvalidRootNameDeserializationException(CachedType type, string format, string actual, string expected) :
            base(MessageFormat, FriendlyMessageFormat, format.ToInitialCaps(), actual, expected, type.FriendlyFullName) { }
    }

    public static class NodeFactory
    {
        // Serializable nodes

        public static ObjectNodeBase CreateSerializableRoot(object @object, CachedType type, Options options, string format)
        {
            var context = new Context(options, Mode.Serialize, format);
            return CreateSerializable(
                context.Options.TypeNameConventions.GetName(type, context, true), 
                ValueFactory.Create(@object, type, true, context.Options), null, context).As<ObjectNodeBase>();
        }

        public static ObjectNodeBase CreateSerializable(string name, IValue @object, INode parent,
            Context context, CachedMember member = null)
        {
            switch (GetTypeKind(@object.SpecifiedType, context.Options))
            {
                case TypeKind.Simple:
                    if (parent == null) throw new TypeNotSupportedException("simple type", @object.SpecifiedType, Mode.Serialize, "complex types");
                    return new ValueNode(context, name, @object, member, parent);
                case TypeKind.Dictionary: return new DictionaryNode(context, name, @object, member, parent);
                case TypeKind.Enumerable: return new EnumerableNode(context, name, @object, member, parent);
                default: return new ObjectNode(context, name, @object, member, parent);
            }
        }

        // Deserializable nodes

        public static ObjectNodeBase CreateDeserializableRoot(CachedType type, string format, Options options)
        {
            return CreateDeserializable(null, ValueFactory.Create(type), null, new Context(options, Mode.Deserialize, format));
        }

        public static ObjectNodeBase CreateDeserializableRoot(string name, CachedType type, string format, Options options)
        {
            var context = new Context(options, Mode.Deserialize, format);
            var @object = ValueFactory.Create(type);
            if (!context.Options.Deserialization.IgnoreRootName)
            {
                var expectedName = context.Options.TypeNameConventions.GetName(@object.SpecifiedType, context, true);
                if (!name.Equals(expectedName, options.Deserialization.NameComparison)) 
                    throw new InvalidRootNameDeserializationException(type, format, name, expectedName);
            }
            return CreateDeserializable(name, @object, null, context);
        }

        public static ObjectNodeBase CreateDeserializable(string name, IValue @object, INode parent,
             Context context, CachedMember member = null)
        {
            var type = @object.SpecifiedType;
            var kind = GetTypeKind(type, context.Options);

            if (kind == TypeKind.Simple)
            {
                if (parent == null) throw new TypeNotSupportedException("simple type", 
                    @object.SpecifiedType, Mode.Deserialize, "complex types");
                return new ValueNode(context, name, @object, member, parent);
            }

            Func<object> factory = () => ObjectFactory.CreateInstance(type,
                context.Options.Deserialization.ObjectFactory, parent.MapOrDefault(x => x.Value));

            if (member == null || parent == null) @object.Instance = factory();
            else @object = ValueFactory.Create(@object, factory);

            switch (kind)
            {
                case TypeKind.Dictionary: return new DictionaryNode(context, name, @object, member, parent);
                case TypeKind.Enumerable: return new EnumerableNode(context, name, @object, member, parent);
                default: return new ObjectNode(context, name, @object, member, parent);
            }
        }

        private static TypeKind GetTypeKind(CachedType type, Options options)
        {
            return type.GetKind(
                options.TreatEnumerableImplsAsObjects,
                options.TreatDictionaryImplsAsObjects);
        }
    }
}
