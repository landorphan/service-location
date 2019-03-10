namespace Landorphan.TestUtilities.TestFacilities
{
   using System;

   /// <summary>
   /// A collection of well known test categories excluding those found in <see cref="TestTiming"/>.
   /// </summary>
   public static class WellKnownTestCategories
   {
      /// <summary>
      /// Used by the test designer to signify a class that proves a defect and is used to prevent regression of the same issue in future releases.
      /// </summary>
      public const String PreventRegressionBug = "Prevent Regression Bug";
   }
}
