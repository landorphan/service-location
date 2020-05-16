namespace Landorphan.InstrumentationManagement.PlugIns
{
    using Landorphan.InstrumentationManagement.Interfaces;

    /// <summary>
   /// Used to manage tracking entry points within the
   /// application.
   /// </summary>
   public interface IInstrumentationPluginEntryPointStorage
   {
       ///// <summary>
       ///// Used to set the current entry point.  When called, Any existing
       ///// entry point for the context should be disposed.
       /////
       ///// The context is implementation specific for example in a web
       ///// application it would be a session (and a global context for out of
       ///// a session).
       /////
       ///// For a desktop application it may be a form for example.
       ///// </summary>
       ///// <param name="entryPoint"></param>
       //void SetEntryPoint(IEntryPointExecution entryPoint);

       /// <summary>
      /// Gets the current entry point for the context.
      /// </summary>
      IEntryPointExecution CurrentEntryPoint { get; }
   }
}
