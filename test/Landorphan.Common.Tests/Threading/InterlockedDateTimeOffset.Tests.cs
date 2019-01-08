namespace Landorphan.Common.Tests.Threading
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class InterlockedDateTimeOffset_Tests
   {
      [TestClass]
      public class When_I_create_an_InterlockedDateTimeOffset : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Accept_A_Starting_DateTime()
         {
            var original = DateTimeOffset.UtcNow;
            var target = new InterlockedDateTimeOffset(original.UtcDateTime);
            target.GetValue().Should().Be(original);
            target.Equals(original).Should().BeTrue();
            target.Equals(new InterlockedDateTimeOffset(original.UtcDateTime)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_Expected_Value_By_Default()
         {
            var target = new InterlockedDateTimeOffset();
            target.Equals(new DateTimeOffset(new DateTime(0, DateTimeKind.Utc))).Should().BeTrue();
            target.GetHashCode().Should().Be(new DateTimeOffset(new DateTime(0, DateTimeKind.Utc)).GetHashCode());
         }
      }

      [TestClass]
      public class When_I_have_an_InterlockedDateTimeOffset : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Accept_The_Value_Set()
         {
            var expected = DateTimeOffset.UtcNow;
            var target = new InterlockedDateTimeOffset();
            target.Equals(expected).Should().BeFalse();
            target.SetValue(expected);
            target.Equals(expected).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Comparable_To_Earlier_And_Later_Values()
         {
            var now = DateTimeOffset.UtcNow;
            var before = now.AddDays(-1);
            var after = now.AddDays(1);

            var target = new InterlockedDateTimeOffset(now.UtcDateTime);
            target.GetValue().Should().Be(now);
            target.Should().Be(new InterlockedDateTimeOffset(now.UtcDateTime));
            (target >= before).Should().BeTrue();
            (before <= target).Should().BeTrue();
            (target >= new InterlockedDateTimeOffset(before.UtcDateTime)).Should().BeTrue();
            (new InterlockedDateTimeOffset(before.UtcDateTime) <= target).Should().BeTrue();
            (target > before).Should().BeTrue();
            (before < target).Should().BeTrue();
            (target > new InterlockedDateTimeOffset(before.UtcDateTime)).Should().BeTrue();
            (new InterlockedDateTimeOffset(before.UtcDateTime) < target).Should().BeTrue();
            (target == now).Should().BeTrue();
            (now == target).Should().BeTrue();
            (target == new InterlockedDateTimeOffset(now.UtcDateTime)).Should().BeTrue();
            (new InterlockedDateTimeOffset(now.UtcDateTime) == target).Should().BeTrue();
            (target != after).Should().BeTrue();
            (after != target).Should().BeTrue();
            (target != new InterlockedDateTimeOffset(after.UtcDateTime)).Should().BeTrue();
            (new InterlockedDateTimeOffset(after.UtcDateTime) != target).Should().BeTrue();
            (target <= before).Should().BeFalse();
            (before >= target).Should().BeFalse();
            (target <= new InterlockedDateTimeOffset(before.UtcDateTime)).Should().BeFalse();
            (new InterlockedDateTimeOffset(before.UtcDateTime) >= target).Should().BeFalse();
            (target < before).Should().BeFalse();
            (before > target).Should().BeFalse();
            (target < new InterlockedDateTimeOffset(before.UtcDateTime)).Should().BeFalse();
            (new InterlockedDateTimeOffset(before.UtcDateTime) > target).Should().BeFalse();
            target.As<IComparable<DateTimeOffset>>().Should().BeGreaterThan(before);
            target.As<IComparable<DateTimeOffset>>().Should().BeLessThan(after);
            target.As<IComparable<InterlockedDateTimeOffset>>().Should().BeGreaterThan(new InterlockedDateTimeOffset(before.UtcDateTime));
            target.CompareTo(before).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Evaluate_Equality_With_Value_Semantics()
         {
            var now = DateTimeOffset.UtcNow;
            var target = new InterlockedDateTimeOffset(now.UtcDateTime);
            target.Equals(null).Should().BeFalse();
            target.Equals((Object) new InterlockedDateTimeOffset(now.UtcDateTime)).Should().BeTrue();

            // ReSharper disable once SuspiciousTypeConversion.Global
            target.Equals((Object) now).Should().BeTrue();

            target.Equals(new Object()).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_Explicitly_IComparable()
         {
            var now = DateTimeOffset.UtcNow;

            var target = (IComparable) new InterlockedDateTimeOffset(now.UtcDateTime);
            target.CompareTo(now).Should().Be(0);
            target.CompareTo(new InterlockedDateTimeOffset(now.UtcDateTime)).Should().Be(0);
            target.CompareTo(null).Should().BeGreaterThan(0);

            Action throwingAction = () => target.CompareTo(new Object());
            throwingAction.Should()
               .Throw<ArgumentException>()
               .WithMessage("Object must be of type DateTimeOffset or of type InterlockedDateTimeOffset.\r\nParameter name: obj")
               .And.ParamName.Should()
               .Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_IComparable_of_T()
         {
            var now = DateTimeOffset.UtcNow;
            var target = new InterlockedDateTimeOffset(now.UtcDateTime);

            target.CompareTo(now).Should().Be(0);
            target.CompareTo(new InterlockedDateTimeOffset(now.UtcDateTime)).Should().Be(0);
         }
      }
   }
}