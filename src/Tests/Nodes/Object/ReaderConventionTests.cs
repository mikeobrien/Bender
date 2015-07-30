using System;
using Bender;
using Bender.Configuration;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class ReaderConventionTests
    {
        private INode _source;
        private ValueNode _target;
        private Options _options;
        private ReaderConventions _readers;
        private Action<INode, INode, Options> _visitorIncrement;
        private Action<INode, INode, Options> _readerIncrement;
        private Func<object, INode, INode, Options, int> _readerIncrementValue;
        private Func<object, INode, INode, Options, int?> _readerIncrementNullableValue;

        [SetUp]
        public void Setup()
        {
            _options = Options.Create();
            _source = new ValueNode(
                new Context(_options, Mode.Deserialize, "json"), null,
                new SimpleValue(typeof(int?).ToCachedType()), null, null);
            _target = new ValueNode(
                new Context(_options, Mode.Deserialize, "json"), null,
                new SimpleValue(typeof(int?).ToCachedType()), null, null);
            _readers = new ReaderConventions(_options);

            _visitorIncrement = (s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int?)t.Value + 1;
            };

            _readerIncrement = (s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int?)t.Value + 1;
            };

            _readerIncrementValue = (v, s, t, o) =>
            {
                should_have_valid_parameters(v, s, t, o);
                return s.Value == null ? 1 : (int)s.Value + 1;
            };

            _readerIncrementNullableValue = (v, s, t, o) =>
            {
                should_have_valid_parameters(v, s, t, o);
                return s.Value == null ? 1 : (int?)s.Value + 1;
            };
        }

        private void should_have_valid_parameters(
            INode source, INode target, Options options)
        {
            source.ShouldBeSameAs(_source);
            target.ShouldBeSameAs(_target);
            options.ShouldBeSameAs(_options);
        }

        private void should_have_valid_parameters(
            object value, INode source, INode target, Options options)
        {
            value.ShouldEqual(source.Value);
            source.ShouldBeSameAs(_source);
            target.ShouldBeSameAs(_target);
            options.ShouldBeSameAs(_options);
        }

        // Visit all
            
        [Test]
        public void should_add_visitor_and_visit()
        {
            _readers
                .AddVisitingReader(_visitorIncrement)
                .AddVisitingReader(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_and_fail_visit_with_reader_exception()
        {
            var exception = new Exception();
            _readers.AddVisitingReader((s, t, o) => { throw exception; });

            Assert.DoesNotThrow(() => _readers.Visitors.HasVisitor(_source, _target));

            var visitorException = Assert.Throws<ReaderException>(() => _readers
                .Visitors.Visit(_source, _target));

            visitorException.InnerException.ShouldBeSameAs(exception);
            visitorException.Message.ShouldEqual("Reader failed: Exception of type 'System.Exception' was thrown.");
        }

        public class SomeFriendlyBenderException : FriendlyBenderException
        {
            public SomeFriendlyBenderException(string message) : base(message, message) { }
        }

        [Test]
        public void should_add_visitor_and_fail_visit_with_friendly_bender_exception()
        {
            var exception = new SomeFriendlyBenderException("yada");
            _readers.AddVisitingReader((s, t, o) => { throw exception; });

            Assert.DoesNotThrow(() => _readers.Visitors.HasVisitor(_source, _target));

            var visitorException = Assert.Throws<SomeFriendlyBenderException>(() => _readers
                .Visitors.Visit(_source, _target));

            visitorException.ShouldBeSameAs(exception);
            visitorException.Message.ShouldEqual("yada");
        }

        // Visit matches

        [Test]
        public void should_add_visitor_and_visit_when_matches()
        {
            _readers
                .AddVisitingReader(_visitorIncrement, (s, t, o) => true)
                .AddVisitingReader(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_not_matches()
        {
            _readers
                .AddVisitingReader(_visitorIncrement, (s, t, o) => false)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_visitor_and_fail_visit_when_with_reader_exception()
        {
            var exception = new Exception();
            _readers.AddVisitingReader((s, t, o) => { }, (s, t, o) => { throw exception; });

            Assert.Throws<ReaderException>(() => _readers
                .Visitors.HasVisitor(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<ReaderException>(() => _readers
                .Visitors.Visit(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        // Visit type

        [Test]
        public void should_add_visitor_and_visit_when_type_matches()
        {
            _readers
                .AddVisitingReader<int?>(_visitorIncrement)
                .AddVisitingReader<int?>(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_type_does_not_match()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Visit nullable type

        [Test]
        public void should_add_visitor_and_visit_when_nullable_type_matches()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, true)
                .AddVisitingReader<int>(_visitorIncrement, true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_nullable_type_does_not_match()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, false)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Visit type matches

        [Test]
        public void should_add_visitor_and_visit_when_type_and_predicate_matches()
        {
            _readers
                .AddVisitingReader<int?>(_visitorIncrement, (s, t, o) => true)
                .AddVisitingReader<int?>(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddVisitingReader<int?>(_visitorIncrement, (s, t, o) => false)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_predicate_matches_but_type_does_not_match()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Visit nullable type matches

        [Test]
        public void should_add_visitor_and_visit_when_non_nullable_type_and_predicate_matches()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, (s, t, o) => true, true)
                .AddVisitingReader<int>(_visitorIncrement, (s, t, o) => true, true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_nullable_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, (s, t, o) => false, true)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_predicate_matches_but_nullable_type_does_not_match()
        {
            _readers
                .AddVisitingReader<int>(_visitorIncrement, (s, t, o) => true, false)
                .Visitors.Visit(_source, _target);

            _readers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read matches

        [Test]
        public void should_add_reader_and_read_when_matches()
        {
            _readers
                .AddReader(_readerIncrement, (s, t, o) => true)
                .AddReader(_readerIncrement, (s, t, o) => true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_reader_but_not_read_when_not_matches()
        {
            _readers
                .AddReader(_readerIncrement, (s, t, o) => false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_reader_and_fail_read_with_reader_exception()
        {
            var exception = new Exception();
            _readers.AddReader((s, t, o) => { throw exception; }, (s, t, o) => true);

            Assert.DoesNotThrow(() => _readers.Mapping.HasMapping(_source, _target));

            var mappingException = Assert.Throws<ReaderException>(() => _readers
                .Mapping.Map(_source, _target));

            mappingException.InnerException.ShouldBeSameAs(exception);
            mappingException.Message.ShouldEqual("Reader failed: Exception of type 'System.Exception' was thrown.");
        }

        [Test]
        public void should_add_reader_and_fail_read_with_friendly_bender_exception()
        {
            var exception = new SomeFriendlyBenderException("yada");
            _readers.AddReader((s, t, o) => { throw exception; }, (s, t, o) => true);

            Assert.DoesNotThrow(() => _readers.Mapping.HasMapping(_source, _target));

            var mappingException = Assert.Throws<SomeFriendlyBenderException>(() => _readers
                .Mapping.Map(_source, _target));

            mappingException.ShouldBeSameAs(exception);
            mappingException.Message.ShouldEqual("yada");
        }

        [Test]
        public void should_add_reader_and_fail_read_when_with_reader_exception()
        {
            var exception = new Exception();
            _readers.AddReader((s, t, o) => { }, (s, t, o) => { throw exception; });

            Assert.Throws<ReaderException>(() => _readers
                .Mapping.HasMapping(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<ReaderException>(() => _readers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        // Read type

        [Test]
        public void should_add_reader_and_read_when_type_matches()
        {
            _readers
                .AddReader<int?>(_readerIncrement)
                .AddReader<int?>(_readerIncrement)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_reader_but_not_read_when_type_does_not_match()
        {
            _readers
                .AddReader<int>(_readerIncrement)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read nullable type

        [Test]
        public void should_add_reader_and_read_when_nullable_type_matches()
        {
            _readers
                .AddReader<int>(_readerIncrement, true)
                .AddReader<int>(_readerIncrement, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_reader_but_not_read_when_nullable_type_does_not_match()
        {
            _readers
                .AddReader<int>(_readerIncrement, false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read type matches

        [Test]
        public void should_add_reader_and_read_when_type_and_predicate_matches()
        {
            _readers
                .AddReader<int?>(_readerIncrement, (s, t, o) => true)
                .AddReader<int?>(_readerIncrement, (s, t, o) => true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_reader_but_not_read_when_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddReader<int?>(_readerIncrement, (s, t, o) => false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_reader_but_not_read_when_predicate_matches_but_type_does_not_match()
        {
            _readers
                .AddReader<int>(_readerIncrement, (s, t, o) => true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read nullable type matches

        [Test]
        public void should_add_reader_and_read_when_non_nullable_type_and_predicate_matches()
        {
            _readers
                .AddReader<int>(_readerIncrement, (s, t, o) => true, true)
                .AddReader<int>(_readerIncrement, (s, t, o) => true, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_reader_but_not_read_when_nullable_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddReader<int>(_readerIncrement, (s, t, o) => false, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_reader_but_not_read_when_predicate_matches_but_nullable_type_does_not_match()
        {
            _readers
                .AddReader<int>(_readerIncrement, (s, t, o) => true, false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read value type

        [Test]
        public void should_add_value_reader_and_not_read_when_source_value_is_null()
        {
            _source.Value = null;
            Assert.DoesNotThrow(() => _readers
                .AddValueReader<int?>((v, s, t, o) => { throw new Exception(); })
                .Mapping.Map(_source, _target));

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_reader_and_read_when_type_matches()
        {
            _source.Value = 0;
            _readers
                .AddValueReader<int?>(_readerIncrementNullableValue)
                .AddValueReader<int?>(_readerIncrementNullableValue)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_type_does_not_match()
        {
            _readers
                .AddValueReader<int>(_readerIncrementValue)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_reader_and_fail_read_with_value_reader_exception_when_friendly_error_does_not_exist()
        {
            _source.Value = 0;
            var target = new ValueNode(
                new Context(_options, Mode.Deserialize, "json"), null,
                new SimpleValue(typeof(Tuple<string>).ToCachedType()), null, null);
            var exception = new Exception();
            _readers.AddValueReader<Tuple<string>>((v, s, t, o) => { throw exception; }, (v, s, t, o) => true);

            Assert.DoesNotThrow(() => _readers.Mapping.HasMapping(_source, target));

            Assert.Throws<ReaderException>(() => _readers
                .Mapping.Map(_source, target))
                .InnerException.ShouldBeSameAs(exception);
        }

        [Test]
        public void should_add_value_reader_and_fail_read_with_value_parse_exception_when_friendly_error_exists()
        {
            var exception = new Exception();
            _source.Value = 0;
            _readers.AddValueReader<int?>((v, s, t, o) => { throw exception; }, (v, s, t, o) => true);

            Assert.DoesNotThrow(() => _readers.Mapping.HasMapping(_source, _target));

            Assert.Throws<ValueParseException>(() => _readers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        [Test]
        public void should_add_value_reader_and_fail_read_when_with_reader_exception()
        {
            var exception = new Exception();
            _readers.AddValueReader<int?>((v, s, t, o) => 0, (v, s, t, o) => { throw exception; });

            Assert.Throws<ReaderException>(() => _readers
                .Mapping.HasMapping(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<ReaderException>(() => _readers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        // Read value nullable type

        [Test]
        public void should_add_value_reader_and_read_when_nullable_type_matches()
        {
            _source.Value = 0;
            _readers
                .AddValueReader<int>(_readerIncrementValue, true)
                .AddValueReader<int>(_readerIncrementValue, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_nullable_type_does_not_match()
        {
            _readers
                .AddValueReader<int>(_readerIncrementValue, false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read value type matches

        [Test]
        public void should_add_value_reader_and_read_when_type_and_predicate_matches()
        {
            _source.Value = 0;
            _readers
                .AddValueReader<int?>(_readerIncrementNullableValue, (v, s, t, o) => true)
                .AddValueReader<int?>(_readerIncrementNullableValue, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddValueReader<int?>(_readerIncrementNullableValue, (v, s, t, o) => false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_predicate_matches_but_type_does_not_match()
        {
            _readers
                .AddValueReader<int>(_readerIncrementValue, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Read value nullable type matches

        [Test]
        public void should_add_value_reader_and_read_when_non_nullable_type_and_predicate_matches()
        {
            _source.Value = 0;
            _readers
                .AddValueReader<int>(_readerIncrementValue, (v, s, t, o) => true, true)
                .AddValueReader<int>(_readerIncrementValue, (v, s, t, o) => true, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_nullable_type_matches_but_predicate_does_not_match()
        {
            _readers
                .AddValueReader<int>(_readerIncrementValue, (v, s, t, o) => false, true)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_reader_but_not_read_when_predicate_matches_but_nullable_type_does_not_match()
        {
            _readers
                .AddValueReader<int>(_readerIncrementValue, (v, s, t, o) => true, false)
                .Mapping.Map(_source, _target);

            _readers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }
    }
}