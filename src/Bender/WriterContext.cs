using System.Reflection;

namespace Bender
{
    public class WriterContext
    {
        public WriterContext(Options options, PropertyInfo property, Format format, object value, ValueNode valueNode)
        {
            Options = options;
            Property = property;
            Format = format;
            Value = value;
            Node = valueNode;
        }

        public Options Options { get; private set; }
        public PropertyInfo Property { get; private set; }
        public object Value { get; private set; }
        public Format Format { get; private set; }
        public ValueNode Node { get; private set; }
    }

    public class WriterContext<T>
    {
        private readonly WriterContext _writerContext;

        public WriterContext(WriterContext writerContext)
        {
            _writerContext = writerContext;
        }

        public Options Options { get { return _writerContext.Options; } }
        public PropertyInfo Property { get { return _writerContext.Property; } }
        public T Value { get { return (T)_writerContext.Value; } }
        public Format Format { get { return _writerContext.Format; } }
        public ValueNode Node { get { return _writerContext.Node; } }
    }
}
