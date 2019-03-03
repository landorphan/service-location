namespace Landorphan.Ioc.ServiceLocation.Testability.Tests.Architecture
{
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations.Architecture;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class Ruleset_Tests : RulesetRequirements
   {
      [TestMethod]
      [TestCategory(TestTiming.IdeOnly)]
      public void Testability_ruleset_should_not_have_duplicate_rule_ids()
      {
         var ruleset = @"../../../../source/Landorphan.Ioc.ServiceLocation.Testability/Landorphan.Ioc.ServiceLocation.Testability.NetStd.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(ruleset);
      }
   }
}
