using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Interfaces
{
   /// <summary>
   /// Provides Context Data for Instrumentation.
   /// </summary>
   public interface IInstrumentationContext
   {
      /// <summary>
      /// Returns the root Application Name.  This is the name of the
      /// application as launched.
      /// </summary>
      string RootApplicationName { get; }

      /// <summary>
      /// Returns the session ID for the current session.
      /// </summary>
      Guid SessionId { get; }

      /// <summary>
      /// Gets a value indicating if the system is in a session.
      /// True if the system is in a session otherwise false.
      /// </summary>
      bool IsInSession { get; }

      /// <summary>
      /// Gets the name of the application currently executing code.
      /// </summary>
      string ExecutingApplicationName { get; }

      /// <summary>
      /// Get's the user's anonymous identity to be used to identify the user
      /// within the session.
      /// </summary>
      string UserAnonymousIdentity { get; }

      /// <summary>
      /// Get's the user's actual identity (if known) otherwise null.
      /// </summary>
      string UserIdentity { get; }

      /// <summary>
      /// Provides data associated with the User.  This can be set to
      /// any type designated by the application.
      /// </summary>
      object UserData { get; }
   }
}
