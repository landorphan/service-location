namespace Landorphan.Common.Tests.Collections.BclBaseline
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using System.Runtime.CompilerServices;
   using FluentAssertions;
   using Landorphan.Common.Collections;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class HashSet_Baseline_Tests
   {
      public static class Issues_To_Address
      {
         [TestClass]
         public class
            Dictionary_Keys_When_I_call_contains_and_the_items_GetHashcode_implementation_does_not_match_the_sets_comparers_GetHashCode_Implementation_Contains_fails
            : ArrangeActAssert
         {
            private EquatableTestItem itemA;
            private EquatableTestItem itemB;
            private Dictionary<EquatableTestItem, String> target;

            protected override void ArrangeMethod()
            {
               target = new Dictionary<EquatableTestItem, String>(new EquatableTestItemEqualityComparer());
               itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
               itemB = new EquatableTestItem {Name = "Bbb", Value = 2};

               target.Add(itemA, "abc");
               target.Add(itemB, "def");

               target.Keys.Count.Should().Be(2);
            }

            protected override void ActMethod()
            {
               itemB.Name = itemA.Name;
               itemB.Value = itemA.Value;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_prove_Contains_is_broken()
            {
               ReferenceEquals(itemA, itemB).Should().BeFalse();
               target.ContainsKey(itemA).Should().BeTrue();
               target.ContainsKey(itemB).Should().BeTrue();
               target.ContainsKey(new EquatableTestItem(itemA)).Should().BeTrue();
               target.ContainsKey(new EquatableTestItem(itemB)).Should().BeTrue();
               target.Keys.Count.Should().Be(2);

               var itemC = new EquatableTestItem(itemA);
               var itemD = new EquatableTestItem(itemB);

               // THE FIX: target.RemoveExistingDuplicates();

               target.Remove(itemA).Should().BeTrue();

               // ALL should be true
               target.ContainsKey(itemA).Should().BeFalse();
               target.ContainsKey(itemB).Should().BeFalse();
               target.ContainsKey(itemC).Should().BeFalse();
               target.ContainsKey(itemD).Should().BeFalse();

               target.Count.Should().Be(1);
            }
         }

         [TestClass]
         public class
            ImmutableHashSet_When_I_call_contains_and_the_items_GetHashcode_implementation_does_not_match_the_sets_comparers_GetHashCode_Implementation_Contains_fails
            : ArrangeActAssert
         {
            private EquatableTestItem itemA;
            private EquatableTestItem itemB;
            private ImmutableHashSet<EquatableTestItem> target;

            protected override void ArrangeMethod()
            {
               var builder = ImmutableHashSet<EquatableTestItem>.Empty.ToBuilder();
               builder.KeyComparer = new EquatableTestItemEqualityComparer();
               itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
               builder.Add(itemA);
               itemB = new EquatableTestItem {Name = "Bbb", Value = 2};
               builder.Add(itemB);

               target = builder.ToImmutable();
               target.Count.Should().Be(2);
            }

            protected override void ActMethod()
            {
               itemB.Name = itemA.Name;
               itemB.Value = itemA.Value;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_prove_Contains_is_broken()
            {
               ReferenceEquals(itemA, itemB).Should().BeFalse();
               target.Contains(itemA).Should().BeTrue();
               target.Contains(itemB).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemA)).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemB)).Should().BeTrue();
               target.Count.Should().Be(2);

               var itemC = new EquatableTestItem(itemA);
               var itemD = new EquatableTestItem(itemB);

               // THE FIX: target.RemoveExistingDuplicates();

               target = target.Remove(itemA);

               // ALL should be true
               target.Contains(itemA).Should().BeFalse();
               target.Contains(itemB).Should().BeFalse();
               target.Contains(itemC).Should().BeFalse();
               target.Contains(itemD).Should().BeFalse();

               target.Count.Should().Be(1);
            }
         }

         [TestClass]
         public class
            SimpleSet_When_I_call_contains_and_the_items_GetHashcode_implementation_does_not_match_the_sets_comparers_GetHashCode_Implementation_Contains_fails
            : ArrangeActAssert
         {
            private EquatableTestItem itemA;
            private EquatableTestItem itemB;
            private SimpleSet<EquatableTestItem> target;

            protected override void ArrangeMethod()
            {
               target = new SimpleSet<EquatableTestItem>(new EquatableTestItemEqualityComparer());
               itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
               itemB = new EquatableTestItem {Name = "Bbb", Value = 2};

               target.Add(itemA);
               target.Add(itemB);

               target.Count.Should().Be(2);
            }

            protected override void ActMethod()
            {
               itemB.Name = itemA.Name;
               itemB.Value = itemA.Value;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_prove_Contains_is_broken()
            {
               ReferenceEquals(itemA, itemB).Should().BeFalse();
               target.Contains(itemA).Should().BeTrue();
               target.Contains(itemB).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemA)).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemB)).Should().BeTrue();
               target.Count.Should().Be(2);

               var itemC = new EquatableTestItem(itemA);
               var itemD = new EquatableTestItem(itemB);

               // THE FIX: target.RemoveExistingDuplicates();

               target.Remove(itemA).Should().BeTrue();

               // ALL should be true
               target.Contains(itemA).Should().BeFalse();
               target.Contains(itemB).Should().BeFalse();
               target.Contains(itemC).Should().BeFalse();
               target.Contains(itemD).Should().BeFalse();

               target.Count.Should().Be(1);
            }
         }

         [TestClass]
         public class
            ThreadSafeSet_When_I_call_contains_and_the_items_GetHashcode_implementation_does_not_match_the_sets_comparers_GetHashCode_Implementation_Contains_fails
            : ArrangeActAssert
         {
            private EquatableTestItem itemA;
            private EquatableTestItem itemB;
            private ThreadSafeSet<EquatableTestItem> target;

            protected override void ArrangeMethod()
            {
               target = new ThreadSafeSet<EquatableTestItem>(new EquatableTestItemEqualityComparer());
               itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
               itemB = new EquatableTestItem {Name = "Bbb", Value = 2};

               target.Add(itemA);
               target.Add(itemB);

               target.Count.Should().Be(2);
            }

            protected override void ActMethod()
            {
               itemB.Name = itemA.Name;
               itemB.Value = itemA.Value;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_prove_Contains_is_broken()
            {
               ReferenceEquals(itemA, itemB).Should().BeFalse();
               target.Contains(itemA).Should().BeTrue();
               target.Contains(itemB).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemA)).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemB)).Should().BeTrue();
               target.Count.Should().Be(2);

               var itemC = new EquatableTestItem(itemA);
               var itemD = new EquatableTestItem(itemB);

               // THE FIX: target.RemoveExistingDuplicates();

               target.Remove(itemA).Should().BeTrue();

               // ALL should be true
               target.Contains(itemA).Should().BeFalse();
               target.Contains(itemB).Should().BeFalse();
               target.Contains(itemC).Should().BeFalse();
               target.Contains(itemD).Should().BeFalse();

               target.Count.Should().Be(1);
            }
         }

         [TestClass]
         public class
            When_I_call_contains_and_the_items_GetHashcode_implementation_does_not_match_the_sets_comparers_GetHashCode_Implementation_Contains_fails
            : ArrangeActAssert
         {
            private EquatableTestItem itemA;
            private EquatableTestItem itemB;
            private HashSet<EquatableTestItem> target;

            protected override void ArrangeMethod()
            {
               target = new HashSet<EquatableTestItem>(new EquatableTestItemEqualityComparer());
               itemA = new EquatableTestItem {Name = "Aaa", Value = 1};
               itemB = new EquatableTestItem {Name = "Bbb", Value = 2};

               target.Add(itemA);
               target.Add(itemB);

               target.Count.Should().Be(2);
            }

            protected override void ActMethod()
            {
               itemB.Name = itemA.Name;
               itemB.Value = itemA.Value;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_prove_Contains_is_broken()
            {
               ReferenceEquals(itemA, itemB).Should().BeFalse();
               target.Contains(itemA).Should().BeTrue();
               target.Contains(itemB).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemA)).Should().BeTrue();
               target.Contains(new EquatableTestItem(itemB)).Should().BeTrue();
               target.Count.Should().Be(2);

               var itemC = new EquatableTestItem(itemA);
               var itemD = new EquatableTestItem(itemB);

               target.Remove(itemA).Should().BeTrue();

               // ALL should be true
               target.Contains(itemA).Should().BeFalse();
               target.Contains(itemB).Should().BeFalse();
               target.Contains(itemC).Should().BeFalse();
               target.Contains(itemD).Should().BeFalse();

               target.Comparer.Equals(itemA, itemB).Should().BeTrue();
               target.Comparer.Equals(itemB, itemC).Should().BeTrue();
               target.Comparer.Equals(itemC, itemD).Should().BeTrue();
               target.Comparer.Equals(itemD, target.First()).Should().BeTrue();
               target.Contains(target.First()).Should().BeFalse();

               target.Count.Should().Be(1);
            }
         }
      }

      [TestClass]
      public class When_I_call_HashSet_Add : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_item_is_not_unique_It_should_not_add_the_item()
         {
            var target = new HashSet<EquatableTestItem>();
            var item = new EquatableTestItem {Name = Guid.NewGuid().ToString(), Value = 5};
            target.Add(item).Should().BeTrue();
            var was = target.Count;

            target.Add(item).Should().BeFalse();
            target.Count.Should().Be(was);

            target.Add(new EquatableTestItem(item)).Should().BeFalse();
            target.Count.Should().Be(was);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_item_is_null_It_should_add_the_null()
         {
            var target = new HashSet<EquatableTestItem>();

            target.Count.Should().Be(0);

            // ReSharper disable once RedundantCast
            target.Add((EquatableTestItem) null);
            target.Count.Should().Be(1);
            target.Contains(null).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_item_is_unique_It_should_add_the_item()
         {
            var target = new HashSet<EquatableTestItem>();
            var item = new EquatableTestItem {Name = Guid.NewGuid().ToString(), Value = 5};
            var was = target.Count;

            target.Add(item).Should().BeTrue();
            target.Contains(item).Should().BeTrue();
            target.Count.Should().Be(was + 1);
         }
      }

      private sealed class EquatableTestItem : IEquatable<EquatableTestItem>
      {
         private String _name = string.Empty;

         public EquatableTestItem()
         {
         }

         public EquatableTestItem(EquatableTestItem other) : this()
         {
            other.ArgumentNotNull(nameof(other));

            Name = other.Name;
            Value = other.Value;
         }

         public String Name
         {
            get => _name;
            set => _name = value.TrimNullToEmpty();
         }

         public Int32 Value { get; set; }

         public Boolean Equals(EquatableTestItem other)
         {
            if (ReferenceEquals(null, other))
            {
               return false;
            }

            return EqualityComparer<String>.Default.Equals(_name, other.Name) && Value == other.Value;
         }

         public override Boolean Equals(Object obj)
         {
            return Equals(obj as EquatableTestItem);
         }

         public override Int32 GetHashCode()
         {
            // TODO: Figure out why this is not reliable for HashSet<T>.Contains
            //unchecked
            //{
            //   return (_name.GetHashCode() * 397) ^ Value;
            //}

            // return EqualityComparer<EquatableTestItem>.DefaultValue.GetHashCode();

            unchecked
            {
               return (Name.TrimNullToEmpty().ToUpperInvariant().GetHashCode() * 397) ^ Value;
            }
         }

         [SuppressMessage(
            "Microsoft.Globalization",
            "CA1305: Specify IFormatProvide",
            Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
         public override String ToString()
         {
            return $"Name: {Name}, Value: {Value}";
         }
      }

      private sealed class EquatableTestItemEqualityComparer : IEqualityComparer<EquatableTestItem>
      {
         public Boolean Equals(EquatableTestItem x, EquatableTestItem y)
         {
            if (x.IsNull())
            {
               if (y.IsNull())
               {
                  return true;
               }

               return false;
            }

            if (y.IsNull())
            {
               return false;
            }

            return x.Value == y.Value &&
                   string.Equals(
                      x.Name.TrimNullToEmpty().ToUpperInvariant(),
                      y.Name.TrimNullToEmpty().ToUpperInvariant(),
                      StringComparison.Ordinal);
         }

         public Int32 GetHashCode(EquatableTestItem obj)
         {
            if (obj.IsNull())
            {
               return 0;
            }

            unchecked
            {
               return (obj.Name.TrimNullToEmpty().ToUpperInvariant().GetHashCode() * 397) ^ obj.Value;
            }
         }
      }
   }
}