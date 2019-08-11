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
   }
}
