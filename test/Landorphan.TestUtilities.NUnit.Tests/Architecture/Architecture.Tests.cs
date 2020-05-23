namespace Landorphan.TestUtilities.NUnit.Tests.Architecture
{
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::NUnit.Framework;
    using Landorphan.TestUtilities.ReusableTestImplementations;

    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class Architecture_Tests : ArchitecturalRequirements
    {
        [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
        [Test]
        [Category(TestTiming.CheckIn)]
        public void All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested()
        {
            All_Declarations_Not_Under_An_Internal_Or_Resources_Namespace_Are_Public_Or_Nested_Implementation();
        }

        [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
        [Test]
        [Category(TestTiming.CheckIn)]
        public void All_Declarations_Under_An_Internal_Namespace_Are_Not_Public()
        {
            All_Declarations_Under_An_Internal_Namespace_Are_Not_Public_Implementation();
        }

        protected override IImmutableSet<Assembly> GetAssembliesUnderTest()
        {
            return TestAssemblyInitTearDown.AssembliesUnderTest;
        }
    }
}
