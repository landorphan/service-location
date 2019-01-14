namespace Landorphan.Common.Tests.Decorators
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Decorators;
   using Landorphan.Common.Tests.Decorators.EntityClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class RankDecoratorDeepComparer_Tests
   {
      [TestClass]
      public class When_I_call_RankDecoratorDeepComparer_Compare : DisposableArrangeActAssert
      {
         private readonly RankDecoratorDeepComparer<NameValueComparableTestClass, Int64> target =
            new RankDecoratorDeepComparer<NameValueComparableTestClass, Int64>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_evaluate_the_rank_and_items()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(new NameValueComparableTestClass(), 5)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(new NameValueComparableTestClass(), 5)))
            {
               target.Compare(x, y).Should().Be(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_negative_value_when_x_and_y_have_the_same_rank_and_x_Value_is_null_and_y_Value_is_not_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 3)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(new NameValueComparableTestClass(), 3)))
            {
               target.Compare(x, y).Should().BeLessThan(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_negative_value_when_x_is_null_and_y_is_not_null()
         {
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 1)))
            {
               target.Compare(null, y).Should().BeLessThan(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_positive_value_when_x_and_y_have_the_same_rank_and_x_Value_is_not_null_and_y_Value_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(new NameValueComparableTestClass(), 4)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 4)))
            {
               target.Compare(x, y).Should().BeGreaterThan(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_positive_value_when_x_is_not_null_and_y_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 1)))
            {
               target.Compare(x, null).Should().BeGreaterThan(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_zero_when_both_comparands_are_null()
         {
            target.Compare(null, null).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_zero_when_x_and_y_have_the_same_rank_and_both_values_are_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 2)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueComparableTestClass, Int64>(null, 2)))
            {
               target.Compare(x, y).Should().Be(0);
            }
         }
      }
   }
}
