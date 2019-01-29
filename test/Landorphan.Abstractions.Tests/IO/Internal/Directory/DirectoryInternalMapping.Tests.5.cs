namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming   

   public static partial class DirectoryInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryInternalMapping_SetCreationTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         // [Ignore]
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
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
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
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";
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
            var path = _pathUtilities.Combine(@"c:\", Guid.NewGuid().ToString());
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
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();
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
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetCreationTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
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
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + "It_should_set_the_creation_time");
            _target.CreateDirectory(path);
            try
            {
               _target.SetCreationTime(path, _target.MinimumFileTimeAsDateTimeOffset);
               _target.GetCreationTime(path).Should().Be(_target.MinimumFileTimeAsDateTimeOffset);

               var expected = DateTimeOffset.UtcNow;
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

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_SetLastAccessTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         // [Ignore]
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
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _target.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetLastAccessTime(path, lastAccessTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("lastAccessTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: lastAccessTime");
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"c:\", Guid.NewGuid().ToString());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
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

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
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
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
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

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
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

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
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

               Action throwingAction = () => _target.SetLastAccessTime(path, value);
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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastAccessTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_access_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + "It_should_set_the_last_access_time");
            _target.CreateDirectory(path);
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
               _target.DeleteRecursively(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_SetLastWriteTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         // [Ignore]
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
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _target.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetLastWriteTime(path, lastWriteTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("lastWriteTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: lastWriteTime");
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"c:\", Guid.NewGuid().ToString());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
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

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
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
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
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

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
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

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
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

               Action throwingAction = () => _target.SetLastWriteTime(path, value);
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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String path = ":";
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            var value = new DateTimeOffset(DateTimeOffset.UtcNow.Ticks, TimeSpan.Zero);

            Action throwingAction = () => _target.SetLastWriteTime(path, value);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_write_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + "It_should_set_the_last_write_time");
            _target.CreateDirectory(path);
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
               _target.DeleteRecursively(path);
            }
         }
      }
   }
}
