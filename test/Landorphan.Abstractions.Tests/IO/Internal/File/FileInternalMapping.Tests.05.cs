namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Collections.Immutable;
   using System.Globalization;
   using System.IO;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_ReadAllBytes : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.ReadAllBytes(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.ReadAllBytes(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.ReadAllBytes(fileFullPath);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find file '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               _target.ReadAllBytes(Spaces + filePath);
            }
            finally
            {
               _target.DeleteFile(filePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               _target.ReadAllBytes(filePath + Spaces);
            }
            finally
            {
               _target.DeleteFile(filePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.ReadAllBytes(String.Empty);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.ReadAllBytes(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void And_the_path_is_on_a_known_host_and_known_share_it_read_the_file()
         {
            var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.WriteAllBytes(path, expected);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_IOException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.ReadAllBytes(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            var fileFullPath = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.ReadAllBytes(fileFullPath);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var fileFullPath = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            Action throwingAction = () => _target.ReadAllBytes(fileFullPath);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.ReadAllBytes(fileFullPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.ReadAllBytes(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The file name is invalid.  The path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' is a directory.");

            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.ReadAllBytes(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_read_all_bytes()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
               _target.WriteAllBytes(path, expected);
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_ReadAllLines : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_encoding_is_null_It_should_throw_ArgumentNullException()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.ReadAllLines(filePath, null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("encoding");
            }
            finally
            {
               _target.DeleteFile(filePath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.ReadAllLines(path, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.ReadAllLines(path, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.ReadAllLines(fileFullPath, Encoding.UTF8);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find file '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               _target.ReadAllLines(Spaces + filePath, Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(filePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               _target.ReadAllLines(filePath + Spaces, Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(filePath);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.ReadAllLines(String.Empty, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.ReadAllLines(null, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void And_the_path_is_on_a_known_host_and_known_share_it_read_the_file()
         {
            var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();

            var path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();

               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_IOException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.ReadAllLines(path, Encoding.UTF8);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            var fileFullPath = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.ReadAllLines(fileFullPath, Encoding.UTF8);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var fileFullPath = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            Action throwingAction = () => _target.ReadAllLines(fileFullPath, Encoding.UTF8);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.ReadAllLines(fileFullPath, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.ReadAllLines(path, Encoding.UTF8);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The file name is invalid.  The path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' is a directory.");

            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.ReadAllLines(path, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_read_all_lines()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();
               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }
   }
}
