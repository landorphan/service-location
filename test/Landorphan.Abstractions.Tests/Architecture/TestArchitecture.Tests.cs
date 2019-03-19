namespace Landorphan.Abstractions.Tests.Architecture
{
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class TestArchitecture_Tests : TestArchitecturalRequirements
   {
      [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_But_Excluded_Tests_Descend_From_TestBase()
      {
         All_But_Excluded_Tests_Descend_From_TestBase_Or_Where_Generated_By_SpecFlow_Implementation();
      }

      [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category()
      {
         All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category_Implementation();
      }

      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void Check_TestHardCodes_WindowsPaths()
      {
         // strange to test test code, but a lot of arranging is going on behind the scenes.
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            TestHardCodes.WindowsLocalTestPaths.LocalFolderRoot.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFileFullControlFolderOwnerOnlyFile.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFileOuterFolderNoPermissionsChildFile.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFolderEveryoneFullControl.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissions.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.LocalFolderReadExecuteListFolderContents.Should().NotBeNull();

            TestHardCodes.WindowsLocalTestPaths.MappedDrive.Should().NotBeNull();
            TestHardCodes.WindowsLocalTestPaths.UnmappedDrive.Should().NotBeNull();

            TestHardCodes.WindowsUncTestPaths.UncShareRoot.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFileFullControlFolderOwnerOnlyFile.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFileOuterFolderNoPermissionsChildFile.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFolderEveryoneFullControl.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFolderOuterFolderNoPermissions.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFolderOuterFolderNoPermissionsInnerFolderNoPermissions.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder.Should().NotBeNull();
            TestHardCodes.WindowsUncTestPaths.UncFolderReadExecuteListFolderContents.Should().NotBeNull();
         }
      }

      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void Find_ignored_tests()
      {
         var asm = GetType().Assembly;
         Find_ignored_tests_in_assembly(asm);

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      protected override Assembly GetTestAssembly()
      {
         return GetType().Assembly;
      }
   }
}
