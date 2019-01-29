namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming   

   [SuppressMessage("SonarLint.CodeSmell", "S4058: Overloads with a StringComparison parameter should be used")]
   public static partial class DirectoryInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryInternalMapping_DeleteRecursively : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_is_empty_It_should_delete_the_directory()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteRecursively(path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_is_not_empty_It_should_delete_the_directory()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var filePath = _pathUtilities.Combine(path, Guid.NewGuid() + ".tmp");

            // nested file
            _target.CreateDirectory(path);
            _fileInternalMapping.CreateFile(filePath);
            _fileInternalMapping.FileExists(filePath).Should().BeTrue();
            _target.DeleteRecursively(path);
            _target.DirectoryExists(path).Should().BeFalse();

            // nested directory
            var nestedPath = _pathUtilities.Combine(path, Guid.NewGuid().ToString());
            _target.CreateDirectory(path);
            _target.CreateDirectory(nestedPath);

            _target.DirectoryExists(nestedPath).Should().BeTrue();
            _target.DeleteRecursively(path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _target.DeleteRecursively(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteRecursively(Spaces + path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteRecursively(path + Spaces);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();

            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var filePath = _pathUtilities.Combine(path, Guid.NewGuid() + ".tmp");

            _target.CreateDirectory(path);
            _fileInternalMapping.CreateFile(filePath);
            try
            {
               _target.DirectoryExists(path).Should().BeTrue();
               _fileInternalMapping.FileExists(filePath).Should().BeTrue();

               Action throwingAction = () => _target.DeleteRecursively(filePath);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The directory name '");
               e.And.Message.Should().Contain(filePath);
               e.And.Message.Should().Contain("is invalid because a file with the same name already exists.");
               e.And.HResult.Should().Be(unchecked((Int32)0x8007010B));

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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.DeleteRecursively(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            _target.DeleteRecursively(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            _target.DeleteRecursively(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void It_should_delete_a_directory_with_files_and_subdirectories()
         {
            // absolute
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories");
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(_pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories"));
               _fileInternalMapping.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories.tmp"));
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // relative
            _target.SetCurrentDirectory(_tempPath);
            path = @".\" + Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories";
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(_pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories"));
               _fileInternalMapping.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories.tmp"));
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // unc
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories");
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(_pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories"));
               _fileInternalMapping.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + "It_should_delete_a_directory_with_files_and_subdirectories.tmp"));
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

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_DirectoryExists : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_return_false()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_return_false()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "<";
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_return_false()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_exists_with_leading_whitespace_It_should_return_true()
         {
            var paddedPath = Spaces + _tempPath;

            _target.DirectoryExists(_tempPath).Should().BeTrue();
            _target.DirectoryExists(paddedPath).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_exists_with_trailing_whitespace_It_should_return_true()
         {
            var paddedPath = _tempPath + Spaces;

            _target.DirectoryExists(_tempPath).Should().BeTrue();
            _target.DirectoryExists(paddedPath).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_return_false()
         {
            var path = String.Empty;
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_return_false()
         {
            String path = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_return_false()
         {
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_return_false()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_return_false()
         {
            const String path = " \t ";
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_return_false()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();
            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _fileInternalMapping.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_return_false()
         {
            const String path = ":";

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void It_should_distinguish_between_extant_and_non_extant_directories()
         {
            // absolute -- extant
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories");
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // absolute -- non-extant
            path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories");
            _target.DirectoryExists(path).Should().BeFalse();

            // relative -- extant
            _target.SetCurrentDirectory(_tempPath);
            path = @".\" + Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories";
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // relative -- non-extant
            path = @".\" + Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories";
            _target.DirectoryExists(path).Should().BeFalse();

            // unc -- extant
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories");
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // unc -- non-extant
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + "It_should_distinguish_between_extant_and_non_extant_directories");
            _target.DirectoryExists(path).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_EnumerateDirectories : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            const String expectedMessage = "The path is not well-formed (':' used outside the drive label).\r\nParameter name: path";

            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be(expectedMessage);

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be(expectedMessage);

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be(expectedMessage);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"c:\", Guid.NewGuid().ToString());

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // TODO: create a test against an existing directory that is left padded with children that are not left-padded.
            // Does the left-padding on the parent still cascade to the results?

            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString()))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               _pathUtilities.IsPathRooted(Spaces + outerFullPath).Should().BeTrue();

               var actual = _target.EnumerateDirectories(Spaces + outerFullPath);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.EnumerateDirectories(Spaces + outerFullPath, "*");
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.EnumerateDirectories(Spaces + outerFullPath, "*", SearchOption.AllDirectories);
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
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString()))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               var actual = _target.EnumerateDirectories(outerFullPath + Spaces);
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.EnumerateDirectories(outerFullPath + Spaces, "*");
               actual.Should().Contain(expected);
               foreach (var path in actual)
               {
                  // BCL will inject the "padding" if not handled by the wrapper.
                  path.Contains(Spaces).Should().BeFalse();
               }

               actual = _target.EnumerateDirectories(outerFullPath + Spaces, "*", SearchOption.AllDirectories);
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

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
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

               Action throwingAction = () => _target.EnumerateDirectories(path);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("Parameter name: path");

               throwingAction = () => _target.EnumerateDirectories(path, "*");
               e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("Parameter name: path");

               throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());

            Action throwingAction = () => _target.EnumerateDirectories(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*");
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_searchOption_is_unrecognized_It_should_throw_ArgumentOutOfRangeException()
         {
            const String path = @".\";
            const String SearchPattern = "*..";
            var searchOption = (SearchOption)(-5);

            Action throwingAction = () => _target.EnumerateDirectories(path, SearchPattern, searchOption);
            var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
            e.And.ParamName.Should().Be("searchOption");
            e.And.Message.Should().Be("Enum value was out of legal range.\r\nParameter name: searchOption");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_the_searchPattern_is_malformed_It_should_throw_ArgumentException()
         {
            const String path = @".\";
            const String SearchPattern = "*.|";

            Action throwingAction = () => _target.EnumerateDirectories(path, SearchPattern);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("searchPattern");
            e.And.Message.Should().Contain("Search pattern cannot contain");

            throwingAction = () => _target.EnumerateDirectories(path, SearchPattern, SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("searchPattern");
            e.And.Message.Should().Contain("Search pattern cannot contain");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_searchPattern_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = @".\";
            const String SearchPattern = null;

            Action throwingAction = () => _target.EnumerateDirectories(path, SearchPattern);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("searchPattern");

            throwingAction = () => _target.EnumerateDirectories(path, SearchPattern, SearchOption.AllDirectories);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("searchPattern");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_subdirectories()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()));
            var expected = new List<String>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString())),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString()))
            };

            try
            {
               _target.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _target.CreateDirectory(sd);
               }

               _target.EnumerateDirectories(outerFullPath).Should().Contain(expected);
               _target.EnumerateDirectories(Spaces + outerFullPath, "*").Should().Contain(expected);
               _target.EnumerateDirectories(Spaces + outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _target.DeleteRecursively(outerFullPath);
            }
         }
      }
   }
}
