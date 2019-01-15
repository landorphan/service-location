namespace Landorphan.Common.Tests.Extensions
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class Guard_Tests
   {
      [TestClass]
      public class When_I_call_ArgumentGreaterThan : ArrangeActAssert
      {
         private const String ExceptionMessageFmt =
            "The value must be greater than '{0}' but is '{1}'.\r\nParameter name: {2}\r\nActual value was {1}.";

         private const String NullParamNameExceptionMessageFmt =
            "The value must be greater than '{0}' but is '{1}'.\r\nActual value was {1}.";

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_greater_than_the_comparand()
         {
            Action act = () => 5.ArgumentGreaterThan(0, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_equals_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 1;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentGreaterThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 1;
            const Int32 comparand = 5;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentGreaterThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand_and_handle_null_argument_names()
         {
            // When the parameter name is null, the BCL changes the message body.
            const String givenParameterName = null;
            const Int32 value = 1;
            const Int32 comparand = 5;
            var expectedMessage = String.Format(
               CultureInfo.InvariantCulture,
               NullParamNameExceptionMessageFmt,
               comparand,
               value);

            Action throwingAction = () => value.ArgumentGreaterThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_call_ArgumentGreaterThanOrEqualTo : ArrangeActAssert
      {
         private const String ExceptionMessageFmt =
            "The value must be greater than or equal to '{0}' but is '{1}'.\r\nParameter name: {2}\r\nActual value was {1}.";

         private const String NullParamNameExceptionMessageFmt =
            "The value must be greater than or equal to '{0}' but is '{1}'.\r\nActual value was {1}.";

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_equal_to_the_comparand()
         {
            Action act = () => 5.ArgumentGreaterThanOrEqualTo(5, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_greater_than_the_comparand()
         {
            Action act = () => 5.ArgumentGreaterThanOrEqualTo(0, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 1;
            const Int32 comparand = 5;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentGreaterThanOrEqualTo(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand_and_handle_null_argument_names()
         {
            // When the parameter name is null, the BCL changes the message body.
            const String givenParameterName = null;
            const Int32 value = 1;
            const Int32 comparand = 5;
            var expectedMessage = String.Format(
               CultureInfo.InvariantCulture,
               NullParamNameExceptionMessageFmt,
               comparand,
               value);

            Action throwingAction = () => value.ArgumentGreaterThanOrEqualTo(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_call_ArgumentLessThan : ArrangeActAssert
      {
         private const String ExceptionMessageFmt =
            "The value must be less than '{0}' but is '{1}'.\r\nParameter name: {2}\r\nActual value was {1}.";

         private const String NullParamNameExceptionMessageFmt =
            "The value must be less than '{0}' but is '{1}'.\r\nActual value was {1}.";

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_greater_than_the_comparand()
         {
            Action act = () => 1.ArgumentLessThan(5, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_equals_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 1;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentLessThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 5;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentLessThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand_and_handle_null_argument_names()
         {
            // When the parameter name is null, the BCL changes the message body.
            const String givenParameterName = null;
            const Int32 value = 5;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(
               CultureInfo.InvariantCulture,
               NullParamNameExceptionMessageFmt,
               comparand,
               value);

            Action throwingAction = () => value.ArgumentLessThan(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_call_ArgumentLessThanOrEqualTo : ArrangeActAssert
      {
         private const String ExceptionMessageFmt =
            "The value must be less than or equal to '{0}' but is '{1}'.\r\nParameter name: {2}\r\nActual value was {1}.";

         private const String NullParamNameExceptionMessageFmt =
            "The value must be less than or equal to '{0}' but is '{1}'.\r\nActual value was {1}.";

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_equal_to_the_comparand()
         {
            Action act = () => 5.ArgumentLessThanOrEqualTo(5, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_greater_than_the_comparand()
         {
            Action act = () => 1.ArgumentLessThanOrEqualTo(5, "myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand()
         {
            const String givenParameterName = "myParameterName";
            const Int32 value = 5;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(CultureInfo.InvariantCulture, ExceptionMessageFmt, comparand, value, givenParameterName);

            Action throwingAction = () => value.ArgumentLessThanOrEqualTo(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            It_should_throw_with_the_provided_argument_name_when_the_value_is_less_than_the_comparand_and_handle_null_argument_names()
         {
            // When the parameter name is null, the BCL changes the message body.
            const String givenParameterName = null;
            const Int32 value = 5;
            const Int32 comparand = 1;
            var expectedMessage = String.Format(
               CultureInfo.InvariantCulture,
               NullParamNameExceptionMessageFmt,
               comparand,
               value);

            Action throwingAction = () => value.ArgumentLessThanOrEqualTo(comparand, givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.WithMessage(expectedMessage);
            e.And.ActualValue.Should().BeEquivalentTo(value);
            e.And.ParamName.Should().Be(givenParameterName);
         }
      }

      [TestClass]
      public class When_I_call_ArgumentNotEmptyNorWhiteSpace : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_not_null_not_empty_and_not_whitespace()
         {
            const String givenParameterName = "myParameterName";

            Action act = () => " \tabc\t ".ArgumentNotEmptyNorWhiteSpace(givenParameterName);
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_null()
         {
            const String givenParameterName = "myParameterName";
            const String value = null;

            Action act = () => value.ArgumentNotEmptyNorWhiteSpace(givenParameterName);
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_the_value_is_empty()
         {
            const String givenParameterName = "myParameterName";
            var value = String.Empty;

            Action throwingAction = () => value.ArgumentNotEmptyNorWhiteSpace(givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentEmptyException>();
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_the_value_is_not_null_not_empty_and_not_whitespace()
         {
            const String givenParameterName = "myParameterName";
            const String value = " \t ";

            Action throwingAction = () => value.ArgumentNotEmptyNorWhiteSpace(givenParameterName);
            var e = throwingAction.Should().Throw<ArgumentWhiteSpaceException>();
            e.And.ParamName.Should().Be(givenParameterName);
         }
      }

      [TestClass]
      public class When_I_call_ArgumentNotNull : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_argument_names()
         {
            Action throwingAction = () => ((String)null).ArgumentNotNull(null);

            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.WithMessage("Value cannot be null.");
            e.And.ParamName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_non_null_references()
         {
            Action act = () => Guid.NewGuid().ToString().ArgumentNotNull("myParameterName");
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException_with_the_provided_argument_name_on_null_references()
         {
            const String givenParameterName = "myParameterName";
            var expectedMessage = String.Format(
               CultureInfo.InvariantCulture,
               "Value cannot be null.\r\nParameter name: {0}",
               givenParameterName);

            Action throwingAction = () => ((String)null).ArgumentNotNull(givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.WithMessage(expectedMessage);
            e.And.ParamName.Should().Be(givenParameterName);
         }
      }

      [TestClass]
      public class When_I_call_ArgumentNotNullNorContainsNull : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_argument_names_when_contains_null()
         {
            IEnumerable<String> value = new[] {Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString()};

            Action throwingAction = () => value.ArgumentNotNullNorContainsNull(null);
            var e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_argument_names_when_null()
         {
            Action throwingAction = () => ((IEnumerable<String>)null).ArgumentNotNullNorContainsNull(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentContainsNullException()
         {
            var givenParamName = Guid.NewGuid().ToString();
            IEnumerable<String> value = new[] {Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString()};

            Action throwingAction = () => value.ArgumentNotNullNorContainsNull(givenParamName);
            var e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(givenParamName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException_on_null()
         {
            var givenParamName = Guid.NewGuid().ToString();

            Action throwingAction = () => ((IEnumerable<String>)null).ArgumentNotNullNorContainsNull(givenParamName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(givenParamName);
         }
      }

      [TestClass]
      public class When_I_call_ArgumentNotNullNorEmpty : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_argument_names_when_empty()
         {
            IEnumerable<String> value = new List<String>();

            Action throwingAction = () => value.ArgumentNotNullNorEmpty(null);
            var e = throwingAction.Should().Throw<ArgumentEmptyException>();
            e.And.ParamName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_null_argument_names_when_null()
         {
            Action throwingAction = () => ((IEnumerable<String>)null).ArgumentNotNullNorEmpty(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentEmptyException()
         {
            var givenParamName = Guid.NewGuid().ToString();
            IEnumerable<String> value = new List<String>();

            Action throwingAction = () => value.ArgumentNotNullNorEmpty(givenParamName);
            var e = throwingAction.Should().Throw<ArgumentEmptyException>();
            e.And.ParamName.Should().Be(givenParamName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException_on_null()
         {
            var givenParamName = Guid.NewGuid().ToString();

            Action throwingAction = () => ((IEnumerable<String>)null).ArgumentNotNullNorEmpty(givenParamName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(givenParamName);
         }
      }

      [TestClass]
      public class When_I_call_ArgumentNotNullNorEmptyNorWhiteSpace : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_value_is_not_null_not_empty_and_not_whitespace()
         {
            const String givenParameterName = "myParameterName";

            Action act = () => " \tabc\t ".ArgumentNotNullNorEmptyNorWhiteSpace(givenParameterName);
            act.Should().NotThrow();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_the_value_is_empty()
         {
            const String givenParameterName = "myParameterName";
            var value = String.Empty;

            Action throwingAction = () => value.ArgumentNotNullNorEmptyNorWhiteSpace(givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentEmptyException>();
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_the_value_is_null()
         {
            const String givenParameterName = "myParameterName";
            const String value = null;

            Action throwingAction = () => value.ArgumentNotNullNorEmptyNorWhiteSpace(givenParameterName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(givenParameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_the_value_is_whitespace()
         {
            const String givenParameterName = "myParameterName";
            const String value = " \t ";

            Action throwingAction = () => value.ArgumentNotNullNorEmptyNorWhiteSpace(givenParameterName);

            var e = throwingAction.Should().Throw<ArgumentWhiteSpaceException>();
            e.And.ParamName.Should().Be(givenParameterName);
         }
      }
   }
}
