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
      public static Dictionary<string, MethodInfo> WebMethods = new Dictionary<string, MethodInfo>(
                                                            from m in typeof(MockWebApp).GetMethods()
                                                            where m.Name.Contains("WebMethod")
                                                            select new KeyValuePair<string, MethodInfo>(m.Name, m));

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
