namespace Landorphan.TestUtilities.TestFilters
{
   using System;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Signifies that a test will be executed (Inconclusive will not be be returned)
   /// only if the platform where the test is executed is Windows.
   /// Although: Other TestFilterAttributes may be applied to the class that alters this behavior.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   public sealed class RunTestOnlyOnWindowsAttribute : TestFilterAttribute
   {
      /// <inheritdoc/>
      public override Boolean ReturnInconclusiveTestResult()
      {
         return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      }
   }
}
