namespace Landorphan.TestUtilities.TestFilters
{
   using System;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Signifies that a test will be executed (Inconclusive will not be be returned)
   /// only if the platform where the test is executed is Linux.
   /// Although: Other TestFilterAttributes may be applied to the class that alters this behavior.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
   public sealed class RunTestOnlyOnLinuxAttribute : TestFilterAttribute
   {
      /// <inheritdoc/>
      public override Boolean ReturnInconclusiveTestResult()
      {
         return !RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      }
   }
}
