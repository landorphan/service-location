namespace Landorphan.TestUtilities.MSTest.Tests.Architecture
{
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using Landorphan.TestUtilities.MSTest.Tests.TestFacilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
   [TestClass]
   public class Architecture_Tests : ArchitecturalRequirements
   {
      [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested()
      {
         All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested_Implementation();
      }

      [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_Declarations_Under_An_Internal_Namespace_Are_Not_Public()
      {
         All_Declarations_Under_An_Internal_Namespace_Are_Not_Public_Implementation();
      }

      protected override IImmutableSet<Assembly> GetAssembliesUnderTest()
      {
         return TestAssemblyInitializeCleanup.AssembliesUnderTest;
      }
   }
}
