// ReSharper disable once CheckNamespace
namespace Landorphan.TestUtilities.ReusableTestImplementations
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Linq;
   using System.Xml.Linq;
   using FluentAssertions;
   using Landorphan.Common;

   // ReSharper disable StringLiteralTypo
   // ReSharper disable InconsistentNaming

   /// <summary>
   /// Test implementations for static analyzer ruleset files.
   /// </summary>
   public class RulesetRequirements : TestBase
   {
      // there is no such thing as .Net Standard Test project

      /// <summary>
      /// Asserts the default production ruleset for .Net Core analyzers contains no duplicate rule id values.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Default_Production_Ruleset_NetCore_should_not_have_duplicate_rule_ids_implementation()
      {
         var rulesetPath = "..\\..\\..\\..\\build\\BuildFiles\\Default.Production.NetCore.FxCop.15.0.WithSonarLint.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(rulesetPath);
      }

      /// <summary>
      /// Asserts the default production ruleset for .Net Framework analyzers contains no duplicate rule id values.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Default_Production_Ruleset_NetFx_should_not_have_duplicate_rule_ids_implementation()
      {
         var rulesetPath = "..\\..\\..\\..\\build\\BuildFiles\\Default.Production.NetFx.FxCop.15.0.WithSonarLint.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(rulesetPath);
      }

      /// <summary>
      /// Asserts the default production ruleset for .Net Standard analyzers contains no duplicate rule id values.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Default_Production_Ruleset_NetStd_should_not_have_duplicate_rule_ids_implementation()
      {
         var rulesetPath = "..\\..\\..\\..\\build\\BuildFiles\\Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(rulesetPath);
      }

      /// <summary>
      /// Asserts the default test ruleset for .Net Core analyzers contains no duplicate rule id values.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Default_Test_Ruleset_NetCore_should_not_have_duplicate_rule_ids_implementation()
      {
         var rulesetPath = "..\\..\\..\\..\\build\\BuildFiles\\Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(rulesetPath);
      }

      /// <summary>
      /// Asserts the default test ruleset for .Net Framework analyzers contains no duplicate rule id values.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Default_Test_Ruleset_NetFx_should_not_have_duplicate_rule_ids_implementation()
      {
         var rulesetPath = "..\\..\\..\\..\\build\\BuildFiles\\Default.Test.NetFx.FxCop.15.0.WithSonarLint.ruleset";
         Rulesets_should_not_have_duplicate_rule_ids_implementation(rulesetPath);
      }

      /// <summary>
      /// Asserts the given ruleset file contains no duplicate rule id values.
      /// </summary>
      /// <param name="rulesetPath">
      /// The path the the ruleset file to inspect.
      /// </param>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      protected void Rulesets_should_not_have_duplicate_rule_ids_implementation(String rulesetPath)
      {
         rulesetPath.ArgumentNotNull(nameof(rulesetPath));
         if (!File.Exists(rulesetPath))
         {
            throw new FileNotFoundException(null, rulesetPath);
         }

         var ruleIdCountMap = new SortedDictionary<String, Int32>(StringComparer.OrdinalIgnoreCase);

         var doc = XDocument.Load(rulesetPath);
         var ruleSets = doc.Descendants("RuleSet");
         foreach (var ruleSet in ruleSets)
         {
            var ruleCollections = ruleSet.Descendants("Rules");
            foreach (var ruleCollection in ruleCollections)
            {
               var rules = ruleCollection.Descendants("Rule");
               foreach (var rule in rules)
               {
                  var key = rule.Attribute("Id").Value;
                  if (ruleIdCountMap.ContainsKey(key))
                  {
                     ruleIdCountMap[key] += 1;
                  }
                  else
                  {
                     ruleIdCountMap.Add(key, 1);
                  }
               }
            }
         }

         var echoedTestSet = false;

         var duplicates = false;

         var count = (
            from kvp in ruleIdCountMap
            where kvp.Value > 1
            select kvp).Count();
         if (count > 0)
         {
            Trace.WriteLine($"{count} duplicate rule ids");
         }

         foreach (var kvp in ruleIdCountMap)
         {
            if (kvp.Value > 1)
            {
               duplicates = true;
               if (!echoedTestSet)
               {
                  Trace.WriteLine(rulesetPath);
                  echoedTestSet = true;
               }

               Trace.WriteLine(kvp.Key + " has " + kvp.Value + " instances");
            }
         }

         duplicates.Should().BeFalse();
      }
   }
}
