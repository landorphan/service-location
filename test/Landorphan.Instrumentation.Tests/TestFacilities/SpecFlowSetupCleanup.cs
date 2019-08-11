using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.TestFacilities
{
   using System.Runtime.InteropServices;
   using Landorphan.Instrumentation.Tests.HelperClasses;
   using TechTalk.SpecFlow.Assist;
   using TechTalk.SpecFlow.Assist.ValueRetrievers;

   [Binding]
   public sealed class SpecFlowSetupCleanup
   {
      private readonly ScenarioContext _scenarioContext;
      private readonly FeatureContext _featureContext;

      // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
      public SpecFlowSetupCleanup(ScenarioContext scenarioContext, FeatureContext featureContext)
      {
         _scenarioContext = scenarioContext;
         _featureContext = featureContext;
      }


      [BeforeTestRun]
      public static void BeforeTestRun()
      {
         Service.Instance.ValueRetrievers.Register(new NullValueRetriever("(null)"));
      }

      [BeforeScenario]
      public void BeforeScenario()
      {
         _scenarioContext.Set<Instrumentation>((Instrumentation) null);
         _scenarioContext.Set<object>(null, "return");
         Instrumentation.getInstance = () => _scenarioContext.Get<Instrumentation>();
         Instrumentation.setInstance = x => _scenarioContext.Set<Instrumentation>(x);
         var identityManager = new IdentityManager();
         _scenarioContext.Set<IdentityManager>(identityManager);
         //TODO: implement logic that has to run before executing each scenario
      }

      [AfterScenario]
      public void AfterScenario()
      {
         Instrumentation.getInstance = Instrumentation.originalGetInstance;
         Instrumentation.setInstance = Instrumentation.originalSetInstance;
         //TODO: implement logic that has to run after executing each scenario
      }
   }
}
