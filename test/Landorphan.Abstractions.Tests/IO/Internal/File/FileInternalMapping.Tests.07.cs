namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_ReplaceContents : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            And_the_destinationBackupFileName_does_exist_It_should_replace_the_contents_of_both_the_destinationFile_and_the_destinationBackupFile()
         {
            var cleanupFileNames = new List<String>();
            var enc = new UTF8Encoding(false, true);
            try
            {
               // ArrangeMethod
               var sourceFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(sourceFileName, "originally in source", enc);

               var destinationFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationFileName, "originally in destination", enc);

               var destinationBackupFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationBackupFileName, "originally in backup", enc);

               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});

               // ActMethod
               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);

               // Assert
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               var dfContent = _target.ReadAllLines(destinationFileName, enc).ToArray();
               dfContent.Length.Should().Be(1);
               dfContent[0].Should().Be("originally in source");

               var dfbContent = _target.ReadAllLines(destinationBackupFileName, enc).ToArray();
               dfbContent.Length.Should().Be(1);
               dfbContent[0].Should().Be("originally in destination");

               //---

               // ArrangeMethod
               sourceFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(sourceFileName, "originally in source", enc);

               destinationFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationFileName, "originally in destination", enc);

               destinationBackupFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationBackupFileName, "originally in backup", enc);

               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});

               // ActMethod
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);

               // Assert
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               dfContent = _target.ReadAllLines(destinationFileName, enc).ToArray();
               dfContent.Length.Should().Be(1);
               dfContent[0].Should().Be("originally in source");

               dfbContent = _target.ReadAllLines(destinationBackupFileName, enc).ToArray();
               dfbContent.Length.Should().Be(1);
               dfbContent[0].Should().Be("originally in destination");
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            And_the_destinationBackupFileName_does_not_exist_It_should_replace_the_contents_of_destinationFile_and_create_the_destinationBackupFile()
         {
            var cleanupFileNames = new List<String>();
            var enc = new UTF8Encoding(false, true);
            try
            {
               // ArrangeMethod
               var sourceFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(sourceFileName, "originally in source", enc);

               var destinationFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationFileName, "originally in destination", enc);

               // force the creation of a directory as well
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid() + ".tmp");

               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});

               // ActMethod
               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);

               // Assert
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               var dfContent = _target.ReadAllLines(destinationFileName, enc).ToArray();
               dfContent.Length.Should().Be(1);
               dfContent[0].Should().Be("originally in source");

               var dfbContent = _target.ReadAllLines(destinationBackupFileName, enc).ToArray();
               dfbContent.Length.Should().Be(1);
               dfbContent[0].Should().Be("originally in destination");

               //---

               // ArrangeMethod
               sourceFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(sourceFileName, "originally in source", enc);

               destinationFileName = _target.CreateTemporaryFile();
               _target.AppendAllText(destinationFileName, "originally in destination", enc);

               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid() + ".tmp");

               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});

               // ActMethod
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);

               // Assert
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               dfContent = _target.ReadAllLines(destinationFileName, enc).ToArray();
               dfContent.Length.Should().Be(1);
               dfContent[0].Should().Be("originally in source");

               dfbContent = _target.ReadAllLines(destinationBackupFileName, enc).ToArray();
               dfbContent.Length.Should().Be(1);
               dfbContent[0].Should().Be("originally in destination");
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_has_leading_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, Spaces + destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, Spaces + destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_has_trailing_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName + Spaces);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName + Spaces);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = String.Empty;
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_null_It_should_throw_ArgumentNullException()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            String destinationBackupFileName = null;
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }

            // ReSharper restore ExpressionIsAlwaysNull
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_destinationBackupFileName_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_the_destinationFileName_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = destinationFileName;
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("' because the destination file and destination backup file are the same.");

               throwingAction = () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the destination file and destination backup file are the same.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_the_sourceFileName_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = sourceFileName;
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("' because the source file and destination backup file are the same ('");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("').");

               throwingAction = () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the source file and destination backup file are the same ('");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("').");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_is_white_space_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            const String destinationBackupFileName = " \t ";
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_matches_an_existing_directory_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination backup file name '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination backup file name '");
               e.And.Message.Should().Contain(destinationBackupFileName);
               e.And.Message.Should().Contain("' is a directory.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationBackupFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = ":" + _tempPath;
            try
            {
               Action throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationBackupFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationBackupFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationBackupFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_has_leading_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName});
               _target.ReplaceContentsNoBackup(sourceFileName, Spaces + destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, Spaces + destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(sourceFileName, Spaces + destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, Spaces + destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_has_trailing_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName});
               _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName + Spaces);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName + Spaces);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName + Spaces, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName + Spaces, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = String.Empty;
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_is_null_It_should_throw_ArgumentNullException()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var sourceFileName = _target.CreateTemporaryFile();
            String destinationFileName = null;
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }

            // ReSharper restore ExpressionIsAlwaysNull
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_destinationFileName_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            var sourceFileName = _target.CreateTemporaryFile();
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();
            var destinationFileName = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_is_the_sourceFileName_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = sourceFileName;
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the source file and destination file are the same.");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the source file and destination file are the same.");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the source file and destination file are the same.");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot replace the contents of destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' because the source file and destination file are the same.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
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
         public void And_the_destinationFileName_is_white_space_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            const String destinationFileName = " \t ";
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destinationFileName");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_matches_an_existing_directory_It_should_throw_IOException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The destination file '");
               e.And.Message.Should().Contain(destinationFileName);
               e.And.Message.Should().Contain("' is a directory.");
            }
            finally
            {
               _target.DeleteFile(sourceFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destinationFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = _target.CreateTemporaryFile();
            var destinationFileName = ":" + _tempPath;
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destinationFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destinationFileName");
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
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceFileName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var sourceFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_leading_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName});
               _target.ReplaceContentsNoBackup(Spaces + sourceFileName, destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               _target.ReplaceContentsNoBackupIgnoringMetadataErrors(Spaces + sourceFileName, destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(Spaces + sourceFileName, destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(Spaces + sourceFileName, destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_has_trailing_spaces_It_should_not_throw()
         {
            var cleanupFileNames = new List<String>();
            try
            {
               var sourceFileName = _target.CreateTemporaryFile();
               var destinationFileName = _target.CreateTemporaryFile();
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName});
               _target.ReplaceContentsNoBackup(sourceFileName + Spaces, destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName + Spaces, destinationFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackup(sourceFileName + Spaces, destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();

               sourceFileName = _target.CreateTemporaryFile();
               destinationFileName = _target.CreateTemporaryFile();
               destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
               cleanupFileNames.AddRange(new[] {sourceFileName, destinationFileName, destinationBackupFileName});
               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName + Spaces, destinationFileName, destinationBackupFileName);
               _target.FileExists(sourceFileName).Should().BeFalse();
               _target.FileExists(destinationFileName).Should().BeTrue();
               _target.FileExists(destinationBackupFileName).Should().BeTrue();
            }
            finally
            {
               foreach (var path in cleanupFileNames)
               {
                  _target.DeleteFile(path);
               }
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceFileName = String.Empty;
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_null_It_should_throw_ArgumentNullException()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            String sourceFileName = null;
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }

            // ReSharper restore ExpressionIsAlwaysNull
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_sourceFileName_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();
            var sourceFileName = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("'.\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceFileName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_is_white_space_It_should_throw_ArgumentException()
         {
            const String sourceFileName = " \t ";
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_matches_an_existing_directory_It_should_throw_IOException()
         {
            var sourceFileName = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The source file '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The source file '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The source file '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("' is a directory.");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The file name is invalid.  The source file '");
               e.And.Message.Should().Contain(sourceFileName);
               e.And.Message.Should().Contain("' is a directory.");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceFileName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceFileName = ":" + _tempPath;
            var destinationFileName = _target.CreateTemporaryFile();
            var destinationBackupFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               Action throwingAction = () => _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction = () => _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");

               throwingAction =
                  () => _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
               e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("sourceFileName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceFileName");
            }
            finally
            {
               _target.DeleteFile(destinationFileName);
            }
         }
      }
   }
}
