using Bender.Nodes;
using NUnit.Framework;
using Should;

namespace Tests.Nodes
{
    [TestFixture]
    public class MetadataTests
    {
        public class SomeMetadata { }

        [Test]
        public void should_add_and_indicate_that_metadata_exists()
        {
            var metadata = new SomeMetadata();
            new Metadata().Add(metadata).Contains<SomeMetadata>().ShouldBeTrue();
        }

        [Test]
        public void should_add_and_indicate_that_metadata_does_not_exist()
        {
            new Metadata().Contains<SomeMetadata>().ShouldBeFalse();
        }

        [Test]
        public void should_add_and_get_metadata()
        {
            var metadata = new SomeMetadata();
            new Metadata().Add(metadata).Get<SomeMetadata>().ShouldBeSameAs(metadata);
        }

        [Test]
        public void should_return_default_when_metadata_doesent_exist()
        {
            new Metadata().Get<SomeMetadata>().ShouldBeNull();
        }

        [Test]
        public void should_add_multiple_and_get_all_metadata()
        {
            var metadata1 = new SomeMetadata();
            var metadata2 = new SomeMetadata();
            var results = new Metadata().Add(metadata1).Add(metadata2).GetAll<SomeMetadata>();
            results.ShouldTotal(2);
            results.ShouldContain(metadata1);
            results.ShouldContain(metadata2);
        }

        [Test]
        public void should_return_empty_results_when_metadata_doesent_exist()
        {
            new Metadata().GetAll<SomeMetadata>().ShouldBeEmpty();
        }
    }
}
