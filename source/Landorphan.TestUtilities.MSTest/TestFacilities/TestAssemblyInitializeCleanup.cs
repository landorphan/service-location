namespace Landorphan.TestUtilities.TestFacilities
{
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Defines test assembly initialization and cleanup.
   /// </summary>
   [TestClass]
   public static class TestAssemblyInitializeCleanup
   {
      /// <summary>
      /// Performs assembly level initialization.
      /// </summary>
      /// <remarks>
      /// Executes once, before any tests to be executed are run.
      /// </remarks>
      [AssemblyInitialize]
      public static void AssemblyInitialize(TestContext context)
      {
         // quieting intermittent static analysis warning.
         TestHelp.DoNothing(context);

         // currently no resources to initialize.
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
         // currently no resources to clean up.
      }
   }
}
