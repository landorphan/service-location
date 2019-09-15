namespace Landorphan.Instrumentation.Tests.Steps
{
   using System;
   using System.Collections.Generic;
   using System.Reflection;
   using FluentAssertions;
   using Landorphan.Instrumentation.Implementation;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;
   using Landorphan.Instrumentation.Tests.HelperClasses;
   using Landorphan.Instrumentation.Tests.MockApplications.DesktopApp;
   using Landorphan.Instrumentation.Tests.MockApplications.WebApp;
   using TechTalk.SpecFlow;
   using TechTalk.SpecFlow.Assist;

   [Binding]
   public class InstrumentationSteps
   {
      private readonly ScenarioContext scenarioContext;
      private readonly FeatureContext featureContext;

      public InstrumentationSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
      {
         this.scenarioContext = scenarioContext;
         this.featureContext = featureContext;
      }

      [Given(@"I do nothing")]
      public void GivenIDoNothing()
      {
      }

      [Given(@"I mock the user id to be '(.*)'")]
      public void GivenISetTheUserIdToBe(string value)
      {
         var userIdentity = scenarioContext.Get<PluginIdentityManager>();
         userIdentity.UserId = value;
      }

      [When(@"the user requests an action from the (web) method '(.*)'")]
      [When(@"the user opens the (form) '(.*)'")]
      public void WhenTheUserOpensTheForm(string formOrMethod, string formName)
      {
         Dictionary<string, object> formOrMethods = scenarioContext.Get<Dictionary<string, object>>(nameof(formOrMethods));

         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         var entryPoint = Instrumentation.Current.Context.CreateEntryPoint(formName);
         entryPointStorage.cotextEntryPoints.Add(formName, entryPoint);
         entryPointStorage.CurrentContext = formName;
         Type type;
         MethodInfo method;
         switch (formOrMethod)
         {
            case "form":
               type = MockDesktopApp.FormsClasses[formName];
               var formInstance = (MyFormBase) Activator.CreateInstance(type);
               formInstance.OnCreateControl();
               formOrMethods[formName] = formInstance;
               break;
            case "web":
               MockWebApp mockWebApp = scenarioContext.Get<MockWebApp>();
               if (mockWebApp == null)
               {
                  mockWebApp = new MockWebApp();
                  scenarioContext.Set<MockWebApp>(mockWebApp);
               }
               method = MockWebApp.WebMethods[formName];
               formOrMethods[formName] = method;
               break;
            default:
               throw new InvalidOperationException($"Expecting form or web but got {formOrMethod}");
         }
      }

      [When(@"actions occur within the (form|web method) '(.*)'")]
      public void WhenActionsOccurWithinTheFormOrWebMethod(string ignore, string formOrWebMethodName)
      {
         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         entryPointStorage.CurrentContext = formOrWebMethodName;
      }

      [When(@"actions occur outside the scope of any (form|web method)")]
      public void WhenActionsOccurOutsideTheScopeOfAnyFormOrWebMethod(string ignore)
      {
         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         entryPointStorage.CurrentContext = Instrumentation.Current.Context.ApplicationEntryPoint.EntryPointName;
      }

      [When(@"the (form is closed|web method completes)")]
      public void WhenTheFormClosesOrTheWebMethodCompletes(string formOrWeb)
      {
         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         CloseForOrCompleteMethod(formOrWeb, entryPointStorage.CurrentContext, scenarioContext);
      }

      public static void CloseForOrCompleteMethod(string formOrWeb, string name, ScenarioContext scenarioContext)
      {
         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         var mockWebApp = scenarioContext.Get<MockWebApp>();
         Dictionary<string, object> formOrMethods = scenarioContext.Get<Dictionary<string, object>>(nameof(formOrMethods));
         formOrWeb = formOrWeb.Substring(0, 3);
         if (formOrWeb == "for")
         {
            var form = (MyFormBase)formOrMethods[name];
            form.OnClosing(new EventArgs());
         }
         else
         {
            var method = (MethodInfo)formOrMethods[name];
            var retval = method.Invoke(mockWebApp, new[] { "Hello World" });
         }
         formOrMethods.Remove(name);

         var entryPoint = entryPointStorage.cotextEntryPoints[name];
         entryPointStorage.cotextEntryPoints.Remove(name);
         entryPointStorage.CurrentContext = Instrumentation.Current.Context.ApplicationEntryPoint.EntryPointName;
         entryPoint.Dispose();

      }

      [Then(@"the value of (.*) should (not )?be '(.*)'")]
      public void WhenIEvaluate(string name, string isNot, string value)
      {
         // throw new NotImplementedException();
         var split = name.Split('.');
         PropertyInfo propertyInfo = null;
         object retval = null;
         object instance;
         var scope = split[0];
         switch (scope)
         {
            case "Instrumentation":
               propertyInfo = typeof(Instrumentation).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(null);
               break;
            case "Context":
               propertyInfo = typeof(InstrumentationContextManager).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(Instrumentation.Current.Context);
               break;
            case "ApplicationEntryPoint":
            case "CurrentEntryPoint":
               propertyInfo = typeof(IEntryPointExecution).GetFirstPropertyByName(split[1]);
               var context = Instrumentation.Current.Context;
               object refObj = context.ApplicationEntryPoint;
               if (scope == "CurrentEntryPoint")
               {
                  refObj = context.CurrentEntryPoint;
               }
               retval = propertyInfo.GetValue(refObj);
               break;
            case "Test":
               instance = scenarioContext.Get<TestData>();
               propertyInfo = typeof(TestData).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(instance);
               break;
            case "UserData":
               var testData = scenarioContext.Get<TestData>();
               instance = testData.UserData; 
               propertyInfo = typeof(UserData).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(instance);
               break;
            default:
               throw new InvalidOperationException("Unknown value class.");
               break;
         }
         ThenTheReturnValueShouldBe(isNot, value, retval);
      }

      private UserData internalUserData = null;

      [Given(@"I set the value of (.*) to '(.*)'")]
      public void GivenISetTheValueOfInternalUserData_EmailTo(string name, string value)
      {
         var split = name.Split('.');
         PropertyInfo propertyInfo = null;
         switch (split[0])
         {
            case "InternalUserData":
               if (internalUserData == null)
               {
                  internalUserData = new UserData();
                  scenarioContext.Set<UserData>(internalUserData, "InternalUserData");
               }
               propertyInfo = typeof(UserData).GetFirstPropertyByName(split[1]);
               propertyInfo.SetValue(internalUserData, value);
               break;
            default:
               throw new InvalidOperationException("Unexpected value type.");
               break;
         }
      }

      [When(@"I set a session data value of '(.*)' to '(.*)'")]
      public void WhenISetASessionValueOfTo(string name, string value)
      {
         Instrumentation.Current.Context.SetSessionData(name, value);
      }

      [Then(@"the session data value '(.*)' should be '(.*)'")]
      public void ThenTheSessionDataValueOfNameShouldBeValue(string name, string expected)
      {
         var actual = Instrumentation.Current.Context.GetSessionData(name);
         if (expected == "(null)")
         {
            expected = null;
         }
         actual.Should().Be(expected);
      }

      [When(@"I identify the user as '(.*)'")]
      public void WhenIIdentifyTheUserAs(string userIdentity)
      {
         Instrumentation.Current.Context.IdentifyUser(userIdentity, internalUserData);
      }

      public void ThenTheReturnValueShouldBe(string isNot, string value, object retval)
      {
         if (isNot.Length > 0)
         {
            retval.Should().NotBe(value);
         }
         else
         {
            if (value != "(null)")
            {
               retval.Should().NotBeNull();
            }
            else if (object.ReferenceEquals(null, retval))
            {
               retval = "(null)";
            }
            retval.ToString().Should().Be(value);
         }
      }

      [Given(@"I setup bootstrap data with:")]
      public void GivenISetupBootstrapWith(Table table)
      {
         var testBootStrapInfo = table.CreateInstance<InstrumentationTestBootstrapSetup>();
         scenarioContext.Set(testBootStrapInfo);
      }

      [Given(@"I override bootstrap data with:")]
      public void GivenIOverrideBootstrapDataWith(Table table)
      {
         var testBootStrapInfo = scenarioContext.Get<InstrumentationTestBootstrapSetup>();
         var bootStrapOverride = table.CreateInstance<InstrumentationTestBootstrapSetup>();
         var properties = typeof(InstrumentationTestBootstrapSetup).GetProperties();
         HashSet<string> tableFields = new HashSet<String>();
         foreach (var tableRow in table.Rows)
         {
            tableFields.Add(tableRow[0]);
         }
         foreach (var propertyInfo in properties)
         {
            if (tableFields.Contains(propertyInfo.Name))
            {
               propertyInfo.SetValue(testBootStrapInfo, propertyInfo.GetValue(bootStrapOverride));
            }
         }
      }

      [Given(@"Instrumentation is bootstrapped")]
      public void GivenInstrumentationIsBootstrapped()
      {
         Instrumentation.IsBootstrapped.Should().BeTrue();
      }

      [Given(@"I bootstrap Instrumentation")]
      public void GivenIBootstrapInstrumentation()
      {
         var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
         var testBootStrapInfo = scenarioContext.Get<InstrumentationTestBootstrapSetup>();
         InstrumentationBootstrapData bootstrapData = new InstrumentationBootstrapData();
         if (testBootStrapInfo.SetAsyncStorage)
         {
            bootstrapData.AsyncStorage = new AsyncStorage();
            scenarioContext.Set<IInstrumentationPluginStorage>(bootstrapData.AsyncStorage, "Async");
         }
         if (testBootStrapInfo.SetSessionStorage)
         {
            bootstrapData.SessionStorage = new SessionStorage();
            scenarioContext.Set<IInstrumentationPluginStorage>(bootstrapData.SessionStorage, "Session");
         }
         if (testBootStrapInfo.SetIdentityManager)
         {
            bootstrapData.IdentityManager = scenarioContext.Get<PluginIdentityManager>();
         }
         if (testBootStrapInfo.SetLogger)
         {
            bootstrapData.Logger = scenarioContext.Get<TestLogger>();
         }
         if (testBootStrapInfo.SetPerfManager)
         {
            bootstrapData.ApplicationPerformanceManager = scenarioContext.Get<TestPerfManager>();
         }
         if (testBootStrapInfo.SetEntryPointStorage)
         {
            bootstrapData.EntryPointStorage = entryPointStorage;
         }
         bootstrapData.ApplicationEntryPointName = testBootStrapInfo.ApplicationEntryPointName;
         bootstrapData.ApplicationName = testBootStrapInfo.Application.ToString();
         if (!testBootStrapInfo.SetBootstrapData)
         {
            bootstrapData = null;
         }

         var entryPoint = Instrumentation.Bootstrap(bootstrapData);
         if (entryPoint != null)
         {
            entryPointStorage.cotextEntryPoints.Add(bootstrapData.ApplicationEntryPointName, entryPoint);
            entryPointStorage.CurrentContext = bootstrapData.ApplicationEntryPointName;
         }
         // ScenarioContext.Current.Pending();
      }

      [Given(@"I enter a session")]
      [When(@"I enter a session")]
      public void WhenIEnterASession()
      {
         Instrumentation.Current.Context.EnterSession();
      }

      public class WellKnownStorageValues
      {
         public Guid SessionId;
         public string RootApplicationName;
         public string ExecutingApplicationName;
         public string Location;
      }

      [Then(@"the (Session|Async) Storage Value '(.*)' should be '(.*)'")]
      public void ThenTheSessionStorageValueShouldBe(string sessionOrAsync, string name, string expected)
      {
         var field = typeof(WellKnownStorageValues).GetField(name);
         var type = field.FieldType;
         object typedExpectedValue = GetValueAsType(name, expected, type);

         IInstrumentationPluginStorage storage = scenarioContext.Get<IInstrumentationPluginStorage>(sessionOrAsync);
         var actual = storage.Get(name);
         actual.Should().Be(typedExpectedValue);
      }


      [When(@"I set the (Session|Async) Storage Value '(.*)' to '(.*)'")]
      public void WhenISetTheSessionOrAsyncStorageValueTo(string sessionOrAsync, string name, string value)
      {
         var field = typeof(WellKnownStorageValues).GetField(name);
         var type = field.FieldType;
         object typedObjectValue = GetValueAsType(name, value, type);

         IInstrumentationPluginStorage storage = scenarioContext.Get<IInstrumentationPluginStorage>(sessionOrAsync);
         storage.Set(name, typedObjectValue);
      }

      private static object GetValueAsType(string name, string value, Type type)
      {
         KeyValuePair<string, string> pair = new KeyValuePair<string, string>(name, value);
         Table holdTable = new Table(new[] {"Field", "Value"});
         holdTable.AddRow(new[] { name, value});
         TableRow row = holdTable.Rows[0];
         var retriever = Service.Instance.GetValueRetrieverFor(row, type, type);
         return retriever.Retrieve(pair, type, type);
      }
   }
}
