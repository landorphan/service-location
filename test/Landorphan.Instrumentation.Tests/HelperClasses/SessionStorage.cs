using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using Landorphan.Instrumentation.Interfaces;

   public class SessionStorage : IInstrumentationStorage
   {
      Dictionary<string, object> sessionValues = new Dictionary<string, object>();
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
