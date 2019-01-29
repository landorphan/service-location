namespace Landorphan.Abstractions.Tests.Console
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Abstractions.Console;
   using Landorphan.Abstractions.Console.Interfaces;
   using Landorphan.Common;
   using Landorphan.Common.Exceptions;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class ConsoleUtilities_Tests
   {
      public static class ConsoleAppearance_Tests
      {
         [TestClass]
         public class ConsoleAppearance_BackgroundProperty_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_get_the_value_it_is_a_valid_ConsoleColor_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.BackgroundColor;
                  actual.IsValidEnumValue().Should().BeTrue();
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_invalid_ConsoleColor_Value_it_should_throw()
            {
               const Int32 invalidValue = Int32.MaxValue;

               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.BackgroundColor = (ConsoleColor)invalidValue;
                  var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
                  e.And.EnumType.Should().Be(typeof(ConsoleColor));
                  e.And.InvalidValue.Should().Be(invalidValue);
                  e.And.Message.Should().Contain("The value of argument 'value' (");
                  e.And.Message.Should().Contain(") is invalid for Enum type 'ConsoleColor'");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_valid_ConsoleColor_Value_it_should_take()
            {
               const ConsoleColor expected = ConsoleColor.Magenta;

               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  try
                  {
                     target.BackgroundColor = expected;
                     var actual = target.BackgroundColor;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.ResetColor();
                  }
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_ForegroundProperty_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_get_the_value_it_is_a_valid_ConsoleColor_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.ForegroundColor;
                  actual.IsValidEnumValue().Should().BeTrue();
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_invalid_ConsoleColor_Value_it_should_throw()
            {
               const Int32 invalidValue = Int32.MaxValue;

               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.ForegroundColor = (ConsoleColor)invalidValue;
                  var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
                  e.And.EnumType.Should().Be(typeof(ConsoleColor));
                  e.And.InvalidValue.Should().Be(invalidValue);
                  e.And.Message.Should().Contain("The value of argument 'value' (");
                  e.And.Message.Should().Contain(") is invalid for Enum type 'ConsoleColor'");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_valid_ConsoleColor_Value_it_should_take()
            {
               const ConsoleColor expected = ConsoleColor.Magenta;

               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  try
                  {
                     target.ForegroundColor = expected;
                     var actual = target.ForegroundColor;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.ResetColor();
                  }
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_LargestWindowHeight_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_an_Int32()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Object actual = target.LargestWindowHeight;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Int32));
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_LargestWindowWidth_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_an_Int32()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Object actual = target.LargestWindowWidth;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Int32));
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_Title_Tests : ConsoleTestBase
         {
            private const Int32 MaxChars = 24500;

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            // This test does not work in .Net 4.6
            // [Ignore]
            public void And_I_get_the_value_it_is_a_String_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.Title;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(String));
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_string_that_is_too_long_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var invalid = new String('a', MaxChars + 1);
                  Action throwingAction = () => target.Title = invalid;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("value");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            // This test does not work in .Net 4.6
            // [Ignore]
            public void And_I_set_the_value_to_a_valid_value_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var expected = Guid.NewGuid().ToString();

                  var original = target.Title;
                  try
                  {
                     target.Title = expected;
                     var actual = target.Title;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.Title = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_null_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.Title = null;
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("value");
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_WindowHeight_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_get_the_value_it_is_greater_than_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.WindowHeight;
                  actual.Should().BeGreaterThan(0);
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = Int32.MinValue;

                  Action throwingAction = () => target.WindowHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var expected = target.WindowHeight + target.WindowTop;

                  var original = target.WindowHeight;
                  try
                  {
                     target.WindowHeight = expected;
                     var actual = target.WindowHeight;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.WindowHeight = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = Int32.MaxValue;

                  Action throwingAction = () => target.WindowHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = 0;

                  Action throwingAction = () => target.WindowHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_WindowLeft_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.WindowLeft = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var expected = full.BufferWidth - target.WindowWidth;

                  var original = target.WindowLeft;
                  try
                  {
                     target.WindowLeft = expected;
                     var actual = target.WindowLeft;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.WindowLeft = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = Int32.MaxValue;

                  Action throwingAction = () => target.WindowLeft = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_or_equal_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.WindowLeft;
                  actual.Should().BeGreaterOrEqualTo(0);
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_WindowTop_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.WindowTop = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var expected = full.BufferHeight - target.WindowHeight;

                  var original = target.WindowTop;
                  try
                  {
                     target.WindowTop = expected;
                     var actual = target.WindowTop;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.WindowTop = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               const Int32 invalidValue = Int32.MaxValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  Action throwingAction = () => target.WindowTop = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_or_equal_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.WindowTop;
                  actual.Should().BeGreaterOrEqualTo(0);
               }
            }
         }

         [TestClass]
         public class ConsoleAppearance_WindowWidth_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_get_the_value_it_is_greater_than_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var actual = target.WindowWidth;
                  actual.Should().BeGreaterThan(0);
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = Int32.MinValue;

                  Action throwingAction = () => target.WindowWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var expected = target.WindowWidth + target.WindowLeft;

                  var original = target.WindowWidth;
                  try
                  {
                     target.WindowWidth = expected;
                     var actual = target.WindowWidth;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.WindowWidth = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = Int32.MaxValue;

                  Action throwingAction = () => target.WindowWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  const Int32 invalidValue = 0;

                  Action throwingAction = () => target.WindowWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleAppearance_Clear : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  target.Clear();
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }

         [TestClass]
         public class When_I_call_ConsoleAppearance_ResetColor : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  target.ResetColor();
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }

         [TestClass]
         public class When_I_call_ConsoleAppearance_SetWindowPosition : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_use_valid_values_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var originalLeft = target.WindowLeft;
                  var originalTop = target.WindowTop;
                  var originalBufferHeight = full.BufferHeight;
                  var originalBufferWidth = full.BufferWidth;

                  try
                  {
                     full.BufferHeight = Int16.MaxValue - 1;
                     full.BufferWidth = Int16.MaxValue - 1;

                     var expectedLeft = target.WindowLeft + 1;
                     var expectedTop = target.WindowTop + 1;
                     target.SetWindowPosition(expectedLeft, expectedTop);
                     target.WindowLeft.Should().Be(expectedLeft);
                     target.WindowTop.Should().Be(expectedTop);
                  }
                  finally
                  {
                     target.SetWindowPosition(originalLeft, originalTop);
                     full.BufferHeight = originalBufferHeight;
                     full.BufferWidth = originalBufferWidth;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_left_is_greater_than_BufferWidth_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validTop = target.WindowTop;
                  var invalidLeft = full.BufferWidth + 1;

                  Action throwingAction = () => target.SetWindowPosition(invalidLeft, validTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_left_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validTop = target.WindowTop;
                  var invalidLeft = -1;

                  Action throwingAction = () => target.SetWindowPosition(invalidLeft, validTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_top_is_greater_than_BufferHeight_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validLeft = target.WindowLeft;
                  var invalidTop = full.BufferHeight + 1;

                  Action throwingAction = () => target.SetWindowPosition(validLeft, invalidTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_top_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validLeft = target.WindowLeft;
                  var invalidTop = -1;

                  Action throwingAction = () => target.SetWindowPosition(validLeft, invalidTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleAppearance_SetWindowSize : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_height_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validWidth = target.WindowWidth;
                  var invalidHeight = -1;

                  Action throwingAction = () => target.SetWindowSize(validWidth, invalidHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_height_is_too_large_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;

                  // too large:  The value must be less than the console's current maximum window size of (???) in that dimension. 
                  // Note that this value depends on screen resolution and the console font.
                  var validWidth = target.WindowWidth;
                  var invalidHeight = Int16.MaxValue + 1;

                  Action throwingAction = () => target.SetWindowSize(validWidth, invalidHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_use_valid_values_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var originalWidth = target.WindowWidth;
                  var originalHeight = target.WindowHeight;
                  try
                  {
                     var expectedWidth = target.LargestWindowWidth;
                     var expectedHeight = target.LargestWindowHeight;
                     target.SetWindowSize(expectedWidth, expectedHeight);
                     target.WindowWidth.Should().Be(expectedWidth);
                     target.WindowHeight.Should().Be(expectedHeight);
                  }
                  finally
                  {
                     target.SetWindowSize(originalWidth, originalHeight);
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_width_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;
                  var validHeight = target.WindowHeight;
                  var invalidWidth = -1;

                  Action throwingAction = () => target.SetWindowSize(invalidWidth, validHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should().Contain("Positive number required.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_width_is_too_large_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleAppearance target = full;

                  // too large:  The value must be less than the console's current maximum window size of (???) in that dimension. 
                  // Note that this value depends on screen resolution and the console font.
                  var validHeight = target.WindowHeight;
                  var invalidWidth = Int16.MaxValue + 1;

                  Action throwingAction = () => target.SetWindowSize(invalidWidth, validHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
               }
            }
         }
      }

      public static class ConsoleBuffer_Tests
      {
         [TestClass]
         public class ConsoleBuffer_BufferHeight_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var expected = full.WindowHeight + full.WindowTop;

                  var original = target.BufferHeight;
                  try
                  {
                     target.BufferHeight = expected;
                     var actual = target.BufferHeight;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.BufferHeight = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               const Int32 invalidValue = Int32.MaxValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_zero()
            {
               const Int32 invalidValue = 0;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferHeight = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var actual = target.BufferHeight;
                  actual.Should().BeGreaterThan(0);
               }
            }
         }

         [TestClass]
         public class ConsoleBuffer_BufferWidth_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var expected = full.WindowWidth + full.WindowLeft;

                  var original = target.BufferWidth;
                  try
                  {
                     target.BufferWidth = expected;
                     var actual = target.BufferWidth;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.BufferWidth = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               const Int32 invalidValue = Int32.MaxValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_zero()
            {
               const Int32 invalidValue = 0;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  Action throwingAction = () => target.BufferWidth = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var actual = target.BufferWidth;
                  actual.Should().BeGreaterThan(0);
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_MoveBufferArea : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  target.MoveBufferArea(1, 1, 1, 1, 2, 2);
                  target.MoveBufferArea(1, 1, 1, 1, 2, 2, 'a', ConsoleColor.Cyan, ConsoleColor.White);
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.Manual)]
            [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Placeholder for manual test")]
            public void It_should_work()
            {
               // TODO: manually test
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_SetBufferSize : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_height_is_greater_than_int16_max_value_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  var validWidth = target.BufferWidth;
                  var invalidHeight = Int16.MaxValue + 1;

                  Action throwingAction = () => target.SetBufferSize(validWidth, invalidHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_height_is_less_than_windows_top_plus_windows_height_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  var validWidth = target.BufferWidth;
                  var invalidHeight = full.WindowTop + full.WindowHeight - 1;

                  Action throwingAction = () => target.SetBufferSize(validWidth, invalidHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_height_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  var validWidth = target.BufferWidth;
                  var invalidHeight = -1;

                  Action throwingAction = () => target.SetBufferSize(validWidth, invalidHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("height");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_use_valid_values_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;

                  // handle invalid thrown when covering with dotCover 3.1.1
                  var originalWidth = target.BufferWidth;
                  var originalHeight = target.BufferHeight;
                  try
                  {
                     var expectedWidth = Int16.MaxValue - 1;
                     var expectedHeight = Int16.MaxValue - 1;
                     target.SetBufferSize(expectedWidth, expectedHeight);
                     target.BufferWidth.Should().Be(expectedWidth);
                     target.BufferHeight.Should().Be(expectedHeight);
                  }
                  finally
                  {
                     target.SetBufferSize(originalWidth, originalHeight);
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_width_is_greater_than_int16_max_value_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var validHeight = target.BufferHeight;
                  var invalidWidth = Int16.MaxValue + 1;

                  Action throwingAction = () => target.SetBufferSize(invalidWidth, validHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_width_is_less_than_windows_left_plus_windows_width_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var validHeight = target.BufferHeight;
                  var invalidWidth = full.WindowLeft + full.WindowWidth - 1;

                  Action throwingAction = () => target.SetBufferSize(invalidWidth, validHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_width_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleBuffer target = full;
                  var validHeight = target.BufferHeight;
                  var invalidWidth = -1;

                  Action throwingAction = () => target.SetBufferSize(invalidWidth, validHeight);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("width");
                  e.And.Message.Should()
                     .Contain(
                        "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
               }
            }
         }
      }
   }
}
