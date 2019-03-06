namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Landorphan.TestUtilities.TestFilters;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming   

   public static partial class DirectoryInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryInternalMapping_Move : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_destDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_destDirName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_has_leading_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = Spaces + _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_has_trailing_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + Spaces;
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = String.Empty;
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_null_It_should_throw_ArgumentNullException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            String destDirName = null;
            try
            {
               _target.CreateDirectory(sourceDirName);

               // ReSharper disable once ExpressionIsAlwaysNull
               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_white_space_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            const String DestDirName = " \t ";

            Action throwingAction = () => _target.Move(sourceDirName, DestDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("destDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_within_SourceDirName_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to the destination directory");
               e.And.Message.Should().Contain(destDirName);
               e.And.Message.Should().Contain("because it is a subdirectory of the source directory");
               e.And.Message.Should().Contain(sourceDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_matches_an_existing_directory_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);
               _target.CreateDirectory(destDirName);

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to the destination directory");
               e.And.Message.Should().Contain(destDirName);
               e.And.Message.Should().Contain("because a directory with the same name already exists.");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_matches_an_existing_file_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _fileInternalMapping.CreateTemporaryFile();
            try
            {
               _target.CreateDirectory(sourceDirName);

               _fileInternalMapping.FileExists(destDirName).Should().BeTrue();

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot move to the destination directory");
               e.And.Message.Should().Contain(destDirName);
               e.And.Message.Should().Contain("because a file with the same name already exists.");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _fileInternalMapping.DeleteFile(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_destDirName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            const String DestDirName = ":";
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Move(sourceDirName, DestDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_and_DestDirName_do_not_share_a_common_root_and_Source_DirName_Exists_It_should_throw_ArgumentException()
         {
            var sourceDirName = _tempPath;
            var destDirName = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Be("Source and destination path must have identical roots. Move will not work across volumes.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_sourceDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_sourceDirName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_has_leading_spaces_It_should_not_throw()
         {
            var sourceDirName = Spaces + _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeFalse();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_has_trailing_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + Spaces;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeFalse();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceDirName = String.Empty;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_null_It_should_throw_ArgumentNullException()
         {
            String sourceDirName = null;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var sourceDirName = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceDirName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_white_space_It_should_throw_ArgumentException()
         {
            const String SourceDirName = " \t ";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(SourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _fileInternalMapping.CreateTemporaryFile();
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _fileInternalMapping.FileExists(sourceDirName).Should().BeTrue();

               Action throwingAction = () => _target.Move(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(sourceDirName);
               e.And.Message.Should().Contain("Parameter name: sourceDirName");
            }
            finally
            {
               _fileInternalMapping.DeleteFile(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_sourceDirName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String SourceDirName = ":";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(SourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var host = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var share = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            const String Host = "localhost";
            var share = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Move(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_move_the_directory()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeFalse();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_SetCreationTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_creationTime_is_greater_than_maximum_It_should_throw_ArgumentOutOfRangeException()
         {
            // Test cannot be written/executed because the maximum is at DateTimeOffset.MaxValue
            // (cannot add even one tick to the value).
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_creationTime_is_less_than_minimum_It_should_throw_ArgumentOutOfRangeException()
         {
            var creationTime = _target.MinimumFileTimeAsDateTimeOffset.Add(TimeSpan.FromTicks(-1));
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetCreationTime(path, creationTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("creationTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: creationTime");
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
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
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var path = _pathUtilities.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetCreationTime(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.SetCreationTime(path, value);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("Parameter name: path");
            }
            finally
            {
               _fileInternalMapping.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
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
               @"\\{0}\{1}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_creation_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_set_the_creation_time));
            _target.CreateDirectory(path);
            try
            {
               _target.SetCreationTime(path, _target.MinimumFileTimeAsDateTimeOffset);
               _target.GetCreationTime(path).Should().Be(_target.MinimumFileTimeAsDateTimeOffset);

               var expected = AbstractionsTestHelper.GetUtcNowForFileTest();
               _target.SetCreationTime(path, expected);
               _target.GetCreationTime(path).Should().Be(expected);

               _target.SetCreationTime(path, _target.MaximumFileTimeAsDateTimeOffset);
               _target.GetCreationTime(path).Should().Be(_target.MaximumFileTimeAsDateTimeOffset);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }
      }
   }
}
