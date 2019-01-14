namespace Landorphan.Common.Tests.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Collections;
   using Landorphan.Common.Tests.Collections.TestClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class SimpleSet_Tests
   {
      [TestClass]
      public class When_I_create_a_SimpleSet : ArrangeActAssert
      {
         private readonly SimpleSet<Int32> target = new SimpleSet<Int32>();

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
      public class When_I_create_a_SimpleSet_from_a_collection : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_collection_contains_null_values_It_should_contain_only_the_distinct_non_null_values()
         {
            var original = new List<Object> {new Object(), null, new Object(), null};
            var expected = (from o in original where o.IsNotNull() select o).ToList();
            var target = new SimpleSet<Object>(original);

            target.Should().Contain(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_collection_is_null_It_should_return_an_empty_set()
         {
            var target = new SimpleSet<Int32>((IEnumerable<Int32>)null);
            target.IsEmpty.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_only_the_distinct_values()
         {
            var original = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
            var target = new SimpleSet<Int32>(original);
            var distinct = (from v in original select v).Distinct().ToList();
            target.Should().Contain(distinct);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_Add : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_only_the_distinct_values()
         {
            var duplicates = new List<Int32> {3, 2, 1, 0, 0, 2, 4};
            var distinct = (from v in duplicates select v).Distinct().ToList();
            var target = new SimpleSet<Int32>();

            foreach (var v in duplicates)
            {
               target.Add(v);
            }

            target.Should().Contain(distinct);
         }

         protected void And_item_is_null_It_should_not_throw()
         {
            var target = new SimpleSet<Object>();

            // ReSharper disable once AssignNullToNotNullAttribute
            var actual = target.Add(null);
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_AddRange : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_source_contains_a_null_It_should_contain_the_distinct_values()
         {
            var obj = new Object();

            var target = new SimpleSet<Object> {obj, new Object()};
            var was = target.Count;

            // a duplicate, a null, and a unique item.
            target.AddRange(new[] {obj, null, new Object()});
            target.Count.Should().Be(was + 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_source_is_null_It_should_not_throw()
         {
            var target = new SimpleSet<Int32> {3, 2, 1, 0, 0, 2, 4};
            var was = target.Count;
            target.AddRange(null);
            target.Count.Should().Be(was);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_contain_the_distinct_values()
         {
            var target = new SimpleSet<Int32> {3, 2, 1, 0, 0, 2, 4};
            var additional = new List<Int32> {4, 5, 6, 7};
            var expected = new List<Int32>();
            expected.AddRange(target);
            expected.AddRange((from v in additional where !target.Contains(v) select v).Distinct());

            target.AddRange(additional);

            target.Should().Contain(expected);
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_Clear : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_empty()
         {
            var target = new SimpleSet<Int32> {3, 2, 1, 0, 4};

            target.Clear();

            target.IsEmpty.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_Contains : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_Item_is_null_it_should_return_false()
         {
            var target = new SimpleSet<Object>(new[] {new Object(), null, new Object()});
            target.Contains(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_for_values_not_contained()
         {
            var target = new SimpleSet<Int32> {3, 2, 1, 0, 4};

            var below = (from value in target select value).Min() - 1;
            var above = (from value in target select value).Max() + 1;

            target.Contains(below).Should().BeFalse();
            target.Contains(above).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_for_values_contained()
         {
            var target = new SimpleSet<Int32> {3, 2, 1, 0, 4};

            var values = (from value in target select value).Distinct().ToList();
            foreach (var v in values)
            {
               target.Contains(v).Should().BeTrue();
            }
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_MakeReadOnly : ArrangeActAssert
      {
         private readonly SimpleSet<Int32> target = new SimpleSet<Int32> {3, 2, 1, 0, 4};

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_calls_to_RemoveExistingDuplicates_and_should_return_false()
         {
            target.RemoveExistingDuplicates().Should().BeFalse();
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
         public void It_should_throw_on_calls_to_Clear()
         {
            Action throwingAction = () => target.Clear();
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_ExceptWith()
         {
            Action throwingAction = () => target.ExceptWith(new[] {1});
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_IntersectWith()
         {
            Action throwingAction = () => target.IntersectWith(new[] {1});
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_Remove()
         {
            var item = target.First();
            Action throwingAction = () => target.Remove(item);
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_SymmetricExceptWith()
         {
            Action throwingAction = () => target.SymmetricExceptWith(new[] {1});
            throwingAction.Should().Throw<NotSupportedException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_calls_to_UnionWith()
         {
            Action throwingAction = () => target.UnionWith(new[] {1});
            throwingAction.Should().Throw<NotSupportedException>();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_call_Remove : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_Item_is_null_It_should_return_false()
         {
            var target = new SimpleSet<Object>();
            target.Add(new Object());
            target.Add(new Object());
            target.Add(new Object());

            var was = target.Count;
            target.Remove(null).Should().BeFalse();
            target.Count.Should().Be(was);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_the_value()
         {
            var expected = new List<Int32> {3, 2, 1, 0};
            const Int32 valueToRemove = 4;

            var target = new SimpleSet<Int32>();
            target.AddRange(expected);
            target.Add(valueToRemove);
            var originalCount = target.Count;

            var actual = target.Remove(valueToRemove);

            actual.Should().BeTrue();
            target.Should().Contain(expected);
            target.Count.Should().Be(originalCount - 1);
            target.Contains(valueToRemove).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_and_perform_set_operations : ArrangeActAssert
      {
         // ReSharper disable AssignNullToNotNullAttribute

         private SimpleSet<Int32> EmptySet;
         private SimpleSet<Int32> Match;
         private SimpleSet<Int32> NoOverlap;

         // ReSharper disable once CollectionNeverUpdated.Local
         private SimpleSet<Object> ReferencesEmpty;
         private SimpleSet<Object> ReferencesMatch;
         private SimpleSet<Object> ReferencesTarget;
         private SimpleSet<Int32> SomeOverlap;
         private SimpleSet<Int32> Subset;
         private SimpleSet<Int32> Superset;
         private SimpleSet<Int32> Target;

         protected override void ArrangeMethod()
         {
            EmptySet = new SimpleSet<Int32>();
            EmptySet.MakeReadOnly();

            Match = new SimpleSet<Int32>(new[] {3, 4, 5});
            Match.MakeReadOnly();

            NoOverlap = new SimpleSet<Int32>(new[] {0, 1, 2});
            NoOverlap.MakeReadOnly();

            SomeOverlap = new SimpleSet<Int32>(new[] {1, 2, 3});
            SomeOverlap.MakeReadOnly();

            Subset = new SimpleSet<Int32>(new[] {3, 5});
            Subset.MakeReadOnly();

            Superset = new SimpleSet<Int32>(new[] {3, 4, 5, 6});
            Superset.MakeReadOnly();

            Target = new SimpleSet<Int32>(new[] {3, 4, 5});

            ReferencesTarget = new SimpleSet<Object>(new[] {new Object(), new Object(), new Object()});
            ReferencesMatch = new SimpleSet<Object>(ReferencesTarget);
            ReferencesEmpty = new SimpleSet<Object>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_ExceptWith_correctly()
         {
            Target.ExceptWith(null);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.ExceptWith(collectionWithOnlyNulls);
            ReferencesTarget.SetEquals(ReferencesMatch).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.ExceptWith(EmptySet);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.ExceptWith(Match);
            Target.SetEquals(EmptySet).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.ExceptWith(collectionWithNulls);
            ReferencesTarget.SetEquals(ReferencesEmpty).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.ExceptWith(NoOverlap);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.ExceptWith(SomeOverlap);
            Target.Count.Should().BeLessThan(Match.Count);
            Target.Clear();
            Target.AddRange(Match);

            Target.ExceptWith(Subset);
            Target.Count.Should().BeLessThan(Match.Count);
            Target.Clear();
            Target.AddRange(Match);

            Target.ExceptWith(Superset);
            Target.SetEquals(EmptySet).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_IntersectWith_correctly()
         {
            Target.IntersectWith(null);
            Target.SetEquals(EmptySet).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.IntersectWith(collectionWithOnlyNulls);
            ReferencesTarget.SetEquals(ReferencesEmpty).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.IntersectWith(EmptySet);
            Target.SetEquals(EmptySet).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.IntersectWith(Match);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.IntersectWith(collectionWithNulls);
            ReferencesTarget.SetEquals(ReferencesMatch).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.IntersectWith(NoOverlap);
            Target.SetEquals(Match).Should().BeFalse();
            Target.Clear();
            Target.AddRange(Match);

            Target.IntersectWith(SomeOverlap);
            Target.Count.Should().BeLessThan(Match.Count);
            Target.Clear();
            Target.AddRange(Match);

            Target.IntersectWith(Subset);
            Target.Count.Should().BeLessThan(Match.Count);
            Target.Clear();
            Target.AddRange(Match);

            Target.IntersectWith(Superset);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_IsProperSubsetOf_correctly()
         {
            Target.IsProperSubsetOf(null).Should().BeFalse();
            Target.IsProperSubsetOf(EmptySet).Should().BeFalse();

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.IsProperSubsetOf(collectionWithOnlyNulls).Should().BeFalse();

            Target.IsProperSubsetOf(Match).Should().BeFalse();

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.IsProperSubsetOf(collectionWithNulls).Should().BeFalse();

            Target.IsProperSubsetOf(NoOverlap).Should().BeFalse();
            Target.IsProperSubsetOf(SomeOverlap).Should().BeFalse();
            Target.IsProperSubsetOf(Subset).Should().BeFalse();
            Target.IsProperSubsetOf(Superset).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_IsProperSupersetOf_correctly()
         {
            Target.IsProperSupersetOf(null).Should().BeTrue();
            Target.IsProperSupersetOf(EmptySet).Should().BeTrue();

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.IsProperSupersetOf(collectionWithOnlyNulls).Should().BeTrue();

            Target.IsProperSupersetOf(Match).Should().BeFalse();

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.IsProperSupersetOf(collectionWithNulls).Should().BeFalse();

            Target.IsProperSupersetOf(NoOverlap).Should().BeFalse();
            Target.IsProperSupersetOf(SomeOverlap).Should().BeFalse();
            Target.IsProperSupersetOf(Subset).Should().BeTrue();
            Target.IsProperSupersetOf(Superset).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_IsSubsetOf_correctly()
         {
            Target.IsSubsetOf(null).Should().BeFalse();
            Target.IsSubsetOf(EmptySet).Should().BeFalse();

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.IsSubsetOf(collectionWithOnlyNulls).Should().BeFalse();

            Target.IsSubsetOf(Match).Should().BeTrue();

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.IsSubsetOf(collectionWithNulls).Should().BeTrue();

            Target.IsSubsetOf(NoOverlap).Should().BeFalse();
            Target.IsSubsetOf(SomeOverlap).Should().BeFalse();
            Target.IsSubsetOf(Subset).Should().BeFalse();
            Target.IsSubsetOf(Superset).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_IsSupersetOf_correctly()
         {
            Target.IsSupersetOf(null).Should().BeTrue();
            Target.IsSupersetOf(EmptySet).Should().BeTrue();

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.IsSupersetOf(collectionWithOnlyNulls).Should().BeTrue();

            Target.IsSupersetOf(Match).Should().BeTrue();

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.IsSupersetOf(collectionWithNulls).Should().BeTrue();

            Target.IsSupersetOf(NoOverlap).Should().BeFalse();
            Target.IsSupersetOf(SomeOverlap).Should().BeFalse();
            Target.IsSupersetOf(Subset).Should().BeTrue();
            Target.IsSupersetOf(Superset).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_Overlaps_correctly()
         {
            Target.Overlaps(null).Should().BeFalse();
            Target.Overlaps(EmptySet).Should().BeFalse();

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.Overlaps(collectionWithOnlyNulls).Should().BeFalse();

            Target.Overlaps(Match).Should().BeTrue();

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.Overlaps(collectionWithNulls).Should().BeTrue();

            Target.Overlaps(NoOverlap).Should().BeFalse();
            Target.Overlaps(SomeOverlap).Should().BeTrue();
            Target.Overlaps(Subset).Should().BeTrue();
            Target.Overlaps(Superset).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_SymmetricExceptWith_correctly()
         {
            Target.SymmetricExceptWith(null);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.SymmetricExceptWith(EmptySet);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.SymmetricExceptWith(collectionWithOnlyNulls);
            ReferencesTarget.SetEquals(ReferencesMatch).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.SymmetricExceptWith(Match);
            Target.SetEquals(EmptySet).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.SymmetricExceptWith(collectionWithNulls);
            ReferencesTarget.SetEquals(ReferencesEmpty);
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            var expected = new HashSet<Int32>(Match);
            expected.SymmetricExceptWith(NoOverlap);
            Target.SymmetricExceptWith(NoOverlap);
            Target.Should().Contain(expected);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            expected = new HashSet<Int32>(Match);
            expected.SymmetricExceptWith(SomeOverlap);
            Target.SymmetricExceptWith(SomeOverlap);
            Target.Should().Contain(expected);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            expected = new HashSet<Int32>(Match);
            expected.SymmetricExceptWith(Subset);
            Target.SymmetricExceptWith(Subset);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            expected = new HashSet<Int32>(Match);
            expected.SymmetricExceptWith(Superset);
            Target.SymmetricExceptWith(Superset);
            Target.Should().Contain(expected);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_perform_UnionWith_correctly()
         {
            Target.UnionWith(null);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.UnionWith(EmptySet);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithOnlyNulls = new List<Object>(new Object[] {null, null, null});
            ReferencesTarget.UnionWith(collectionWithOnlyNulls);
            ReferencesTarget.SetEquals(ReferencesMatch).Should().BeTrue();
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            Target.UnionWith(Match);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            var collectionWithNulls = new List<Object>(new Object[] {null, null, null});
            collectionWithNulls.AddRange(ReferencesMatch);
            collectionWithNulls.AddRange(new Object[] {null, null, null});
            ReferencesTarget.UnionWith(collectionWithNulls);
            ReferencesTarget.SetEquals(ReferencesMatch);
            ReferencesTarget.Clear();
            ReferencesTarget.AddRange(ReferencesMatch);

            var expected = new HashSet<Int32>(Match);
            expected.UnionWith(NoOverlap);
            Target.UnionWith(NoOverlap);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            expected = new HashSet<Int32>(Match);
            expected.UnionWith(SomeOverlap);
            Target.UnionWith(SomeOverlap);
            Target.SetEquals(expected).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.UnionWith(Subset);
            Target.SetEquals(Match).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);

            Target.UnionWith(Superset);
            Target.SetEquals(Superset).Should().BeTrue();
            Target.Clear();
            Target.AddRange(Match);
         }

         // ReSharper restore AssignNullToNotNullAttribute
      }

      [TestClass]
      public class When_I_have_a_SimpleSet_with_mutable_values_and_mutate_them_into_a_collision : ArrangeActAssert
      {
         private EquatableTestItem itemA;
         private EquatableTestItem itemB;
         private SimpleSet<EquatableTestItem> target;

         protected override void ArrangeMethod()
         {
            // create a set of unique items
            target = new SimpleSet<EquatableTestItem>(new EquatableTestItemEqualityComparer());
            itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
            itemB = new EquatableTestItem {Name = "Bbb", Value = 2};
            target.Add(itemA);
            target.Add(itemB);

            // the comparer.GetHashCode() must differ from EquatableTestItem.GetHashCode()
            new EquatableTestItemEqualityComparer().GetHashCode(itemA).Should().NotBe(itemA.GetHashCode());
         }

         protected override void ActMethod()
         {
            // mutate one of the items so it collides with another item.
            itemB.Name = itemA.Name;
            itemB.Value = itemA.Value;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void As_a_side_effect_removing_an_item_corrupts_Contains_should_be_corrupted()
         {
            // this is because of a side-effect in the BCL.
            target.Remove(itemA);

            // All should be true but are false.
            target.Contains(itemA).Should().BeFalse();
            target.Contains(itemB).Should().BeFalse();
            target.Contains(target.First()).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void CheckForExistingDuplicates_Should_Be_True()
         {
            target.CheckForExistingDuplicates().Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void RemoveExistingDuplicates_Should_fix_the_issue()
         {
            target.RemoveExistingDuplicates();
            target.CheckForExistingDuplicates().Should().BeFalse();
         }
      }
   }
}
