namespace Landorphan.Abstractions.Tests.Architecture
{
   using Landorphan.TestUtilities;

   // ReSharper disable InconsistentNaming

   public static class ExecutionEnvironment_Tests
   {
      //[TestClass]
      public class When_executing_tests_in_windows : TestBase
      {
         //[TestMethod]
         //[TestCategory(WellKnownTestCategories.PreventRegressionBug)]
         //[TestCategory(TestTiming.CheckIn)]
         //// Disabling for now due to some environmental problems: tgs
         //public void The_32_bit_PowerShell_environment_should_support_scripting()
         //{
         //   if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         //   {
         //      // the test run executes local unsigned scripts in 32-bit powershell 5.1

         //      // see https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-5.1
         //      var allowedExecutionPolicies = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { "RemoteSigned", "Unrestricted", "Bypass" };
         //      var disallowedExecutionPolicies = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { "Restricted", "AllSigned", "Undefined" };
         //      var psExecutionPolicy = TestAssemblyInitializeCleanupWindowsHelper.GetPSExecutionPolicyUser();
         //      if (allowedExecutionPolicies.Contains(psExecutionPolicy))
         //      {
         //         Trace.WriteLine($"Current PowerShell 32-bit environment execution policy is {psExecutionPolicy}.");
         //         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         //      }
         //      else if (disallowedExecutionPolicies.Contains(psExecutionPolicy))
         //      {
         //         var assemblyName = GetType().Assembly.GetName();
         //         var allowed = String.Join(',', allowedExecutionPolicies);
         //         var msg = $"Current PowerShell 32-bit environment execution policy is {psExecutionPolicy}.  The tests in this assembly {assemblyName}," +
         //                   $"require one of the following PowerShell execution policies on Windows {allowed}.";
         //         Assert.Fail(msg);
         //      }
         //      else
         //      {
         //         var msg = $"The PowerShell 32-bit execution policy of {psExecutionPolicy} is unknown. {Environment.NewLine}" +
         //                   "See: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-5.1";
         //         Assert.Fail(msg);
         //      }
         //   }
         //   else
         //   {
         //      // This test does not apply in non-Windows environments
         //      TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         //   }
         //}
      }
   }
}
