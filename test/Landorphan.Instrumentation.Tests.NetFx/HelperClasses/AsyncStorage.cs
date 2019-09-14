using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Collections.Concurrent;
   using System.Collections.ObjectModel;
   using System.Runtime.Remoting.Messaging;
   using System.Threading;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;

   public class AsyncStorage : IInstrumentationPluginStorage
   {
      private static object lockObject = new object();

      public void Set(string name, object value)
      {
         if (!(CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<String, Object> localStorage))
         {
            localStorage = new ConcurrentDictionary<String, Object>();
            CallContext.SetData(nameof(AsyncStorage), localStorage);
         }

         localStorage[name] = value;
      }

      public object Get(string name)
      {
         object retval = null;
         if(CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<String, Object> localStorage)
         {
            retval = localStorage[name];
         }
         return retval;
      }
   }
}
