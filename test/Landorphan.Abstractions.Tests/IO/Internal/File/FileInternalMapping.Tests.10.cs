namespace Landorphan.Abstractions.Tests.IO.Internal.File
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
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class FileInternalMapping_Tests
   {
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
               _target.WriteAllLines(path, Array.Empty<String>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path, (IEnumerable<String>)Array.Empty<String>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
               _target.WriteAllLines(path, Array.Empty<String>().ToImmutableList(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();
            }
            finally
            {
               _target.DeleteFile(path);
            }

            path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllLines(path, Array.Empty<String>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllLines(path, (IEnumerable<String>)Array.Empty<String>(), Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllLines(path, Array.Empty<String>().ToImmutableList(), Encoding.UTF8);
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
               Action throwingAction = () => _target.WriteAllLines(path, (String[])null, Encoding.UTF8);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");

               throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)null, Encoding.UTF8);
               e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("contents");

               throwingAction = () => _target.WriteAllLines(path, (IImmutableList<String>)null, Encoding.UTF8);
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

               throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, null);
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
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               _target.FileExists(path).Should().BeFalse();
               _target.WriteAllLines(path, expected.ToArray(), Encoding.UTF8);
               _target.FileExists(path).Should().BeTrue();
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.DeleteFile(path);
               _target.WriteAllLines(path, (IEnumerable<String>)expected, Encoding.UTF8);
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
               _target.WriteAllLines(Spaces + path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
               _target.WriteAllLines(path + Spaces, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
            Action throwingAction = () => _target.WriteAllLines(String.Empty, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(String.Empty, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(String.Empty, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.WriteAllLines(null, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.WriteAllLines(null, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
               _target.WriteAllLines(path, (IEnumerable<String>)expected, Encoding.UTF8);
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
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() + ".tmp");

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(_pathUtilities.GetParentPath(path));
            e.And.Message.Should().Contain("'.");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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
            const String path = " \t ";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}.ToImmutableList(), Encoding.UTF8);
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

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Cannot create the file '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("' because a directory with the same name already exists.");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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

               _target.WriteAllLines(path, (IEnumerable<String>)first, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(first);
               _target.WriteAllLines(path, (IEnumerable<String>)second, Encoding.UTF8);
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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.WriteAllLines(path, new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.WriteAllLines(path, (IEnumerable<String>)new[] {"zero", "one", "two", "three"}, Encoding.UTF8);
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

               _target.WriteAllLines(path, (IEnumerable<String>)expected, Encoding.UTF8);
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
               _target.WriteAllText(path, String.Empty, Encoding.UTF8);
               _target.ReadAllLines(path, Encoding.UTF8).Should().BeEmpty();

               _target.DeleteFile(path);
               _target.WriteAllText(path, String.Empty, Encoding.UTF8);
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
            const String expected = "contents";

            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
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
            Action throwingAction = () => _target.WriteAllText(String.Empty, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
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

            const String expected = "contents";
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
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() + ".tmp");

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
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
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
            const String first = "first";
            const String second = "this\nis\the\nsecond";

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
         public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            // ReSharper disable once StringLiteralTypo
            const String path = ":abcd";

            Action throwingAction = () => _target.WriteAllText(path, "contents", Encoding.UTF8);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_write_the_contents()
         {
            const String expected = "zero\r\none\r\ntwo\r\nthree";

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
