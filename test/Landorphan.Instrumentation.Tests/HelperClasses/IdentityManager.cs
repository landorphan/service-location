using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using Landorphan.Instrumentation.Interfaces;
   using TechTalk.SpecFlow;

   public class IdentityManager : IInstrumentationIdentityManager
   {
      private ScenarioContext scenarioContext;

      public IdentityManager(ScenarioContext scenarioContext)
      {
         this.scenarioContext = scenarioContext;
      }

      public string UserId { get; set; }

      public string GetAnonymousUserId()
      {
         return UserId;
      }

      public void IdentifyUser(string userIdentity, object userData)
      {
         var testData = scenarioContext.Get<TestData>();
         testData.UserIdentity = userIdentity;
         testData.UserData = (UserData) userData;
      }
   }
}
