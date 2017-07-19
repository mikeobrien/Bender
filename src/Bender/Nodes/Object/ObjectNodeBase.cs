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
            Context context,
            int? index = null) : base(parent)
        {
            _name = name;
            Source = source;
            Context = context;
            Index = index;
            Member = member;
            HasMember = member != null;
            if (HasMember) Metadata.Add(member.Attributes);
            else if (Source?.ActualType != null) Metadata.Add(ActualType.Attributes);
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
        public int? Index { get; }

        protected Context Context { get; }

        public override object Value
        {
            get => Source.Instance;
            set => Source.Instance = value;
        }

        public override string Path
        {
            get
            {
                return this.Walk(x => x.Parent.As<ObjectNodeBase>())
                    .Reverse().Select(BuildPath).Aggregate();
            }
        }

        private string BuildPath(ObjectNodeBase node)
        {
            string path;
            if (node.HasParent && (node.Parent is EnumerableNode || node.Parent is DictionaryNode))
                path = "[" + (node.Parent is EnumerableNode ? node.Index?.ToString() : 
                    "\"{0}\"".ToFormat(node.Name)) + "]";
            else
                path = node.HasParent ? "." + (node.HasMember ? node.Member.Name : node.Name) : 
                    (Context.Mode == Mode.Deserialize ? node.SpecifiedType : 
                        node.ActualType).FriendlyFullName;

            return path.Replace("+", ".");
        }

        protected override string GetName()
        {
            return _name;
        }
    }
}