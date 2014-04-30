namespace Bender.Nodes
{
    public enum Mode { Serialize, Deserialize }

    public static class ModeExtensions
    {
        public static bool IsSerialize(this Mode mode) { return mode == Mode.Serialize; }
        public static bool IsDeserialize(this Mode mode) { return mode == Mode.Deserialize; }
    }
}
