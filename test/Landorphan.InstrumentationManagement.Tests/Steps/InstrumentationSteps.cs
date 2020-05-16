namespace Landorphan.InstrumentationManagement.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FluentAssertions;
    using Landorphan.Common;
    using Landorphan.InstrumentationManagement.Implementation;
    using Landorphan.InstrumentationManagement.Interfaces;
    using Landorphan.InstrumentationManagement.PlugIns;
    using Landorphan.InstrumentationManagement.Tests.HelperClasses;
    using Landorphan.InstrumentationManagement.Tests.MockApplications;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable S4487 // Unread "private" fields should be removed

    [Binding]
    public class InstrumentationSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        private UserData internalUserData;

        public InstrumentationSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }

        public static void CloseForOrCompleteMethod(string formOrWeb, string name, SpecFlowContext scenarioContext)
        {
            formOrWeb.ArgumentNotNull(nameof(formOrWeb));
            scenarioContext.ArgumentNotNull(nameof(scenarioContext));

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
                method.Invoke(mockWebApp, new object[] { "Hello World" });
            }
            formOrMethods.Remove(name);

            var entryPoint = entryPointStorage.cotextEntryPoints[name];
            entryPointStorage.cotextEntryPoints.Remove(name);
            entryPointStorage.CurrentContext = Instrumentation.Current.Context.ApplicationEntryPoint.EntryPointName;
            entryPoint.Dispose();

        }

        [Given(@"I bootstrap Instrumentation")]
        public void GivenIBootstrapInstrumentation()
        {
            var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
            var testBootStrapInfo = scenarioContext.Get<InstrumentationTestBootstrapSetup>();
            var bootstrapData = new InstrumentationBootstrapData();
            if (testBootStrapInfo.SetAsyncStorage)
            {
                bootstrapData.AsyncStorage = new AsyncStorage();
                scenarioContext.Set(bootstrapData.AsyncStorage, "Async");
            }
            if (testBootStrapInfo.SetSessionStorage)
            {
                bootstrapData.SessionStorage = new SessionStorage();
                scenarioContext.Set(bootstrapData.SessionStorage, "Session");
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
        }

        [Given(@"I do nothing")]
        public void GivenIDoNothing()
        {
            // Method intentionally left empty because this method is used to support the statement 'Given I do nothing'.
        }

        [Given(@"Instrumentation is bootstrapped")]
        public void GivenInstrumentationIsBootstrapped()
        {
            Instrumentation.IsBootstrapped.Should().BeTrue();
        }

        [Given(@"I override bootstrap data with:")]
        public void GivenIOverrideBootstrapDataWith(Table table)
        {
            table.ArgumentNotNull(nameof(table));
            var testBootStrapInfo = scenarioContext.Get<InstrumentationTestBootstrapSetup>();
            var bootStrapOverride = table.CreateInstance<InstrumentationTestBootstrapSetup>();
            var properties = typeof(InstrumentationTestBootstrapSetup).GetProperties();
            var tableFields = new HashSet<string>();
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

        [Given(@"I mock the user id to be '(.*)'")]
        public void GivenISetTheUserIdToBe(string value)
        {
            var userIdentity = scenarioContext.Get<PluginIdentityManager>();
            userIdentity.UserId = value;
        }

        [Given(@"I set the value of (.*) to '(.*)'")]
        public void GivenISetTheValueOfInternalUserData_EmailTo(string name, string value)
        {
            name.ArgumentNotNull(nameof(name));
            var split = name.Split('.');
            PropertyInfo propertyInfo;
            switch (split[0])
            {
                case "InternalUserData":
                    if (internalUserData == null)
                    {
                        internalUserData = new UserData();
                        scenarioContext.Set(internalUserData, "InternalUserData");
                    }
                    propertyInfo = typeof(UserData).GetFirstPropertyByName(split[1]);
                    propertyInfo.SetValue(internalUserData, value);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value type.");
            }
        }

        [Given(@"I setup bootstrap data with:")]
        public void GivenISetupBootstrapWith(Table table)
        {
            var testBootStrapInfo = table.CreateInstance<InstrumentationTestBootstrapSetup>();
            scenarioContext.Set(testBootStrapInfo);
        }

        public void ThenTheReturnValueShouldBe(string isNot, string value, object inputRetval)
        {
            var retval = inputRetval;
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
                else if (ReferenceEquals(null, retval))
                {
                    retval = "(null)";
                }
                retval.ToString().Should().Be(value);
            }
        }

        [Then(@"the session data value '(.*)' should be '(.*)'")]
        public void ThenTheSessionDataValueOfNameShouldBeValue(string name, string inputExpected)
        {
            var expected = inputExpected;
            var actual = Instrumentation.Current.Context.GetSessionData(name);
            if (expected == "(null)")
            {
                expected = null;
            }
            actual.Should().Be(expected);
        }

        [Then(@"the (Session|Async) Storage Value '(.*)' should be '(.*)'")]
        public void ThenTheSessionStorageValueShouldBe(string sessionOrAsync, string name, string expected)
        {
            var field = typeof(WellKnownStorageValues).GetField(name);
            var type = field.FieldType;
            var typedExpectedValue = GetValueAsType(name, expected, type);

            var storage = scenarioContext.Get<IInstrumentationPluginStorage>(sessionOrAsync);
            var actual = storage.Get(name);
            actual.Should().Be(typedExpectedValue);
        }

        [When(@"actions occur outside the scope of any (form|web method)")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void WhenActionsOccurOutsideTheScopeOfAnyFormOrWebMethod(string ignore)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
            entryPointStorage.CurrentContext = Instrumentation.Current.Context.ApplicationEntryPoint.EntryPointName;
        }

        [When(@"actions occur within the (form|web method) '(.*)'")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void WhenActionsOccurWithinTheFormOrWebMethod(string ignore, string formOrWebMethodName)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
            entryPointStorage.CurrentContext = formOrWebMethodName;
        }

        [Given(@"I enter a session")]
        [When(@"I enter a session")]
        public void WhenIEnterASession()
        {
            Instrumentation.Current.Context.EnterSession();
        }

        [Then(@"the value of (.*) should (not )?be '(.*)'")]
        public void WhenIEvaluate(string name, string isNot, string value)
        {
            name.ArgumentNotNull(nameof(name));
            var split = name.Split('.');
            PropertyInfo propertyInfo;
            object retval;
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
            }
            ThenTheReturnValueShouldBe(isNot, value, retval);
        }

        [When(@"I identify the user as '(.*)'")]
        public void WhenIIdentifyTheUserAs(string userIdentity)
        {
            Instrumentation.Current.Context.IdentifyUser(userIdentity, internalUserData);
        }

        [When(@"I set a session data value of '(.*)' to '(.*)'")]
        public void WhenISetASessionValueOfTo(string name, string value)
        {
            Instrumentation.Current.Context.SetSessionData(name, value);
        }

        [When(@"I set the (Session|Async) Storage Value '(.*)' to '(.*)'")]
        public void WhenISetTheSessionOrAsyncStorageValueTo(string sessionOrAsync, string name, string value)
        {
            var field = typeof(WellKnownStorageValues).GetField(name);
            var type = field.FieldType;
            var typedObjectValue = GetValueAsType(name, value, type);

            var storage = scenarioContext.Get<IInstrumentationPluginStorage>(sessionOrAsync);
            storage.Set(name, typedObjectValue);
        }

        [When(@"the (form is closed|web method completes)")]
        public void WhenTheFormClosesOrTheWebMethodCompletes(string formOrWeb)
        {
            var entryPointStorage = scenarioContext.Get<TestEntryPointStorage>();
            CloseForOrCompleteMethod(formOrWeb, entryPointStorage.CurrentContext, scenarioContext);
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
                    var formInstance = (MyFormBase)Activator.CreateInstance(type);
                    formInstance.OnCreateControl();
                    formOrMethods[formName] = formInstance;
                    break;
                case "web":
                    var mockWebApp = scenarioContext.Get<MockWebApp>();
                    if (mockWebApp == null)
                    {
                        mockWebApp = new MockWebApp();
                        scenarioContext.Set(mockWebApp);
                    }
                    method = MockWebApp.WebMethods[formName];
                    formOrMethods[formName] = method;
                    break;
                default:
                    throw new InvalidOperationException($"Expecting form or web but got {formOrMethod}");
            }
        }

        private static object GetValueAsType(string name, string value, Type type)
        {
            var pair = new KeyValuePair<string, string>(name, value);
            var holdTable = new Table("Field", "Value");
            holdTable.AddRow(name, value);
            var row = holdTable.Rows[0];
            var retriever = Service.Instance.GetValueRetrieverFor(row, type, type);
            return retriever.Retrieve(pair, type, type);
        }

        public class WellKnownStorageValues
        {
            public string ExecutingApplicationName;
            public string Location;
            public string RootApplicationName;
            public Guid SessionId;
        }
    }
}
