namespace Landorphan.Common.Tests.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Collections;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class SimpleBag_Tests
   {
      [TestClass]
      public class When_I_create_a_configured_SimpleBag : ArrangeActAssert
      {
         private IEqualityComparer<String> expectedEqualityComparer;
         private Boolean expectedIgnoresNullValues;
         private SimpleBag<String> target;

         protected override void ArrangeMethod()
         {
            expectedEqualityComparer = StringComparer.Ordinal;
            expectedIgnoresNullValues = false;
            target = new SimpleBag<String>(null, expectedEqualityComparer, expectedIgnoresNullValues);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_the_specified_comparer()
         {
            target.EqualityComparer.Should().Be(expectedEqualityComparer);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_treat_nulls_as_specified()
         {
            target.IgnoresNullValues.Should().Be(expectedIgnoresNullValues);
         }
      }

      [TestClass]
      public class When_I_create_a_default_SimpleBag : ArrangeActAssert
      {
         private SimpleBag<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new SimpleBag<Int32>();
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
            target.GetEnumerator().Should().NotBeNull();
            ((IEnumerable)target).GetEnumerator().Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_a_default_comparer()
         {
            target.EqualityComparer.Should().Be(EqualityComparer<Int32>.Default);
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
      }

      [TestClass]
      public class When_I_create_a_SimpleBag_from_a_collection : ArrangeActAssert
      {
         private List<Int32> expected;
         private SimpleBag<Int32> target;

         protected override void ArrangeMethod()
         {
            expected = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
            target = new SimpleBag<Int32>(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_create_a_SimpleBag_from_a_collection_with_nulls_and_do_not_ignore_nulls : ArrangeActAssert
      {
         private List<Object> expected;
         private SimpleBag<Object> target;

         protected override void ArrangeMethod()
         {
            var original = new List<Object> {new Object(), null, new Object(), null};
            target = new SimpleBag<Object>(original, null, false);
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
      public class When_I_create_a_SimpleBag_from_a_collection_with_nulls_and_ignore_nulls : ArrangeActAssert
      {
         private List<Object> expected;
         private SimpleBag<Object> target;

         protected override void ArrangeMethod()
         {
            var original = new List<Object> {new Object(), null, new Object(), null};
            target = new SimpleBag<Object>(original, null, true);
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
      public class When_I_create_a_SimpleBag_from_a_null_collection : ArrangeActAssert
      {
         private SimpleBag<Int32> target;

         protected override void ArrangeMethod()
         {
            target = new SimpleBag<Int32>(null);
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
      public class When_I_have_a_SimpleBag_and_call_Add : ArrangeActAssert
      {
         private List<Int32> expected;
         private SimpleBag<Int32> target;

         protected override void ArrangeMethod()
         {
            expected = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
            target = new SimpleBag<Int32>();
         }

         protected override void ActMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void it_should_accept_duplicate_values()
         {
            var countBefore = target.Count;
            target.Add(target.First()).Should().BeTrue();
            target.Count.Should().Be(countBefore + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_Add_with_a_null_and_it_does_not_ignore_nulls : ArrangeActAssert
      {
         private readonly SimpleBag<Object> target = new SimpleBag<Object>(null, null, false);

         protected override void ActMethod()
         {
            target.Add(null);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(new Object[] {null});
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_Add_with_a_null_and_it_ignores_nulls : ArrangeActAssert
      {
         private readonly SimpleBag<Object> target = new SimpleBag<Object>(null, null, true);

         protected override void ActMethod()
         {
            target.Add(null);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().BeEmpty();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_AddRange : ArrangeActAssert
      {
         private readonly List<Int32> additional = new List<Int32> {5, 6, 7};
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};
         private List<Int32> expected;

         protected override void ArrangeMethod()
         {
            expected = new List<Int32>(target);
            expected.AddRange(additional);
         }

         protected override void ActMethod()
         {
            target.AddRange(additional);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_AddRange_with_a_null_collection : ArrangeActAssert
      {
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => target.AddRange(null);
            throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_Clear : ArrangeActAssert
      {
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};

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
      public class When_I_have_a_SimpleBag_and_call_Contains : ArrangeActAssert
      {
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_for_values_not_contained()
         {
            var below = (from value in target select value).Min() - 1;
            var above = (from value in target select value).Max() + 1;

            target.Contains(below).Should().BeFalse();
            target.Contains(above).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_for_values_contained()
         {
            var values = (from value in target select value).Distinct().ToList();
            foreach (var v in values)
            {
               target.Contains(v).Should().BeTrue();
            }
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_CopyTo : ArrangeActAssert
      {
         private readonly Int32[] array = new Int32[7];
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};

         protected override void ActMethod()
         {
            var t = target as ICollection<Int32>;
            t.CopyTo(array, 0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_copy_the_values()
         {
            array.Should().Contain(target);
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TAdd")]
      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_ICollectionOfT_Add : ArrangeActAssert
      {
         private readonly List<Int32> expected = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32>();

         protected override void ActMethod()
         {
            foreach (var item in expected)
            {
               ((ICollection<Int32>)target).Add(item);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void it_should_accept_duplicate_values()
         {
            var countBefore = target.Count;
            target.Add(target.First()).Should().BeTrue();
            target.Count.Should().Be(countBefore + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleBag_and_call_MakeReadOnly : ArrangeActAssert
      {
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};

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
            Action throwingAction = () => target.Add(Int32.MaxValue);
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_AddRange()
         {
            Action throwingAction = () => target.AddRange(new[] {Int32.MinValue, Int32.MaxValue});
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
      public class When_I_have_a_SimpleBag_and_call_Remove : ArrangeActAssert
      {
         private readonly SimpleBag<Int32> target = new SimpleBag<Int32> {3, 2, 1, 0, 0, 2, 4};
         private Boolean matchedResult;
         private Int32 originalCount;
         private Boolean unmatchedResult;

         protected override void ArrangeMethod()
         {
            originalCount = target.Count;
         }

         protected override void ActMethod()
         {
            matchedResult = target.Remove(2);
            unmatchedResult = target.Remove(Int32.MaxValue - 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_only_one_matching_value()
         {
            target.Count.Should().Be(originalCount - 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_success_of_removal()
         {
            matchedResult.Should().BeTrue();
            unmatchedResult.Should().BeFalse();
         }
      }
   }
}
