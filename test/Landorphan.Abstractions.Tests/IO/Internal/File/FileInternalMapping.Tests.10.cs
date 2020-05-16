﻿namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using Landorphan.Abstractions.IO.Internal;
    using Landorphan.Abstractions.Tests.TestFacilities;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.TestFacilities;
    using Landorphan.TestUtilities.TestFilters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
       [TestClass]
      public class When_I_call_FileInternalMapping_WriteAllBytes : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_bytes_are_empty_It_should_create_an_empty_file_or_clear_an_existing_file()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});

               _target.WriteAllBytes(path, Array.Empty<byte>());
               _target.ReadAllBytes(path).Should().BeEmpty();

               _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());

               _target.WriteAllBytes(path, Array.Empty<byte>().ToImmutableList());
               _target.ReadAllBytes(path).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});

               _target.WriteAllBytes(path, Array.Empty<byte>());
               _target.ReadAllBytes(path).Should().BeEmpty();
               _target.DeleteFile(path);

               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());

               _target.WriteAllBytes(path, Array.Empty<byte>().ToImmutableList());
               _target.ReadAllBytes(path).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_bytes_are_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.WriteAllBytes(path, (byte[])null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("bytes");

               throwingAction = () => _target.WriteAllBytes(path, (IImmutableList<byte>)null);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("bytes");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_create_the_file_and_write_the_bytes()
         {
            IImmutableList<byte> expected = new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();

               _target.WriteAllBytes(path, expected.ToArray());

               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();

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
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(Spaces + filePath, new byte[] {0x00, 0x01, 0x02, 0x03});
               _target.WriteAllBytes(Spaces + filePath, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
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
               _target.WriteAllBytes(filePath + Spaces, new byte[] {0x00, 0x01, 0x02, 0x03});
               _target.WriteAllBytes(filePath + Spaces, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
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
            Action throwingAction = () => _target.WriteAllBytes(string.Empty, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllBytes(string.Empty, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.WriteAllBytes(null, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.WriteAllBytes(null, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_write_the_file()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var expected = new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllBytes(path, expected.ToArray());
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
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
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = string.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");
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

            var fileFullPath = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var fileFullPath = _tempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const string fileFullPath = " \t ";

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");

            throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_replace_the_contents_of_the_file()
         {
            IImmutableList<byte> first = new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            IImmutableList<byte> second = new byte[] {0x04, 0x05, 0x06, 0x07}.ToImmutableList();

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, first.ToArray());
               _target.ReadAllBytes(path).Should().BeEquivalentTo(first);

               _target.WriteAllBytes(path, second.ToArray());
               _target.ReadAllBytes(path).Should().BeEquivalentTo(second);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, first);
               _target.ReadAllBytes(path).Should().BeEquivalentTo(first);

               _target.WriteAllBytes(path, second);
               _target.ReadAllBytes(path).Should().BeEquivalentTo(second);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const string path = ":abcd";

            Action throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_write_the_bytes()
         {
            var expected = new byte[] {0x01, 0x01, 0x02, 0x03}.ToImmutableList();

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, expected);
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, expected.ToArray());
               _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_WriteAllLines : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_empty_It_should_create_an_empty_file_or_clear_an_existing_file()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path, Array.Empty<string>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path, (IEnumerable<string>)Array.Empty<string>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path, Array.Empty<string>().ToImmutableList(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllLines(path, Array.Empty<string>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllLines(path, (IEnumerable<string>)Array.Empty<string>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllLines(path, Array.Empty<string>().ToImmutableList(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.WriteAllLines(path, (string[])null, Encoding.UTF8);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");

               throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)null, Encoding.UTF8);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");

               throwingAction = () => _target.WriteAllLines(path, (IImmutableList<string>)null, Encoding.UTF8);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_encoding_is_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("encoding");

               throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, null);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("encoding");

               throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), null);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("encoding");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_create_the_file_and_write_the_bytes()
         {
            var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllLines(path, expected.ToArray(), Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
               _target.WriteAllLines(path, (IEnumerable<string>)expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
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
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(Spaces + path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(Spaces + path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(Spaces + path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(path + Spaces, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path + Spaces, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path + Spaces, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.WriteAllLines(string.Empty, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllLines(string.Empty, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllLines(string.Empty, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.WriteAllLines(null, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.WriteAllLines(null, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.WriteAllLines(null, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_write_the_file()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllLines(path, expected.ToArray(), Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
               _target.WriteAllLines(path, (IEnumerable<string>)expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
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
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = string.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");
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

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const string path = " \t ";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_replace_the_contents_of_the_file()
         {
            var first = new[] {"zero", "one", "two", "three"}.ToImmutableList();
            var second = new[] {"4", "5", "6", "7"}.ToImmutableList();

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(path, first.ToArray(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(first);
               _target.WriteAllLines(path, second.ToArray(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(second);

               _target.WriteAllLines(path, (IEnumerable<string>)first, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(first);
               _target.WriteAllLines(path, (IEnumerable<string>)second, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(second);

               _target.WriteAllLines(path, first, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(first);
               _target.WriteAllLines(path, second, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(second);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const string path = ":abcd";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<string>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_write_the_contents()
         {
            var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(path, expected.ToArray(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.WriteAllLines(path, (IEnumerable<string>)expected, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_WriteAllText : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_empty_It_should_create_an_empty_file_or_clear_an_existing_file()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(path, "contents", Encoding.UTF8);
               _target.WriteAllText(path, string.Empty, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllText(path, string.Empty, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.WriteAllText(path, null, Encoding.UTF8);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_encoding_is_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.WriteAllText(path, "contents", null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("encoding");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
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

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_create_the_file_and_write_the_bytes()
         {
            const string expected = "contents";

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllText(path, expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllText(path, Encoding.UTF8).Should().Be(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(Spaces + path, "contents", Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(path + Spaces, "contents", Encoding.UTF8);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            Action throwingAction = () => _target.WriteAllText(string.Empty, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.WriteAllText(null, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_write_the_file()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            const string expected = "contents";
            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllText(path, expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllText(path, Encoding.UTF8).Should().Be(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = string.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");
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

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();
            var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const string path = " \t ";

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();
            _target.FileExists(path).Should().BeFalse();

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_replace_the_contents_of_the_file()
         {
            const string first = "first";
            const string second = "this\nis\the\nsecond";

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(path, first, Encoding.UTF8);
               _target.ReadAllText(path, Encoding.UTF8).Should().Be(first);
               _target.WriteAllText(path, second, Encoding.UTF8);
               _target.ReadAllText(path, Encoding.UTF8).Should().Be(second);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const string path = ":abcd";

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_write_the_contents()
         {
            const string expected = "zero\r\none\r\ntwo\r\nthree";

            var path = _target.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(path, expected, Encoding.UTF8);
               _target.ReadAllText(path, Encoding.UTF8).Should().Be(expected);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_service_locate_IFileInternalMapping : ArrangeActAssert
      {
          private IFileInternalMapping actual;

          protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IFileInternalMapping>();
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_FileInternalMapping()
         {
            actual.Should().BeOfType<FileInternalMapping>();
         }
      }
   }
}
