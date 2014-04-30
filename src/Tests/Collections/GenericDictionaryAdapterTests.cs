using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bender.Collections;
using NUnit.Framework;
using Should;
using Tests.Collections.Implementations;

namespace Tests.Collections
{
    [TestFixture]
    public class GenericDictionaryAdapterTests
    {
        private GenericDictionaryImpl<string, string> _dictionary; 
        private IDictionary _adapter;

        [SetUp]
        public void Setup()
        {
            _dictionary = new GenericDictionaryImpl<string, string> 
                { { "item1", "oh" }, { "item2", "hai" } };
            _adapter = GenericDictionaryAdapter.Create(_dictionary);
        }

        [Test]
        public void should_return_object_if_it_implements_idictionary()
        {
            var dictionary = new Dictionary<string, string>();
            GenericDictionaryAdapter.Create(dictionary).ShouldBeSameAs(dictionary);
        }

        [Test]
        public void should_fail_if_not_a_dictionary()
        {
            Assert.Throws<ArgumentException>(() => GenericDictionaryAdapter.Create(new object()));
        }

        [Test]
        public void should_return_keys()
        {
            var keys = _adapter.Keys.Cast<string>();
            keys.ShouldTotal(2);
            keys.ShouldContain("item1");
            keys.ShouldContain("item2");
        }

        [Test]
        public void should_return_values()
        {
            var values = _adapter.Values.Cast<string>();
            values.ShouldTotal(2);
            values.ShouldContain("oh");
            values.ShouldContain("hai");
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
            _adapter.SyncRoot.ShouldEqual(_dictionary);
        }

        [Test]
        public void should_return_value()
        {
            _adapter["item1"].ShouldEqual("oh");
            _adapter["item2"].ShouldEqual("hai");
        }

        [Test]
        public void should_return_contains()
        {
            _adapter.Contains("item1").ShouldBeTrue();
            _adapter.Contains("item2").ShouldBeTrue();
            _adapter.Contains("item3").ShouldBeFalse();
        }

        [Test]
        public void should_add_item()
        {
            _adapter.Add("item3", "yo");
            _dictionary.Count.ShouldEqual(3);
            _dictionary["item3"].ShouldEqual("yo");
        }

        [Test]
        public void should_remove_item()
        {
            _adapter.Remove("item2");
            _dictionary.Count.ShouldEqual(1);
            _dictionary["item1"].ShouldEqual("oh");
        }

        [Test]
        public void should_clear()
        {
            _adapter.Clear();
            _dictionary.Count.ShouldEqual(0);
        }

        [Test]
        public void should_copy_to_array()
        {
            var array = new DictionaryEntry[2];
            _adapter.CopyTo(array, 0);
            array[0].Key.ShouldEqual("item1");
            array[0].Value.ShouldEqual("oh");
            array[1].Key.ShouldEqual("item2");
            array[1].Value.ShouldEqual("hai");
        }

        [Test]
        public void should_enumerate()
        {
            var entries = _adapter.Cast<DictionaryEntry>().ToList();
            entries[0].Key.ShouldEqual("item1");
            entries[0].Value.ShouldEqual("oh");
            entries[1].Key.ShouldEqual("item2");
            entries[1].Value.ShouldEqual("hai");
        }
    }
}
