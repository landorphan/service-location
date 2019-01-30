namespace Landorphan.Abstractions.Tests.IO.Internal.Path
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo
   // ReSharper disable CommentTypo

   public static partial class PathUtilities_Tests
   {
      [TestClass]
      public class When_I_call_PathMapper_GetParentPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: permissions")]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_but_does_on_the_parent_directory_It_should_return_the_parent()
         {
            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissions;
            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsTestPaths.LocalTestTargetRootFolder);
            _directoryUtilities.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: permissions")]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_nor_on_the_parent_directory_It_should_return_the_parent()
         {
            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissionsChildFolder;
            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissions);
            _directoryUtilities.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _tempPath + random0 + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _target.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetParentPath(path);
            actual.Should().Be(driveNoSep);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_return_the_parent()
         {
            // HAPPY PATH TEST:
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(@"\");
            var path1 = Spaces + _tempPath.Substring(0, _tempPath.Length - 1);

            var actual = _target.GetParentPath(path0);
            var expected = _target.GetParentPath(_tempPath);
            actual.Should().Be(expected);

            actual = _target.GetParentPath(path1);
            expected = _target.GetParentPath(_tempPath);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_return_the_parent()
         {
            // HAPPY PATH TEST:
            var path0 = _tempPath + "  ";
            _tempPath.Should().EndWith(@"\");
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            var actual = _target.GetParentPath(path0);
            var expected = _target.GetParentPath(_tempPath);
            actual.Should().Be(expected);

            actual = _target.GetParentPath(path1);
            expected = _target.GetParentPath(_tempPath);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_null()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var actual = _target.GetParentPath(drive);
            actual.Should().BeNull();

            actual = _target.GetParentPath(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl);
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsTestPaths.UnmappedDrive.Substring(0, 2));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_spaces_or_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetParentPath(String.Empty);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetParentPath(Spaces);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetParentPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random1 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", random0, random1, random2);

            var actual = _target.GetParentPath(path);
            actual.Should().Be(String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", random0, random1));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random0, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetParentPath(path);
            actual.Should().Be(String.Format(CultureInfo.InvariantCulture, @"\\localhost\{0}", random0));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_null_or_value_without_a_trailing_directory_separator_char()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:

            // Local File System (LFS) absolute roots
            _target.GetParentPath(driveNoSep).Should().BeNull();
            _target.GetParentPath(driveNoSep + _target.DirectorySeparatorCharacter).Should().BeNull();
            _target.GetParentPath(driveNoSep + _target.AltDirectorySeparatorCharacter).Should().BeNull();

            // LFS absolute subdirectories
            _target.GetParentPath(drive + @"temp").Should().Be(driveNoSep);
            _target.GetParentPath(drive + @"temp\").Should().Be(driveNoSep);
            _target.GetParentPath(driveNoSep + _target.AltDirectorySeparatorCharacter + @"temp/").Should().Be(driveNoSep);
            _target.GetParentPath(drive + @"temp/").Should().Be(driveNoSep);

            // LFS relative subdirectories (paths likely do not exist)
            _target.GetParentPath(@". \abc").Should().Be(@". "); // TODO: This is the behavior, is it correct? (trailing whitespace)
            _target.GetParentPath(@". \abc\").Should().Be(@". "); // TODO: This is the behavior, is it correct? (trailing whitespace)
            _target.GetParentPath(@". /abc/").Should().Be(@". "); // TODO: This is the behavior, is it correct? (trailing whitespace)
            _target.GetParentPath(@". /abc/").Should().Be(@". "); // TODO: This is the behavior, is it correct? (trailing whitespace)
            _target.GetParentPath(@".\abc").Should().Be(@".");
            _target.GetParentPath(@".\abc\").Should().Be(@".");
            _target.GetParentPath(@"./abc/").Should().Be(@".");
            _target.GetParentPath(@"./abc/").Should().Be(@".");

            _target.GetParentPath(@".").Should().Be(String.Empty);

            _target.GetParentPath(@"\abc").Should().Be(String.Empty);
            _target.GetParentPath(@"\abc\").Should().Be(String.Empty);
            _target.GetParentPath(@"/abc/").Should().Be(String.Empty);
            _target.GetParentPath(@"/abc/").Should().Be(String.Empty);
            _target.GetParentPath(@"abc").Should().Be(String.Empty);
            _target.GetParentPath(@"abc\").Should().Be(String.Empty);
            _target.GetParentPath(@"abc/").Should().Be(String.Empty);

            _target.GetParentPath(@".\abc\123").Should().Be(@".\abc");
            _target.GetParentPath(@".\abc\123\").Should().Be(@".\abc");
            _target.GetParentPath(@"./abc/123/").Should().Be(@".\abc");
            _target.GetParentPath(@"./abc\123/").Should().Be(@".\abc");
            _target.GetParentPath(@"\abc\123").Should().Be(@"\abc");
            _target.GetParentPath(@"\abc\123\").Should().Be(@"\abc");
            _target.GetParentPath(@"/abc/123/").Should().Be(@"\abc");
            _target.GetParentPath(@"/abc\123/").Should().Be(@"\abc");
            _target.GetParentPath(@"abc\123").Should().Be(@"abc");
            _target.GetParentPath(@"abc\123\").Should().Be(@"abc");
            _target.GetParentPath(@"abc/123/").Should().Be(@"abc");
            _target.GetParentPath(@"abc\123/").Should().Be(@"abc");

            // LFS relative subdirectories (path exists)
            var childDirectory = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.IsPathRooted(childDirectory).Should().BeFalse();
            var grandChildDirectory = _target.Combine(childDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.IsPathRooted(grandChildDirectory).Should().BeFalse();
            _directoryUtilities.SetCurrentDirectory(_environmentUtilities.GetTemporaryDirectoryPath());

            _directoryUtilities.CreateDirectory(_target.GetFullPath(grandChildDirectory));
            try
            {
               _directoryUtilities.DirectoryExists(grandChildDirectory).Should().BeTrue();

               _target.GetParentPath(childDirectory).Should().Be(String.Empty);
               _target.GetParentPath(grandChildDirectory).Should().Be(childDirectory);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(_target.GetFullPath(childDirectory));
            }

            // UNC directories 
            _target.GetParentPath(@"\\server\share").Should().BeNull();
            _target.GetParentPath(@"\\server\share\").Should().BeNull();
            _target.GetParentPath(@"\\server\share\directory").Should().Be(@"\\server\share");
            _target.GetParentPath(@"\\server\share\directory\").Should().Be(@"\\server\share");

            // UNC directories AltDirectorySeparatorCharacter
            _target.GetParentPath(@"//server/share").Should().BeNull();
            _target.GetParentPath(@"\\server/share/").Should().BeNull();
            _target.GetParentPath(@"\\server/share/directory").Should().Be(@"\\server\share");
            _target.GetParentPath(@"\\server\share\directory/").Should().Be(@"\\server\share");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetRootPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: permissions")]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_but_does_on_the_parent_directory_It_should_return_the_root()
         {
            if (TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissions == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissions)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissions;
            var actual = _target.GetRootPath(path);
            var bclActual = Path.GetPathRoot(path);
            actual.Should().Be(bclActual);

            _directoryUtilities.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: permissions")]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_nor_on_the_parent_directory_It_should_return_the_root()
         {
            if (TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissionsChildFolder == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissionsChildFolder)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.LocalOuterFolderWithoutPermissionsChildFolder;
            var actual = _target.GetRootPath(path);
            var bclActual = Path.GetPathRoot(path);
            actual.Should().Be(bclActual);

            _directoryUtilities.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _target.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetRootPath(path);
            actual.Should().Be(driveNoSep);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_return_the_root()
         {
            // HAPPY PATH TEST:
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(@"\");
            var path1 = Spaces + _tempPath.Substring(0, _tempPath.Length - 1);

            var actual = _target.GetRootPath(path0);
            var expected = _target.GetRootPath(_tempPath);
            actual.Should().Be(expected);

            actual = _target.GetRootPath(path1);
            expected = _target.GetRootPath(_tempPath);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_return_the_root()
         {
            // HAPPY PATH TEST:
            var path0 = _tempPath + Spaces;
            _tempPath.Should().EndWith(@"\");
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            var actual = _target.GetRootPath(path0);
            var expected = _target.GetRootPath(_tempPath);
            actual.Should().Be(expected);

            actual = _target.GetRootPath(path1);
            expected = _target.GetRootPath(_tempPath);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: UNC paths and permissions")]
         public void And_the_path_is_a_root_It_should_return_the_root()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:
            var actual = _target.GetRootPath(drive);
            actual.Should().Be(driveNoSep);

            actual = _target.GetRootPath(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl);
            actual.Should().Be(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsTestPaths.UnmappedDrive.Substring(0, 2));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_spaces_or_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.GetRootPath(String.Empty);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetRootPath(Spaces);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetRootPath(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random1 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}\", random0, random1, random2);

            var actual = _target.GetRootPath(path);
            actual.Should().Be(String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", random0, random1));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random0, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetRootPath(path);
            actual.Should().Be(String.Format(CultureInfo.InvariantCulture, @"\\localhost\{0}", random0));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_string_empty_or_a_value_without_a_trailing_directory_separator_char()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);
            var driveAltSep = driveNoSep + _target.AltDirectorySeparatorCharacter;

            // HAPPY PATH TEST:

            // Local File System (LFS) absolute roots (case-in,case-out)
            _target.GetRootPath(driveNoSep).Should().Be(driveNoSep);
            _target.GetRootPath(drive).Should().Be(driveNoSep);
            _target.GetRootPath(driveAltSep).Should().Be(driveNoSep);
            _target.GetRootPath(driveNoSep).Should().Be(driveNoSep);
            _target.GetRootPath(drive).Should().Be(driveNoSep);
            _target.GetRootPath(driveAltSep).Should().Be(driveNoSep);

            // LFS absolute subdirectories
            _target.GetRootPath(drive + @"temp").Should().Be(driveNoSep);
            _target.GetRootPath(drive + @"temp\").Should().Be(driveNoSep);
            _target.GetRootPath(driveAltSep + @"temp/").Should().Be(driveNoSep);

            // LFS relative subdirectories (paths likely do not exist)
            _target.GetRootPath(@".\abc").Should().Be(String.Empty);
            _target.GetRootPath(@".\abc\").Should().Be(String.Empty);
            _target.GetRootPath(@"./abc/").Should().Be(String.Empty);
            _target.GetRootPath(@"./abc/").Should().Be(String.Empty);
            _target.GetRootPath(@"\abc").Should().Be(String.Empty);
            _target.GetRootPath(@"\abc\").Should().Be(String.Empty);
            _target.GetRootPath(@"/abc/").Should().Be(String.Empty);
            _target.GetRootPath(@"/abc/").Should().Be(String.Empty);
            _target.GetRootPath(@"abc").Should().Be(String.Empty);
            _target.GetRootPath(@"abc\").Should().Be(String.Empty);
            _target.GetRootPath(@"abc/").Should().Be(String.Empty);

            _target.GetRootPath(@".\abc\123").Should().Be(String.Empty);
            _target.GetRootPath(@".\abc\123\").Should().Be(String.Empty);
            _target.GetRootPath(@"./abc/123/").Should().Be(String.Empty);
            _target.GetRootPath(@"./abc\123/").Should().Be(String.Empty);
            _target.GetRootPath(@"\abc\123").Should().Be(String.Empty);
            _target.GetRootPath(@"\abc\123\").Should().Be(String.Empty);
            _target.GetRootPath(@"/abc/123/").Should().Be(String.Empty);
            _target.GetRootPath(@"/abc\123/").Should().Be(String.Empty);
            _target.GetRootPath(@"abc\123").Should().Be(String.Empty);
            _target.GetRootPath(@"abc\123\").Should().Be(String.Empty);
            _target.GetRootPath(@"abc/123/").Should().Be(String.Empty);
            _target.GetRootPath(@"abc\123/").Should().Be(String.Empty);

            // LFS relative subdirectories (path exists)
            var childDirectory = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.IsPathRooted(childDirectory).Should().BeFalse();
            var grandChildDirectory = _target.Combine(childDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.IsPathRooted(grandChildDirectory).Should().BeFalse();
            _directoryUtilities.SetCurrentDirectory(_environmentUtilities.GetTemporaryDirectoryPath());

            _directoryUtilities.CreateDirectory(_target.GetFullPath(grandChildDirectory));
            try
            {
               _directoryUtilities.DirectoryExists(grandChildDirectory).Should().BeTrue();

               _target.GetRootPath(childDirectory).Should().Be(String.Empty);
               _target.GetRootPath(grandChildDirectory).Should().Be(String.Empty);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(_target.GetFullPath(childDirectory));
            }

            // UNC directories 
            _target.GetRootPath(@"\\server\share").Should().Be(@"\\server\share");
            _target.GetRootPath(@"\\server\share\").Should().Be(@"\\server\share");
            _target.GetRootPath(@"\\server\share\directory").Should().Be(@"\\server\share");
            _target.GetRootPath(@"\\server\share\directory\").Should().Be(@"\\server\share");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_HasExtension : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _tempPath + random0 + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.HasExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _target.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.HasExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.HasExtension(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(@"\");
            var path1 = Spaces + _tempPath.Substring(0, _tempPath.Length - 1);

            _target.HasExtension(path0).Should().BeFalse();
            _target.HasExtension(path1).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path0 = _tempPath + "  ";
            _tempPath.Should().EndWith(@"\");
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            _target.HasExtension(path0).Should().BeFalse();
            _target.HasExtension(path1).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Need to arrange local paths: UNC paths")]
         public void And_the_path_is_a_root_It_should_return_false()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            _target.HasExtension(drive).Should().BeFalse();
            _target.HasExtension(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_or_spaces_It_should_return_false()
         {
            _target.HasExtension(String.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.HasExtension(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            _target.HasExtension(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.HasExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.HasExtension(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random1 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", random0, random1, random2);

            _target.HasExtension(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random0, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.HasExtension(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_determine_if_the_value_has_an_extension()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // expectation 
            // c:\ has no extension
            // c:\ has no extension
            // c:\tmp.txt has an extension
            // c:\temp.txt\ has an extension

            // TODO: needs work

            // HAPPY PATH TEST:
            _target.HasExtension(driveNoSep).Should().BeFalse();
            _target.HasExtension(drive).Should().BeFalse();
            _target.HasExtension(drive + @"temp").Should().BeFalse();
            _target.HasExtension(drive + @"temp\").Should().BeFalse();
            _target.HasExtension(drive + @"1.tmp").Should().BeTrue();
            _target.HasExtension(drive + @"1.tmp\").Should().BeTrue();
            _target.HasExtension(drive + @".tmp").Should().BeTrue();
            _target.HasExtension(drive + @".t").Should().BeTrue();

            _target.HasExtension(@".\abc\123").Should().BeFalse();
            _target.HasExtension(@".\abc\123\a.tmp").Should().BeTrue();
            _target.HasExtension(@".\abc\123\a.tmp\").Should().BeTrue();

            _target.HasExtension(@"\\server\share").Should().BeFalse();
            _target.HasExtension(@"\\server\share.").Should().BeFalse();
            _target.HasExtension(@"\\server\share.   ").Should().BeFalse();
            _target.HasExtension(@"\\server\share\").Should().BeFalse();
            _target.HasExtension(@"\\server\share\abc.def").Should().BeTrue();
            _target.HasExtension(@"\\server\share\abc.def\").Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_IsPathRooted : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _tempPath + random0 + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.IsPathRooted(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _target.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.IsPathRooted(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.IsPathRooted(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            _tempPath.Should().EndWith(@"\");
            _target.IsPathRooted(_tempPath).Should().BeTrue();

            _target.IsPathRooted(Spaces + _tempPath).Should().BeTrue();
            _target.IsPathRooted(Spaces + IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            _tempPath.Should().EndWith(@"\");
            _target.IsPathRooted(_tempPath).Should().BeTrue();

            _target.IsPathRooted(_tempPath + Spaces).Should().BeTrue();
            _target.IsPathRooted(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath) + Spaces).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_true()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            _target.IsPathRooted(drive).Should().BeTrue();
            _target.IsPathRooted(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.IsPathRooted(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            _target.IsPathRooted(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_spaces_or_empty_It_should_return_false()
         {
            _target.IsPathRooted(String.Empty).Should().BeFalse();
            _target.IsPathRooted(Spaces).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.IsPathRooted(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.IsPathRooted(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random1 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var random2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", random0, random1, random2);

            _target.IsPathRooted(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random0, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.IsPathRooted(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_determine_if_the_path_is_rooted()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:
            _target.IsPathRooted(driveNoSep).Should().BeTrue();
            _target.IsPathRooted(drive).Should().BeTrue();
            _target.IsPathRooted(drive + @"temp").Should().BeTrue();
            _target.IsPathRooted(drive + @"temp\").Should().BeTrue();
            _target.IsPathRooted(drive + @"1.tmp").Should().BeTrue();
            _target.IsPathRooted(drive + @"1.tmp\").Should().BeTrue();
            _target.IsPathRooted(drive + @".tmp").Should().BeTrue();
            _target.IsPathRooted(drive + @".t").Should().BeTrue();

            _target.IsPathRooted(@".\abc\123").Should().BeFalse();
            _target.IsPathRooted(@".\abc\123\a.tmp").Should().BeFalse();
            _target.IsPathRooted(@".\abc\123\a.tmp\").Should().BeFalse();

            _target.IsPathRooted(@"\\server\share").Should().BeTrue();
            _target.IsPathRooted(@"\\server\share\").Should().BeTrue();
            _target.IsPathRooted(@"\\server\share\abc.def").Should().BeTrue();
            _target.IsPathRooted(@"\\server\share\abc.def\").Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_service_locate_IPathUtilities : ArrangeActAssert
      {
         private IPathUtilities actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IPathUtilities>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_PathUtilities()
         {
            actual.Should().BeOfType<PathInternalMapping>();
         }
      }
   }
}
