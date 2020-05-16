namespace Landorphan.InstrumentationManagement
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Landorphan.InstrumentationManagement.Implementation;
    using Landorphan.InstrumentationManagement.Interfaces;

    /// <summary>
   /// This is the main Instrumentation class is used to interact
   /// with the Instrumentation system.
   /// </summary>
#pragma warning disable CA1724 // name Instrumentation conflicts with System.Management.Instrumentation -- This name is prefered. 
   public class Instrumentation : IInstrumentationRecordMethod, IInstrumentationRecordAction
#pragma warning restore CA1724
   {
       internal static readonly Func<Instrumentation> originalGetInstance = () => singleton;
       internal static readonly Action<Instrumentation> originalSetInstance = x => singleton = x;
#pragma warning disable S2223 // Non-constant static fields should not be visible -- Used for Test Hooks as this project does not take an IOC dependency.
       internal static Func<Instrumentation> getInstance = originalGetInstance;
#pragma warning restore S2223 // Non-constant static fields should not be visible
#pragma warning disable S2223 // Non-constant static fields should not be visible -- Used for Test Hooks as this project does not take an IOC dependency.
       internal static Action<Instrumentation> setInstance = originalSetInstance;
#pragma warning restore S2223 // Non-constant static fields should not be visible
       private static readonly List<Exception> bootstrapErrors = new List<Exception>(new Exception[] { new InvalidOperationException("Bootstrap not called") });

       private static readonly object lockObject = new object();

       /// <summary>
      /// The instrumentation singleton instance.  Only one instrumentation
      /// instance should exist at a time.
      /// </summary>
      private static Instrumentation singleton;

       private readonly InstrumentationBootstrapData bootstrapData;

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

       /// <summary>
      /// Gets the current Instrumentation instance.
      /// </summary>
      public static Instrumentation Current => getInstance();

       /// <summary>
      /// Gets a flag indicating if instrumentation has been bootstrapped.
      /// </summary>
      public static bool IsBootstrapped => Current != null;

       /// <summary>
      /// Gets the Instrumentation Context.
      /// </summary>
      public IInstrumentationContextManager Context { get; private set; }

       /// <inheritdoc />
      public void RecordAction(string actionName, KeyValuePair<string, string>[] actionTags)
      {
         bootstrapData.Logger.LogAction(actionName, Context, actionTags);
      }

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
               var bootstrapFailure = false;
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
#pragma warning disable S4056 // Overloads with a "CultureInfo" or an "IFormatProvider" parameter should be used
                              // -- This rule is not useful on this method as the value will be returned.  Further
                              // This is strictly doing a null check.
                     if (ReferenceEquals(null, propertyInfo.GetValue(bootstrapData)))
#pragma warning restore S4056 // Overloads with a "CultureInfo" or an "IFormatProvider" parameter should be used
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
   }
}
