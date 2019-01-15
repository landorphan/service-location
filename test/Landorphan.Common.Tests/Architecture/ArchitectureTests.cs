namespace Landorphan.Common.Tests.Architecture
{
   using System.Collections.Immutable;
   using System.Reflection;
   using Landorphan.Common.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class ArchitectureTests : ArchitecturalRequirements
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested()
      {
         All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested_Implementation();
      }

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
