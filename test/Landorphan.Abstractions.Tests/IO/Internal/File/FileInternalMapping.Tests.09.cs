namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Collections.Immutable;
   using System.Globalization;
   using System.IO;
   using System.Linq;
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
      public class When_I_call_FileInternalMapping_SetLastAccessTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_lastAccessTime_is_greater_than_maximum_It_should_throw_ArgumentOutOfRangeException()
         {
            // Test cannot be written/executed because the maximum is at DateTimeOffset.MaxValue
            // (cannot add even one tick to the value).
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_lastAccessTime_is_less_than_minimum_It_should_throw_ArgumentOutOfRangeException()
         {
            var lastAccessTime = _target.MinimumFileTimeAsDateTimeOffset.Add(TimeSpan.FromTicks(-1));
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.SetLastAccessTime(path, lastAccessTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("lastAccessTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: lastAccessTime");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.SetLastAccessTime(Spaces + path, DateTimeOffset.UtcNow);
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
               _target.SetLastAccessTime(path + Spaces, DateTimeOffset.UtcNow);
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
            var path = String.Empty;

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _directoryInternalMapping.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("'.\r\nParameter name: path");
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_FileNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.SetLastAccessTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_access_time()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.SetLastAccessTime(path, _target.MinimumFileTimeAsDateTimeOffset);
               _target.GetLastAccessTime(path).Should().Be(_target.MinimumFileTimeAsDateTimeOffset);

               var expected = DateTimeOffset.UtcNow;
               _target.SetLastAccessTime(path, expected);
               _target.GetLastAccessTime(path).Should().Be(expected);

               _target.SetLastAccessTime(path, _target.MaximumFileTimeAsDateTimeOffset);
               _target.GetLastAccessTime(path).Should().Be(_target.MaximumFileTimeAsDateTimeOffset);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_SetLastWriteTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_lastWriteTime_is_greater_than_maximum_It_should_throw_ArgumentOutOfRangeException()
         {
            // Test cannot be written/executed because the maximum is at DateTimeOffset.MaxValue
            // (cannot add even one tick to the value).
            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_lastWriteTime_is_less_than_minimum_It_should_throw_ArgumentOutOfRangeException()
         {
            var lastWriteTime = _target.MinimumFileTimeAsDateTimeOffset.Add(TimeSpan.FromTicks(-1));
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.SetLastWriteTime(path, lastWriteTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("lastWriteTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: lastWriteTime");
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.SetLastWriteTime(Spaces + path, DateTimeOffset.UtcNow);
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
               _target.SetLastWriteTime(path + Spaces, DateTimeOffset.UtcNow);
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
            var path = String.Empty;

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();
            var path = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _directoryInternalMapping.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
               var e = throwingAction.Should().Throw<FileNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the file path '");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("'.\r\nParameter name: path");
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_FileNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            Action throwingAction = () => _target.SetLastWriteTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_write_time()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               _target.SetLastWriteTime(path, _target.MinimumFileTimeAsDateTimeOffset);
               _target.GetLastWriteTime(path).Should().Be(_target.MinimumFileTimeAsDateTimeOffset);

               var expected = DateTimeOffset.UtcNow;
               _target.SetLastWriteTime(path, expected);
               _target.GetLastWriteTime(path).Should().Be(expected);

               _target.SetLastWriteTime(path, _target.MaximumFileTimeAsDateTimeOffset);
               _target.GetLastWriteTime(path).Should().Be(_target.MaximumFileTimeAsDateTimeOffset);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

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
               _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});

               _target.WriteAllBytes(path, Array.Empty<Byte>());
               _target.ReadAllBytes(path).Should().BeEmpty();

               _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());

               _target.WriteAllBytes(path, Array.Empty<Byte>().ToImmutableList());
               _target.ReadAllBytes(path).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});

               _target.WriteAllBytes(path, Array.Empty<Byte>());
               _target.ReadAllBytes(path).Should().BeEmpty();
               _target.DeleteFile(path);

               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());

               _target.WriteAllBytes(path, Array.Empty<Byte>().ToImmutableList());
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
               Action throwingAction = () => _target.WriteAllBytes(path, (Byte[])null);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("bytes");

               throwingAction = () => _target.WriteAllBytes(path, (IImmutableList<Byte>)null);
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
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_create_the_file_and_write_the_bytes()
         {
            IImmutableList<Byte> expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
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

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
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
               _target.WriteAllBytes(Spaces + filePath, new Byte[] {0x00, 0x01, 0x02, 0x03});
               _target.WriteAllBytes(Spaces + filePath, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
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
               _target.WriteAllBytes(filePath + Spaces, new Byte[] {0x00, 0x01, 0x02, 0x03});
               _target.WriteAllBytes(filePath + Spaces, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
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
            Action throwingAction = () => _target.WriteAllBytes(String.Empty, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(String.Empty, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.WriteAllBytes(null, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.WriteAllBytes(null, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void And_the_path_is_on_a_known_host_and_known_share_it_should_write_the_file()
         {
            var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkUncShareEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
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
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            if (TestHardCodes.WindowsTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.UnmappedDrive)}");
               return;
            }

            var fileFullPath = TestHardCodes.WindowsTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var fileFullPath = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(fileFullPath, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
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

            Action throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");

            throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_replace_the_contents_of_the_file()
         {
            IImmutableList<Byte> first = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            IImmutableList<Byte> second = new Byte[] {0x04, 0x05, 0x06, 0x07}.ToImmutableList();

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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03});
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllBytes(path, new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList());
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_write_the_bytes()
         {
            var expected = new Byte[] {0x01, 0x01, 0x02, 0x03}.ToImmutableList();

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
   }
}
