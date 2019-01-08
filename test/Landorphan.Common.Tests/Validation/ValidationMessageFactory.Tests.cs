namespace Landorphan.Common.Tests.Validation
{
   using FluentAssertions;
   using Landorphan.Common.Resources;
   using Landorphan.Common.Validation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UseObjectOrCollectionInitializer

   public static class ValidationMessageFactory_Tests
   {
      [TestClass]
      public class When_I_call_ValidationMessageFactory_CreateErrorMessage : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_error_ValidationMessage()
         {
            var target = new ValidationMessageFactory();

            var actual = target.CreateErrorMessage("My Text");
            actual.IsError.Should().BeTrue();
            actual.Message.Should().Be("My Text");
            actual.MessageType.Should().Be(StringResources.MessageTypeError);
            actual.IsReadOnly.Should().BeTrue();

            actual = target.CreateErrorMessage("{0} + {1}", "x", "y");
            actual.IsError.Should().BeTrue();
            actual.Message.Should().Be("x + y");
            actual.MessageType.Should().Be(StringResources.MessageTypeError);
            actual.IsReadOnly.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ValidationMessageFactory_CreateInformationMessage : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_information_ValidationMessage()
         {
            var target = new ValidationMessageFactory();

            var actual = target.CreateInformationMessage("My Text");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("My Text");
            actual.MessageType.Should().Be(StringResources.MessageTypeInformation);
            actual.IsReadOnly.Should().BeTrue();

            actual = target.CreateInformationMessage("{0} + {1}", "x", "y");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("x + y");
            actual.MessageType.Should().Be(StringResources.MessageTypeInformation);
            actual.IsReadOnly.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ValidationMessageFactory_CreateVerboseMessage : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_information_ValidationMessage()
         {
            var target = new ValidationMessageFactory();

            var actual = target.CreateVerboseMessage("My Text");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("My Text");
            actual.MessageType.Should().Be(StringResources.MessageTypeVerbose);
            actual.IsReadOnly.Should().BeTrue();

            actual = target.CreateVerboseMessage("{0} + {1}", "x", "y");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("x + y");
            actual.MessageType.Should().Be(StringResources.MessageTypeVerbose);
            actual.IsReadOnly.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ValidationMessageFactory_CreateWarningMessage : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_information_ValidationMessage()
         {
            var target = new ValidationMessageFactory();

            var actual = target.CreateWarningMessage("My Text");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("My Text");
            actual.MessageType.Should().Be(StringResources.MessageTypeWarning);
            actual.IsReadOnly.Should().BeTrue();

            actual = target.CreateWarningMessage("{0} + {1}", "x", "y");
            actual.IsError.Should().BeFalse();
            actual.Message.Should().Be("x + y");
            actual.MessageType.Should().Be(StringResources.MessageTypeWarning);
            actual.IsReadOnly.Should().BeTrue();
         }
      }
   }
}