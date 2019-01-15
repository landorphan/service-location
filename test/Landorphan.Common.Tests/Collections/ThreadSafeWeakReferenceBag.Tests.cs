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

   public static class ThreadSafeWeakReferenceBag_Tests
   {
      // TODO: serialization test.

      [TestClass]
      public class When_I_create_a_ThreadSafeWeakReferenceBag : DisposableArrangeActAssert
      {
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

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
            target.Should().BeAssignableTo(typeof(IEnumerable<Object>));
            target.GetEnumerator().Should().NotBeNull();
            ((IEnumerable)target).GetEnumerator().Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_thread_safe()
         {
            target.IsThreadSafe.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_a_ReferenceEqualityComparer()
         {
            target.EqualityComparer.Should().BeAssignableTo<ReferenceEqualityComparer<Object>>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_nulls()
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
         public void It_should_not_be_serializable()
         {
            target.GetType().IsSerializable.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Add : DisposableArrangeActAssert
      {
         private readonly List<Object> expected = new List<Object> {new Object(), new Object(), new Object()};
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item);
            }
         }

         [SuppressMessage("SonarLint.CodeSmell", "S1215:GC.Collect should not be called")]
         [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
         protected override void ActMethod()
         {
            GC.Collect();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_expected_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Add_with_a_duplicates : DisposableArrangeActAssert
      {
         private readonly List<Object> expected = new List<Object> {new Object(), new Object(), new Object()};
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ArrangeMethod()
         {
            foreach (var item in expected)
            {
               target.Add(item);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_any_duplicates()
         {
            target.Add(expected.First()).Should().BeFalse();
            target.Add(expected.Last()).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Add_with_a_null : DisposableArrangeActAssert
      {
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

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
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Add_with_unreferenced_objects : DisposableArrangeActAssert
      {
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ArrangeMethod()
         {
            for (var i = 0; i < 5000; ++i)
            {
               target.Add(new Object());
            }
         }

         [SuppressMessage("SonarLint.CodeSmell", "S1215:GC.Collect should not be called")]
         [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
         protected override void ActMethod()
         {
            GC.Collect();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            target.IsEmpty.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_AddRange : DisposableArrangeActAssert
      {
         private readonly List<Object> expected = new List<Object> {new Object(), new Object(), new Object()};
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ActMethod()
         {
            target.AddRange(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_same_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class
         When_I_have_a_ThreadSafeWeakReferenceBag_and_call_AddRange_with_a_collection_containing_a_null : DisposableArrangeActAssert
      {
         private readonly List<Object> expected = new List<Object> {new Object(), new Object(), new Object()};
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ActMethod()
         {
            var toAdd = expected.ToList();
            toAdd.Add(null);
            toAdd.Insert(0, null);
            target.AddRange(toAdd);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_non_null_values()
         {
            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_AddRange_with_a_null_collection : DisposableArrangeActAssert
      {
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => target.AddRange(null);
            throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Clear : DisposableArrangeActAssert
      {
         private readonly List<Object> expected = new List<Object> {new Object(), new Object(), new Object()};
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ActMethod()
         {
            target.AddRange(expected);
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
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Contains : DisposableArrangeActAssert
      {
         private readonly Object contained = new Object();
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();

         protected override void ArrangeMethod()
         {
            target.Add(contained);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_for_values_not_contained()
         {
            var notContained = new Object();

            target.Contains(notContained).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_for_values_contained()
         {
            target.Contains(contained).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_ThreadSafeWeakReferenceBag_and_call_Remove : DisposableArrangeActAssert
      {
         private readonly Object contained = new Object();
         private readonly ThreadSafeWeakReferenceBag<Object> target = new ThreadSafeWeakReferenceBag<Object>();
         private Boolean matchedResult;
         private Int32 originalCount;
         private Boolean unmatchedResult;

         protected override void ArrangeMethod()
         {
            target.Add(contained);
            target.Add(contained);
            target.Add(new Object());
            target.Add(new Object());

            originalCount = target.Count;
         }

         protected override void ActMethod()
         {
            matchedResult = target.Remove(contained);
            unmatchedResult = target.Remove(new Object());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_only_one_matching_value()
         {
            target.Count.Should().Be(originalCount - 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_false_when_referencing_an_uncontained_object()
         {
            unmatchedResult.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_true_when_referencing_a_contained_object()
         {
            matchedResult.Should().BeTrue();
         }
      }
   }
}
