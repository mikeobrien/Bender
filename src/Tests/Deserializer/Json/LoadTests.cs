using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class LoadTests
    {
        public class Model { public int Value { get; set; } }
        const string Json = "{ \"Value\": 3 }";

        [Test]
        public void should_load_string()
        {
            Bender.Deserializer.Create().DeserializeJson<Model>(Json).Value.ShouldEqual(3);
        }

        [Test]
        public void should_load_from_stream()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Json)))
            {
                Bender.Deserializer.Create().DeserializeJson<Model>(stream).Value.ShouldEqual(3);
            }
        }

        [Test]
        public void should_load_from_file()
        {
            var path = Guid.NewGuid().ToString("N");
            File.WriteAllText(path, Json);
            try
            {
                Bender.Deserializer.Create().DeserializeJsonFile<Model>(path).Value.ShouldEqual(3);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
