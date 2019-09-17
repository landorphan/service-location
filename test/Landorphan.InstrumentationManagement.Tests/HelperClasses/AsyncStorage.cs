namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
#pragma warning disable CS8019 // Unnecessary using directive -- This is needed for some targets and not others.  Keeping in for all.
   using System;
#pragma warning restore CS8019 // Unnecessary using directive
   using System.Collections.Concurrent;
   using Landorphan.InstrumentationManagement.PlugIns;

   public class AsyncStorage : IInstrumentationPluginStorage
   {
#if NETCORE
      private readonly System.Threading.AsyncLocal<ConcurrentDictionary<string, object>> asyncLocal = new System.Threading.AsyncLocal<ConcurrentDictionary<string, object>>();
#endif

      public void Set(string name, object value)
      {
#if NETFX
         if (!(System.Runtime.Remoting.Messaging.CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<string, object> localStorage))
         {
            localStorage = new ConcurrentDictionary<string, object>();
            System.Runtime.Remoting.Messaging.CallContext.SetData(nameof(AsyncStorage), localStorage);
         }

         localStorage[name] = value;
#elif NETCORE
         if (!(asyncLocal.Value is ConcurrentDictionary<string, object> localStorage))
         {
            localStorage = new ConcurrentDictionary<string, object>();
            asyncLocal.Value = localStorage;
         }
         localStorage[name] = value;
#else
         throw new NotImplementedException("This operation is not implemented for this runtime.");
#endif

      }

      public object Get(string name)
      {
#if NETFX
         object retval = null;
         if(System.Runtime.Remoting.Messaging.CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<string, object> localStorage)
         {
            retval = localStorage[name];
         }
         return retval;
#elif NETCORE
         object retval = null;
         if (asyncLocal.Value is ConcurrentDictionary<string, object> localStorage)
         {
            retval = localStorage[name];
         }
         return retval;
#else
         throw new NotImplementedException("This operation is not implemented for this runtime.");
#endif
      }
   }
}
