using System;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class ReaderException : BenderException
    {
        public ReaderException(Exception exception) :
            base(exception, "Reader failed: {0}", exception.Message) { }
    }

    public class ReaderConventions
    {
        private readonly Options _options;

        public ReaderConventions(Options options)
        {
            _options = options;
            Func<Exception, INode, INode, Exception> handler = (e, s, t) => 
                e is FriendlyBenderException ? e : new ReaderException(e);
            Mapping = new MapConventions<INode, ObjectNodeBase>(options, handler);
            Visitors = new VisitConventions<INode, ObjectNodeBase>(options, handler);
        }

        public MapConventions<INode, ObjectNodeBase> Mapping { get; }
        public VisitConventions<INode, ObjectNodeBase> Visitors { get; }

        // Visitors

        public ReaderConventions AddVisitingReader(Action<INode, ObjectNodeBase, Options> visitor)
        {
            Visitors.Add(visitor);
            return this;
        }

        public ReaderConventions AddVisitingReader(Action<INode, ObjectNodeBase, Options> visitor,
            Func<INode, ObjectNodeBase, Options, bool> when)
        {
            Visitors.Add(visitor, when);
            return this;
        }

        public ReaderConventions AddVisitingReader<T>(Action<INode, ObjectNodeBase, Options> visitor)
        {
            Visitors.Add(visitor, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>());
            return this;
        }

        public ReaderConventions AddVisitingReader<T>(Action<INode, ObjectNodeBase, Options> visitor, bool includeNullable) where T : struct
        {
            Visitors.Add(visitor, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>(includeNullable));
            return this;
        }

        public ReaderConventions AddVisitingReader<T>(Action<INode, ObjectNodeBase, Options> visitor,
            Func<INode, ObjectNodeBase, Options, bool> where)
        {
            Visitors.Add(visitor, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>() && where(s, t, o));
            return this;
        }

        public ReaderConventions AddVisitingReader<T>(Action<INode, ObjectNodeBase, Options> visitor,
            Func<INode, ObjectNodeBase, Options, bool> where, bool includeNullable) where T : struct
        {
            Visitors.Add(visitor, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>(includeNullable) && where(s, t, o));
            return this;
        }

        // Readers

        public ReaderConventions AddReader(Action<INode, ObjectNodeBase, Options> reader,
            Func<INode, ObjectNodeBase, Options, bool> when)
        {
            Mapping.Add(reader, when);
            return this;
        }

        public ReaderConventions AddReader<T>(Action<INode, ObjectNodeBase, Options> reader)
        {
            Mapping.Add(reader, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>());
            return this;
        }

        public ReaderConventions AddReader<T>(Action<INode, ObjectNodeBase, Options> reader, bool includeNullable) where T : struct
        {
            Mapping.Add(reader, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>(includeNullable));
            return this;
        }

        public ReaderConventions AddReader<T>(Action<INode, ObjectNodeBase, Options> reader,
            Func<INode, ObjectNodeBase, Options, bool> where)
        {
            Mapping.Add(reader, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>() && where(s, t, o));
            return this;
        }

        public ReaderConventions AddReader<T>(Action<INode, ObjectNodeBase, Options> reader,
            Func<INode, ObjectNodeBase, Options, bool> where, bool includeNullable) where T : struct
        {
            Mapping.Add(reader, (s, t, o) => t.SpecifiedType.Type.CanBeCastTo<T>(includeNullable) && where(s, t, o));
            return this;
        }

        public ReaderConventions AddValueReader<T>(Func<object, INode, ObjectNodeBase, Options, T> reader)
        {
            AddReader<T>(SetValue(reader));
            return this;
        }

        public ReaderConventions AddValueReader<T>(Func<object, INode, ObjectNodeBase, Options, T> reader, bool includeNullable) where T : struct
        {
            AddReader<T>(SetValue(reader), includeNullable);
            return this;
        }

        public ReaderConventions AddValueReader<T>(Func<object, INode, ObjectNodeBase, Options, T> reader,
            Func<object, INode, ObjectNodeBase, Options, bool> where)
        {
            AddReader<T>(SetValue(reader), (s, t, o) => where(s.Value, s, t, o));
            return this;
        }

        public ReaderConventions AddValueReader<T>(Func<object, INode, ObjectNodeBase, Options, T> reader,
            Func<object, INode, ObjectNodeBase, Options, bool> where, bool includeNullable) where T : struct
        {
            AddReader<T>(SetValue(reader), (s, t, o) => where(s.Value, s, t, o), includeNullable);
            return this;
        }

        private Action<INode, ObjectNodeBase, Options> SetValue<T>(Func<object, INode, ObjectNodeBase, Options, T> reader)
        {
            return (s, t, o) =>
            {
                var errorMessages = _options.Deserialization.FriendlyParseErrorMessages;
                object value = null;
                try
                {
                    if (s.Value != null) value = reader(s.Value, s, t, o);
                    t.Value = value;
                }
                catch (Exception exception)
                {
                    var type = typeof(T).ToCachedType().UnderlyingType.Type;
                    if (!errorMessages.ContainsKey(type)) throw;
                    throw new ValueParseException(exception, value, errorMessages[type].ToFormat(s.Value));
                }
            };
        }
    }
}