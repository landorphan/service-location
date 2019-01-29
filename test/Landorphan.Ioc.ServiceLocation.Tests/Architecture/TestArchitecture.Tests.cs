namespace Landorphan.Ioc.Tests.Architecture
{
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations.Architecture;
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
         All_But_Excluded_Tests_Descend_From_TestBase_Implementation();
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
