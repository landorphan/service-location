namespace Landorphan.Common.Tests.Decorators
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Decorators;
   using Landorphan.Common.Tests.Decorators.EntityClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class RankDecoratorShallowComparer_Tests
   {
      [TestClass]
      public class When_I_call_RankDecoratorShallowComparer_Compare : TestBase
      {
         private readonly RankDecoratorShallowComparer<NameValueTestClass, Int64> target = new RankDecoratorShallowComparer<NameValueTestClass, Int64>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_evaluate_the_rank_only()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(new NameValueTestClass(), 5)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 5)))
            {
               target.Compare(x, y).Should().Be(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_negative_value_when_x_is_null_and_y_is_not_null()
         {
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 1)))
            {
               target.Compare(null, y).Should().BeLessThan(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_positive_value_when_x_is_not_null_and_y_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 1)))
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
         public void It_should_return_zero_when_x_and_y_have_the_same_rank()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 2)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 2)))
            {
               target.Compare(x, y).Should().Be(0);
            }
         }
      }
   }
}