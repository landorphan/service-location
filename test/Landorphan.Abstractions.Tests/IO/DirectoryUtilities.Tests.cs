namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DirectoryUtilities_Tests
   {
      [TestClass]
      public class Given_I_have_a_DirectoryUtilities : TestBase
      {
         // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.

         private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         private static readonly IDirectoryUtilities _target = IocServiceLocator.Resolve<IDirectoryUtilities>();
         private static readonly String _tempPath = _environmentUtilities.GetTemporaryDirectoryPath();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void When_I_call_DirectoryUtilities_CreateDirectory_It_should_create_the_directory()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_CreateDirectory_It_should_create_the_directory));
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
            path = @".\" + Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_CreateDirectory_It_should_create_the_directory);
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
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_CreateDirectory_It_should_create_the_directory));
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

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void When_I_call_DirectoryUtilities_DeleteEmpty_It_should_delete_an_empty_directory()
         {
            // absolute
            var path = _pathUtilities.Combine(_pathUtilities.GetFullPath(_tempPath), Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteEmpty_It_should_delete_an_empty_directory));
            try
            {
               _target.CreateDirectory(path);
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteEmpty(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }

            // relative
            _target.SetCurrentDirectory(_tempPath);
            path = @".\" + Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteEmpty_It_should_delete_an_empty_directory);
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
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteEmpty_It_should_delete_an_empty_directory));
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

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories()
         {
            // absolute
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories));
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories) + ".tmp"));
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
            path = @".\" + Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories);
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories) + ".tmp"));
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
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories));
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(path, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DeleteRecursively_It_should_delete_a_directory_with_files_and_subdirectories) + ".tmp"));
               _target.DirectoryExists(path).Should().BeTrue();
               _target.DeleteRecursively(path);
               _target.DirectoryExists(path).Should().BeFalse();
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("failing in .Net Standard 2.0, Need a known UNC file share")]
         public void When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories()
         {
            // absolute -- extant
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories));
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
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories));
            _target.DirectoryExists(path).Should().BeFalse();

            // relative -- extant
            _target.SetCurrentDirectory(_tempPath);
            path = @".\" + Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories);
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
            path = @".\" + Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories);
            _target.DirectoryExists(path).Should().BeFalse();

            // unc -- extant
            path = _pathUtilities.Combine(
               TestHardCodes.WindowsTestPaths.TodoRethinkNetworkShareEveryoneFullControl,
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories));
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
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_DirectoryExists_It_should_distinguish_between_extant_and_non_extant_directories));
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetCreationTime_It_should_get_the_creation_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_GetCreationTime_It_should_get_the_creation_time));
            _target.CreateDirectory(path);
            try
            {
               var expected = DateTimeOffset.UtcNow;
               _target.SetCreationTime(path, expected);
               _target.GetCreationTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetCurrentDirectory_It_should_get_the_current_directory()
         {
            var path = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _target.SetCurrentDirectory(path);
            _target.GetCurrentDirectory().Should().Be(path);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetLastAccessTime_It_should_get_the_last_access_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_GetLastAccessTime_It_should_get_the_last_access_time));
            _target.CreateDirectory(path);
            try
            {
               var expected = DateTimeOffset.UtcNow;
               _target.SetLastAccessTime(path, expected);
               _target.GetLastAccessTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetLastWriteTime_It_should_get_the_last_write_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_GetLastWriteTime_It_should_get_the_last_write_time));
            _target.CreateDirectory(path);
            try
            {
               var expected = DateTimeOffset.UtcNow;
               _target.SetLastWriteTime(path, expected);
               _target.GetLastWriteTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetRandomDirectoryName_It_should_get_a_random_directory_name_that_is_not_rooted_and_does_not_exist
            ()
         {
            var actual = _target.GetRandomDirectoryName();
            _target.DirectoryExists(actual).Should().BeFalse();
            _pathUtilities.IsPathRooted(actual).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_GetTemporaryDirectoryPath_It_should_get_the_temporary_directory()
         {
            var expected =
               IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_environmentUtilities.ExpandEnvironmentVariables("%temp%"));
            var actual = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_target.GetTemporaryDirectoryPath());
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_SetCreationTime_It_should_set_the_creation_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + " " + nameof(When_I_call_DirectoryUtilities_SetCreationTime_It_should_set_the_creation_time));
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

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_SetCurrentDirectory_It_should_set_the_current_directory()
         {
            var was = _target.GetCurrentDirectory();
            var expected = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_tempPath);
            _target.SetCurrentDirectory(expected);
            var actual = _target.GetCurrentDirectory();

            actual.Should().NotBe(was);
            actual.Should().Be(expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_SetLastAccessTime_It_should_set_the_last_access_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_SetLastAccessTime_It_should_set_the_last_access_time));
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

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DirectoryUtilities_SetLastWriteTime_It_should_set_the_last_write_time()
         {
            var path = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + nameof(When_I_call_DirectoryUtilities_SetLastWriteTime_It_should_set_the_last_write_time));
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

      [TestClass]
      public class When_I_service_locate_IDirectoryUtilities : ArrangeActAssert
      {
         private IDirectoryUtilities actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IDirectoryUtilities>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_DirectoryUtilities()
         {
            actual.Should().BeOfType<DirectoryUtilities>();
         }
      }
   }
}
