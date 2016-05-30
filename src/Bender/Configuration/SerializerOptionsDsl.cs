using System;
using System.Xml.Linq;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes;
using Bender.Nodes.Xml;
using NodeBase = Bender.Nodes.Object.ObjectNodeBase;

namespace Bender.Configuration
{
    public class SerializerOptionsDsl
    {
        private readonly Options _options;

        public SerializerOptionsDsl(Options options)
        {
            _options = options;
        }

        public SerializerOptionsDsl PrettyPrint()
        {
            _options.Serialization.PrettyPrint = true;
            return this;
        }

        public SerializerOptionsDsl IncludeNullMembers()
        {
            _options.Serialization.IncludeNullMembers = true;
            return this;
        }

        public SerializerOptionsDsl UseActualType()
        {
            _options.Serialization.SerializationType = SerializationType.ActualType;
            return this;
        }

        public SerializerOptionsDsl WriteDateTimeAsUtcIso8601()
        {
            AddWriter<DateTime>((v, s, t, o) => v.ToUniversalTime().ToString("o"));
            return this;
        }

        public SerializerOptionsDsl WriteMicrosoftJsonDateTime()
        {
            AddWriter<DateTime>((v, s, t, o) => v.ToMicrosoftJsonDateFormat());
            return this;
        }

        public SerializerOptionsDsl AsSimpleType<T>()
        {
            AddWriter<T>((v, s, t, o) => v.ToString());
            return this;
        }

        // Xml

        public SerializerOptionsDsl XmlValuesAsAttributes()
        {
            _options.Serialization.XmlValueNodeType = XmlValueNodeType.Attribute;
            return this;
        }

        public SerializerOptionsDsl WithDefaultXmlNamespace(string @namespace)
        {
            _options.Serialization.DefaultXmlNamespace = XNamespace.Get(@namespace);
            return this;
        }

        public SerializerOptionsDsl AddXmlNamespace(string prefix, string @namespace)
        {
            _options.Serialization.XmlNamespaces.Add(prefix, XNamespace.Get(@namespace));
            return this;
        }

        public SerializerOptionsDsl OmitXmlDeclaration()
        {
            _options.Serialization.OmitXmlDeclaration = true;
            return this;
        }

        // Enum naming conventions

        public SerializerOptionsDsl WithEnumNamingConvention(
            Func<string, string> convention)
        {
            _options.EnumNameConventions.Add(
                (v, c) => convention(v),
                (v, c) => c.Mode == Mode.Serialize);
            return this;
        }

        public SerializerOptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention)
        {
            _options.EnumNameConventions.Add(convention,
                (v, c) => c.Mode == Mode.Serialize);
            return this;
        }

        public SerializerOptionsDsl WithEnumNamingConvention(
            Func<string, EnumContext, string> convention,
            Func<string, EnumContext, bool> when)
        {
            _options.EnumNameConventions.Add(convention,
                (v, c) => c.Mode == Mode.Serialize && when(v, c));
            return this;
        }

        public SerializerOptionsDsl UseEnumCamelCaseNaming()
        {
            return WithEnumNamingConvention((v, c) => v.ToCamelCase());
        }

        public SerializerOptionsDsl UseEnumSnakeCaseNaming(bool lower = false)
        {
            return WithEnumNamingConvention((v, c) => v.ToSeparatedCase(lower, "_"));
        }

        // Visitors

        public SerializerOptionsDsl AddVisitor(Action<NodeBase, INode, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter(visitor);
            return this;
        }

        public SerializerOptionsDsl AddVisitor(Action<NodeBase, INode, Options> visitor, 
            Func<NodeBase, INode, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter(visitor, when);
            return this;
        }

        public SerializerOptionsDsl AddJsonVisitor(Action<NodeBase, JsonNode, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter((s, t, o) => visitor(s, (JsonNode)t, o), 
                (s, t, o) => t.IsJson());
            return this;
        }

