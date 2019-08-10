using System;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.Steps
{
   using FluentAssertions;

   [Binding]
    public class BootstrapSteps
    {
       private readonly ScenarioContext _scenarioContext;
       private readonly FeatureContext _featureContext;

       public BootstrapSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
       {
          _scenarioContext = scenarioContext;
          _featureContext = featureContext;
       }
      
       [Given(@"I do nothing")]
        public void GivenIDoNothing()
        {
        }
        
        [When(@"I evaluate Instrumentation.IsBootstraped")]
        public void WhenICallIsBootstraped()
        {
           throw new NotImplementedException();
           // _scenarioContext.Set(Instrumentation.IsBootstrapped, "return");
        }
        
        [Then(@"the return value should be '(.*)'")]
        public void ThenTheReturnValueShouldBe(string value)
        {
           _scenarioContext.Get<object>("return").ToString().Should().Be(value);
        }

        [Given(@"I bootstrap Instrumentation")]
        public void GivenIBootstrapInstrumentation()
        {
           // ScenarioContext.Current.Pending();
        }

        [When(@"I evaluate Instrumentation\.Context\.ApplicationName")]
        public void WhenIEvaluateInstrumentation_Context_ApplicationName()
        {
           // ScenarioContext.Current.Pending();
        }
   }
}
