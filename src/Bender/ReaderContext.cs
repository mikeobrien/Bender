using System.Reflection;

namespace Bender
{
    public class ReaderContext
    {
        public ReaderContext(Options options, PropertyInfo property, Format format, ValueNode valueNode)
        {
            Options = options;
            Property = property;
            Format = format;
            Node = valueNode;
        }

        public Options Options { get; private set; }
        public PropertyInfo Property { get; private set; }
        public Format Format { get; private set; }
        public ValueNode Node { get; private set; }
    }
}
