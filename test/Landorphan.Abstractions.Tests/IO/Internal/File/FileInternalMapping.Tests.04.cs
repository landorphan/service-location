namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.Attributes;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_GetLastWriteTime : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var path = _pathUtilities.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
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

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(path));
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_directory_file_It_should_throw_FileNotFoundException()
         {
            var path = _tempPath;

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(path));
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_last_write_time()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var actual = _target.GetLastWriteTime(path);
               actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_GetRandomFileName : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_file_name_that_is_not_rooted_and_does_not_exist()
         {
            var actual = _target.GetRandomFileName();
            _target.FileExists(actual).Should().BeFalse();
            _pathUtilities.IsPathRelative(actual).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_Move : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_destFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var destFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_destFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var destFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_has_leading_spaces_It_should_not_throw()
         {
            var destFileName = Spaces + _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               _target.Move(sourceFileName, destFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_has_trailing_spaces_It_should_not_throw()
         {
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + Spaces;

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               _target.Move(sourceFileName, destFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, String.Empty);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_null_It_should_throw_ArgumentNullException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destFileName = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_the_sourceFileName_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = sourceFileName;
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because the source file name and destination file name are the same.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var destFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_white_space_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, " \t ");
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_matches_an_existing_directory_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
               Action throwingAction = () => _target.Move(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because it is a directory.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_matches_an_existing_file_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var existingFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = existingFileName;
               Action throwingAction = () => _target.Move(sourceFileName, existingFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because the file already exists.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(existingFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_destFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.Move(sourceFileName, ":");
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_and_destFileName_do_not_share_a_common_root_It_should_move_the_file()
         {
            if (TestHardCodes.WindowsLocalTestPaths.LocalFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.LocalFolderEveryoneFullControl)}");
               return;
            }

            var sourceFileName = _target.CreateTemporaryFile();
            var destFileName = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.LocalFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.Move(sourceFileName, destFileName);

               _target.FileExists(destFileName).Should().BeTrue();
               _target.FileExists(sourceFileName).Should().BeFalse();
            }
            finally
            {
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_sourceFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_sourceFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|.tmp";
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var sourceFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(sourceFileName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_leading_spaces_It_should_not_throw()
         {
            var sourceFileName = Spaces + _target.CreateTemporaryFile();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.Move(sourceFileName, destFileName);

               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_trailing_spaces_It_should_not_throw()
         {
            var sourceFileName = _target.CreateTemporaryFile() + Spaces;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.Move(sourceFileName, destFileName);

               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = String.Empty;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_null_It_should_throw_ArgumentNullException()
         {
            String sourceFileName = null;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var sourceFileName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(sourceFileName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_white_space_It_should_throw_ArgumentException()
         {
            const String SourceDirName = " \t ";
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(SourceDirName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_matches_an_existing_Directory_It_should_throw_FileNotFoundException()
         {
            var sourceFileName = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _directoryInternalMapping.DirectoryExists(sourceFileName).Should().BeTrue();

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(sourceFileName);
            e.And.Message.Should().Contain("Parameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_sourceFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String SourceDirName = ":";
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(SourceDirName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_uses_an_unknown_network_name_host_It_should_throw_FileNotFoundException()
         {
            var host = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var share = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(sourceFileName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_uses_an_unknown_network_name_share_It_should_throw_FileNotFoundException()
         {
            const String Host = "localhost";
            var share = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(sourceFileName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_move_the_file()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_move_the_file) + ".txt");
            try
            {
               _target.Move(sourceFileName, destFileName);

               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }
      }
   }
}
