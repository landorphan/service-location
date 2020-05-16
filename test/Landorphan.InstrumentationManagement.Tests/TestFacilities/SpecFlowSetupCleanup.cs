namespace Landorphan.InstrumentationManagement.Tests.TestFacilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Landorphan.InstrumentationManagement.Tests.HelperClasses;
    using Landorphan.InstrumentationManagement.Tests.MockApplications;
    using Landorphan.InstrumentationManagement.Tests.Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using TechTalk.SpecFlow.Assist.ValueRetrievers;

    [Binding]
   public sealed class SpecFlowSetupCleanup
   {
       private readonly FeatureContext featureContext;
       private readonly ScenarioContext scenarioContext;

       // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
       public SpecFlowSetupCleanup(ScenarioContext scenarioContext, FeatureContext featureContext)
      {
         this.scenarioContext = scenarioContext;
         this.featureContext = featureContext;
      }

       [BeforeTestRun]
      public static void BeforeTestRun()
      {
         Service.Instance.ValueRetrievers.Register(new NullValueRetriever("(null)"));
      }

      [BeforeScenario]
      public void BeforeScenario()
      {
         scenarioContext.Set((MockWebApp)null);
         var formOrMethods = new Dictionary<string, object>();
         scenarioContext.Set(formOrMethods, nameof(formOrMethods));
         scenarioContext.Set((Instrumentation) null);
         scenarioContext.Set<object>(null, "return");
         Instrumentation.getInstance = () => scenarioContext.Get<Instrumentation>();
         Instrumentation.setInstance = x => scenarioContext.Set(x);
         scenarioContext.Set(new TestLogger());
         scenarioContext.Set(new TestData());
         scenarioContext.Set(new TestEntryPointStorage());
         scenarioContext.Set(new TestPerfManager());
         var identityManager = new PluginIdentityManager(scenarioContext);
         scenarioContext.Set(identityManager);
         //TODO: implement logic that has to run before executing each scenario
      }

      [AfterScenario]
      public void AfterScenario()
      {
         Dictionary<string, object> formOrMethods = scenarioContext.Get<Dictionary<string, object>>(nameof(formOrMethods));

         var names = formOrMethods.Keys.ToArray();

         foreach (var name in names)
         {
            var instance = formOrMethods[name];
            var forOrWeb = "for";
            if (instance is MethodInfo)
            {
               forOrWeb = "web";
            }
            InstrumentationSteps.CloseForOrCompleteMethod(forOrWeb, name, scenarioContext);
         }

         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         foreach (var entryPoint in entryPointStorage.cotextEntryPoints)
         {
            entryPoint.Value.Dispose();
         }
         entryPointStorage.cotextEntryPoints.Clear();

         Instrumentation.getInstance = Instrumentation.originalGetInstance;
         Instrumentation.setInstance = Instrumentation.originalSetInstance;

         //TODO: implement logic that has to run after executing each scenario
      }
   }
}
