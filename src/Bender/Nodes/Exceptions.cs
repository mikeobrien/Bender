namespace Bender.Nodes
{
    public class UnnamedChildrenNotSupportedException : BenderException
    {
        public UnnamedChildrenNotSupportedException() : 
            base("Unamed children are not supported.") { }
    }
}
