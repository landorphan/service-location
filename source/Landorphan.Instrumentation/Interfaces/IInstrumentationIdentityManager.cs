using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Interfaces
{
   /// <summary>
   /// Provides a method for the application to control how
   /// users are identified within the application.
   /// </summary>
   public interface IInstrumentationIdentityManager
   {
      /// <summary>
      /// Get's an anonymous user identity for a user who has
      /// not initiated a login action.
      /// </summary>
      /// <returns>
      /// A unique identifier that identifies this user throughout the session
      /// life cycle.
      /// </returns>
      string GetAnonymousUserId();
   }
}
