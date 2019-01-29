namespace Landorphan.Abstractions.Tests.IO.Internal
{
   using System;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo
   // ReSharper disable CommentTypo

   public static class IOStringUtilities_Tests
   {
      private const String Spaces = "   ";

      [TestClass]
      public class When_I_call_IOStringUtilities_ConditionallyTrimSpaceFromPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_has_leading_or_trailing_spaces_it_should_trim_appropriately()
         {
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"   c:").Should().Be(@"c:");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"c:\   ").Should().Be(@"c:\");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"   /myfile.txt").Should().Be(@"/myfile.txt");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"   .\myfile.txt").Should().Be(@".\myfile.txt");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"   \\someserver\someshare\resource ").Should().Be(@"\\someserver\someshare\resource");

            // it should not trim leading space in resource names (e.g., "   myfile.txt")
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"    mydirectory\myfile.txt").Should().Be(@"    mydirectory\myfile.txt");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"    myfile.txt").Should().Be(@"    myfile.txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_not_have_leading_or_trailing_spaces_it_should_return_the_path()
         {
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"c:").Should().Be(@"c:");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"c:\").Should().Be(@"c:\");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"c:/").Should().Be(@"c:/");
            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"\\someserver\someshare\resource").Should().Be(@"\\someserver\someshare\resource");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_empty_it_should_return_empty()
         {
            IOStringUtilities.ConditionallyTrimSpaceFromPath(String.Empty).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_null_it_should_return_null()
         {
            IOStringUtilities.ConditionallyTrimSpaceFromPath(null).Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_call_IOStringUtilities_DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_contains_a_colon_after_the_drive_label_colon_It_should_return_true()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@"c:\abc:defg\").Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_contains_a_colon_that_is_not_part_of_the_drive_label_It_should_return_true()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@".\abc:defg\").Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_not_contain_a_colon_It_should_return_false()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@".\abc\defg\").Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_a_colon_at_the_drive_label_only_It_should_return_false()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@"c:").Should().BeFalse();
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@"c:\").Should().BeFalse();
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(@"c:/").Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_empty_It_should_return_false()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(String.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_null_It_should_return_false()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_starts_with_colon_It_should_return_true()
         {
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(":Abc").Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_IOStringUtilities_RemoveOneTrailingDirectorySeparatorCharacter : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_not_have_a_SepChar_It_should_return_path()
         {
            var expected = @"c:";
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(expected);
            actual.Should().Be(expected);

            expected = @"abc";
            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(expected);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_a_SepChar_It_should_remove_it()
         {
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"c:\");
            actual.Should().Be("c:");

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"c:/");
            actual.Should().Be("c:");

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"c:\Program Files (x86)\");
            actual.Should().Be(@"c:\Program Files (x86)");

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"c:/Program Files (x86)/");
            actual.Should().Be(@"c:/Program Files (x86)");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_multiple_trailing_directory_separator_characters_It_should_remove_only_1()
         {
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"\/\").Should().Be(@"\/");
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"/\/").Should().Be(@"/\");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_multiple_trailing_SepChars_it_should_remove_only_one()
         {
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"\\").Should().Be(@"\");
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"\/").Should().Be(@"\");
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"/\").Should().Be(@"/");
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"//").Should().Be(@"/");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_empty_It_should_return_empty()
         {
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(String.Empty);
            actual.Length.Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_null_It_should_return_null()
         {
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(null);
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_spaces_It_should_return_spaces()
         {
            const String expected = "  ";
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(expected);
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_call_IOStringUtilities_StandardizeDirectorySeparatorCharacters : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_not_have_mixed_separator_characters_it_should_return_path()
         {
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"c:/myfolder/myfile.txt").Should().Be(@"c:/myfolder/myfile.txt");
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"c:\myfolder\myfile.txt").Should().Be(@"c:\myfolder\myfile.txt");
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"\\server\share\file.txt").Should().Be(@"\\server\share\file.txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_mixed_separator_characters_it_should_standardize_them()
         {
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"c:\myfolder/myfile.txt").Should().Be(@"c:\myfolder\myfile.txt");
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"\\server\share/file.txt").Should().Be(@"\\server\share\file.txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_empty_it_should_return_empty()
         {
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(String.Empty).Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => IOStringUtilities.StandardizeDirectorySeparatorCharacters(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }
      }

      [TestClass]
      public class When_I_call_IOStringUtilities_ValidateCanonicalPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_contains_a_colon_character_which_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            const String directoryPath = @"c:\Windows:System32";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (':' used outside the drive label).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            const String directoryPath = @"c:\|";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (invalid characters).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_a_root_it_should_return_the_root()
         {
            IOStringUtilities.ValidateCanonicalPath(@"c:", "arg").Should().Be(@"c:");
            IOStringUtilities.ValidateCanonicalPath(@"c:\", "arg").Should().Be(@"c:\");
            IOStringUtilities.ValidateCanonicalPath(@"c:/", "arg").Should().Be(@"c:/");
            IOStringUtilities.ValidateCanonicalPath(@"\", "arg").Should().Be(@"\");
            IOStringUtilities.ValidateCanonicalPath(@"/", "arg").Should().Be(@"/");
            IOStringUtilities.ValidateCanonicalPath(@".", "arg").Should().Be(@".");
            IOStringUtilities.ValidateCanonicalPath(@"\\", "arg").Should().Be(@"\\");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_empty_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            var directoryPath = String.Empty;
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_not_rooted_with_leading_spaces_It_should_leave_the_leading_spaces()
         {
            const String argName = "testArg";
            const String directoryPath = @"   windows   ";
            var actual = IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            actual.Should().Be(directoryPath.TrimEnd());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String argName = "testArg";
            const String directoryPath = null;
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be(argName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_rooted_with_leading_spaces_It_should_remove_the_leading_spaces()
         {
            // TODO: improve tests and implementation.  Edge cases:
            // Should remove leading spaces on:
            //    "  c:" 
            //    "  c:\" 
            //    "  c:/" 
            //    "  .\temp"
            //    "  \temp"
            //    "  /temp"
            //  Should not remove leading spaces on
            //    "   abc"
            //    "   abc.tmp"

            const String argName = "testArg";
            const String directoryPath = @"   c:\windows   ";
            var actual = IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            actual.Should().Be(directoryPath.Trim());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_too_long_It_should_throw_PathTooLongException()
         {
            const String argName = "testArg";
            var directoryPath = new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            const String directoryPath = Spaces;
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_starts_with_a_colon_character_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            const String directoryPath = @":c:\";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (':' used outside the drive label).");
         }
      }
   }
}
