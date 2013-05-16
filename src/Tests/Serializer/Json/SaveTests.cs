using System;
using System.IO;
using NUnit.Framework;
using Should;

namespace Tests.Serializer.Json
{
    [TestFixture]
    public class SaveTests
    {
        public class Model { public int Value { get; set; } }

        public Model Instance = new Model {Value = 3};
        const string Json = "{\"Value\":3}";

        [Test]
        public void should_save_string()
        {
            Bender.Serializer.Create().SerializeJson(Instance).ShouldEqual(Json);
        }

        [Test]
        public void should_save_to_stream()
        {
            using (var stream = new MemoryStream())
            {
                Bender.Serializer.Create().SerializeJson(Instance, stream).Position = 0;
                new StreamReader(stream).ReadToEnd().ShouldEqual(Json);
            }
        }

        [Test]
        public void should_save_to_file()
        {
            var path = Guid.NewGuid().ToString("N");
            try
            {
                Bender.Serializer.Create().SerializeJson(Instance, path);
                File.ReadAllText(path).ShouldEqual(Json);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
