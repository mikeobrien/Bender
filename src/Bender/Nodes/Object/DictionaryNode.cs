using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;
using Bender.Nodes.Object.Values;
using Bender.Extensions;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class DictionaryNode : ObjectNodeBase
    {
        private readonly Lazy<IDictionary> _dictionary;
        private readonly CachedType _itemType;
        private readonly Lazy<IEnumerable<INode>> _nodes; 

        public DictionaryNode(
            Context context, 
            string name, 
            IValue dictionary, 
            CachedMember member,
            INode parent,
            int? index = null)
            : base(name, dictionary, member, parent, context, index)
        {
            _dictionary = new Lazy<IDictionary>(() =>
                dictionary.Instance.MapOrDefault(GenericDictionaryAdapter.Create));
            if (SpecifiedType.IsGenericDictionary) _itemType = SpecifiedType.GenericDictionaryTypes.Value;
            _nodes = new Lazy<IEnumerable<INode>>(EnumerateNodes);
        }

        public override string Type => "dictionary";

        protected override NodeType GetNodeType()
        {
            return NodeType.Object;
        }

        public override void Initialize()
        {
            Source.EnsureValue();
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (!SpecifiedType.IsGenericDictionary) throw new TypeNotSupportedException(
                "non generic dictionary", SpecifiedType, Mode.Deserialize, "generic dictionaries");

            if (node.IsNamed && _itemType.CanBeCastTo<INode>())
            {
                if (_itemType.Is<INode>() || _itemType.IsTypeOf(node))
                    _dictionary.Value.Add(node.Name, node);
            }
            else
            {
                if (Context.Options.TypeFilter.WhenNot(_itemType, Context.Options)) return;
                var value = ValueFactory.Create(_itemType);
                NodeFactory.CreateDeserializable(node.Name, value, this, Context).Configure(modify);
                _dictionary.Value.Add(node.Name, value.Instance);
            }
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return Context.Mode.IsDeserialize() ? Enumerable.Empty<INode>() : _nodes.Value;
        }

        private IEnumerable<INode> EnumerateNodes()
        {
            return _dictionary.Value.AsEnumerable()
                .Where(x => x.Value != null && this.Walk<INode>(y => y.Parent).All(y => y.Value != x.Value))
                .Select(x => new
                {
                    Name = x.Key.ToString(),
                    Value = ValueFactory.Create(x.Value, _itemType, Context.Options)
                })
                .Where(x => x.Value.SpecifiedType, x => Context.Options, Context.Options.TypeFilter)
                .Select(x => x.Value.ActualType.CanBeCastTo<INode>() ? x.Value.Instance.As<INode>() :
                    NodeFactory.CreateSerializable(x.Name, x.Value, this, Context)).ToList();
        } 
    }
}