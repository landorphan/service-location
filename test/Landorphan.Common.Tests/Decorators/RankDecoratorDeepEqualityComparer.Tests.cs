namespace Landorphan.Common.Tests.Decorators
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Common.Decorators;
   using Landorphan.Common.Tests.Decorators.EntityClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class RankDecoratorDeepEqualityComparerTests
   {
      [TestClass]
      public class When_I_call_RankDecoratorDeepEqualityComparer_Equals : TestBase
      {
         private readonly RankDecoratorDeepEqualityComparer<NameValueCloneableEquatableTestClass, Int64> target =
            new RankDecoratorDeepEqualityComparer<NameValueCloneableEquatableTestClass, Int64>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_and_y_have_the_same_rank_and_x_Value_is_not_null_and_y_Value_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass(), 3)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 3)))
            {
               Equals(x, y).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_and_y_have_the_same_rank_and_x_Value_is_null_and_y_Value_is_not_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 3)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass(), 3)))
            {
               Equals(x, y).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_not_null_and_y_is_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 5)))
            {
               // ReSharper disable once ConditionIsAlwaysTrueOrFalse
               Equals(x, null).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_null_and_y_is_not_null()
         {
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 5)))
            {
               // ReSharper disable once ConditionIsAlwaysTrueOrFalse
               Equals(null, y).Should().BeFalse();
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
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator")]
         public void It_should_be_true_when_x_and_y_have_the_same_rank_and_both_values_are_null()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 3)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 3)))
            {
               // ReSharper disable once EqualExpressionComparison
               Equals(x, y).Should().BeTrue();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_evaluate_the_rank_and_value()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass(), 5)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 5)))
            {
               Equals(x, y).Should().BeFalse();
            }

            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 6)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 6)))
            {
               Equals(x, y).Should().BeTrue();
            }

            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 100)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 6)))
            {
               Equals(x, y).Should().BeFalse();
            }
         }
      }

      [TestClass]
      public class When_I_call_RankDecoratorDeepEqualityComparer_GetHashCode : TestBase
      {
         private readonly RankDecoratorDeepEqualityComparer<NameValueCloneableEquatableTestClass, Int64> target =
            new RankDecoratorDeepEqualityComparer<NameValueCloneableEquatableTestClass, Int64>();

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
            using (var item = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(null, 1)))
            {
               target.GetHashCode(item).Should().NotBe(0);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_same_value_for_equal_ranks()
         {
            using (var x = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 2)))
            using (var y = DisposableHelper.SafeCreate(() => new RankDecorator<NameValueCloneableEquatableTestClass, Int64>(new NameValueCloneableEquatableTestClass {Name = "5", Value = 5}, 2)))
            {
               target.GetHashCode(x).Should().Be(target.GetHashCode(y));
            }
         }
      }
   }
}
