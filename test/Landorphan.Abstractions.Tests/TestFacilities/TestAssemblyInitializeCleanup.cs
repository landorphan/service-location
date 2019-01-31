namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public static class TestAssemblyInitializeCleanup
   {
      internal static IImmutableSet<Assembly> AssembliesUnderTest { get; private set; }

      /// <summary>
      /// Performs assembly level initialization.
      /// </summary>
      /// <remarks>
      /// Executes once, before any tests to be executed are run.
      /// </remarks>
      [AssemblyInitialize]
      [SuppressMessage("SonarLint.CodeSmell", "S3923: All branches in a conditional structure should not have exactly the same implementation")]
      public static void AssemblyInitialize(TestContext context)
      {
         // acquire assemblies under test
         var assemblies = ImmutableHashSet<Assembly>.Empty.ToBuilder();
         assemblies.Add(typeof(DirectoryUtilities).Assembly);
         AssembliesUnderTest = assemblies.ToImmutable();

         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            // Design decision:  create a rooted directory (which has issues)
            // rather than something in the temp folder bc some tests navigate out and attempt to manipulate files and folders
            // something I do not want to happen to user files (e.g. C:\users\user\temp..\..)

            var envUtils = IocServiceLocator.Resolve<IEnvironmentUtilities>();
            var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
            var dirUtils = IocServiceLocator.Resolve<IDirectoryUtilities>();
            var fileUtils = IocServiceLocator.Resolve<IFileUtilities>();

            // TODO: set permissions

            /* *****************************************************************************************************
            Create the following folder\file structure:
                  C:\Landorphan.Abstractions.Test.UnitTestTarget
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\Outer
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\SharedEveryoneFullControl
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\Outer\Inner
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\Outer\OuterExistingFile.txt
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\Outer\ReadExecuteListFolderContents
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\Outer\Inner\InnerExistingFile.txt
                  C:\Landorphan.Abstractions.Test.UnitTestTarget\SharedEveryoneFullControl\DeniedToAllButOwner.txt
            ***************************************************************************************************** */

            // usually C:
            var rootSystem = pathUtils.GetRootPath(envUtils.GetSpecialFolderPath(Environment.SpecialFolder.System));
            var rootTestFolder = dirUtils.CreateDirectory(pathUtils.Combine(rootSystem, "Landorphan.Abstractions.Test.UnitTestTarget"));
            TestHardCodes.WindowsTestPaths.SetFolderPathLocalTestTargetRootFolder(rootTestFolder);
            var sharedEveryoneFolder = dirUtils.CreateDirectory(pathUtils.Combine(rootTestFolder, "SharedEveryoneFullControl"));
            TestHardCodes.WindowsTestPaths.SetFolderPathLocalSharedFolderEveryoneFullControl(sharedEveryoneFolder);
            var sharedEveryoneFolderDeniedToAllButOwnerFile = fileUtils.CreateFile(pathUtils.Combine(sharedEveryoneFolder, "DeniedToAllButOwner.txt"));
            TestHardCodes.WindowsTestPaths.SetFilePathLocalSharedEveryoneFolderDeniedToAllButOwnerFile(sharedEveryoneFolderDeniedToAllButOwnerFile);
            var outerFolder = dirUtils.CreateDirectory(pathUtils.Combine(rootTestFolder, "Outer"));
            TestHardCodes.WindowsTestPaths.SetFolderPathLocalOuterFolderWithoutPermissions(outerFolder);
            var outerFile = fileUtils.CreateFile(pathUtils.Combine(outerFolder, "OuterExistingFile.txt"));
            TestHardCodes.WindowsTestPaths.SetFilePathLocalOuterFolderWithoutPermissionsChildFile(outerFile);
            var outerInnerFolder = dirUtils.CreateDirectory(pathUtils.Combine(outerFolder, "Inner"));
            TestHardCodes.WindowsTestPaths.SetFolderPathLocalOuterFolderWithoutPermissionsChildFolder(outerInnerFolder);
            var outerInnerFile = fileUtils.CreateFile(pathUtils.Combine(outerInnerFolder, "InnerExistingFile.txt"));
            TestHardCodes.WindowsTestPaths.SetFilePathLocalOuterFolderWithoutPermissionsChildFolderChildFile(outerInnerFile);
            var readExecuteFolder = dirUtils.CreateDirectory(pathUtils.Combine(outerFolder, "ReadExecuteListFolderContents"));
            TestHardCodes.WindowsTestPaths.SetFolderPathLocalReadExecuteListFolderContentsFolder(readExecuteFolder);

            // TODO: Arrange unc paths
            // TODO: do we currently have a server that can host a test share?
         }
         else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
         {
            // TODO: Arrange local paths
            // TODO: Arrange unc paths
            // TODO: do we currently have a server that can host a test share?
         }

         // Configure the simulator as needed.
      }

      /// <summary>
      /// Frees resources obtained by the test assembly.
      /// </summary>
      /// <remarks>
      /// Executes once, after all tests to be executed are run.
      /// </remarks>
      [AssemblyCleanup]
      public static void AssemblyCleanup()
      {
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            var dirUtils = IocServiceLocator.Resolve<IDirectoryUtilities>();
            try
            {
               dirUtils.DeleteRecursively(TestHardCodes.WindowsTestPaths.LocalTestTargetRootFolder);
            }
            catch (Exception e)
            {
               Trace.WriteLine(e);
            }
         }
      }
   }
}
