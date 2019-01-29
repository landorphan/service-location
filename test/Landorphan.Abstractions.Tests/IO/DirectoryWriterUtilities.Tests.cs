namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DirectoryWriterUtilities_Tests
   {
      [TestClass]
      public class Given_I_have_an_IDirectoryWriterUtilities : TestBase
      {
         // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.

         private static readonly IDirectoryUtilities _directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         private static readonly IDirectoryWriterUtilities _target = IocServiceLocator.Resolve<IDirectoryWriterUtilities>();
         private static readonly String _tempPath = _environmentUtilities.GetTemporaryDirectoryPath();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_IDirectoryWriterUtilities_Copy_It_should_copy_the_directory()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _directoryUtilities.CreateDirectory(sourceDirName);
               var filePath = _pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString());
               _fileUtilities.CreateFile(filePath);

               var childDirPath = _directoryUtilities.CreateDirectory(_pathUtilities.Combine(sourceDirName, Guid.NewGuid().ToString()));
               filePath = _pathUtilities.Combine(childDirPath, Guid.NewGuid().ToString());
               _fileUtilities.CreateFile(filePath);

               _target.Copy(sourceDirName, destDirName);

               _directoryUtilities.DirectoryExists(sourceDirName).Should().BeTrue();
               _directoryUtilities.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(sourceDirName);
               _directoryUtilities.DeleteRecursively(destDirName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_IDirectoryWriterUtilities_Move_It_should_move_the_directory()
         {
            var sourceDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());
            var destDirName = _pathUtilities.Combine(_tempPath, Guid.NewGuid().ToString());

            try
            {
               _directoryUtilities.CreateDirectory(sourceDirName);

               _target.Move(sourceDirName, destDirName);

               _directoryUtilities.DirectoryExists(sourceDirName).Should().BeFalse();
               _directoryUtilities.DirectoryExists(destDirName).Should().BeTrue();
            }
            finally
            {
               _directoryUtilities.DeleteRecursively(sourceDirName);
               _directoryUtilities.DeleteRecursively(destDirName);
            }
         }
      }

      [TestClass]
      public class When_I_service_locate_IDirectoryWriterUtilities : ArrangeActAssert
      {
         private IDirectoryWriterUtilities actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IDirectoryWriterUtilities>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_DirectoryReaderUtilitiesFactory()
         {
            actual.Should().BeOfType<DirectoryWriterUtilities>();
         }
      }
   }
}
