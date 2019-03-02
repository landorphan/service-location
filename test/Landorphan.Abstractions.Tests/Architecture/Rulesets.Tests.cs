namespace Landorphan.Abstractions.Tests.Architecture
{
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class Ruleset_Tests : RulesetRequirements
   {
      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void Abstractions_ruleset_should_not_have_duplicate_rule_ids()
      {
         var ruleset = @"../../../../source/Landorphan.Abstractions/Landorphan.Abstractions.NetStd.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(ruleset);
      }

      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void Abstractions_Tests_ruleset_should_not_have_duplicate_rule_ids()
      {
         var ruleset = @"../../../../test/Landorphan.Abstractions.Tests/Landorphan.Abstractions.Tests.NetCore.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(ruleset);
      }
   }
}
