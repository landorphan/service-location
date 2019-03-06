namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using System.Globalization;
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.IO.Internal.Directory;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DirectoryUtilities_Tests
   {
      // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.

      private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
      private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
      private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
      private static readonly IDirectoryUtilities _target = IocServiceLocator.Resolve<IDirectoryUtilities>();
      private static readonly String _tempPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(_target.GetTemporaryDirectoryPath());

      [TestClass]
      public class When_I_call_DirectoryUtilities_CreateDirectory : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_the_directory_absolute()
         {
            // absolute
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_absolute));
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
         public void It_should_create_the_directory_relative()
         {
            // relative
            _target.SetCurrentDirectory(_target.GetTemporaryDirectoryPath());
            var path = _pathUtilities.Combine(_pathUtilities.Combine(
               _pathUtilities.DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_relative)));
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
         public void It_should_create_the_directory_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_create_the_directory_unc));
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
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_DeleteEmpty : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_an_empty_directory_absolute()
         {
            // absolute
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_absolute));
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
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_an_empty_directory_relative()
         {
            // relative
            _target.SetCurrentDirectory(_tempPath);
            var path = @".\" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_relative);
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
         public void It_should_delete_an_empty_directory_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncShareRoot,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_an_empty_directory_unc));
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

      [TestClass]
      public class When_I_call_DirectoryUtilities_DeleteRecursively : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_delete_a_directory_with_files_and_subdirectories_absolute()
         {
            // absolute
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
               nameof(It_should_delete_a_directory_with_files_and_subdirectories_absolute));
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_absolute)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_absolute) +
                     ".tmp"));
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
         public void It_should_delete_a_directory_with_files_and_subdirectories_relative()
         {
            // relative
            _target.SetCurrentDirectory(_tempPath);
            var path = @".\" +
                       Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                       nameof(It_should_delete_a_directory_with_files_and_subdirectories_relative);
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_relative)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_relative) +
                     ".tmp"));
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
         public void It_should_delete_a_directory_with_files_and_subdirectories_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_delete_a_directory_with_files_and_subdirectories_unc));
            try
            {
               _target.CreateDirectory(path);
               _target.CreateDirectory(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_unc)));
               _fileUtilities.CreateFile(
                  _pathUtilities.Combine(
                     path,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     nameof(It_should_delete_a_directory_with_files_and_subdirectories_unc) +
                     ".tmp"));
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

      [TestClass]
      public class When_I_call_DirectoryUtilities_DirectoryExists : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_distinguish_between_extant_and_non_extant_directories_absolute()
         {
            // absolute -- extant
            var path = _pathUtilities.Combine(
               _pathUtilities.GetFullPath(_tempPath),
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
               nameof(It_should_distinguish_between_extant_and_non_extant_directories_absolute));
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
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
               nameof(It_should_distinguish_between_extant_and_non_extant_directories_absolute));
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_distinguish_between_extant_and_non_extant_directories_relative()
         {
            // relative -- extant
            _target.SetCurrentDirectory(_tempPath);
            var path = @".\" +
                       Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                       nameof(It_should_distinguish_between_extant_and_non_extant_directories_relative);
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
            path = @".\" +
                   Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                   nameof(It_should_distinguish_between_extant_and_non_extant_directories_relative);
            _target.DirectoryExists(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_distinguish_between_extant_and_non_extant_directories_unc()
         {
            if (TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl)}");
               return;
            }

            // unc -- extant
            var path = _pathUtilities.Combine(
               TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_distinguish_between_extant_and_non_extant_directories_unc));
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
               TestHardCodes.WindowsUncTestPaths.UncShareRoot,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_distinguish_between_extant_and_non_extant_directories_unc));
            _target.DirectoryExists(path).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetCreationTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_creation_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_get_the_creation_time));
            _target.CreateDirectory(path);
            try
            {
               DateTimeOffset expected;
               if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
               {
                  // On Windows, Date Time Offset precision matches file time precision
                  expected = DateTimeOffset.UtcNow;
               }
               else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
               {
                  // On Linux, Date Time OffSet precision is to 100ns, but file time precision is to 1s.
                  var utcTicks = DateTimeOffset.UtcNow.Ticks;
                  // trim off all values below 1 second.
                  var ticks = (utcTicks - (utcTicks % TimeSpan.TicksPerSecond));
                  expected = new DateTimeOffset(new DateTime(ticks,DateTimeKind.Utc));
               }
               else
               {
                  // OSX
                  throw new InvalidOperationException("OSX Test needs some OSX love, it does not know what to do.");
                  expected = DateTimeOffset.UtcNow;
               }

               _target.SetCreationTime(path, expected);
               _target.GetCreationTime(path).Should().Be(expected);
            }
            finally
            {
               _target.DeleteRecursively(path);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetCurrentDirectory : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_current_directory()
         {
            _target.SetCurrentDirectory(_tempPath);
            _target.GetCurrentDirectory().Should().Be(_tempPath);
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetLastAccessTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_last_access_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_get_the_last_access_time));
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
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetLastWriteTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_the_last_write_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_get_the_last_write_time));
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
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetRandomDirectoryName : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_a_random_directory_name_that_is_not_rooted_and_does_not_exist
            ()
         {
            var actual = _target.GetRandomDirectoryName();
            _target.DirectoryExists(actual).Should().BeFalse();
            _pathUtilities.IsPathRelative(actual).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_GetTemporaryDirectoryPath : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [Ignore("Fails on build server:  expected is a 8.3 dir name, actual is full dir name")]
         public void It_should_get_the_temporary_directory()
         {
            var expected = _environmentUtilities.ExpandEnvironmentVariables("%temp%");
            var actual = _target.GetTemporaryDirectoryPath();
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_SetCreationTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_creation_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + " " + nameof(It_should_set_the_creation_time));
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
      public class When_I_call_DirectoryUtilities_SetCurrentDirectory : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_current_directory()
         {
            var was = _target.GetCurrentDirectory();
            var expected = _tempPath;
            _target.SetCurrentDirectory(expected);
            var actual = _target.GetCurrentDirectory();

            actual.Should().NotBe(was);
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_call_DirectoryUtilities_SetLastAccessTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_access_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_set_the_last_access_time));
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
      public class When_I_call_DirectoryUtilities_SetLastWriteTime : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_last_write_time()
         {
            var path = _pathUtilities.Combine(
               _tempPath,
               Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_set_the_last_write_time));
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
