namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class DirectoryInternalMapping_Tests
   {
      private const String Spaces = "   ";
      private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
      private static readonly IFileInternalMapping _fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
      private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
      private static readonly DirectoryInternalMapping _target = new DirectoryInternalMapping();
      private static readonly String _tempPath = _environmentUtilities.GetTemporaryDirectoryPath();

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_Copy : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_has_leading_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = Spaces + _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_has_trailing_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + Spaces;
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = String.Empty;
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_null_It_should_throw_ArgumentNullException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            String destDirName = null;
            try
            {
               _target.CreateDirectory(sourceDirName);

               // ReSharper disable once ExpressionIsAlwaysNull
               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<ArgumentNullException>();
               e.And.ParamName.Should().Be("destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<PathTooLongException>();
               e.And.Message.Should().StartWith("The path");
               e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_white_space_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            const String DestDirName = " \t ";

            Action throwingAction = () => _target.Copy(sourceDirName, DestDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("destDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: destDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_is_within_SourceDirName_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString());
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot copy to the destination directory");
               e.And.Message.Should().Contain(destDirName);
               e.And.Message.Should().Contain("because it is a subdirectory of the source directory");
               e.And.Message.Should().Contain(sourceDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_matches_an_existing_directory_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            try
            {
               _target.CreateDirectory(sourceDirName);
               _target.CreateDirectory(destDirName);

               _target.Copy(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_matches_an_existing_file_It_should_throw_IOException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _fileInternalMapping.CreateTemporaryFile();
            try
            {
               _target.CreateDirectory(sourceDirName);

               _fileInternalMapping.FileExists(destDirName).Should().BeTrue();

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot copy to the destination directory");
               e.And.Message.Should().Contain(destDirName);
               e.And.Message.Should().Contain("because a file with the same name already exists.");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _fileInternalMapping.DeleteFile(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_matches_sourceDirName_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = sourceDirName;
            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_destDirName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            const String DestDirName = ":";
            try
            {
               _target.CreateDirectory(sourceDirName);

               Action throwingAction = () => _target.Copy(sourceDirName, DestDirName);
               var e = throwingAction.Should().Throw<ArgumentException>();
               e.And.ParamName.Should().Be("destDirName");
               e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: destDirName");
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_and_DestDirName_do_not_share_a_common_root_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.LocalTestTargetRootFolder, Guid.NewGuid().ToString());

            _target.CreateDirectory(sourceDirName);
            try
            {
               _target.Copy(sourceDirName, destDirName);
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var sourceDirName = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();
            var destDirName = _tempPath + Guid.NewGuid();

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_does_not_exist_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_has_leading_spaces_It_should_not_throw()
         {
            var sourceDirName = Spaces + _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_has_trailing_spaces_It_should_not_throw()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + Spaces;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _target.CreateDirectory(sourceDirName);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_empty_It_should_throw_ArgumentException()
         {
            var sourceDirName = String.Empty;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_null_It_should_throw_ArgumentNullException()
         {
            String sourceDirName = null;
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            // ReSharper disable once ExpressionIsAlwaysNull
            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_too_long_It_should_throw_PathTooLongException()
         {
            var sourceDirName = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_is_white_space_It_should_throw_ArgumentException()
         {
            const String SourceDirName = " \t ";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(SourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
         {
            var sourceDirName = _fileInternalMapping.CreateTemporaryFile();
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _fileInternalMapping.FileExists(sourceDirName).Should().BeTrue();

               Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
               var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
               e.And.Message.Should().Contain("Could not find a part of the directory path");
               e.And.Message.Should().Contain(sourceDirName);
               e.And.Message.Should().Contain("Parameter name: sourceDirName");
            }
            finally
            {
               _fileInternalMapping.DeleteFile(sourceDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_starts_with_a_colon_It_should_throw_ArgumentException()
         {
            const String SourceDirName = ":";
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            Action throwingAction = () => _target.Copy(SourceDirName, destDirName);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("sourceDirName");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: sourceDirName");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var host = Guid.NewGuid().ToString();
            var share = Guid.NewGuid().ToString();

            var sourceDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid());
            var destDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", host, share, Guid.NewGuid());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_sourceDirName_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            const String Host = "localhost";
            var share = Guid.NewGuid().ToString();

            var sourceDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid());
            var destDirName = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}\{2}", Host, share, Guid.NewGuid());

            Action throwingAction = () => _target.Copy(sourceDirName, destDirName);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(sourceDirName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_copy_the_directory()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _target.CreateDirectory(sourceDirName);
               var filePath = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString());
               _fileInternalMapping.CreateFile(filePath);

               var childDirPath = _target.CreateDirectory(_pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString()));
               filePath = _pathUtilities.Combine(childDirPath, Guid.NewGuid().ToString());
               _fileInternalMapping.CreateFile(filePath);

               _target.Copy(sourceDirName, destDirName);

               _target.DirectoryExists(sourceDirName).Should().BeTrue();
               _target.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(sourceDirName);
               _target.DeleteRecursively(destDirName);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_CreateDirectory : TestBase
      {
         // TODO: add tests for directorySecurity

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_more_than_one_directory_in_the_path_does_not_exist_It_should_create_the_directory()
         {
            // HAPPY PATH TEST:
            var firstDirCreatedUnderTemp = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            var path = _pathUtilities.Combine(firstDirCreatedUnderTemp, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }

            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }

            firstDirCreatedUnderTemp.Last().Should().NotBe('\\');
            var pathWithSpaces = String.Format(
               CultureInfo.InvariantCulture,
               @"{0}\   {1}   \   {2}   \",
               firstDirCreatedUnderTemp,
               Guid.NewGuid(),
               Guid.NewGuid());
            try
            {
               _target.CreateDirectory(pathWithSpaces);
               _target.DirectoryExists(pathWithSpaces).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(firstDirCreatedUnderTemp);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_already_exists_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            // CreateDirectory()
            // arrange
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();

            // act
            _target.CreateDirectory(path);

            // assert
            _target.DirectoryExists(path).Should().BeTrue();

            // teardown
            _target.DeleteRecursively(path);

            //---

            // CreateDirectory(path, directorySecurity)
            // arrange
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();

            // act
            _target.CreateDirectory(path);

            // assert
            _target.DirectoryExists(path).Should().BeTrue();

            // teardown
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_does_not_exist_It_should_create_the_directory()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);

            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Maximum Directory length varies")]
         public void And_the_directory_path_is_at_the_maximum_length_It_should_create_the_directory()
         {
            // on a local drive, the drive label and the intervening backslashes count in the length that must be less than 248 characters.
            _tempPath.Last().Should().Be('\\');
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            // ensure the implementation allows for a trailing \
            path = path.Substring(0, TestHardCodes.PathAlwaysTooLong-1) + '\\';

            var directoryFullName = _target.CreateDirectory(path);
            _target.DirectoryExists(directoryFullName).Should().BeTrue();
            directoryFullName.Length.Should().Be(TestHardCodes.PathMaxDirLengthWithoutTrailingSepChar);
            _target.DeleteRecursively(path);

            // ReSharper disable once AccessToModifiedClosure
            Action throwingAction = () => _target.CreateDirectory(IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(path) + 'B');
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            // network shares stop at 247 regardless of the path length on disk
            TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl.Last().Should().NotBe('\\');
            path = TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl + '\\' + new String('A', 247);

            // ensure the implementation allows for a trailing \
            path = path.Substring(0, 247) + '\\';
            directoryFullName = _target.CreateDirectory(path);
            _target.DirectoryExists(directoryFullName).Should().BeTrue();
            directoryFullName.Length.Should().Be(TestHardCodes.PathMaxDirLengthWithoutTrailingSepChar);
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
         {
            var path = _tempPath + Guid.NewGuid() + ":" + Guid.NewGuid();

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void And_the_path_extends_a_known_host_and_known_share_it_should_not_throw()
         {
            var path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl, Guid.NewGuid().ToString());

            _target.DirectoryExists(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl).Should().BeTrue();
            _target.CreateDirectory(path);
            _target.DirectoryExists(path).Should().BeTrue();
            _target.DeleteRecursively(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            // when tested, the existence of a trailing backslash made no difference
            var path = String.Format(CultureInfo.InvariantCulture, @"   {0}\{1}\", _tempPath, Guid.NewGuid());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            // when tested, the existence of a trailing backslash made no difference
            var path = String.Format(CultureInfo.InvariantCulture, @"{0}\{1}\   ", _tempPath, Guid.NewGuid());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());

            _target.CreateDirectory(path);
            _target.DirectoryExists(path.Trim()).Should().BeTrue();
            _target.DeleteRecursively(path.Trim());
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path '");
            e.And.Message.Should().Contain(path);
            e.And.Message.Should().Contain("'.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
         {
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<DirectoryNotFoundException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_directory_It_should_not_throw()
         {
            // HAPPY PATH TEST:
            var path = _tempPath;
            _target.CreateDirectory(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_matches_an_existing_file_It_should_throw_IOException()
         {
            var path = _fileInternalMapping.CreateTemporaryFile();

            try
            {
               _fileInternalMapping.FileExists(path).Should().BeTrue();

               Action throwingAction = () => _target.CreateDirectory(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create");
               e.And.Message.Should().Contain("because a file or directory with the same name already exists.");
               e.And.Message.Should().Contain(path);

               throwingAction = () => _target.CreateDirectory(path);
               e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("Cannot create");
               e.And.Message.Should().Contain("because a file or directory with the same name already exists.");
               e.And.Message.Should().Contain(path);
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

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void It_should_create_the_directory()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid() + "It_should_create_the_directory");
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // relative
            _target.SetCurrentDirectory(_tempPath);
            path = @".\" + Guid.NewGuid() + "It_should_create_the_directory";
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // unc
            path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl, Guid.NewGuid() + "It_should_create_the_directory");
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_network_name_It_should_throw_IOException()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());

            Action throwingAction = () => _target.CreateDirectory(path);
            var e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);

            throwingAction = () => _target.CreateDirectory(path);
            e = throwingAction.Should().Throw<IOException>();
            e.And.Message.Should().Contain("Could not find a part of the directory path");
            e.And.Message.Should().Contain(path);
         }
      }

      [TestClass]
      public class When_I_call_DirectoryInternalMapping_DeleteEmpty : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_directory_is_empty_It_should_delete_the_directory()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteEmpty(path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0")]
         public void And_the_directory_is_not_empty_It_should_throw_IOException()
         {
            var fileMapper = new FileInternalMapping();
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var filePath = _pathUtilities.Combine(path, Guid.NewGuid() + ".tmp");

            // nested file
            _target.CreateDirectory(path);
            fileMapper.CreateFile(filePath);
            try
            {
               fileMapper.FileExists(filePath).Should().BeTrue();

               Action throwingAction = () => _target.DeleteEmpty(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The directory is not empty");
               e.And.HResult.Should().Be(unchecked((Int32)0x80070091));

               _target.DirectoryExists(path).Should().BeTrue();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // nested directory
            _target.CreateDirectory(_pathUtilities.Combine(path, Guid.NewGuid().ToString()));
            try
            {
               _target.DirectoryExists(path).Should().BeTrue();

               Action throwingAction = () => _target.DeleteEmpty(path);
               var e = throwingAction.Should().Throw<IOException>();
               e.And.Message.Should().Contain("The directory is not empty.");

               _target.DirectoryExists(path).Should().BeTrue();
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

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString()) + "|";

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_does_not_exist_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            _target.DeleteEmpty(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_leading_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteEmpty(Spaces + path);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_has_trailing_spaces_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            _target.CreateDirectory(path);
            _target.DeleteEmpty(path + Spaces);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_throw_ArgumentException()
         {
            var path = String.Empty;

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (cannot be empty or all whitespace).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_throw_ArgumentNullException()
         {
            const String path = null;

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_not_throw()
         {
            var path = @"A:\" + Guid.NewGuid();
            _target.DirectoryExists(@"A:\").Should().BeFalse();

            _target.DeleteEmpty(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
         {
            var path = _tempPath + new String('A', TestHardCodes.PathAlwaysTooLong);

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<PathTooLongException>();
            e.And.Message.Should().StartWith("The path");
            e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_white_space_It_should_throw_ArgumentException()
         {
            const String path = " \t ";

            Action throwingAction = () => _target.DeleteEmpty(path);
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

               Action throwingAction = () => _target.DeleteEmpty(filePath);
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

            Action throwingAction = () => _target.DeleteEmpty(path);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_host_It_should_not_throw()
         {
            var path = String.Format(CultureInfo.InvariantCulture, @"\\{0}\{1}", Guid.NewGuid(), Guid.NewGuid());
            _target.DeleteEmpty(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_uses_an_unknown_network_name_share_It_should_not_throw()
         {
            var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString());
            _target.DeleteEmpty(path);

            TestUtilities.TestFacilities.TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void It_should_delete_an_empty_directory()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid() + "It_should_delete_an_empty_directory");
            try
            {
               _target.CreateDirectory(path);
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
            path = @".\" + Guid.NewGuid() + "It_should_delete_an_empty_directory";
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // unc
            path = _pathUtilities.Combine(TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl, Guid.NewGuid() + "It_should_delete_an_empty_directory");
            try
            {
               _target.CreateDirectory(path);
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
   }
}
