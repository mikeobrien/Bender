using Bender.Configuration;
using Bender.NamingConventions;
using NUnit.Framework;
using Should;

namespace Tests.NamingConventions
{
    [TestFixture]
    public class NamingConventionsTests
    {
        private static NamingConventions<int> GetConventions()
        {
            return new NamingConventions<int>(x => x.ToString());
        }

        [Test]
        public void should_get_default_convention()
        {
            GetConventions().GetName(5).ShouldEqual("5");
        }

        [Test]
        public void should_allow_you_to_set_the_default_convention()
        {
            GetConventions().SetDefault(c => c + c.ToString()).GetName(5).ShouldEqual("55");
        }

        [Test]
        public void should_apply_override_to_source_if_added_before()
        {
            var conventions = GetConventions()
                .Add(n => n += "*")
                .Add(c => (c * c).ToString(), c => c > 4);

            conventions.GetName(4).ShouldEqual("4*");
            conventions.GetName(5).ShouldEqual("25*");
        }

        [Test]
        public void should_apply_override_to_source_if_added_after()
        {
            var conventions = GetConventions()
                .Add(c => (c * c).ToString(), c => c > 4)
                .Add(n => n += "*");

            conventions.GetName(4).ShouldEqual("4*");
            conventions.GetName(5).ShouldEqual("25*");
        }

        [Test]
        public void should_apply_overrides_in_the_order_they_were_added()
        {
            GetConventions()
                .Add(n => n += "-1")
                .Add(n => n += "-2", n => true)
                .Add((n, c) => n += "-3" + c)
                .Add((n, c) => n += "-4" + c, (n, c) => true)
                .GetName(5).ShouldEqual("5-1-2-35-45");
        }

        [Test]
        public void should_apply_overrides_conditionally()
        {
            GetConventions()
                .Add(n => n += "-1", n => true)
                .Add(n => n += "-2", n => false)
                .Add((n, c) => n += "-3" + c, (n, c) => true)
                .Add((n, c) => n += "-4" + c, (n, c) => false)
                .GetName(5).ShouldEqual("5-1-35");
        }

        [Test]
        public void should_evaluate_last_added_sources_first()
        {
            var conventions = GetConventions()
                .Add(c => "1" + c, c => c > 4)
                .Add(c => "2" + c, c => c > 5)
                .Add(c => "3" + c, c => c > 6);

            conventions.GetName(4).ShouldEqual("4");
            conventions.GetName(5).ShouldEqual("15");
            conventions.GetName(6).ShouldEqual("26");
            conventions.GetName(7).ShouldEqual("37");
        }
    }
}
