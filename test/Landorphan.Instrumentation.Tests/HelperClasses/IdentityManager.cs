using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using Landorphan.Instrumentation.Interfaces;

   public class IdentityManager : IInstrumentationIdentityManager
   {
      public string UserId { get; set; }

      public string GetAnonymousUserId()
      {
         return UserId;
      }
   }
}
