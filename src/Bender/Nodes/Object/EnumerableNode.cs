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
        private bool _enumerated;

        public EnumerableNode(
            Context context, 
            string name, 
            IValue enumerable, 
            CachedMember member,
            INode parent,
            int? index = null)
            : base(name, enumerable, member, parent, context, index)
        {
            _list = new Lazy<IList>(() => 
                enumerable.Instance.MapOrDefault(x => enumerable.ActualType.IsArray ? 
                    ArrayAdapter.Create(enumerable) : GenericListAdapter.Create(x)));
            if (SpecifiedType.IsGenericEnumerable) _itemType = enumerable.SpecifiedType.GenericEnumerableType;
        }

        public override string Type => "list";

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
                    if (!node.Name.Equals(itemName, Context.Options.Deserialization.NameComparison))
                    {
                        if (Context.Options.Deserialization.IgnoreUnmatchedArrayItems) return;
                        if (!Context.Options.Deserialization.IgnoreArrayItemNames)
                            throw new InvalidItemNameDeserializationException(node.Name, itemName);
                    }
                    
                }
                var value = ValueFactory.Create(_itemType);
                NodeFactory.CreateDeserializable(named ? Name : null, value, this, Context).Configure(modify);
                _list.Value.Add(value.Instance);
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return Context.Mode.IsDeserialize() ? Enumerable.Empty<INode>() : EnumerateNodes();
        }

        private IEnumerable<INode> EnumerateNodes()
        {
            // This is more of an internal check to make sure nothing 
            // in the library enumerates multiple times.
            if (_enumerated) throw new Exception("Should not enumerate more than once.");
            _enumerated = true;
            var index = 0;
            return Value.As<IEnumerable>().Cast<object>()
                .Where(x => x != null && this.Walk<INode>(y => y.Parent).All(y => y.Value != x))
                .Select(x => ValueFactory.Create(x, _itemType, Context.Options))
                .Where(x => x.SpecifiedType, x => Context.Options, Context.Options.TypeFilter)
                .Select(x => x.ActualType.CanBeCastTo<INode>() ? x.Instance.As<INode>() :
                    NodeFactory.CreateSerializable(GetItemName(x.SpecifiedType), 
                        x, this, Context, index: index++));
        } 

        private string GetItemName(CachedType type)
        {
            return Context.Options.ArrayItemNameConventions.GetName(type, Member, Context) ??
                   Context.Options.TypeNameConventions.GetName(type, Context);
        }
    }
}