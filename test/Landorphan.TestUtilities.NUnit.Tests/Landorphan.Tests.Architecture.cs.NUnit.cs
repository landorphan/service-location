namespace Landorphan.Tests.Architecture
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.ReusableTestImplementations;
    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class Ruleset_Tests : RulesetRequirements
    {
        [Test]
        [Category(TestTiming.IdeOnly)]
        public void Ruleset_should_not_have_duplicate_rule_ids()
        {
            var ruleSets = new List<KeyValuePair<string, Assembly>>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic)
                {
                    var embeddedResourcesNames = assembly.GetManifestResourceNames();
                    foreach (var embeddedResourceName in embeddedResourcesNames)
                    {
                        if (embeddedResourceName.EndsWith(".ruleset", StringComparison.OrdinalIgnoreCase))
                        {
                            ruleSets.Add(new KeyValuePair<string, Assembly>(embeddedResourceName, assembly));
                        }
                    }
                }
            }

#pragma warning disable S3902
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#pragma warning restore S3902
            foreach (var ruleSetInfo in ruleSets)
            {
                var newRuleSetPath = Path.Combine(currentDirectory, ruleSetInfo.Key);
                using (var streamWriter = new StreamWriter(new FileStream(newRuleSetPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)))
                using (var resourceReader = new StreamReader(ruleSetInfo.Value.GetManifestResourceStream(ruleSetInfo.Key)))
                {
                    streamWriter.Write(resourceReader.ReadToEnd());
                }

                Rulesets_should_not_have_duplicate_rule_ids_implementation(newRuleSetPath);
            }
        }
    }
}
