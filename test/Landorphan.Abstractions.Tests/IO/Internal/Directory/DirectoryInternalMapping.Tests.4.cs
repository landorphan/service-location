namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.Attributes;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Common.Exceptions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming   

   public static partial class DirectoryInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryInternalMapping_GetFileSystemEntries : AbstractionTestBase
      {
         // An empty search pattern does not throw, does it change behavior?
         // An white-space search pattern does not throw, does it change behavior?

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
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

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               var actual = _target.GetFileSystemEntries(Spaces + outerFullPath);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.GetFileSystemEntries(Spaces + outerFullPath, "*");
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.GetFileSystemEntries(Spaces + outerFullPath, "*", SearchOption.AllDirectories);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }
            }
            finally
            {
               _target.DeleteRecursively(outerFullPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               var actual = _target.GetFileSystemEntries(outerFullPath + Spaces);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.GetFileSystemEntries(outerFullPath + Spaces, "*");
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual =
                  _target.GetFileSystemEntries(outerFullPath + Spaces, "*", SearchOption.AllDirectories);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }
            }
            finally
            {
               _target.DeleteRecursively(outerFullPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentNullException>();
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

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.GetFileSystemEntries(path);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("Parameter name: path");

               throwingAction = () => _target.GetFileSystemEntries(path, "*");
               e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("Parameter name: path");

               throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
               e = throwingAction.Should().Throw<DirectoryNotFoundException>();
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
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
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

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetFileSystemEntries(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_searchOption_is_unrecognized_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            const String path = @".\";
            const String SearchPattern = "*.";
            var searchOption = (SearchOption)(-5);

            Action throwingAction = () => _target.GetFileSystemEntries(path, SearchPattern, searchOption);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("searchOption");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_searchPattern_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            const String path = @".\";

            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var SearchPattern = "*." + pathUtils.GetInvalidFileNameCharacters().First();

            Action throwingAction = () => _target.GetFileSystemEntries(path, SearchPattern);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("searchPattern");
            e.And.Message.Should().Contain("The search pattern is not well-formed (contains invalid characters).");

            throwingAction = () => _target.GetFileSystemEntries(path, SearchPattern, SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("searchPattern");
            e.And.Message.Should().Contain("The search pattern is not well-formed (contains invalid characters).");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_searchPattern_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = @".\";
            const String SearchPattern = null;

            Action throwingAction = () => _target.GetFileSystemEntries(path, SearchPattern);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("searchPattern");

            throwingAction = () => _target.GetFileSystemEntries(path, SearchPattern, SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("searchPattern");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_FileSystemEntries()
         {
            var outerFullPath =
               _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries)));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries)))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               _target.GetFileSystemEntries(outerFullPath).Should().Contain(expected);
               _target.GetFileSystemEntries(outerFullPath, "*").Should().Contain(expected);
               _target.GetFileSystemEntries(outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _target.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_GetLastAccessTime : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.GetLastAccessTime(path);
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

            Action throwingAction = () => _target.GetLastAccessTime(path);
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

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            var path1 = Spaces + _tempPath.Substring(0, _tempPath.Length - 1);

            var actual = _target.GetLastAccessTime(path0);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);

            actual = _target.GetLastAccessTime(path1);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should__not_throw()
         {
            var path0 = _tempPath + "  ";
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            var actual = _target.GetLastAccessTime(path0);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);

            actual = _target.GetLastAccessTime(path1);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.GetLastAccessTime(path);
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

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.GetLastAccessTime(path);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(path);
            }
            finally
            {
               _fileInternalMapping.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.GetLastAccessTime(path);
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

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetLastAccessTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_last_access_time()
         {
            // NOTE: when using %temp%, SetLastAccessTime was running afoul of the following IOException: 
            // The process cannot access the file 'c:\temp' because it is being used by another process.
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_get_the_last_access_time));
            _target.CreateDirectory(path);
            try
            {
               var expected = DateTimeOffset.UtcNow;
               _target.SetLastAccessTime(path, expected);
               _target.GetLastAccessTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_GetLastWriteTime : AbstractionTestBase
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

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path0 = Spaces + _tempPath;
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            var path1 = Spaces + _tempPath.Substring(0, _tempPath.Length - 1);

            var actual = _target.GetLastWriteTime(path0);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);

            actual = _target.GetLastWriteTime(path1);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should__not_throw()
         {
            var path0 = _tempPath + "  ";
            _tempPath.Should().EndWith(_pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture));
            var path1 = _tempPath.Substring(0, _tempPath.Length - 1) + Spaces;

            var actual = _target.GetLastWriteTime(path0);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);

            actual = _target.GetLastWriteTime(path1);
            actual.Should().BeOnOrBefore(DateTimeOffset.UtcNow);
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
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
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
         public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.GetLastWriteTime(path);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(path);
            }
            finally
            {
               _fileInternalMapping.DeleteFile(path);
            }
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
               @"\\{0}\{1}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.GetLastWriteTime(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_last_write_time()
         {
            // NOTE: when using %temp%, SetLastWriteTime was running afoul of the following IOException: 
            // The process cannot access the file 'c:\temp' because it is being used by another process.
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_get_the_last_write_time));
            _target.CreateDirectory(path);
            try
            {
               var expected = DateTimeOffset.UtcNow;
               _target.SetLastWriteTime(path, expected);
               _target.GetLastWriteTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_GetRandomDirectoryName : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_directory_name_that_is_not_rooted_and_does_not_exist()
         {
            var actual = _target.GetRandomDirectoryName();
            _target.DirectoryExists(actual).Should().BeFalse();
            _pathUtilities.IsPathRelative(actual).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_GetTemporaryDirectoryPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Fails on build server:  expected is a 8.3 dir name, actual is full dir name")]
         public void It_should_return_the_temporary_directory_path_for_the_current_user()
         {
            var expected = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_environmentUtilities.ExpandEnvironmentVariables("%temp%"));
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_target.GetTemporaryDirectoryPath());
            actual.Should().Be(expected);
         }
      }
   }
}
