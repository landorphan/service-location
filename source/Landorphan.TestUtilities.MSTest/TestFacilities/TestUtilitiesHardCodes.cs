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
      /// The point is to self-document the intent, as wells as to avoid capricious warnings.
      /// TODO: consider disabling S2699:  It fires on no exception tests, manual tests, and test that merely document what is...
      /// </remarks>
      public const Boolean NoExceptionWasThrown = true;
   }
}
