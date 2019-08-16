using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.TestFacilities
{
   using System.Reflection;
   using System.Runtime.InteropServices;
   using Landorphan.Instrumentation.Tests.HelperClasses;
   using Landorphan.Instrumentation.Tests.MockApplications.WebApp;
   using Landorphan.Instrumentation.Tests.Steps;
   using TechTalk.SpecFlow.Assist;
   using TechTalk.SpecFlow.Assist.ValueRetrievers;

   [Binding]
   public sealed class SpecFlowSetupCleanup
   {
      private readonly ScenarioContext scenarioContext;
      private readonly FeatureContext featureContext;

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
         scenarioContext.Set<MockWebApp>((MockWebApp)null);
         Dictionary<string, object> formOrMethods = new Dictionary<string, object>();
         scenarioContext.Set<Dictionary<string, object>>(formOrMethods, nameof(formOrMethods));
         scenarioContext.Set<Instrumentation>((Instrumentation) null);
         scenarioContext.Set<object>(null, "return");
         Instrumentation.getInstance = () => scenarioContext.Get<Instrumentation>();
         Instrumentation.setInstance = x => scenarioContext.Set<Instrumentation>(x);
         scenarioContext.Set<TestLogger>(new TestLogger());
         scenarioContext.Set<TestData>(new TestData());
         scenarioContext.Set<TestEntryPointStorage>(new TestEntryPointStorage());
         scenarioContext.Set<TestPerfManager>(new TestPerfManager());
         var identityManager = new PluginIdentityManager(scenarioContext);
         scenarioContext.Set<PluginIdentityManager>(identityManager);
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
            string forOrWeb = "for";
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
