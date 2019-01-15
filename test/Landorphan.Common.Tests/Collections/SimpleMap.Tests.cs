namespace Landorphan.Common.Tests.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using FluentAssertions;
   using Landorphan.Common.Collections;
   using Landorphan.Common.Tests.Collections.TestClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   public static class SimpleMap_Tests
   {
      [TestClass]
      public class When_I_create_a_configured_SimpleMap : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Object, String>> expected = new List<KeyValuePair<Object, String>>
         {
            new KeyValuePair<Object, String>(new Object(), Guid.NewGuid().ToString()),
            new KeyValuePair<Object, String>(new Object(), Guid.NewGuid().ToString()),
            new KeyValuePair<Object, String>(new Object(), Guid.NewGuid().ToString())
         };

         private IEqualityComparer<Object> expectedKeyComparer;
         private IEqualityComparer<String> expectedValueComparer;
         private SimpleMap<Object, String> target;

         protected override void ArrangeMethod()
         {
            expectedKeyComparer = new ReferenceEqualityComparer<Object>();
            expectedValueComparer = StringComparer.OrdinalIgnoreCase;

            target = new SimpleMap<Object, String>(expected, expectedKeyComparer, expectedValueComparer);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_default_comparers()
         {
            target.KeyComparer.Should().Be(expectedKeyComparer);
            target.ValueComparer.Should().Be(expectedValueComparer);
         }
      }

      [TestClass]
      public class When_I_create_a_default_SimpleMap : ArrangeActAssert
      {
         private SimpleMap<Int64, Int32> target;

         protected override void ArrangeMethod()
         {
            target = new SimpleMap<Int64, Int32>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            target.Count.Should().Be(0);
            target.IsEmpty.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_enumerable()
         {
            target.Should().BeAssignableTo(typeof(IEnumerable));
            target.Should().BeAssignableTo(typeof(IEnumerable<KeyValuePair<Int64, Int32>>));
            ((IEnumerable<KeyValuePair<Int64, Int32>>)target).GetEnumerator().Should().NotBeNull();
            ((IEnumerable)target).GetEnumerator().Should().NotBeNull();

            target.Keys.Should().BeAssignableTo(typeof(IEnumerable));
            target.Keys.Should().BeAssignableTo(typeof(IEnumerable<Int64>));
            target.Keys.GetEnumerator().Should().NotBeNull();
            ((IEnumerable)target.Keys).GetEnumerator().Should().NotBeNull();

            target.Values.Should().BeAssignableTo(typeof(IEnumerable));
            target.Values.Should().BeAssignableTo(typeof(IEnumerable<Int32>));
            target.Values.GetEnumerator().Should().NotBeNull();
            ((IEnumerable)target.Values).GetEnumerator().Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_default_comparers()
         {
            target.KeyComparer.Should().Be(EqualityComparer<Int64>.Default);
            target.ValueComparer.Should().Be(EqualityComparer<Int32>.Default);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_read_only()
         {
            target.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_thread_safe()
         {
            target.IsThreadSafe.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_create_a_SimpleMap_from_a_collection : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private SimpleMap<Int64, Int32> target;

         protected override void ArrangeMethod()
         {
            target = new SimpleMap<Int64, Int32>(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_default_comparers()
         {
            target.KeyComparer.Should().Be(EqualityComparer<Int64>.Default);
            target.ValueComparer.Should().Be(EqualityComparer<Int32>.Default);
         }
      }

      [TestClass]
      public class When_I_create_a_SimpleMap_from_a_null_collection : ArrangeActAssert
      {
         private SimpleMap<Int64, Int32> target;

         protected override void ArrangeMethod()
         {
            target = new SimpleMap<Int64, Int32>(null, null, null);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            target.Count.Should().Be(0);
            target.IsEmpty.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_default_comparers()
         {
            target.KeyComparer.Should().Be(EqualityComparer<Int64>.Default);
            target.ValueComparer.Should().Be(EqualityComparer<Int32>.Default);
         }
      }

      [TestClass]
      public class When_I_create_a_SimpleMap_with_specified_comparers : ArrangeActAssert
      {
         private IEqualityComparer<Object> expectedKeyComparer;
         private IEqualityComparer<String> expectedValueComparer;
         private SimpleMap<Object, String> target;

         protected override void ArrangeMethod()
         {
            expectedKeyComparer = new ReferenceEqualityComparer<Object>();
            expectedValueComparer = StringComparer.OrdinalIgnoreCase;

            target = new SimpleMap<Object, String>(expectedKeyComparer, expectedValueComparer);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_default_comparers()
         {
            target.KeyComparer.Should().Be(expectedKeyComparer);
            target.ValueComparer.Should().Be(expectedValueComparer);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_assign_by_key : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_add_when_the_key_does_not_exist()
         {
            var before = target.Count;
            target[-1] = -1;
            target.Count.Should().Be(before + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_update_when_the_key_exists()
         {
            var before = target.Count;
            target[0] = -1;
            target.Count.Should().Be(before);
            target[0].Should().Be(-1);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_Add : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ActMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_Add_with_a_duplicate_key : ArrangeActAssert
      {
         private readonly Guid existingKey = Guid.NewGuid();
         private readonly Guid existingValue = Guid.NewGuid();
         private readonly SimpleMap<Guid, Guid> target = new SimpleMap<Guid, Guid>();

         protected override void ArrangeMethod()
         {
            target.Add(existingKey, existingValue);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_the_add()
         {
            var newValue = Guid.NewGuid();
            var added = target.Add(existingKey, newValue);
            added.Should().BeFalse();
            target[existingKey].Should().Be(existingValue);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_Add_with_a_null_key : ArrangeActAssert
      {
         private readonly SimpleMap<String, String> target = new SimpleMap<String, String>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => target.Add(null, Guid.NewGuid().ToString());
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("key");
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_AddOrUpdate : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_add_when_the_key_does_not_exist()
         {
            var before = target.Count;
            target.AddOrUpdate(-1, -1);
            target.Count.Should().Be(before + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_update_when_the_key_exists()
         {
            var before = target.Count;
            target.AddOrUpdate(0, -1);
            target.Count.Should().Be(before);
            target[0].Should().Be(-1);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_Clear : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         protected override void ActMethod()
         {
            target.Clear();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            target.Count.Should().Be(0);
            target.IsEmpty.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_ContainsKey : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_the_key_does_not_exist()
         {
            target.ContainsKey(-1).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_key_exists()
         {
            target.ContainsKey(0).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_ContainsValue : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_the_value_does_not_exist()
         {
            target.ContainsValue(-1).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_value_exists()
         {
            target.ContainsValue(0).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_MakeReadOnly : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_read_only_status()
         {
            target.IsReadOnly.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_thread_safe_status()
         {
            target.IsThreadSafe.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_Add()
         {
            Action throwingAction = () => target.Add(Int64.MaxValue, Int32.MaxValue);
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_AddOrUpdate()
         {
            Action throwingAction = () => target.AddOrUpdate(0, Int32.MaxValue);
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_Clear()
         {
            Action throwingAction = () => target.Clear();
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_Remove()
         {
            Action throwingAction = () => target.Remove(0);
            throwingAction.Should().Throw<NotSupportedException>();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_Remove : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_key_does_not_exist()
         {
            var before = target.Count;
            var result = target.Remove(-1);
            result.Should().BeFalse();
            target.Count.Should().Be(before);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_when_the_key_exists()
         {
            var before = target.Count;
            var result = target.Remove(0);
            result.Should().BeTrue();
            target.Count.Should().Be(before - 1);
            target.ContainsKey(0).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_ToDictionary : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();
         private IDictionary<Int64, Int32> actual;

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         protected override void ActMethod()
         {
            actual = target.ToDictionary();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_creates_an_equivalent_dictionary()
         {
            actual.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_call_TryGetValue : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 2),
            new KeyValuePair<Int64, Int32>(2, 4)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_an_invalid_key()
         {
            target[-1].Should().Be(default);

            Int32 actual;
            var retrieved = target.TryGetValue(-1, out actual);
            retrieved.Should().BeFalse();
            actual.Should().Be(default);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_expected_value_for_a_valid_key()
         {
            target[0].Should().Be(0);
            target[1].Should().Be(2);
            target[2].Should().Be(4);

            Int32 actual;
            var retrieved = target.TryGetValue(2, out actual);
            retrieved.Should().BeTrue();
            actual.Should().Be(4);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_and_have_the_same_value_assigned_to_more_than_one_key : ArrangeActAssert
      {
         private readonly List<KeyValuePair<Int64, Int32>> expected = new List<KeyValuePair<Int64, Int32>>
         {
            new KeyValuePair<Int64, Int32>(0, 0),
            new KeyValuePair<Int64, Int32>(1, 0),
            new KeyValuePair<Int64, Int32>(2, 0)
         };

         private readonly SimpleMap<Int64, Int32> target = new SimpleMap<Int64, Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item.Key, item.Value);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_duplicates_when_I_get_the_values()
         {
            target.Values.Should().Contain(new[] {0, 0, 0});
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleMap_with_mutable_keys_and_mutate_them_into_a_collision : ArrangeActAssert
      {
         private EquatableTestItem keyA;
         private EquatableTestItem keyB;
         private SimpleMap<EquatableTestItem, Int32> target;

         protected override void ArrangeMethod()
         {
            // create a set of unique items
            target = new SimpleMap<EquatableTestItem, Int32>(new EquatableTestItemEqualityComparer(), EqualityComparer<Int32>.Default);
            keyA = new EquatableTestItem {Name = "Aaa", Value = 1};
            keyB = new EquatableTestItem {Name = "Bbb", Value = 2};
            target.Add(keyA, 1);
            target.Add(keyB, 2);

            // the comparer.GetHashCode() must differ from EquatableTestItem.GetHashCode()
            new EquatableTestItemEqualityComparer().GetHashCode(keyA).Should().NotBe(keyA.GetHashCode());
         }

         protected override void ActMethod()
         {
            // mutate one of the keys so it collides with another key.
            keyB.Name = keyA.Name;
            keyB.Value = keyA.Value;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void IsKeyHashCorrupted_Should_Be_True()
         {
            target.IsKeyHashCorrupted().Should().BeTrue();
         }
      }
   }
}
