namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Collections.Immutable;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.Attributes;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_FileInternalMapping_Open : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.Open(path, FileMode.Create);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
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

            Action throwingAction = () => _target.Open(path, FileMode.Create);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_and_FileMode_Create_is_not_set_It_should_throw_FileNotFoundException()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.Open(fileFullPath, FileMode.Open);
            var e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find file '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find file '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<FileNotFoundException>();
            e.And.Message.Should().Contain("Could not find file '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_and_FileMode_Create_is_set_It_should_create_the_file()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            try
            {
               using (_target.Open(fileFullPath, FileMode.Create))
               {
                  _target.FileExists(fileFullPath).Should().BeTrue();
               }
            }
            finally
            {
               _target.DeleteFile(fileFullPath);
            }

            try
            {
               using (_target.Open(fileFullPath, FileMode.Create, FileAccess.ReadWrite))
               {
                  _target.FileExists(fileFullPath).Should().BeTrue();
               }
            }
            finally
            {
               _target.DeleteFile(fileFullPath);
            }

            try
            {
               using (_target.Open(fileFullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
               {
                  _target.FileExists(fileFullPath).Should().BeTrue();
               }
            }
            finally
            {
               _target.DeleteFile(fileFullPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var filePath = _target.CreateTemporaryFile();
            try
            {
               using (_target.Open(Spaces + filePath, FileMode.Open))
               {
                  // no exception thrown, closing the stream.
               }

               using (_target.Open(Spaces + filePath, FileMode.Open, FileAccess.ReadWrite))
               {
                  // no exception thrown, closing the stream.
               }

               using (_target.Open(Spaces + filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
               {
                  // no exception thrown, closing the stream.
               }
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
               using (_target.Open(filePath + Spaces, FileMode.Open))
               {
                  // no exception thrown, closing the stream.
               }

               using (_target.Open(filePath + Spaces, FileMode.Open, FileAccess.ReadWrite))
               {
                  // no exception thrown, closing the stream.
               }

               using (_target.Open(filePath + Spaces, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
               {
                  // no exception thrown, closing the stream.
               }
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
            Action throwingAction = () => _target.Open(String.Empty, FileMode.Open);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.Open(String.Empty, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.Open(String.Empty, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.Open(null, FileMode.Open);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.Open(null, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.Open(null, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_IOException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.Open(path, FileMode.Open);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");

            throwingAction = () => _target.Open(path, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");

            throwingAction = () => _target.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");
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

            var fileFullPath = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.Open(fileFullPath, FileMode.Open);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(fileFullPath);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var fileFullPath = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.Open(fileFullPath, FileMode.Open);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.Open(fileFullPath, FileMode.Open);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.Open(fileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();
            _target.FileExists(_tempPath).Should().BeFalse();

            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();

            Action throwingAction = () => _target.Open(_tempPath, FileMode.Create);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().StartWith("Cannot create the file");
            e.And.Message.Should().EndWith("because a directory with the same name already exists.");

            throwingAction = () => _target.Open(_tempPath, FileMode.Open, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().StartWith("Cannot open the file");
            e.And.Message.Should().EndWith("because a directory with the same name already exists.");

            throwingAction = () => _target.Open(_tempPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().StartWith("Cannot open the file");
            e.And.Message.Should().EndWith("because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.Open(path, FileMode.Create);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_local()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
               _target.WriteAllBytes(path, expected);
               _target.FileExists(path).Should().BeTrue();

               using (var fs = _target.Open(path, FileMode.Open))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }

               using (var fs = _target.Open(path, FileMode.Open, FileAccess.Read))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }

               using (var fs = _target.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.WriteAllBytes(path, expected);
               _target.FileExists(path).Should().BeTrue();

               using (var fs = _target.Open(path, FileMode.Open))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }

               using (var fs = _target.Open(path, FileMode.Open, FileAccess.Read))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }

               using (var fs = _target.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_OpenRead : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.OpenRead(path);
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

            Action throwingAction = () => _target.OpenRead(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.OpenRead(fileFullPath);
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
               using (_target.OpenRead(Spaces + filePath))
               {
                  // no exception thrown, closing the stream.
               }
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
               using (_target.OpenRead(filePath + Spaces))
               {
                  // no exception thrown, closing the stream.
               }
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
            Action throwingAction = () => _target.OpenRead(String.Empty);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.OpenRead(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_IOException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.OpenRead(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");
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

            var fileFullPath = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.OpenRead(fileFullPath);
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

            Action throwingAction = () => _target.OpenRead(fileFullPath);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.OpenRead(fileFullPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();
            _target.FileExists(_tempPath).Should().BeFalse();

            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();

            Action throwingAction = () => _target.OpenRead(_tempPath);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().StartWith("Cannot open the file");
            e.And.Message.Should().EndWith("because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.OpenRead(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_local()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
               _target.WriteAllBytes(path, expected);
               _target.FileExists(path).Should().BeTrue();

               using (var fs = _target.OpenRead(path))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               _target.WriteAllBytes(path, expected);
               _target.FileExists(path).Should().BeTrue();

               using (var fs = _target.OpenRead(path))
               {
                  var buffer = new Byte[4];
                  fs.Read(buffer, 0, 4);
                  buffer.Should().BeEquivalentTo(expected);
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_OpenText : AbstractionTestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.OpenText(path);
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

            Action throwingAction = () => _target.OpenText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_throw_FileNotFoundException()
         {
            var fileFullPath = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.OpenText(fileFullPath);
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
               using (_target.OpenText(Spaces + filePath))
               {
                  // no exception thrown, closing the stream.
               }
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
               using (_target.OpenText(filePath + Spaces))
               {
                  // no exception thrown, closing the stream.
               }
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
            Action throwingAction = () => _target.OpenText(String.Empty);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.OpenText(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_IOException()
         {
            var path = String.Format(
               CultureInfo.InvariantCulture,
               @"\\{0}\{1}\{2}",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".tmp");

            Action throwingAction = () => _target.OpenText(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("The network path was not found");
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

            var fileFullPath = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            _directoryInternalMapping.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            Action throwingAction = () => _target.OpenText(fileFullPath);
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

            Action throwingAction = () => _target.OpenText(fileFullPath);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String fileFullPath = " \t ";

            Action throwingAction = () => _target.OpenText(fileFullPath);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_throw_IOException()
         {
            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();
            _target.FileExists(_tempPath).Should().BeFalse();

            _directoryInternalMapping.DirectoryExists(_tempPath).Should().BeTrue();

            Action throwingAction = () => _target.OpenText(_tempPath);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().StartWith("Cannot open the file");
            e.And.Message.Should().EndWith("because a directory with the same name already exists.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [WindowsTestOnly]
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.OpenText(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_local()
         {
            var path = _target.CreateTemporaryFile();
            try
            {
               var expected = new[] {"Line 0", "Line 1", "Line2", "Line 3"}.ToImmutableList();
               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();

               using (var sr = _target.OpenText(path))
               {
                  var actual = ImmutableList<String>.Empty.ToBuilder();
                  var line = sr.ReadLine();
                  while (line != null)
                  {
                     actual.Add(line);
                     line = sr.ReadLine();
                  }

                  actual.SequenceEqual(expected).Should().BeTrue();
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_open_the_file_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            var path = _pathUtilities.Combine(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

            try
            {
               var expected = new[] {"Line 0", "Line 1", "Line2", "Line 3"}.ToImmutableList();
               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();

               using (var sr = _target.OpenText(path))
               {
                  var actual = ImmutableList<String>.Empty.ToBuilder();
                  var line = sr.ReadLine();
                  while (line != null)
                  {
                     actual.Add(line);
                     line = sr.ReadLine();
                  }

                  actual.SequenceEqual(expected).Should().BeTrue();
               }
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }
   }
}
