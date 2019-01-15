namespace Landorphan.Common.Tests.Decorators
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Decorators;
   using Landorphan.Common.Tests.Decorators.EntityClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class RankDecoratorShallowEqualityComparer_Tests
   {
      [TestClass]
      public class When_I_call_RankDecoratorShallowEqualityComparer_Equals : TestBase
      {
         private readonly RankDecoratorShallowEqualityComparer<NameValueTestClass, Int64> target =
            new RankDecoratorShallowEqualityComparer<NameValueTestClass, Int64>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_not_null_and_y_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 5)))
            {
               target.Equals(x, null).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_null_and_y_is_not_null()
         {
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 5)))
            {
               target.Equals(null, y).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_when_both_are_null()
         {
            target.Equals(null, null).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_evaluate_the_rank_only()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(new NameValueTestClass(), 5)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 5)))
            {
               target.Equals(x, y).Should().BeTrue();
            }
         }
      }

      [TestClass]
      public class When_I_call_RankDecoratorShallowEqualityComparer_GetHashCode : TestBase
      {
         private readonly RankDecoratorShallowEqualityComparer<NameValueTestClass, Int64> target =
            new RankDecoratorShallowEqualityComparer<NameValueTestClass, Int64>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            target.GetHashCode(null).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_values()
         {
            using (var item = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 1)))
            {
               target.GetHashCode(item).Should().NotBe(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_same_value_for_equal_ranks()
         {
            using (var item0 = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(null, 2)))
            using (var item1 = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueTestClass, Int64>(new NameValueTestClass {Name = "5", Value = 5}, 2)))
            {
               target.GetHashCode(item0).Should().Be(target.GetHashCode(item1));
            }
         }
      }
   }
}
