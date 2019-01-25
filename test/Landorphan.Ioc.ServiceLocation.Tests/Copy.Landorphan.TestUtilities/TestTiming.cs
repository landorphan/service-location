// ReSharper disable once CheckNamespace
namespace Landorphan.TestUtilities
{
   using System;

   /// <summary>
   /// Categories describing when a test is executed.
   /// </summary>
   public static class TestTiming
   {
      /// <summary>
      /// Check-in test category.
      /// </summary>
      /// <remarks>
      /// Tests that must pass before check-in.
      /// </remarks>
      public const String CheckIn = "Check-In";

      /// <summary>
      /// Check-In tests that are part of the quality gate but are excluded by default from IDE test runs (due to being slow but not too slow, e.g., 3 seconds).
      /// </summary>
      /// <remarks>
      /// Tests must pass before check-in, but are excluded from IDE test runs.
      /// </remarks>
      public const String CheckInNonIde = "Check-In(Non-Ide)";

      /// <summary>
      /// IDE only test category.
      /// </summary>
      /// <remarks>
      /// Tests that are intended solely for use in an IDE environment and are not part of the quality gate
      /// </remarks>
      public const String IdeOnly = "IDE-Only";

      /// <summary>
      /// Manual test category.
      /// </summary>
      /// <remarks>
      /// Tests that are executed manually.
      /// </remarks>
      public const String Manual = "Manual";

      /// <summary>
      /// Nightly test category.
      /// </summary>
      /// <remarks>
      /// Tests that are executed nightly.
      /// </remarks>
      public const String Nightly = "Nightly";
   }
}
