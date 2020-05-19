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

        public string GetWebMethod1(string value)
        {
            return value;
        }

        public void GlobalStart()
        {
            // Method intentionally left empty as this strictly simulates a web site
            // but doesn't actually crate one or leverage the ASP subsystem.
        }

        public string PostWebMethod1(string value)
        {
            return value;
        }

        public string PostWebMethod2(string value)
        {
            return value;
        }

        private static Dictionary<string, MethodInfo> GetWebMethods()
        {
            var retval = new Dictionary<string, MethodInfo>();
            var methods = (
                from m in typeof(MockWebApp).GetMethods()
#pragma warning disable S4058 // Overloads with a "StringComparison" parameter should be used
                where m.Name.Contains("WebMethod")
#pragma warning restore S4058 // Overloads with a "StringComparison" parameter should be used
                select m).ToList();
            foreach (var method in methods)
            {
                retval.Add(method.Name, method);
            }

            return retval;
        }
    }
}
