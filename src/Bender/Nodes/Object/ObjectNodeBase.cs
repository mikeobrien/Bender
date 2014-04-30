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
        }

        public override string Format { get { return NodeFormat; } }
        public override bool IsNamed { get { return true; } }
        public override bool HasFixedNodeType { get { return true; } }

        public IValue Source { get; private set; }
        public CachedMember Member { get; private set; }
        public bool HasMember { get; private set; }
        public CachedType SpecifiedType { get { return Source.SpecifiedType; } }
        public CachedType ActualType { get { return Source.ActualType; } }
        public Mode Mode { get { return Context.Mode; } }

        protected Context Context { get; private set; }

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