        public SerializerOptionsDsl AddJsonVisitor(Action<NodeBase, JsonNode, Options> visitor,
            Func<NodeBase, JsonNode, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter((s, t, o) => visitor(s, (JsonNode)t, o), 
                (s, t, o) => t.IsJson() && when(s, (JsonNode)t, o));
            return this;
        }

        public SerializerOptionsDsl AddXmlVisitor(Action<NodeBase, XmlNodeBase, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter((s, t, o) => visitor(s, (XmlNodeBase)t, o), 
                (s, t, o) => t.IsXml());
            return this;
        }

        public SerializerOptionsDsl AddXmlVisitor(Action<NodeBase, XmlNodeBase, Options> visitor,
            Func<NodeBase, XmlNodeBase, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter((s, t, o) => visitor(s, (XmlNodeBase)t, o), 
                (s, t, o) => t.IsXml() && when(s, (XmlNodeBase)t, o));
            return this;
        }

        public SerializerOptionsDsl AddVisitor<T>(Action<NodeBase, INode, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>(visitor);
            return this;
        }

        public SerializerOptionsDsl AddVisitor<T>(Action<NodeBase, INode, Options> visitor,
            Func<NodeBase, INode, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>(visitor, when);
            return this;
        }

        public SerializerOptionsDsl AddJsonVisitor<T>(Action<NodeBase, JsonNode, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>((s, t, o) => visitor(s, (JsonNode)t, o), 
                (s, t, o) => t.IsJson());
            return this;
        }

        public SerializerOptionsDsl AddJsonVisitor<T>(Action<NodeBase, JsonNode, Options> visitor,
            Func<NodeBase, JsonNode, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>((s, t, o) => visitor(s, (JsonNode)t, o), 
                (s, t, o) => t.IsJson() && when(s, (JsonNode)t, o));
            return this;
        }

        public SerializerOptionsDsl AddXmlVisitor<T>(Action<NodeBase, XmlNodeBase, Options> visitor)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>((s, t, o) => visitor(s, (XmlNodeBase)t, o), 
                (s, t, o) => t.IsXml());
            return this;
        }

        public SerializerOptionsDsl AddXmlVisitor<T>(Action<NodeBase, XmlNodeBase, Options> visitor,
            Func<NodeBase, XmlNodeBase, Options, bool> when)
        {
            _options.Serialization.Writers.AddVisitingWriter<T>((s, t, o) => visitor(s, (XmlNodeBase)t, o),
                (s, t, o) => t.IsXml() && when(s, (XmlNodeBase)t, o));
            return this;
        }

        // Writers

        public SerializerOptionsDsl AddWriter(Action<object, NodeBase, INode, Options> writer,
            Func<object, NodeBase, INode, Options, bool> when)
        {
            _options.Serialization.Writers.AddWriter(writer, when);
            return this;
        }

        public SerializerOptionsDsl AddWriter<T>(Action<T, NodeBase, INode, Options> writer)
        {
            _options.Serialization.Writers.AddWriter(writer);
            return this;
        }

        public SerializerOptionsDsl AddWriter<T>(Action<T, NodeBase, INode, Options> mapper,
            Func<T, NodeBase, INode, Options, bool> where)
        {
            _options.Serialization.Writers.AddWriter(mapper, where);
            return this;
        }

        public SerializerOptionsDsl AddWriter<T>(Func<T, object> writer)
        {
            _options.Serialization.Writers.AddValueWriter<T>((v, s, t, o) => writer(v));
            return this;
        }

        public SerializerOptionsDsl AddWriter<T>(Func<T, NodeBase, INode, Options, object> writer)
        {
            _options.Serialization.Writers.AddValueWriter(writer);
            return this;
        }

        public SerializerOptionsDsl AddWriter<T>(Func<T, NodeBase, INode, Options, object> writer,
            Func<T, NodeBase, INode, Options, bool> where)
        {
            _options.Serialization.Writers.AddValueWriter(writer, where);
            return this;
        }
    }
}