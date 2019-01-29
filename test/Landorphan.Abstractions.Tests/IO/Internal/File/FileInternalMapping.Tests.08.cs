namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Globalization;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Common.Exceptions;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_SetAttributes : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_fileAttributes_is_invalid_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            const FileAttributes fileAttributes = (FileAttributes)Int32.MaxValue;
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
               var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
               e.And.ParamName.Should().Be("fileAttributes");
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
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;
            try
            {
               _target.SetAttributes(Spaces + path, fileAttributes);
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
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;
            try
            {
               _target.SetAttributes(path + Spaces, fileAttributes);
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
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            _directoryInternalMapping.DirectoryExists(@"A:\").Should().BeFalse();
            var path = @"A:\" + Guid.NewGuid();
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _directoryInternalMapping.CreateDirectory(path);
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;
            try
            {
               Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
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
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_FileNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            const FileAttributes fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;

            Action throwingAction = () => _target.SetAttributes(path, fileAttributes);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_attributes()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var fileAttributes = FileAttributes.Archive | FileAttributes.Hidden;
               _target.SetAttributes(path, fileAttributes);
               _target.GetAttributes(path).Should().Be(fileAttributes);

               fileAttributes = FileAttributes.Normal;
               _target.SetAttributes(path, fileAttributes);
               _target.GetAttributes(path).Should().Be(fileAttributes);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_SetCreationTime : TestBase
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
            var path = _target.CreateTemporaryFile();
            try
            {
               Action throwingAction = () => _target.SetCreationTime(path, creationTime);
               var e = throwingAction.Should().Throw<ArgumentOutOfRangeException>();
               e.And.ParamName.Should().Be("creationTime");
               e.And.Message.Should().Be("The value must be greater than or equal to (504,911,232,000,000,001 ticks).\r\nParameter name: creationTime");
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
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
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
               _target.SetCreationTime(Spaces + path, DateTimeOffset.UtcNow);
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
               _target.SetCreationTime(path + Spaces, DateTimeOffset.UtcNow);
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

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Unmapped drive tests fail on build server")]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_FileNotFoundException()
         {
            _directoryInternalMapping.DirectoryExists(@"A:\").Should().BeFalse();
            var path = @"A:\" + Guid.NewGuid();

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _directoryInternalMapping.CreateDirectory(path);
            try
            {
               Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
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

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_FileNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_FileNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());

            Action throwingAction = () => _target.SetCreationTime(path, DateTimeOffset.UtcNow);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the file path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_creation_time()
         {
            var path = _target.CreateTemporaryFile();
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
               _target.DeleteFile(path);
            }
         }
      }
   }
}
