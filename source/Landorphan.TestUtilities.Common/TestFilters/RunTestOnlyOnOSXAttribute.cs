namespace Landorphan.TestUtilities.TestFilters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
   /// Signifies that a test will be executed (Inconclusive will not be be returned)
   /// only if the platform where the test is executed is OSX.
   /// Although: Other TestFilterAttributes may be applied to the class that alters this behavior.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   [SuppressMessage(
      "SonarLint.CodeSmell",
      "S101: Name does not match camel case rules",
      Justification = "This name is consistent with the name chosen by dotnet core for OSPlatform")]
   public sealed class RunTestOnlyOnOSXAttribute : TestFilterAttribute
   {
       /// <inheritdoc/>
      public override bool ReturnInconclusiveTestResult()
      {
         return !RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      }
   }
}
