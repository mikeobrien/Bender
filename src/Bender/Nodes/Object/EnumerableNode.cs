using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;
using Bender.Extensions;
using Bender.NamingConventions;
using Bender.Nodes.Object.Values;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class InvalidItemNameDeserializationException : FriendlyBenderException
    {
        public const string MessageFormat = "Name '{0}' does not match expected name of '{1}'.";

        public InvalidItemNameDeserializationException(string actual, string expected) :
            base(MessageFormat, MessageFormat, actual, expected) { }
    }

    public class EnumerableNode : ObjectNodeBase
    {
        private readonly Lazy<IList> _list;
        private readonly CachedType _itemType;
        private readonly Lazy<IEnumerable<INode>> _nodes; 

        public EnumerableNode(
            Context context, 
            string name, 
            IValue enumerable, 
            CachedMember member,
            INode parent)
            : base(name, enumerable, member, parent, context)
        {
            _list = new Lazy<IList>(() => 
                enumerable.Instance.MapOrDefault(x => enumerable.ActualType.IsArray ? 
                    ArrayAdapter.Create(enumerable) : GenericListAdapter.Create(x)));
            if (SpecifiedType.IsGenericEnumerable) _itemType = enumerable.SpecifiedType.GenericEnumerableType;
            _nodes = new Lazy<IEnumerable<INode>>(EnumerateNodes);
        }

        public override string Type { get { return "list"; } }

        protected override NodeType GetNodeType()
        {
            return NodeType.Array;
        }

        public override void Initialize()
        {
            Source.EnsureValue();
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            const string supported = "generic lists and generic enumerable interfaces";
            if (!SpecifiedType.IsGenericEnumerable) throw new TypeNotSupportedException(
                "non generic {0}".ToFormat(SpecifiedType.IsList ? "list" : "enumerable"), 
                SpecifiedType, Mode.Deserialize, supported);

            if (_itemType.CanBeCastTo<INode>())
            {
                if (_itemType.Is<INode>() || _itemType.IsTypeOf(node)) _list.Value.Add(node);
            }
            else
            {
                if (Context.Options.TypeFilter.WhenNot(_itemType, Context.Options)) return;
                if (!ActualType.IsList) throw new TypeNotSupportedException(
                    "enumerable", ActualType, Mode.Deserialize, supported);
                if (named)
                {
                    var itemName = GetItemName(_itemType);
                    if (!Context.Options.Deserialization.IgnoreArrayItemNames &&
                        !node.Name.Equals(itemName, Context.Options.Deserialization.NameComparison))
                        throw new InvalidItemNameDeserializationException(node.Name, itemName);
                }
                var value = ValueFactory.Create(_itemType);
                NodeFactory.CreateDeserializable(named ? Name : null, value, this, Context).Configure(modify);
                _list.Value.Add(value.Instance);
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return Context.Mode.IsDeserialize() ? Enumerable.Empty<INode>() : _nodes.Value;
        }

        private IEnumerable<INode> EnumerateNodes()
        {
            return Value.As<IEnumerable>().Cast<object>()
                .Where(x => x != null && this.Walk<INode>(y => y.Parent).All(y => y.Value != x))
                .Select(x => ValueFactory.Create(x, _itemType, Context.Options))
                .Where(x => x.SpecifiedType, x => Context.Options, Context.Options.TypeFilter)
                .Select(x => x.ActualType.CanBeCastTo<INode>() ? x.Instance.As<INode>() :
                    NodeFactory.CreateSerializable(GetItemName(x.SpecifiedType), x, this, Context)).ToList();
        } 

        private string GetItemName(CachedType type)
        {
            return Context.Options.ArrayItemNameConventions.GetName(type, Member, Context) ??
                   Context.Options.TypeNameConventions.GetName(type, Context);
        }
    }
}