using System.IO;

namespace Bender.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadToEnd(this Stream stream)
        {
            using (stream)
            using (var reader = new StreamReader(stream))
            {
                stream.Reset();
                return reader.ReadToEnd();
            }
        }

        public static byte[] ReadAllBytes(this Stream source)
        {
            if (source is MemoryStream) return ((MemoryStream)source).ToArray();
            using (var stream = new MemoryStream())
            {
                source.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static void SaveToFile(this Stream source, string path)
        {
            using (source)
            using (var target = File.Create(path))
            {
                source.Reset();
                source.CopyTo(target);
            }
        }

        public static void Reset(this Stream stream)
        {
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
