using System;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using NUnit.Framework;
using Should;

namespace Tests.Collections
{
    [TestFixture]
    public class ArrayAdapterTests
    {
        private IValue _array;
        private ArrayAdapter _adapter;

        [SetUp]
        public void Setup()
        {
            _array = new SimpleValue(new[] { "oh", "hai" }, typeof(string[]).GetCachedType());
            _adapter = new ArrayAdapter(_array);
        }

        [Test]
        public void should_return_object_if_it_implements_ilist()
        {
            var list = new List<string>();
            IValue value = new SimpleValue(list, typeof(List<string>).GetCachedType());
            ArrayAdapter.Create(value).ShouldBeSameAs(list);
        }

        [Test]
        public void should_fail_if_not_a_array()
        {
            Assert.Throws<ArgumentException>(() => ArrayAdapter.Create(
                new SimpleValue(new object(), typeof(object).GetCachedType())));
        }

        [Test]
        public void should_return_readonly_flag()
        {
            _adapter.IsReadOnly.ShouldBeFalse();
        }

        [Test]
        public void should_return_fixed_size_flag()
        {
            _adapter.IsFixedSize.ShouldBeFalse();
        }

        [Test]
        public void should_return_count()
        {
             _adapter.Count.ShouldEqual(2);
        }
        
        [Test]
        public void should_return_sync_flag()
        {
            _adapter.IsSynchronized.ShouldBeFalse();
        }

        [Test]
        public void should_return_sync_root()
        {
            _adapter.SyncRoot.ShouldBeSameAs(_array.Instance);
        }

        [Test]
        public void should_return_value()
        {
            _adapter[0].ShouldEqual("oh");
            _adapter[1].ShouldEqual("hai");
        }

        [Test]
        public void should_return_index_of()
        {
            _adapter.IndexOf("oh").ShouldEqual(0);
            _adapter.IndexOf("hai").ShouldEqual(1);
            _adapter.IndexOf("orly").ShouldEqual(-1);
        }

        [Test]
        public void should_return_contains()
        {
            _adapter.Contains("oh").ShouldBeTrue();
            _adapter.Contains("hai").ShouldBeTrue();
            _adapter.Contains("orly").ShouldBeFalse();
        }

        [Test]
        public void should_add_item()
        {
            _adapter.Add("yo").ShouldEqual(2);
            var array = _array.Instance.As<string[]>();
            array.Length.ShouldEqual(3);
            array[2].ShouldEqual("yo");
        }

        [Test]
        public void should_insert_item()
        {
            _adapter.Insert(1, "yo");
            var array = _array.Instance.As<string[]>();
            array.Length.ShouldEqual(3);
            array[1].ShouldEqual("yo");
        }

        [Test]
        public void should_remove_item()
        {
            _adapter.Remove("hai");
            var array = _array.Instance.As<string[]>();
            array.Length.ShouldEqual(1);
            array[0].ShouldEqual("oh");
        }

        [Test]
        public void should_remove_item_at()
        {
            _adapter.RemoveAt(1);
            var array = _array.Instance.As<string[]>();
            array.Length.ShouldEqual(1);
            array[0].ShouldEqual("oh");
        }

        [Test]
        public void should_clear()
        {
            _adapter.Clear();
            _array.Instance.As<string[]>().Length.ShouldEqual(0);
        }

        [Test]
        public void should_copy_to_array()
        {
            var array = new object[2];
            _adapter.CopyTo(array, 0);
            array[0].ShouldEqual("oh");
            array[1].ShouldEqual("hai");
        }

        [Test]
        public void should_enumerate()
        {
            var entries = _adapter.Cast<string>().ToList();
            entries[0].ShouldEqual("oh");
            entries[1].ShouldEqual("hai");
        }
    }
}
