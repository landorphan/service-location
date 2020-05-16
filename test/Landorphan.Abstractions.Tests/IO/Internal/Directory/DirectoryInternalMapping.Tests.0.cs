﻿namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;
    using Landorphan.Abstractions.Tests.TestFacilities;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.TestFacilities;
    using Landorphan.TestUtilities.TestFilters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static partial class DirectoryInternalMapping_Tests
   {
       private const string Spaces = "   ";
       private static readonly IDirectoryUtilities _directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
       private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
       private static readonly IFileInternalMapping _fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
       private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
       private static readonly DirectoryInternalMapping _target = new DirectoryInternalMapping();

       private static string TempPath { get; } = _directoryUtilities.GetTemporaryDirectoryPath();

       [TestClass]
      public class When_I_call_DirectoryInternalMapping_Copy : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_destDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = TempPath +
                              Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                              ":" +
                              Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = Spaces + _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + Spaces;
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = string.Empty;
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_null_It_should_throw_ArgumentNullException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            string destDirName = null;
            try
            {
               _target.CreateDirectory(sourceDirName);

               // ReSharper disable once ExpressionIsAlwaysNull
               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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

            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            const string DestDirName = " \t ";

            Action throwingAction = () => _target.Copy(sourceDirName, DestDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("destDirName");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: destDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_within_SourceDirName_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot copy to the destination directory");
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
         public void And_the_destDirName_matches_an_existing_directory_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(sourceDirName);
               _target.CreateDirectory(destDirName);

               _target.Copy(sourceDirName, destDirName);
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
         public void And_the_destDirName_matches_an_existing_file_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _fileInternalMapping.CreateTemporaryFile();
            try
            {
               _target.CreateDirectory(sourceDirName);

               _fileInternalMapping.FileExists(destDirName).Should().BeTrue();

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot copy to the destination directory");
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
         public void And_the_destDirName_matches_sourceDirName_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = sourceDirName;
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
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
         [RunTestOnlyOnWindows]
         public void And_the_destDirName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            const string DestDirName = ":";
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, DestDirName);
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
         // Test Setup not running on XPlat bases.
         public void And_the_sourceDirName_and_DestDirName_do_not_share_a_common_root_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.LocalFolderRoot, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(sourceDirName);
            try
            {
               _target.Copy(sourceDirName, destDirName);
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
         [RunTestOnlyOnWindows]
         public void And_the_sourceDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = TempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var destDirName = TempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_sourceDirName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_has_leading_spaces_It_should_not_throw()
         {
            var sourceDirName = Spaces + _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
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
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + Spaces;
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
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
            var sourceDirName = string.Empty;
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_null_It_should_throw_ArgumentNullException()
         {
            string sourceDirName = null;
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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

            var sourceDirName = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceDirName = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_white_space_It_should_throw_ArgumentException()
         {
            const string SourceDirName = " \t ";
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(SourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _fileInternalMapping.CreateTemporaryFile();
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _fileInternalMapping.FileExists(sourceDirName).Should().BeTrue();

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
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
            const string SourceDirName = ":";
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(SourceDirName, destDirName);
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

            var sourceDirName = string.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = string.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            const string Host = "localhost";
            var share = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var sourceDirName = string.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = string.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_copy_the_directory()
         {
            var sourceDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var destDirName = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(sourceDirName);
               var filePath = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
               _fileInternalMapping.CreateFile(filePath);

               var childDirPath = _target.CreateDirectory(_pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
               filePath = _pathUtilities.Combine(childDirPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
               _fileInternalMapping.CreateFile(filePath);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
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
      public class When_I_call_DirectoryInternalMapping_CreateDirectory : TestBase
      {
          // TODO: add tests for directorySecurity

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_directory()
         {
            // HAPPY PATH TEST:
            var firstDirCreatedUnderTemp = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var path = _pathUtilities.Combine(firstDirCreatedUnderTemp, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }

            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }

            firstDirCreatedUnderTemp.Last().Should().NotBe(_pathUtilities.DirectorySeparatorCharacter);
            var pathWithSpaces = string.Format(
               CultureInfo.InvariantCulture,
               @"{0}\   {1}   \   {2}   \",
               firstDirCreatedUnderTemp,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.CreateDirectory(pathWithSpaces);
               _target.DirectoryExists(pathWithSpaces).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_already_exists_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            // CreateDirectory()
            // arrange
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();

            // act
            _target.CreateDirectory(path);

            // assert
            _target.DirectoryExists(path).Should().BeTrue();

            // teardown
            _target.DeleteRecursively(path);

            //---

            // CreateDirectory(path, directorySecurity)
            // arrange
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();

            // act
            _target.CreateDirectory(path);

            // assert
            _target.DirectoryExists(path).Should().BeTrue();

            // teardown
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_does_not_exist_It_should_create_the_directory()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);

            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = TempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_extends_a_known_host_and_known_share_it_should_not_throw()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.DirectoryExists(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl).Should().BeTrue();
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // when tested, the existence of a trailing backslash made no difference
            var path = string.Format(CultureInfo.InvariantCulture, @"   {0}\{1}\", TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // when tested, the existence of a trailing backslash made no difference
            var path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}\   ", TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = string.Empty;

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const string path = null;

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = string.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path");
            e.And.Message.Should().Contain(path);
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

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const string path = " \t ";

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = TempPath;
            _target.CreateDirectory(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_IOException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.CreateDirectory(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("already exists.");
               e.And.Message.Should().Contain(path);

               throwingAction = () => _target.CreateDirectory(path);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("already exists.");
               e.And.Message.Should().Contain(path);
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
            const string path = ":";

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_directory_absolute()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(TempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_absolute));
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_directory_relative()
         {
            // relative
            _target.SetCurrentDirectory(TempPath);
            var path = @".\" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_relative);
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_directory_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_unc));
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_network_name_It_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_DeleteEmpty : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_is_empty_It_should_delete_the_directory()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DeleteEmpty(path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_is_not_empty_It_should_throw_IOException()
         {
            var fileMapper = new FileInternalMapping();
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var filePath = _pathUtilities.Combine(path, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            // .Net Standard removed the period in empty. and changed it to empty : (path)
            var matchRegEx = ".*[Dd]irectory (is )?not empty.*";

            // nested file
            _target.CreateDirectory(path);
            fileMapper.CreateFile(filePath);
            try
            {
               fileMapper.FileExists(filePath).Should().BeTrue();

               Action throwingAction = () => _target.DeleteEmpty(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().MatchRegex(matchRegEx);
               if (RuntimePlatform.IsWindows())
               {
                  e.And.HResult.Should().Be(unchecked((int)0x80070091));
               }
               else if (RuntimePlatform.IsOSX())
               {
                  e.And.HResult.Should().Be(66);
               }
               else if (RuntimePlatform.IsLinux())
               {
                  e.And.HResult.Should().Be(39);
               }
               else
               {
                  throw new NotSupportedException(TestHardCodes.UnrecognizedPlatform);
               }

               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // nested directory
            _target.CreateDirectory(_pathUtilities.Combine(path, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
            try
            {
               _target.DirectoryExists(path).Should().BeTrue();

               Action throwingAction = () => _target.DeleteEmpty(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().MatchRegex(matchRegEx);
               if (RuntimePlatform.IsWindows())
               {
                  e.And.HResult.Should().Be(unchecked((int)0x80070091));
               }
               else if (RuntimePlatform.IsOSX())
               {
                  e.And.HResult.Should().Be(66);
               }
               else if (RuntimePlatform.IsLinux())
               {
                  e.And.HResult.Should().Be(39);
               }
               else
               {
                  throw new NotSupportedException(TestHardCodes.UnrecognizedPlatform);
               }

               _target.DirectoryExists(path).Should().BeTrue();
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
            var path = TempPath +
                       Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                       ":" +
                       Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteEmpty(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DeleteEmpty(Spaces + path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.CreateDirectory(path);
            _target.DeleteEmpty(path + Spaces);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = string.Empty;

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const string path = null;

            Action throwingAction = () => _target.DeleteEmpty(path);
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
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            _target.DeleteEmpty(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const string path = " \t ";

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            var filePath = _pathUtilities.Combine(path, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            _target.CreateDirectory(path);
            _fileInternalMapping.CreateFile(filePath);
            try
            {
               _target.DirectoryExists(path).Should().BeTrue();
               _fileInternalMapping.FileExists(filePath).Should().BeTrue();

               Action throwingAction = () => _target.DeleteEmpty(filePath);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The directory name '");
               e.And.Message.Should().Contain(filePath);
               e.And.Message.Should().Contain("is invalid because a file with the same name already exists.");
               e.And.HResult.Should().Be(unchecked((int)0x8007010B));

               _target.DirectoryExists(path).Should().BeTrue();
               _fileInternalMapping.FileExists(filePath).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const string path = ":";

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            var path = string.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteEmpty(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.DeleteEmpty(path);

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_an_empty_directory_absolute()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(TempPath), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_absolute));
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_an_empty_directory_relative()
         {
            // relative
            _target.SetCurrentDirectory(TempPath);
            var path = @".\" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_relative);
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_an_empty_directory_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncShareRoot,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_unc));
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }
      }
   }
}
