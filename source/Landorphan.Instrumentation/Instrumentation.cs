namespace Landorphan.Instrumentation
{
   using Landorphan.Instrumentation.Implementation;
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// This is the main Instrumentation class is used to interact
   /// with the Instrumentation system.
   /// </summary>
   public class Instrumentation
   {
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
      private Instrumentation(IInstrumentationContext context)
      {
         Context = context;
      }

      /// <summary>
      /// Bootstraps the Instrumentation system.
      /// </summary>
      /// <param name="asyncStorage">
      /// Implementation of Asy
      /// </param>
      /// <param name="bootstrapData"></param>
      public static void Bootstrap(IInstrumentationAsyncStorage asyncStorage,
                                   InstrumentationBootstrapData bootstrapData)
      {
         if (singleton == null)
         {
            singleton = new Instrumentation(new InstrumentationContext());
         }
      }

      public IInstrumentationContext Context { get; private set; }

      public static Instrumentation Current
      {
         get { return singleton; }
      }

      public static bool IsBootstrapped
      {
         get
         {
            return singleton != null;
         }
      }
   }
}
