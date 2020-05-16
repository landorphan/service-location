namespace Landorphan.Abstractions.Tests.IO
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using FluentAssertions;
    using Landorphan.Abstractions.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static class DirectoryReaderUtilities_Tests
   {
       // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.
       private const string Spaces = "   ";
       private static readonly IDirectoryUtilities _directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
       private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
       private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
       private static readonly IDirectoryReaderUtilities _target = IocServiceLocator.Resolve<IDirectoryReaderUtilities>();
       private static readonly string _tempPath = _directoryUtilities.GetTemporaryDirectoryPath();

       [TestClass]
      public class When_I_call_DirectoryReaderUtilities_EnumerateDirectories : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_subdirectories()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateDirectories"));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateDirectories")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateDirectories")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateDirectories"))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _directoryUtilities.CreateDirectory(sd);
               }

               _target.EnumerateDirectories(outerFullPath).Should().Contain(expected);
               _target.EnumerateDirectories(Spaces + outerFullPath, "*").Should().Contain(expected);
               _target.EnumerateDirectories(Spaces + outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_EnumerateFiles : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_files()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateFiles"));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateFiles" + ".txt")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateFiles" + ".txt")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateFiles" + ".txt"))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var filePath in expected)
               {
                  _fileUtilities.CreateFile(filePath);
               }

               _target.EnumerateFiles(outerFullPath + Spaces).Should().Contain(expected);
               _target.EnumerateFiles(outerFullPath + Spaces, "*").Should().Contain(expected);
               _target.EnumerateFiles(outerFullPath + Spaces, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_EnumerateFileSystemEntries : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_FileSystemEntries()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "When_I_call_EnumerateFileSystemEntries"));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     "When_I_call_EnumerateFileSystemEntries")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     "When_I_call_EnumerateFileSystemEntries")),
               _pathUtilities.GetFullPath(
                  _pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) +
                     "When_I_call_EnumerateFileSystemEntries"))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _directoryUtilities.CreateDirectory(sd);
               }

               _target.EnumerateFileSystemEntries(outerFullPath).Should().Contain(expected);
               _target.EnumerateFileSystemEntries(outerFullPath, "*").Should().Contain(expected);
               _target.EnumerateFileSystemEntries(outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_GetDirectories : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_non_null_collection()
         {
            var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _directoryUtilities.CreateDirectory(sd);
               }

               _target.GetDirectories(outerFullPath).Should().Contain(expected);
               _target.GetDirectories(Spaces + outerFullPath, "*").Should().Contain(expected);
               _target.GetDirectories(Spaces + outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_GetFiles : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_non_null_collection()
         {
            var outerFullPath = _pathUtilities.GetFullPath(
               _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection)));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection) + ".txt")),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection) + ".txt")),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection) + ".txt"))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var filePath in expected)
               {
                  _fileUtilities.CreateFile(filePath);
               }

               _target.GetFiles(outerFullPath + Spaces).Should().Contain(expected);
               _target.GetFiles(outerFullPath + Spaces, "*").Should().Contain(expected);
               _target.GetFiles(outerFullPath + Spaces, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_GetFileSystemEntries : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_non_null_collection()
         {
            var outerFullPath =
               _pathUtilities.GetFullPath(_pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection)));
            var expected = new List<string>
            {
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection))),
               _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_a_non_null_collection)))
            };

            try
            {
               _directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  _directoryUtilities.CreateDirectory(sd);
               }

               _target.GetFileSystemEntries(outerFullPath).Should().Contain(expected);
               _target.GetFileSystemEntries(outerFullPath, "*").Should().Contain(expected);
               _target.GetFileSystemEntries(outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(outerFullPath);
            }
         }
      }

      [TestClass]
      public class When_I_service_locate_IDirectoryReaderUtilities : ArrangeActAssert
      {
          private IDirectoryReaderUtilities actual;

          protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IDirectoryReaderUtilities>();
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_DirectoryReaderUtilities()
         {
            actual.Should().BeOfType<DirectoryReaderUtilities>();
         }
      }
   }
}
