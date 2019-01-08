namespace Landorphan.Common.Tests.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Collections;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class InlinePriorityQueue_Tests
   {
      [TestClass]
      public class When_I_create_a_configured_InlinePriorityQueue : ArrangeActAssert
      {
         private List<String> expectedCollection;
         private Comparison<String> expectedComparison;
         private Boolean expectedIgnoresNullValues;
         private InlinePriorityQueue<String> target;

         protected override void ArrangeMethod()
         {
            expectedCollection = new List<String> {Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            expectedComparison = StringComparer.Ordinal.Compare;
            expectedIgnoresNullValues = false;

            target = new InlinePriorityQueue<String>(expectedCollection, expectedComparison, expectedIgnoresNullValues);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expectedCollection);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_treat_nulls_as_configured()
         {
            target.IgnoresNullValues.Should().Be(expectedIgnoresNullValues);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_use_the_specified_comparison()
         {
            target.PriorityComparison.Should().Be(expectedComparison);
         }
      }

      [TestClass]
      public class When_I_create_a_default_InlinePriorityQueue : ArrangeActAssert
      {
         private InlinePriorityQueue<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new InlinePriorityQueue<Int32>();
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
            target.Should().BeAssignableTo(typeof(IEnumerable<Int32>));
            ((IEnumerable<Int32>) target).GetEnumerator().Should().NotBeNull();
            ((IEnumerable) target).GetEnumerator().Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_null_values()
         {
            target.IgnoresNullValues.Should().BeTrue();
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

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_support_EqualityComparer()
         {
            ((IReadableBagEnumerable<Int32>) target).EqualityComparer.Should().BeNull();
         }
      }

      // TODO: serialization test.

      [TestClass]
      public class When_I_create_a_InlinePriorityQueue_and_specify_the_comparison : ArrangeActAssert
      {
         private Comparison<String> expectedComparison;
         private InlinePriorityQueue<String> target;

         protected override void ArrangeMethod()
         {
            expectedComparison = StringComparer.Ordinal.Compare;

            target = new InlinePriorityQueue<String>(expectedComparison);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            target.IsEmpty.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_null_values()
         {
            target.IgnoresNullValues.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_use_the_specified_comparison()
         {
            target.PriorityComparison.Should().Be(expectedComparison);
         }
      }

      [TestClass]
      public class When_I_create_a_InlinePriorityQueue_from_a_collection_with_nulls_and_do_not_ignore_nulls : ArrangeActAssert
      {
         private List<String> expected;
         private InlinePriorityQueue<String> target;

         protected override void ArrangeMethod()
         {
            var original = new List<String> {Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString(), null};
            target = new InlinePriorityQueue<String>(original, null, false);
            expected = (from o in original select o).ToList();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_nulls()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_create_a_InlinePriorityQueue_from_a_collection_with_nulls_and_ignore_nulls : ArrangeActAssert
      {
         private List<String> expected;
         private InlinePriorityQueue<String> target;

         protected override void ArrangeMethod()
         {
            var original = new List<String> {Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString(), null};
            target = new InlinePriorityQueue<String>(original, null, true);
            expected = (from o in original where o.IsNotNull() select o).ToList();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_contain_the_nulls()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_create_an_InlinePriorityQueue_from_a_collection : ArrangeActAssert
      {
         private List<Int32> expected;
         private InlinePriorityQueue<Int32> target;

         protected override void ArrangeMethod()
         {
            expected = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
            target = new InlinePriorityQueue<Int32>(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_create_an_InlinePriorityQueue_from_a_null_collection : ArrangeActAssert
      {
         private InlinePriorityQueue<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new InlinePriorityQueue<Int32>(null, null, true);
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
      public class When_I_have_a_InlinePriorityQueue_and_call_Add : ArrangeActAssert
      {
         private Int32 addedValue;
         private InlinePriorityQueue<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new InlinePriorityQueue<Int32>();
         }

         protected override void ActMethod()
         {
            addedValue = 3;
            ((ICollection<Int32>) target).Add(addedValue);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_enqueue_the_value()
         {
            target.Peek().Should().Be(addedValue);
         }
      }

      [TestClass]
      public class When_I_have_a_InlinePriorityQueue_and_call_Remove : ArrangeActAssert
      {
         private IQueue<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new InlinePriorityQueue<Int32>();

            target.Enqueue(3);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => ((ICollection<Int32>) target).Remove(3);
            throwingAction.Should().Throw<NotSupportedException>().WithMessage("*Specified method is not supported*");
         }
      }

      [TestClass]
      public class When_I_have_an_empty_InlinePriorityQueue_and_call_Dequeue : ArrangeActAssert
      {
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => target.Dequeue();
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*empty*");
         }
      }

      [TestClass]
      public class When_I_have_an_empty_InlinePriorityQueue_and_call_Peek : ArrangeActAssert
      {
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => target.Peek();
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*empty*");
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Clear : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {0, 1, 2};
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Enqueue(item);
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
            target.IsEmpty.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Contains : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {0, 1, 2};
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Enqueue(item);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_for_values_not_contained()
         {
            var below = (from value in target select value).Min() - 1;
            var above = (from value in target select value).Max() + 1;

            ((IContainsEnumerable<Int32>) target).Contains(below).Should().BeFalse();
            ((IContainsEnumerable<Int32>) target).Contains(above).Should().BeFalse();

            ((ICollection<Int32>) target).Contains(below).Should().BeFalse();
            ((ICollection<Int32>) target).Contains(above).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_for_values_contained()
         {
            var values = (from value in target select value).Distinct().ToList();
            foreach (var v in values)
            {
               ((IContainsEnumerable<Int32>) target).Contains(v).Should().BeTrue();
               ((ICollection<Int32>) target).Contains(v).Should().BeTrue();
            }
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Dequeue : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {3, 3, 0, 1, 2, 2};
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();
         private Int32 originalCount;
         private Int32 value;

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Enqueue(item);
            }

            originalCount = target.Count;
         }

         protected override void ActMethod()
         {
            value = target.Dequeue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_only_one_value()
         {
            value.Should().Be(0);
            target.Count.Should().Be(originalCount - 1);
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Enqueue : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
         private readonly InlinePriorityQueue<Int32> target = new InlinePriorityQueue<Int32>();

         protected override void ActMethod()
         {
            foreach (var item in expected)
            {
               target.Enqueue(item);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_duplicate_values()
         {
            var countBefore = target.Count;
            target.Enqueue(target.First());
            target.Count.Should().Be(countBefore + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values_in_the_expected_order()
         {
            target.Should().Contain(expected);
            expected.Sort();

            var actualSequence = new List<Int32>();
            while (!target.IsEmpty)
            {
               actualSequence.Add(target.Dequeue());
            }

            actualSequence.SequenceEqual(expected).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Peek : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {2, 1, 0, 1, 2};
         private readonly IQueue<Int32> target = new InlinePriorityQueue<Int32>();
         private Int32 value;

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Enqueue(item);
            }
         }

         protected override void ActMethod()
         {
            value = target.Peek();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_prioritized_value()
         {
            value.Should().Be(0);
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_and_call_Reschedule : ArrangeActAssert
      {
         private readonly List<TestValue> expected = new List<TestValue>();
         private readonly InlinePriorityQueue<TestValue> target = new InlinePriorityQueue<TestValue>(Comparer<TestValue>.Default.Compare);
         private TestValue before;
         private TestValue changed;

         protected override void ArrangeMethod()
         {
            changed = new TestValue(100);
            expected.Add(changed);
            expected.Add(new TestValue(10));
            expected.Add(new TestValue(20));
            expected.Add(new TestValue(30));

            target.AddRange(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_reprioritized_item()
         {
            before = target.Peek();
            changed.Value = 1;
            target.Reschedule(changed);
            before.Value.Should().Be(10);
            target.Peek().Value.Should().Be(1);

            changed.Value = 100;
            target.Reschedule(changed);
            target.Peek().Value.Should().Be(10);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_reprioritized_value()
         {
            before = target.Peek();
            changed.Value = 1;
            target.RescheduleAll();
            before.Value.Should().Be(10);
            target.Peek().Value.Should().Be(1);
         }

         private class TestValue : IEquatable<TestValue>, IComparable<TestValue>
         {
            public TestValue(Int32 value)
            {
               Value = value;
            }

            public Int32 Value { get; set; }

            public static Boolean operator ==(TestValue left, TestValue right)
            {
               return Equals(left, right);
            }

            public static Boolean operator !=(TestValue left, TestValue right)
            {
               return !Equals(left, right);
            }

            public Int32 CompareTo(TestValue other)
            {
               if (ReferenceEquals(null, other))
               {
                  return 1;
               }

               return Value.CompareTo(other.Value);
            }

            public Boolean Equals(TestValue other)
            {
               if (ReferenceEquals(null, other))
               {
                  return false;
               }

               if (ReferenceEquals(this, other))
               {
                  return true;
               }

               return Value == other.Value;
            }

            public override Boolean Equals(Object obj)
            {
               if (ReferenceEquals(null, obj))
               {
                  return false;
               }

               if (ReferenceEquals(this, obj))
               {
                  return true;
               }

               if (obj.GetType() != GetType())
               {
                  return false;
               }

               return Equals((TestValue) obj);
            }

            public override Int32 GetHashCode()
            {
               return Value;
            }
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_that_does_not_ignore_nulls_and_call_Enqueue_with_a_null : ArrangeActAssert
      {
         private readonly InlinePriorityQueue<Object> target = new InlinePriorityQueue<Object>(null, null, false);

         protected override void ActMethod()
         {
            target.Enqueue(null);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(new Object[] {null});
         }
      }

      [TestClass]
      public class When_I_have_an_InlinePriorityQueue_that_ignores_nulls_and_call_Enqueue_with_a_null : ArrangeActAssert
      {
         private readonly InlinePriorityQueue<Object> target = new InlinePriorityQueue<Object>(null, null, true);

         protected override void ActMethod()
         {
            target.Enqueue(null);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().BeEmpty();
         }
      }
   }
}