using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Collections.Concurrent;
   using System.Collections.ObjectModel;
   using System.Threading;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;

   public class AsyncStorage : IInstrumentationPluginStorage
   {
      private static object lockObject = new object();
      private AsyncLocal<ConcurrentDictionary<string, object>> local = new AsyncLocal<ConcurrentDictionary<String, Object>>();

      public void Set(string name, object value)
      {
         ConcurrentDictionary<string, object> localStorage;
         localStorage = local.Value;
         if (localStorage == null)
         {
            localStorage = new ConcurrentDictionary<String, Object>();
            local.Value = localStorage;
         }

         localStorage[name] = value;
      }

      public object Get(string name)
      {
         ConcurrentDictionary<string, object> localStorage;
         localStorage = local.Value;
         return localStorage[name];
      }
   }
}
