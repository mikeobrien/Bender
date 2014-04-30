using Bender.Configuration;

namespace Bender.Nodes.Object
{
    public class Context
    {
        public Context(Options options, Mode mode, string format)
        {
            Options = options;
            Mode = mode;
            Format = format;
        }

        public Options Options { get; private set; }
        public Mode Mode { get; set; }
        public string Format { get; private set; }
    }
}
