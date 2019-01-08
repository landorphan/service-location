namespace Landorphan.Common.Tests.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Tests.Validation.ExampleRules;
   using Landorphan.Common.Validation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UseObjectOrCollectionInitializer

   public static class ValidationRuleBase_Tests
   {
      [TestClass]
      public class When_I_call_ValidationRuleBase_AddPropertyName : TestBase
      {
         private readonly ValidationRuleBase<MyTestEntity> target = new MyTestEntityIsNotNullValidationRule();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_add_a_unique_property_name()
         {
            var expected = Guid.NewGuid().ToString();

            var actual = target.AddPropertyName(expected);
            actual.Should().BeTrue();
            target.PropertyNames.Count().Should().Be(1);
            target.PropertyNames.Should().Contain(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_add_a_duplicate_property_name()
         {
            var expected = Guid.NewGuid().ToString();
            target.AddPropertyName(expected);

            var repeated = expected;
            var actual = target.AddPropertyName(repeated);
            actual.Should().BeFalse();
            target.PropertyNames.Count().Should().Be(1);
            target.PropertyNames.Should().Contain(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_not_throw_on_null()
         {
            var actual = target.AddPropertyName(null);
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleBase_BuildValidationRuleResult : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_copy_the_ValidationRuleName_and_ValidationRuleDescription()
         {
            // copying the name and description are the responsibility of the base class.
            var target = new MyTestEntityIsNotNullValidationRule();
            var actual = target.Validate(null);
            actual.ValidationRuleName.Should().Be(target.Name);
            actual.ValidationRuleDescription.Should().Be(target.Description);
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleBase_ClearPropertyNames : TestBase
      {
         private readonly ValidationRuleBase<MyTestEntity> target = new MyTestEntityIsNotNullValidationRule();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_clear_the_property_names()
         {
            target.AddPropertyName(Guid.NewGuid().ToString());
            target.AddPropertyName(Guid.NewGuid().ToString());
            target.AddPropertyName(Guid.NewGuid().ToString());

            target.ClearPropertyNames();
            target.PropertyNames.Count().Should().Be(0);
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleBase_RemovePropertyName : TestBase
      {
         private readonly ValidationRuleBase<MyTestEntity> target = new MyTestEntityIsNotNullValidationRule();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_not_throw_on_null()
         {
            var actual = target.RemovePropertyName(null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_remove_a_non_extant_property_name()
         {
            var extant = Guid.NewGuid().ToString();
            target.AddPropertyName(extant);
            target.PropertyNames.Count().Should().Be(1);
            target.PropertyNames.Should().Contain(extant);

            var actual = target.RemovePropertyName(Guid.NewGuid().ToString());
            actual.Should().BeFalse();
            target.PropertyNames.Count().Should().Be(1);
            target.PropertyNames.Should().Contain(extant);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_an_extant_property_name()
         {
            var expected = Guid.NewGuid().ToString();
            target.AddPropertyName(expected);
            target.PropertyNames.Count().Should().Be(1);
            target.PropertyNames.Should().Contain(expected);

            var actual = target.RemovePropertyName(expected);
            actual.Should().BeTrue();
            target.PropertyNames.Count().Should().Be(0);
            target.PropertyNames.Should().NotContain(expected);
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleBase_Clone : CloneableArrangeActAssert<IValidationRule<MyTestEntity>>
      {
         private Object actualObject;
         private String expectedPropertyName0;
         private String expectedPropertyName1;
         private String expectedPropertyName2;

         protected override IValidationRule<MyTestEntity> Target { get; set; }

         protected override void ArrangeMethod()
         {
            var obj = new MyTestEntityIsNotNullValidationRule();
            expectedPropertyName0 = Guid.NewGuid().ToString();
            obj.AddPropertyName(expectedPropertyName0);

            expectedPropertyName1 = Guid.NewGuid().ToString();
            obj.AddPropertyName(expectedPropertyName1);

            expectedPropertyName2 = Guid.NewGuid().ToString();
            obj.AddPropertyName(expectedPropertyName2);

            obj.MakeReadOnly();
            Target = obj;
         }

         protected override void ActMethod()
         {
            actualObject = Target.Clone();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_Should_Clone_Correctly()
         {
            It_Should_Clone_Correctly_Implementation();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false()
         {
            actualObject.Should().BeAssignableTo<ValidationRuleBase<MyTestEntity>>();

            var actualInterface = (IValidationRule<MyTestEntity>) actualObject;
            actualInterface.Equals(Target).Should().BeTrue();
            actualInterface.PropertyNames.Should()
               .BeEquivalentTo(
                  expectedPropertyName0,
                  expectedPropertyName1,
                  expectedPropertyName2);

            actualInterface.IsReadOnly.Should().BeFalse();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleBase_MakeReadOnly : ArrangeActAssert
      {
         private ValidationRuleBase<MyTestEntity> target;

         protected override void ArrangeMethod()
         {
            target = new MyTestEntityIsNotNullValidationRule();
            target.AddPropertyName(Guid.NewGuid().ToString());
            target.AddPropertyName(Guid.NewGuid().ToString());
            target.AddPropertyName(Guid.NewGuid().ToString());
         }

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_call_clone_It_should_create_an_equivalent_value_that_is_not_read_only()
         {
            var actual = (ValidationRuleBase<MyTestEntity>) target.Clone();
            actual.Equals(target).Should().BeTrue();
            actual.GetHashCode().Should().Be(target.GetHashCode());
            actual.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_read_only()
         {
            target.IsReadOnly.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_AddPropertyName()
         {
            Action throwingAction = () => target.AddPropertyName(Guid.NewGuid().ToString());
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_ClearPropertyNames()
         {
            Action throwingAction = () => target.ClearPropertyNames();
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_RemovePropertyName()
         {
            Action throwingAction = () => target.RemovePropertyName(Guid.NewGuid().ToString());
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_set_ValidationRuleDescription()
         {
            Action throwingAction = () => target.Description = Guid.NewGuid().ToString();
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_set_ValidationRuleName()
         {
            Action throwingAction = () => target.Name = Guid.NewGuid().ToString();
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_construct_a_ValidationRuleBase : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_default_values()
         {
            ValidationRuleBase<MyTestEntity> rule = new MyTestEntityIsNotNullValidationRule();
            ((IValidationRule<MyTestEntity>) rule).GetStringComparer().Should().Be(EqualityComparer<String>.Default);
            rule.IsReadOnly.Should().BeFalse();
            rule.PropertyNames.Count().Should().Be(0);
            rule.Description.Should().NotBeNull(); // set by descendant class
            rule.Name.Should().NotBeNull(); // set by descendant class
            rule.EntityType.Should().Be(typeof(MyTestEntity));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_use_the_specified_string_comparer()
         {
            var rule = new MyTestEntityIsNotNullValidationRule(StringComparer.CurrentCulture);
            ((IValidationRule<MyTestEntity>) rule).GetStringComparer().Should().Be(StringComparer.CurrentCulture);
         }
      }

      [TestClass]
      public class When_I_evaluate_ValidationRuleBase_equality : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_value_semantics_with_case_sensitivity()
         {
            ValidationRuleBase<MyTestEntity> left = new MyTestEntityIsNotNullValidationRule();
            ValidationRuleBase<MyTestEntity> right = new MyTestEntityIsNotNullValidationRule();
            left.Equals(right).Should().BeTrue();

            right = new MyTestEntityIsNotNullValidationRule();
            right.AddPropertyName(Guid.NewGuid().ToString());
            left.Equals(right).Should().BeFalse();

            right = new MyTestEntityIsNotNullValidationRule {Description = Guid.NewGuid().ToString()};
            left.Equals(right).Should().BeFalse();

            right = new MyTestEntityIsNotNullValidationRule {Name = Guid.NewGuid().ToString()};
            left.Equals(right).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            ValidationRuleBase<MyTestEntity> target = new MyTestEntityIsNotNullValidationRule();
            target.Equals(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_objects()
         {
            ValidationRuleBase<MyTestEntity> target = new MyTestEntityIsNotNullValidationRule();
            target.Equals(new Object()).Should().BeFalse();
            target.Equals(target.Clone()).Should().BeTrue();
         }
      }
   }
}