namespace Landorphan.Instrumentation.Tests.Steps
{
   using System;
   using System.Collections.Generic;
   using System.Net.Security;
   using System.Reflection;
   using System.Reflection.Metadata;
   using System.Threading;
   using FluentAssertions;
   using FluentAssertions.Common;
   using Landorphan.Instrumentation.Implementation;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.Tests.HelperClasses;
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
         var userIdentity = scenarioContext.Get<IdentityManager>();
         userIdentity.UserId = value;
      }

      [Then(@"the value of (.*) should (not )?be '(.*)'")]
      public void WhenIEvaluate(string name, string isNot, string value)
      {
         // throw new NotImplementedException();
         var split = name.Split(".");
         PropertyInfo propertyInfo = null;
         object retval = null;
         object instance;
         switch (split[0])
         {
            case "Instrumentation":
               propertyInfo = typeof(Instrumentation).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(null);
               break;
            case "Context":
               propertyInfo = typeof(InstrumentationContextManager).GetFirstPropertyByName(split[1]);
               retval = propertyInfo.GetValue(Instrumentation.Current.Context);
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
               break;
         }
         ThenTheReturnValueShouldBe(isNot, value, retval);
      }

      private UserData internalUserData = null;

      [Given(@"I set the value of (.*) to '(.*)'")]
      public void GivenISetTheValueOfInternalUserData_EmailTo(string name, string value)
      {
         var split = name.Split(".");
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
         var testBootStrapInfo = scenarioContext.Get<InstrumentationTestBootstrapSetup>();
         InstrumentationBootstrapData bootstrapData = new InstrumentationBootstrapData();
         if (testBootStrapInfo.SetAsyncStorage)
         {
            bootstrapData.AsyncStorage = new AsyncStorage();
            scenarioContext.Set<IInstrumentationStorage>(bootstrapData.AsyncStorage, "Async");
         }
         if (testBootStrapInfo.SetSessionStorage)
         {
            bootstrapData.SessionStorage = new SessionStorage();
            scenarioContext.Set<IInstrumentationStorage>(bootstrapData.SessionStorage, "Session");
         }
         if (testBootStrapInfo.SetIdentityManager)
         {
            bootstrapData.IdentityManager = scenarioContext.Get<IdentityManager>();
         }

         bootstrapData.ApplicationName = testBootStrapInfo.Application.ToString();
         if (!testBootStrapInfo.SetBootstrapData)
         {
            bootstrapData = null;
         }

         Instrumentation.Bootstrap(bootstrapData);
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

         IInstrumentationStorage storage = scenarioContext.Get<IInstrumentationStorage>(sessionOrAsync);
         var actual = storage.Get(name);
         actual.Should().Be(typedExpectedValue);
      }


      [When(@"I set the (Session|Async) Storage Value '(.*)' to '(.*)'")]
      public void WhenISetTheSessionOrAsyncStorageValueTo(string sessionOrAsync, string name, string value)
      {
         var field = typeof(WellKnownStorageValues).GetField(name);
         var type = field.FieldType;
         object typedObjectValue = GetValueAsType(name, value, type);

         IInstrumentationStorage storage = scenarioContext.Get<IInstrumentationStorage>(sessionOrAsync);
         storage.Set(name, typedObjectValue);
      }

      private static object GetValueAsType(string name, string value, Type type)
      {
         KeyValuePair<string, string> pair = new KeyValuePair<string, string>(name, value);
         Table holdTable = new Table(new string[] {"Field", "Value"});
         holdTable.AddRow(new string[] {name, value});
         TableRow row = holdTable.Rows[0];
         var retriever = Service.Instance.GetValueRetrieverFor(row, type, type);
         return retriever.Retrieve(pair, type, type);
      }
   }
}
