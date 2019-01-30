namespace Landorphan.Abstractions.Tests.Console
{
   using System;
   using System.IO;
   using System.Text;
   using System.Threading;
   using FluentAssertions;
   using Landorphan.Abstractions.Console;
   using Landorphan.Abstractions.Console.Interfaces;
   using Landorphan.Common;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class ConsoleUtilities_Tests
   {
      public static class ConsoleCursor_Tests
      {
         [TestClass]
         public class ConsoleCursor_CursorLeft_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorLeft = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var expected = full.BufferWidth - 1;

                  var original = target.CursorLeft;
                  try
                  {
                     target.CursorLeft = expected;
                     var actual = target.CursorLeft;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.CursorLeft = original;
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
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorLeft = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_or_equal_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var actual = target.CursorLeft;
                  actual.Should().BeGreaterOrEqualTo(0);
               }
            }
         }

         [TestClass]
         public class ConsoleCursor_CursorSize_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;

               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;

                  Action throwingAction = () => target.CursorSize = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("value");
                  e.And.Message.Should().Contain("The cursor size is invalid. It must be a percentage between 1 and 100.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               var expected = 100;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var original = target.CursorSize;
                  try
                  {
                     target.CursorSize = expected;
                     var actual = target.CursorSize;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.CursorSize = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_value_that_is_too_large()
            {
               const Int32 invalidValue = Int32.MaxValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorSize = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("value");
                  e.And.Message.Should().Contain("The cursor size is invalid. It must be a percentage between 1 and 100.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_zero()
            {
               const Int32 invalidValue = 0;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorSize = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("value");
                  e.And.Message.Should().Contain("cursor size is invalid. It must be a percentage between 1 and 100.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var actual = target.CursorSize;
                  actual.Should().BeGreaterThan(0);
               }
            }
         }

         [TestClass]
         public class ConsoleCursor_CursorTop_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_negative_value()
            {
               const Int32 invalidValue = Int32.MinValue;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorTop = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_positive_number_that_is_not_too_large()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var expected = full.BufferWidth - 1;

                  var original = target.CursorTop;
                  try
                  {
                     target.CursorTop = expected;
                     var actual = target.CursorTop;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.CursorTop = original;
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
                  IConsoleCursor target = full;
                  Action throwingAction = () => target.CursorTop = invalidValue;
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_greater_than_or_equal_to_zero()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var actual = target.CursorTop;
                  actual.Should().BeGreaterOrEqualTo(0);
               }
            }
         }

         [TestClass]
         public class ConsoleCursor_CursorVisible_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_value_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var expected = !target.CursorVisible;

                  try
                  {
                     target.CursorVisible = expected;
                     var actual = target.CursorVisible;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.CursorVisible = !expected;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_should_get_the_value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var actual = target.CursorVisible;
                  actual.GetType().Should().Be(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleCursor_SetCursorPosition : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_use_valid_values_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var originalLeft = target.CursorLeft;
                  var originalTop = target.CursorTop;
                  try
                  {
                     var expectedLeft = full.BufferWidth - 1;
                     var expectedTop = full.BufferHeight - 1;
                     target.SetCursorPosition(expectedLeft, expectedTop);
                     target.CursorLeft.Should().Be(expectedLeft);
                     target.CursorTop.Should().Be(expectedTop);
                  }
                  finally
                  {
                     target.SetCursorPosition(originalLeft, originalTop);
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
                  IConsoleCursor target = full;
                  var validTop = target.CursorTop;
                  var invalidLeft = full.BufferWidth + 1;

                  Action throwingAction = () => target.SetCursorPosition(invalidLeft, validTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_left_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var validTop = target.CursorTop;
                  var invalidLeft = -1;

                  Action throwingAction = () => target.SetCursorPosition(invalidLeft, validTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("left");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_top_is_greater_than_BufferHeight_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var validLeft = target.CursorLeft;
                  var invalidTop = full.BufferHeight + 1;

                  Action throwingAction = () => target.SetCursorPosition(validLeft, invalidTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_top_is_less_than_zero_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleCursor target = full;
                  var validLeft = target.CursorLeft;
                  var invalidTop = -1;

                  Action throwingAction = () => target.SetCursorPosition(validLeft, invalidTop);
                  var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
                  e.And.ParamName.Should().Be("top");
                  e.And.Message.Should()
                     .Contain(
                        "The value must be greater than or equal to zero and less than the console's buffer size in that dimension.");
               }
            }
         }
      }

      public static class ConsoleMisc_Tests
      {
         [TestClass]
         public class When_I_call_ConsoleMisc_Beep : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.Nightly)]
            public void It_should_Beep()
            {
               // takes 5 seconds on my box
               using (var full = new ConsoleUtilities())
               {
                  IConsoleMisc target = full;
                  target.Beep();
                  Thread.Sleep(2 * 1000);
                  target.Beep(37, 3 * 1000);
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }
      }

      public static class ConsoleReader_Tests
      {
         [TestClass]
         public class ConsoleReader_KeyAvailable_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_give_me_a_Boolean()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleReader target = full;
                  Object actual = target.KeyAvailable;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleReader_Read : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleReader target = full;

                  // causes a hung test run
                  var x = target.Read();
                  x.Should().BeGreaterOrEqualTo(-1);
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleReader_ReadKey : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleReader target = full;

                  // causes a hung test run
                  var x = target.ReadKey();
                  TestHelp.DoNothing(x);
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }

         [TestClass]
         public class When_I_call_ConsoleReader_ReadLine : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleReader target = full;

                  // causes a hung test run
                  var x = target.ReadLine();
                  TestHelp.DoNothing(x);
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }
      }

      public static class ConsoleStreams_Tests
      {
         [TestClass]
         public class ConsoleStreams_Error_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_give_me_a_TextWriter()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.Error;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(TextWriter));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_Input_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_give_me_a_TextReader()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.Input;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(TextReader));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_InputEncoding_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_valid_InputEncoding_Value_it_should_take()
            {
               var expected = Encoding.ASCII;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var original = target.InputEncoding;
                  try
                  {
                     target.InputEncoding = expected;
                     var actual = target.InputEncoding;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.InputEncoding = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_null_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Action throwingAction = () => target.InputEncoding = null;
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("value");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void The_value_is_an_Encoding_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var actual = target.InputEncoding;
                  actual.Should().NotBeNull();
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_IsErrorRedirected_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_Boolean()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.IsErrorRedirected;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_IsInputRedirected_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_Boolean()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.IsInputRedirected;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_IsOutputRedirected_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_Boolean()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.IsOutputRedirected;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_Output_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_give_me_a_TextWriter()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Object actual = target.Output;
                  actual.Should().NotBeNull();
                  actual.Should().BeOfType(typeof(TextWriter));
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_OutputEncoding_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_a_valid_OutputEncoding_Value_it_should_take()
            {
               var expected = Encoding.ASCII;
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var original = target.OutputEncoding;
                  try
                  {
                     target.OutputEncoding = expected;
                     var actual = target.OutputEncoding;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.OutputEncoding = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_set_the_value_to_null_it_should_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Action throwingAction = () => target.OutputEncoding = null;
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("value");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void The_value_is_an_Encoding_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var actual = target.OutputEncoding;
                  actual.Should().NotBeNull();
               }
            }
         }

         [TestClass]
         public class ConsoleStreams_TreatControlCAsInput_Tests : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void And_I_set_the_value_to_a_valid_value_it_should_take()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var expected = !target.TreatControlCAsInput;

                  var original = target.TreatControlCAsInput;
                  try
                  {
                     target.TreatControlCAsInput = expected;
                     var actual = target.TreatControlCAsInput;
                     actual.Should().Be(expected);
                  }
                  finally
                  {
                     target.TreatControlCAsInput = original;
                  }
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void The_value_is_a_Boolean_Value()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var actual = target.TreatControlCAsInput;

                  actual.GetType().Should().Be(typeof(Boolean));
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_OpenStandardError : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var x = target.OpenStandardError();
                  x.Should().NotBeNull();
                  x.Should().BeOfType<Stream>();

                  var y = target.OpenStandardError(1024);
                  y.Should().NotBeNull();
                  y.Should().BeOfType<Stream>();
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_OpenStandardInput : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var x = target.OpenStandardInput();
                  x.Should().NotBeNull();
                  x.Should().BeOfType<Stream>();

                  var y = target.OpenStandardInput(1024);
                  y.Should().NotBeNull();
                  y.Should().BeOfType<Stream>();
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_OpenStandardOutput : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [Ignore("failing in .Net Standard 2.0")]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var x = target.OpenStandardOutput();
                  x.Should().NotBeNull();
                  x.Should().BeOfType<Stream>();

                  var y = target.OpenStandardOutput(1024);
                  y.Should().NotBeNull();
                  y.Should().BeOfType<Stream>();
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_SetError : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var original = target.Error;
                  try
                  {
                     var newError = DisposableHelper.SafeCreate(() => new StreamWriter(DisposableHelper.SafeCreate<MemoryStream>()));
                     target.SetError(newError);
                  }
                  finally
                  {
                     target.SetError(original);
                  }
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_ArgumentNullException()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Action throwingAction = () => target.SetError(null);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("newError");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_SetInput : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var original = target.Input;
                  try
                  {
                     var newError = DisposableHelper.SafeCreate(() => new StreamReader(DisposableHelper.SafeCreate<MemoryStream>()));
                     target.SetInput(newError);
                  }
                  finally
                  {
                     target.SetInput(original);
                  }
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_ArgumentNullException()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Action throwingAction = () => target.SetInput(null);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("newIn");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleStreams_SetOutput : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  var original = target.Output;
                  try
                  {
                     var newError = DisposableHelper.SafeCreate(() => new StreamWriter(DisposableHelper.SafeCreate<MemoryStream>()));
                     target.SetOutput(newError);
                  }
                  finally
                  {
                     target.SetError(original);
                  }
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_ArgumentNullException()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleStreams target = full;
                  Action throwingAction = () => target.SetOutput(null);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("newOut");
               }
            }
         }
      }
   }
}
