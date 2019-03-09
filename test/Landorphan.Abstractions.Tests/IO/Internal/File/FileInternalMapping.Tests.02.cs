namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Landorphan.TestUtilities.TestFilters;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_CreateFile : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_file()
         {
            // HAPPY PATH TEST:
            var firstDirCreatedUnderTemp = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var dirName = _pathUtilities.Combine(firstDirCreatedUnderTemp, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var fileName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_file.tmp";
            var fullPath = _pathUtilities.Combine(dirName, fileName);
            try
            {
               _target.CreateFile(fullPath);
               _target.FileExists(fullPath).Should().BeTrue();
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(firstDirCreatedUnderTemp);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_directory_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         // This behvior is not present on Linux/Unix OSs
         public void And_the_extension_is_empty_or_spaces_It_should_create_a_file_without_an_extension()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            try
            {
               _target.CreateFile(_tempPath + "myfile");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }

            try
            {
               _target.CreateFile(_tempPath + "myfile.");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }

            try
            {
               _target.CreateFile(_tempPath + "myfile.   ");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_file_already_exists_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var tempFilePath = _target.CreateTemporaryFile();
            try
            {
               _target.CreateFile(tempFilePath);
            }
            finally
            {
               _target.DeleteFile(tempFilePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_file_does_not_exist_It_should_create_the_file()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            _target.FileExists(path).Should().BeFalse();
            try
            {
               _target.CreateFile(path);
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_file_name_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|.tmp";

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Spaces + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            _target.CreateFile(path);
            _target.FileExists(path).Should().BeTrue();
            _target.DeleteFile(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp" + Spaces);
            _target.CreateFile(path);
            _target.FileExists(path).Should().BeTrue();
            _target.DeleteFile(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_create_the_file()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateFile(path).Should().Be(path);
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            var expectedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.GetParentPath(path));

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(expectedPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            var expectedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.GetParentPath(path));

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(expectedPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            // directory path issue
            var dirNameTooLong = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var dirNameTooLongAndFileName = dirNameTooLong + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".TMP";

            Action throwingAction = () => _target.CreateFile(dirNameTooLongAndFileName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            // combined directory path + file name issue
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var fileNameTooLong = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            throwingAction = () => _target.CreateFile(fileNameTooLong);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S4144: Methods should not have identical implementations")]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.CreateFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_file()
         {
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_file));
            try
            {
               _target.FileExists(path).Should().BeFalse();
               var actual = _target.CreateFile(path);

               _target.FileExists(actual).Should().BeTrue();
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_CreateTemporaryFile : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_a_file_in_the_temp_directory()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.FileExists(path).Should().BeTrue();
               _pathUtilities.GetParentPath(path).ToUpperInvariant().Should().Be(_pathUtilities.GetFullPath(_tempPath).ToUpperInvariant());
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_CreateText : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_file()
         {
            // HAPPY PATH TEST:
            var firstDirCreatedUnderTemp = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var dirName = _pathUtilities.Combine(firstDirCreatedUnderTemp, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var fileName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_file.tmp";
            var fullPath = _pathUtilities.Combine(dirName, fileName);
            try
            {
               _target.CreateText(fullPath);
               _target.FileExists(fullPath).Should().BeTrue();
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(firstDirCreatedUnderTemp);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_directory_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         // Linux/Unix do not consider a file with an extention and one without equivilent
         public void And_the_extension_is_empty_or_spaces_It_should_create_a_file_without_an_extension()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            try
            {
               _target.CreateText(_tempPath + "myfile");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }

            try
            {
               _target.CreateText(_tempPath + "myfile.");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }

            try
            {
               _target.CreateText(_tempPath + "myfile.   ");
               _target.FileExists(_tempPath + "myfile").Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(_tempPath + "myfile");
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_file_already_exists_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var tempFilePath = _target.CreateTemporaryFile();
            try
            {
               _target.CreateText(tempFilePath);
            }
            finally
            {
               _target.DeleteFile(tempFilePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_file_does_not_exist_It_should_create_the_file()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            _target.FileExists(path).Should().BeFalse();
            try
            {
               _target.CreateText(path);
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_file_name_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|.tmp";

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Spaces + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            _target.CreateText(path);
            _target.FileExists(path).Should().BeTrue();
            _target.DeleteFile(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp" + Spaces);
            _target.CreateText(path);
            _target.FileExists(path).Should().BeTrue();
            _target.DeleteFile(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_create_the_file()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateText(path).Should().Be(path);
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            var expectedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.GetParentPath(path));

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(expectedPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            var expectedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_pathUtilities.GetParentPath(path));

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(expectedPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            // directory path issue
            var dirNameTooLong = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var dirNameTooLongAndFileName = dirNameTooLong + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".TMP";

            Action throwingAction = () => _target.CreateText(dirNameTooLongAndFileName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            // combined directory path + file name issue
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var fileNameTooLong = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            throwingAction = () => _target.CreateText(fileNameTooLong);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S4144: Methods should not have identical implementations")]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.CreateText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_file()
         {
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_file));
            try
            {
               _target.FileExists(path).Should().BeFalse();
               var actual = _target.CreateText(path);

               _target.FileExists(actual).Should().BeTrue();
               _target.FileExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_DeleteFile : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteFile(path);
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateFile(path);
            _target.DeleteFile(Spaces + path);
            _target.FileExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateFile(path);
            _target.DeleteFile(path + Spaces);
            _target.FileExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [TestCategory(WellKnownTestCategories.PreventRegressionBug)]
         public void And_the_path_is_a_file_in_the_present_working_directory_with_no_path_information_save_the_file_name_It_should_delete_the_file()
         {
            // DeleteFile was failing to delete unadorned file names in the present working directory.
            // DeleteFile .\SomeFile.text >> worked
            // DeleteFile SomeFile.txt >> returned before deleting because the directory did not exist.

            // change the pwd
            Directory.SetCurrentDirectory(Path.GetTempPath());
            var fileName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt";
            _target.CreateFile(fileName);
            _target.FileExists(fileName).Should().BeTrue();
            _target.DeleteFile(fileName);
            _target.FileExists(fileName).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            _target.DeleteFile(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            //_target.DeleteFile(path);
            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The file name is invalid.  The path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' is a directory.");

            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.DeleteFile(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteFile(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteFile(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_the_file()
         {
            var path = _target.CreateTemporaryFile();
            _target.FileExists(path).Should().BeTrue();
            _target.DeleteFile(path);
            _target.FileExists(path).Should().BeFalse();
         }
      }
   }
}
