using System;
using System.Collections.Generic;
using System.Linq;
using Bender.Nodes.Object.Values;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class MemberDefinition
    {
        public MemberDefinition(CachedMember member, string name, IValue value)
        {
            Name = name;
            Value = value;
            Member = member;
            IsNodeType = value.SpecifiedType.Type.CanBeCastTo<INode>();
        }

        public string Name { get; private set; }
        public IValue Value { get; private set; }
        public CachedMember Member { get; private set; }
        public bool IsNodeType { get; private set; }
    }

    public static class MemberDefinitionExtensions
    {
        public static MemberDefinition GetMember(this IEnumerable<MemberDefinition> members,
            string name, StringComparison comparison)
        {
            return members.FirstOrDefault(x => x.Name.Equals(name, comparison));
        }
    }
}