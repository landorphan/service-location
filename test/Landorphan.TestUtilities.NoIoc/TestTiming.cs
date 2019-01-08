namespace Landorphan.TestUtilities
{
   using System;
   using System.Diagnostics.CodeAnalysis;

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
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S2339:Public constant members should not be used",
         Justification = "This constant is used to initialize attributes, static members are not acceptable in this role (MWP)")]
      public const String CheckIn = "Check-In";

      /// <summary>
      /// Check-In tests that are part of the quality gate but are excluded by default from IDE test runs (due to being slow but not too slow, e.g., 3 seconds).
      /// </summary>
      /// <remarks>
      /// Tests must pass before check-in, but are excluded from IDE test runs.
      /// </remarks>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S2339:Public constant members should not be used",
         Justification = "This constant is used to initialize attributes, static members are not acceptable in this role (MWP)")]
      public const String CheckInNonIde = "Check-In(Non-Ide)";

      /// <summary>
      /// Manual test category.
      /// </summary>
      /// <remarks>
      /// Tests that are executed manually.
      /// </remarks>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S2339:Public constant members should not be used",
         Justification = "This constant is used to initialize attributes, static members are not acceptable in this role (MWP)")]
      public const String Manual = "Manual";
      /// <summary>
      /// Check-in test category.
      /// </summary>
      /// <remarks>
      /// Tests that are executed nightly.
      /// </remarks>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S2339:Public constant members should not be used",
         Justification = "This constant is used to initialize attributes, static members are not acceptable in this role (MWP)")]
      public const String Nightly = "Nightly";
   }
}