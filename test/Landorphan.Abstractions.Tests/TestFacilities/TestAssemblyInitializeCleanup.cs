namespace Landorphan.Abstractions.Tests.TestFacilities
{
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Runtime.InteropServices;
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
        [SuppressMessage("SonarLint.CodeSmell", "S3923: All branches in a conditional structure should not have exactly the same implementation")]
        [SuppressMessage("Microsoft.IDE", "IDE0060: remove unused parameter if it is not part of a public API.", Justification = "This is part of a MS public API")]
        public static void AssemblyInitialize(TestContext context)
        {
            // acquire assemblies under test
            var assemblies = ImmutableHashSet<Assembly>.Empty.ToBuilder();
            assemblies.Add(typeof(IEnvironmentUtilities).Assembly);
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
            // currently empty
        }
    }
}
