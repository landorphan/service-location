namespace Landorphan.Common.Tests.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Tests.Validation.ExampleRules;
   using Landorphan.Common.Validation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UseObjectOrCollectionInitializer

   public static class Validator_Tests
   {
      [TestClass]
      public class When_I_call_Validator_AddValidationRule : DisposableArrangeActAssert
      {
         private IValidationRule<MyTestEntity> expectedRule;
         private Validator<MyTestEntity> target;

         protected override void ArrangeMethod()
         {
            target = new Validator<MyTestEntity>();
            expectedRule = new MyTestEntityNameValidationRule();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_an_equivalent_reference_It_should_return_false()
         {
            target.AddValidationRule(expectedRule);

            var equivalentRule = new MyTestEntityNameValidationRule();
            target.AddValidationRule(equivalentRule).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_an_existing_reference_It_should_return_false()
         {
            target.AddValidationRule(expectedRule);
            target.AddValidationRule(expectedRule).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_null_It_should_return_false()
         {
            target.AddValidationRule(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_unique_It_should_return_true_and_add_it()
         {
            var was = target.Rules.Count();
            target.AddValidationRule(expectedRule).Should().BeTrue();
            target.Rules.Count().Should().Be(was + 1);
            target.Rules.Contains(expectedRule).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.AddValidationRule(null);
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");

            throwingAction = () => target.AddValidationRule(expectedRule);
            e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");

            throwingAction = () => target.AddValidationRule(new MyTestEntityNameValidationRule());
            e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_call_Validator_ClearValidationRules : DisposableArrangeActAssert
      {
         private IValidationRule<MyTestEntity> expectedRule;
         private Validator<MyTestEntity> target;

         protected override void ArrangeMethod()
         {
            target = new Validator<MyTestEntity>();
            expectedRule = new MyTestEntityNameValidationRule();
            target.AddValidationRule(expectedRule);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.ClearValidationRules();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_clear_the_Messages()
         {
            target.Rules.Count().Should().BeGreaterThan(0);
            target.ClearValidationRules();
            target.Rules.Count().Should().Be(0);
         }
      }

      [TestClass]
      public class When_I_call_Validator_Clone : CloneableArrangeActAssert<IValidator<MyTestEntity>>
      {
         private Object actualObject;

         private IValidationRule<MyTestEntity> expectedRule;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_Should_Clone_Correctly()
         {
            It_Should_Clone_Correctly_Implementation();
         }

         protected override IValidator<MyTestEntity> Target { get; set; }

         protected override void TeardownTestMethod()
         {
            (Target as IDisposable)?.Dispose();
            base.TeardownTestMethod();
         }

         [SuppressMessage("Microsoft.Reliability", "CA2000: Dispose objects before losing scope", Justification = "Disposed in teardown")]
         protected override void ArrangeMethod()
         {
            var obj = new Validator<MyTestEntity>();
            expectedRule = new MyTestEntityNameValidationRule();
            obj.AddValidationRule(expectedRule);
            obj.MakeReadOnly();
            Target = obj;
         }

         protected override void ActMethod()
         {
            actualObject = Target.Clone();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false()
         {
            var actualInterface = (IValidator<MyTestEntity>)actualObject;
            actualInterface.Equals(Target).Should().BeTrue();
            actualInterface.IsReadOnly.Should().BeFalse();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_call_Validator_MakeReadOnly : DisposableArrangeActAssert
      {
         private IValidationRule<MyTestEntity> expectedRule;
         private Validator<MyTestEntity> target;

         protected override void ArrangeMethod()
         {
            target = new Validator<MyTestEntity>();
            expectedRule = new MyTestEntityNameValidationRule();
            target.AddValidationRule(expectedRule);
         }

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_read_only_as_well_as_its_aggregates()
         {
            target.IsReadOnly.Should().BeTrue();
            foreach (var rule in target.Rules)
            {
               rule.IsReadOnly.Should().BeTrue();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_rules_to_be_added()
         {
            Action throwingAction = () => target.AddValidationRule(null);
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_rules_to_be_cleared()
         {
            Action throwingAction = () => target.ClearValidationRules();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_rules_to_be_removed()
         {
            Action throwingAction = () => target.RemoveValidationRule(null);
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_ValidatorDescription_to_be_set()
         {
            Action throwingAction = () => target.Description = Guid.NewGuid().ToString();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_ValidatorName_to_be_set()
         {
            Action throwingAction = () => target.Name = Guid.NewGuid().ToString();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_I_call_MakeReadOnly_again()
         {
            target.MakeReadOnly();
         }
      }

      [TestClass]
      public class When_I_call_Validator_RemoveValidationRule : DisposableArrangeActAssert
      {
         private IValidationRule<MyTestEntity> expectedRule;
         private Validator<MyTestEntity> target;

         protected override void ArrangeMethod()
         {
            target = new Validator<MyTestEntity>();
            expectedRule = new MyTestEntityNameValidationRule();
            target.AddValidationRule(expectedRule);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_null_It_should_return_false()
         {
            target.RemoveValidationRule(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_unique_It_should_return_false()
         {
            var rule = new MyTestEntityValuePositiveRule();

            var was = target.Rules.Count();
            target.RemoveValidationRule(rule).Should().BeFalse();
            target.Rules.Count().Should().Be(was);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_an_equivalent_reference_It_should_return_true_and_remove_the_rule()
         {
            var was = target.Rules.Count();
            target.RemoveValidationRule(new MyTestEntityNameValidationRule()).Should().BeTrue();
            target.Rules.Count().Should().Be(was - 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_rule_is_an_existing_reference_It_should_return_true_and_remove_the_rule()
         {
            var was = target.Rules.Count();
            target.RemoveValidationRule(expectedRule).Should().BeTrue();
            target.Rules.Count().Should().Be(was - 1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.RemoveValidationRule(null);
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");

            throwingAction = () => target.RemoveValidationRule(expectedRule);
            e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");

            throwingAction = () => target.RemoveValidationRule(new MyTestEntityValuePositiveRule());
            e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_construct_a_Validator : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_default_values()
         {
            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>()))
            {
               target.IsReadOnly.Should().BeFalse();
               target.Rules.Should().NotBeNull();
               target.Rules.Count().Should().Be(0);

               ((IValidator<MyTestEntity>)target).GetStringComparer().Should().Be(EqualityComparer<String>.Default);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_use_the_specified_string_comparer()
         {
            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>(StringComparer.CurrentCulture)))
            {
               ((IValidator<MyTestEntity>)target).GetStringComparer().Should().Be(StringComparer.CurrentCulture);
            }
         }
      }

      [TestClass]
      public class When_I_evaluate_ValidatorOfT_equality : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive")]
         public void It_should_employ_value_semantics()
         {
            using (var left = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>()))
            {
               var expectedRule = new MyTestEntityNameValidationRule();
               left.AddValidationRule(expectedRule);

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>(left)))
               {
                  left.Equals(right).Should().BeTrue();
               }

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>(left) {Description = Guid.NewGuid().ToString()}))
               {
                  left.Equals(right).Should().BeFalse();
               }

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>(left) {Name = Guid.NewGuid().ToString()}))
               {
                  left.Equals(right).Should().BeFalse();
               }

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>(left)))
               {
                  right.ClearValidationRules();
                  left.Equals(right).Should().BeFalse();
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>()))
            {
               var expectedRule = new MyTestEntityNameValidationRule();
               target.AddValidationRule(expectedRule);
               target.Equals(null).Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_objects()
         {
            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity>()))
            {
               var expectedRule = new MyTestEntityNameValidationRule();
               target.AddValidationRule(expectedRule);
               target.Equals(new Object()).Should().BeFalse();
               target.Equals(target.Clone()).Should().BeTrue();
            }
         }
      }

      [TestClass]
      public class When_I_set_ValidatorOfT_properties : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive")]
         public void It_should_coalesce_and_trim_MessageType()
         {
            const String ExpectedName = "My Validator Name";

            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity> {Name = null}))
            {
               target.Name.Should().Be(String.Empty);

               target.Name = String.Empty;
               target.Name.Should().Be(String.Empty);

               target.Name = " \t ";
               target.Name.Should().Be(String.Empty);

               target.Name = ExpectedName;
               target.Name.Should().Be(ExpectedName);

               target.Name = " \t " + ExpectedName + " \t ";
               target.Name.Should().Be(ExpectedName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive")]
         public void It_should_coalesce_and_trim_ValidatorDescription()
         {
            const String ExpectedDescription = "My Validator Description";

            using (var target = DisposableHelper.SafeCreate(() => new Validator<MyTestEntity> {Description = null}))
            {
               target.Description.Should().Be(String.Empty);

               target.Description = String.Empty;
               target.Description.Should().Be(String.Empty);

               target.Description = " \t ";
               target.Description.Should().Be(String.Empty);

               target.Description = ExpectedDescription;
               target.Description.Should().Be(ExpectedDescription);

               target.Description = " \t " + ExpectedDescription + " \t ";
               target.Description.Should().Be(ExpectedDescription);
            }
         }
      }
   }
}
