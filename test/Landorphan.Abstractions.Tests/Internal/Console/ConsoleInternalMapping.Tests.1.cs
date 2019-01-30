namespace Landorphan.Abstractions.Tests.Internal.Console
{
   using System;
   using System.Globalization;
   using System.IO;
   using System.Threading;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.Console;
   using Landorphan.Common;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class ConsoleInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Beep : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.Nightly)]
         public void It_should_Beep()
         {
            // takes 5 seconds on my box
            _target.Beep();
            Thread.Sleep(2 * 1000);
            _target.Beep(37, 3 * 1000);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Clear : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_not_throw()
         {
            // handle invalid thrown when covering with dotCover 3.1.1
            _target.Clear();

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
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
            // handle invalid thrown when covering with dotCover 3.1.1
            _target.MoveBufferArea(1, 1, 1, 1, 2, 2);
            _target.MoveBufferArea(1, 1, 1, 1, 2, 2, 'a', ConsoleColor.Cyan, ConsoleColor.White);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.Manual)]
         public void It_should_work()
         {
            // to do: manually test

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_OpenStandardError : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var x = _target.OpenStandardError();
            x.Should().NotBeNull();
            x.Should().BeAssignableTo<Stream>();

            var y = _target.OpenStandardError(1024);
            y.Should().NotBeNull();
            y.Should().BeAssignableTo<Stream>();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_OpenStandardInput : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var x = _target.OpenStandardInput();
            x.Should().NotBeNull();
            x.Should().BeAssignableTo<Stream>();

            var y = _target.OpenStandardInput(1024);
            y.Should().NotBeNull();
            y.Should().BeAssignableTo<Stream>();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_OpenStandardOutput : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var x = _target.OpenStandardOutput();
            x.Should().NotBeNull();
            x.Should().BeAssignableTo<Stream>();

            var y = _target.OpenStandardOutput(1024);
            y.Should().NotBeNull();
            y.Should().BeAssignableTo<Stream>();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Read : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            // causes a hung test run
            var x = _target.Read();
            x.Should().BeGreaterOrEqualTo(-1);
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_ReadKey : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void It_should_not_throw()
         {
            // causes a hung test run
            var x = _target.ReadKey();
            TestHelp.DoNothing(x);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_ReadLine : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            // causes a hung test run
            var x = _target.ReadLine();
            TestHelp.DoNothing(x);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_ResetColor : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            _target.ResetColor();

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
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
            // handle invalid thrown when covering with dotCover 3.1.1
            var validWidth = _target.BufferWidth;
            var invalidHeight = Int16.MaxValue + 1;

            Action throwingAction = () => _target.SetBufferSize(validWidth, invalidHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_height_is_less_than_windows_top_plus_windows_height_it_should_throw()
         {
            // handle invalid thrown when covering with dotCover 3.1.1
            var validWidth = _target.BufferWidth;
            var invalidHeight = _target.WindowTop + _target.WindowHeight - 1;

            Action throwingAction = () => _target.SetBufferSize(validWidth, invalidHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_height_is_less_than_zero_it_should_throw()
         {
            // handle invalid thrown when covering with dotCover 3.1.1
            var validWidth = _target.BufferWidth;
            var invalidHeight = -1;

            Action throwingAction = () => _target.SetBufferSize(validWidth, invalidHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_use_valid_values_it_should_take()
         {
            // handle invalid thrown when covering with dotCover 3.1.1
            var originalWidth = _target.BufferWidth;
            var originalHeight = _target.BufferHeight;
            try
            {
               var expectedWidth = Int16.MaxValue - 1;
               var expectedHeight = Int16.MaxValue - 1;
               _target.SetBufferSize(expectedWidth, expectedHeight);
               _target.BufferWidth.Should().Be(expectedWidth);
               _target.BufferHeight.Should().Be(expectedHeight);
            }
            finally
            {
               _target.SetBufferSize(originalWidth, originalHeight);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_width_is_greater_than_int16_max_value_it_should_throw()
         {
            var validHeight = _target.BufferHeight;
            var invalidWidth = Int16.MaxValue + 1;

            Action throwingAction = () => _target.SetBufferSize(invalidWidth, validHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_width_is_less_than_windows_left_plus_windows_width_it_should_throw()
         {
            var validHeight = _target.BufferHeight;
            var invalidWidth = _target.WindowLeft + _target.WindowWidth - 1;

            Action throwingAction = () => _target.SetBufferSize(invalidWidth, validHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_width_is_less_than_zero_it_should_throw()
         {
            var validHeight = _target.BufferHeight;
            var invalidWidth = -1;

            Action throwingAction = () => _target.SetBufferSize(invalidWidth, validHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should()
               .Contain(
                  "The console buffer size must not be less than the current size and position of the console window, nor greater than or equal to Int16.MaxValue.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetCursorPosition : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_use_valid_values_it_should_take()
         {
            var originalLeft = _target.CursorLeft;
            var originalTop = _target.CursorTop;
            try
            {
               var expectedLeft = _target.BufferWidth - 1;
               var expectedTop = _target.BufferHeight - 1;
               _target.SetCursorPosition(expectedLeft, expectedTop);
               _target.CursorLeft.Should().Be(expectedLeft);
               _target.CursorTop.Should().Be(expectedTop);
            }
            finally
            {
               _target.SetCursorPosition(originalLeft, originalTop);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_left_is_greater_than_BufferWidth_it_should_throw()
         {
            var validTop = _target.CursorTop;
            var invalidLeft = _target.BufferWidth + 1;

            Action throwingAction = () => _target.SetCursorPosition(invalidLeft, validTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_left_is_less_than_zero_it_should_throw()
         {
            var validTop = _target.CursorTop;
            var invalidLeft = -1;

            Action throwingAction = () => _target.SetCursorPosition(invalidLeft, validTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_top_is_greater_than_BufferHeight_it_should_throw()
         {
            var validLeft = _target.CursorLeft;
            var invalidTop = _target.BufferHeight + 1;

            Action throwingAction = () => _target.SetCursorPosition(validLeft, invalidTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_top_is_less_than_zero_it_should_throw()
         {
            var validLeft = _target.CursorLeft;
            var invalidTop = -1;

            Action throwingAction = () => _target.SetCursorPosition(validLeft, invalidTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should().Contain("The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetError : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var original = _target.Error;
            try
            {
               var newError = DisposableHelper.SafeCreate(() => new StreamWriter(DisposableHelper.SafeCreate<MemoryStream>()));
               _target.SetError(newError);
            }
            finally
            {
               _target.SetError(original);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.SetError(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("newError");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetInput : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var original = _target.Input;
            try
            {
               var newError = DisposableHelper.SafeCreate(() => new StreamReader(DisposableHelper.SafeCreate<MemoryStream>()));
               _target.SetInput(newError);
            }
            finally
            {
               _target.SetInput(original);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.SetInput(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("newIn");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetOutput : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var original = _target.Output;
            try
            {
               var newError = DisposableHelper.SafeCreate(() => new StreamWriter(DisposableHelper.SafeCreate<MemoryStream>()));
               _target.SetOutput(newError);
            }
            finally
            {
               _target.SetError(original);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.SetOutput(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("newOut");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetWindowPosition : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_use_valid_values_it_should_take()
         {
            var originalLeft = _target.WindowLeft;
            var originalTop = _target.WindowTop;
            var originalBufferHeight = _target.BufferHeight;
            var originalBufferWidth = _target.BufferWidth;

            try
            {
               _target.BufferHeight = Int16.MaxValue - 1;
               _target.BufferWidth = Int16.MaxValue - 1;

               var expectedLeft = _target.WindowLeft + 1;
               var expectedTop = _target.WindowTop + 1;
               _target.SetWindowPosition(expectedLeft, expectedTop);
               _target.WindowLeft.Should().Be(expectedLeft);
               _target.WindowTop.Should().Be(expectedTop);
            }
            finally
            {
               _target.SetWindowPosition(originalLeft, originalTop);
               _target.BufferHeight = originalBufferHeight;
               _target.BufferWidth = originalBufferWidth;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_left_is_greater_than_BufferWidth_it_should_throw()
         {
            var validTop = _target.WindowTop;
            var invalidLeft = _target.BufferWidth + 1;

            Action throwingAction = () => _target.SetWindowPosition(invalidLeft, validTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_left_is_less_than_zero_it_should_throw()
         {
            var validTop = _target.WindowTop;
            var invalidLeft = -1;

            Action throwingAction = () => _target.SetWindowPosition(invalidLeft, validTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("left");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_top_is_greater_than_BufferHeight_it_should_throw()
         {
            var validLeft = _target.WindowLeft;
            var invalidTop = _target.BufferHeight + 1;

            Action throwingAction = () => _target.SetWindowPosition(validLeft, invalidTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_top_is_less_than_zero_it_should_throw()
         {
            var validLeft = _target.WindowLeft;
            var invalidTop = -1;

            Action throwingAction = () => _target.SetWindowPosition(validLeft, invalidTop);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("top");
            e.And.Message.Should()
               .Contain(
                  "The window position must be set such that the current window size fits within the console's buffer, and the numbers must not be negative.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_SetWindowSize : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_height_is_less_than_zero_it_should_throw()
         {
            var validWidth = _target.WindowWidth;
            var invalidHeight = -1;

            Action throwingAction = () => _target.SetWindowSize(validWidth, invalidHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_height_is_too_large_it_should_throw()
         {
            // too large:  The value must be less than the console's current maximum window size of (???) in that dimension. 
            // Note that this value depends on screen resolution and the console font.
            var validWidth = _target.WindowWidth;
            var invalidHeight = Int16.MaxValue + 1;

            Action throwingAction = () => _target.SetWindowSize(validWidth, invalidHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("height");
            e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_I_use_valid_values_it_should_take()
         {
            var originalWidth = _target.WindowWidth;
            var originalHeight = _target.WindowHeight;
            try
            {
               var expectedWidth = _target.LargestWindowWidth;
               var expectedHeight = _target.LargestWindowHeight;
               _target.SetWindowSize(expectedWidth, expectedHeight);
               _target.WindowWidth.Should().Be(expectedWidth);
               _target.WindowHeight.Should().Be(expectedHeight);
            }
            finally
            {
               _target.SetWindowSize(originalWidth, originalHeight);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_width_is_less_than_zero_it_should_throw()
         {
            var validHeight = _target.WindowHeight;
            var invalidWidth = -1;

            Action throwingAction = () => _target.SetWindowSize(invalidWidth, validHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should().Contain("Positive number required.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_width_is_too_large_it_should_throw()
         {
            // too large:  The value must be less than the console's current maximum window size of (???) in that dimension. 
            // Note that this value depends on screen resolution and the console font.
            var validHeight = _target.WindowHeight;
            var invalidWidth = Int16.MaxValue + 1;

            Action throwingAction = () => _target.SetWindowSize(invalidWidth, validHeight);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("width");
            e.And.Message.Should().Contain("The new console window size would force the console buffer size to be too large.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Boolean : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            _target.Write(true);
            _target.Write(false);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Char : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 'a';
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Char_Array : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Char[] value = {'a', 'b', 'c'};
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Char_Array_Index_Count : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Char[] value = {'a', 'b', 'c'};
            _target.Write(value, 1, 2);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Decimal : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = new Decimal(123.45);
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Double : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123.45;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_formatted_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}' ";
            var value = new Object();
            _target.Write(format, value);

            value = null;
            _target.Write(format, value);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value = new Object();

            Action throwingAction = () => _target.Write(null, value);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Array : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}', '{1}', '{2}', '{3}' ";
            Object[] value = {new Object(), new Object(), new Object(), new Object()};
            _target.Write(format, value);

            format = String.Empty;
            value = null;
            _target.Write(format, value);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            Object[] value = {new Object(), new Object(), new Object(), new Object()};

            Action throwingAction = () => _target.Write(null, value);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_there_is_an_index_in_the_format_string_that_is_out_of_range_of_values_supplied()
         {
            var format = "{100}";
            Object[] value = {new Object()};

            Action throwingAction = () => _target.Write(format, value);
            var e = throwingAction.Should().Throw<FormatException>();
            e.And.Message.Should().Contain("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}', '{1}' ";
            var value0 = new Object();
            var value1 = new Object();
            _target.Write(format, value0, value1);

            value0 = null;
            value1 = null;
            _target.Write(format, value0, value1);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value0 = new Object();
            var value1 = new Object();

            Action throwingAction = () => _target.Write(null, value0, value1);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Object_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            const String format = " '{0}', '{1}', '{2}' ";
            var value0 = new Object();
            var value1 = new Object();
            var value2 = new Object();
            _target.Write(format, value0, value1, value2);

            value0 = null;
            value1 = null;
            value2 = null;
            _target.Write(format, value0, value1, value2);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value0 = new Object();
            var value1 = new Object();
            var value2 = new Object();

            Action throwingAction = () => _target.Write(null, value0, value1, value2);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Int32 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Int64 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Int64 value = 123;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = new Object();
            _target.Write(value);

            value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_Single : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123.45f;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_String : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.Write(value);

            value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_UInt32 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            UInt32 value = 123;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_Write_UInt64 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            UInt64 value = 123;
            _target.Write(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Boolean : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            _target.WriteLine(true);
            _target.WriteLine(false);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Char : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 'a';
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Char_Array : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Char[] value = {'a', 'b', 'c'};
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Char_Array_Index_Count : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Char[] value = {'a', 'b', 'c'};
            _target.WriteLine(value, 1, 2);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Decimal : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = new Decimal(123.45);
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Double : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123.45;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Empty : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            _target.WriteLine();

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}' ";
            var value = new Object();
            _target.WriteLine(format, value);

            value = null;
            _target.WriteLine(format, value);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value = new Object();

            Action throwingAction = () => _target.WriteLine(null, value);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Array : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}', '{1}', '{2}', '{3}' ";
            Object[] value = {new Object(), new Object(), new Object(), new Object()};
            _target.WriteLine(format, value);

            format = String.Empty;
            value = null;
            _target.WriteLine(format, value);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            Object[] value = {new Object(), new Object(), new Object(), new Object()};

            Action throwingAction = () => _target.WriteLine(null, value);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_there_is_an_index_in_the_format_string_that_is_out_of_range_of_values_supplied()
         {
            var format = "{100}";
            Object[] value = {new Object()};

            Action throwingAction = () => _target.WriteLine(format, value);
            var e = throwingAction.Should().Throw<FormatException>();
            e.And.Message.Should().Contain("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var format = " '{0}', '{1}' ";
            var value0 = new Object();
            var value1 = new Object();
            _target.WriteLine(format, value0, value1);

            value0 = null;
            value1 = null;
            _target.WriteLine(format, value0, value1);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value0 = new Object();
            var value1 = new Object();

            Action throwingAction = () => _target.WriteLine(null, value0, value1);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Object_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_format_is_not_null()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            const String format = " '{0}', '{1}', '{2}' ";
            var value0 = new Object();
            var value1 = new Object();
            var value2 = new Object();
            _target.WriteLine(format, value0, value1, value2);

            value0 = null;
            value1 = null;
            value2 = null;
            _target.WriteLine(format, value0, value1, value2);

            // ReSharper restore ExpressionIsAlwaysNull

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_format_is_null()
         {
            var value0 = new Object();
            var value1 = new Object();
            var value2 = new Object();

            Action throwingAction = () => _target.WriteLine(null, value0, value1, value2);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("format");
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Int32 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Int64 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            Int64 value = 123;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Object : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = new Object();
            _target.WriteLine(value);

            value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_Single : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = 123.45f;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_String : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            var value = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.WriteLine(value);

            value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_UInt32 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            UInt32 value = 123;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_ConsoleInternalMapping_WriteLine_UInt64 : ConsoleTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw()
         {
            UInt64 value = 123;
            _target.WriteLine(value);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }
      }
   }
}
