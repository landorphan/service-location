namespace Landorphan.InstrumentationManagement.Implementation
{
    using Landorphan.Common;
    using Landorphan.InstrumentationManagement.Interfaces;
    using Landorphan.InstrumentationManagement.PlugIns;

    /// <summary>
   /// Represents an Entry Point within the applciaiton.
   /// </summary>
   public class EntryPointExecution : DisposableObject, IEntryPointExecution
   {
       /// <summary>
      /// Creates a new instance of the EntryPointExecution class.
      /// </summary>
      /// <param name="trace">
      /// The trace that was created for the entry point.
      /// </param>
      /// <param name="name">
      /// The name of the entry point.
      /// </param>
      public EntryPointExecution(IPerfTrace trace, string name)
      {
         Trace = trace;
         EntryPointName = name;
      }

       /// <inheritdoc />
      public string EntryPointName { get; private set; }

       /// <inheritdoc />
      public IPerfTrace Trace { get; private set; }
   }

   ///// <summary>
   ///// Used to record the creation of an entry point in the application.
   ///// </summary>
   //public interface IInstrumentationRecordEntryPoint
   //{
   //   /// <summary>
   //   /// Creates an entry point into the application.
   //   /// </summary>
   //   /// <param name="entryPointName">
   //   /// The name of the entry point.
   //   /// </param>
   //   /// <returns>
   //   /// An object used to track the life cycle of the entry point.
   //   /// </returns>
   //   IEntryPointExecution CreateEntryPoint(string entryPointName);
   //}
}
