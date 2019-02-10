namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_CopyNoOverwrite : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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
         public void And_the_destFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
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
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Spaces + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp" + Spaces);
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = String.Empty;

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
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
               const String destFileName = null;

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
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

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";
               _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("'.");
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
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
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
               const String destFileName = " \t ";

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
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
            var existingDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = existingDirName;
            try
            {
               _directoryInternalMapping.CreateDirectory(existingDirName);

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create the destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because a directory with the same name already exists.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _directoryInternalMapping.DeleteRecursively(existingDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_matches_an_existing_file_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destFileName = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create the destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because it already exists.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
               var destFileName = $@"{_tempPath}:{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp"}";

               Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
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
         public void And_the_sourceFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"{0}{1}|", _tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            sourceFileName = Spaces + sourceFileName;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            sourceFileName = sourceFileName + Spaces;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = String.Empty;
            var destFileName = _tempPath + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_null_It_should_throw_ArgumentNullException()
         {
            const String sourceFileName = null;
            var destFileName = _tempPath + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
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

            var sourceFileName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(sourceFileName);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_white_space_It_should_throw_ArgumentException()
         {
            const String sourceFileName = " \t ";
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var existingDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var sourceFileName = existingDirName;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(sourceFileName);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"{0}:{1}.tmp", _tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyNoOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_copy_the_file()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_CopyWithOverwrite : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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
         public void And_the_destFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Spaces + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp" + Spaces);
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = String.Empty;

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
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
               const String destFileName = null;

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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

            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";
               _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("'.");
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
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               var destFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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
               const String destFileName = " \t ";

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destFileName");
               e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destFileName");
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
            var existingDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = existingDirName;

            try
            {
               _directoryInternalMapping.CreateDirectory(existingDirName);

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create the destination file '");
               e.And.Message.Should().Contain(destFileName);
               e.And.Message.Should().Contain("' because a directory with the same name already exists.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _directoryInternalMapping.DeleteRecursively(existingDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_matches_an_existing_file_It_copy_the_file()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            var destFileName = _target.CreateTemporaryFile();
            try
            {
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            try
            {
               _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
               var destFileName = $@"{_tempPath}:{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp"}";

               Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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
         public void And_the_sourceFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"{0}{1}|", _tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            sourceFileName = Spaces + sourceFileName;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            sourceFileName = sourceFileName + Spaces;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName.Trim()).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = String.Empty;
            var destFileName = _tempPath + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_null_It_should_throw_ArgumentNullException()
         {
            const String sourceFileName = null;
            var destFileName = _tempPath + _pathUtilities.DirectorySeparatorCharacter + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
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

            var sourceFileName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp";
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(sourceFileName);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_white_space_It_should_throw_ArgumentException()
         {
            const String sourceFileName = " \t ";
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Contain("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var existingDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var sourceFileName = existingDirName;
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(sourceFileName);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_matches_an_existing_file_It_should_copy_the_file()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _target.CreateTemporaryFile();
            String destFileName = null;
            try
            {
               destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
               _target.CopyWithOverwrite(sourceFileName, destFileName);
               _target.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               if (destFileName != null)
               {
                  _target.DeleteFile(destFileName);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            _tempPath.Last().Should().Be(_pathUtilities.DirectorySeparatorCharacter);
            var sourceFileName = String.Format(CultureInfo.InvariantCulture, @"{0}:{1}.tmp", _tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.CopyWithOverwrite(sourceFileName, destFileName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceFileName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
         }
      }
   }
}
