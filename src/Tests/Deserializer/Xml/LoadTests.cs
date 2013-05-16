using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Xml
{
    [TestFixture]
    public class LoadTests
    {
        public class Model { public int Value { get; set; } }
        const string Xml = "<Model><Value>3</Value></Model>";

        [Test]
        public void should_load_string()
        {
            Bender.Deserializer.Create().DeserializeXml<Model>(Xml).Value.ShouldEqual(3);
        }

        [Test]
        public void should_load_from_stream()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Xml)))
            {
                Bender.Deserializer.Create().DeserializeXml<Model>(stream).Value.ShouldEqual(3);
            }
        }

        [Test]
        public void should_load_from_file()
        {
            var path = Guid.NewGuid().ToString("N");
            File.WriteAllText(path, Xml);
            try
            {
                Bender.Deserializer.Create().DeserializeXmlFile<Model>(path).Value.ShouldEqual(3);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
