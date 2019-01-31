namespace Landorphan.TestUtilities.ReusableTestImplementations
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Linq;
   using System.Reflection;
   using Landorphan.Common;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   /// <summary>
   /// Test implementations for test architectural requirements.
   /// </summary>
   public abstract class TestArchitecturalRequirements : TestBase
   {
      /// <summary>
      /// Verifies that all test classes descend from <see cref="TestBase" /> except for those explicitly excluded.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void All_But_Excluded_Tests_Descend_From_TestBase_Implementation()
      {
         var failureMessages = new List<String>();

         var excludedTypes = GetTestTypesNotRequiredToDescendFromTestBase();
         Trace.Assert(excludedTypes != null, "GetTestTypesNotRequiredToDescendFromTestBase() returned null -- which it must not do");

         var testClassTypes = GetAllEffectiveTestTypesTestAssembly();
         foreach (var testClass in testClassTypes)
         {
            if (!typeof(TestBase).IsAssignableFrom(testClass) && !excludedTypes.Contains(testClass))
            {
               failureMessages.Add(
                  String.Format(
                     CultureInfo.InvariantCulture,
                     "The test class '{0}' is not descended from TestBase but should be.",
                     testClass.Name));
            }
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Verifies that all tests that are not ignored have one and only one timing category.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Control flow statements if, switch, for, foreach, while, do and try should not be nested too deeply")]
      protected void All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category_Implementation()
      {
         var failureMessages = new List<String>();

         // Get the Timing fields
         var testTimingValues = new HashSet<String>(StringComparer.Ordinal);
         var categoryTimingType = typeof(TestTiming);
         var fields = categoryTimingType.GetFields(BindingFlags.Public | BindingFlags.Static);
         foreach (var f in fields)
         {
            testTimingValues.Add((String)f.GetValue(null));
         }

         var testClassTypes = GetAllEffectiveTestTypesTestAssembly();
         foreach (var testClass in testClassTypes)
         {
            var allPublicMethodsForType = testClass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var m in allPublicMethodsForType)
            {
               if (m.GetCustomAttributes(typeof(TestMethodAttribute), true).Any() &&
                   !m.GetCustomAttributes(typeof(IgnoreAttribute), true).Any())
               {
                  var testMethod = m.Name;

                  var testCategories = (TestCategoryAttribute[])m.GetCustomAttributes(typeof(TestCategoryAttribute), true);
                  var timingMatches = 0;
                  foreach (var c in testCategories)
                  {
                     foreach (var v in c.TestCategories)
                     {
                        if (testTimingValues.Contains(v))
                        {
                           timingMatches++;
                        }
                     }
                  }

                  switch (timingMatches)
                  {
                     case 1:
                        break;

                     case 0:
                        failureMessages.Add(
                           String.Format(
                              CultureInfo.InvariantCulture,
                              "The test '{0}.{1}' is not decorated with [TestCategory(WellKnownCategories.Timing.*)] but should be.",
                              testClass.Name,
                              testMethod));
                        break;

                     default:
                        failureMessages.Add(
                           String.Format(
                              CultureInfo.InvariantCulture,
                              "The test '{0}.{1}' is decorated with more than one [TestCategory(WellKnownCategories.Timing.*)] but should only have one timing attribute.",
                              testClass.Name,
                              testMethod));
                        break;
                  }
               }
            }
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
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
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Find_ignored_tests_in_assembly(Assembly assembly)
      {
         assembly.ArgumentNotNull(nameof(assembly));
         Trace.WriteLine($"Examining {assembly.GetName().Name} looking for ignored test methods...");
         var types = assembly.SafeGetTypes();

         var builder = ImmutableHashSet<String>.Empty.WithComparer(StringComparer.InvariantCultureIgnoreCase).ToBuilder();
         foreach (var t in types)
         {
            var isMSTestClass = (
                  from attributeObjects in t.GetCustomAttributes(true)
                  where attributeObjects.GetType() == typeof(TestClassAttribute)
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
      /// Gets the test assembly to be evaluated.
      /// </summary>
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

      private IList<Type> GetAllEffectiveTestTypesTestAssembly()
      {
         return (
            from t in GetAllTestTypesInTestAssembly()
            where
               (t.IsPublic || t.IsNestedPublic) &&
               !t.IsAbstract /* excludes statics as well */ &&
               !t.GetCustomAttributes(typeof(IgnoreAttribute), true).Any()
            select t).ToList();
      }

      private IList<Type> GetAllTestTypesInTestAssembly()
      {
         return (from t in GetTestAssembly().SafeGetTypes() where t.GetCustomAttributes(typeof(TestClassAttribute), true).Any() select t)
            .ToList();
      }
   }
}
