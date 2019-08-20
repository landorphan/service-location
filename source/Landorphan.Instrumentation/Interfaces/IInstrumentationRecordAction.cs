namespace Landorphan.Instrumentation.Interfaces
{
   using System.Collections.Generic;

   /// <summary>
   /// Used to record a user action within the Instrumentation
   /// system.  This is an event that would have business relevance
   /// to track such as opening a form or clicking a button.
   /// </summary>
   public interface IInstrumentationRecordAction
   {
      void RecordAction(string actionName, KeyValuePair<string, string> actionTags);
   }
}
