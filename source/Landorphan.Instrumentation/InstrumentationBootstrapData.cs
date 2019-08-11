namespace Landorphan.Instrumentation
{
   using System;
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// Provides information on the bootstrapping of the
   /// Instrumentation system.
   /// </summary>
   public class InstrumentationBootstrapData
   {
      /// <summary>
      /// Gets or sets the name of the application.
      /// </summary>
      public string ApplicationName { get; set; }

      /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationStorage"/> used to store
      /// data between async calls.
      /// </summary>
      public IInstrumentationStorage AsyncStorage { get; set; }

      /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationStorage"/> used to store
      /// data within a user session.
      /// </summary>
      public IInstrumentationStorage SessionStorage { get; set; }

      /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationIdentityManager"/> used to
      /// determine and set user Identity Information.
      /// </summary>
      public IInstrumentationIdentityManager IdentityManager { get; set; }
   }
}
