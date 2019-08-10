using System;
using TechTalk.SpecFlow;

namespace Landorphan.Instrumentation.Tests.Steps
{
    [Binding]
    public class BootstrapSteps
    {
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
            ScenarioContext.Current.Pending();
        }
    }
}
