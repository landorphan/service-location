namespace Landorphan.Abstractions.Tests.IO.Internal
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFilters;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo
   // ReSharper disable CommentTypo

   public static class IOStringUtilities_Tests
   {
      private const String Spaces = "   ";
      private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

      [TestClass]
      public class When_I_call_IOStringUtilities_ConditionallyTrimSpaceFromPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_does_has_leading_or_trailing_spaces_it_should_trim_appropriately()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            IOStringUtilities.ConditionallyTrimSpaceFromPath(@"   " + driveNoSep).Should().Be(driveNoSep);
            IOStringUtilities.ConditionallyTrimSpaceFromPath(drive + @"   ").Should().Be(drive);
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            IOStringUtilities.ConditionallyTrimSpaceFromPath(driveNoSep).Should().Be(driveNoSep);
            IOStringUtilities.ConditionallyTrimSpaceFromPath(drive).Should().Be(drive);
            IOStringUtilities.ConditionallyTrimSpaceFromPath(driveNoSep + _pathUtilities.AltDirectorySeparatorCharacter).Should().Be(driveNoSep + _pathUtilities.AltDirectorySeparatorCharacter);
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(drive + @"abc:defg\").Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);
            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var driveAltSep = driveNoSep + pathUtils.AltDirectorySeparatorCharacter;

            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(driveNoSep).Should().BeFalse();
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(drive).Should().BeFalse();
            IOStringUtilities.DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(driveAltSep).Should().BeFalse();
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
         [RunTestOnlyOnWindows]
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);
            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var driveAltSep = driveNoSep + pathUtils.AltDirectorySeparatorCharacter;

            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(drive);
            actual.Should().Be(driveNoSep);

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(driveAltSep);
            actual.Should().Be(driveNoSep);

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(drive + @"Program Files (x86)\");
            actual.Should().Be(drive + @"Program Files (x86)");

            actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(driveAltSep + @"Program Files (x86)/");
            actual.Should().Be(driveAltSep + @"Program Files (x86)");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_multiple_trailing_directory_separator_characters_It_should_remove_only_1()
         {
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"\/" + _pathUtilities.DirectorySeparatorString).Should().Be(@"\/");
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(@"/\" + _pathUtilities.AltDirectorySeparatorString).Should().Be(@"/\");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_multiple_trailing_SepChars_it_should_remove_only_one()
         {
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.DirectorySeparatorString + _pathUtilities.DirectorySeparatorString)
               .Should().Be(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.DirectorySeparatorString + _pathUtilities.AltDirectorySeparatorString)
               .Should().Be(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.AltDirectorySeparatorString + _pathUtilities.DirectorySeparatorString)
               .Should().Be(_pathUtilities.AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.AltDirectorySeparatorString + _pathUtilities.AltDirectorySeparatorString)
               .Should().Be(_pathUtilities.AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);
            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var driveAltSep = driveNoSep + pathUtils.AltDirectorySeparatorCharacter;

            IOStringUtilities.StandardizeDirectorySeparatorCharacters(driveAltSep + @"myfolder/myfile.txt").Should().Be(driveAltSep + @"myfolder/myfile.txt");
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(drive + @"myfolder\myfile.txt").Should().Be(drive + @"myfolder\myfile.txt");
            IOStringUtilities.StandardizeDirectorySeparatorCharacters(@"\\server\share\file.txt").Should().Be(@"\\server\share\file.txt");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_has_mixed_separator_characters_it_should_standardize_them()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            IOStringUtilities.StandardizeDirectorySeparatorCharacters(drive + @"myfolder/myfile.txt").Should().Be(drive + @"myfolder\myfile.txt");
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            const String argName = "testArg";
            var directoryPath = drive + "Any : Folder";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (':' used outside the drive label).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            const String argName = "testArg";
            var directoryPath = drive + "|";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (invalid characters).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_path_is_a_root_it_should_return_the_root()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);
            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var driveAltSep = driveNoSep + pathUtils.AltDirectorySeparatorCharacter;

            IOStringUtilities.ValidateCanonicalPath(driveNoSep, "arg").Should().Be(driveNoSep);
            IOStringUtilities.ValidateCanonicalPath(drive, "arg").Should().Be(drive);
            IOStringUtilities.ValidateCanonicalPath(driveAltSep, "arg").Should().Be(driveAltSep);
            IOStringUtilities.ValidateCanonicalPath(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture), "arg")
               .Should()
               .Be(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            IOStringUtilities.ValidateCanonicalPath(_pathUtilities.AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture), "arg")
               .Should()
               .Be(_pathUtilities.AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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

            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            const String argName = "testArg";
            var directoryPath = @"   " + drive + "windows   ";
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
         [RunTestOnlyOnWindows]
         public void And_path_starts_with_a_colon_character_It_should_throw_ArgumentException()
         {
            const String argName = "testArg";
            const String directoryPath = @":abcdef";
            Action throwingAction = () => IOStringUtilities.ValidateCanonicalPath(directoryPath, argName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be(argName);
            e.And.Message.Should().Contain("The path is not well-formed (':' used outside the drive label).");
         }
      }
   }
}
