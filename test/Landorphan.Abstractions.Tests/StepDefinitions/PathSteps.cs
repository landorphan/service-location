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
      PathParser pathParser = new PathParser();
      private string[] tokens;
      private ISegment[] segments;
      public string preParsedPath;
      public IPath parsedPath;
      public IPath originalForm;

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

      [When(@"I segment the (Windows|Linux|OSX) path")]
      public void WhenISegmentThePath(string pathType)
      {
         OSPlatform platform;
         WhenITokenizeThePathWithTheTokenizer(pathType);
         switch (pathType)
         {
            case "Windows":
               platform = OSPlatform.Windows;
               break;
            case "Linux":
               platform = OSPlatform.Linux;
               break;
            case "OSX":
               platform = OSPlatform.OSX;
               break;
         }
         segments = pathParser.GetSegments(tokens, platform);
      }

      [Then(@"segment '(.*)' should be: (.*)")]
      public void ThenSegmentShouldBeNull(int segment, string value)
      {
         ISegment expected = null;
         if (value == "{N} (null)" || segment > segments.Length)
         {
            expected = Segment.NullSegment;
         }
         else if (value == "{E} (empty)")
         {
            expected = Segment.EmptySegment;
         }
         else if (value == "{.} .")
         {
            expected = Segment.SelfSegment;
         }
         else if (value == "{..} ..")
         {
            expected = Segment.ParentSegment;
         }
         else if (value.StartsWith("{U}"))
         {
            expected = new Segment(SegmentType.UncSegment, value.Substring(4));
         }
         else if (value.StartsWith("{R}"))
         {
            expected = new Segment(SegmentType.RootSegment, value.Substring(4));
         }
         else if (value.StartsWith("{D}"))
         {
            expected = new Segment(SegmentType.DeviceSegment, value.Substring(4));
         }
         else if (value.StartsWith("{/}"))
         {
            expected = new Segment(SegmentType.VolumelessRootSegment, value.Substring(4));
         }
         else if (value.StartsWith("{V}"))
         {
            expected = new Segment(SegmentType.VolumeRelativeSegment, value.Substring(4));
         }
         else if (value.StartsWith("{G}"))
         {
            expected = new Segment(SegmentType.GenericSegment, value.Substring(4));
         }

         ISegment actual;
         if (segment >= segments.Length)
         {
            actual = Segment.NullSegment;
         }
         else
         {
            actual = segments[segment];
         }
         actual.SegmentType.Should().Be(expected.SegmentType);
         actual.Name.Should().Be(expected.Name);
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

      [When(@"I evaluate the original form")]
      public void WhenIEvaluteTheNonnormalizedForm()
      {
         this.originalForm = parsedPath.SuppliedPath;
         segments = originalForm.Segments;
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
      public void ThenTheSegmentLengthShouldBe(int expected)
      {
         segments.Length.Should().Be(expected);
      }
   }
}
