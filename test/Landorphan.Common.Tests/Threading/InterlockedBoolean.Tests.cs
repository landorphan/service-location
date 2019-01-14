namespace Landorphan.Common.Tests.Threading
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class InterlockedBoolean_Tests
   {
      [TestClass]
      public class When_I_create_an_InterlockedBoolean : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_False_By_Default()
         {
            var target = new InterlockedBoolean();
            target.GetValue().Should().BeFalse();
            target.Equals(false).Should().BeTrue();
            target.Equals(new InterlockedBoolean(false)).Should().BeTrue();
            target.GetHashCode().Should().Be(false.GetHashCode());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_False_Upon_Demand()
         {
            var target = new InterlockedBoolean(false);
            target.GetValue().Should().BeFalse();
            target.Equals(false).Should().BeTrue();
            target.Equals(new InterlockedBoolean(false)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_True_Upon_Demand()
         {
            var target = new InterlockedBoolean(true);
            target.GetValue().Should().BeTrue();
            target.Equals(true).Should().BeTrue();
            target.Equals(new InterlockedBoolean(true)).Should().BeTrue();
            target.GetHashCode().Should().Be(true.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_have_an_InterlockedBoolean : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Accept_The_Value_Set()
         {
            var target = new InterlockedBoolean(true);
            target.SetValue(true);
            target.GetValue().Should().BeTrue();
            target.SetValue(false);
            target.GetValue().Should().BeFalse();
         }

         [SuppressMessage("SonarLint.CodeSmell", "S1125:Boolean literals should not be redundant", Justification = "Test method (MWP)")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Greater_Than_False_When_True()
         {
            var target = new InterlockedBoolean(true);
            target.GetValue().Should().BeTrue();
            target.Should().Be(new InterlockedBoolean(true));
            (target >= false).Should().BeTrue();
            (false <= target).Should().BeTrue();
            (target >= new InterlockedBoolean(false)).Should().BeTrue();
            (new InterlockedBoolean(false) <= target).Should().BeTrue();
            (target > false).Should().BeTrue();
            (false < target).Should().BeTrue();
            (target > new InterlockedBoolean(false)).Should().BeTrue();
            (new InterlockedBoolean(false) < target).Should().BeTrue();
            (target == true).Should().BeTrue();
            (true == target).Should().BeTrue();
            (target == new InterlockedBoolean(true)).Should().BeTrue();
            (new InterlockedBoolean(true) == target).Should().BeTrue();
            (target != false).Should().BeTrue();
            (false != target).Should().BeTrue();
            (target != new InterlockedBoolean(false)).Should().BeTrue();
            (new InterlockedBoolean(false) != target).Should().BeTrue();
            (target <= false).Should().BeFalse();
            (false >= target).Should().BeFalse();
            (target <= new InterlockedBoolean(false)).Should().BeFalse();
            (new InterlockedBoolean(false) >= target).Should().BeFalse();
            (target < false).Should().BeFalse();
            (false > target).Should().BeFalse();
            (target < new InterlockedBoolean(false)).Should().BeFalse();
            (new InterlockedBoolean(false) > target).Should().BeFalse();
            target.As<IComparable<Boolean>>().Should().BeGreaterThan(false);
            target.As<IComparable<InterlockedBoolean>>().Should().BeGreaterThan(new InterlockedBoolean(false));
            target.CompareTo(false).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Be_Less_Than_True_When_False()
         {
            var target = new InterlockedBoolean(false);
            target.GetValue().Should().BeFalse();
            target.Should().Be(new InterlockedBoolean(false));
            target.As<IComparable<Boolean>>().Should().BeLessThan(true);
            target.As<IComparable<InterlockedBoolean>>().Should().BeLessThan(new InterlockedBoolean(true));
            target.As<IComparable>().CompareTo(true).Should().BeLessThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Evaluate_Equality_With_Value_Semantics()
         {
            var target = new InterlockedBoolean(true);
            target.Equals(null).Should().BeFalse();
            target.Equals((Object)new InterlockedBoolean(true)).Should().BeTrue();

            // ReSharper disable once SuspiciousTypeConversion.Global
            target.Equals((Object)true).Should().BeTrue();
            target.Equals(new Object()).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_Explicitly_IComparable()
         {
            var target = (IComparable)new InterlockedBoolean(true);
            target.CompareTo(true).Should().Be(0);
            target.CompareTo(new InterlockedBoolean(true)).Should().Be(0);
            target.CompareTo(null).Should().BeGreaterThan(0);

            Action throwingAction = () => target.CompareTo(new Object());
            throwingAction.Should()
               .Throw<ArgumentException>()
               .WithMessage("Object must be of type Boolean or of type InterlockedBoolean.\r\nParameter name: obj")
               .And.ParamName.Should()
               .Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Implement_IComparable_of_T()
         {
            var target = new InterlockedBoolean(true);

            target.CompareTo(true).Should().Be(0);
            target.CompareTo(new InterlockedBoolean(true)).Should().Be(0);
         }
      }
   }
}
