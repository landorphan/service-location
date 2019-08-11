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
      private readonly ScenarioContext _scenarioContext;
      private readonly FeatureContext _featureContext;

      public InstrumentationSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
      {
         _scenarioContext = scenarioContext;
         _featureContext = featureContext;
      }

      [Given(@"I do nothing")]
      public void GivenIDoNothing()
      {
      }

      [Given(@"I mock the user id to be '(.*)'")]
      public void GivenISetTheUserIdToBe(string value)
      {
         var userIdentity = _scenarioContext.Get<IdentityManager>();
         userIdentity.UserId = value;
      }

      [Then(@"the value of (.*) should (not )?be '(.*)'")]
      public void WhenIEvaluate(string name, string isNot, string value)
      {
         // throw new NotImplementedException();
         var split = name.Split(".");
         PropertyInfo propertyInfo = null;
         object retval = null;
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
            default:
               break;
         }
         ThenTheReturnValueShouldBe(isNot, value, retval);
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
         _scenarioContext.Set(testBootStrapInfo);
      }

      [Given(@"I override bootstrap data with:")]
      public void GivenIOverrideBootstrapDataWith(Table table)
      {
         var testBootStrapInfo = _scenarioContext.Get<InstrumentationTestBootstrapSetup>();
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
         var testBootStrapInfo = _scenarioContext.Get<InstrumentationTestBootstrapSetup>();
         InstrumentationBootstrapData bootstrapData = new InstrumentationBootstrapData();
         if (testBootStrapInfo.SetAsyncStorage)
         {
            bootstrapData.AsyncStorage = new AsyncStorage();
            _scenarioContext.Set<IInstrumentationStorage>(bootstrapData.AsyncStorage, "Async");
         }
         if (testBootStrapInfo.SetSessionStorage)
         {
            bootstrapData.SessionStorage = new SessionStorage();
            _scenarioContext.Set<IInstrumentationStorage>(bootstrapData.SessionStorage, "Session");
         }
         if (testBootStrapInfo.SetIdentityManager)
         {
            bootstrapData.IdentityManager = _scenarioContext.Get<IdentityManager>();
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
      }

      [When(@"I set the (Session|Async) Storage Value '(.*)' to '(.*)'")]
      public void WhenISetTheSessionOrAsyncStorageValueTo(string sessionOrAsync, string name, string value)
      {
         var field = typeof(WellKnownStorageValues).GetField(name);
         var type = field.FieldType;
         object typedObjectValue = GetValueAsType(name, value, type);

         IInstrumentationStorage storage = _scenarioContext.Get<IInstrumentationStorage>(sessionOrAsync);
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
