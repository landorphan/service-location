namespace Landorphan.InstrumentationManagement.Interfaces
{
    using System;
    using Landorphan.InstrumentationManagement.PlugIns;

    /// <summary>
   /// Manages the tracking of an entry point.
   /// </summary>
   public interface IEntryPointExecution : IDisposable
   {
       /// <summary>
      /// Gets the name of the entry point.
      /// </summary>
      string EntryPointName { get; }

       /// <summary>  
      /// Returns the performance trace for the current entry point.
      /// </summary>
      IPerfTrace Trace { get; }
   }
}
