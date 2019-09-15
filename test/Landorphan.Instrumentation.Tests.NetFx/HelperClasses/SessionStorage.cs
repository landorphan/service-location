using System.Collections.Generic;
namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using Landorphan.Instrumentation.PlugIns;

   public class SessionStorage : IInstrumentationPluginStorage
   {
      private readonly Dictionary<string, object> sessionValues = new Dictionary<string, object>();
      public void Set(string name, object value)
      {
         sessionValues[name] = value;
      }

      public object Get(string name)
      {
         sessionValues.TryGetValue(name, out var retval);
         return retval;
      }
   }
}
