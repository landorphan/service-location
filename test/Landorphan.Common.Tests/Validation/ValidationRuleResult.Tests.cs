namespace Landorphan.Common.Tests.Validation
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common.Validation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UseObjectOrCollectionInitializer

   public static class ValidationRuleResult_Tests
   {
      [TestClass]
      public class When_I_call_ValidationRuleResult_AddMessage : DisposableArrangeActAssert
      {
         private IValidationMessage expectedValidationMessage0;
         private IValidationMessage expectedValidationMessage1;
         private IValidationMessage expectedValidationMessage2;
         private IValidationMessage expectedValidationMessage3;
         private IValidationMessageFactory messageFactory;

         private ValidationRuleResult target;

         protected override void ArrangeMethod()
         {
            messageFactory = new ValidationMessageFactory();

            expectedValidationMessage0 = messageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            expectedValidationMessage1 = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            expectedValidationMessage2 = messageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            expectedValidationMessage3 = messageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            target = new ValidationRuleResult();
            target.AddMessage(expectedValidationMessage0);
            target.AddMessage(expectedValidationMessage1);
            target.AddMessage(expectedValidationMessage2);
            target.AddMessage(expectedValidationMessage3);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_an_equivalent_reference_It_should_return_false()
         {
            target.AddMessage(new ValidationMessage(expectedValidationMessage0)).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_an_existing_reference_It_should_return_false()
         {
            target.AddMessage(expectedValidationMessage0).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_null_It_should_return_false()
         {
            target.AddMessage(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_unique_It_should_return_true_and_add_it()
         {
            var was = target.Messages.Count();
            var messageToAdd = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            target.AddMessage(messageToAdd).Should().BeTrue();
            target.Messages.Count().Should().Be(was + 1);
            target.Messages.Contains(messageToAdd).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.AddMessage(null);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");

            throwingAction = () => target.AddMessage(expectedValidationMessage0);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");

            throwingAction = () => target.AddMessage(messageFactory.CreateInformationMessage(Guid.NewGuid().ToString()));
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleResult_ClearMessages : DisposableArrangeActAssert
      {
         private IValidationMessage expectedValidationMessage0;
         private IValidationMessage expectedValidationMessage1;
         private IValidationMessage expectedValidationMessage2;
         private IValidationMessage expectedValidationMessage3;
         private IValidationMessageFactory messageFactory;

         private ValidationRuleResult target;

         protected override void ArrangeMethod()
         {
            messageFactory = new ValidationMessageFactory();

            expectedValidationMessage0 = messageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            expectedValidationMessage1 = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            expectedValidationMessage2 = messageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            expectedValidationMessage3 = messageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            target = new ValidationRuleResult();
            target.AddMessage(expectedValidationMessage0);
            target.AddMessage(expectedValidationMessage1);
            target.AddMessage(expectedValidationMessage2);
            target.AddMessage(expectedValidationMessage3);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.ClearMessages();
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_clear_the_Messages()
         {
            target.Messages.Count().Should().BeGreaterThan(0);
            target.ClearMessages();
            target.Messages.Count().Should().Be(0);
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleResult_Clone : CloneableArrangeActAssert<IValidationRuleResult>
      {
         private Object actualObject;

         private Object evaluatedObject = new Object();
         private String expectedDescription;
         private String expectedName;

         private IValidationMessage expectedValidationMessage0;
         private IValidationMessage expectedValidationMessage1;
         private IValidationMessage expectedValidationMessage2;
         private IValidationMessage expectedValidationMessage3;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_Should_Clone_Correctly()
         {
            It_Should_Clone_Correctly_Implementation();
         }

         protected override IValidationRuleResult Target { get; set; }

         protected override void TeardownTestMethod()
         {
            (Target as IDisposable)?.Dispose();
            base.TeardownTestMethod();
         }

         [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
         protected override void ArrangeMethod()
         {
            var obj = new ValidationRuleResult();
            evaluatedObject = new Object();
            obj.SetEvaluatedEntity(evaluatedObject);
            expectedDescription = Guid.NewGuid().ToString();
            obj.SetValidationRuleDescription(expectedDescription);
            expectedName = Guid.NewGuid().ToString();
            obj.SetValidationRuleName(expectedName);

            var validationMessageFactory = new ValidationMessageFactory();

            expectedValidationMessage0 = validationMessageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            expectedValidationMessage1 = validationMessageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            expectedValidationMessage2 = validationMessageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            expectedValidationMessage3 = validationMessageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            obj.AddMessage(expectedValidationMessage0);
            obj.AddMessage(expectedValidationMessage1);
            obj.AddMessage(expectedValidationMessage2);
            obj.AddMessage(expectedValidationMessage3);
            obj.MakeReadOnly();
            Target = obj;
         }

         protected override void ActMethod()
         {
            actualObject = Target.Clone();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false_on_the_instance_and_the_aggregates()
         {
            actualObject.Should().BeOfType<ValidationRuleResult>();

            var actualInterface = (IValidationRuleResult)actualObject;
            actualInterface.Equals(Target).Should().BeTrue();

            actualInterface.EvaluatedEntity.Should().BeSameAs(evaluatedObject);
            actualInterface.ValidationRuleDescription.Should().Be(expectedDescription);
            actualInterface.ValidationRuleName.Should().Be(expectedName);
            actualInterface.Messages.Should()
               .BeEquivalentTo(
                  expectedValidationMessage0,
                  expectedValidationMessage1,
                  expectedValidationMessage2,
                  expectedValidationMessage3);
            actualInterface.GetHasError().Should().BeTrue();

            actualInterface.IsReadOnly.Should().BeFalse();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
            foreach (var msg in actualInterface.Messages)
            {
               msg.IsReadOnly.Should().BeFalse();
            }
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleResult_MakeReadOnly : DisposableArrangeActAssert
      {
         private ValidationRuleResult target;

         protected override void ArrangeMethod()
         {
            var validationMessageFactory = new ValidationMessageFactory();

            var expectedValidationMessage0 = validationMessageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage1 = validationMessageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage2 = validationMessageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage3 = validationMessageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            target = new ValidationRuleResult();
            target.AddMessage(expectedValidationMessage0);
            target.AddMessage(expectedValidationMessage1);
            target.AddMessage(expectedValidationMessage2);
            target.AddMessage(expectedValidationMessage3);
         }

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_call_clone_It_should_create_an_equivalent_value_that_is_not_read_only()
         {
            var actual = (ValidationRuleResult)target.Clone();
            actual.Equals(target).Should().BeTrue();
            actual.GetHashCode().Should().Be(target.GetHashCode());
            actual.IsReadOnly.Should().BeFalse();
            foreach (var msg in actual.Messages)
            {
               msg.IsReadOnly.Should().BeFalse();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_read_only_as_well_as_its_aggregates()
         {
            target.IsReadOnly.Should().BeTrue();
            foreach (var msg in target.Messages)
            {
               msg.IsReadOnly.Should().BeTrue();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_add_messages()
         {
            Action throwingAction = () => target.AddMessage(null);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_clear_messages()
         {
            Action throwingAction = () => target.ClearMessages();
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_remove_messages()
         {
            Action throwingAction = () => target.RemoveMessage(null);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_call_ValidationRuleResult_RemoveMessage : DisposableArrangeActAssert
      {
         private IValidationMessage expectedValidationMessage0;
         private IValidationMessage expectedValidationMessage1;
         private IValidationMessage expectedValidationMessage2;
         private IValidationMessage expectedValidationMessage3;
         private IValidationMessageFactory messageFactory;

         private ValidationRuleResult target;

         protected override void ArrangeMethod()
         {
            messageFactory = new ValidationMessageFactory();

            expectedValidationMessage0 = messageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            expectedValidationMessage1 = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            expectedValidationMessage2 = messageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            expectedValidationMessage3 = messageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            target = new ValidationRuleResult();
            target.AddMessage(expectedValidationMessage0);
            target.AddMessage(expectedValidationMessage1);
            target.AddMessage(expectedValidationMessage2);
            target.AddMessage(expectedValidationMessage3);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_an_equivalent_reference_It_should_return_true()
         {
            target.RemoveMessage(new ValidationMessage(expectedValidationMessage0)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_an_existing_reference_It_should_return_true()
         {
            target.RemoveMessage(expectedValidationMessage0).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_null_It_should_return_false()
         {
            target.RemoveMessage(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_message_is_unique_It_should_return_false()
         {
            var messageToRemove = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            target.RemoveMessage(messageToRemove).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_target_is_read_only_It_should_throw()
         {
            target.MakeReadOnly();
            Action throwingAction = () => target.RemoveMessage(null);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");

            throwingAction = () => target.RemoveMessage(expectedValidationMessage0);
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");

            throwingAction = () => target.RemoveMessage(messageFactory.CreateInformationMessage(Guid.NewGuid().ToString()));
            throwingAction.Should()
               .Throw<NotSupportedException>()
               .WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_construct_a_ValidationRuleResult : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_clone_constructor_and_other_is_null_It_should_throw_ArgumentNullException()
         {
            // ReSharper disable once ObjectCreationAsStatement
            Action throwingAction = () => DisposableHelper.SafeCreate(() => new ValidationRuleResult((ValidationRuleResult)null));
            throwingAction.Should()
               .Throw<ArgumentNullException>()
               .And.ParamName.Should()
               .Be("other");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_clone_constructor_It_should_copy_the_values_and_set_IsReadOnly_to_false()
         {
            var validationMessageFactory = new ValidationMessageFactory();

            var expectedValidationMessage0 = validationMessageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage1 = validationMessageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage2 = validationMessageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            var expectedValidationMessage3 = validationMessageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            using (var original = DisposableHelper.SafeCreate(() => new ValidationRuleResult()))
            {
               original.AddMessage(expectedValidationMessage0);
               original.AddMessage(expectedValidationMessage1);
               original.AddMessage(expectedValidationMessage2);
               original.AddMessage(expectedValidationMessage3);

               original.MakeReadOnly();
               original.IsReadOnly.Should().BeTrue();

               // ReSharper disable once AccessToDisposedClosure
               using (var actual = DisposableHelper.SafeCreate(() => new ValidationRuleResult(original)))
               {
                  actual.Equals(original).Should().BeTrue();
                  actual.IsReadOnly.Should().BeFalse();
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_default_constructor_It_should_have_the_expected_values()
         {
            using (var target = DisposableHelper.SafeCreate(() => new ValidationRuleResult()))
            {
               target.GetHasError().Should().BeFalse();
               target.IsReadOnly.Should().BeFalse();
               target.Messages.Should().NotBeNull();
               target.Messages.Count().Should().Be(0);
            }
         }
      }

      [TestClass]
      public class When_I_evaluate_ValidationRuleResult_equality : DisposableArrangeActAssert
      {
         private IValidationMessage expectedValidationMessage0;
         private IValidationMessage expectedValidationMessage1;
         private IValidationMessage expectedValidationMessage2;
         private IValidationMessage expectedValidationMessage3;
         private IValidationMessageFactory messageFactory;

         private ValidationRuleResult target;

         protected override void ArrangeMethod()
         {
            messageFactory = new ValidationMessageFactory();

            expectedValidationMessage0 = messageFactory.CreateErrorMessage(Guid.NewGuid().ToString());
            expectedValidationMessage1 = messageFactory.CreateInformationMessage(Guid.NewGuid().ToString());
            expectedValidationMessage2 = messageFactory.CreateVerboseMessage(Guid.NewGuid().ToString());
            expectedValidationMessage3 = messageFactory.CreateWarningMessage(Guid.NewGuid().ToString());

            target = new ValidationRuleResult();
            target.AddMessage(expectedValidationMessage0);
            target.AddMessage(expectedValidationMessage1);
            target.AddMessage(expectedValidationMessage2);
            target.AddMessage(expectedValidationMessage3);
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_value_semantics_with_case_sensitivity()
         {
            using (var left = DisposableHelper.SafeCreate(() => new ValidationRuleResult(target)))
            {
               left.GetHasError().Should().BeTrue();

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new ValidationRuleResult(left)))
               {
                  foreach (var msg in right.Messages)
                  {
                     if (msg.IsError)
                     {
                        ((ValidationMessage)msg).IsError = false;
                     }
                  }

                  right.GetHasError().Should().BeFalse();
                  left.Equals(right).Should().BeFalse();
               }

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new ValidationRuleResult(left)))
               {
                  foreach (var msg in right.Messages)
                  {
                     ((ValidationMessage)msg).Message = msg.Message.ToUpperInvariant();
                  }

                  left.Equals(right).Should().BeFalse();
               }

               // ReSharper disable once AccessToDisposedClosure
               using (var right = DisposableHelper.SafeCreate(() => new ValidationRuleResult(left)))
               {
                  foreach (var msg in right.Messages)
                  {
                     ((ValidationMessage)msg).MessageType = msg.MessageType.ToUpperInvariant();
                  }

                  left.Equals(right).Should().BeFalse();
               }
            }
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            target.Equals(null).Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_objects()
         {
            target.Equals(new Object()).Should().BeFalse();

            target.Equals(target.Clone()).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_set_ValidationRuleResult_properties : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_and_trim_ValidationRuleDescription()
         {
            const String Expected = "My Validation Rule Description";

            using (var target = DisposableHelper.SafeCreate(() => new ValidationRuleResult()))
            {
               target.ValidationRuleDescription = null;
               target.ValidationRuleDescription.Should().Be(String.Empty);

               target.ValidationRuleDescription = String.Empty;
               target.ValidationRuleDescription.Should().Be(String.Empty);

               target.ValidationRuleDescription = " \t ";
               target.ValidationRuleDescription.Should().Be(String.Empty);

               target.ValidationRuleDescription = Expected;
               target.ValidationRuleDescription.Should().Be(Expected);

               target.ValidationRuleDescription = " \t " + Expected + " \t ";
               target.ValidationRuleDescription.Should().Be(Expected);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_and_trim_ValidationRuleName()
         {
            const String Expected = "My Validation Rule Description";

            using (var target = DisposableHelper.SafeCreate(() => new ValidationRuleResult()))

            {
               target.ValidationRuleName = null;
               target.ValidationRuleName.Should().Be(String.Empty);

               target.ValidationRuleName = String.Empty;
               target.ValidationRuleName.Should().Be(String.Empty);

               target.ValidationRuleName = " \t ";
               target.ValidationRuleName.Should().Be(String.Empty);

               target.ValidationRuleName = Expected;
               target.ValidationRuleName.Should().Be(Expected);

               target.ValidationRuleName = " \t " + Expected + " \t ";
               target.ValidationRuleName.Should().Be(Expected);
            }
         }
      }
   }
}
