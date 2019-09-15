namespace Landorphan.InstrumentationManagement.Tests.MockApplications
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using Landorphan.InstrumentationManagement.Tests.Aspects;

   [LogMethod]
   public class MockWebApp
   {
      public static Dictionary<string, MethodInfo> WebMethods = GetWebMethods();

      private static Dictionary<string, MethodInfo> GetWebMethods()
      {
         var retval = new Dictionary<string, MethodInfo>();
         var methods = (from m in typeof(MockWebApp).GetMethods()
                       where m.Name.Contains("WebMethod")
                      select m).ToList();
         foreach (var method in methods)
         {
            retval.Add(method.Name, method);
         }

         return retval;
      }

      public string PostWebMethod1 (string value)
      {
         return value;
      }

      public string PostWebMethod2 (string value)
      {
         return value;
      }

      public string GetWebMethod1 (string value)
      {
         return value;
      }

      public MockWebApp()
      {

      }

      public void GlobalStart()
      {
         // Method intentionally left empty as this strictly simulates a web site
         // but doesn't actually crate one or leverage the ASP subsystem.
      }
   }
}
