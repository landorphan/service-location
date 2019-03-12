using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Landorphan.Abstractions.Tests.StepDefinitions
{
   [Binding]
   public sealed class DoNothingSteps
   {
      [Given(@"I have done nothing")]
      [When(@"I do nothing")]
      [Then(@"nothing should occur")]
      public void GivenIHaveDoneNothing()
      {
      }
   }
}
