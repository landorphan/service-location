namespace Landorphan.TestUtilities.TestFacilities
{
   using System;

   /// <summary>
   /// Hard-coded values for use in multiple test projects.
   /// </summary>
   public static class TestUtilitiesHardCodes
   {
      /// <summary>
      /// Used by tests to avoid S2699: Tests should contain at least one assertion.
      /// </summary>
      /// <remarks>
      /// For use by tests that call methods with troublesome input (0, null, etc.) to verify no exception is thrown.
      /// <example>
      /// <code>
      /// TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      /// </code>
      /// </example>
      /// </remarks>
      public const Boolean NoExceptionWasThrown = true;
   }
}
