namespace Landorphan.TestUtilities.NUnit.Tests.Architecture
{
    using global::NUnit.Framework;
    using Landorphan.TestUtilities.ReusableTestImplementations;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class TestArchitecture_Tests : TestArchitecturalRequirements
    {
        [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
        [Test]
        [Category(TestTiming.CheckIn)]
        public void All_But_Excluded_Tests_Descend_From_TestBase()
        {
            All_But_Excluded_Tests_Descend_From_TestBase_Implementation();
        }

        [SuppressMessage("SonarLint.CodeSmell", "S2699: Tests should include assertions", Justification = "Base implementation has assertion (MWP)")]
        [Test]
        [Category(TestTiming.CheckIn)]
        public void All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category()
        {
            All_Tests_Not_Ignored_Have_Exactly_One_Timing_Category_Implementation();
        }

        protected override Assembly GetTestAssembly()
        {
            return GetType().Assembly;
        }
    }
}
