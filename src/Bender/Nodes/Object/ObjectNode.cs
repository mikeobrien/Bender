using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes.Object.Values;
using Bender.Nodes.Xml;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class MissingNodeDeserializationException : FriendlyBenderException
    {
        public const string MessageFormat = "The following children were expected but not found: {0}.";

        public MissingNodeDeserializationException(IEnumerable<INode> missing) :
            base(MessageFormat, MessageFormat, missing.Aggregate(x => "'{0}'".ToFormat(x.Name), ", ")) { }
    }

    public class UnrecognizedNodeDeserializationException : FriendlyBenderException
    {
        public const string MessageFormat = "{0} '{1}' is not recognized.";

        public UnrecognizedNodeDeserializationException(INode node) : 
            base(MessageFormat, MessageFormat, node.Type.ToInitialCaps(), node.Name) { }
    }

    public class ObjectNode : ObjectNodeBase
    {
        private readonly Lazy<IEnumerable<MemberDefinition>> _members;
        private readonly Lazy<IEnumerable<INode>> _nodes;
        private readonly Lazy<IList<INode>> _addedNodes; 

        public ObjectNode(
            Context context,
            string name,
            IValue @object,
            CachedMember member,
            INode parent) :
                base(name, @object, member, parent, context)
        {
            _members = new Lazy<IEnumerable<MemberDefinition>>(EnumerateMembers);
            _nodes = new Lazy<IEnumerable<INode>>(EnumerateNodes);
            _addedNodes = new Lazy<IList<INode>>(() => new List<INode>());
        }

        public override string Type => "object";

        protected override NodeType GetNodeType()
        {
            return NodeType.Object;
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            var match = _members.Value.GetMember(node.Name, 
                Context.Options.Deserialization.NameComparison);
            if (match != null && match.IsNodeType)
            {
                if (match.Value.SpecifiedType.Is<INode>() || match.Value.SpecifiedType.IsTypeOf(node))
                    match.Value.Instance = node;
            }
            else
            {
                var matchingNode = _nodes.Value.GetNode(node.Name, 
                    Context.Options.Deserialization.NameComparison);
                if (matchingNode == null)
                {
                    if (Context.Options.Deserialization.IgnoreUnmatchedElements) return;
                    throw new UnrecognizedNodeDeserializationException(node);
                }
                if (matchingNode.Metadata.Contains<XmlSiblingsAttribute>())
                    matchingNode.Add(node, modify);
                else matchingNode.Configure(modify);
                if (!_addedNodes.Value.Contains(matchingNode))
                    _addedNodes.Value.Add(matchingNode);
            }
        }

        public override void Initialize()
        {
            Source.EnsureValue();
        }

        public override void Validate()
        {
            if (Mode.IsSerialize() || Context.Options.Deserialization.IgnoreUnmatchedMembers) return;
            var unmappedNodes = _nodes.Value.GetUnmatchedNodes(
                _addedNodes.Value, Context.Options.Deserialization.NameComparison).ToList();
            if (unmappedNodes.Any()) throw new MissingNodeDeserializationException(unmappedNodes);
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return _nodes.Value;
        }

        private IEnumerable<INode> EnumerateNodes() 
        {
            return _members.Value.Where(x => !x.IsNodeType).Select(x => Context.Mode == Mode.Deserialize ? 
                NodeFactory.CreateDeserializable(x.Name, x.Value, this, Context, x.Member) :
                NodeFactory.CreateSerializable(x.Name, x.Value, this, Context, x.Member))
                .Concat(EnumerateNodeInstances()).ToList();
        }

        private IEnumerable<INode> EnumerateNodeInstances()
        {
            return _members.Value.Where(x => x.IsNodeType)
                .Select(x => x.Value.Instance != null ? x.Value.Instance.As<INode>() : 
                    new ValueNode(Context, x.Name, x.Value, x.Member, this));
        }

        private IEnumerable<MemberDefinition> EnumerateMembers()
        {
            var members = SpecifiedType.Members
                .Select(x => new MemberDefinition(x, GetMemberName(x),
                    ValueFactory.Create(Context.Mode, Source, x, Context.Options)
                ));

            if (Context.Mode.IsSerialize()) members = members
                .Where(x => !x.Member.IsOptional || x.Value.HasValue)
                .Where(x => !x.Member.IsProperty || x.Member.HasGetter)
                .Where(x => Context.Options.Serialization.IncludeNullMembers || x.Value.Instance != null)
                .Where(x => this.Walk<INode>(y => y.Parent).All(y => y.Value != x.Value.Instance));

            if (Context.Mode.IsDeserialize()) members = members.Where(x => !x.Member.IsReadonly);

            return members
                .Where(x => DefaultMemberFilter.And(Context.Options.MemberFilter)(x.Member, Context.Options))
                .Where(x => x.Value.SpecifiedType, x => Context.Options, Context.Options.TypeFilter).ToList();
        } 

        private string GetMemberName(CachedMember member)
        {
            return member.IsField
                ? Context.Options.FieldNameConventions.GetName(member, Context)
                : Context.Options.PropertyNameConventions.GetName(member, Context);
        }

        private static readonly Func<CachedMember, Options, bool> DefaultMemberFilter = (m, o) =>
        {
            var isPublic = m.IsPublicPropertyOrField;
            return ((m.IsProperty && (isPublic || o.IncludeNonPublicProperties)) ||
                    (m.IsField && ((isPublic && o.IncludePublicFields) ||
                                   (!isPublic && o.IncludeNonPublicFields)))) &&
                   !m.HasAttribute<XmlIgnoreAttribute>();
        };
    }
}