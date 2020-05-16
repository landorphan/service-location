namespace Landorphan.InstrumentationManagement
{
    using Landorphan.InstrumentationManagement.PlugIns;

    /// <summary>
   /// Provides information on the bootstrapping of the
   /// Instrumentation system.
   /// </summary>
   public class InstrumentationBootstrapData
   {
       /// <summary>
      /// The name of the main application entry point for the application.
      /// Typically the function where bootstrapping occurs.
      /// </summary>
      public string ApplicationEntryPointName { get; set; }

       /// <summary>
      /// Gets or sets the name of the application.
      /// </summary>
      public string ApplicationName { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginPerfManager"/> used
      /// to create and manage APM traces.
      /// </summary>
      public IInstrumentationPluginPerfManager ApplicationPerformanceManager { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginStorage"/> used to store
      /// data between async calls.
      /// </summary>
      public IInstrumentationPluginStorage AsyncStorage { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginEntryPointStorage"/> used
      /// to allow access to the currently defined entry point.
      /// </summary>
      public IInstrumentationPluginEntryPointStorage EntryPointStorage { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginIdentityManager"/> used to
      /// determine and set user Identity Information.
      /// </summary>
      public IInstrumentationPluginIdentityManager IdentityManager { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginLog"/> used
      /// to log data to the underlying data store.
      /// </summary>
      public IInstrumentationPluginLog Logger { get; set; }

       /// <summary>
      /// Gets or sets an implementation of <see cref="IInstrumentationPluginStorage"/> used to store
      /// data within a user session.
      /// </summary>
      public IInstrumentationPluginStorage SessionStorage { get; set; }
   }
}
