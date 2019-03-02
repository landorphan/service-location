namespace Landorphan.Abstractions.Tests.IO.Internal.Path
{
   using System;
   using System.Collections.Immutable;
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
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_but_does_on_the_parent_directory_It_should_return_the_parent()
         {
            if (TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions;
            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsLocalTestPaths.LocalFolderRoot);
            _directoryUtilities.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_nor_on_the_parent_directory_It_should_return_the_parent()
         {
            if (TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions);

            // TODO: examine in depth
            // The behavior of _directoryUtilities.DirectoryExists on extant directories without permissions
            // appears to be affected by UAC, specifically both registry settings for EnableLUA and ConsentPromptBehaviorAdmin
            // can be true or false
            //
            // _directoryUtilities.DirectoryExists(path).Should().BeFalse();
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetParentPath(path);
            actual.Should().Be(drive);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_return_the_parent()
         {
            // HAPPY PATH TEST:
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
         public void And_the_path_is_a_resource_name_It_should_return_null_unc()
         {
            var actual = _target.GetParentPath(@"\\localhost");
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_null_local()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var root = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var actual = _target.GetParentPath(root);
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
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive);
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:

            // Local File System (LFS) absolute roots
            _target.GetParentPath(driveNoSep).Should().BeNull();
            _target.GetParentPath(driveNoSep + _target.DirectorySeparatorCharacter).Should().BeNull();
            _target.GetParentPath(driveNoSep + _target.AltDirectorySeparatorCharacter).Should().BeNull();

            // LFS absolute subdirectories
            _target.GetParentPath(drive + @"temp").Should().Be(drive);
            _target.GetParentPath(drive + @"temp\").Should().Be(drive);
            _target.GetParentPath(driveNoSep + _target.AltDirectorySeparatorCharacter + @"temp/").Should().Be(drive);
            _target.GetParentPath(drive + @"temp/").Should().Be(drive);

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

            _target.GetParentPath(@"\abc").Should().Be(_target.DirectorySeparatorCharacter.ToString());
            _target.GetParentPath(@"\abc\").Should().Be(_target.DirectorySeparatorCharacter.ToString());
            _target.GetParentPath(@"/abc/").Should().Be(_target.DirectorySeparatorCharacter.ToString());
            _target.GetParentPath(@"/abc/").Should().Be(_target.DirectorySeparatorCharacter.ToString());
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
            _target.IsPathRelative(childDirectory).Should().BeTrue();
            var grandChildDirectory = _target.Combine(childDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.IsPathRelative(grandChildDirectory).Should().BeTrue();
            _directoryUtilities.SetCurrentDirectory(_directoryUtilities.GetTemporaryDirectoryPath());

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
            _target.GetParentPath(@"\\server\share").Should().Be(@"\\server");
            _target.GetParentPath(@"\\server\share\").Should().Be(@"\\server");
            _target.GetParentPath(@"\\server\share\directory").Should().Be(@"\\server\share");
            _target.GetParentPath(@"\\server\share\directory\").Should().Be(@"\\server\share");

            // UNC directories AltDirectorySeparatorCharacter
            _target.GetParentPath(@"//server/share").Should().Be(@"//server");
            _target.GetParentPath(@"\\server/share/").Should().Be(@"\\server");
            _target.GetParentPath(@"\\server/share/directory").Should().Be(@"\\server\share");
            _target.GetParentPath(@"\\server\share\directory/").Should().Be(@"\\server\share");
         }
      }

      [TestClass]
      public class When_I_call_PathMapper_GetRootPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_but_does_on_the_parent_directory_It_should_return_the_root()
         {
            if (TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions;
            var actual = _target.GetRootPath(path);
            var bclActual = Path.GetPathRoot(path);
            actual.Should().Be(bclActual);

            _directoryUtilities.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_caller_does_not_have_permissions_on_the_target_directory_nor_on_the_parent_directory_It_should_return_the_root()
         {
            if (TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            var actual = _target.GetRootPath(path);
            var bclActual = Path.GetPathRoot(path);
            actual.Should().Be(bclActual);

            // TODO: examine in depth
            // The behavior of _directoryUtilities.DirectoryExists on extant directories without permissions
            // appears to be affected by UAC, specifically both registry settings for EnableLUA and ConsentPromptBehaviorAdmin
            // can be true or false
            //
            // _directoryUtilities.DirectoryExists(path).Should().BeFalse();
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            var actual = _target.GetRootPath(path);
            actual.Should().Be(drive);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_return_the_root()
         {
            // HAPPY PATH TEST:
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
         public void And_the_path_is_a_root_It_should_return_the_root_mappedDrive()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var actual = _target.GetRootPath(drive);
            actual.Should().Be(drive);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_the_root_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var actual = _target.GetRootPath(TestHardCodes.WindowsUncTestPaths.UncShareRoot);
            actual.Should().Be(@"\\localhost");
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
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            var actual = _target.GetParentPath(path);
            actual.Should().Be(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive);
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
            actual.Should().Be(@"\\" + random0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random);

            var actual = _target.GetRootPath(path);
            actual.Should().Be(@"\\localhost");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_as_expected()
         {
            // HAPPY PATH TEST:

            var builder = ImmutableHashSet<String>.Empty.ToBuilder();
            builder.KeyComparer = StringComparer.Ordinal;
            builder.Add(_target.DirectorySeparatorCharacter.ToString());
            builder.Add(_target.AltDirectorySeparatorCharacter.ToString());
            var bothSeparators = builder.ToImmutable();

            // Local File System (LFS) absolute roots
            _target.GetRootPath(@"c:").Should().Be(@"c:\");
            _target.GetRootPath(@"c:\").Should().Be(@"c:\");
            _target.GetRootPath(@"c:/").Should().Be(@"c:\");
            _target.GetRootPath(@"c:").Should().Be(@"c:\");
            _target.GetRootPath(@"c:\").Should().Be(@"c:\");
            _target.GetRootPath(@"c:/").Should().Be(@"c:\");

            // LFS absolute subdirectories
            _target.GetRootPath(@"c:\" + @"temp").Should().Be(@"c:\");
            _target.GetRootPath(@"c:\" + @"temp\").Should().Be(@"c:\");
            _target.GetRootPath(@"c:/" + @"temp/").Should().Be(@"c:\");

            // LFS relative subdirectories
            _target.GetRootPath(@"\abc").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"\abc\").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@".\abc").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@".\abc\").Should().BeOneOf(bothSeparators);

            _target.GetRootPath(@"/abc").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"/abc/").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"./abc").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"./abc/").Should().BeOneOf(bothSeparators);

            _target.GetRootPath(@"abc").Should().Be(String.Empty);
            _target.GetRootPath(@"abc\").Should().Be(String.Empty);
            _target.GetRootPath(@"abc/").Should().Be(String.Empty);

            _target.GetRootPath(@"\abc\123").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"\abc\123\").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@".\abc\123").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@".\abc\123\").Should().BeOneOf(bothSeparators);

            _target.GetRootPath(@"/abc/123").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"/abc/123/").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"./abc/123/").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"./abc\123/").Should().BeOneOf(bothSeparators);

            // mixed
            _target.GetRootPath(@"/abc\123/").Should().BeOneOf(bothSeparators);
            _target.GetRootPath(@"\abc\123/").Should().BeOneOf(bothSeparators);

            // LFS relative subdirectories (path exists)
            var childDirectory = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.IsPathRelative(childDirectory).Should().BeTrue();
            var grandChildDirectory = _target.Combine(childDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _target.IsPathRelative(grandChildDirectory).Should().BeTrue();
            _directoryUtilities.SetCurrentDirectory(_directoryUtilities.GetTemporaryDirectoryPath());

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
            _target.GetRootPath(@"\\server\share").Should().Be(@"\\server");
            _target.GetRootPath(@"\\server\share\").Should().Be(@"\\server");
            _target.GetRootPath(@"\\server\share\directory").Should().Be(@"\\server");
            _target.GetRootPath(@"\\server\share\directory\").Should().Be(@"\\server");
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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

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
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
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
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            _target.HasExtension(path0).Should().BeFalse();
            _target.HasExtension(path1).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_false_mapped_drive()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            _target.HasExtension(TestHardCodes.WindowsLocalTestPaths.MappedDrive).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_false_mapped_unc_resource_name()
         {
            // HAPPY PATH TEST:
            _target.HasExtension(@"\\localhost").Should().BeFalse();
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
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

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
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
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
      public class When_I_call_PathMapper_IsPathRelative : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _tempPath + random0 + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.IsPathRelative(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _target.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.IsPathRelative(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            // HAPPY PATH TEST:
            var path = _target.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.IsPathRelative(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            _target.IsPathRelative(_tempPath).Should().BeFalse();

            _target.IsPathRelative(Spaces + _tempPath).Should().BeFalse();
            _target.IsPathRelative(Spaces + IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath)).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            _target.IsPathRelative(_tempPath).Should().BeFalse();

            _target.IsPathRelative(_tempPath + Spaces).Should().BeFalse();
            _target.IsPathRelative(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath) + Spaces).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_false_local()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // HAPPY PATH TEST:
            _target.IsPathRelative(TestHardCodes.WindowsLocalTestPaths.MappedDrive).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_a_root_It_should_return_false_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncShareRoot == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncShareRoot)}");
               return;
            }

            // HAPPY PATH TEST:
            _target.IsPathRelative(TestHardCodes.WindowsUncTestPaths.UncShareRoot).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.IsPathRelative(null);
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

            // HAPPY PATH TEST:
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryUtilities.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            _target.IsPathRelative(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_spaces_or_empty_It_should_return_false()
         {
            _target.IsPathRelative(String.Empty).Should().BeTrue();
            _target.IsPathRelative(Spaces).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_return_false()
         {
            const String path = " \t ";
            _target.IsPathRelative(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.IsPathRelative(path);
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

            _target.IsPathRelative(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var random0 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var path = _target.Combine(@"\\localhost\", random0, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            _target.IsPathRelative(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_determine_if_the_path_is_relative()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
            var driveNoSep = drive.Substring(0, 2);

            // HAPPY PATH TEST:
            _target.IsPathRelative(driveNoSep).Should().BeFalse();
            _target.IsPathRelative(drive).Should().BeFalse();
            _target.IsPathRelative(drive + @"temp").Should().BeFalse();
            _target.IsPathRelative(drive + @"temp\").Should().BeFalse();
            _target.IsPathRelative(drive + @"1.tmp").Should().BeFalse();
            _target.IsPathRelative(drive + @"1.tmp\").Should().BeFalse();
            _target.IsPathRelative(drive + @".tmp").Should().BeFalse();
            _target.IsPathRelative(drive + @".t").Should().BeFalse();

            _target.IsPathRelative(@".\abc\123").Should().BeTrue();
            _target.IsPathRelative(@".\abc\123\a.tmp").Should().BeTrue();
            _target.IsPathRelative(@".\abc\123\a.tmp\").Should().BeTrue();

            _target.IsPathRelative(@"\\server\share").Should().BeFalse();
            _target.IsPathRelative(@"\\server\share\").Should().BeFalse();
            _target.IsPathRelative(@"\\server\share\abc.def").Should().BeFalse();
            _target.IsPathRelative(@"\\server\share\abc.def\").Should().BeFalse();
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
