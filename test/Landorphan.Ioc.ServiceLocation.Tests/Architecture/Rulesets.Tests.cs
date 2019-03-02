namespace Landorphan.Ioc.Tests.Architecture
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
      public void Ioc_ruleset_should_not_have_duplicate_rule_ids()
      {
         var ruleset = @"../../../../source/Landorphan.Ioc.ServiceLocation/Landorphan.Ioc.ServiceLocation.NetStd.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(ruleset);
      }
   }
}
