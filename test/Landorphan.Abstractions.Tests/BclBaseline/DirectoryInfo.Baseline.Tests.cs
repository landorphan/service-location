namespace Landorphan.Abstractions.Tests.BclBaseline
{
   using System.Diagnostics;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DirectoryInfo_Baseline_Tests
   {
      // These tests document what is:  test failures means an implementation detail has changed
      // change the assertion to document "what is"
      // if you believe the behavior to be incorrect, modify the behavior of the abstraction, fix the abstraction tests, and update these documentation tests
      // to show "what is"

      // Currently there is no abstraction for DirectoryInfo
      [TestClass]
      public class Issues_To_Address : TestBase
      {
         [TestMethod]
         [TestCategory(WellKnownTestCategories.ProofOfWorkaroundNeeded)]
         [TestCategory(TestTiming.Manual)]
         [Ignore("documents behavior, does not test code")]
         public void It_does_not_normalize_trailing_directory_separator_chars()
         {
            if (TestHardCodes.WindowsTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsTestPaths.MappedDrive)}");
               return;
            }

            // Windows behavior shown here:

            // NOTE: failed with MSTestRunner 2015.03.29
            // TODO: consider an abstraction around DirectoryInfo

            var driveColon = TestHardCodes.WindowsTestPaths.MappedDrive.Substring(0, 2);

            var a = new DirectoryInfo(driveColon + @"\temp");
            var b = new DirectoryInfo(driveColon + @"\temp\");
            var c = new DirectoryInfo(driveColon + @"/temp");
            var d = new DirectoryInfo(driveColon + @"/temp/");

            Trace.WriteLine(a.FullName); // c:\temp 
            Trace.WriteLine(b.FullName); // c:\temp\
            Trace.WriteLine(c.FullName); // c:\temp
            Trace.WriteLine(d.FullName); // c:\temp\

            // Reference equality, not equivalence.
            b.FullName.Should().Be(a.FullName + @"\");
            b.Should().NotBe(a);

            d.FullName.Should().NotBe(c.FullName + @"/");
            d.FullName.Should().Be(c.FullName + @"\");
            d.Should().NotBe(c);

            // DirectoryInfo does not support value semantics
            a.Should().NotBe(c);
            b.Should().NotBe(d);
         }
      }
   }
}
