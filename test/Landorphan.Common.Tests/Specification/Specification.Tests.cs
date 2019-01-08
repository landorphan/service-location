namespace Landorphan.Common.Tests.Specification
{
   using System;
   using System.Linq.Expressions;
   using FluentAssertions;
   using Landorphan.Common.Specification;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class Specification_Tests
   {
      [TestClass]
      public class When_I_use_AndSpecification : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_both_left_and_right_are_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 1};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.And(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_left_is_not_satisfied_and_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.And(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_left_is_satisfied_and_right_is_not_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.And(posSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_neither_left_nor_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var negSpec = new ValueIsNegativeSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            negSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posSpec.And(negSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_the_correct_type()
         {
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            var composite = new AndSpecification<Entity>(posSpec, posOrZeroSpec);
            composite.GetEntityType().Should().Be(typeof(Entity));
         }
      }

      [TestClass]
      public class When_I_use_DelegateSpecification : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_able_to_construct_it_with_a_Func_predicate()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 1};

            // satisfied when value is positive
            // ReSharper disable once ConvertToLocalFunction
            Func<Entity, Boolean> predicate = e => e.Value > 0;
            var target = new DelegateSpecification<Entity>(predicate);
            target.IsSatisfiedBy(entity).Should().BeTrue();

            target.GetEntityType().Should().Be(typeof(Entity));

            target.Delegate.Should().BeSameAs(predicate);
            target.Delegate.Should().Be(predicate);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_able_to_construct_it_with_an_Expression_predicate()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 1};

            Expression<Func<Entity, Boolean>> predicate = e => e.Value > 0;
            var target = new DelegateSpecification<Entity>(predicate);
            target.IsSatisfiedBy(entity).Should().BeTrue();

            target.GetEntityType().Should().Be(typeof(Entity));

            target.Delegate.Should().NotBe(predicate);
         }
      }

      [TestClass]
      public class When_I_use_NotSpecification : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_negate_the_specification()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posSpec.Not().IsSatisfiedBy(entity).Should().BeTrue();

            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.Not().IsSatisfiedBy(entity).Should().BeFalse();

            TestHelp.DoNothing(entity.Name);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_the_correct_type()
         {
            var posSpec = new ValueIsPositiveSpecification();

            var composite = new NotSpecification<Entity>(posSpec);
            composite.GetEntityType().Should().Be(typeof(Entity));
         }
      }

      [TestClass]
      public class When_I_use_OrSpecification : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_both_left_and_right_are_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 1};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.Or(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_left_is_not_satisfied_and_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.Or(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_left_is_satisfied_and_right_is_not_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.Or(posSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_neither_left_nor_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var negSpec = new ValueIsNegativeSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            negSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posSpec.Or(negSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_the_correct_type()
         {
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            var composite = new OrSpecification<Entity>(posSpec, posOrZeroSpec);
            composite.GetEntityType().Should().Be(typeof(Entity));
         }
      }

      [TestClass]
      public class When_I_use_XorSpecification : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_left_is_not_satisfied_and_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.Xor(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_satisfied_when_left_is_satisfied_and_right_is_not_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.Xor(posSpec).IsSatisfiedBy(entity).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_both_left_and_right_are_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 1};
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posOrZeroSpec.IsSatisfiedBy(entity).Should().BeTrue();
            posSpec.Xor(posOrZeroSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_satisfied_when_neither_left_nor_right_is_satisfied()
         {
            var entity = new Entity {Name = Guid.NewGuid().ToString(), Value = 0};
            var posSpec = new ValueIsPositiveSpecification();
            var negSpec = new ValueIsNegativeSpecification();

            posSpec.IsSatisfiedBy(entity).Should().BeFalse();
            negSpec.IsSatisfiedBy(entity).Should().BeFalse();
            posSpec.Xor(negSpec).IsSatisfiedBy(entity).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_report_the_correct_type()
         {
            var posSpec = new ValueIsPositiveSpecification();
            var posOrZeroSpec = new ValueIsPositiveOrZeroSpecification();

            var composite = new XorSpecification<Entity>(posSpec, posOrZeroSpec);
            composite.GetEntityType().Should().Be(typeof(Entity));
         }
      }

      private class Entity
      {
         private String _name = string.Empty;

         internal String Name
         {
            get => _name;
            set => _name = value ?? string.Empty;
         }

         internal Int32 Value { get; set; } = -1;
      }

      private class ValueIsNegativeSpecification : ISpecification<Entity>
      {
         /// <inheritdoc/>
         public Type GetEntityType()
         {
            return typeof(Entity);
         }

         public Boolean IsSatisfiedBy(Entity entity)
         {
            var rv = false;
            if (entity != null)
            {
               rv = entity.Value < 0;
            }

            return rv;
         }
      }

      private class ValueIsPositiveOrZeroSpecification : ISpecification<Entity>
      {
         /// <inheritdoc/>
         public Type GetEntityType()
         {
            return typeof(Entity);
         }

         public Boolean IsSatisfiedBy(Entity entity)
         {
            var rv = false;
            if (entity != null)
            {
               rv = entity.Value >= 0;
            }

            return rv;
         }
      }

      private class ValueIsPositiveSpecification : ISpecification<Entity>
      {
         /// <inheritdoc/>
         public Type GetEntityType()
         {
            return typeof(Entity);
         }

         public Boolean IsSatisfiedBy(Entity entity)
         {
            var rv = false;
            if (entity != null)
            {
               rv = entity.Value > 0;
            }

            return rv;
         }
      }
   }
}