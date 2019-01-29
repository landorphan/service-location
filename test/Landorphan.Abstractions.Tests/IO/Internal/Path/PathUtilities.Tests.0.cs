namespace Landorphan.Abstractions.Tests.IO.Internal.Path
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Common.Exceptions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo
   // ReSharper disable CommentTypo

   public static partial class PathUtilities_Tests
   {
      private const String Spaces = "   ";
      private static readonly IDirectoryUtilities _directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
      private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
      private static readonly PathInternalMapping _target = new PathInternalMapping();
      private static readonly String _tempPath = _environmentUtilities.GetTemporaryDirectoryPath();

      [TestClass]
      public class When_I_call_PathMapper_ChangeExtension : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_contains_a_colon_character_It_should_throw_ArgumentException()
         {
            const String ValidPathThatMayNotExist = @"c:\temp\tools.txt";
            Action throwingAction = () => _target.ChangeExtension(ValidPathThatMayNotExist, "r:f");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("extension");
            e.And.Message.Should().Be("The extension is not well-formed (invalid characters).\r\nParameter name: extension");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String ValidPathThatMayNotExist = @"c:\temp\tools.txt";
            Action throwingAction = () => _target.ChangeExtension(ValidPathThatMayNotExist, "<");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("extension");
            e.And.Message.Should().Be("The extension is not well-formed (invalid characters).\r\nParameter name: extension");

            throwingAction = () => _target.ChangeExtension(ValidPathThatMayNotExist, ".<");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("extension");
            e.And.Message.Should().Be("The extension is not well-formed (invalid characters).\r\nParameter name: extension");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_has_leading_spaces_It_should_not_trim_them()
         {
            _target.ChangeExtension(@"c:\temp\tools.txt", " abc").Should().Be(@"c:\temp\tools. abc");
            _target.ChangeExtension(@"c:\temp\tools.txt", ". abc").Should().Be(@"c:\temp\tools. abc");
            _target.ChangeExtension(@"c:\temp\tools.txt", " . abc").Should().Be(@"c:\temp\tools. . abc");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_is_empty_It_should_remove_any_extensions()
         {
            _target.ChangeExtension(@"c:\temp\temp.tmp", null).Should().Be(@"c:\temp\temp");
            _target.ChangeExtension(@"c:\temp\temp.xyz.tmp", null).Should().Be(@"c:\temp\temp.xyz");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_is_empty_or_spaces_It_removes_the_current_extension()
         {
            const String ValidPathThatMayNotExist = @"c:\temp\tools.txt";

            var actual = _target.ChangeExtension(ValidPathThatMayNotExist, "");
            actual.Should().Be(@"c:\temp\tools");

            actual = _target.ChangeExtension(ValidPathThatMayNotExist, ".");
            actual.Should().Be(@"c:\temp\tools");

            actual = _target.ChangeExtension(ValidPathThatMayNotExist, ".   ");
            actual.Should().Be(@"c:\temp\tools");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S4144: Methods should not have identical implementations")]
         public void And_the_extension_is_null_It_should_remove_any_extensions()
         {
            _target.ChangeExtension(@"c:\temp\temp.tmp", null).Should().Be(@"c:\temp\temp");
            _target.ChangeExtension(@"c:\temp\temp.xyz.tmp", null).Should().Be(@"c:\temp\temp.xyz");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_is_spaces_It_should_remove_the_extension()
         {
            _target.ChangeExtension(@"c:\temp\temp.tmp", Spaces).Should().Be(@"c:\temp\temp");
            _target.ChangeExtension(@"c:\temp\temp.xyz.tmp", "." + Spaces).Should().Be(@"c:\temp\temp.xyz");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var random0 = Guid.NewGuid().ToString();
            var path = _tempPath + random0 + ":" + Guid.NewGuid() + ".txt";

            Action throwingAction = () => _target.ChangeExtension(path, "rtf");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt";

            _target.ChangeExtension(path, "rtf").Should().Be(pathWithoutExtension + ".rtf");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String InvalidPath = @"c:\|temp\tools.txt";
            Action throwingAction = () => _target.ChangeExtension(InvalidPath, null);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw_but_should_trim_them()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = Spaces + _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt";

            _target.ChangeExtension(path, "rtf").Should().Be(pathWithoutExtension.Trim() + ".rtf");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw_but_should_trim_them()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt" + Spaces;

            _target.ChangeExtension(path, "rtf").Should().Be(pathWithoutExtension + ".rtf");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_it_should_return_empty()
         {
            // edge case:  starting will null or empty returns null or empty
            _target.ChangeExtension(String.Empty, @"txt").Should().Be(String.Empty);
            _target.ChangeExtension(String.Empty, @".txt").Should().Be(String.Empty);
            _target.ChangeExtension(String.Empty, @".abc").Should().Be(String.Empty);
            _target.ChangeExtension(String.Empty, null).Should().Be(String.Empty);

         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_spaces_it_should_return_empty()
         {
            _target.ChangeExtension(Spaces, @"txt").Should().Be(@".txt");
            _target.ChangeExtension(Spaces, @".txt").Should().Be(@".txt");
            _target.ChangeExtension(Spaces, @".abc").Should().Be(@".abc");
            _target.ChangeExtension(Spaces, null).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_it_should_return_null()
         {
            // edge case:  starting will null or empty returns null or empty
            _target.ChangeExtension(null, null).Should().BeNull();
            _target.ChangeExtension(null, "txt").Should().BeNull();
            _target.ChangeExtension(null, ".txt").Should().BeNull();
            _target.ChangeExtension(null, String.Empty).Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.ChangeExtension(path, "rtf");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_change_the_extension()
         {
            var path = @"c:\temp\my.txt";
            var extension = @".tmp";
            var expected = @"c:\temp\my.tmp";
            _target.ChangeExtension(path, extension).Should().Be(expected);

            // strange but expected result.
            path = @"c:";
            extension = @"txt";
            expected = @"c:.txt";
            _target.ChangeExtension(path, extension).Should().Be(expected);

            // no change but removes trailing directory separator character.
            path = @"c:\";
            extension = null;
            expected = @"c:\";

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.ChangeExtension(path, extension).Should().Be(expected);

            // no change
            path = @"c:\temp\my.txt\";
            extension = @".tmp";
            expected = @"c:\temp\my.tmp";
            _target.ChangeExtension(path, extension).Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_Combine : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_a_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.Combine(@"c:\", @"|");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("paths");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: paths");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_paths_does_have_leading_or_trailing_spaces_it_should_trim_appropriately()
         {
            //
            // Single value behavior
            //
            _target.Combine(@"   c:").Should().Be(@"c:");
            _target.Combine(@"c:\   ").Should().Be(@"c:\");
            _target.Combine(@"   /myfile.txt").Should().Be(@"/myfile.txt");
            _target.Combine(@"   .\myfile.txt").Should().Be(@".\myfile.txt");
            _target.Combine(@"   \\someserver\someshare\resource ").Should().Be(@"\\someserver\someshare\resource");

            // it should not trim leading space in resource names (e.g., "   myfile.txt")
            _target.Combine(@"    mydirectory\myfile.txt").Should().Be(@"    mydirectory\myfile.txt");
            _target.Combine(@"    myfile.txt").Should().Be(@"    myfile.txt");

            //
            // Multi-value continues behavior
            //
            _target.Combine(@"   c:\   ", @"   myfile.txt   ").Should().Be(@"c:\   myfile.txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_paths_is_too_long_it_should_not_throw()
         {
            // path length is not enforced
            _target.Combine(new String('A', 300), new String('B', 300));
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_result_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_not_throw()
         {
            _target.Combine(@"c:\", @":");
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_for_drive_current_directory_syntax()
         {
            _target.Combine(@"c:", @"door").Should().Be(@"c:door");
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_for_search_combinations()
         {
            _target.Combine(@"c:\", @"door", @"*.*").Should().Be(@"c:\door\*.*");
            _target.Combine(@"c:\", @"door", @"a?c.tmp").Should().Be(@"c:\door\a?c.tmp");
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_combine_values_inserting_directory_separator_characters_as_needed()
         {
            // HAPPY PATH TESTS:
            _target.Combine(@"c:\temp", "temp.tmp").Should().Be(@"c:\temp\temp.tmp");
            _target.Combine(@"c:\temp\", "temp.tmp").Should().Be(@"c:\temp\temp.tmp");
            _target.Combine(@"c:", @"\temp", @"temp.tmp\").Should().Be(@"c:\temp\temp.tmp");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_handle_roots_alone()
         {
            _target.Combine(@"Z:").Should().Be(@"Z:");
            _target.Combine(@"Z:\").Should().Be(@"Z:\");
            _target.Combine(@"Z:/").Should().Be(@"Z:/");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_ignore_paths_that_are_wholly_whitespace()
         {
            _target.Combine(Spaces, @"\").Should().Be(@"\");
            _target.Combine(@"\", Spaces).Should().Be(@"\");
            _target.Combine(Spaces, @"\", Spaces).Should().Be(@"\");
            _target.Combine(@"/", Spaces).Should().Be(@"/");
            _target.Combine(Spaces, @"/", Spaces).Should().Be(@"/");
            _target.Combine(Spaces, "a", Spaces, "b", Spaces).Should().Be(@"a\b");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_insert_directory_separator_characters_when_needed()
         {
            _target.Combine(@"c:\temp", @"a", @"b", @"c", @"d", @"e").Should().Be(@"c:\temp\a\b\c\d\e");
            _target.Combine(@"c:\temp\", @"a\", @"b\", @"c\", @"d\", @"e\").Should().Be(@"c:\temp\a\b\c\d\e");

            // current directory on C not C:\
            _target.Combine(@"c:", @"temp", @"a", @"b", @"c", @"d", @"e").Should().Be(@"c:temp\a\b\c\d\e");
            _target.Combine(@"c:temp", @"a", @"b", @"c", @"d", @"e").Should().Be(@"c:temp\a\b\c\d\e");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_collapse_multiple_directory_separator_characters()
         {
            // TODO: how is @"\\" handled

            _target.Combine(@"\").Should().Be(@"\");
            _target.Combine(@"\", @"\").Should().Be(@"\\");
            _target.Combine(@"\", @"\", @"\").Should().Be(@"\\\");

            _target.Combine(@"/").Should().Be(@"/");
            _target.Combine(@"/", @"/").Should().Be(@"//");
            _target.Combine(@"/", @"/", @"/").Should().Be(@"///");

            _target.Combine(@"\", @"/").Should().Be(@"\/");
            _target.Combine(@"/", String.Empty, @"\").Should().Be(@"/\");
            _target.Combine(@"\", String.Empty, @"/").Should().Be(@"\/");
            _target.Combine(@"\", String.Empty, @"/", String.Empty, @"/").Should().Be(@"\//");
            _target.Combine(@"\", String.Empty, @"\", String.Empty, @"/").Should().Be(@"\\/");
            _target.Combine(@"/", String.Empty, @"\", String.Empty, @"\").Should().Be(@"/\\");
            _target.Combine(@"/", String.Empty, @"/", String.Empty, @"\").Should().Be(@"//\");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_directory_separator_char()
         {
            _target.Combine(@"\").Should().Be(@"\");
            _target.Combine(Spaces, @"\").Should().Be(@"\");
            _target.Combine(@"\", Spaces).Should().Be(@"\");
            _target.Combine(Spaces + @"\" + Spaces).Should().Be(@"\");

            _target.Combine(@"/").Should().Be(@"/");
            _target.Combine(Spaces, @"/").Should().Be(@"/");
            _target.Combine(@"/", Spaces).Should().Be(@"/");
            _target.Combine(Spaces + @"/" + Spaces).Should().Be(@"/");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_string_Empty()
         {
            _target.Combine(String.Empty).Should().Be(String.Empty);
            _target.Combine(String.Empty, String.Empty).Should().Be(String.Empty);
            _target.Combine(Spaces).Should().Be(String.Empty);
            _target.Combine(Spaces, Spaces).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentContainsNullException()
         {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Action throwingAction = () => _target.Combine(new String[] { null });
            var e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            throwingAction = () => _target.Combine(null, null);
            e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            throwingAction = () => _target.Combine(null, null, null);
            e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            throwingAction = () => _target.Combine(null, null, null, null);
            e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            throwingAction = () => _target.Combine(null, null, null, null, null);
            e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            throwingAction = () => _target.Combine(@"a", @"b", @"c", @"d", null);
            e = throwingAction.Should().Throw<ArgumentContainsNullException>();
            e.And.ParamName.Should().Be(@"paths");

            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Action throwingAction = () => _target.Combine(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("paths");

            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_trim_leading_and_trailing_spaces_appropriately()
         {
            _target.Combine(Spaces + @"x" + Spaces).Should().Be(Spaces + @"x");
            _target.Combine(Spaces + @"x" + Spaces, Spaces + @"y" + Spaces).Should().Be(Spaces + @"x\" + Spaces + @"y");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetExtension : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_contains_a_colon_character_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetExtension(@"c:\temp\tools.a:b");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.GetExtension(@"c:\temp\to:ols.txt");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_does_not_exist_It_should_return_empty()
         {
            _target.GetExtension(@"c:\temp\temp").Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_has_illegal_characters_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetExtension(@"c:\temp\tools.a|b");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.GetExtension(@"c:\temp\to|ols.txt");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_extension_is_empty_or_spaces_It_should_return_string_empty()
         {
            _target.GetExtension(@"c:\temp\temp").Should().Be(String.Empty);
            _target.GetExtension(@"c:\temp\temp.").Should().Be(String.Empty);
            _target.GetExtension(@"c:\temp\temp.   ").Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetExtension(@"c:\temp:directory\tempfile.tmp");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt";

            _target.GetExtension(path).Should().Be(".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String InvalidPath = @"c:\|temp\tools.txt";
            Action throwingAction = () => _target.GetExtension(InvalidPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = Spaces + _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt";

            _target.GetExtension(path).Should().Be(".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw_but_should_trim_them()
         {
            // HAPPY PATH TEST:
            var pathWithoutExtension = _target.Combine(@"c:\", Guid.NewGuid().ToString());
            var path = pathWithoutExtension + ".txt" + Spaces;

            _target.GetExtension(path).Should().Be(".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_or_spaces_It_should_return_string_empty()
         {
            _target.GetExtension(String.Empty).Should().Be(String.Empty);
            _target.GetExtension(Spaces).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.GetExtension(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_extension()
         {
            var path = @"c:\temp\my.txt";
            _target.GetExtension(path).Should().Be(".txt");

            path = @"c:";
            _target.GetExtension(path).Should().Be(String.Empty);

            path = @"c:\";
            _target.GetExtension(path).Should().Be(String.Empty);

            path = @"c:\temp\my.txt\";
            _target.GetExtension(path).Should().Be(".txt");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetFileName : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileName(nonExtantPath).Should().Be(random + ".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String InvalidPath = @"c:\|temp\tools.txt";
            Action throwingAction = () => _target.GetFileName(InvalidPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileName(Spaces + nonExtantPath).Should().Be(random + ".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileName(nonExtantPath + Spaces).Should().Be(random + ".txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_or_spaces_It_should_return_string_empty()
         {
            _target.GetFileName(String.Empty).Should().Be(String.Empty);
            _target.GetFileName(Spaces).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.GetFileName(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetFileName(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_file_name_with_extension()
         {
            _target.GetFileName(@"c:\temp\my.txt").Should().Be(@"my.txt");
            _target.GetFileName(@"c:\temp\my").Should().Be(@"my");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetFileNameWithoutExtension : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileNameWithoutExtension(nonExtantPath).Should().Be(random);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String InvalidPath = @"c:\|temp\tools.txt";
            Action throwingAction = () => _target.GetFileNameWithoutExtension(InvalidPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileNameWithoutExtension(Spaces + nonExtantPath).Should().Be(random);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString();
            var nonExtantPath = _target.Combine(@"c:\", random + ".txt");
            _target.GetFileNameWithoutExtension(nonExtantPath + Spaces).Should().Be(random);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_or_spaces_It_should_return_string_empty()
         {
            _target.GetFileNameWithoutExtension(String.Empty).Should().Be(String.Empty);
            _target.GetFileNameWithoutExtension(Spaces).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.GetFileNameWithoutExtension(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetFileNameWithoutExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_file_name_without_extension()
         {
            _target.GetFileNameWithoutExtension(@"c:\temp\my.txt").Should().Be(@"my");
            _target.GetFileNameWithoutExtension(@"c:\temp\my").Should().Be(@"my");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetFullPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetFullPath(@"c:\temp:directory\tempfile.tmp");
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            // HAPPY PATH TEST:

            // absolute path
            var randomDir = Guid.NewGuid().ToString();
            var randomFile = Guid.NewGuid() + ".txt";
            var path = _target.Combine(@"c:\", randomDir, randomFile);
            _target.GetFullPath(path).Should().Be(path);

            // relative path
            path = _target.Combine(randomDir, randomFile);
            _target.GetFullPath(path).Should().Be(Path.Combine(Path.GetFullPath(_directoryUtilities.GetCurrentDirectory()), path));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_illegal_characters_It_should_throw_ArgumentException()
         {
            const String InvalidPath = @"c:\|temp\tools.txt";
            Action throwingAction = () => _target.GetFullPath(InvalidPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _target.Combine(@"c:\", Guid.NewGuid() + ".txt");
            _target.GetFullPath(Spaces + path).Should().Be(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw_but_should_trim_them()
         {
            // HAPPY PATH TEST:
            var path = _target.Combine(@"c:\", Guid.NewGuid() + ".txt");
            _target.GetFullPath(path + Spaces).Should().Be(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_or_spaces_It_should_return_string_empty()
         {
            _target.GetFullPath(String.Empty).Should().Be(String.Empty);
            _target.GetFullPath(Spaces).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.GetFullPath(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetFullPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_full_path()
         {
            var path = @"c:\temp\my.txt";
            _target.GetFullPath(path).Should().Be(path);

            // unexpected change in case
            // unexpected change in behavior after upgrading from VS 2013 to VS 2015... (.Net 4.6)
            //      instead of root drive, now get current working directory

            path = @"c:";
            Directory.SetCurrentDirectory(path);
            _target.GetFullPath(path).Should().Be(Directory.GetCurrentDirectory());

            path = @"c:\";
            _target.GetFullPath(path).Should().Be(path);

            // unexpected change in directory separator characters
            path = @"c:/";
            _target.GetFullPath(path).Should().Be(@"c:\");

            // unexpected change in directory separator characters
            path = @"c:/mydirectory/myfile.txt";
            _target.GetFullPath(path).Should().Be(@"c:\mydirectory\myfile.txt");

            path = @"c:\temp\my.txt\";
            _target.GetFullPath(path).Should().Be(@"c:\temp\my.txt");

            path = Guid.NewGuid().ToString();
            _target.GetFullPath(path).Should().Be(Path.Combine(Path.GetFullPath(_directoryUtilities.GetCurrentDirectory()), path));
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetInvalidFileNameCharacters : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_retrieve_the_expected_values()
         {
            _target.GetInvalidFileNameCharacters().Should().Contain(Path.GetInvalidFileNameChars());
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetInvalidPathCharacters : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_retrieve_the_expected_values()
         {
            _target.GetInvalidPathCharacters().Should().Contain(Path.GetInvalidPathChars());
         }
      }
   }
}
