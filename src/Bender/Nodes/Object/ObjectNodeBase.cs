using System.Linq;
using Bender.Collections;
using Bender.Extensions;
using Bender.Nodes.Object.Values;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public abstract class ObjectNodeBase : NodeBase
    {
        public static string NodeFormat = "object";

        private readonly string _name;

        protected ObjectNodeBase(
            string name, 
            IValue source,
            CachedMember member,
            INode parent,  
            Context context) : base(parent)
        {
            _name = name;
            Source = source;
            Context = context;
            Member = member;
            HasMember = member != null;
            if (HasMember) Metadata.Add(member.Attributes);
            else if (HasMember && Source != null) Metadata.Add(ActualType.Attributes);
        }

        public override string Format => NodeFormat;
        public override bool IsNamed => true;
        public override bool HasFixedNodeType => true;

        public IValue Source { get; }
        public CachedMember Member { get; }
        public bool HasMember { get; }
        public CachedType SpecifiedType => Source.SpecifiedType;
        public CachedType ActualType => Source.ActualType;
        public Mode Mode => Context.Mode;

        protected Context Context { get; }

        public override object Value
        {
            get { return Source.Instance; }
            set { Source.Instance = value; }
        }

        public override string Path
        {
            get
            {
                return this.Walk(x => x.Parent.As<ObjectNodeBase>()).Reverse().Select(x => 
                    x.HasParent && (x.Parent is EnumerableNode || x.Parent is DictionaryNode)
                    ? "[" + (x.Parent is EnumerableNode ? x.Parent.ToList().IndexOf(x).Map(y => y >= 0 ? y.ToString() : "") : "\"{0}\"".ToFormat(x.Name)) + "]"
                    : (x.HasParent ? "." + (x.HasMember ? x.Member.Name : x.Name) : 
                        (Context.Mode == Mode.Deserialize ? x.SpecifiedType : x.ActualType).FriendlyFullName)).Aggregate()
                    .Replace("+", ".");
            }
        }

        protected override string GetName()
        {
            return _name;
        }
    }
}