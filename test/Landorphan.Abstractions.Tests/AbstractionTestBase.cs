namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System.Reflection;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.Tests.Attributes;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   public class AbstractionTestBase : TestBase
   {
      protected override void InitializeTestMethod()
      {
         base.InitializeTestMethod();
            
         MethodInfo methodInfo = this.GetType().GetMethod(this.TestContext.TestName);
         if (methodInfo != null)
         {
            var windowsTestOnly = methodInfo.GetCustomAttribute<WindowsTestOnlyAttribute>();
            if (windowsTestOnly != null)
            {
               if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
               {
                  Assert.Inconclusive("This test is designed to run on Windows only.  It is inconclusive on other OS variants.");
               }
            }
         }
      }
   }
}
