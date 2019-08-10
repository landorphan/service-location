namespace Landorphan.Instrumentation
{
   using System;
   using Landorphan.Instrumentation.Implementation;
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// This is the main Instrumentation class is used to interact
   /// with the Instrumentation system.
   /// </summary>
   public class Instrumentation
   {
      internal static Action<Instrumentation> setInstance = x => singleton = x;
      internal static Func<Instrumentation> getInstance = () => singleton;

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
      internal Instrumentation(IInstrumentationContext context)
      {
         Context = context;
      }

      /// <summary>
      /// Bootstraps the Instrumentation system.
      /// </summary>
      /// <param name="asyncStorage">
      /// Implementation of AsyncStorage system used to store data between
      /// async calls.
      /// </param>
      /// <param name="bootstrapData">
      /// Application specific bootstrap data.
      /// </param>
      public static void Bootstrap(IInstrumentationAsyncStorage asyncStorage,
                                   InstrumentationBootstrapData bootstrapData)
      {
         if (Current == null)
         {
            setInstance(new Instrumentation(new InstrumentationContext()));
         }
      }

      /// <summary>
      /// Gets the Instrumentation Context.
      /// </summary>
      public IInstrumentationContext Context { get; private set; }

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
