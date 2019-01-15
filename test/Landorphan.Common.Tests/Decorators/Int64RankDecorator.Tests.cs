namespace Landorphan.Common.Tests.Decorators
{
   using System;
   using FluentAssertions;
   using Landorphan.Common.Decorators;
   using Landorphan.Common.Tests.Decorators.EntityClasses;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class Int64RankDecorator_Tests
   {
      [TestClass]
      public class When_I_call_Int64RankDecorator_Clone : TestBase
      {
         private const Int64 ExpectedRank = -2;
         private const Int32 ExpectedValue = -5;
         private readonly String ExpectedName = Guid.NewGuid().ToString();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_decorated_class_is_ICloneable_and_IEquatable_It_should_create_an_equivalent_clone_and_set_IsReadOnly_to_false()
         {
            var decorated = new NameValueCloneableEquatableTestClass {Name = ExpectedName, Value = ExpectedValue};
            using (var original = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decorated, ExpectedRank)))
            {
               original.MakeReadOnly();
               // ReSharper disable once AccessToDisposedClosure
               using (var cloned = DisposableHelper.SafeCreate(() => (Int64RankDecorator<NameValueCloneableEquatableTestClass>)original.Clone()))
               {
                  cloned.Rank.Should().Be(ExpectedRank);
                  cloned.Value.Name.Should().Be(ExpectedName);
                  cloned.Value.Value.Should().Be(ExpectedValue);
                  cloned.IsReadOnly.Should().BeFalse();

                  cloned.Equals(original).Should().BeTrue();

                  // this is contingent on the implementation of IEquatable<T> in the decorated ICloneable class, I can't think of a situation off the type
                  // of my head when I would use ICloneable without implementing IEquatable<T>.
                  original.Equals(cloned).Should().BeTrue();
                  ReferenceEquals(cloned, original).Should().BeFalse();
                  ReferenceEquals(cloned.Value, original.Value).Should().BeFalse();
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_decorated_class_is_not_cloneable_It_should_create_an_equivalent_clone_and_set_IsReadOnly_to_false()
         {
            var decorated = new NameValueTestClass {Name = ExpectedName, Value = ExpectedValue};
            using (var original = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decorated, ExpectedRank)))
            {
               original.MakeReadOnly();
               // ReSharper disable once AccessToDisposedClosure
               using (var cloned = DisposableHelper.SafeCreate(() => (Int64RankDecorator<NameValueTestClass>)original.Clone()))
               {
                  cloned.Rank.Should().Be(ExpectedRank);
                  cloned.Value.Name.Should().Be(ExpectedName);
                  cloned.Value.Value.Should().Be(ExpectedValue);
                  cloned.IsReadOnly.Should().BeFalse();

                  cloned.Equals(original).Should().BeTrue();
                  original.Equals(cloned).Should().BeTrue();
                  ReferenceEquals(cloned, original).Should().BeFalse();

                  ReferenceEquals(cloned.Value, original.Value).Should().BeTrue();
               }
            }
         }
      }

      [TestClass]
      public class When_I_call_Int64RankDecorator_MakeReadOnly : DisposableArrangeActAssert
      {
         private Int64RankDecorator<NameValueMakeReadOnlyTestClass> targetDecoratedMakeReadOnly;
         private Int64RankDecorator<NameValueTestClass> targetDecoratedNotMakeReadOnly;

         protected override void ArrangeMethod()
         {
            var notMakeReadOnlyItem = new NameValueTestClass {Name = Guid.NewGuid().ToString(), Value = 1};
            targetDecoratedNotMakeReadOnly = new Int64RankDecorator<NameValueTestClass>(notMakeReadOnlyItem, 2);

            var makeReadOnlyItem = new NameValueMakeReadOnlyTestClass {Name = Guid.NewGuid().ToString(), Value = 3};
            targetDecoratedMakeReadOnly = new Int64RankDecorator<NameValueMakeReadOnlyTestClass>(makeReadOnlyItem, 4);
         }

         protected override void ActMethod()
         {
            targetDecoratedNotMakeReadOnly.MakeReadOnly();
            targetDecoratedMakeReadOnly.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_call_clone_It_should_create_an_equivalent_value_that_is_not_read_only()
         {
            var actualNotMakeReadOnly = (Int64RankDecorator<NameValueTestClass>)targetDecoratedNotMakeReadOnly.Clone();
            actualNotMakeReadOnly.Equals(targetDecoratedNotMakeReadOnly).Should().BeTrue();
            actualNotMakeReadOnly.GetHashCode().Should().Be(targetDecoratedNotMakeReadOnly.GetHashCode());
            actualNotMakeReadOnly.IsReadOnly.Should().BeFalse();

            var actualMakeReadOnly = (Int64RankDecorator<NameValueMakeReadOnlyTestClass>)targetDecoratedMakeReadOnly.Clone();
            actualMakeReadOnly.Equals(targetDecoratedMakeReadOnly).Should().BeTrue();
            actualMakeReadOnly.GetHashCode().Should().Be(targetDecoratedMakeReadOnly.GetHashCode());
            actualMakeReadOnly.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_read_only()
         {
            targetDecoratedNotMakeReadOnly.IsReadOnly.Should().BeTrue();
            targetDecoratedMakeReadOnly.IsReadOnly.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_make_IConvertsToReadOnly_decorated_classes_read_only()
         {
            targetDecoratedMakeReadOnly.Value.IsReadOnly.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_set_rank()
         {
            Action throwingAction = () => targetDecoratedNotMakeReadOnly.Rank = Int64.MaxValue;
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.And.Message.Should().Contain("current instance is read-only");

            throwingAction = () => targetDecoratedMakeReadOnly.Rank = Int64.MaxValue;
            e = throwingAction.Should().Throw<NotSupportedException>();
            e.And.Message.Should().Contain("current instance is read-only");
         }
      }

      [TestClass]
      public class When_I_construct_a_Int64RankDecorator_using_the_clone_constructor : TestBase
      {
         private const Int64 ExpectedRank = -2;
         private const Int32 ExpectedValue = -5;
         private readonly String ExpectedName = Guid.NewGuid().ToString();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_decorated_class_is_ICloneable_and_IEquatable_It_should_create_an_equivalent_clone_and_set_IsReadOnly_to_false()
         {
            var decorated = new NameValueCloneableEquatableTestClass {Name = ExpectedName, Value = ExpectedValue};
            using (var original = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decorated, ExpectedRank)))
            {
               original.MakeReadOnly();
               // ReSharper disable once AccessToDisposedClosure
               using (var cloned = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(original)))
               {
                  cloned.Rank.Should().Be(ExpectedRank);
                  cloned.Value.Name.Should().Be(ExpectedName);
                  cloned.Value.Value.Should().Be(ExpectedValue);
                  cloned.IsReadOnly.Should().BeFalse();

                  cloned.Equals(original).Should().BeTrue();

                  // this is contingent on the implementation of IEquatable<T> in the decorated ICloneable class, I can't think of a situation off the type
                  // of my head when I would use ICloneable without implementing IEquatable<T>.
                  original.Equals(cloned).Should().BeTrue();
                  ReferenceEquals(cloned, original).Should().BeFalse();
                  ReferenceEquals(cloned.Value, original.Value).Should().BeFalse();
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_decorated_class_is_not_cloneable_It_should_create_an_equivalent_clone_and_set_IsReadOnly_to_false()
         {
            var decorated = new NameValueTestClass {Name = ExpectedName, Value = ExpectedValue};
            using (var original = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decorated, ExpectedRank)))
            {
               original.MakeReadOnly();
               // ReSharper disable once AccessToDisposedClosure
               using (var cloned = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(original)))
               {
                  cloned.Rank.Should().Be(ExpectedRank);
                  cloned.Value.Name.Should().Be(ExpectedName);
                  cloned.Value.Value.Should().Be(ExpectedValue);
                  cloned.IsReadOnly.Should().BeFalse();

                  cloned.Equals(original).Should().BeTrue();
                  original.Equals(cloned).Should().BeTrue();
                  ReferenceEquals(cloned, original).Should().BeFalse();

                  ReferenceEquals(cloned.Value, original.Value).Should().BeTrue();
               }
            }
         }
      }

      [TestClass]
      public class When_I_construct_a_Int64RankDecorator_using_the_rank_value_constructor : DisposableArrangeActAssert
      {
         private const Int64 ExpectedRank = -2;
         private const Int32 ExpectedValue = -5;
         private readonly String ExpectedName = Guid.NewGuid().ToString();
         private NameValueTestClass decorated;
         private Int64RankDecorator<NameValueTestClass> target;

         protected override void ArrangeMethod()
         {
            decorated = new NameValueTestClass {Name = ExpectedName, Value = ExpectedValue};
            target = new Int64RankDecorator<NameValueTestClass>(decorated, ExpectedRank);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_thread_safe()
         {
            target.IsThreadSafe.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_decorate_the_given_instance()
         {
            ReferenceEquals(target.Value, decorated).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_rank()
         {
            target.Rank.Should().Be(ExpectedRank);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_read_only()
         {
            target.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_modify_the_decorated_instance()
         {
            target.Value.Name.Should().Be(ExpectedName);
            target.Value.Value.Should().Be(ExpectedValue);
         }
      }

      [TestClass]
      public class When_I_construct_a_Int64RankDecorator_using_the_value_constructor : DisposableArrangeActAssert
      {
         private const Int32 ExpectedValue = -3;
         private readonly String ExpectedName = Guid.NewGuid().ToString();
         private NameValueTestClass decorated;
         private Int64RankDecorator<NameValueTestClass> target;

         protected override void ArrangeMethod()
         {
            decorated = new NameValueTestClass {Name = ExpectedName, Value = ExpectedValue};
            target = new Int64RankDecorator<NameValueTestClass>(decorated);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_thread_safe()
         {
            target.IsThreadSafe.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_decorate_the_given_instance()
         {
            ReferenceEquals(target.Value, decorated).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_rank_of_zero()
         {
            target.Rank.Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_read_only()
         {
            target.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_modify_the_decorated_instance()
         {
            target.Value.Name.Should().Be(ExpectedName);
            target.Value.Value.Should().Be(ExpectedValue);
         }
      }

      [TestClass]
      public class When_I_evaluate_Int64RankDecorator_equality : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_reference_semantics_on_the_decorated_instance_when_the_decorated_class_implements_IEquatable()
         {
            var SameName = Guid.NewGuid().ToString();
            var DifferentName = Guid.NewGuid().ToString();
            const Int32 SameValue = 2;
            const Int32 DifferentValue = 3;
            const Int64 SameRank = 4;
            const Int64 DifferentRank = 5;

            var decoratedSameValue0 = new NameValueTestClass {Name = SameName, Value = SameValue};
            var decoratedSameValue1 = new NameValueTestClass {Name = SameName, Value = SameValue};
            var decoratedDifferentValue = new NameValueTestClass {Name = DifferentName, Value = DifferentValue};

            using (var targetSameRankSameValue0 = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedSameValue0, SameRank)))
            using (var targetSameRankSameReferenceValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedSameValue0, SameRank)))
            using (var targetSameRankEquivalentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedSameValue1, SameRank)))
            using (var targetSameRankDifferentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedDifferentValue, SameRank)))
            using (var targetSameRankNullValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(null, SameRank)))
            using (var targetDifferentRankSameValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedSameValue0, DifferentRank)))
            using (var targetDifferentRankDifferentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueTestClass>(decoratedDifferentValue, DifferentRank)))
            {
               targetSameRankSameValue0.Equals(targetSameRankSameReferenceValue).Should().BeTrue();
               targetSameRankSameValue0.GetHashCode().Should().Be(targetSameRankSameReferenceValue.GetHashCode());

               targetSameRankSameValue0.Equals(targetSameRankNullValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetSameRankEquivalentValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetSameRankDifferentValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetDifferentRankSameValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetDifferentRankDifferentValue).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_value_semantics_on_the_decorated_instance_when_the_decorated_class_implements_IEquatable()
         {
            var SameName = Guid.NewGuid().ToString();
            var DifferentName = Guid.NewGuid().ToString();
            const Int32 SameValue = 2;
            const Int32 DifferentValue = 3;
            const Int64 SameRank = 4;
            const Int64 DifferentRank = 5;

            var decoratedSameValue0 = new NameValueCloneableEquatableTestClass {Name = SameName, Value = SameValue};
            var decoratedSameValue1 = new NameValueCloneableEquatableTestClass {Name = SameName, Value = SameValue};
            var decoratedDifferentValue = new NameValueCloneableEquatableTestClass {Name = DifferentName, Value = DifferentValue};

            using (var targetSameRankSameValue0 = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedSameValue0, SameRank)))
            using (var targetSameRankSameReferenceValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedSameValue0, SameRank)))
            using (var targetSameRankEquivalentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedSameValue1, SameRank)))
            using (var targetSameRankDifferentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedDifferentValue, SameRank)))
            using (var targetSameRankNullValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(null, SameRank)))
            using (var targetDifferentRankSameValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedSameValue0, DifferentRank)))
            using (var targetDifferentRankDifferentValue = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decoratedDifferentValue, DifferentRank)))
            {
               targetSameRankSameValue0.Equals(targetSameRankSameReferenceValue).Should().BeTrue();
               targetSameRankSameValue0.GetHashCode().Should().Be(targetSameRankSameReferenceValue.GetHashCode());

               targetSameRankSameValue0.Equals(targetSameRankEquivalentValue).Should().BeTrue();
               targetSameRankSameValue0.GetHashCode().Should().Be(targetSameRankEquivalentValue.GetHashCode());

               // handling of null values
               // ReSharper disable once AccessToDisposedClosure
               using (var y = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(targetSameRankNullValue)))
               {
                  targetSameRankNullValue.Equals(y).Should().BeTrue();
                  targetSameRankNullValue.GetHashCode().Should().Be(y.GetHashCode());
               }

               targetSameRankSameValue0.Equals(targetSameRankNullValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetSameRankDifferentValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetDifferentRankSameValue).Should().BeFalse();
               targetSameRankSameValue0.Equals(targetDifferentRankDifferentValue).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            var decorated = new NameValueCloneableEquatableTestClass {Name = Guid.NewGuid().ToString(), Value = 4};
            using (var target = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decorated, 2)))
            {
               target.Equals(null).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_objects()
         {
            var decorated = new NameValueCloneableEquatableTestClass {Name = Guid.NewGuid().ToString(), Value = 4};
            using (var target = DisposableHelper.SafeCreate(() => new Int64RankDecorator<NameValueCloneableEquatableTestClass>(decorated, 2)))
            {
               target.Equals(new Object()).Should().BeFalse();
               target.Equals(target.Clone()).Should().BeTrue();
            }
         }
      }
   }
}
