using System.Text;

namespace Bender.Extensions
{
    public static class UTF8Encoding
    {
        static UTF8Encoding()
        {
            NoBOM = new System.Text.UTF8Encoding(false);
        }

        public static readonly System.Text.UTF8Encoding NoBOM;
    }
}
