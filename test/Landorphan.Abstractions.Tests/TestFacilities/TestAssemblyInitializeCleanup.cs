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
            // TestAssemblyInitializeCleanupWindowsHelper.Arrange() is now called upon demand in TestHardCodes.WindowsTestPaths
         }
         else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
         {
            // TODO: Arrange local paths
         }
         // TODO: figure out how to share "secrets" so build server can use
         // TODO: any other UNC paths needed besides TestHardCodes.UncTestPaths.AzureUncFolderEveryoneFullControl,
         // if so, arrange them here

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
            // if the windows test target root directory exists, tear it down (requires elevation) by calling PS script.
            var envUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
            var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
            var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
            var systemDrive = pathUtilities.GetRootPath(envUtilities.GetSpecialFolderPath(Environment.SpecialFolder.System));
            var localFolderRootTest = pathUtilities.Combine(systemDrive, @"Landorphan.Abstractions.Test.UnitTestTarget");

            if (dirUtilities.DirectoryExists(localFolderRootTest))
            {
               var windowsArrange = new TestAssemblyInitializeCleanupWindowsHelper();
               try
               {
                  windowsArrange.Teardown();
               }
               catch (Exception e)
               {
                  Trace.WriteLine("ERROR", e.ToString());
               }
            }
         }
      }
   }
}
