using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Script.Serialization;
using NUnit.Framework;
using Should;

namespace Tests.Deserializer.Json
{
    [TestFixture]
    public class PerformanceTests
    {
        public class SpeedTestCollection
        {
            public List<SpeedTestItem> Value0 { get; set; } 
            public List<SpeedTestItem> Value1 { get; set; }
            public List<SpeedTestItem> Value2 { get; set; } 
            public List<SpeedTestItem> Value3 { get; set; }
            public List<SpeedTestItem> Value4 { get; set; }
        }

        public class SpeedTestItem
        {
            public string Value0 { get; set; } 
            public string Value1 { get; set; }
            public string Value2 { get; set; } 
            public string Value3 { get; set; }
            public string Value4 { get; set; }
        }

        [Test]
        public void should_be_faster_than_the_fcl_json_deserializer()
        {
            var json = "[" + Enumerable.Range(0, 5).Select(x => "{" + 
                    "\"Value0\": [ " + Enumerable.Range(0, 5).Select(y => "{ \"Value0\": \"ssdfsfsfd\", \"Value1\": \"sfdsfsdf\", \"Value2\": \"adasd\", \"Value3\": \"wqerqwe\", \"Value4\": \"qwerqwer\" }").Aggregate((a, i) => a + "," + i) + " ], " +
                    "\"Value1\": [ " + Enumerable.Range(0, 5).Select(y => "{ \"Value0\": \"ssdfsfsfd\", \"Value1\": \"sfdsfsdf\", \"Value2\": \"adasd\", \"Value3\": \"wqerqwe\", \"Value4\": \"qwerqwer\" }").Aggregate((a, i) => a + "," + i) + " ], " +
                    "\"Value2\": [ " + Enumerable.Range(0, 5).Select(y => "{ \"Value0\": \"ssdfsfsfd\", \"Value1\": \"sfdsfsdf\", \"Value2\": \"adasd\", \"Value3\": \"wqerqwe\", \"Value4\": \"qwerqwer\" }").Aggregate((a, i) => a + "," + i) + " ], " +
                    "\"Value3\": [ " + Enumerable.Range(0, 5).Select(y => "{ \"Value0\": \"ssdfsfsfd\", \"Value1\": \"sfdsfsdf\", \"Value2\": \"adasd\", \"Value3\": \"wqerqwe\", \"Value4\": \"qwerqwer\" }").Aggregate((a, i) => a + "," + i) + " ], " +
                    "\"Value4\": [ " + Enumerable.Range(0, 5).Select(y => "{ \"Value0\": \"ssdfsfsfd\", \"Value1\": \"sfdsfsdf\", \"Value2\": \"adasd\", \"Value3\": \"wqerqwe\", \"Value4\": \"qwerqwer\" }").Aggregate((a, i) => a + "," + i) + " ] " +
                "}").Aggregate((a, i) => a + ", " + i) + "]";
            
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var i = 0; i < 100; i++) Bender.Deserializer.Create().DeserializeJson<List<SpeedTestCollection>>(json);
            stopwatch.Stop();
            var benderSpeed = stopwatch.ElapsedTicks;

            var jsonSerializer = new JavaScriptSerializer();
            stopwatch.Start();
            for (var i = 0; i < 100; i++) jsonSerializer.Deserialize<List<SpeedTestCollection>>(json);
            stopwatch.Stop();
            var serializerSpeed = stopwatch.ElapsedTicks;

            Debug.WriteLine("Bender speed (ticks): {0:#,##0}", benderSpeed);
            Debug.WriteLine("JavaScriptSerializer speed (ticks): {0:#,##0}", serializerSpeed);
            (benderSpeed < serializerSpeed).ShouldBeTrue();
        }
    }
}
