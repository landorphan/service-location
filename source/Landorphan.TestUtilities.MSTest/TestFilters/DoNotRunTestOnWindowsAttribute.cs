namespace Landorphan.TestUtilities.TestFilters
{
   using System;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Signifies that a test will not be executed (Inconclusive will be returned)
   /// if the platform where the test is executed is Windows.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   public sealed class DoNotRunTestOnWindowsAttribute : TestFilterAttribute
   {
      /// <inheritdoc/>
      public override Boolean ReturnInconclusiveTestResult()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      }
   }
}
