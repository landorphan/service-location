namespace Landorphan.TestUtilities.Tests.Architecture
{
   using System.Reflection;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class TestArchitectureTests : TestArchitecturalRequirements
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_But_Excluded_Tests_Descend_From_TestBase()
      {
         All_But_Excluded_Tests_Descend_From_TestBase_Implementation();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category()
      {
         All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category_Implementation();
      }

      protected override Assembly GetTestAssembly()
      {
         return GetType().Assembly;
      }
   }
}
