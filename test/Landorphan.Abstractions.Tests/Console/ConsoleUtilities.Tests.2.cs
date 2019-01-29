namespace Landorphan.Abstractions.Tests.Console
{
   using System;
   using FluentAssertions;
   using Landorphan.Abstractions.Console;
   using Landorphan.Abstractions.Console.Interfaces;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class ConsoleUtilities_Tests
   {
      public static class ConsoleWriter_Tests
      {
         [TestClass]
         public class When_I_call_ConsoleInternalMapping_Write_Boolean : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  target.Write(true);
                  target.Write(false);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 'a';
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Char[] value = {'a', 'b', 'c'};
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Char[] value = {'a', 'b', 'c'};
                  target.Write(value, 1, 2);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Decimal(123.45);
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123.45;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}' ";
                  var value = new Object();
                  target.Write(format, value);

                  value = null;
                  target.Write(format, value);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Object();

                  Action throwingAction = () => target.Write(null, value);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Array : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}', '{1}', '{2}', '{3}' ";
                  Object[] value = {new Object(), new Object(), new Object(), new Object()};
                  target.Write(format, value);

                  format = String.Empty;
                  value = null;
                  target.Write(format, value);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Object[] value = {new Object(), new Object(), new Object(), new Object()};

                  Action throwingAction = () => target.Write(null, value);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_there_is_an_index_in_the_format_string_that_is_out_of_range_of_values_supplied()
            {
               var format = "{100}";
               Object[] value = {new Object()};
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Action throwingAction = () => target.Write(format, value);
                  var e = throwingAction.Should().Throw<FormatException>();
                  e.And.Message.Should().Contain("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Object : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}', '{1}' ";
                  var value0 = new Object();
                  var value1 = new Object();
                  target.Write(format, value0, value1);

                  value0 = null;
                  value1 = null;
                  target.Write(format, value0, value1);

                  // ReSharper restore ExpressionIsAlwaysNull

                  TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               var value0 = new Object();
               var value1 = new Object();
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Action throwingAction = () => target.Write(null, value0, value1);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_Write_formatted_Object_Object_Object : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  const String format = " '{0}', '{1}', '{2}' ";
                  var value0 = new Object();
                  var value1 = new Object();
                  var value2 = new Object();
                  target.Write(format, value0, value1, value2);

                  value0 = null;
                  value1 = null;
                  value2 = null;
                  target.Write(format, value0, value1, value2);

                  // ReSharper restore ExpressionIsAlwaysNull

                  TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value0 = new Object();
                  var value1 = new Object();
                  var value2 = new Object();

                  Action throwingAction = () => target.Write(null, value0, value1, value2);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_Write_Int32 : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Int64 value = 123;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Object();
                  target.Write(value);

                  value = null;

                  // ReSharper disable once ExpressionIsAlwaysNull
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123.45f;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = Guid.NewGuid().ToString();
                  target.Write(value);

                  value = null;

                  // ReSharper disable once ExpressionIsAlwaysNull
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  UInt32 value = 123;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  UInt64 value = 123;
                  target.Write(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  target.WriteLine(true);
                  target.WriteLine(false);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 'a';
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Char[] value = {'a', 'b', 'c'};
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Char[] value = {'a', 'b', 'c'};
                  target.WriteLine(value, 1, 2);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Decimal(123.45);
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123.45;
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  target.WriteLine();
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}' ";
                  var value = new Object();
                  target.WriteLine(format, value);

                  value = null;
                  target.WriteLine(format, value);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Object();

                  Action throwingAction = () => target.WriteLine(null, value);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Array : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}', '{1}', '{2}', '{3}' ";
                  Object[] value = {new Object(), new Object(), new Object(), new Object()};
                  target.WriteLine(format, value);

                  format = String.Empty;
                  value = null;
                  target.WriteLine(format, value);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Object[] value = {new Object(), new Object(), new Object(), new Object()};

                  Action throwingAction = () => target.WriteLine(null, value);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_there_is_an_index_in_the_format_string_that_is_out_of_range_of_values_supplied()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var format = "{100}";
                  Object[] value = {new Object()};

                  Action throwingAction = () => target.WriteLine(format, value);
                  var e = throwingAction.Should().Throw<FormatException>();
                  e.And.Message.Should().Contain("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Object : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  var format = " '{0}', '{1}' ";
                  var value0 = new Object();
                  var value1 = new Object();
                  target.WriteLine(format, value0, value1);

                  value0 = null;
                  value1 = null;
                  target.WriteLine(format, value0, value1);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value0 = new Object();
                  var value1 = new Object();

                  Action throwingAction = () => target.WriteLine(null, value0, value1);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_WriteLine_formatted_Object_Object_Object : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw_when_format_is_not_null()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;

                  // ReSharper disable ExpressionIsAlwaysNull
                  const String format = " '{0}', '{1}', '{2}' ";
                  var value0 = new Object();
                  var value1 = new Object();
                  var value2 = new Object();
                  target.WriteLine(format, value0, value1, value2);

                  value0 = null;
                  value1 = null;
                  value2 = null;
                  target.WriteLine(format, value0, value1, value2);

                  // ReSharper restore ExpressionIsAlwaysNull
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_format_is_null()
            {
               var value0 = new Object();
               var value1 = new Object();
               var value2 = new Object();
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Action throwingAction = () => target.WriteLine(null, value0, value1, value2);
                  var e = throwingAction.Should().Throw<ArgumentNullException>();
                  e.And.ParamName.Should().Be("format");
               }
            }
         }

         [TestClass]
         public class When_I_call_ConsoleInternalMapping_WriteLine_Int32 : ConsoleTestBase
         {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_throw()
            {
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123;
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  Int64 value = 123;
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = new Object();
                  target.WriteLine(value);

                  value = null;

                  // ReSharper disable once ExpressionIsAlwaysNull
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = 123.45f;
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  var value = Guid.NewGuid().ToString();
                  target.WriteLine(value);

                  value = null;

                  // ReSharper disable once ExpressionIsAlwaysNull
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  UInt32 value = 123;
                  target.WriteLine(value);
               }

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
               using (var full = new ConsoleUtilities())
               {
                  IConsoleWriter target = full;
                  UInt64 value = 123;
                  target.WriteLine(value);
               }

               TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
            }
         }
      }
   }
}
