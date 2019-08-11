namespace Landorphan.Instrumentation.Interfaces
{
   using System;

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
      /// Called when the system has identified the user via it's authentication
      /// system.
      /// </summary>
      /// <param name="userId">
      /// A string that represents the user's actual identity.
      /// </param>
      void IdentifyUser(string userId);
   }
}
