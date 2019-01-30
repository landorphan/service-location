namespace Landorphan.Abstractions.Tests.Internal.Console
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.Internal;
   using Landorphan.Abstractions.Tests.Console;
   using Landorphan.Common;
   using Landorphan.Common.Exceptions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
   public partial class ConsoleInternalMapping_Tests
   {
      private static readonly ConsoleInternalMapping _target = new ConsoleInternalMapping();

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_BackgroundColor_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_invalid_ConsoleColor_Value_it_should_throw()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.BackgroundColor = (ConsoleColor)invalidValue;
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.EnumType.Should().Be(typeof(ConsoleColor));
            e.And.InvalidValue.Should().Be(invalidValue);
            e.And.Message.Should().Contain("The value of argument 'value' (");
            e.And.Message.Should().Contain(") is invalid for Enum type 'ConsoleColor'");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_valid_ConsoleColor_Value_it_should_take()
         {
            const ConsoleColor expected = ConsoleColor.Magenta;

            try
            {
               _target.BackgroundColor = expected;
               var actual = _target.BackgroundColor;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.ResetColor();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void The_value_is_a_valid_ConsoleColor_Value()
         {
            var actual = _target.BackgroundColor;
            actual.IsValidEnumValue().Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_BufferHeight_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.BufferHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.WindowHeight + _target.WindowTop;

            var original = _target.BufferHeight;
            try
            {
               _target.BufferHeight = expected;
               var actual = _target.BufferHeight;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.BufferHeight = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.BufferHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_zero()
         {
            const Int32 invalidValue = 0;

            Action throwingAction = () => _target.BufferHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_zero()
         {
            var actual = _target.BufferHeight;
            actual.Should().BeGreaterThan(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_BufferWidth_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.BufferWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.WindowWidth + _target.WindowLeft;

            var original = _target.BufferWidth;
            try
            {
               _target.BufferWidth = expected;
               var actual = _target.BufferWidth;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.BufferWidth = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.BufferWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_zero()
         {
            const Int32 invalidValue = 0;

            Action throwingAction = () => _target.BufferWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_zero()
         {
            var actual = _target.BufferWidth;
            actual.Should().BeGreaterThan(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_CursorLeft_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.CursorLeft = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.BufferWidth - 1;

            var original = _target.CursorLeft;
            try
            {
               _target.CursorLeft = expected;
               var actual = _target.CursorLeft;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.CursorLeft = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.CursorLeft = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_or_equal_to_zero()
         {
            var actual = _target.CursorLeft;
            actual.Should().BeGreaterOrEqualTo(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_CursorSize_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.CursorSize = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("value");
            e.And.Message.Should().Contain("The cursor size is invalid. It must be a percentage between 1 and 100.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = 100;

            var original = _target.CursorSize;
            try
            {
               _target.CursorSize = expected;
               var actual = _target.CursorSize;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.CursorSize = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.CursorSize = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("value");
            e.And.Message.Should().Contain("The cursor size is invalid. It must be a percentage between 1 and 100.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_zero()
         {
            const Int32 invalidValue = 0;

            Action throwingAction = () => _target.CursorSize = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("value");
            e.And.Message.Should().Contain("cursor size is invalid. It must be a percentage between 1 and 100.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_zero()
         {
            var actual = _target.CursorSize;
            actual.Should().BeGreaterThan(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_CursorTop_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.CursorTop = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.BufferWidth - 1;

            var original = _target.CursorTop;
            try
            {
               _target.CursorTop = expected;
               var actual = _target.CursorTop;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.CursorTop = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.CursorTop = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_or_equal_to_zero()
         {
            var actual = _target.CursorTop;
            actual.Should().BeGreaterOrEqualTo(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_CursorVisible_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_it_should_take()
         {
            var expected = !_target.CursorVisible;

            try
            {
               _target.CursorVisible = expected;
               var actual = _target.CursorVisible;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.CursorVisible = !expected;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_should_get_the_value()
         {
            var actual = _target.CursorVisible;
            actual.GetType().Should().Be(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_Error_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_give_me_a_TextWriter()
         {
            Object actual = _target.Error;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(TextWriter));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_ForegroundColor_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_invalid_ConsoleColor_Value_it_should_throw()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.ForegroundColor = (ConsoleColor)invalidValue;
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.EnumType.Should().Be(typeof(ConsoleColor));
            e.And.InvalidValue.Should().Be(invalidValue);
            e.And.Message.Should().Contain("The value of argument 'value' (");
            e.And.Message.Should().Contain(") is invalid for Enum type 'ConsoleColor'");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_valid_ConsoleColor_Value_it_should_take()
         {
            const ConsoleColor expected = ConsoleColor.Magenta;

            try
            {
               _target.ForegroundColor = expected;
               var actual = _target.ForegroundColor;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.ResetColor();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void The_value_is_a_valid_ConsoleColor_Value()
         {
            var actual = _target.ForegroundColor;
            actual.IsValidEnumValue().Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_Input_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_give_me_a_TextReader()
         {
            Object actual = _target.Input;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(TextReader));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_InputEncoding_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_valid_InputEncoding_Value_it_should_take()
         {
            var expected = Encoding.ASCII;

            var original = _target.InputEncoding;
            try
            {
               _target.InputEncoding = expected;
               var actual = _target.InputEncoding;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.InputEncoding = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_null_it_should_throw()
         {
            Action throwingAction = () => _target.InputEncoding = null;
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("value");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void The_value_is_an_Encoding_Value()
         {
            var actual = _target.InputEncoding;
            actual.Should().NotBeNull();
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_IsErrorRedirected_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_Boolean()
         {
            Object actual = _target.IsErrorRedirected;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_IsInputRedirected_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_Boolean()
         {
            Object actual = _target.IsInputRedirected;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_IsOutputRedirected_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_Boolean()
         {
            Object actual = _target.IsOutputRedirected;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_KeyAvailable_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_give_me_a_Boolean()
         {
            Object actual = _target.KeyAvailable;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_LargestWindowHeight_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_an_Int32()
         {
            Object actual = _target.LargestWindowHeight;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Int32));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_LargestWindowWidth_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_an_Int32()
         {
            Object actual = _target.LargestWindowWidth;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(Int32));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_Output_property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_give_me_a_TextWriter()
         {
            Object actual = _target.Output;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(TextWriter));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_OutputEncoding_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_valid_OutputEncoding_Value_it_should_take()
         {
            var expected = Encoding.ASCII;

            var original = _target.OutputEncoding;
            try
            {
               _target.OutputEncoding = expected;
               var actual = _target.OutputEncoding;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.OutputEncoding = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_null_it_should_throw()
         {
            Action throwingAction = () => _target.OutputEncoding = null;
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("value");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void The_value_is_an_Encoding_Value()
         {
            var actual = _target.OutputEncoding;
            actual.Should().NotBeNull();
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_Title_Property : ConsoleTestBase
      {
         private const Int32 MaxChars = 24500;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_string_that_is_too_long_it_should_throw()
         {
            var invalid = new String('a', MaxChars + 1);
            Action throwingAction = () => _target.Title = invalid;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("value");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_a_valid_value_it_should_take()
         {
            var expected = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var original = _target.Title;
            try
            {
               _target.Title = expected;
               var actual = _target.Title;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.Title = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_I_set_the_value_to_null_it_should_throw()
         {
            Action throwingAction = () => _target.Title = null;
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("value");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void The_value_is_a_String_Value()
         {
            var actual = _target.Title;
            actual.Should().NotBeNull();
            actual.Should().BeOfType(typeof(String));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_TreatControlCAsInput_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_valid_value_it_should_take()
         {
            var expected = !_target.TreatControlCAsInput;

            var original = _target.TreatControlCAsInput;
            try
            {
               _target.TreatControlCAsInput = expected;
               var actual = _target.TreatControlCAsInput;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.TreatControlCAsInput = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_a_Boolean_Value()
         {
            var actual = _target.TreatControlCAsInput;
            actual.GetType().Should().Be(typeof(Boolean));
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_WindowHeight_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.WindowHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.WindowHeight + _target.WindowTop;

            var original = _target.WindowHeight;
            try
            {
               _target.WindowHeight = expected;
               var actual = _target.WindowHeight;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.WindowHeight = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.WindowHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_zero()
         {
            const Int32 invalidValue = 0;

            Action throwingAction = () => _target.WindowHeight = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_zero()
         {
            var actual = _target.WindowHeight;
            actual.Should().BeGreaterThan(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_WindowLeft_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.WindowLeft = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.BufferWidth - _target.WindowWidth;

            var original = _target.WindowLeft;
            try
            {
               _target.WindowLeft = expected;
               var actual = _target.WindowLeft;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.WindowLeft = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.WindowLeft = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_or_equal_to_zero()
         {
            var actual = _target.WindowLeft;
            actual.Should().BeGreaterOrEqualTo(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_WindowTop_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.WindowTop = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.BufferHeight - _target.WindowHeight;

            var original = _target.WindowTop;
            try
            {
               _target.WindowTop = expected;
               var actual = _target.WindowTop;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.WindowTop = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.WindowTop = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_or_equal_to_zero()
         {
            var actual = _target.WindowTop;
            actual.Should().BeGreaterOrEqualTo(0);
         }
      }

      [TestClass]
      public class When_I_work_with_the_ConsoleInternalMapping_WindowWidth_Property : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_negative_value()
         {
            const Int32 invalidValue = Int32.MinValue;

            Action throwingAction = () => _target.WindowWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
         {
            var expected = _target.WindowWidth + _target.WindowLeft;

            var original = _target.WindowWidth;
            try
            {
               _target.WindowWidth = expected;
               var actual = _target.WindowWidth;
               actual.Should().Be(expected);
            }
            finally
            {
               _target.WindowWidth = original;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_value_that_is_too_large()
         {
            const Int32 invalidValue = Int32.MaxValue;

            Action throwingAction = () => _target.WindowWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_set_the_value_to_zero()
         {
            const Int32 invalidValue = 0;

            Action throwingAction = () => _target.WindowWidth = invalidValue;
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void The_value_is_greater_than_zero()
         {
            var actual = _target.WindowWidth;
            actual.Should().BeGreaterThan(0);
         }
      }
   }
}
