using System;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.Steps
{
   using FluentAssertions;

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
        
        [When(@"I evaluate Instrumentation.IsBootstraped")]
        public void WhenICallIsBootstraped()
        {
            throw new NotImplementedException();
           //_scenarioContext.Set(Instrumentation.IsBootstrapped, "return");
        }
        
        [Then(@"the return value should be '(.*)'")]
        public void ThenTheReturnValueShouldBe(string value)
        {
           _scenarioContext.Get<bool>("return").Should().BeFalse();
        }
    }
}
