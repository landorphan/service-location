namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System.Reflection;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.Tests.Attributes;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class AbstractionTestAssemblySetup 
   {
      [AssemblyInitialize]
      public static void AssemblyInitialize(TestContext context)
      {
         TestBase.OnTestInitialize = OnTestInitialize;
      }

      public static void OnTestInitialize(TestBase instance)
      {
         MethodInfo methodInfo = instance.GetType().GetMethod(instance.TestContext.TestName);
         if (methodInfo != null)
         {
            var windowsTestOnly = methodInfo.GetCustomAttribute<WindowsTestOnlyAttribute>();
            if (windowsTestOnly != null &&
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               Assert.Inconclusive("This test is designed to run on Windows only.  It is inconclusive on other OS variants.");
            }
         }
      }
   }
}
