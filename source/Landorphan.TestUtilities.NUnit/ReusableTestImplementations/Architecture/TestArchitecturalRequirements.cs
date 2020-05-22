namespace Landorphan.TestUtilities.ReusableTestImplementations
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Landorphan.Common;
    using NUnit.Framework;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Test implementations for test architectural requirements.
    /// </summary>
    /// <remarks>
    /// Only considers public types.  
    /// </remarks>
    public abstract class TestArchitecturalRequirements : TestBase
    {
        /// <summary>
        /// Verifies that all test classes descend from <see cref="TestBase" /> except for those explicitly excluded.
        /// </summary>
        /// <remarks>
        /// Ignores abstract types.  Ignores non-public types.  Ignores types decorated with any of the following: CompilerGeneratedAttribute, GeneratedCodeAttribute (i.e., SpecFlow), IgnoreAttribute.
        /// </remarks>
        [Category(TestTiming.CheckIn)]
        [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
        protected void All_But_Excluded_Tests_Descend_From_TestBase_Implementation()
        {
            var failureMessages = new List<string>();

            var excludedTypes = GetTestTypesNotRequiredToDescendFromTestBase();
            Trace.Assert(excludedTypes != null, "GetTestTypesNotRequiredToDescendFromTestBase() returned null -- which it must not do");

            var testClassTypes = GetAllTestTypesInTestAssemblyExcludeNonPublicAndAbstractAndIgnoredAndGenerated();
            foreach (var testClass in testClassTypes)
            {
                if (!typeof(TestBase).IsAssignableFrom(testClass) && !excludedTypes.Contains(testClass))
                {
                    failureMessages.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "The test class '{0}' is not descended from TestBase but should be.",
                            testClass.Name));
                }
            }

            if (failureMessages.Count > 0)
            {
                throw new AssertionException(string.Join(Environment.NewLine, failureMessages.ToArray()));
            }
        }

        /// <summary>
        /// Verifies that all tests that are not ignored have one and only one timing category.
        /// </summary>
        /// <remarks>
        /// Ignores abstract types. Ignores non-public types.  Ignores types decorated with IgnoreAttribute.
        /// Generated tests such as SpecFlow ARE included.
        /// </remarks>
        [Category(TestTiming.CheckIn)]
        [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
        [SuppressMessage("SonarLint.CodeSmell", "S3776: Control flow statements if, switch, for, foreach, while, do and try should not be nested too deeply")]
        protected void All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category_Implementation()
        {
            var failureMessages = new List<string>();

            var testTimingValues = GetRecognizedTestTimingCategories().ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
            var testClassTypes = GetAllTestTypesInTestAssemblyExcludeNonPublicAndAbstractAndIgnored();
            foreach (var testClass in testClassTypes)
            {
                var allPublicMethodsForType = testClass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                foreach (var m in allPublicMethodsForType)
                {
                    if (m.GetCustomAttributes(typeof(TestAttribute), true).Any() && !m.GetCustomAttributes(typeof(IgnoreAttribute), true).Any())
                    {
                        var testMethod = m.Name;

                        var testCategories = (CategoryAttribute[])m.GetCustomAttributes(typeof(CategoryAttribute), true);
                        var timingMatches = 0;
                        foreach (var c in testCategories)
                        {
                            if (testTimingValues.Contains(c.Name))
                            {
                                timingMatches++;
                            }
                        }

                        switch (timingMatches)
                        {
                            case 1:
                                break;

                            case 0:
                                failureMessages.Add(
                                    string.Format(
                                        CultureInfo.InvariantCulture,
                                        "The test '{0}.{1}' is not decorated with [Category(TestTiming.*)] but should be.",
                                        testClass.Name,
                                        testMethod));
                                break;

                            default:
                                failureMessages.Add(
                                    string.Format(
                                        CultureInfo.InvariantCulture,
                                        "The test '{0}.{1}' is decorated with more than one [Category(TestTiming.*)] but should only have one timing attribute.",
                                        testClass.Name,
                                        testMethod));
                                break;
                        }
                    }
                }
            }

            if (failureMessages.Count > 0)
            {
                throw new AssertionException(string.Join(Environment.NewLine, failureMessages.ToArray()));
            }
        }

        /// <summary>
        /// Searches the given <paramref name="assembly"/> for ignored tests.
        /// </summary>
        /// <param name="assembly">
        /// The assembly to inspect.
        /// </param>
        /// <remarks>
        /// Trace output enumerates the ignored tests, if any.
        /// </remarks>
        [Category(TestTiming.CheckIn)]
        [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
        protected void Find_ignored_tests_in_assembly(Assembly assembly)
        {
            assembly.ArgumentNotNull(nameof(assembly));
            Trace.WriteLine($"Examining {assembly.GetName().Name} looking for ignored test methods...");
            var types = assembly.SafeGetTypes();

            var builder = ImmutableHashSet<string>.Empty.WithComparer(StringComparer.InvariantCultureIgnoreCase).ToBuilder();
            foreach (var t in types)
            {
                var isMSTestClass = (
                        from attributeObjects in t.GetCustomAttributes(true)
                        where attributeObjects.GetType() == typeof(TestFixtureAttribute)
                        select attributeObjects)
                    .Any();

                if (isMSTestClass)
                {
                    var publicMethods = t.GetMethods();
                    foreach (var mi in publicMethods)
                    {
                        var ignoreAttribute = mi.GetCustomAttribute<IgnoreAttribute>();
                        if (ignoreAttribute != null)
                        {
                            builder.Add($"{mi.DeclaringType}.{mi.Name} is ignored.");
                        }
                    }
                }
            }

            var set = builder.ToImmutable();
            var list = set.ToList();
            list.Sort(StringComparer.InvariantCultureIgnoreCase);
            foreach (var item in list)
            {
                Trace.WriteLine(item);
            }
        }

        /// <summary>
        /// Gets the set of values recognized as valid test timing categories.
        /// </summary>
        /// <remarks>
        /// The default implementation returns all values in <see cref="Landorphan.TestUtilities.TestTiming"/>.
        /// </remarks>
        /// <returns>
        /// A non-null collection containing at least one recognized value.
        /// </returns>
        protected virtual ICollection<string> GetRecognizedTestTimingCategories()
        {
            var fieldInfos = typeof(TestTiming).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            // no .ToHashSet(StringComparer.OrdinalIgnoreCase) ... do it manually
            var lst = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string)).Select(x => ((string)x.GetRawConstantValue()).TrimNullToEmpty()).ToList();
            var rv = new HashSet<string>(lst, StringComparer.OrdinalIgnoreCase);
            if (rv.Contains(string.Empty))
            {
                rv.Remove(string.Empty);
            }

            return rv.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the test assembly to be evaluated.
        /// </summary>
        /// <example>
        /// <code>
        ///         protected override Assembly GetTestAssembly()
        ///         {
        ///             return GetType().Assembly;
        ///         }
        /// </code>
        /// </example>
        /// <returns>
        /// The test assembly to be evaluated.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected abstract Assembly GetTestAssembly();

        /// <summary>
        /// Gets the test types not required to descend from <see cref="TestBase" />.
        /// </summary>
        /// <returns>
        /// The set of types that are not required to descend from <see cref="TestBase" />.
        /// </returns>
        /// <remarks>
        /// May not be null.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IImmutableSet<Type> GetTestTypesNotRequiredToDescendFromTestBase()
        {
            return ImmutableHashSet<Type>.Empty;
        }

        private IList<Type> GetAllTestTypesInTestAssembly()
        {
            // this is capturing types outside the test assembly in .Net Core 3.1.102
            return (from t in GetTestAssembly().SafeGetTypes() where t.GetCustomAttributes(typeof(TestFixtureAttribute), true).Any() select t).ToList();
        }

        private IList<Type> GetAllTestTypesInTestAssemblyExcludeNonPublicAndAbstractAndIgnored()
        {
            return (
                from t in GetAllTestTypesInTestAssembly()
                where (t.IsPublic || t.IsNestedPublic) &&
                      !t.IsAbstract /* excludes statics as well */ &&
                      !t.GetCustomAttributes(typeof(IgnoreAttribute), true).Any()
                select t).ToList();
        }

        private IList<Type> GetAllTestTypesInTestAssemblyExcludeNonPublicAndAbstractAndIgnoredAndGenerated()
        {
            return (
                from t in GetAllTestTypesInTestAssemblyExcludeNonPublicAndAbstractAndIgnored()
                where !t.GetCustomAttributes(typeof(GeneratedCodeAttribute), true).Any() &&
                      !t.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any()
                select t).ToList();
        }
    }
}
