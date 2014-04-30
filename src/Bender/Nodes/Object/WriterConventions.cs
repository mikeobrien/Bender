using System;
using Bender.Collections;
using Bender.Configuration;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class WriterException : BenderException
    {
        public WriterException(Exception exception) :
            base(exception, "Writer failed: {0}", exception.Message) { }
    }

    public class WriterConventions
    {
        public WriterConventions(Options options)
        {
            Func<Exception, INode, INode, Exception> handler = (e, s, t) =>
                e is FriendlyBenderException ? e : new WriterException(e);
            Mapping = new MapConventions<ObjectNodeBase, INode>(options, handler);
            Visitors = new VisitConventions<ObjectNodeBase, INode>(options, handler);
        }

        public MapConventions<ObjectNodeBase, INode> Mapping { get; private set; }
        public VisitConventions<ObjectNodeBase, INode> Visitors { get; private set; }

        // Visitors

        public WriterConventions AddVisitingWriter(Action<ObjectNodeBase, INode, Options> visitor)
        {
            Visitors.Add(visitor);
            return this;
        }

        public WriterConventions AddVisitingWriter(Action<ObjectNodeBase, INode, Options> visitor,
            Func<ObjectNodeBase, INode, Options, bool> when)
        {
            Visitors.Add(visitor, when);
            return this;
        }

        public WriterConventions AddVisitingWriter<T>(Action<ObjectNodeBase, INode, Options> visitor)
        {
            Visitors.Add(visitor, (s, t, o) => s.ActualType.Type.CanBeCastTo<T>());
            return this;
        }

        public WriterConventions AddVisitingWriter<T>(Action<ObjectNodeBase, INode, Options> visitor,
            Func<ObjectNodeBase, INode, Options, bool> where)
        {
            Visitors.Add(visitor, (s, t, o) => s.ActualType.Type.CanBeCastTo<T>() && where(s, t, o));
            return this;
        }

        // Mapping

        public WriterConventions AddWriter(Action<object, ObjectNodeBase, INode, Options> writer,
            Func<object, ObjectNodeBase, INode, Options, bool> when)
        {
            Mapping.Add((s, t, o) => writer(s.Value, s, t, o),
                (s, t, o) => when(s.Value, s, t, o));
            return this;
        }

        public WriterConventions AddWriter<T>(Action<T, ObjectNodeBase, INode, Options> writer)
        {
            Mapping.Add((s, t, o) => writer(s.Value.As<T>(), s, t, o),
                (s, t, o) => s.ActualType.Type.CanBeCastTo<T>());
            return this;
        }

        public WriterConventions AddWriter<T>(Action<T, ObjectNodeBase, INode, Options> writer,
            Func<T, ObjectNodeBase, INode, Options, bool> where)
        {
            Mapping.Add((s, t, o) => writer(s.Value.As<T>(), s, t, o),
                (s, t, o) => s.ActualType.Type.CanBeCastTo<T>() && where(s.Value.As<T>(), s, t, o));
            return this;
        }

        public WriterConventions AddValueWriter<T>(Func<T, ObjectNodeBase, INode, Options, object> writer)
        {
            AddWriter(SetValue(writer));
            return this;
        }

        public WriterConventions AddValueWriter<T>(Func<T, ObjectNodeBase, INode, Options, object> writer,
            Func<T, ObjectNodeBase, INode, Options, bool> where)
        {
            AddWriter(SetValue(writer), where);
            return this;
        }

        private Action<T, ObjectNodeBase, INode, Options> SetValue<T>(Func<T, ObjectNodeBase, INode, Options, object> writer)
        {
            return (v, s, t, o) =>
            {
                if (!t.HasFixedNodeType) t.NodeType = NodeType.Value;
                t.Value = writer(v, s, t, o);
            };
        }
    }
}
