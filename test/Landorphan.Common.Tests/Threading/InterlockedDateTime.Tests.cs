namespace Landorphan.Common.Tests.Threading
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class InterlockedDateTime_Tests
   {
      [TestClass]
      public class When_I_create_an_InterlockedDateTime : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Accept_A_Starting_DateTime()
         {
            var original = DateTime.UtcNow;
            var target = new InterlockedDateTime(original);
            target.GetValue().Should().Be(original);
            target.Equals(original).Should().BeTrue();
            target.Equals(new InterlockedDateTime(original)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Accept_A_Starting_Tick_Count()
         {
            var original = DateTime.UtcNow;
            var target = new InterlockedDateTime(original.Ticks);
            target.GetValue().Should().Be(original);
            target.Equals(original).Should().BeTrue();
            target.Equals(new InterlockedDateTime(original)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_Expected_Value_By_Default()
         {
            var target = new InterlockedDateTime();
            target.Equals(new DateTime(0, DateTimeKind.Utc)).Should().BeTrue();
            target.Equals(new InterlockedDateTime(0)).Should().BeTrue();
            target.GetHashCode().Should().Be(new DateTime(0, DateTimeKind.Utc).GetHashCode());
         }
      }

      [TestClass]
      public class When_I_have_an_InterlockedDateTime : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Accept_The_Value_Set()
         {
            var expected = DateTime.UtcNow;
            var target = new InterlockedDateTime();
            target.Equals(expected).Should().BeFalse();
            target.SetValue(expected);
            target.Equals(expected).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Comparable_To_Earlier_And_Later_Values()
         {
            var now = DateTime.UtcNow;
            var before = now.AddDays(-1);
            var after = now.AddDays(1);

            var target = new InterlockedDateTime(now);
            target.GetValue().Should().Be(now);
            target.Should().Be(new InterlockedDateTime(now));
            (target >= before).Should().BeTrue();
            (before <= target).Should().BeTrue();
            (target >= new InterlockedDateTime(before)).Should().BeTrue();
            (new InterlockedDateTime(before) <= target).Should().BeTrue();
            (target > before).Should().BeTrue();
            (before < target).Should().BeTrue();
            (target > new InterlockedDateTime(before)).Should().BeTrue();
            (new InterlockedDateTime(before) < target).Should().BeTrue();
            (target == now).Should().BeTrue();
            (now == target).Should().BeTrue();
            (target == new InterlockedDateTime(now)).Should().BeTrue();
            (new InterlockedDateTime(now) == target).Should().BeTrue();
            (target != after).Should().BeTrue();
            (after != target).Should().BeTrue();
            (target != new InterlockedDateTime(after)).Should().BeTrue();
            (new InterlockedDateTime(after) != target).Should().BeTrue();
            (target <= before).Should().BeFalse();
            (before >= target).Should().BeFalse();
            (target <= new InterlockedDateTime(before)).Should().BeFalse();
            (new InterlockedDateTime(before) >= target).Should().BeFalse();
            (target < before).Should().BeFalse();
            (before > target).Should().BeFalse();
            (target < new InterlockedDateTime(before)).Should().BeFalse();
            (new InterlockedDateTime(before) > target).Should().BeFalse();
            target.As<IComparable<DateTime>>().Should().BeGreaterThan(before);
            target.As<IComparable<DateTime>>().Should().BeLessThan(after);
            target.As<IComparable<InterlockedDateTime>>().Should().BeGreaterThan(new InterlockedDateTime(before));
            target.CompareTo(before).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Evaluate_Equality_With_Value_Semantics()
         {
            var now = DateTime.UtcNow;
            var target = new InterlockedDateTime(now);
            target.Equals(null).Should().BeFalse();
            target.Equals((Object)new InterlockedDateTime(now)).Should().BeTrue();

            // ReSharper disable once SuspiciousTypeConversion.Global
            target.Equals((Object)now).Should().BeTrue();

            target.Equals(new Object()).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_Explicitly_IComparable()
         {
            var now = DateTime.UtcNow;

            var target = (IComparable)new InterlockedDateTime(now);
            target.CompareTo(now).Should().Be(0);
            target.CompareTo(new InterlockedDateTime(now)).Should().Be(0);
            target.CompareTo(null).Should().BeGreaterThan(0);

            Action throwingAction = () => target.CompareTo(new Object());
            throwingAction.Should()
               .Throw<ArgumentException>()
               .WithMessage("Object must be of type DateTime or of type InterlockedDateTime.\r\nParameter name: obj")
               .And.ParamName.Should()
               .Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_IComparable_of_T()
         {
            var now = DateTime.UtcNow;
            var target = new InterlockedDateTime(now);

            target.CompareTo(now).Should().Be(0);
            target.CompareTo(new InterlockedDateTime(now)).Should().Be(0);
         }
      }
   }
}
