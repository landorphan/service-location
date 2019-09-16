namespace Landorphan.InstrumentationManagement.Interfaces
{
   /// <summary>
   /// Provides system level context for the Instrumentation assembly.  This
   /// provides information about the current state of the assembly and what will
   /// be reported when records are sent.
   /// </summary>
   public interface IInstrumentationContextManager : IInstrumentationContext
   {
      /// <summary>
      /// Enters a new user session.
      /// </summary>
      void EnterSession();

      /// <summary>
      /// Set's arbitrary data for the session.
      ///
      /// NOTE: For performance reasons only strings are allowed.
      /// NOTE: To set more complex times the caller will need to serialize the instance.
      /// </summary>
      /// <param name="key">
      /// The name of the session level data to set.
      /// </param>
      /// <param name="value">
      /// The value of the session data.
      /// </param>
      void SetSessionData(string key, string value);

      /// <summary>
      /// Get's a previously set data value for the session.
      /// </summary>
      /// <param name="name">
      /// The name of the session data to get.
      /// </param>
      /// <returns>
      /// The value of the session data
      /// </returns>
      string GetSessionData(string name);

      /// <summary>
      /// Called when the system has identified the user via it's authentication
      /// system.
      /// </summary>
      /// <param name="userId">
      /// A string that represents the user's actual identity.
      /// </param>
      void IdentifyUser(string userId);

      /// <summary>
      /// Called when the system has identified the user via it's authentication
      /// system.
      /// </summary>
      /// <param name="userId">
      /// A string that represents the user's actual identity.
      /// </param>
      /// <param name="userData">
      /// Application defined data to associate with the user.
      /// </param>
      void IdentifyUser(string userId, object userData);

      /// <summary>
      /// Creates an entry point into the application.
      /// </summary>
      /// <param name="entryPointName">
      /// The name of the entry point.
      /// </param>
      /// <returns>
      /// An object used to track the life cycle of the entry point.
      /// </returns>
      IEntryPointExecution CreateEntryPoint(string entryPointName);
   }
}
