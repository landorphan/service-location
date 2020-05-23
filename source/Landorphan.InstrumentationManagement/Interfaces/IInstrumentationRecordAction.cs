namespace Landorphan.InstrumentationManagement.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to record a user action within the Instrumentation
    /// system.  This is an event that would have business relevance
    /// to track such as opening a form or clicking a button.
    /// </summary>
    public interface IInstrumentationRecordAction
    {
        /// <summary>
        /// Records an action that occurs on behalf of the user.
        /// </summary>
        /// <param name="actionName">
        ///    The name of the action.
        /// </param>
        /// <param name="actionTags">
        ///    The tags that occur as part of the action.
        /// </param>
        void RecordAction(string actionName, KeyValuePair<string, string>[] actionTags);
    }
}
