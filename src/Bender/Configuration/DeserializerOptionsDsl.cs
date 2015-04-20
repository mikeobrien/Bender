using System;
using Bender.Nodes;
using Bender.Nodes.Xml;
using Bender.Reflection;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Bender.Configuration
{
    public class DeserializerOptionsDsl
    {
        private readonly DeserializationOptions _options;

        public DeserializerOptionsDsl(DeserializationOptions options)
        {
            _options = options;
        }

        public DeserializerOptionsDsl WithObjectFactory(Func<CachedType, object[], object> factory)
        {
            _options.ObjectFactory = factory;
            return this;
        }

        public DeserializerOptionsDsl IgnoreNameCase()
        {
            _options.NameComparison = StringComparison.OrdinalIgnoreCase;
            return this;
        }

        public DeserializerOptionsDsl WithNameComparison(StringComparison comparison)
        {
            _options.NameComparison = comparison;
            return this;
        }

        public DeserializerOptionsDsl IgnoreEnumNameCase()
        {
            _options.EnumNameComparison = StringComparison.OrdinalIgnoreCase;
            return this;
        }

        public DeserializerOptionsDsl WithEnumNameComparison(StringComparison comparison)
        {
            _options.EnumNameComparison = comparison;
            return this;
        }

        public DeserializerOptionsDsl FailOnUnmatchedElements()
        {
            _options.IgnoreUnmatchedElements = false;
            return this;
        }

        public DeserializerOptionsDsl FailOnUnmatchedMembers()
        {
            _options.IgnoreUnmatchedMembers = false;
            return this;
        }

        public DeserializerOptionsDsl IgnoreXmlAttributes()
        {
            _options.IgnoreXmlAttributes = true;
            return this;
        }

        public DeserializerOptionsDsl IgnoreRootName()
        {
            _options.IgnoreRootName = true;
            return this;
        }

        public DeserializerOptionsDsl IgnoreArrayItemNames()
        {
            _options.IgnoreArrayItemNames = true;
            return this;
        }

        public DeserializerOptionsDsl WithFriendlyParseErrorMessage<T>(string message)
        {
            _options.FriendlyParseErrorMessages[typeof(T)] = message;
            return this;
        }

        public DeserializerOptionsDsl WithFriendlyParseErrorMessage(Type type, string message)
        {
            _options.FriendlyParseErrorMessages[type] = message;
            return this;
        }

        public DeserializerOptionsDsl TreatDatesAsUtcAndConvertToLocal()
        {
            AddReader((v, s, t, o) => DateTime.Parse(v.ToString()).ToLocalTime(), true);
            return this;
        }

        // Visitors

        public DeserializerOptionsDsl AddVisitor(Action<INode, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader(visitor);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader(visitor, when);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor(Action<JsonNode, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson());
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor(Action<XmlNodeBase, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml());
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader<T>(visitor);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>(visitor, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader<T>(visitor, when);
            return this;
        }

        public DeserializerOptionsDsl AddVisitor<T>(Action<INode, NodeBase, Options> visitor,
            Func<INode, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>(visitor, when, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson());
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o), 
                (s, t, o) => s.IsJson(), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o),
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddJsonVisitor<T>(Action<JsonNode, NodeBase, Options> visitor,
            Func<JsonNode, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((JsonNode)s, t, o),
                (s, t, o) => s.IsJson() && when((JsonNode)s, t, o), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor)
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml());
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o), 
                (s, t, o) => s.IsXml(), handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when)
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o),
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o));
            return this;
        }

        public DeserializerOptionsDsl AddXmlVisitor<T>(Action<XmlNodeBase, NodeBase, Options> visitor,
            Func<XmlNodeBase, NodeBase, Options, bool> when,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddVisitingReader<T>((s, t, o) => visitor((XmlNodeBase)s, t, o),
                (s, t, o) => s.IsXml() && when((XmlNodeBase)s, t, o), handleNullable);
            return this;
        }

        // Readers

        public DeserializerOptionsDsl AddReader(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> when)
        {
            _options.Readers.AddReader(reader, when);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader)
        {
            _options.Readers.AddReader<T>(reader);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            bool handleNullable) where T : struct
        {
            _options.Readers.AddReader<T>(reader, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> where)
        {
            _options.Readers.AddReader<T>(reader, where);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Action<INode, NodeBase, Options> reader,
            Func<INode, NodeBase, Options, bool> where, bool handleNullable) where T : struct
        {
            _options.Readers.AddReader<T>(reader, where, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader)
        {
            _options.Readers.AddValueReader(reader);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader, 
            bool handleNullable) where T : struct
        {
            _options.Readers.AddValueReader(reader, handleNullable);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader,
            Func<object, INode, NodeBase, Options, bool> where)
        {
            _options.Readers.AddValueReader(reader, where);
            return this;
        }

        public DeserializerOptionsDsl AddReader<T>(Func<object, INode, NodeBase, Options, T> reader,
            Func<object, INode, NodeBase, Options, bool> where, bool handleNullable) where T : struct
        {
            _options.Readers.AddValueReader(reader, where, handleNullable);
            return this;
        }
    }
}