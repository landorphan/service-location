using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Landorphan.Abstractions.Tests.StepDefinitions
{
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.NIO.Paths;
   using Landorphan.Abstractions.NIO.Paths.Internal;

   [Binding]
   public sealed class PathSteps
   {
      public string suppliedPath;
      IPathParser pathParser = new PathParser();
      private string[] tokens;
      public string preParsedPath;
      public IPath parsedPath;

      [Given(@"I have the following path: (.*)")]
      public void GivenIHaveTheFollowingPath(string path)
      {
         if (path == "(null)")
         {
            suppliedPath = null;
         }
         else if (path == "(empty)")
         {
            suppliedPath = string.Empty;
         }
         else
         {
            suppliedPath = path.Replace('`', '\\');
         }
      }

      [When(@"I preparse the path")]
      public void WhenIPreparseThePath()
      {
         preParsedPath = WindowsPathTokenizer.PreParsePath(suppliedPath);
      }

      [Then(@"the resulting path should read: (.*)")]
      public void ThenTheResultingPathShouldRead(string expected)
      {
         if (expected == "(null)")
         {
            expected = null;
         }
         else if (expected == "(empty)")
         {
            expected = string.Empty;
         }
         preParsedPath.Should().Be(expected);
      }

      [When(@"I parse the path as a Windows Path")]
      public void WhenIParseThePathAsAWidnowsPath()
      {
         parsedPath = pathParser.Parse(suppliedPath, OSPlatform.Windows);
      }

      [Then(@"I should receive a path object")]
      public void ThenIShouldReceiveAPathObject()
      {
         parsedPath.Should().NotBeNull();
      }

      [When(@"I tokenize the path with the '(.*)' tokenizer")]
      public void WhenITokenizeThePathWithTheTokenizer(string p0)
      {
         if (p0 == "Windows")
         {
            WindowsPathTokenizer tokenizer = new WindowsPathTokenizer(suppliedPath);
            tokens = tokenizer.GetTokens();
         }
      }

      [Then(@"token '(.*)' should be: (.*)")]
      public void ThenSetmentShouldBe(int loc, string expected)
      {
         string actual = null;
         if (loc < tokens.Length)
         {
            actual = tokens[loc];
         }
         if (expected == "(null)")
         {
            expected = null;
         }
         else if (expected == "(empty)")
         {
            expected = string.Empty;
         }
         actual.Should().Be(expected);
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
