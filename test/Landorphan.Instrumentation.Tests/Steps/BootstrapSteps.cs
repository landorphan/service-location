using System;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.Steps
{
    [Binding]
    public class BootstrapSteps
    {
       ScenarioContext _scenarioContext;
       FeatureContext _featureContext;

       public BootstrapSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
       {
          _scenarioContext = scenarioContext;
          _featureContext = featureContext;
       }

      [Given(@"I do nothing")]
        public void GivenIDoNothing()
        {
        }
        
        [When(@"I call IsBootstraped")]
        public void WhenICallIsBootstraped()
        {
           throw new NotImplementedException();
        }
        
        [Then(@"the return value should be '(.*)'")]
        public void ThenTheReturnValueShouldBe(string value)
        {
           _scenarioContext.Pending();
        }
    }
}
