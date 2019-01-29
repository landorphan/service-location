namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System.Collections.Immutable;
   using System.Reflection;
   using Landorphan.Abstractions.IO;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public static class TestAssemblyInitializeCleanup
   {
      internal static IImmutableSet<Assembly> AssembliesUnderTest { get; private set; }

      /// <summary>
      /// Performs assembly level initialization.
      /// </summary>
      /// <remarks>
      /// Executes once, before any tests to be executed are run.
      /// </remarks>
      [AssemblyInitialize]
      public static void AssemblyInitialize(TestContext context)
      {
         // acquire assemblies under test
         var assemblies = ImmutableHashSet<Assembly>.Empty.ToBuilder();
         assemblies.Add(typeof(DirectoryUtilities).Assembly); 
         AssembliesUnderTest = assemblies.ToImmutable();

         // Configure the simulator as needed.
      }

      /// <summary>
      /// Frees resources obtained by the test assembly.
      /// </summary>
      /// <remarks>
      /// Executes once, after all tests to be executed are run.
      /// </remarks>
      [AssemblyCleanup]
      public static void AssemblyCleanup()
      {
         // nothing to clean up
      }
   }
}
