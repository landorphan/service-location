namespace Landorphan.TestUtilities.TestFilters
{
   using System;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Signifies that a test will not be executed (Inconclusive will be returned)
   /// if the platform where the test is executed is OSX.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
   public sealed class DoNotRunTestOnOSXAttribute : TestFilterAttribute
   {
      /// <inheritdoc/>
      public override Boolean ReturnInconclusiveTestResult()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      }
   }
}
