namespace Landorphan.TestUtilities.ReusableTestImplementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using FluentAssertions;
    using Landorphan.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test implementations for architectural requirements.
    /// </summary>
    public abstract class ArchitecturalRequirements : TestBase
    {
        /// <summary>
        /// Asserts all declarations not under an internal or resource namespace leaf node are either public, or nested.
        /// </summary>
        [TestCategory(TestTiming.CheckIn)]
        [SuppressMessage("SonarLint.CodeSmell", "S1067:Expressions should not be too complex", Justification = "Test Code addressing a general problem (MWP)")]
        [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high", Justification = "Test Code addressing a general problem (MWP)")]
        [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
        [SuppressMessage("Microsoft.Globalization", "CA1307: Specify StringComparison", Justification = "Not available in .Net Standard or .Net Framework")]
        [SuppressMessage("SonarLint.CodeSmell", "S4058: Overloads with a StringComparison parameter should be used", Justification = "Not available in .Net Standard or .Net Framework")]
        public void All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested_Implementation()
        {
            var assemblies = GetAssembliesUnderTest();
            assemblies.Count.Should().BeGreaterThan(0);

            var violatingTypes = new HashSet<Type>();

            foreach (var asm in assemblies)
            {
                var allAssemblyTypes = asm.SafeGetTypes();
                foreach (var type in allAssemblyTypes)
                {
                    var namespaceName = type.Namespace ?? string.Empty;
                    if (namespaceName.Length == 0)
                    {
                        // skip anonymous types.
                        continue;
                    }

                    if (string.Equals(namespaceName, "JetBrains.Profiler.Core.Instrumentation", StringComparison.Ordinal) &&
                        string.Equals(type.Name, "DataOnStack", StringComparison.Ordinal))
                    {
                        // appears when performing test coverage using dotCover.
                        continue;
                    }

                    // No StringComparison argument in .Net Standard
                    if (!namespaceName.Contains(".Internal") &&
                        !namespaceName.Contains(".Resources") &&
                        !namespaceName.Contains(".TestFacilities") &&
                        !type.IsPublic &&
                        !type.IsNestedPublic &&
                        !type.IsNested)

                    {
                        violatingTypes.Add(type);
                    }
                }
            }

            violatingTypes.Should().BeEmpty();
        }

        /// <summary>
        /// Evaluates all declarations under an internal namespace.
        /// </summary>
        [TestCategory(TestTiming.CheckIn)]
        [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
        [SuppressMessage("Microsoft.Globalization", "CA1307: Specify StringComparison", Justification = "Not available in .Net Standard or .Net Framework")]
        [SuppressMessage("SonarLint.CodeSmell", "S4058: Overloads with a StringComparison parameter should be used", Justification = "Not available in .Net Standard or .Net Framework")]
        public void All_Declarations_Under_An_Internal_Namespace_Are_Not_Public_Implementation()
        {
            var assemblies = GetAssembliesUnderTest();
            assemblies.Count.Should().BeGreaterThan(0);

            var violatingTypes = new HashSet<Type>();

            foreach (var asm in assemblies)
            {
                var allAssemblyTypes = asm.SafeGetTypes();
                foreach (var type in allAssemblyTypes)
                {
                    var namespaceName = type.Namespace ?? string.Empty;
                    // No StringComparison argument available in .Net Standard
                    if (namespaceName.Contains(".Internal") && (type.IsPublic || type.IsNestedPublic))
                    {
                        violatingTypes.Add(type);
                    }
                }
            }

            violatingTypes.Should().BeEmpty();
        }

        /// <summary>
        /// Gets the assemblies to be evaluated.
        /// </summary>
        /// <returns>
        /// The assemblies to be evaluated.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected abstract IImmutableSet<Assembly> GetAssembliesUnderTest();
    }
}
