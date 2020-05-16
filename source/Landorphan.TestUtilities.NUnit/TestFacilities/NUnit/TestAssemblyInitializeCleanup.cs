using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

/// <summary>
/// Defines test assembly initialization and cleanup.
/// </summary>
/// <remarks>
/// <see href="https://github.com/nunit/docs/wiki/SetUpFixture-Attribute"/>
/// </remarks>
[SetUpFixture]
[SuppressMessage("Microsoft.Design", "CA1050: Declare types in namespaces", Justification = "This class follows the design specification of NUnit (mproch 2020.05.08)")]
[SuppressMessage("Sonar", "S3903: Types should be defined in named namespaces", Justification = "This class follows the design specification of NUnit (mproch 2020.05.08)")]
// ReSharper disable once CheckNamespace
public static class TestAssemblyInitializeCleanup
{
    /// <summary>
    /// Frees resources obtained by the test assembly.
    /// </summary>
    /// <remarks>
    /// Executes once, after all tests to be executed are run.
    /// </remarks>
    [OneTimeTearDown]
    public static void AssemblyCleanup()
    {
        // currently no resources to clean up.
    }

    /// <summary>
    /// Performs assembly level initialization.
    /// </summary>
    /// <remarks>
    /// Executes once, before any tests to be executed are run.
    /// </remarks>
    [OneTimeSetUp]
    public static void AssemblyInitialize()
    {
        // currently no resources to initialize.
    }
}
