namespace Landorphan.Common.Tests.Validation
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Common.Validation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UseObjectOrCollectionInitializer

   public static class ValidationMessage_Tests
   {
      private const String Whitespace = " \t ";

      [TestClass]
      public class When_I_call_ValidationMessage_Clone : CloneableArrangeActAssert<IValidationMessage>
      {
         private const Boolean ExpectedIsError = true;
         private const String ExpectedMessage = "My Message";
         private const String ExpectedMessageType = "My Message Type";
         private Object actualObject;

         protected override IValidationMessage Target { get; set; }

         protected override void ArrangeMethod()
         {
            var obj = new ValidationMessage {IsError = ExpectedIsError, Message = ExpectedMessage, MessageType = ExpectedMessageType};
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
            actualObject.Should().BeOfType<ValidationMessage>();

            var actualInterface = (IValidationMessage) actualObject;
            actualInterface.Equals(Target).Should().BeTrue();

            actualInterface.IsError.Should().Be(ExpectedIsError);
            actualInterface.Message.Should().Be(ExpectedMessage);
            actualInterface.MessageType.Should().Be(ExpectedMessageType);

            actualInterface.IsReadOnly.Should().BeFalse();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_call_ValidationMessage_MakeReadOnly : ArrangeActAssert
      {
         private ValidationMessage target;

         protected override void ArrangeMethod()
         {
            target = new ValidationMessage();
         }

         protected override void ActMethod()
         {
            target.MakeReadOnly();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_call_clone_It_should_create_an_equivalent_value_that_is_not_read_only()
         {
            var actual = (ValidationMessage) target.Clone();
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
         public void It_should_not_let_me_set_IsError()
         {
            Action throwingAction = () => target.IsError = true;
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_set_Message()
         {
            Action throwingAction = () => target.Message = Guid.NewGuid().ToString();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_let_me_set_MessageType()
         {
            Action throwingAction = () => target.MessageType = Guid.NewGuid().ToString();
            var e = throwingAction.Should().Throw<NotSupportedException>();
            e.WithMessage("*current instance is read-only*");
         }
      }

      [TestClass]
      public class When_I_construct_a_ValidationMessage : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_clone_constructor_and_other_is_null_It_should_throw_ArgumentNullException()
         {
            // ReSharper disable once ObjectCreationAsStatement
            Action throwingAction = () => new ValidationMessage((ValidationMessage) null);
            throwingAction.Should()
               .Throw<ArgumentNullException>()
               .And.ParamName.Should()
               .Be("other");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_clone_constructor_It_should_copy_the_values_and_set_IsReadOnly_to_false()
         {
            const Boolean ExpectedIsError = true;
            const String ExpectedMessage = "My Message";
            const String ExpectedMessageType = "My Message Type";

            var original = new ValidationMessage {IsError = ExpectedIsError, Message = ExpectedMessage, MessageType = ExpectedMessageType};
            original.MakeReadOnly();

            var target = new ValidationMessage(original);
            target.Equals(original).Should().BeTrue();

            target.IsError.Should().Be(ExpectedIsError);
            target.Message.Should().Be(ExpectedMessage);
            target.MessageType.Should().Be(ExpectedMessageType);

            target.IsReadOnly.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Using_the_default_constructor_It_should_have_the_expected_values()
         {
            var target = new ValidationMessage();
            target.IsError.Should().BeFalse();
            target.IsReadOnly.Should().BeFalse();
            target.Message.Should().Be(string.Empty);
            target.MessageType.Should().Be(string.Empty);
         }
      }

      [TestClass]
      public class When_I_evaluate_ValidationMessage_equality : TestBase
      {
         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_employ_value_semantics_with_case_sensitivity()
         {
            var left = new ValidationMessage
            {
               IsError = true,
               Message = Guid.NewGuid().ToString().ToLowerInvariant(),
               MessageType = Guid.NewGuid().ToString().ToLowerInvariant()
            };

            var right = new ValidationMessage(left);
            right.IsError = !left.IsError;
            left.Equals(right).Should().BeFalse();

            right = new ValidationMessage(left);
            right.Message = left.Message.ToUpperInvariant();
            left.Equals(right).Should().BeFalse();

            right = new ValidationMessage(left);
            right.MessageType = left.MessageType.ToUpperInvariant();
            left.Equals(right).Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null()
         {
            var target = new ValidationMessage
            {
               IsError = true,
               Message = Guid.NewGuid().ToString().ToLowerInvariant(),
               MessageType = Guid.NewGuid().ToString().ToLowerInvariant()
            };

            target.Equals(null).Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_objects()
         {
            var target = new ValidationMessage
            {
               IsError = true,
               Message = Guid.NewGuid().ToString().ToLowerInvariant(),
               MessageType = Guid.NewGuid().ToString().ToLowerInvariant()
            };

            target.Equals(new Object()).Should().BeFalse();

            target.Equals(target.Clone()).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_set_ValidationMessage_properties : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accurately_transpose_IsError()
         {
            var target = new ValidationMessage();

            target.IsError = true;
            target.IsError.Should().BeTrue();

            target.IsError = false;
            target.IsError.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_and_trim_Message()
         {
            const String ExpectedMessage = "My Message";

            var target = new ValidationMessage();

            target.Message = null;
            target.Message.Should().Be(string.Empty);

            target.Message = string.Empty;
            target.Message.Should().Be(string.Empty);

            target.Message = Whitespace;
            target.Message.Should().Be(string.Empty);

            target.Message = ExpectedMessage;
            target.Message.Should().Be(ExpectedMessage);

            target.Message = Whitespace + ExpectedMessage + Whitespace;
            target.Message.Should().Be(ExpectedMessage);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_and_trim_MessageType()
         {
            const String ExpectedMessageType = "My Message Type";

            var target = new ValidationMessage();

            target.MessageType = null;
            target.MessageType.Should().Be(string.Empty);

            target.MessageType = string.Empty;
            target.MessageType.Should().Be(string.Empty);

            target.MessageType = Whitespace;
            target.MessageType.Should().Be(string.Empty);

            target.MessageType = ExpectedMessageType;
            target.MessageType.Should().Be(ExpectedMessageType);

            target.MessageType = Whitespace + ExpectedMessageType + Whitespace;
            target.MessageType.Should().Be(ExpectedMessageType);
         }
      }
   }
}