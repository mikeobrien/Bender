using System;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Bender.Configuration
{
    public class DeserializerOptionsDsl
    {
        private readonly Options _options;

        public DeserializerOptionsDsl(Options options)
        {
            _options = options;
        }

        public DeserializerOptionsDsl WithObjectFactory(Func<CachedType, object[], object> factory)
        {
            _options.Deserialization.ObjectFactory = factory;
            return this;
        }

        public DeserializerOptionsDsl IgnoreNameCase()
        {
            _options.Deserialization.NameComparison = StringComparison.OrdinalIgnoreCase;
            return this;
        }

        public DeserializerOptionsDsl WithNameComparison(StringComparison comparison)
        {
            _options.Deserialization.NameComparison = comparison;
            return this;
        }

        public DeserializerOptionsDsl WithCaseSensitiveEnumValues()
        {
            _options.Deserialization.EnumValueComparison = StringComparison.Ordinal;
            return this;
        }

        public DeserializerOptionsDsl WithEnumValueComparison(StringComparison comparison)
        {
            _options.Deserialization.EnumValueComparison = comparison;
            return this;
        }

        public DeserializerOptionsDsl FailOnUnmatchedElements()
        {
            _options.Deserialization.IgnoreUnmatchedElements = false;
            return this;
        }

        public DeserializerOptionsDsl FailOnUnmatchedMembers()
        {
            _options.Deserialization.IgnoreUnmatchedMembers = false;
            return this;
        }

        public DeserializerOptionsDsl IgnoreUnmatchedArrayItems()
        {
            _options.Deserialization.IgnoreUnmatchedArrayItems = true;
            return this;
        }

        public DeserializerOptionsDsl IgnoreXmlAttributes()
        {
            _options.Deserialization.IgnoreXmlAttributes = true;
            return this;
        }

        public DeserializerOptionsDsl IgnoreRootName()
        {
            _options.Deserialization.IgnoreRootName = true;
            return this;
        }

        public DeserializerOptionsDsl IgnoreArrayItemNames()
        {
            _options.Deserialization.IgnoreArrayItemNames = true;
            return this;
        }

        public DeserializerOptionsDsl WithFriendlyParseErrorMessage<T>(string message)
        {
            _options.Deserialization.FriendlyParseErrorMessages[typeof(T)] = message;
            return this;
        }

        public DeserializerOptionsDsl WithFriendlyParseErrorMessage(Type type, string message)
        {
            _options.Deserialization.FriendlyParseErrorMessages[type] = message;
            return this;
        }

        public DeserializerOptionsDsl TreatDatesAsUtcAndConvertToLocal()
        {
            AddReader((v, s, t, o) => DateTime.Parse(v.ToString()).ToLocalTime(), true);
            return this;
        }

        // Enum naming conventions

        public DeserializerOptionsDsl WithEnumNamingConvention(
            Func<string, string> convention)
        {
            _options.EnumValueNameConventions.Add(
                (v, c) => convention(v),
                (v, c) => c.Mode == Mode.Deserialize);
            return this;
        }

        public DeserializerOptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention)
        {
            _options.EnumValueNameConventions.Add(convention,
                (v, c) => c.Mode == Mode.Deserialize);
            return this;
        }

        public DeserializerOptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention,
            Func<string, EnumContext, bool> when)
        {
            _options.EnumValueNameConventions.Add(convention,
                (v, c) => c.Mode == Mode.Deserialize && when(v, c));
            return this;
        }

        public DeserializerOptionsDsl UseEnumSnakeCaseNaming()
        {
            return WithEnumNamingConvention((v, c) => v.Replace("_", ""));
        }

        // Visitors

        public DeserializerOptionsDsl AddVisitor(Action<INode, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader(visitor);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader(visitor, when);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor(Action<JsonNode, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson());
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor(Action<XmlNodeBase, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml());
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>(visitor);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>(visitor, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>(visitor, when);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>(visitor, when, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson());
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson(), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o),
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o),
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml());
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml(), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o),
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o),
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o), handleNullable);
            return this;
        }

        // Readers

        public DeserializerOptionsDsl AddReader(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Deserialization.Readers.AddReader(reader, when);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader)
        {
            _options.Deserialization.Readers.AddReader<T>(reader);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddReader<T>(reader, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> where)
        {
            _options.Deserialization.Readers.AddReader<T>(reader, where);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> where, bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddReader<T>(reader, where, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, T> reader)
        {
            _options.Deserialization.Readers.AddValueReader((v, s, t, o) => reader(v));
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader)
        {
            _options.Deserialization.Readers.AddValueReader(reader);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader, 
            bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddValueReader(reader, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader,
            Func<object, INode, NodeBase, Options, bool> where)
        {
            _options.Deserialization.Readers.AddValueReader(reader, where);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader,
            Func<object, INode, NodeBase, Options, bool> where, bool handleNullable) where T : struct
        {
            _options.Deserialization.Readers.AddValueReader(reader, where, handleNullable);
            return this;
        }
    }
}