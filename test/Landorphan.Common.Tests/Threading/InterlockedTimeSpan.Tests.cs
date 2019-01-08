namespace Landorphan.Common.Tests.Threading
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class InterlockedTimeSpan_Tests
   {
      [TestClass]
      public class When_I_create_an_InterlockedTimeSpan : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_starting_ticks_value()
         {
            var original = TimeSpan.FromDays(1);
            var target = new InterlockedTimeSpan(original.Ticks);
            target.GetValue().Should().Be(original);
            target.Equals(original).Should().BeTrue();
            target.Equals(new InterlockedTimeSpan(original)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_starting_TimeSpan_value()
         {
            var original = TimeSpan.FromHours(1);
            var target = new InterlockedTimeSpan(original);
            target.GetValue().Should().Be(original);
            target.Equals(original).Should().BeTrue();
            target.Equals(new InterlockedTimeSpan(original)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_default_value()
         {
            var target = new InterlockedTimeSpan();
            target.Equals(TimeSpan.Zero).Should().BeTrue();
            target.Equals(new InterlockedTimeSpan(TimeSpan.Zero)).Should().BeTrue();
            target.GetHashCode().Should().Be(TimeSpan.Zero.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_have_an_InterlockedTimeSpan : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Accept_The_Value_Set()
         {
            var expected = TimeSpan.FromMilliseconds(1);
            var target = new InterlockedTimeSpan();
            target.Equals(expected).Should().BeFalse();
            target.SetValue(expected);
            target.Equals(expected).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Comparable_To_Shorter_And_Longer_Values()
         {
            var shorter = TimeSpan.FromHours(23);
            var mid = TimeSpan.FromHours(24);
            var longer = TimeSpan.FromHours(25);

            var target = new InterlockedTimeSpan(mid);
            target.GetValue().Should().Be(mid);
            target.Should().Be(new InterlockedTimeSpan(mid));
            (target >= shorter).Should().BeTrue();
            (shorter <= target).Should().BeTrue();
            (target >= new InterlockedTimeSpan(shorter)).Should().BeTrue();
            (new InterlockedTimeSpan(shorter) <= target).Should().BeTrue();
            (target > shorter).Should().BeTrue();
            (shorter < target).Should().BeTrue();
            (target > new InterlockedTimeSpan(shorter)).Should().BeTrue();
            (new InterlockedTimeSpan(shorter) < target).Should().BeTrue();
            (target == mid).Should().BeTrue();
            (mid == target).Should().BeTrue();
            (target == new InterlockedTimeSpan(mid)).Should().BeTrue();
            (new InterlockedTimeSpan(mid) == target).Should().BeTrue();
            (target != longer).Should().BeTrue();
            (longer != target).Should().BeTrue();
            (target != new InterlockedTimeSpan(longer)).Should().BeTrue();
            (new InterlockedTimeSpan(longer) != target).Should().BeTrue();
            (target <= shorter).Should().BeFalse();
            (shorter >= target).Should().BeFalse();
            (target <= new InterlockedTimeSpan(shorter)).Should().BeFalse();
            (new InterlockedTimeSpan(shorter) >= target).Should().BeFalse();
            (target < shorter).Should().BeFalse();
            (shorter > target).Should().BeFalse();
            (target < new InterlockedTimeSpan(shorter)).Should().BeFalse();
            (new InterlockedTimeSpan(shorter) > target).Should().BeFalse();
            target.As<IComparable<TimeSpan>>().Should().BeGreaterThan(shorter);
            target.As<IComparable<TimeSpan>>().Should().BeLessThan(longer);
            target.As<IComparable<InterlockedTimeSpan>>().Should().BeGreaterThan(new InterlockedTimeSpan(shorter));
            target.CompareTo(shorter).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Evaluate_Equality_With_Value_Semantics()
         {
            var value = TimeSpan.FromHours(1);
            var target = new InterlockedTimeSpan(value);
            target.Equals(null).Should().BeFalse();
            target.Equals((Object) new InterlockedTimeSpan(value)).Should().BeTrue();

            // ReSharper disable once SuspiciousTypeConversion.Global
            target.Equals((Object) value).Should().BeTrue();

            target.Equals(new Object()).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_Explicitly_IComparable()
         {
            var value = TimeSpan.FromHours(1);

            var target = (IComparable) new InterlockedTimeSpan(value);
            target.CompareTo(value).Should().Be(0);
            target.CompareTo(new InterlockedTimeSpan(value)).Should().Be(0);
            target.CompareTo(null).Should().BeGreaterThan(0);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action throwingAction = () => target.CompareTo(new Object());
            throwingAction.Should()
               .Throw<ArgumentException>()
               .WithMessage("Object must be of type TimeSpan or of type InterlockedTimeSpan.\r\nParameter name: obj")
               .And.ParamName.Should()
               .Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_IComparable_of_T()
         {
            var value = TimeSpan.FromHours(1);
            var target = new InterlockedTimeSpan(value);

            target.CompareTo(value).Should().Be(0);
            target.CompareTo(new InterlockedTimeSpan(value)).Should().Be(0);
         }
      }
   }
}