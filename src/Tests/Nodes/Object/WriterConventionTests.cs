using System;
using Bender;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes;
using Bender.Nodes.Object;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Nodes.Object
{
    [TestFixture]
    public class WriterConventionTests
    {
        private ValueNode _source;
        private Node _target;
        private Options _options;
        private WriterConventions _writers;
        private Action<INode, INode, Options> _visitorIncrement;
        private Action<object, INode, INode, Options> _writerObjectIncrement;
        private Action<int, INode, INode, Options> _writerIncrement;
        private Action<int?, INode, INode, Options> _writerNullableIncrement;
        private Func<int, INode, INode, Options, object> _writerIncrementValue;
        private Func<int?, INode, INode, Options, object> _writerIncrementNullableValue;

        [SetUp]
        public void Setup()
        {
            _options = Options.Create();
            _source = new ValueNode(
                new Context(_options, Mode.Deserialize, "object"), null,
                new SimpleValue(typeof(int?).ToCachedType()), null, null);
            _target = new Node("yada") { NodeType = NodeType.Value, Format = "xml", Type = "element" };
            _writers = new WriterConventions(_options);

            _visitorIncrement = (s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int?)t.Value + 1;
            };

            _writerObjectIncrement = (v, s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int)t.Value + 1;
            };

            _writerIncrement = (v, s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int)t.Value + 1;
            };

            _writerNullableIncrement = (v, s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                t.Value = t.Value == null ? 1 : (int?)t.Value + 1;
            };

            _writerIncrementValue = (v, s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                return v == null ? 1 : (int)v + 1;
            };

            _writerIncrementNullableValue = (v, s, t, o) =>
            {
                should_have_valid_parameters(s, t, o);
                return v == null ? 1 : (int?)v + 1;
            };
        }

        private void should_have_valid_parameters(
            INode source, INode target, Options options)
        {
            source.ShouldBeSameAs(_source);
            target.ShouldBeSameAs(_target);
            options.ShouldBeSameAs(_options);
        }

        // Visit all

        [Test]
        public void should_add_visitor_and_visit()
        {
            _writers
                .AddVisitingWriter(_visitorIncrement)
                .AddVisitingWriter(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_and_fail_visit_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddVisitingWriter((s, t, o) => { throw exception; });

            Assert.DoesNotThrow(() => _writers.Visitors.HasVisitor(_source, _target));

            var visitorException = Assert.Throws<WriterException>(() => _writers
                .Visitors.Visit(_source, _target));

            visitorException.InnerException.ShouldBeSameAs(exception);

            visitorException.Message.ShouldEqual("Writer failed: Exception of type 'System.Exception' was thrown.");
        }

        public class SomeFriendlyBenderException : FriendlyBenderException
        {
            public SomeFriendlyBenderException(string message) : base(message, message) { }
        }

        [Test]
        public void should_add_visitor_and_fail_visit_with_friendly_bender_exception()
        {
            var exception = new SomeFriendlyBenderException("yada");
            _writers.AddVisitingWriter((s, t, o) => { throw exception; });

            Assert.DoesNotThrow(() => _writers.Visitors.HasVisitor(_source, _target));

            var visitorException = Assert.Throws<SomeFriendlyBenderException>(() => _writers
                .Visitors.Visit(_source, _target));

            visitorException.ShouldBeSameAs(exception);
            visitorException.Message.ShouldEqual("yada");
        }

        // Visit matches

        [Test]
        public void should_add_visitor_and_visit_when_matches()
        {
            _writers
                .AddVisitingWriter(_visitorIncrement, (s, t, o) => true)
                .AddVisitingWriter(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_not_matches()
        {
            _writers
                .AddVisitingWriter(_visitorIncrement, (s, t, o) => false)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_visitor_and_fail_visit_when_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddVisitingWriter((s, t, o) => { }, (s, t, o) => { throw exception; });

            Assert.Throws<WriterException>(() => _writers
                .Visitors.HasVisitor(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<WriterException>(() => _writers
                .Visitors.Visit(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        // Visit type

        [Test]
        public void should_add_visitor_and_visit_when_type_matches()
        {
            _writers
                .AddVisitingWriter<int?>(_visitorIncrement)
                .AddVisitingWriter<int?>(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_type_does_not_match()
        {
            _writers
                .AddVisitingWriter<int>(_visitorIncrement)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Visit type matches

        [Test]
        public void should_add_visitor_and_visit_when_type_and_predicate_matches()
        {
            _writers
                .AddVisitingWriter<int?>(_visitorIncrement, (s, t, o) => true)
                .AddVisitingWriter<int?>(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(2);
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_type_matches_but_predicate_does_not_match()
        {
            _writers
                .AddVisitingWriter<int?>(_visitorIncrement, (s, t, o) => false)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_visitor_but_not_visit_when_predicate_matches_but_type_does_not_match()
        {
            _writers
                .AddVisitingWriter<int>(_visitorIncrement, (s, t, o) => true)
                .Visitors.Visit(_source, _target);

            _writers.Visitors.HasVisitor(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Write matches

        [Test]
        public void should_add_writer_and_write_when_matches()
        {
            _writers
                .AddWriter(_writerObjectIncrement, (v, s, t, o) => true)
                .AddWriter(_writerObjectIncrement, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_writer_but_not_write_when_not_matches()
        {
            _writers
                .AddWriter(_writerObjectIncrement, (v, s, t, o) => false)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_writer_and_fail_write_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddWriter((v, s, t, o) => { throw exception; }, (v, s, t, o) => true);

            Assert.DoesNotThrow(() => _writers.Mapping.HasMapping(_source, _target));

            var mappingException = Assert.Throws<WriterException>(() => _writers
                .Mapping.Map(_source, _target));

            mappingException.InnerException.ShouldBeSameAs(exception);
            mappingException.Message.ShouldEqual("Writer failed: Exception of type 'System.Exception' was thrown.");
        }

        [Test]
        public void should_add_writer_and_fail_read_with_friendly_bender_exception()
        {
            var exception = new SomeFriendlyBenderException("yada");
            _writers.AddWriter((v, s, t, o) => { throw exception; }, (v, s, t, o) => true);

            Assert.DoesNotThrow(() => _writers.Mapping.HasMapping(_source, _target));

            var mappingException = Assert.Throws<SomeFriendlyBenderException>(() => _writers
                .Mapping.Map(_source, _target));

            mappingException.ShouldBeSameAs(exception);
            mappingException.Message.ShouldEqual("yada");
        }

        [Test]
        public void should_add_writer_and_fail_write_when_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddWriter((v, s, t, o) => { }, (v, s, t, o) => { throw exception; });

            Assert.Throws<WriterException>(() => _writers
                .Mapping.HasMapping(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<WriterException>(() => _writers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        // Write type

        [Test]
        public void should_add_writer_and_write_when_type_matches()
        {
            _writers
                .AddWriter<int?>(_writerNullableIncrement)
                .AddWriter<int?>(_writerNullableIncrement)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_writer_but_not_write_when_type_does_not_match()
        {
            _writers
                .AddWriter<int>(_writerIncrement)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Write type matches

        [Test]
        public void should_add_writer_and_write_when_type_and_predicate_matches()
        {
            _writers
                .AddWriter<int?>(_writerNullableIncrement, (v, s, t, o) => true)
                .AddWriter<int?>(_writerNullableIncrement, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_writer_but_not_write_when_type_matches_but_predicate_does_not_match()
        {
            _writers
                .AddWriter<int?>(_writerNullableIncrement, (v, s, t, o) => false)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_writer_but_not_write_when_predicate_matches_but_type_does_not_match()
        {
            _writers
                .AddWriter<int>(_writerIncrement, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        // Write value type

        [Test]
        public void should_add_value_writer_and_write_when_type_matches()
        {
            _writers
                .AddValueWriter<int?>(_writerIncrementNullableValue)
                .AddValueWriter<int?>(_writerIncrementNullableValue)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_writer_but_not_write_when_type_does_not_match()
        {
            _writers
                .AddValueWriter<int>(_writerIncrementValue)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_writer_and_fail_write_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddValueWriter<int?>((v, s, t, o) => { throw exception; }, (v, s, t, o) => true);

            Assert.DoesNotThrow(() => _writers.Mapping.HasMapping(_source, _target));

            Assert.Throws<WriterException>(() => _writers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        [Test]
        public void should_add_value_writer_and_fail_write_when_with_writer_exception()
        {
            var exception = new Exception();
            _writers.AddValueWriter<int?>((v, s, t, o) => 0, (v, s, t, o) => { throw exception; });

            Assert.Throws<WriterException>(() => _writers
                .Mapping.HasMapping(_source, _target))
                .InnerException.ShouldBeSameAs(exception);

            Assert.Throws<WriterException>(() => _writers
                .Mapping.Map(_source, _target))
                .InnerException.ShouldBeSameAs(exception);
        }

        [Test]
        public void should_should_change_target_node_type_to_value_when_writing_value()
        {
            _target.NodeType = NodeType.Object;
            _writers
                .AddValueWriter<int?>(_writerIncrementNullableValue)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
            _target.NodeType.ShouldEqual(NodeType.Value);
        }

        [Test]
        public void should_not_change_target_node_type_if_set_as_not_changeable_when_writing_value()
        {
            _target.NodeType = NodeType.Object;
            _target.HasFixedNodeType = true;
            var exception = Assert.Throws<WriterException>(() => _writers
                .AddValueWriter<int?>(_writerIncrementNullableValue)
                .Mapping.Map(_source, _target));

            exception.Message.ShouldEqual("Writer failed: Values not supported on object nodes.");
            exception.InnerException.ShouldBeType<ValueNotSupportedException>();
        }

        // Write value type matches

        [Test]
        public void should_add_value_writer_and_write_when_type_and_predicate_matches()
        {
            _writers
                .AddValueWriter<int?>(_writerIncrementNullableValue, (v, s, t, o) => true)
                .AddValueWriter<int?>(_writerIncrementNullableValue, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeTrue();
            _target.Value.ShouldEqual(1);
        }

        [Test]
        public void should_add_value_writer_but_not_write_when_type_matches_but_predicate_does_not_match()
        {
            _writers
                .AddValueWriter<int?>(_writerIncrementNullableValue, (v, s, t, o) => false)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }

        [Test]
        public void should_add_value_writer_but_not_write_when_predicate_matches_but_type_does_not_match()
        {
            _writers
                .AddValueWriter<int>(_writerIncrementValue, (v, s, t, o) => true)
                .Mapping.Map(_source, _target);

            _writers.Mapping.HasMapping(_source, _target).ShouldBeFalse();
            _target.Value.ShouldBeNull();
        }
    }
}
