using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.MockApplications.WebApp
{
   using System.Linq;
   using System.Reflection;
   using Landorphan.Instrumentation.Tests.HelperClasses;



   [LogMethod]
   public class MockWebApp
   {
      public static Dictionary<string, MethodInfo> WebMethods;

      static MockWebApp()
      {
         WebMethods = new Dictionary<string, MethodInfo>();
         var methdos = (from m in typeof(MockWebApp).GetMethods()
                       where m.Name.Contains("WebMethod")
                      select m);
         foreach (var method in methdos)
         {
            WebMethods.Add(method.Name, method);
         }
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

      }
   }
}
