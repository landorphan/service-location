namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
   using Landorphan.InstrumentationManagement.PlugIns;
   using TechTalk.SpecFlow;

   public class PluginIdentityManager : IInstrumentationPluginIdentityManager
   {
      private readonly ScenarioContext scenarioContext;

      public PluginIdentityManager(ScenarioContext scenarioContext)
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
