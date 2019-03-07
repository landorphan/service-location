namespace Landorphan.TestUtilities.TestFilters
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Signifies that a test will not be executed (Inconclusive will be returned)
   /// if the platform where the test is executed is OSX.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
   [SuppressMessage("SonarLint.CodeSmell", "S101: Name does not match camel case rules",
      Justification = "This name is consistent with the name chosen by dotnet core for OSPlatform")]
   public sealed class DoNotRunTestOnOSXAttribute : TestFilterAttribute
   {
      /// <inheritdoc/>
      public override Boolean ReturnInconclusiveTestResult()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      }
   }
}
