namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DirectoryReaderUtilities_Tests
   {
      // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.
      private const String Spaces = "   ";

      [TestClass]
      public class When_I_call_DirectoryReaderUtilities_EnumerateDirectories : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_known_subdirectories()
         {
            var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
            var environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
            var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
            var target = IocServiceLocator.Resolve<IDirectoryReaderUtilities>();
            var tempPath = environmentUtilities.GetTemporaryDirectoryPath();

            var outerFullPath = pathUtilities.GetFullPath(pathUtilities.Combine(tempPath, Guid.NewGuid() + "When_I_call_EnumerateDirectories"));
            var expected = new List<String>
            {
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateDirectories")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateDirectories")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateDirectories"))
            };

            try
            {
               directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  directoryUtilities.CreateDirectory(sd);
               }

               target.EnumerateDirectories(outerFullPath).Should().Contain(expected);
               target.EnumerateDirectories(Spaces + outerFullPath, "*").Should().Contain(expected);
               target.EnumerateDirectories(Spaces + outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               directoryUtilities.DeleteRecursively(outerFullPath);
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
            var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
            var environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
            var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
            var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
            var target = IocServiceLocator.Resolve<IDirectoryReaderUtilities>();
            var tempPath = environmentUtilities.GetTemporaryDirectoryPath();

            var outerFullPath = pathUtilities.GetFullPath(pathUtilities.Combine(tempPath, Guid.NewGuid() + "When_I_call_EnumerateFiles"));
            var expected = new List<String>
            {
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateFiles" + ".txt")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateFiles" + ".txt")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() + "When_I_call_EnumerateFiles" + ".txt"))
            };

            try
            {
               directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var filePath in expected)
               {
                  fileUtilities.CreateFile(filePath);
               }

               target.EnumerateFiles(outerFullPath + Spaces).Should().Contain(expected);
               target.EnumerateFiles(outerFullPath + Spaces, "*").Should().Contain(expected);
               target.EnumerateFiles(outerFullPath + Spaces, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               directoryUtilities.DeleteRecursively(outerFullPath);
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
            var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
            var environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
            var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
            var target = IocServiceLocator.Resolve<IDirectoryReaderUtilities>();
            var tempPath = environmentUtilities.GetTemporaryDirectoryPath();

            var outerFullPath = pathUtilities.GetFullPath(pathUtilities.Combine(tempPath, Guid.NewGuid() + "When_I_call_EnumerateFileSystemEntries"));
            var expected = new List<String>
            {
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() +
                     "When_I_call_EnumerateFileSystemEntries")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() +
                     "When_I_call_EnumerateFileSystemEntries")),
               pathUtilities.GetFullPath(
                  pathUtilities.Combine(
                     outerFullPath,
                     Guid.NewGuid() +
                     "When_I_call_EnumerateFileSystemEntries"))
            };

            try
            {
               directoryUtilities.CreateDirectory(outerFullPath);
               foreach (var sd in expected)
               {
                  directoryUtilities.CreateDirectory(sd);
               }

               target.EnumerateFileSystemEntries(outerFullPath).Should().Contain(expected);
               target.EnumerateFileSystemEntries(outerFullPath, "*").Should().Contain(expected);
               target.EnumerateFileSystemEntries(outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
            }
            finally
            {
               directoryUtilities.DeleteRecursively(outerFullPath);
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
