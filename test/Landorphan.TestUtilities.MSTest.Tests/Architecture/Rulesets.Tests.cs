namespace Landorphan.TestUtilities.MSTest.Tests.Architecture
{
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
   [TestClass]
   public class Ruleset_Tests : RulesetRequirements
   {
      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void TestUtilities_ruleset_should_not_have_duplicate_rule_ids()
      {
         var ruleset = @"../../../../source/Landorphan.TestUtilities.MSTest/Landorphan.TestUtilities.MSTest.NetStd.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(ruleset);
      }
   }
}
