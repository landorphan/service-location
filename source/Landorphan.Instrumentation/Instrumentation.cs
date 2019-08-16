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
   public class Instrumentation : IInstrumentationRecordMethod
   {
      internal static readonly Action<Instrumentation> originalSetInstance = x => singleton = x;
      internal static Action<Instrumentation> setInstance = originalSetInstance;

      internal static Func<Instrumentation> originalGetInstance = () => singleton;
      internal static Func<Instrumentation> getInstance = originalGetInstance;

      private InstrumentationBootstrapData bootstrapData;

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
      /// <param name="bootstrapData">
      /// The bootstrap data for this implementation.
      /// </param>
      internal Instrumentation(IInstrumentationContextManager context, InstrumentationBootstrapData bootstrapData)
      {
         Context = context;
         this.bootstrapData = bootstrapData;
      }

      private static readonly object lockObject = new Object();
      private static List<Exception> bootstrapErrors = new List<Exception>(new Exception[] { new InvalidOperationException("Bootstrap not called") });

      /// <summary>
      /// Bootstraps the Instrumentation system.
      /// </summary>
      /// <param name="bootstrapData">
      /// Application specific bootstrap data.
      /// </param>
      public static IEntryPointExecution Bootstrap(InstrumentationBootstrapData bootstrapData)
      {
         IEntryPointExecution retval = null;
         lock (lockObject)
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
                  var trace = bootstrapData.ApplicationPerformanceManager.StartTrace(bootstrapData.ApplicationEntryPointName);
                  retval = new EntryPointExecution(trace, trace.Name);
                  bootstrapData.AsyncStorage.Set(nameof(InstrumentationContextManager.RootApplicationName), bootstrapData.ApplicationName);
                  setInstance(new Instrumentation(new InstrumentationContextManager(bootstrapData)
                  {
                     ApplicationEntryPoint = retval
                  }, bootstrapData));
               }
            }
         }

         return retval;
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

      /// <summary>
      /// Called when a method is entered to manage recording data around the
      /// method call.
      /// </summary>
      /// <param name="compilationData">
      /// The compilation data for the method call.
      /// </param>
      /// <param name="arguments">
      /// The arguments to the method.
      /// </param>
      /// <returns></returns>
      public IMethodExecution EnterMethod(IMethodCompilationData compilationData, ArgumentData[] arguments)
      {
         return new MethodExecution(compilationData, bootstrapData, Context, arguments);
      }

   }
}
