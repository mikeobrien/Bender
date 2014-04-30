using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using Bender.Collections;
using NUnit.Framework;
using Bender.Extensions;
using Should;

namespace Tests.Extensions
{
    [TestFixture]
    public class LinqExtensionTests
    {
        [Test]
        public void should_convert_name_value_collection_to_dictionary()
        {
            var results = new NameValueCollection { { "your", "mom" }, { "goes to", "college" } }.ToDictionary();
            results.ShouldTotal(2);
            results["your"].ShouldEqual("mom");
            results["goes to"].ShouldEqual("college");
        }

        [Test]
        public void should_conditionally_pipe_values()
        {
            "yada".PipeWhen(true).ShouldEqual("yada");
            "yada".PipeWhen(false).ShouldBeNull();

            "yada".PipeWhen(false, "").ShouldEqual("");

            "yada".PipeWhen(x => x.Length == 4).ShouldEqual("yada");
            "yada".PipeWhen(x => x.Length == 5).ShouldBeNull();

            "yada".PipeWhen(x => x.Length == 5, "").ShouldEqual("");

            "yada".PipeWhen(x => x + "yada", false).ShouldEqual("yada");
            "yada".PipeWhen(x => x + "yada", true).ShouldEqual("yadayada");

            string result1 = null;
            "yada".PipeWhen(x => { result1 = x; }, false);
            result1.ShouldBeNull();

            string result2 = null;
            "yada".PipeWhen(x => { result2 = x; }, true);
            result2.ShouldEqual("yada");

            "yada".PipeWhen(x => x + "yada", x => x.Length == 5).ShouldEqual("yada");
            "yada".PipeWhen(x => x + "yada", x => x.Length == 4).ShouldEqual("yadayada");

            string result3 = null;
            "yada".PipeWhen(x => { result3 = x; }, x => x.Length == 5);
            result3.ShouldBeNull();

            string result4 = null;
            "yada".PipeWhen(x => { result4 = x; }, x => x.Length == 4);
            result4.ShouldEqual("yada");
        }

        [Test]
        public void should_map_values()
        {
            "yada".Map(x => x.Length).ShouldEqual(4);

            ((string)null).MapOrDefault(x => x.Length).ShouldEqual(0);
            "yada".MapOrDefault(x => x.Length).ShouldEqual(4);

            ((string)null).MapOrDefault(x => x.Substring(2)).ShouldBeNull();
            "yada".MapOrDefault(x => x.Substring(2)).ShouldEqual("da");

            ((string)null).MapOrDefault(x => x.Substring(2), "").ShouldEqual("");
            "yada".MapOrDefault(x => x.Substring(2), "").ShouldEqual("da");
        }

        readonly Url[] _urls = { new Url("http://oh"), new Url("http://hai") };

        [Test]
        public void should_aggregate_with_seperator()
        {
            _urls.Aggregate(x => x.Value)
                .ShouldEqual("http://ohhttp://hai");
        }

        [Test]
        public void should_aggregate_with_no_seperator()
        {
            _urls.Aggregate(x => x.Value, ", ")
                .ShouldEqual("http://oh, http://hai");
        }

        [Test]
        public void should_aggregate_no_items_to_a_empty_string()
        {
            new List<string>().Aggregate("(", " ,", ")").ShouldBeEmpty();
        }

        [Test]
        public void should_aggregate_items_and_seperator()
        {
            new List<string> { "oh", "hai" }.Aggregate(", ").ShouldEqual("oh, hai");
        }

        [Test]
        public void should_aggregate_items_and_seperator_and_qualifiers()
        {
            new List<string> { "oh", "hai" }.Aggregate("(", ", ", ")").ShouldEqual("(oh, hai)");
        }

        [Test]
        public void should_conditionally_add_items_to_a_list()
        {
            new List<string>().Add("hai", true).ShouldContain("hai");
            new List<string>().Add("hai", false).ShouldBeEmpty();
        }

        [Test]
        public void should_foreach_on_ienumerable()
        {
            var count = 0;
            new [] { 1, 2 }.ForEach(x => count += x);
            count.ShouldEqual(3);
        }

        [Test]
        public void should_exclude_matching_results()
        {
            new [] { 1, 2, 3 }.Exclude(new [] { 2 }, (s, c) => s == c).ShouldEqual(new [] { 1, 3 });
        }

        public class Walkable
        {
            public Walkable Parent { get; set; }
        }

        [Test]
        public void should_walk()
        {
            var node1 = new Walkable();
            var node2 = new Walkable { Parent = node1 };
            var node3 = new Walkable { Parent = node2 };

            var result = node3.Walk(x => x.Parent).ToList();

            result.ShouldTotal(3);
            result[0].ShouldEqual(node3);
            result[1].ShouldEqual(node2);
            result[2].ShouldEqual(node1);
        }

        [Test]
        public void should_aggregate()
        {
            new[] { "h", "a", "i" }.Aggregate().ShouldEqual("hai");
        }

        [Test]
        public void should_map_non_generic_dictionary_to_generic_enumerable_of_dictionary_entry()
        {
            var results = new Dictionary<string, object> 
                {{"oh", 1}, {"hai", 2}}.As<IDictionary>().AsEnumerable();
            results.ShouldTotal(2);

            var result = results.First();
            result.ShouldBeType<DictionaryEntry>();
            result.Key.ShouldEqual("oh");
            result.Value.ShouldEqual(1);

            result = results.Skip(1).First();
            result.ShouldBeType<DictionaryEntry>();
            result.Key.ShouldEqual("hai");
            result.Value.ShouldEqual(2);
        }

        public class Where
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }

        private static readonly List<Where> WhereData = new List<Where>
            {
                new Where { Value1 = 1, Value2 = "a" }, 
                new Where { Value1 = 2, Value2 = "b" }, 
                new Where { Value1 = 3, Value2 = "c" }
            }; 

        [Test]
        public void should_filter_results_when_predicate_is_not_null()
        {
            WhereData.Where(x => x.Value1, x => x > 1).ShouldAllMatch(x => x.Value1 > 1);
            WhereData.Where(x => x.Value1, x => x.Value2, (x1, x2) => x1 > 1 && x2 != "c")
                .ShouldAllMatch(x => x.Value1 == 2);
        }

        [Test]
        public void should_not_filter_results_when_predicate_is_null()
        {
            WhereData.Where(x => x.Value1, null).ShouldTotal(3);
            WhereData.Where(x => x.Value1, x => x.Value2, null).ShouldTotal(3);
        }

        [Test]
        public void should_cast_object()
        {
            ((object)new Tuple<string>("hai")).As<Tuple<string>>().Item1.ShouldEqual("hai");
        }

        [Test]
        public void should_cast_null_object_as_default_type()
        {
            ((object)null).As<int>().ShouldEqual(0);
            ((object)null).As<int?>().ShouldBeNull();
        }
    }
}
