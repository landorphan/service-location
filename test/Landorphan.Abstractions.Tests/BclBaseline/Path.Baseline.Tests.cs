﻿namespace Landorphan.Abstractions.Tests.BclBaseline
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Common.Exceptions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable StringLiteralTypo

   public static class Path_Baseline_Tests
   {
      // These tests document what is:  test failures means an implementation detail has changed
      // change the assertion to document "what is"
      // if you believe the behavior to be incorrect, modify the behavior of the abstraction, fix the abstraction tests, and update these documentation tests
      // to show "what is"

      private const String Spaces = "   ";
      // this name 'util' makes the parallel function calls line up
      private static readonly IPathUtilities util = IocServiceLocator.Resolve<IPathUtilities>();
      // private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();

      [TestClass]
      public class Path_BCL_Fixed_Issues : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [TestCategory(WellKnownTestCategories.ProofOfWorkaroundNeeded)]
         public void GetParentPath_on_a_UNC_path_should_NOT_return_null_unless_it_is_a_root()
         {
            // ReSharper disable CommentTypo
            //
            // \\share                 >> null
            // \\share\file.txt        >> null  *** this is a confusing design choice
            // \\share\folder\file.txt >> \\share\folder
            // C:\                     >> null
            // C:\file.txt             >> C:\
            // C:\folder\file.txt      >> C:\folder
            //
            // ReSharper restore CommentTypo
            const String uncPathShareFile = @"\\share\file.txt";
            const String uncPathShare = @"\\share";

            // Proof of fix:
            util.GetParentPath(uncPathShareFile).Should().Be(uncPathShare);

            // Proof of workaround needed:
            Path.GetDirectoryName(uncPathShareFile).Should().BeNull();

            // no workaround needed:
            Path.GetDirectoryName(uncPathShare).Should().BeNull();
            util.GetParentPath(uncPathShare).Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [TestCategory(WellKnownTestCategories.ProofOfWorkaroundNeeded)]
         public void GetRootPath_should_return_the_root_of_a_unc_path()
         {
            const String uncPathShareFile = @"\\share\file.txt";
            const String uncPathShare = @"\\share";

            // Proof of fix:
            util.GetRootPath(uncPathShareFile).Should().Be(uncPathShare);

            // Proof of workaround needed:
            Path.GetPathRoot(uncPathShareFile).Should().Be(uncPathShareFile);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [TestCategory(WellKnownTestCategories.ProofOfWorkaroundNeeded)]
         public void InvalidFileNameChars_in_extension_argument_to_ChangeExtension_Fixed()
         {
            const String legalPath = @"temp.txt";
            const String illegalExtension = @".<";

            // Proof of fix:
            Action throwingAction = () => util.ChangeExtension(legalPath, illegalExtension);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.Message.Should().Contain("The extension is not well-formed (invalid characters).");
            e.And.ParamName.Should().Be("extension");

            // Proof of workaround needed
            // This threw an ArgumentException with different text and a null ParamName value in .Net 4.6.1
            var actual = Path.ChangeExtension(legalPath, illegalExtension);
            actual.Should().Be(@"temp.<");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [TestCategory(WellKnownTestCategories.ProofOfWorkaroundNeeded)]
         public void InvalidPathChars_in_path_argument_to_ChangeExtension_Fixed()
         {
            const String IllegalPath = @"|";
            const String legalExtension = @".txt";

            // Proof of fix:
            Action throwingAction = () => util.ChangeExtension(IllegalPath, legalExtension);
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("path");
            e.And.Message.Should().Contain("The path is not well-formed (invalid characters).");

            // Proof of workaround needed
            // This threw an ArgumentException with different text and a null ParamName value in .Net 4.6.1
            var actual = Path.ChangeExtension(IllegalPath, legalExtension);
            actual.Should().Be("|.txt");
         }
      }

      [TestClass]
      public class Path_BCL_Non_Issues : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Path_ChangeExtension_Behavior()
         {
            // null propagation:
            Path.ChangeExtension(null, null).Should().Be(null);
            Path.ChangeExtension(null, "txt").Should().Be(null);
            Path.ChangeExtension(null, ".txt").Should().Be(null);
            util.ChangeExtension(null, null).Should().Be(null);
            util.ChangeExtension(null, "txt").Should().Be(null);
            util.ChangeExtension(null, ".txt").Should().Be(null);

            // string.Empty propagation:
            Path.ChangeExtension(String.Empty, null).Should().Be(String.Empty);
            Path.ChangeExtension(String.Empty, "txt").Should().Be(String.Empty);
            Path.ChangeExtension(String.Empty, ".txt").Should().Be(String.Empty);
            util.ChangeExtension(String.Empty, null).Should().Be(String.Empty);
            util.ChangeExtension(String.Empty, "txt").Should().Be(String.Empty);
            util.ChangeExtension(String.Empty, ".txt").Should().Be(String.Empty);

            // leading '.' is not required in change extension
            Path.ChangeExtension(@"temp.txt", @"tmp").Should().Be(@"temp.tmp");
            Path.ChangeExtension(@"temp.txt", @".tmp").Should().Be(@"temp.tmp");
            util.ChangeExtension(@"temp.txt", @"tmp").Should().Be(@"temp.tmp");
            util.ChangeExtension(@"temp.txt", @".tmp").Should().Be(@"temp.tmp");

            // spaces in extension are allowed
            Path.ChangeExtension(@"temp.txt", @"   tmp").Should().Be(@"temp.   tmp");
            Path.ChangeExtension(@"temp.txt", @"   tmp").Should().Be(@"temp.   tmp");
            Path.ChangeExtension(@"temp.txt", @"   .tmp").Should().Be(@"temp.   .tmp"); // edge case
            Path.ChangeExtension(@"temp.txt", @".   tmp").Should().Be(@"temp.   tmp");
            util.ChangeExtension(@"temp.txt", @"   tmp").Should().Be(@"temp.   tmp");
            util.ChangeExtension(@"temp.txt", @"   tmp").Should().Be(@"temp.   tmp");
            util.ChangeExtension(@"temp.txt", @"   .tmp").Should().Be(@"temp.   .tmp"); // edge case
            util.ChangeExtension(@"temp.txt", @".   tmp").Should().Be(@"temp.   tmp");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Path_Combine_Behavior()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // usually c:\
            var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;

            // method handles directory separator character insertion
            Path.Combine(drive + @"temp", "temp.tmp").Should().Be(drive + @"temp\temp.tmp");
            util.Combine(drive + @"temp", "temp.tmp").Should().Be(drive + @"temp\temp.tmp");

            Path.Combine(drive + @"temp\", "temp.tmp").Should().Be(drive + @"temp\temp.tmp");
            util.Combine(drive + @"temp\", "temp.tmp").Should().Be(drive + @"temp\temp.tmp");

            // string empty is ignored
            Path.Combine(String.Empty).Should().Be(String.Empty);
            util.Combine(String.Empty).Should().Be(String.Empty);

            Path.Combine(String.Empty, String.Empty).Should().Be(String.Empty);
            util.Combine(String.Empty, String.Empty).Should().Be(String.Empty);

            // directory separator characters are collapsed by Path, not by PathUtilities (primary)
            //  Path.Combine(@"\", @"\") returns @"\";
            //  PUtilities.Combine(@"\", @"\") returns @"\\":  it does as asked.
            var alternate = Path.DirectorySeparatorChar.ToString();
            var primary = Path.DirectorySeparatorChar.ToString();
            var alternate2 = alternate + alternate;
            var alternate2Primary = alternate + alternate + primary;
            var alternate3 = alternate + alternate + alternate;
            var alternatePrimary = alternate + primary;
            var alternatePrimary2 = alternate + primary + primary;
            var alternatePrimaryAlternate = alternate + primary + alternate;
            var primary2 = primary + primary;
            var primary2Alternate = primary + primary + alternate;
            var primary3 = primary + primary + primary;
            var primaryAlternate = primary + alternate;
            var primaryAlternate2 = primary + alternate + alternate;
            var primaryAlternatePrimary = primary + alternate + primary;

            Path.Combine(primary).Should().Be(primary);
            util.Combine(primary).Should().Be(primary);

            Path.Combine(primary, primary).Should().Be(primary);
            util.Combine(primary, primary).Should().Be(primary2);

            Path.Combine(String.Empty, primary).Should().Be(primary);
            util.Combine(String.Empty, primary).Should().Be(primary);

            Path.Combine(primary, String.Empty).Should().Be(primary);
            util.Combine(primary, String.Empty).Should().Be(primary);

            Path.Combine(String.Empty, primary, String.Empty).Should().Be(primary);
            util.Combine(String.Empty, primary, String.Empty).Should().Be(primary);

            Path.Combine(primary, String.Empty, primary).Should().Be(primary);
            util.Combine(primary, String.Empty, primary).Should().Be(primary2);

            // directory separator characters are collapsed by Path, not by PathUtilities (alternate)
            //  Path.Combine(alternate, alternate) returns alternate;
            //  PUtilities.Combine(alternate, alternate) returns @"//":  it does as asked.
            Path.Combine(alternate).Should().Be(alternate);
            util.Combine(alternate).Should().Be(alternate);

            Path.Combine(alternate, alternate).Should().Be(alternate);
            util.Combine(alternate, alternate).Should().Be(alternate2);

            Path.Combine(String.Empty, alternate).Should().Be(alternate);
            util.Combine(String.Empty, alternate).Should().Be(alternate);

            Path.Combine(alternate, String.Empty).Should().Be(alternate);
            util.Combine(alternate, String.Empty).Should().Be(alternate);

            Path.Combine(String.Empty, alternate, String.Empty).Should().Be(alternate);
            util.Combine(String.Empty, alternate, String.Empty).Should().Be(alternate);

            Path.Combine(alternate, String.Empty, alternate).Should().Be(alternate);
            util.Combine(alternate, String.Empty, alternate).Should().Be(alternate2);

            // directory separator characters are collapsed by Path "right-most" wins
            // not by PathUtilities, which does as asked.
            Path.Combine(primary, alternate).Should().Be(alternate);
            util.Combine(primary, alternate).Should().Be(primaryAlternate);

            Path.Combine(alternate, String.Empty, primary).Should().Be(primary);
            util.Combine(alternate, String.Empty, primary).Should().Be(alternatePrimary);

            Path.Combine(primary, String.Empty, alternate).Should().Be(alternate);
            util.Combine(primary, String.Empty, alternate).Should().Be(primaryAlternate);

            Path.Combine(primary, String.Empty, alternate, String.Empty, alternate).Should().Be(alternate);
            util.Combine(primary, String.Empty, alternate, String.Empty, alternate).Should().Be(primaryAlternate2);

            Path.Combine(primary, String.Empty, primary, String.Empty, alternate).Should().Be(alternate);
            util.Combine(primary, String.Empty, primary, String.Empty, alternate).Should().Be(primary2Alternate);

            Path.Combine(alternate, String.Empty, primary, String.Empty, primary).Should().Be(primary);
            util.Combine(alternate, String.Empty, primary, String.Empty, primary).Should().Be(alternatePrimary2);

            Path.Combine(alternate, String.Empty, alternate, String.Empty, primary).Should().Be(primary);
            util.Combine(alternate, String.Empty, alternate, String.Empty, primary).Should().Be(alternate2Primary);

            // spaces are not *entirely* ignored by Path
            // PathUtilities does as asked, and ignoring whitespace on the boundaries of separator characters
            Path.Combine(Spaces, primary).Should().Be(primary);
            util.Combine(Spaces, primary).Should().Be(primary);

            Path.Combine(primary, Spaces).Should().Be(primary + Spaces);
            util.Combine(primary, Spaces).Should().Be(primary);

            Path.Combine(Spaces, primary, Spaces).Should().Be(primary + Spaces);
            util.Combine(Spaces, primary, Spaces).Should().Be(primary);

            Path.Combine(alternate, Spaces).Should().Be(alternate + Spaces);
            util.Combine(alternate, Spaces).Should().Be(alternate);

            Path.Combine(Spaces, alternate, Spaces).Should().Be(alternate + Spaces);
            util.Combine(Spaces, alternate, Spaces).Should().Be(alternate);

            Path.Combine(primary, Spaces, primary).Should().Be(primary);
            util.Combine(primary, Spaces, primary).Should().Be(primary2);

            Path.Combine(Spaces, alternate).Should().Be(alternate);
            util.Combine(Spaces, alternate).Should().Be(alternate);

            Path.Combine(alternate, Spaces, alternate).Should().Be(alternate);
            util.Combine(alternate, Spaces, alternate).Should().Be(alternate2);

            Path.Combine(Spaces, primary).Should().Be(primary);
            util.Combine(Spaces, primary).Should().Be(primary);

            Path.Combine(alternate, String.Empty, alternate, String.Empty, alternate).Should().Be(alternate);
            util.Combine(alternate, String.Empty, alternate, String.Empty, alternate).Should().Be(alternate3);

            Path.Combine(primary, String.Empty, primary, String.Empty, primary).Should().Be(primary);
            util.Combine(primary, String.Empty, primary, String.Empty, primary).Should().Be(primary3);

            Path.Combine(primary, String.Empty, alternate, String.Empty, primary).Should().Be(primary);
            util.Combine(primary, String.Empty, alternate, String.Empty, primary).Should().Be(primaryAlternatePrimary);

            Path.Combine(alternate, String.Empty, primary, String.Empty, alternate).Should().Be(alternate);
            util.Combine(alternate, String.Empty, primary, String.Empty, alternate).Should().Be(alternatePrimaryAlternate);

            Path.Combine(alternate, Spaces, alternate).Should().Be(alternate);
            util.Combine(alternate, Spaces, alternate).Should().Be(alternate2);

            Path.Combine(alternate, Spaces, primary).Should().Be(primary);
            util.Combine(alternate, Spaces, primary).Should().Be(alternatePrimary);

            Path.Combine(primary, Spaces, alternate).Should().Be(alternate);
            util.Combine(primary, Spaces, alternate).Should().Be(primaryAlternate);

            Path.Combine(primary, Spaces, alternate, Spaces, alternate).Should().Be(alternate);
            util.Combine(primary, Spaces, alternate, Spaces, alternate).Should().Be(primaryAlternate2);

            Path.Combine(primary, Spaces, primary, Spaces, alternate).Should().Be(alternate);
            util.Combine(primary, Spaces, primary, Spaces, alternate).Should().Be(primary2Alternate);

            Path.Combine(alternate, Spaces, primary, Spaces, primary).Should().Be(primary);
            util.Combine(alternate, Spaces, primary, Spaces, primary).Should().Be(alternatePrimary2);

            Path.Combine(alternate, Spaces, alternate, Spaces, primary).Should().Be(primary);
            util.Combine(alternate, Spaces, alternate, Spaces, primary).Should().Be(alternate2Primary);

            // drive letter rooted paths (behaviors differ)
            Path.Combine(@"Z").Should().Be(@"Z"); // IMO: BUG
            util.Combine(@"Z").Should().Be(@"Z\");

            Path.Combine(@"Z:").Should().Be(@"Z:");
            util.Combine(@"Z:").Should().Be(@"Z:");

            Path.Combine(@"Z:\").Should().Be(@"Z:\");
            util.Combine(@"Z:\").Should().Be(@"Z:\");

            Path.Combine(@"Z\:").Should().Be(@"Z\:");
            util.Combine(@"Z\:").Should().Be(@"Z\:");

            Path.Combine(@"Z/:").Should().Be(@"Z/:");
            util.Combine(@"Z/:").Should().Be(@"Z/:");

            // .Net Standard inserts '\' into the result, .Net Framework 4.6.1 did not
            Path.Combine(@"Z:", "temp.txt").Should().Be(@"Z:\temp.txt");
            util.Combine(@"Z:", "temp.txt").Should().Be(@"Z:temp.txt"); // TODO: is this correct?

            Path.Combine(@"Z:\", "temp.txt").Should().Be(@"Z:\temp.txt");
            util.Combine(@"Z:\", "temp.txt").Should().Be(@"Z:\temp.txt");

            Path.Combine(@"Z:/", "temp.txt").Should().Be(@"Z:/temp.txt");
            util.Combine(@"Z:/", "temp.txt").Should().Be(@"Z:/temp.txt");

            // .Net Standard:          Z\:\temp.txt
            // .Net Framework 4.6.1:   Z\:temp.txt
            Path.Combine(@"Z\:", "temp.txt").Should().Be(@"Z\:\temp.txt");
            util.Combine(@"Z\:", "temp.txt").Should().Be(@"Z\:\temp.txt"); // TODO: is this correct?

            // .Net Standard:          Z/:\temp.txt
            // .Net Framework 4.6.1:   Z/:temp.txt
            Path.Combine(@"Z/:", "temp.txt").Should().Be(@"Z/:\temp.txt");
            util.Combine(@"Z/:", "temp.txt").Should().Be(@"Z/:\temp.txt"); // TODO: is this correct?

            // driver letter rooted paths with spaces (behaviors differ)
            Path.Combine(@"c:" + Spaces, Spaces, primary, "temp.tmp").Should().Be(@"\temp.tmp"); // IMO bug (a likely current directory root)
            util.Combine(@"c:" + Spaces, Spaces, primary, "temp.tmp").Should().Be(@"c:\temp.tmp");

            Path.Combine(@"c:\" + Spaces, Spaces, "temp.tmp").Should().Be(@"c:\" + Spaces + primary + Spaces + @"\temp.tmp");
            util.Combine(@"c:\" + Spaces, Spaces, "temp.tmp").Should().Be(@"c:\temp.tmp"); // TODO: is this correct?

            Path.Combine(@"c\:" + Spaces, Spaces, "temp.tmp").Should().Be(@"c\:" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug (a likely current directory root)
            util.Combine(@"c\:" + Spaces, Spaces, "temp.tmp").Should().Be(@"c\:\temp.tmp"); // TODO: is this correct?

            Path.Combine(@"Z:" + Spaces, Spaces, primary, "temp.tmp").Should().Be(@"\temp.tmp"); // IMO bug on user error (a likely current directory root)
            util.Combine(@"Z:" + Spaces, Spaces, primary, "temp.tmp").Should().Be(@"Z:\temp.tmp");

            Path.Combine(@"Z:\" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z:\" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug on user error (a likely current directory root)
            util.Combine(@"Z:\" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z:\temp.tmp");

            Path.Combine(@"Z\:" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z\:" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug (a likely current directory root)
            util.Combine(@"Z\:" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z\:\temp.tmp"); // TODO: is this correct?

            Path.Combine(@"c:" + Spaces, Spaces, alternate, "temp.tmp").Should().Be(@"\temp.tmp");
            util.Combine(@"c:" + Spaces, Spaces, alternate, "temp.tmp").Should().Be(@"c:\temp.tmp");

            Path.Combine(@"c:/" + Spaces, Spaces, "temp.tmp").Should().Be(@"c:/" + Spaces + primary + Spaces + @"\temp.tmp");
            util.Combine(@"c:/" + Spaces, Spaces, "temp.tmp").Should().Be(@"c:/temp.tmp");

            Path.Combine(@"c/:" + Spaces, Spaces, "temp.tmp").Should().Be(@"c/:" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug (a likely current directory root)
            util.Combine(@"c/:" + Spaces, Spaces, "temp.tmp").Should().Be(@"c/:\temp.tmp"); // TODO: is this correct?

            Path.Combine(@"Z:" + Spaces, Spaces, alternate, "temp.tmp").Should().Be(@"\temp.tmp"); // IMO bug on user error (a likely current directory root)
            util.Combine(@"Z:" + Spaces, Spaces, alternate, "temp.tmp").Should().Be(@"Z:\temp.tmp");

            Path.Combine(@"Z:/" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z:/" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug on user error (a likely current directory root)
            util.Combine(@"Z:/" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z:/temp.tmp");

            Path.Combine(@"Z/:" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z/:" + Spaces + primary + Spaces + @"\temp.tmp"); // IMO bug on user error (a likely current directory root)
            util.Combine(@"Z/:" + Spaces, Spaces, "temp.tmp").Should().Be(@"Z/:\temp.tmp"); // TODO: is this correct?

            // null values throw
            // slight difference between Path and PathInternalMapping
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Action throwingAction = () => Path.Combine(null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("paths");
            throwingAction = () => util.Combine(null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("paths");

            throwingAction = () => Path.Combine(null, null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("path1");
            throwingAction = () => util.Combine(null, null);
            throwingAction.Should().Throw<ArgumentContainsNullException>().And.ParamName.Should().Be("paths");

            throwingAction = () => Path.Combine(null, null, null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("path1");
            throwingAction = () => util.Combine(null, null, null);
            throwingAction.Should().Throw<ArgumentContainsNullException>().And.ParamName.Should().Be("paths");

            throwingAction = () => Path.Combine(null, null, null, null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("path1");
            throwingAction = () => util.Combine(null, null, null, null);
            throwingAction.Should().Throw<ArgumentContainsNullException>().And.ParamName.Should().Be("paths");

            throwingAction = () => Path.Combine(null, null, null, null, null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("paths");
            throwingAction = () => util.Combine(null, null, null, null, null);
            throwingAction.Should().Throw<ArgumentContainsNullException>().And.ParamName.Should().Be("paths");

            throwingAction = () => Path.Combine("a", "b", "c", "d", null);
            throwingAction.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("paths");
            throwingAction = () => util.Combine("a", "b", "c", "d", null);
            throwingAction.Should().Throw<ArgumentContainsNullException>().And.ParamName.Should().Be("paths");
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Path_GetInvalidFileNameChars_Windows_Behavior()
         {
            // Path: GetInvalidFileNameChars
            // Util: GetInvalidFileNameCharacters 
            var actual = Path.GetInvalidFileNameChars();
            var expected = new HashSet<Char>(
               new[]
               {
                  '"', '<', '>', '|', Char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011',
                  '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', ':', '*', '?', '\\', '/'
               });

            actual.Should().OnlyContain(element => expected.Contains(element));
            util.GetInvalidFileNameCharacters().Should().OnlyContain(element => expected.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void Path_GetInvalidPathChars_Windows_Behavior()
         {
            // Path: GetInvalidPathChars
            // Util: GetInvalidPathCharacters 
            var actual = Path.GetInvalidPathChars();
            var expected = new HashSet<Char>(
               new[]
               {
                  '|', Char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013',
                  '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F'
               });

            actual.Should().OnlyContain(element => expected.Contains(element));
            util.GetInvalidPathCharacters().Should().OnlyContain(element => expected.Contains(element));
         }
      }
   }
}
