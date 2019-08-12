namespace Landorphan.Instrumentation
{
   using System;
   using System.Collections.Generic;
   using System.Reflection;
   using Landorphan.Instrumentation.Implementation;
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// This is the main Instrumentation class is used to interact
   /// with the Instrumentation system.
   /// </summary>
   public class Instrumentation
   {
      internal static readonly Action<Instrumentation> originalSetInstance = x => singleton = x;
      internal static Action<Instrumentation> setInstance = originalSetInstance;

      internal static Func<Instrumentation> originalGetInstance = () => singleton;
      internal static Func<Instrumentation> getInstance = originalGetInstance;

      /// <summary>
      /// The instrumentation singleton instance.  Only one instrumentation
      /// instance should exist at a time.
      /// </summary>
      private static Instrumentation singleton;

      /// <summary>
      /// Constructor is private to force the use of bootstrapping pattern.
      /// </summary>
      /// <param name="context">
      /// The instrumentation context which tracks the current state of
      /// instrumentation information.
      /// </param>
      internal Instrumentation(IInstrumentationContextManager context)
      {
         Context = context;
      }

      private static readonly object lockObject = new Object();
      private static List<Exception> bootstrapErrors = new List<Exception>(new Exception[] { new InvalidOperationException("Bootstrap not called") });

      /// <summary>
      /// Bootstraps the Instrumentation system.
      /// </summary>
      /// <param name="bootstrapData">
      /// Application specific bootstrap data.
      /// </param>
      public static void Bootstrap(InstrumentationBootstrapData bootstrapData)
      {
         lock(lockObject)
         {
            if (Current == null)
            {
               bootstrapErrors.Clear();
               // NOTE: To prevent Instrumentation Exceptions shutting down the application.  No Exceptions are thrown
               // Exceptions are instead the responsibility of the bootstrapper, via a call to
               // ThrowIfNotBootstrapped method.
               bool bootstrapFailure = false;
               if (bootstrapData == null)
               {
                  bootstrapErrors.Add(new ArgumentException("the argument bootstrapData can not be null."));
                  bootstrapFailure = true;
               }
               else
               {
                  var properties = typeof(InstrumentationBootstrapData).GetRuntimeProperties();

                  foreach (var propertyInfo in properties)
                  {
                     if (object.ReferenceEquals(null, propertyInfo.GetValue(bootstrapData)))
                     {
                        bootstrapErrors.Add(new ArgumentException($"the bootstrapData property {propertyInfo.Name} can not be null."));
                        bootstrapFailure = true;
                     }
                  }
               }
               if (!bootstrapFailure)
               {
                  bootstrapData.AsyncStorage.Set(nameof(InstrumentationContextManager.RootApplicationName), bootstrapData.ApplicationName);
                  setInstance(new Instrumentation(new InstrumentationContextManager(bootstrapData)
                  {
                  }));
               }
            }
         }
      }

      /// <summary>
      /// Gets the Instrumentation Context.
      /// </summary>
      public IInstrumentationContextManager Context { get; private set; }

      /// <summary>
      /// Gets the current Instrumentation instance.
      /// </summary>
      public static Instrumentation Current => getInstance();

      /// <summary>
      /// Gets a flag indicating if instrumentation has been bootstrapped.
      /// </summary>
      public static bool IsBootstrapped => Current != null;
   }
}
