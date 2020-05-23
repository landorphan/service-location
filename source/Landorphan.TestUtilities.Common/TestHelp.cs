namespace Landorphan.TestUtilities
{
    /// <summary>
    /// Helper methods for test projects.
    /// </summary>
    public static class TestHelp
    {
        /// <summary>
        /// Used by tests to suppress CA1801:ReviewUnusedParameters.
        /// </summary>
        /// <param name="inputs">
        /// The inputs.
        /// </param>
        public static void DoNothing(params object[] inputs)
        {
            // this method used to avoid code analysis warnings.
        }
    }
}
