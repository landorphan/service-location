using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Landorphan.Abstractions.Tests.StepDefinitions
{
   [Binding]
   public sealed class PathSteps
   {
      [Given(@"I have the following path")]
      public void GivenIHaveTheFollowingPath()
      {
      }

      [When(@"I parse the path as a Windows Path")]
      public void WhenIParseThePathAsAWidnowsPath()
      {
      }

      [Then(@"I should receive a path object")]
      public void ThenIShouldReceiveAPathObject()
      {
      }

      [Then(@"the segment ""(.*)"" should be (.*)")]
      public void ThenTheSegmentShouldBeC(string root, string value)
      {
      }

      [Then(@"the path should be anchored to (.*)")]
      public void ThenThePathShouldBeAnchoredToAbsolute(string ahchor)
      {
      }

      [Then(@"the parse status should be (.*)")]
      public void ThenTheParseStatusShouldBeLegal(string status)
      {
      }


      [Then(@"the segment length should be (.*)")]
      public void ThenTheSegmentLengthShouldBe(int p0)
      {
      }
   }
}
