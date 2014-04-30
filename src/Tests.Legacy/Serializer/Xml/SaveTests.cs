using System;
using System.IO;
using NUnit.Framework;
using Should;

namespace Tests.Legacy.Serializer.Xml
{
    [TestFixture]
    public class SaveTests
    {
        public class Model { public int Value { get; set; } }
        public Model Instance = new Model { Value = 3 };

        [Test]
        public void should_save_string()
        {
            Bender.Serializer.Create().SerializeXml(Instance).ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Model><Value>3</Value></Model>");
        }

        [Test]
        public void should_save_to_stream()
        {
            var stream = Bender.Serializer.Create().SerializeXmlStream(Instance);
            stream.Position = 0;
            new StreamReader(stream).ReadToEnd().ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Model><Value>3</Value></Model>");
        }

        [Test]
        public void should_save_to_file()
        {
            var path = Guid.NewGuid().ToString("N");
            try
            {
                Bender.Serializer.Create().SerializeXmlFile(Instance, path);
                File.ReadAllText(path).ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Model><Value>3</Value></Model>");
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
