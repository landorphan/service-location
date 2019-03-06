namespace Landorphan.Abstractions.Tests.IO.Internal.File
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Landorphan.TestUtilities.TestFilters;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
      private const String Spaces = "   ";
      private static readonly IDirectoryInternalMapping _directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
      private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
      private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
      private static readonly FileInternalMapping _target = new FileInternalMapping();
      private static readonly String _tempPath = _directoryInternalMapping.GetTemporaryDirectoryPath();

      [TestClass]
      public class When_I_call_FileInternalMapping_AppendAllLines : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_empty_It_should_throw_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _target.CreateTemporaryFile();
            var contents = Array.Empty<String>();

            try
            {
               _target.AppendAllLines(path, contents, Encoding.ASCII);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            const IEnumerable<String> contents = null;

            try
            {
               Action throwingAction = () => _target.AppendAllLines(path, contents, Encoding.ASCII);
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
         public void And_the_contents_contain_nulls_It_should_convert_the_nulls_to_empty()
         {
            // HAPPY PATH TEST:
            var path = _target.CreateTemporaryFile();
            var contents = new[] {"one", null, "three", null, "five"};
            var expected = (from e in contents select e ?? String.Empty).ToList();

            try
            {
               _target.AppendAllLines(path, contents, Encoding.ASCII);
               var actualLines = _target.ReadAllLines(path, Encoding.ASCII);
               actualLines.Should().Contain(expected);
               actualLines.Count.Should().Be(expected.Count);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_name_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               var contents = new[] {"one", "two", "three"};
               var path = _pathUtilities.GetParentPath(unalteredPath) + Spaces + _pathUtilities.DirectorySeparatorCharacter + _pathUtilities.GetFileName(unalteredPath);

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               actualLines.Count.Should().Be(contents.Length);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_encoding_is_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            var contents = new[] {"one", "two", "three"};

            try
            {
               Action throwingAction = () => _target.AppendAllLines(path, contents, null);
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
         public void And_the_file_name_has_leading_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var unalteredPath = _target.CreateTemporaryFile();
            var path = _pathUtilities.GetParentPath(unalteredPath) + _pathUtilities.DirectorySeparatorCharacter + Spaces + _pathUtilities.GetFileName(unalteredPath);
            try
            {
               var contents = new[] {"one", "two", "three"};

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               actualLines.Count.Should().Be(contents.Length);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            // TODO: change the exception to an ArgumentException
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var contents = new[] {"one", "two", "three"};

            var enc = new UTF8Encoding(false, true);
            Action throwingAction = () => _target.AppendAllLines(path, contents, enc);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var contents = new[] {"one", "two", "three"};

            // invalid character in the path before the file name.
            var invalidFilePathDirectoryPath = String.Format(
               CultureInfo.InvariantCulture,
               @"{0}\{1}\{2}",
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt");

            var enc = new UTF8Encoding(false, true);
            Action throwingAction = () => _target.AppendAllLines(invalidFilePathDirectoryPath, contents, enc);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            // invalid character in the file name
            var invalidFilePathFileName = String.Format(
               CultureInfo.InvariantCulture,
               @"{0}\{1}\{2}",
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + ".txt");

            throwingAction = () => _target.AppendAllLines(invalidFilePathFileName, contents, enc);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_filename_It_should_create_the_file_and_append_the_lines()
         {
            var path = _target.CreateTemporaryFile();
            _target.DeleteFile(path);

            var contents = new[] {"one", "two", "three"};

            try
            {
               _target.FileExists(path).Should().BeFalse();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               _target.FileExists(path).Should().BeTrue();
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               _target.DeleteFile(path);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_in_the_parent_directory_It_should_create_the_directory_and_the_file_and_append_the_lines()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt");
            var contents = new[] {"one", "two", "three"};

            try
            {
               _target.FileExists(path).Should().BeFalse();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               _target.FileExists(path).Should().BeTrue();
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               _directoryInternalMapping.DeleteRecursively(_pathUtilities.GetParentPath(path));
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(_pathUtilities.GetParentPath(path));
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_match_an_existing_file_It_should_create_the_file_and_append_the_lines()
         {
            var contents = new[] {"one", "two", "three"};
            var path = _pathUtilities.Combine(
               _tempPath,
               "And_the_path_does_not_match_an_existing_file_It_should_create_the_file_and_append_the_lines.txt");
            try
            {
               _target.FileExists(path).Should().BeFalse();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               _target.FileExists(path).Should().BeTrue();
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
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
            // HAPPY PATH TEST:
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               var contents = new[] {"one", "two", "three"};
               var path = Spaces + unalteredPath;

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               actualLines.Count.Should().Be(contents.Length);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               var contents = new[] {"one", "two", "three"};
               var path = unalteredPath + Spaces;

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
               actualLines.Count.Should().Be(contents.Length);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;
            var contents = new[] {"one", "two", "three"};

            var enc = new UTF8Encoding(false, true);
            Action throwingAction = () => _target.AppendAllLines(path, contents, enc);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            var contents = new[] {"one", "two", "three"};

            var enc = new UTF8Encoding(false, true);
            Action throwingAction = () => _target.AppendAllLines(null, contents, enc);
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
            _target.FileExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            var dirName = _pathUtilities.GetParentPath(path);

            var contents = new[] {"one", "two", "three"};

            Action throwingAction = () => _target.AppendAllLines(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(dirName));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            var contents = new[] {"one", "two", "three"};

            Action throwingAction = () => _target.AppendAllLines(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            // with tab
            const String path0 = " \t ";
            var contents = new[] {"one", "two", "three"};

            Action throwingAction = () => _target.AppendAllLines(path0, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            // without tab
            const String path1 = Spaces;
            throwingAction = () => _target.AppendAllLines(path1, contents, Encoding.ASCII);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_it_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _directoryInternalMapping.CreateDirectory(path);
            var contents = new[] {"one", "two", "three"};

            try
            {
               _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();

               Action throwingAction = () => _target.AppendAllLines(path, contents, Encoding.ASCII);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create the file");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("because a directory with the same name already exists.");
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_existing_file_It_should_append_the_lines()
         {
            // HAPPY PATH TEST:
            var path = _target.CreateTemporaryFile();
            var contents = new[] {"one", "two", "three"};

            try
            {
               _target.FileExists(path).Should().BeTrue();

               _target.AppendAllLines(path, contents, Encoding.ASCII);
               var actualLines = _target.ReadAllLines(path, Encoding.ASCII);
               actualLines.Should().Contain(contents);
               actualLines.Count.Should().Be(contents.Length);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_append_the_lines()
         {
            var contents = new[] {"one", "two", "three"};
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt");
            _target.CreateFile(path);
            try
            {
               _target.FileExists(path).Should().BeTrue();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_append_the_lines()
         {
            var contents = new[] {"one", "two", "three"};
            var path = _target.CreateTemporaryFile();
            try
            {
               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _target.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_FileInternalMapping_AppendAllText : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_empty_It_should_throw_not_throw()
         {
            var path = _target.CreateTemporaryFile();
            var contents = String.Empty;

            try
            {
               _target.AppendAllText(path, contents, Encoding.ASCII);
            }
            finally
            {
               _target.DeleteFile(path);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contents_are_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            const String contents = null;

            try
            {
               Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
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
         public void And_the_directory_name_has_trailing_spaces_It_should_not_throw()
         {
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               const String contents = "Abc123";
               var path = _pathUtilities.GetParentPath(unalteredPath) + Spaces + _pathUtilities.DirectorySeparatorCharacter + _pathUtilities.GetFileName(unalteredPath);

               _target.AppendAllText(path, contents, Encoding.UTF8);
               var actual = _target.ReadAllText(path, Encoding.UTF8);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_encoding_is_null_It_should_throw_ArgumentNullException()
         {
            var path = _target.CreateTemporaryFile();
            const String contents = "Abc123";

            try
            {
               Action throwingAction = () => _target.AppendAllText(path, contents, null);
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
         public void And_the_file_name_has_leading_spaces_It_should_not_throw()
         {
            var unalteredPath = _target.CreateTemporaryFile();
            var path = _pathUtilities.GetParentPath(unalteredPath) + _pathUtilities.DirectorySeparatorCharacter + Spaces + _pathUtilities.GetFileName(unalteredPath);
            try
            {
               const String contents = "Abc123";

               _target.AppendAllText(path, contents, Encoding.ASCII);
               var actual = _target.ReadAllText(path, Encoding.ASCII);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
               _target.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [RunTestOnlyOnWindows]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            const String contents = "Abc123";

            // invalid character in the path before the file name.
            var invalidFilePathDirectoryPath = String.Format(
               CultureInfo.InvariantCulture,
               @"{0}\{1}\{2}",
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|",
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt");

            Action throwingAction = () => _target.AppendAllText(invalidFilePathDirectoryPath, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            // invalid character in the file name
            var invalidFilePathFileName = String.Format(
               CultureInfo.InvariantCulture,
               @"{0}\{1}\{2}",
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "|" + ".txt");

            throwingAction = () => _target.AppendAllText(invalidFilePathFileName, contents, Encoding.ASCII);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_in_the_parent_directory_It_should_create_the_directory_and_the_file_and_append_the_text()
         {
            const String contents = "one\r\ntwo\r\nthree";
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
               "And_the_path_does_not_exist_in_the_parent_directory_It_should_create_the_directory_and_the_file_and_append_the_text.txt");

            try
            {
               _target.FileExists(path).Should().BeFalse();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllText(path, contents, enc);
               _target.FileExists(path).Should().BeTrue();
               var actual = _target.ReadAllText(path, enc);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(_pathUtilities.GetParentPath(path));
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_match_an_existing_file_It_should_create_the_file_and_append_the_text()
         {
            const String contents = "one\r\ntwo\r\nthree";
            var path = _pathUtilities.Combine(
               _tempPath,
               "And_the_path_does_not_match_an_existing_file_It_should_create_the_file_and_append_the_text.txt");
            try
            {
               _target.FileExists(path).Should().BeFalse();

               var enc = new UTF8Encoding(false, true);
               _target.AppendAllText(path, contents, enc);
               _target.FileExists(path).Should().BeTrue();
               var actual = _target.ReadAllText(path, enc);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
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
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               const String contents = "Abc123";
               var path = Spaces + unalteredPath;

               _target.AppendAllText(path, contents, Encoding.ASCII);
               var actual = _target.ReadAllText(path, Encoding.ASCII);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var unalteredPath = _target.CreateTemporaryFile();
            try
            {
               const String contents = "Abc123";
               var path = unalteredPath + Spaces;

               _target.AppendAllText(path, contents, Encoding.ASCII);
               var actual = _target.ReadAllText(path, Encoding.ASCII);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _target.DeleteFile(unalteredPath);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;
            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(null, contents, Encoding.ASCII);
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
            _target.FileExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

            var dirName = _pathUtilities.GetParentPath(path);

            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(dirName));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path0 = " \t ";
            const String contents = "Abc123";

            Action throwingAction = () => _target.AppendAllText(path0, contents, Encoding.ASCII);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_it_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            _directoryInternalMapping.CreateDirectory(path);
            const String contents = "Abc123";

            try
            {
               _directoryInternalMapping.DirectoryExists(path).Should().BeTrue();

               Action throwingAction = () => _target.AppendAllText(path, contents, Encoding.ASCII);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create the file");
               e.And.Message.Should().Contain(path);
               e.And.Message.Should().Contain("because a directory with the same name already exists.");
            }
            finally
            {
               _directoryInternalMapping.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_existing_file_It_should_append_the_text()
         {
            var path = _target.CreateTemporaryFile();
            const String contents = "Abc123";

            try
            {
               _target.FileExists(path).Should().BeTrue();

               _target.AppendAllText(path, contents, Encoding.ASCII);
               var actual = _target.ReadAllText(path, Encoding.ASCII);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _target.DeleteFile(path);
            }
         }
      }
   }
}
